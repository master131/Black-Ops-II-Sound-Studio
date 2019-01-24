using System;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Windows.Forms;

namespace BlackOps2SoundStudio
{
    static class Utilities
    {
        public static bool SameSourceOrDestination(Stream first, Stream second)
        {
            if (first == second)
                return true;

            var firstFile = first as FileStream;
            var secondFile = second as FileStream;

            if (firstFile == null || secondFile == null)
                return false;

            return Path.GetFullPath(firstFile.Name) == Path.GetFullPath(secondFile.Name);
        }

        public static void PreserveStackTrace(Exception e)
        {
            var ctx = new StreamingContext(StreamingContextStates.CrossAppDomain);
            var mgr = new ObjectManager(null, ctx);
            var si = new SerializationInfo(e.GetType(), new FormatterConverter());

            e.GetObjectData(si, ctx);
            mgr.RegisterObject(e, 1, si); // prepare for SetObjectData
            mgr.DoFixups(); // ObjectManager calls SetObjectData

            // voila, e is unmodified save for _remoteStackTraceString
        }

        public static void CopyTo(this Stream input, Stream output)
        {
            var b = new byte[32768];
            int r;
            while ((r = input.Read(b, 0, b.Length)) > 0)
                output.Write(b, 0, r);
        }

        public static void DoubleBuffered(this DataGridView dgv, bool setting)
        {
            Type dgvType = dgv.GetType();
            PropertyInfo pi = dgvType.GetProperty("DoubleBuffered",
                BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(dgv, setting, null);
        }
    }
}
