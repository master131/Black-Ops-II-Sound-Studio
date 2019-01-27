using System;
using System.IO;
using System.Runtime.InteropServices;
using BlackOps2SoundStudio.Encoders;

namespace BlackOps2SoundStudio.Decoders
{
    class SampleProcessedEventArgs : EventArgs
    {
        public long ProcessedSamples { get; set; }
        public long TotalSamples { get; set; }
        public int Progress { get; set; }

        public SampleProcessedEventArgs(long processed, long total)
        {
            ProcessedSamples = processed;
            TotalSamples = total;
            Progress = (int) (processed * 100 / total);
        }
    }

    // Credits to: Stanimir Stoyanov
    // Taken from: http://stoyanov.in/2010/07/26/decoding-flac-audio-files-in-c/
    class FlacReader : IDisposable
    {
        #region Api
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate IntPtr FLACStreamDecoderNewDelegate();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate bool FLACStreamDecoderContextDelegate(IntPtr context);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate long FLACStreamDecoderGetTotalSamplesDelegate(IntPtr context);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate int FLACStreamDecoderInitStreamDelegate(IntPtr context, ReadCallback read, SeekCallback seek, TellCallback tell, LengthCallback length, EofCallback eof, WriteCallback write, MetadataCallback metadata, ErrorCallback error, IntPtr userData);

        private static readonly FLACStreamDecoderNewDelegate _flacStreamDecoderNew;
        private static readonly FLACStreamDecoderContextDelegate _flacStreamDecoderFinish,
            _flacStreamDecoderDelete, _flacStreamDecoderUntilEndOfStream;
        private static readonly FLACStreamDecoderGetTotalSamplesDelegate _flacStreamDecoderGetTotalSamples;
        private static readonly FLACStreamDecoderInitStreamDelegate _flacStreamDecoderInitStream;

        static FlacReader()
        {
            var module = MappedModuleDatabase.libFLAC;
            _flacStreamDecoderNew = (FLACStreamDecoderNewDelegate)Marshal.GetDelegateForFunctionPointer(
                module.GetProcAddress("FLAC__stream_decoder_new"), typeof(FLACStreamDecoderNewDelegate));
            _flacStreamDecoderFinish = (FLACStreamDecoderContextDelegate)Marshal.GetDelegateForFunctionPointer(
                module.GetProcAddress("FLAC__stream_decoder_finish"), typeof(FLACStreamDecoderContextDelegate));
            _flacStreamDecoderDelete = (FLACStreamDecoderContextDelegate)Marshal.GetDelegateForFunctionPointer(
                module.GetProcAddress("FLAC__stream_decoder_delete"), typeof(FLACStreamDecoderContextDelegate));
            _flacStreamDecoderInitStream = (FLACStreamDecoderInitStreamDelegate)Marshal.GetDelegateForFunctionPointer(
                module.GetProcAddress("FLAC__stream_decoder_init_stream"), typeof(FLACStreamDecoderInitStreamDelegate));
            _flacStreamDecoderUntilEndOfStream = (FLACStreamDecoderContextDelegate)Marshal.GetDelegateForFunctionPointer(
                module.GetProcAddress("FLAC__stream_decoder_process_until_end_of_stream"), typeof(FLACStreamDecoderContextDelegate));
            _flacStreamDecoderGetTotalSamples = (FLACStreamDecoderGetTotalSamplesDelegate)Marshal.GetDelegateForFunctionPointer(
                module.GetProcAddress("FLAC__stream_decoder_get_total_samples"), typeof(FLACStreamDecoderGetTotalSamplesDelegate));
        }

        // Callbacks
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate WriteStatus WriteCallback(IntPtr context, IntPtr frame, IntPtr buffer, IntPtr userData);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate void ErrorCallback(IntPtr context, DecodeError status, IntPtr userData);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate void MetadataCallback(IntPtr context, IntPtr metadata, IntPtr userData);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate ReadStatus ReadCallback(IntPtr context, IntPtr bufferHandle, ref IntPtr bytes, IntPtr userData);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate SeekStatus SeekCallback(IntPtr decoder, UInt64 absoluteByteOffset, IntPtr userData);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate TellStatus TellCallback(IntPtr decoder, ref UInt64 absoluteByteOffset, IntPtr userData);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate LengthStatus LengthCallback(IntPtr decoder, ref UInt64 streamLength, IntPtr userData);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate bool EofCallback(IntPtr decoder, IntPtr userData);

        private const int FlacMaxChannels = 8;

        [StructLayout(LayoutKind.Sequential)]
        struct FlacFrame
        {
            public FrameHeader Header;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = FlacMaxChannels)]
            public FlacSubFrame[] Subframes;
            public FrameFooter Footer;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct FrameHeader
        {
            public int BlockSize;
            public int SampleRate;
            public int Channels;
            public int ChannelAssignment;
            public int BitsPerSample;
            public FrameNumberType NumberType;
            public long FrameOrSampleNumber;
            public byte Crc;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct FlacSubFrame
        {
            public SubframeType Type;
            public IntPtr Data;
            public int WastedBits;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct FrameFooter
        {
            public ushort Crc;
        }

        enum FrameNumberType
        {
            Frame,
            Sample
        }

        enum SubframeType
        {
            Constant,
            Verbatim,
            Fixed,
            LPC
        }

        enum DecodeError
        {
            LostSync,
            BadHeader,
            FrameCrcMismatch,
            UnparsableStream
        }

        enum ReadStatus
        {
            Continue,
            EndOfStream,
            Abort
        }

        enum SeekStatus
        {
            OK,
            Error,
            Unsupported
        }

        enum TellStatus
        {
            OK,
            Error,
            Unsupported
        }

        enum LengthStatus
        {
            OK,
            Error,
            Unsupported
        }

        enum WriteStatus
        {
            Continue,
            Abort
        }
        #endregion

        #region Fields
        private IntPtr context;
        private readonly Stream stream;
        private readonly WavWriter writer;

        private int inputBitDepth;
        private int inputChannels;
        private int inputSampleRate;

        private int[] samples;
        private float[] samplesChannel;

        private long totalSamples = -1;
        private long processedSamples;

        private readonly WriteCallback write;
        private readonly MetadataCallback metadata;
        private readonly ErrorCallback error;
        private readonly ReadCallback read;
        private readonly SeekCallback seek;
        private readonly TellCallback tell;
        private readonly LengthCallback length;
        private readonly EofCallback eof;
        #endregion

        #region Events

        public event EventHandler<SampleProcessedEventArgs> SampleProcessed; 

        #endregion

        #region Methods
        public FlacReader(string input, WavWriter output)
            : this(File.OpenRead(input), output)
        {
        }

        public FlacReader(Stream input, WavWriter output)
        {
            if (output == null)
                throw new ArgumentNullException("output");

            stream = input;
            writer = output;

            context = _flacStreamDecoderNew();

            if (context == IntPtr.Zero)
                throw new ApplicationException("FLAC: Could not initialize stream decoder!");

            write = Write;
            metadata = Metadata;
            error = Error;
            read = Read;
            seek = Seek;
            tell = Tell;
            length = Length;
            eof = Eof;

            if (_flacStreamDecoderInitStream(context, read, seek, tell, length, eof, write, metadata,
                error, IntPtr.Zero) != 0)
                throw new ApplicationException("FLAC: Could not open stream for reading!");
        }

        public void Dispose()
        {
            if (context != IntPtr.Zero)
            {
                Check(
                    _flacStreamDecoderFinish(context),
                    "finalize stream decoder");

                Check(
                    _flacStreamDecoderDelete(context),
                    "dispose of stream decoder instance");

                context = IntPtr.Zero;
            }
        }

        public void Close()
        {
            Dispose();
        }

        private void Check(bool result, string operation)
        {
            if (!result)
                throw new ApplicationException(string.Format("FLAC: Could not {0}!", operation));
        }
        #endregion

        #region Callbacks
        private WriteStatus Write(IntPtr callbackContext, IntPtr frame, IntPtr buffer, IntPtr userData)
        {
            var f = (FlacFrame)Marshal.PtrToStructure(frame, typeof(FlacFrame));

            int samplesPerChannel = f.Header.BlockSize;

            inputBitDepth = f.Header.BitsPerSample;
            inputChannels = f.Header.Channels;
            inputSampleRate = f.Header.SampleRate;

            if (!writer.HasHeader)
                writer.WriteHeader(inputSampleRate, inputBitDepth, inputChannels);

            if (totalSamples < 0)
                totalSamples = _flacStreamDecoderGetTotalSamples(callbackContext);

            if (samples == null) samples = new int[samplesPerChannel * inputChannels];
            if (samplesChannel == null) samplesChannel = new float[inputChannels];

            for (int i = 0; i < inputChannels; i++)
            {
                IntPtr pChannelBits = Marshal.ReadIntPtr(buffer, i * IntPtr.Size);
                Marshal.Copy(pChannelBits, samples, i * samplesPerChannel, samplesPerChannel);
            }

            // For each channel, there are BlockSize number of samples, so let's process these.
            for (int i = 0; i < samplesPerChannel; i++)
            {
                for (int c = 0; c < inputChannels; c++)
                {
                    int v = samples[i + c * samplesPerChannel];

                    switch (inputBitDepth / 8)
                    {
                        case 2: // 16-bit
                            writer.WriteInt16(v);
                            break;

                        case 3: // 24-bit
                            writer.WriteInt24(v);
                            break;

                        default:
                            throw new NotSupportedException("Input FLAC bit depth is not supported!");
                    }
                }

                processedSamples++;
            }

            // Show progress
            if (totalSamples > 0)
                SampleProcessed(this, new SampleProcessedEventArgs(processedSamples, totalSamples));

            return WriteStatus.Continue;
        }

        private void Metadata(IntPtr callbackContext, IntPtr callbackMetadata, IntPtr userData)
        {

        }

        private void Error(IntPtr callbackContext, DecodeError status, IntPtr userData)
        {
            throw new ApplicationException(string.Format("FLAC: Could not decode frame: {0}!", status));
        }

        private ReadStatus Read(IntPtr callbackContext, IntPtr bufferHandle, ref IntPtr bytes, IntPtr userData)
        {
            int targetLength = bytes.ToInt32();
            var buffer = new byte[targetLength];
            bytes = (IntPtr) stream.Read(buffer, 0, targetLength);
            Marshal.Copy(buffer, 0, bufferHandle, bytes.ToInt32());
            return ReadStatus.Continue;
        }

        private SeekStatus Seek(IntPtr decoder, UInt64 absoluteByteOffset, IntPtr userData)
        {
            if ((long)absoluteByteOffset >= stream.Length)
                return SeekStatus.Error;
            stream.Position = (long) absoluteByteOffset;
            return SeekStatus.OK;
        }

        private TellStatus Tell(IntPtr decoder, ref UInt64 absoluteByteOffset, IntPtr userData)
        {
            absoluteByteOffset = (ulong) stream.Position;
            return TellStatus.OK;
        }

        private LengthStatus Length(IntPtr decoder, ref UInt64 streamLength, IntPtr userData)
        {
            streamLength = (ulong) stream.Length;
            return LengthStatus.OK;
        }

        private bool Eof(IntPtr decoder, IntPtr userData)
        {
            return stream.Length == stream.Position;
        }

        public void Process()
        {
            Check(
                _flacStreamDecoderUntilEndOfStream(context),
                "process until eof");
        }
        #endregion
    }
}
