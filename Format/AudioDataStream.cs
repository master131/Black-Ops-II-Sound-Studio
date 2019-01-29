using System.IO;

namespace BlackOps2SoundStudio.Format
{
    class AudioDataStream : IAudioData
    {
        private Stream _stream;
        private long _offset;
        private int _length;
        private byte[] _cache;

        public Stream Stream {
            get { return _stream; }
        }

        public int Length { get; private set; }

        public AudioDataStream(Stream stream, int length, long offset = 0)
        {
            _stream = stream;
            _offset = offset;
            _length = length;
            Length = _length;
        }

        public byte[] Get()
        {
            if (_cache != null)
                return _cache;

            var prev = _stream.Position;
            _stream.Position = _offset;
            var reader = new BinaryReader(_stream);
            var data = reader.ReadBytes(_length);
            _stream.Position = prev;
            return data;
        }

        public void Cache()
        {
            _cache = Get();
        }

        public void Clear()
        {
            _cache = null;
        }

        public void Dispose()
        {
            _stream.Close();
            _cache = null;
        }
    }
}
