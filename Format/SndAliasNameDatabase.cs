using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using BlackOps2SoundStudio.Properties;

#if BUILD_XML
using System.IO;
using System.Xml;
using System.Linq;
#endif

#if BUILD_BO3
using System.IO;
using System.Xml;
using System.Linq;
#endif

namespace BlackOps2SoundStudio.Format
{
    static class SndAliasNameDatabase
    {
        public static Dictionary<uint, string> Names { get; set; }

        public static uint SND_HashName(string str) // Modified DJB algorithm, compliant with Black Ops II.
        {
            str = str.ToLower();

            uint hash = 5381;

            for (int x = 0; x < str.Length; x++)
            {
                hash = (hash << 16) + (hash << 6) + str[x] - hash;
                if (hash == 0)
                    hash = 1;
            }

            return hash;
        }


        static SndAliasNameDatabase()
        {
            Names = new Dictionary<uint, string>();

            if (!File.Exists("Hashes.txt")) {
                MessageBox.Show("Hashes.txt was not found in the current directory.", "Error: File Not Found");
                Application.Exit();
            }

            foreach (var line in File.ReadLines("Hashes.txt"))
            {
                uint hash = SND_HashName(line);
                if (!Names.ContainsKey(hash))
                    Names.Add(hash, line);
            }
        }
    }
}
