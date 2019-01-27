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
            return AddXMAHeader(entry);
        }
    }
}
