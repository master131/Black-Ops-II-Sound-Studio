﻿using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
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
        public static Dictionary<int, string> Names { get; set; } 

        static SndAliasNameDatabase()
        {
            byte[] names;
            using (var ms = new MemoryStream(Resources.Names))
            using (var deflate = new DeflateStream(ms, CompressionMode.Decompress))
            {
                using (var decomp = new MemoryStream())
                {
                    deflate.CopyTo(decomp);
                    names = decomp.ToArray();
                }
            }

            Names = new Dictionary<int, string>();
            var document = XDocument.Parse(Encoding.UTF8.GetString(names));
            var entries = document.Element("Entries");
            if (entries == null) return;
            foreach (var desc in entries.Elements("Entry"))
            {
                var hash = desc.Attribute("Hash");
                var path = desc.Attribute("Path");
                if (hash == null || path == null) continue;
                Names.Add(int.Parse(hash.Value, NumberStyles.HexNumber), path.Value);
            }
        }

#if BUILD_BO3
        public static void BuildBO3()
        {
            const string gamePath = @"I:\Call.of.Duty.Black.Ops.III.Beta.SteamEarlyAccess-Fisher\Call of Duty Black Ops III Beta\zone\snd";

            foreach (var sabl in Directory.GetFiles(gamePath, "*.sabl", SearchOption.AllDirectories))
            {
                var sndBank = SndAliasBankReader.Read(sabl);
                sndBank.Dispose();
            }

            foreach (var sabs in Directory.GetFiles(gamePath, "*.sabs", SearchOption.AllDirectories))
            {
                var sndBank = SndAliasBankReader.Read(sabs);
                sndBank.Dispose();
            }

            var document = new XDocument(
                new XElement("Entries",
                    Names.Select(kvp => new XElement("Entry",
                        new XAttribute("Hash", kvp.Key), new XAttribute("Path", kvp.Value)))));


            using (var writer = XmlWriter.Create("Names.xml", new XmlWriterSettings { Indent = true }))
                document.WriteTo(writer);
        }
#endif

#if BUILD_XML
        public static void BuildXML()
        {
            if (!Directory.Exists("Identifiers"))
                return;

            var dict = new Dictionary<string, string>();
            foreach (var file in Directory.GetFiles("Identifiers"))
            {
                var lines = File.ReadAllLines(file);
                foreach (var line in lines)
                {
                    if (string.IsNullOrEmpty(line))
                        continue;

                    var split = line.Split('-');
                    if (split.Length != 2)
                        continue;

                    if (!dict.ContainsKey(split[0]))
                        dict.Add(split[0], split[1]);
                }
            }

            var document = new XDocument(
                new XElement("Entries",
                    dict.Select(kvp => new XElement("Entry",
                        new XAttribute("Hash", kvp.Key), new XAttribute("Path", kvp.Value)))));


            using (var writer = XmlWriter.Create("Names.xml", new XmlWriterSettings { Indent = true }))
                document.WriteTo(writer);
        }
#endif
    }
}
