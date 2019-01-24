using System;

namespace BlackOps2SoundStudio.Format
{
    class SndAssetBankEntryT7 : SndAssetBankEntry
    {
        public int Pad { get; set; }
        public long Unknown { get; set; }
    }

    class SndAssetBankEntry
    {
        private static readonly int[] SampleRateTable = new[] { 8000, 12000, 16000, 24000, 32000, 44100, 48000, 96000, 192000 };

        public int Identifier { get; set; }
        public int Size { get; set; }
        public long Offset { get; set; }
        public int SampleCount { get; set; }
        public byte SampleRateFlag { get; set; }
        public byte ChannelCount { get; set; }
        public bool Loop { get; set; }
        public AudioFormat Format { get; set; }
        public IAudioData Data { get; set; }

        public int SampleRate
        {
            get
            {
                if (SampleRateFlag >= SampleRateTable.Length)
                    throw new InvalidOperationException("Sample rate flag must be less than 9.");
                return SampleRateTable[SampleRateFlag];
            }
            set
            {
                int index = Array.FindIndex(SampleRateTable, s => s == value);
                if (index == -1)
                    throw new ArgumentOutOfRangeException("value", "The specified sample rate is not supported.");
                SampleRateFlag = (byte) index;
            }
        }

        public TimeSpan Duration
        {
            get
            {
                return TimeSpan.FromMilliseconds(1000 * (float) SampleCount / SampleRate);
            }
            set
            {
                SampleCount = (int) (value.TotalMilliseconds * SampleRate / 1000);
            }
        }
    }
}
