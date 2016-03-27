﻿using System;

using Atomix.Kernel_H.core;

namespace Atomix.Kernel_H.io
{
    public abstract class Stream
    {
        public readonly uint FileSize;

        public Stream(uint aSize)
        {
            FileSize = aSize;
        }

        public abstract bool CanSeek();
        public abstract int Position();
        public abstract bool CanRead();
        public abstract bool CanWrite();

        public abstract bool Write(byte[] aBuffer, int aCount);
        public abstract int Read(byte[] aBuffer, int aCount);
        public abstract bool Seek(int val, SEEK pos);

        public abstract bool Close();

        public string ReadToEnd()
        {
            var xData = new byte[FileSize];
            Read(xData, xData.Length);
            var xResult = lib.encoding.ASCII.GetString(xData, 0, xData.Length);
            Heap.Free(xData);
            return xResult;
        }
    }
    public enum SEEK
    {
        SEEK_FROM_ORIGIN,
        SEEK_FROM_CURRENT,
        SEEK_FROM_END,
    }
}