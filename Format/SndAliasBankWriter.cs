using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace BlackOps2SoundStudio.Format
{
    class SndAliasBankWriter
    {
        private SndAliasBank _sndAliasBank;

        public SndAliasBankWriter(SndAliasBank sndAliasBank)
        {
            _sndAliasBank = sndAliasBank;
        }

        public void Write(Stream stream, bool autoCalculate, bool fixHashes = false)
        {
            // If the source and destination are identical, force audio-data to be cached
            // into memory first.
            if (Utilities.SameSourceOrDestination(stream, _sndAliasBank._stream))
            {
                foreach (var entry in _sndAliasBank.Entries)
                    entry.Data.Cache();
            }

            // Calculate asset reference count
            var assetReferenceCount = 0L;
            for (assetReferenceCount = 0; assetReferenceCount < _sndAliasBank.AssetReferences.Length; assetReferenceCount++)
                if (_sndAliasBank.AssetReferences[assetReferenceCount] == null)
                    break;

            // Write the header.
            stream.SetLength(0);
            var writer = new BinaryWriter(stream);
            writer.Write(_sndAliasBank.Magic);
            writer.Write(_sndAliasBank.Version);
            writer.Write(_sndAliasBank.SizeOfAudioEntry);
            writer.Write(_sndAliasBank.SizeOfChecksumEntry);
            writer.Write(_sndAliasBank.SizeOfDependencyEntry);
            writer.Write(_sndAliasBank.Entries.Count);
            writer.Write(assetReferenceCount);
            var lengthAndOffsetsPosition = stream.Position;
            writer.Write(_sndAliasBank.Length);
            writer.Write(_sndAliasBank.OffsetOfEntries);
            writer.Write(_sndAliasBank.OffsetOfChecksums);
            writer.Write(_sndAliasBank.AssetLinkIdentifier);
            
            // Write the asset references.
            for (int i = 0; i < _sndAliasBank.AssetReferences.Length; i++)
            {
                var value = _sndAliasBank.AssetReferences[i] ?? string.Empty;
                
                // Truncate the reference name if required.
                if (value.Length > _sndAliasBank.SizeOfDependencyEntry)
                    value = value.Substring(0, _sndAliasBank.SizeOfDependencyEntry);

                // Write the value.
                writer.Write(Encoding.ASCII.GetBytes(value));
                
                // Pad the string with zeros.
                var padding = new byte[_sndAliasBank.SizeOfDependencyEntry - value.Length];
                writer.Write(padding);
            }

            // Align the header to 0x800.
            var align = new byte[0x800 - stream.Position];
            writer.Write(align);

            // Write the file as-is, or calculate the fields manually.
            if (autoCalculate)
            {
                switch (_sndAliasBank.Version)
                {
                    case SndAliasConstants.Version_T6:
                        WriteCalculatedDataAndStructures(writer, lengthAndOffsetsPosition, fixHashes);
                        break;
                    case SndAliasConstants.Version_T7:
                        WriteCalculatedDataAndStructures_T7(writer, lengthAndOffsetsPosition, fixHashes);
                        break;
                }
            }
            else
            {
                switch (_sndAliasBank.Version)
                {
                    case SndAliasConstants.Version_T6:
                        WriteDataAndStructures(writer);
                        break;
                    case SndAliasConstants.Version_T7:
                        WriteDataAndStructures_T7(writer);
                        break;
                }
            }
        }

        public void WriteDataAndStructures(BinaryWriter writer)
        {
            // Set the stream length to the specified value.
            var stream = writer.BaseStream;
            stream.SetLength(_sndAliasBank.Length);

            // Begin writing the entries.
            stream.Position = _sndAliasBank.OffsetOfEntries;
            foreach (var entry in _sndAliasBank.Entries)
            {
                writer.Write(entry.Identifier);
                writer.Write(entry.Size);
                writer.Write((int)entry.Offset);
                writer.Write(entry.SampleCount);
                writer.Write(((SndAssetBankEntryT6) entry).SampleRateFlag);
                writer.Write(entry.ChannelCount);
                writer.Write(entry.Loop);
                writer.Write((byte) entry.Format);

                // Save the position.
                var pos = stream.Position;

                // Seek to the offset and write the data.
                stream.Position = entry.Offset;
                writer.Write(entry.Data.Get());

                // Restore the position.
                stream.Position = pos;
            }

            // Write the hash entries.
            stream.Position = _sndAliasBank.OffsetOfChecksums;
            foreach (var hash in _sndAliasBank.Checksums)
                writer.Write(hash);
        }

        public void WriteDataAndStructures_T7(BinaryWriter writer)
        {
            // Set the stream length to the specified value.
            var stream = writer.BaseStream;
            stream.SetLength(_sndAliasBank.Length);

            // Begin writing the entries.
            stream.Position = _sndAliasBank.OffsetOfEntries;
            foreach (SndAssetBankEntryT7 entry in _sndAliasBank.Entries)
            {
                writer.Write(entry.Identifier);
                writer.Write(entry.Size);
                writer.Write(entry.SampleCount);
                writer.Write(entry.Pad);
                writer.Write((int)entry.Offset);
                writer.Write(entry.SampleRateFlag);
                writer.Write(entry.ChannelCount);
                writer.Write(entry.Loop);
                writer.Write((byte)entry.Format);
                writer.Write(entry.Unknown);

                // Save the position.
                var pos = stream.Position;

                // Seek to the offset and write the data.
                stream.Position = entry.Offset;
                writer.Write(entry.Data.Get());

                // Restore the position.
                stream.Position = pos;
            }

            // Write the hash entries.
            stream.Position = _sndAliasBank.OffsetOfChecksums;
            foreach (var hash in _sndAliasBank.Checksums)
                writer.Write(hash);
        }

        private static long PaddingAlign(long num, int alignTo)
        {
            return num % alignTo == 0 ? num : alignTo - num % alignTo;
        }

        public void WriteCalculatedDataAndStructures(BinaryWriter writer, long lengthAndOffsetsPosition, bool fixHashes)
        {
            var stream = writer.BaseStream;
            var md5 = MD5.Create();

            // Begin writing the audio data.
            for (int i = 0; i < _sndAliasBank.Entries.Count; i++)
            {
                var entry = _sndAliasBank.Entries[i];

                // Update the entry.
                var data = entry.Data.Get();
                entry.Offset = (int) stream.Position;
                entry.Size = data.Length;

                // Calculate the MD5 hash.
                if (fixHashes)
                    _sndAliasBank.Checksums[i] = md5.ComputeHash(data);

                // Write the data.
                writer.Write(data);
            }

            // Align the file to 0x800 bytes.
            var align = new byte[PaddingAlign(stream.Length, 0x800)];
            writer.Write(align);

            // Update the entry offset and write entries.
            var entriesOffset = stream.Position;
            foreach (var entry in _sndAliasBank.Entries)
            {
                writer.Write(entry.Identifier);
                writer.Write(entry.Size);
                writer.Write((int)entry.Offset);
                writer.Write(entry.SampleCount);
                writer.Write(((SndAssetBankEntryT6)entry).SampleRateFlag);
                writer.Write(entry.ChannelCount);
                writer.Write(entry.Loop ? (byte) 1 : (byte) 0);
                writer.Write((byte) entry.Format);
            }

            // Align the file to 0x800 bytes.
            align = new byte[PaddingAlign(stream.Length, 0x800)];
            writer.Write(align);

            // Update the hash offset and write hashes.
            var hashesOffset = stream.Position;
            foreach (var hash in _sndAliasBank.Checksums)
                writer.Write(hash);

            // Align the file to 0x800 bytes.
            align = new byte[PaddingAlign(stream.Length, 0x800)];
            writer.Write(align);

            // Fix the headers.
            stream.Position = lengthAndOffsetsPosition;
            writer.Write(stream.Length);
            writer.Write(entriesOffset);
            writer.Write(hashesOffset);
        }

        public void WriteCalculatedDataAndStructures_T7(BinaryWriter writer, long lengthAndOffsetsPosition, bool fixHashes)
        {
            var stream = writer.BaseStream;
            var md5 = MD5.Create();

            // Begin writing the audio data.
            for (int i = 0; i < _sndAliasBank.Entries.Count; i++)
            {
                var entry = _sndAliasBank.Entries[i];

                // Update the entry.
                var data = entry.Data.Get();
                entry.Offset = (int) stream.Position;
                entry.Size = data.Length;

                // Calculate the MD5 hash.
                if (fixHashes)
                    _sndAliasBank.Checksums[i] = md5.ComputeHash(data);

                // Write the data.
                writer.Write(data);
            }

            // Align the file to 0x800 bytes.
            var align = new byte[PaddingAlign(stream.Length, 0x800)];
            writer.Write(align);

            // Update the entry offset and write entries.
            var entriesOffset = stream.Position;
            foreach (SndAssetBankEntryT7 entry in _sndAliasBank.Entries)
            {
                writer.Write(entry.Identifier);
                writer.Write(entry.Size);
                writer.Write(entry.SampleCount);
                writer.Write(entry.Pad);
                writer.Write((long)entry.Offset);
                writer.Write(entry.SampleRateFlag);
                writer.Write(entry.ChannelCount);
                writer.Write(entry.Loop ? (byte)1 : (byte)0);
                writer.Write((byte)entry.Format);
                writer.Write(entry.Unknown);
            }

            // Align the file to 0x800 bytes.
            align = new byte[PaddingAlign(stream.Length, 0x800)];
            writer.Write(align);

            // Update the hash offset and write hashes.
            var hashesOffset = stream.Position;
            foreach (var hash in _sndAliasBank.Checksums)
                writer.Write(hash);

            // Align the file to 0x800 bytes.
            align = new byte[PaddingAlign(stream.Length, 0x800)];
            writer.Write(align);

            // Fix the headers.
            stream.Position = lengthAndOffsetsPosition;
            writer.Write(stream.Length);
            writer.Write(entriesOffset);
            writer.Write(hashesOffset);
        }
    }
}
