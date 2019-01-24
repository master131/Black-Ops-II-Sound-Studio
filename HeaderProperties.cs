using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;

namespace BlackOps2SoundStudio
{
    [TypeConverter(typeof(PropertySorter))]
    class HeaderProperties
    {
        [Browsable(false)]
        public int Magic { get; set; }

        [Category("Header"), ReadOnly(true), DisplayName("Magic"), PropertyOrder(1)]
        public string MagicDisplay
        {
            get { return "0x" + Magic.ToString("X"); }
        }

        [Category("Header"), ReadOnly(true), DisplayName("Version"), PropertyOrder(2)]
        public int Version { get; set; }

        [Category("Header"), ReadOnly(true), DisplayName("Size of Audio Entry"), PropertyOrder(3)]
        public int SizeOfAudioEntry { get; set; }

        [Category("Header"), ReadOnly(true), DisplayName("Size of Hash Entry"), PropertyOrder(4)]
        public int SizeOfHashEntry { get; set; }

        [Category("Header"), ReadOnly(true), DisplayName("Size of String Entry"), PropertyOrder(5)]
        public int SizeOfStringEntry { get; set; }

        [Category("Header"), ReadOnly(true), DisplayName("Audio Entry Count"), PropertyOrder(6)]
        public int NumberOfAudioEntries { get; set; }

        [Category("Header"), ReadOnly(true), DisplayName("Asset Link ID"), PropertyOrder(7)]
        public string AssetLinkID { get; set; }

        [Category("Header"), DisplayName("Asset References"), Editor("System.Windows.Forms.Design.StringCollectionEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        [PropertyOrder(8)]
        public List<string> AssetReferences { get; set; }
    }
}
