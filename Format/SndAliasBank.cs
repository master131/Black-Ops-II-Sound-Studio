using System;
using System.Collections.Generic;
using System.IO;

namespace BlackOps2SoundStudio.Format
{
    class SndAliasBank : IDisposable
    {
        internal Stream _stream;

        public int Magic { get; set; }
        public int Version { get; set; }
        public int SizeOfAudioEntry { get; set; }
        public int SizeOfChecksumEntry { get; set; }
        public int SizeOfDependencyEntry { get; set; }
        public List<SndAssetBankEntry> Entries { get; set; }
        public List<byte[]> Checksums { get; set; }
        public long Length { get; set; }
        public long OffsetOfEntries { get; set; }
        public long OffsetOfChecksums { get; set; }
        public byte[] AssetLinkIdentifier { get; set; }
        public string[] AssetReferences { get; set; }

        internal SndAliasBank(Stream stream)
        {
            _stream = stream;
        }

        public void Save(bool autoCalculate, bool fixHashes = false)
        {
            _stream.Position = 0;
            Save(_stream, autoCalculate, fixHashes);
        }

        public void Save(string path, bool autoCalculate, bool fixHashes = false)
        {
            using (var fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Read))
                Save(fs, autoCalculate, fixHashes);
        }

        public void Save(Stream stream, bool autoCalculate, bool fixHashes = false)
        {
            var writer = new SndAliasBankWriter(this);
            writer.Write(stream, autoCalculate, fixHashes);
        }

        public void Dispose()
        {
            _stream.Close();
        }
    }
}
