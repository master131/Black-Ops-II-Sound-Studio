using BlackOps2SoundStudio.Properties;
using System;
using System.Runtime.InteropServices;

namespace BlackOps2SoundStudio
{
    static class MappedModuleDatabase
    {
        public static MemoryModule libFLAC { get; private set; }

        static MappedModuleDatabase()
        {
            libFLAC = new MemoryModule("libFLAC.dll");
        }
    }

    class MemoryModule
    {
        [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern IntPtr LoadLibrary(string lpFileName);

        [DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

        public IntPtr ModuleHandle { get; set; }

        public MemoryModule(string path)
        {
            ModuleHandle = LoadLibrary(path);
            if (ModuleHandle == IntPtr.Zero)
                throw new DllNotFoundException("Couldn't find " + path);
        }

        public IntPtr GetProcAddress(string procName)
        {
            return GetProcAddress(ModuleHandle, procName);
        }
    }
}
