using System.IO;

namespace BlackOps2SoundStudio.Converter
{
    /// <summary>
    /// Class designed to repair audio formats due to the un-seekable nature of the
    /// stdout stream used by FFmpeg.
    /// </summary>
    static class FormatRepair
    {
        public static void RepairWAVStream(Stream input)
        {
            input.Position = 0;
            var binaryReader = new BinaryReader(input);
            var binaryWriter = new BinaryWriter(input);

            // Verifiy the header.
            if (binaryReader.ReadInt32() != 0x46464952) // RIFF
                return;

            // Update the ChunkSize.
            binaryWriter.Write((uint)input.Length);

            if (binaryReader.ReadInt32() != 0x45564157) // WAVE
                return;

            // Keep reading subchunks til we hit the data one.
            while (binaryReader.ReadInt32() != 0x61746164) // data
                input.Position += binaryReader.ReadInt32() + 4; // SubChunkSize

            // Write the data size.
            binaryWriter.Write((uint)(input.Length - input.Position - 4));
            input.Position = 0;
        }
    }
}
