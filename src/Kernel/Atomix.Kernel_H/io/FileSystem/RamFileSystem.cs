﻿using System;

using Atomix.Kernel_H.lib;
using Atomix.Kernel_H.lib.crypto;
using Atomix.Kernel_H.io.FileSystem.RFS;

using System.Runtime.InteropServices;

namespace Atomix.Kernel_H.io.FileSystem
{
    public unsafe class RamFileSystem : GenericFileSystem
    {
        uint DataAddress;
        uint DataLength;
        IDictionary<string, RamFile> Files;

        public RamFileSystem(uint aAddress, uint aLength)
        {
            DataAddress = aAddress;
            DataLength = aLength;
            Files = new IDictionary<string, RamFile>(sdbm.GetHashCode, string.Equals);
            mIsValid = LoadFileSystem();
        }

        private bool LoadFileSystem()
        {
            var Entries = (FileEntry*)DataAddress;
            Entries++;
            while(Entries->StartAddress != 0)
            {
                var FileName = new string(Entries->Name);
                Files.Add(FileName, new RamFile(FileName, Entries->StartAddress + DataAddress, Entries->Length));
                Entries++;
            }
            return true;
        }

        public override Stream GetFile(string[] path, int pointer)
        {
            var FileName = path[pointer];
            if (Files.ContainsKey(FileName))
                return new FileStream(Files[FileName]);
            return null;
        }

        public override bool CreateFile(string[] path, int pointer)
        {
            return false;
        }

        #region Struct
        [StructLayout(LayoutKind.Explicit, Size = 32)]
        struct FileEntry
        {
            [FieldOffset(0)]
            public fixed char Name[12];

            [FieldOffset(24)]
            public uint StartAddress;

            [FieldOffset(28)]
            public uint Length;
        }
        #endregion
    }
}