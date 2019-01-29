using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

namespace BlackOps2SoundStudio.Format
{
    class SndAliasBankReader : IDisposable
    {
        private Stream _stream;
        private BinaryReader _reader;
        private SndAliasBank _sndAliasBank;

        private SndAliasBankReader(Stream stream)
        {
            _stream = stream;
            _reader = new BinaryReader(stream);
            _sndAliasBank = new SndAliasBank(stream);
        }

        internal void Read()
        {
            // Check the stream.
            if (_stream.CanSeek && _stream.Length < 0x800)
                throw new BadImageFormatException("The specified data is smaller than the size of the header.");

            // Check the magic.
            if ((_sndAliasBank.Magic = _reader.ReadInt32()) != SndAliasConstants.Magic)
                throw new BadImageFormatException("Invalid magic value detected, ensure that the file is valid.");

            // Check the version.
            var supportedVersions = new[] {SndAliasConstants.Version_T6, SndAliasConstants.Version_T7, SndAliasConstants.Version_IW7};
            if (!supportedVersions.Contains(_sndAliasBank.Version = _reader.ReadInt32()))
                throw new BadImageFormatException("Invalid version detected, expected [" + string.Join(", ", supportedVersions.Select(i => i.ToString())) + "] but got: " + _sndAliasBank.Version);

            int sizeOfAudioEntry = 0;
            switch (_sndAliasBank.Version)
            {
                case SndAliasConstants.Version_T6:
                    sizeOfAudioEntry = SndAliasConstants.SizeOfAudioEntry_T6;
                    break;
                case SndAliasConstants.Version_T7:
                    sizeOfAudioEntry = SndAliasConstants.SizeOfAudioEntry_T7;
                    break;
                case SndAliasConstants.Version_IW7:
                    sizeOfAudioEntry = SndAliasConstants.SizeOfAudioEntry_IW7;
                    break;
            }

            // Check the sizes.
            if ((_sndAliasBank.SizeOfAudioEntry = _reader.ReadInt32()) != sizeOfAudioEntry)
                throw new BadImageFormatException("Invalid audio entry size detected, expected " + sizeOfAudioEntry + " but got: " + _sndAliasBank.SizeOfAudioEntry);

            if ((_sndAliasBank.SizeOfChecksumEntry = _reader.ReadInt32()) != SndAliasConstants.SizeOfHashEntry)
                throw new BadImageFormatException("Invalid checksum entry size detected, expected " + SndAliasConstants.SizeOfHashEntry + " but got: " + _sndAliasBank.SizeOfChecksumEntry);

            if ((_sndAliasBank.SizeOfDependencyEntry = _reader.ReadInt32()) != SndAliasConstants.SizeOfStringEntry)
                throw new BadImageFormatException("Invalid depedency entry size detected, expected " + SndAliasConstants.SizeOfStringEntry + " but got: " + _sndAliasBank.SizeOfDependencyEntry);

            // Read the number of entries.
            int entryCount = _reader.ReadInt32();
            int dependencyCount = _reader.ReadInt32();

            // Padding.
            _stream.Position += 4;

            // Read the offsets and lengths.
            _sndAliasBank.Length = _reader.ReadInt64();
            _sndAliasBank.OffsetOfEntries = _reader.ReadInt64();
            _sndAliasBank.OffsetOfChecksums = _reader.ReadInt64();

            // Verify the lengths and offsets.
            if (_stream.CanSeek)
            {
                if (_sndAliasBank.Length > _stream.Length)
                    throw new BadImageFormatException("Invalid length value detected, the file may be corrupt or partially downloaded.");
                if (_sndAliasBank.OffsetOfEntries > _stream.Length)
                    throw new BadImageFormatException("Invalid audio entry offset detected, the file may be corrupt or partially downloaded.");
                if (_sndAliasBank.OffsetOfChecksums > _stream.Length)
                    throw new BadImageFormatException("Invalid checksum entry offset detected, the file may be corrupt or partially downloaded.");
            }

            // Read the asset link identifier. Only thing known is that it's in the zone
            // before the snd_alias_list_t struct.
            _sndAliasBank.AssetLinkIdentifier = _reader.ReadBytes(16);

            // Read the asset references, which are like #include directives because the
            // zone asset may reference an audio entry that is not actually inside the
            // referenced SndAliasBank.
            _sndAliasBank.AssetReferences = new string[SndAliasConstants.MaxReferenceCount];
            for (int i = 0; i < dependencyCount; i++)
            {
                var data = _reader.ReadBytes(SndAliasConstants.SizeOfStringEntry);
                var index = Array.FindIndex(data, b => b == 0);
                _sndAliasBank.AssetReferences[i] = index != -1 ? Encoding.ASCII.GetString(data, 0, index) :
                    Encoding.ASCII.GetString(data);
            }

            // Read the audio entries.
            _stream.Position = _sndAliasBank.OffsetOfEntries;
            _sndAliasBank.Entries = new List<SndAssetBankEntry>();
            if (_sndAliasBank.Version == SndAliasConstants.Version_T7)
                ReadEntriesT7(entryCount);
            else if (_sndAliasBank.Version == SndAliasConstants.Version_T6)
                ReadEntriesT6(entryCount);
            else if (_sndAliasBank.Version == SndAliasConstants.Version_IW7)
                ReadEntriesIW7(entryCount);

            // Read the hashes.
            _stream.Position = _sndAliasBank.OffsetOfChecksums;
            _sndAliasBank.Checksums = new List<byte[]>();

            for (int i = 0; i < entryCount; i++)
                _sndAliasBank.Checksums.Add(_reader.ReadBytes(16));

            // Read the names.
            if (_sndAliasBank.Version == SndAliasConstants.Version_IW7)
                ReadNamesIW7();
            /*
            if (_sndAliasBank.Version == SndAliasConstants.Version_T7)
                ReadNamesT7();
            */
        }

        private void ReadNamesIW7() {
            _stream.Position = 0x250;
            var namesPosition = _reader.ReadInt64();
            if (namesPosition == 0)
                return;

            _stream.Position = namesPosition;
            for (int i = 0; i < _sndAliasBank.Entries.Count; i++)
            {
                var nameBytes = _reader.ReadBytes(0x80);
                var sb = new StringBuilder();
                int x = 0;
                while (x < nameBytes.Length && nameBytes[x] != 0)
                {
                    sb.Append((char)nameBytes[x]);
                    x++;
                }
                SndAliasNameDatabase.Names[_sndAliasBank.Entries[i].Identifier] = sb.ToString();
            }
        }

        /*
        private void ReadNamesT7()
        {
            // Begin reading the names.
            _stream.Position = 0x250;
            _stream.Position = _reader.ReadInt64();
            for (int i = 0; i < _sndAliasBank.Entries.Count; i++)
            {
                var nameBytes = _reader.ReadBytes(0x80);
                var sb = new StringBuilder();
                int x = 0;
                while (x < nameBytes.Length && nameBytes[x] != 0)
                {
                    sb.Append((char) nameBytes[x]);
                    x++;
                }
                if (!SndAliasNameDatabase.Names.ContainsKey(_sndAliasBank.Entries[i].Identifier))
                    SndAliasNameDatabase.Names.Add(_sndAliasBank.Entries[i].Identifier, sb.ToString());
                else
                    SndAliasNameDatabase.Names[_sndAliasBank.Entries[i].Identifier] = sb.ToString();
            }
        }
        */

        private void ReadEntriesIW7(int entryCount)
        {
            for (int i = 0; i < entryCount; i++)
            {
                var entry = new SndAssetBankEntryIW7
                {
                    Identifier = _reader.ReadInt32(),
                    Size = _reader.ReadInt32(),
                    Unknown = _reader.ReadInt32(),
                    SampleCount = _reader.ReadInt32(),
                    Unknown2 = _reader.ReadInt32(),
                    Offset = _reader.ReadInt64(),
                    SampleRate = _reader.ReadInt32(),
                    ChannelCount = _reader.ReadByte(),
                    Loop = _reader.ReadBoolean(),
                    Format = (AudioFormat)_reader.ReadByte(),
                    Unknown3 = _reader.ReadBytes(count: 0x9)
                };
                if (entry.Format == AudioFormat.FLAC) {
                    // Use a stream wrapper that appends a FLAC header
                    entry.Data = new AudioDataStream(new HeaderlessFLACStream(_stream, entry), entry.Size);
                }
                else {
                    entry.Data = new AudioDataStream(_stream, entry.Size, entry.Offset);
                }
                
                _sndAliasBank.Entries.Add(entry);
            }
        }

        private void ReadEntriesT7(int entryCount)
        {
            for (int i = 0; i < entryCount; i++)
            {
                var entry = new SndAssetBankEntryT7
                {
                    Identifier = _reader.ReadInt32(),
                    Size = _reader.ReadInt32(),
                    SampleCount = _reader.ReadInt32(),
                    Pad = _reader.ReadInt32(),
                    Offset = _reader.ReadInt64(),
                    SampleRateFlag = _reader.ReadByte(),
                    ChannelCount = _reader.ReadByte(),
                    Loop = _reader.ReadBoolean(),
                    Format = (AudioFormat)_reader.ReadByte(),
                    Unknown = _reader.ReadInt64()
                };
                entry.Data = new AudioDataStream(_stream, entry.Size, entry.Offset);
                _sndAliasBank.Entries.Add(entry);
            }
        }

        private void ReadEntriesT6(int entryCount)
        {
            for (int i = 0; i < entryCount; i++)
            {
                var entry = new SndAssetBankEntryT6
                {
                    Identifier = _reader.ReadInt32(),
                    Size = _reader.ReadInt32(),
                    Offset = _reader.ReadInt32(),
                    SampleCount = _reader.ReadInt32(),
                    SampleRateFlag = _reader.ReadByte(),
                    ChannelCount = _reader.ReadByte(),
                    Loop = _reader.ReadBoolean(),
                    Format = (AudioFormat)_reader.ReadByte(),
                };
                entry.Data = new AudioDataStream(_stream, entry.Size, entry.Offset);
                _sndAliasBank.Entries.Add(entry);
            }
        }

        public static SndAliasBank Read(string path)
        {
            Stream stream = null;

            try
            {
                stream = File.Open(path, FileMode.Open, FileAccess.ReadWrite, FileShare.Read);
                return Read(stream);
            }
            catch
            {
                if (stream != null)
                    stream.Dispose();

                throw;
            }
        }

        public static SndAliasBank Read(byte[] data)
        {
            return Read(new MemoryStream(data));
        }

        public static SndAliasBank Read(Stream stream)
        {
            var reader = new SndAliasBankReader(stream);
            reader.Read();
            return reader._sndAliasBank;
        }

        public void Dispose()
        {
            _reader.Close();
            _sndAliasBank.Dispose();
        }
    }
}
