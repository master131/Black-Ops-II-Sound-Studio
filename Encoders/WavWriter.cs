using System;
using System.IO;
using System.Runtime.InteropServices;

namespace BlackOps2SoundStudio.Encoders
{
    // Credits to: Stanimir Stoyanov
    // Taken from: http://stoyanov.in/2010/07/26/decoding-flac-audio-files-in-c/
    class WavWriter : IDisposable
    {
        #region Fields
        private Stream output;
        private BinaryWriter writer;
        private WaveHeader header;

        private long audioDataBytes;

        private long footerFieldPos1;
        private long footerFieldPos2;

        private const uint WaveHeaderSize = 38;
        private const uint WaveFormatSize = 18;

        private bool hasHeader;
        #endregion

        #region Methods
        public WavWriter(string output)
            : this(File.Create(output))
        {
        }

        public WavWriter(Stream output)
        {
            this.output = output;
            writer = new BinaryWriter(output);
        }

        public void Dispose()
        {
            if (output != null)
            {
                WriteFooter();
            }

            output = null;
        }

        public void WriteHeader(int sampleRate, int bitDepth, int channels)
        {
            header = new WaveHeader(sampleRate, bitDepth, channels);

            writer.Write(new[] { (byte)'R', (byte)'I', (byte)'F', (byte)'F' });
            footerFieldPos1 = output.Position; writer.Write(WaveHeaderSize);
            writer.Write(new[] { (byte)'W', (byte)'A', (byte)'V', (byte)'E' });
            writer.Write(new[] { (byte)'f', (byte)'m', (byte)'t', (byte)' ' });

            writer.Write(WaveFormatSize);
            writer.Write(header.wFormatTag);
            writer.Write(header.nChannels);
            writer.Write(header.nSamplesPerSec);
            writer.Write(header.nAvgBytesPerSec);
            writer.Write(header.nBlockAlign);
            writer.Write(header.wBitsPerSample);
            writer.Write(header.cbSize);

            writer.Write(new[] { (byte)'d', (byte)'a', (byte)'t', (byte)'a' });
            footerFieldPos2 = output.Position; writer.Write((uint)audioDataBytes);

            hasHeader = true;
        }

        private void WriteFooter()
        {
            output.Position = footerFieldPos1; writer.Write((uint)audioDataBytes + WaveHeaderSize);
            output.Position = footerFieldPos2; writer.Write((uint)audioDataBytes);
        }

        public void WriteInt16(int value)
        {
            writer.Write((short)value);
            audioDataBytes += 2;
        }

        public void WriteInt24(int value)
        {
            writer.Write((byte)((value >> 0 * 8) & 0xFF));
            writer.Write((byte)((value >> 1 * 8) & 0xFF));
            writer.Write((byte)((value >> 2 * 8) & 0xFF));
            audioDataBytes += 3;
        }

        public void WriteBytes(byte[] value, int count)
        {
            writer.Write(value, 0, count);
            audioDataBytes += count;
        }
        #endregion

        #region Properties
        public long AudioDataBytes
        {
            get { return audioDataBytes; }
            set { audioDataBytes += value; }
        }

        public bool HasHeader
        {
            get { return hasHeader; }
        }

        public WaveHeader Header
        {
            get { return header; }
        }
        #endregion
    }

    #region Native
    [StructLayout(LayoutKind.Sequential)]
    public class WaveHeader
    {
        public short wFormatTag;
        public short nChannels;
        public int nSamplesPerSec;
        public int nAvgBytesPerSec;
        public short nBlockAlign;
        public short wBitsPerSample;
        public short cbSize;

        public WaveHeader(int rate, int bits, int channels)
        {
            wFormatTag = 1; // 1 = PCM 2 = Float
            nChannels = (short)channels;
            nSamplesPerSec = rate;
            wBitsPerSample = (short)bits;
            cbSize = 0;

            nBlockAlign = (short)(channels * (bits / 8));
            nAvgBytesPerSec = nSamplesPerSec * nBlockAlign;
        }
    }
    #endregion
}
