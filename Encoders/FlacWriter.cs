using System;
using System.IO;
using System.Runtime.InteropServices;

namespace BlackOps2SoundStudio.Encoders
{
    class FlacWriter : IDisposable
    {
        #region Api
        const string Dll = "LibFlac";

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate IntPtr FLACStreamEncoderNewDelegate();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate bool FLACStreamEncoderContextDelegate(IntPtr context);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate bool FLACStreamEncoderSetIntegerDelegate(IntPtr context, int value);
        
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate int FLACStreamEncoderInitStreamDelegate(IntPtr context, WriteCallback write, SeekCallback seek, TellCallback tell, MetadataCallback metadata, IntPtr userData);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate bool FLACStreamEncoderProcessInterleavedDelegate(IntPtr context, IntPtr buffer, int samples);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate bool FLACStreamEncoderSetBooleanDelegate(IntPtr context, bool value);

        private static readonly FLACStreamEncoderNewDelegate _flacStreamEncoderNew;
        private static readonly FLACStreamEncoderContextDelegate _flacStreamEncoderDelete, _flacStreamEncoderFinish;
        private static readonly FLACStreamEncoderSetIntegerDelegate _flacStreamEncoderSetChannels, _flacStreamEncoderSetBitsPerSample,
            _flacStreamEncoderSetSampleRate, _flacStreamEncoderSetCompressionLevel, _flacStreamEncoderSetBlockSize,
            _flacStreamEncoderSetQlpCoeffPrecision, _flacStreamEncoderSetMaxResidualPartitionOrder;
        private static readonly FLACStreamEncoderInitStreamDelegate _flacStreamEncoderInitStream;
        private static readonly FLACStreamEncoderProcessInterleavedDelegate _flacStreamEncoderProcessInterleaved;
        private static readonly FLACStreamEncoderSetBooleanDelegate _flacStreamEncoderSetVerify, _flacStreamEncoderSetStreamableSubset,
            _flacStreamEncoderSetDoMidSideStereo, _flacStreamEncoderSetLooseMidSideStereo, _flacStreamEncoderSetDoQlpCoeffPrecSearch,
            _flacStreamEncoderSetDoMD5;

        static FlacWriter()
        {
            var module = MappedModuleDatabase.libFLAC;
            _flacStreamEncoderNew = (FLACStreamEncoderNewDelegate)Marshal.GetDelegateForFunctionPointer(
                module.GetProcAddress("FLAC__stream_encoder_new"), typeof (FLACStreamEncoderNewDelegate));
            _flacStreamEncoderFinish = (FLACStreamEncoderContextDelegate) Marshal.GetDelegateForFunctionPointer(
                module.GetProcAddress("FLAC__stream_encoder_finish"), typeof(FLACStreamEncoderContextDelegate));
            _flacStreamEncoderDelete = (FLACStreamEncoderContextDelegate)Marshal.GetDelegateForFunctionPointer(
                module.GetProcAddress("FLAC__stream_encoder_delete"), typeof(FLACStreamEncoderContextDelegate));
            _flacStreamEncoderSetChannels = (FLACStreamEncoderSetIntegerDelegate) Marshal.GetDelegateForFunctionPointer(
                module.GetProcAddress("FLAC__stream_encoder_set_channels"), typeof (FLACStreamEncoderSetIntegerDelegate));
            _flacStreamEncoderSetBitsPerSample = (FLACStreamEncoderSetIntegerDelegate)Marshal.GetDelegateForFunctionPointer(
                module.GetProcAddress("FLAC__stream_encoder_set_bits_per_sample"), typeof(FLACStreamEncoderSetIntegerDelegate));
            _flacStreamEncoderSetSampleRate = (FLACStreamEncoderSetIntegerDelegate) Marshal.GetDelegateForFunctionPointer(
                module.GetProcAddress("FLAC__stream_encoder_set_sample_rate"), typeof(FLACStreamEncoderSetIntegerDelegate));
            _flacStreamEncoderSetCompressionLevel = (FLACStreamEncoderSetIntegerDelegate)Marshal.GetDelegateForFunctionPointer(
                module.GetProcAddress("FLAC__stream_encoder_set_compression_level"), typeof(FLACStreamEncoderSetIntegerDelegate));
            _flacStreamEncoderSetBlockSize = (FLACStreamEncoderSetIntegerDelegate) Marshal.GetDelegateForFunctionPointer(
                module.GetProcAddress("FLAC__stream_encoder_set_blocksize"), typeof(FLACStreamEncoderSetIntegerDelegate));
            _flacStreamEncoderSetQlpCoeffPrecision = (FLACStreamEncoderSetIntegerDelegate)Marshal.GetDelegateForFunctionPointer(
                module.GetProcAddress("FLAC__stream_encoder_set_qlp_coeff_precision"), typeof(FLACStreamEncoderSetIntegerDelegate));
            _flacStreamEncoderSetMaxResidualPartitionOrder = (FLACStreamEncoderSetIntegerDelegate)Marshal.GetDelegateForFunctionPointer(
                module.GetProcAddress("FLAC__stream_encoder_set_max_residual_partition_order"), typeof(FLACStreamEncoderSetIntegerDelegate));
            _flacStreamEncoderInitStream = (FLACStreamEncoderInitStreamDelegate) Marshal.GetDelegateForFunctionPointer(
                module.GetProcAddress("FLAC__stream_encoder_init_stream"), typeof(FLACStreamEncoderInitStreamDelegate));
            _flacStreamEncoderProcessInterleaved = (FLACStreamEncoderProcessInterleavedDelegate)Marshal.GetDelegateForFunctionPointer(
                module.GetProcAddress("FLAC__stream_encoder_process_interleaved"), typeof(FLACStreamEncoderProcessInterleavedDelegate));
            _flacStreamEncoderSetVerify = (FLACStreamEncoderSetBooleanDelegate) Marshal.GetDelegateForFunctionPointer(
                 module.GetProcAddress("FLAC__stream_encoder_set_verify"), typeof(FLACStreamEncoderSetBooleanDelegate));
            _flacStreamEncoderSetStreamableSubset = (FLACStreamEncoderSetBooleanDelegate)Marshal.GetDelegateForFunctionPointer(
                module.GetProcAddress("FLAC__stream_encoder_set_streamable_subset"), typeof(FLACStreamEncoderSetBooleanDelegate));
            _flacStreamEncoderSetDoMidSideStereo = (FLACStreamEncoderSetBooleanDelegate)Marshal.GetDelegateForFunctionPointer(
                module.GetProcAddress("FLAC__stream_encoder_set_do_mid_side_stereo"), typeof(FLACStreamEncoderSetBooleanDelegate));
            _flacStreamEncoderSetLooseMidSideStereo = (FLACStreamEncoderSetBooleanDelegate)Marshal.GetDelegateForFunctionPointer(
                module.GetProcAddress("FLAC__stream_encoder_set_loose_mid_side_stereo"), typeof(FLACStreamEncoderSetBooleanDelegate));
            _flacStreamEncoderSetDoQlpCoeffPrecSearch = (FLACStreamEncoderSetBooleanDelegate)Marshal.GetDelegateForFunctionPointer(
                module.GetProcAddress("FLAC__stream_encoder_set_do_qlp_coeff_prec_search"), typeof(FLACStreamEncoderSetBooleanDelegate));
            _flacStreamEncoderSetDoMD5 = (FLACStreamEncoderSetBooleanDelegate)Marshal.GetDelegateForFunctionPointer(
                module.GetProcAddress("FLAC__stream_encoder_set_do_md5"), typeof(FLACStreamEncoderSetBooleanDelegate));
        }

        // Callbacks
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate int WriteCallback(IntPtr context, IntPtr buffer, int bytes, uint samples, uint current_frame, IntPtr userData);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate int SeekCallback(IntPtr context, long absoluteOffset, IntPtr userData);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate int TellCallback(IntPtr context, out long absoluteOffset, IntPtr userData);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate void MetadataCallback(IntPtr context, IntPtr metadata, IntPtr userData);
        #endregion

        #region Fields
        private IntPtr context;
        private Stream stream;

        private int inputBitDepth;
        private int inputChannels;

        private int[] padded;
        private byte[] callbackBuffer;

        private WriteCallback write;
        private SeekCallback seek;
        private TellCallback tell;
        #endregion

        #region Methods
        public FlacWriter(Stream output, int bitDepth, int channels, int sampleRate)
        {
            stream = output;

            inputBitDepth = bitDepth;
            inputChannels = channels;

            context = _flacStreamEncoderNew();

            if (context == IntPtr.Zero)
                throw new ApplicationException("FLAC: Could not initialize stream encoder!");

            Check(
                _flacStreamEncoderSetChannels(context, channels),
                "set channels");

            Check(
                _flacStreamEncoderSetBitsPerSample(context, bitDepth),
                "set bits per sample");

            Check(
                _flacStreamEncoderSetSampleRate(context, sampleRate),
                "set sample rate");

            Check(
                _flacStreamEncoderSetVerify(context, false),
                "set verify");

            Check(
                _flacStreamEncoderSetStreamableSubset(context, true),
                "set verify");

            Check(
               _flacStreamEncoderSetDoMidSideStereo(context, true),
                "set verify");

            Check(
                _flacStreamEncoderSetLooseMidSideStereo(context, true),
                "set verify");

            Check(
                _flacStreamEncoderSetQlpCoeffPrecision(context, 10),
                "set qlp coeff precision");

            Check(
                _flacStreamEncoderSetBlockSize(context, 1024),
                "set block size");

            Check(
                _flacStreamEncoderSetDoQlpCoeffPrecSearch(context, true),
                "set do qlp coeff precision search");

            Check(
                _flacStreamEncoderSetCompressionLevel(context, 8),
                "set compression level");

            Check(
                _flacStreamEncoderSetMaxResidualPartitionOrder(context, 5),
                "set max residual partition order");

            Check(
                _flacStreamEncoderSetDoMD5(context, false),
                "set do md5");

            write = Write;
            seek = Seek;
            tell = Tell;

            if (_flacStreamEncoderInitStream(context,
                                                    write, seek, tell,
                                                    null, IntPtr.Zero) != 0)
                throw new ApplicationException("FLAC: Could not open stream for writing!");

            //if (FLAC__stream_encoder_init_file(context, @"wav\miles_fisher-this_must_be_the_place-iphone2.flac", IntPtr.Zero, IntPtr.Zero) != 0)
            //    throw new ApplicationException("FLAC: Could not open stream for writing!");
        }

        public void Dispose()
        {
            if (context != IntPtr.Zero)
            {
                Check(
                    _flacStreamEncoderFinish(context),
                    "finalize stream encoder");

                Check(
                    _flacStreamEncoderDelete(context),
                    "dispose of stream encoder instance");

                context = IntPtr.Zero;
            }
        }

        public void Close()
        {
            Dispose();
        }

        public unsafe void Write(byte[] buffer, int offset, int uncompressedBytes)
        {
            if (context == IntPtr.Zero)
                throw new ApplicationException("FLAC: Stream encoder is not initialized!");

            int bytes = inputBitDepth / 8;
            int paddedSamples = uncompressedBytes / bytes;
            int samples = paddedSamples / inputChannels;

            // 16/24-bit -> padding to a 32-bit integer
            if (padded == null || padded.Length < paddedSamples)
                padded = new int[paddedSamples];

            if (inputBitDepth == 16)
                for (int i = 0; i < paddedSamples; i++)
                    padded[i] = buffer[i * bytes + 1] << 8 |
                                buffer[i * bytes + 0];

            else if (inputBitDepth == 24)
                for (int i = 0; i < paddedSamples; i++)
                    padded[i] = buffer[i * bytes + 2] << 16 |
                                buffer[i * bytes + 1] << 8 |
                                buffer[i * bytes + 0];

            else
                throw new ApplicationException(string.Format("FLAC: Unsupported bit depth '{0}'!", inputBitDepth));

            fixed (int* fixedInput = padded)
            {
                var input = new IntPtr(fixedInput);
                Check(
                    _flacStreamEncoderProcessInterleaved (context, input, samples),
                    "process audio samples");
            }
        }

        private void Check(bool result, string operation)
        {
            if (!result)
                throw new ApplicationException(string.Format("FLAC: Could not {0}!", operation));
        }
        #endregion

        #region Callbacks
        private int Write(IntPtr callbackContext, IntPtr buffer, int bytes, uint samples, uint current_frame, IntPtr userData)
        {
            // Allocate a 32-KB or [needed bytes] buffer, whichever is larger
            if (callbackBuffer == null || callbackBuffer.Length < bytes)
                callbackBuffer = new byte[Math.Max(bytes, 32 * 1024)];

            Marshal.Copy(buffer, callbackBuffer, 0, bytes);

            stream.Write(callbackBuffer, 0, bytes);

            return 0;
        }

        private int Seek(IntPtr callbackContext, long absoluteOffset, IntPtr userData)
        {
            stream.Position = absoluteOffset;

            return 0;
        }

        private int Tell(IntPtr callbackContext, out long absoluteOffset, IntPtr userData)
        {
            absoluteOffset = stream.Position;

            return 0;
        }
        #endregion
    }
}
