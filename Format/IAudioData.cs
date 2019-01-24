using System;

namespace BlackOps2SoundStudio.Format
{
    interface IAudioData : IDisposable
    {
        byte[] Get();
        void Cache();
        void Clear();
        int Length { get; }
    }
}
