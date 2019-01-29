using System;
using System.IO;

namespace BlackOps2SoundStudio.Format
{
    class HeaderlessFLACStream : Stream
    {
        public Stream BaseStream { get; }
        private long BaseStreamStart { get; set; }
        private byte[] Header { get; set; }
        private SndAssetBankEntry Entry { get; }

        private const int HeaderSize = 0x2A;

        public HeaderlessFLACStream(Stream baseStream, SndAssetBankEntry entry) {
            if (!baseStream.CanRead || !baseStream.CanSeek)
                throw new NotSupportedException();

            BaseStream = baseStream;
            Entry = entry;
        }

        private static byte[] Reverse(byte[] arr, int count = -1) {
            if (count > 0)
                Array.Resize(ref arr, count);
            Array.Reverse(arr);
            return arr;
        }

        private void CreateHeader() {
            using (var ms = new MemoryStream())
            using (var bw = new BinaryWriter(ms))
            {
                // Write FLAC header, note that values are big-endian
                // ---------------------
                // STREAM
                // ---------------------
                bw.Write(0x43614C66); // fLaC

                // ---------------------
                // METADATA_BLOCK
                // ---------------------
                // METADATA_BLOCK_HEADER
                bw.Write((byte) (128)); // Last-metadata-block flag + STREAMINFO block type
                bw.Write(Reverse(BitConverter.GetBytes(34), 3)); // STREAMINFO block size (24-bit int)

                // ---------------------
                // METADATA_BLOCK_STREAMINFO
                // ---------------------
                bw.Write(Reverse(BitConverter.GetBytes((ushort) 0x400))); // Minimum block size (in samples)
                bw.Write(Reverse(BitConverter.GetBytes((ushort) 0x400))); // Minimum block size (in samples)
                bw.Write(new byte[3]); // Minimum frame size (in bytes), can be 0 if unknown
                bw.Write(new byte[3]); // Maximum frame size (in bytes), can be 0 if unknown
                ulong data = (ulong)Entry.SampleRate << 44; // Sample rate in Hz.
                data += ((ulong)Entry.ChannelCount - 1) << 41; // (number of channels)-1
                data += (ulong) 15 << 36; // (bits per sample)-1, we assume 16-bits per sample
                data += (ulong)Entry.SampleCount;
                bw.Write(Reverse(BitConverter.GetBytes(data)));

                bw.Write(0L); /* MD5 */
                bw.Write(0L); /* MD5 */

                Header = ms.ToArray();
            }
        }

        private void FindFirstFrame() {
            var br = new BinaryReader(BaseStream);
            long oldPosition = BaseStream.Position;
            BaseStream.Position = Entry.Offset;
            // Search the current FLAC data for the FF F8 byte sequence
            // which acts as a marker for the first audio frame
            while (BaseStream.Position < Entry.Offset + Entry.Size) {
                if (br.ReadUInt16() == 0xf8ff) {
                    BaseStreamStart = BaseStream.Position - 2;
                    break;
                }
            }
            BaseStream.Position = oldPosition;
        }

        public override void Flush() {
        }

        public override long Seek(long offset, SeekOrigin origin) {
            switch (origin)
            {
                case SeekOrigin.Begin:
                    return Position = offset;
                case SeekOrigin.Current:
                    return Position += offset;
                case SeekOrigin.End:
                    return Position = Length + offset;
            }
            return Position;
        }

        public override void SetLength(long value) {
            throw new InvalidOperationException();
        }

        public override int Read(byte[] buffer, int offset, int count) {
            if (offset < 0 || count < 0 || offset + count > buffer.Length)
                throw new IndexOutOfRangeException();

            // Create the header
            if (Header == null)
            {
                CreateHeader();
                FindFirstFrame();
            }

            // Write out the header first
            var readTotal = 0;
            if (Position < Header.Length)
            {
                var toRead = (int) Math.Min(Header.Length - Position, count);
                Array.Copy(Header, Position, buffer, offset, toRead);
                count -= toRead;
                readTotal += toRead;
                Position += toRead;
            }

            // Shift the base stream position to match the current position
            if (Position >= Header.Length)
                BaseStream.Position = BaseStreamStart + (Position - Header.Length);

            // Clamp the count to not exceed the entry size
            count = (int) Math.Min(count, Entry.Offset + Entry.Size - BaseStream.Position);

            // Write the stream if there's space
            if (count > 0)
            {
                var result = BaseStream.Read(buffer, offset + readTotal, count);
                if (result < 0)
                    return readTotal > 0 ? readTotal : -1;
                readTotal += result;
                Position += result;
            }

            return readTotal;
        }

        public override void Write(byte[] buffer, int offset, int count) {
            throw new InvalidOperationException();
        }

        public override bool CanRead => BaseStream.CanRead;
        public override bool CanSeek => BaseStream.CanSeek;
        public override bool CanWrite => false;
        public override long Length => BaseStream.Length + HeaderSize;
        public override long Position { get; set; }
    }
}
