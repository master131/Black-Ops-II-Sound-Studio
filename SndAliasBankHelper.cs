using System;
using System.IO;
using BlackOps2SoundStudio.Decoders;
using BlackOps2SoundStudio.Encoders;
using BlackOps2SoundStudio.Format;

namespace BlackOps2SoundStudio
{
    static class SndAliasBankHelper
    {
        public static string GetExtensionFromFormat(AudioFormat format)
        {
            if (format == AudioFormat.FLAC)
                return ".flac";
            if (format == AudioFormat.PCMS16)
                return ".wav";
            if (format == AudioFormat.MP3)
                return ".mp3";
            if (format == AudioFormat.XMA4)
                return ".xma";
            return string.Empty;
        }

        public static string GetFormatName(AudioFormat format)
        {
            if (format == AudioFormat.FLAC)
                return "FLAC";
            if (format == AudioFormat.PCMS16)
                return "PCM";
            if (format == AudioFormat.MP3)
                return "MP3";
            if (format == AudioFormat.XMA4)
                return "XMA";
            if (format == AudioFormat.InterweavedDSP)
                return "DSP";

            return "Unknown";
        }

        public static Stream DecodeHeaderlessWav(SndAssetBankEntry entry)
        {
            var data = entry.Data.Get();
            var ms = new MemoryStream();

            // Write the header.
            using (var writer = new WavWriter(ms))
            {
                writer.WriteHeader(48000, 16, entry.ChannelCount);
                writer.AudioDataBytes = data.Length;
            }

            // Write the data.
            ms.Write(data, 0, data.Length);
            data = ms.ToArray();

            ms.Position = 0;
            return ms;
        }

        public static Stream AddXMAHeader(SndAssetBankEntry entry)
        {
            var data = entry.Data.Get();
            var ms = new MemoryStream();
            var writer = new BinaryWriter(ms);
            writer.Write(new[] { (byte)'R', (byte)'I', (byte)'F', (byte)'F' });
            writer.Write(data.Length + 0x34); // ChunkSize
            writer.Write(new[] { (byte)'W', (byte)'A', (byte)'V', (byte)'E' });
            writer.Write(new[] { (byte)'f', (byte)'m', (byte)'t', (byte)' ' });
            writer.Write(0x20); // SubChunk1Size
            writer.Write(new byte[] {
                0x65, 0x01, 0x10, 0x00, 0xD6, 0x10, 0x00, 0x00, 0x01, 0x00, 0x00, 0x03,
                0xE3, 0x9A, 0x00, 0x00
            }); // Unknown XMA header data.
            writer.Write(entry.SampleRate);
            writer.Write(new byte[] {
	            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
            }); // Unknown XMA header data.
            writer.Write(entry.ChannelCount);
            writer.Write((short)2);
            writer.Write(new[] { (byte)'d', (byte)'a', (byte)'t', (byte)'a' });
            writer.Write(data.Length); // SubChunk2Size
            writer.Write(data); // Data
            ms.Position = 0;
            return ms;
        }

        public static Stream GetAudioStreamFromEntry(SndAssetBankEntry entry)
        {
            if (entry.Format == AudioFormat.FLAC ||
                entry.Format == AudioFormat.MP3)
                return new MemoryStream(entry.Data.Get());

            if (entry.Format == AudioFormat.PCMS16)
                return DecodeHeaderlessWav(entry);
            if (entry.Format == AudioFormat.InterweavedDSP)
                return DecodeInterweavedDSP(entry);
            return AddXMAHeader(entry);
        }

        public static Stream DecodeInterweavedDSP(SndAssetBankEntry entry)
        {
            // https://wiki.axiodl.com/w/index.php?title=DSP_(File_Format)
            var data = entry.Data.Get();
            var ms = new MemoryStream();
            var writer = new BinaryWriter(ms);
            for (int x = 0; x < entry.ChannelCount; x++)
            { // Each channel needs its own DSP header.
                writer.Write( // Wii U T6 likes to lie about its sample rate. I do not know why. It is always 32000 no matter what.
                    Utilities.ByteswapU32((uint)((double)entry.SampleCount / (double)entry.SampleRate * 32000))
                );
                writer.Write(Utilities.ByteswapU32((uint)(data.Length - 0xA0))); // 0xA0 is where ADPCM audio data starts in this custom file format.
                writer.Write(Utilities.ByteswapU32(32000)); // Sample Rate (hardcoded)
                writer.Write(Utilities.ByteswapU16(Convert.ToUInt16(entry.Loop))); // Is Looped?
                writer.Write((ushort)0); // Format (always 0)
                writer.Write(0); // For loop points, the start and end are written in.
                writer.Write(Utilities.ByteswapU32((uint)(data.Length - 0xA0))); // 0xA0 is where ADPCM audio data starts in this custom file format.
                writer.Write(0); // Current address (always 0)
                writer.Write(data, 8 + x * 0x30, 0x2E); // 0x1C to 0x48 in the DSP header are already formulated for us in the custom file format.
                writer.Write(Utilities.ByteswapU16(entry.ChannelCount)); // Channel count
                writer.Write(Utilities.ByteswapU16(0x2000)); // Chunk size
                writer.Write(new byte[0x12] {
                    0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                    0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
                }); // Pad out the next 0x12 bytes to finish the header.
            }
            writer.Write(data, 0xA0, data.Length - 0xA0);
            ms.Position = 0;
            return ms;
        }
    }
}
