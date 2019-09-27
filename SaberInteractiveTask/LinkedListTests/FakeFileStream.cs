using System;
using System.IO;
using System.Security.AccessControl;
using Microsoft.Win32.SafeHandles;

namespace LinkedListTests
{
    /// <summary>
    /// This class is fake file stream for testing purposes.
    /// </summary>
    internal class FakeFileStream : FileStream
    {
        public FakeFileStream(SafeFileHandle handle, FileAccess access) : base(handle, access)
        {
        }

        public FakeFileStream(SafeFileHandle handle, FileAccess access, int bufferSize) : base(handle, access,
            bufferSize)
        {
        }

        public FakeFileStream(SafeFileHandle handle, FileAccess access, int bufferSize, bool isAsync) : base(handle,
            access, bufferSize, isAsync)
        {
        }

        public FakeFileStream(IntPtr handle, FileAccess access) : base(handle, access)
        {
        }

        public FakeFileStream(IntPtr handle, FileAccess access, bool ownsHandle) : base(handle, access, ownsHandle)
        {
        }

        public FakeFileStream(IntPtr handle, FileAccess access, bool ownsHandle, int bufferSize) : base(handle, access,
            ownsHandle, bufferSize)
        {
        }

        public FakeFileStream(IntPtr handle, FileAccess access, bool ownsHandle, int bufferSize, bool isAsync) : base(
            handle, access, ownsHandle, bufferSize, isAsync)
        {
        }

        public FakeFileStream(string path, FileMode mode) : base(path, mode)
        {
        }

        public FakeFileStream(string path, FileMode mode, FileAccess access) : base(path, mode, access)
        {
        }

        public FakeFileStream(string path, FileMode mode, FileAccess access, FileShare share) : base(path, mode, access,
            share)
        {
        }

        public FakeFileStream(string path, FileMode mode, FileAccess access, FileShare share, int bufferSize) : base(
            path, mode, access, share, bufferSize)
        {
        }

        public FakeFileStream(string path, FileMode mode, FileAccess access, FileShare share, int bufferSize,
            bool useAsync) : base(path, mode, access, share, bufferSize, useAsync)
        {
        }

        public FakeFileStream(string path, FileMode mode, FileAccess access, FileShare share, int bufferSize,
            FileOptions options) : base(path, mode, access, share, bufferSize, options)
        {
        }

        public FakeFileStream(string path, FileMode mode, FileSystemRights rights, FileShare share, int bufferSize,
            FileOptions options) : base(path, mode, rights, share, bufferSize, options)
        {
        }

        public FakeFileStream(string path, FileMode mode, FileSystemRights rights, FileShare share, int bufferSize,
            FileOptions options, FileSecurity fileSecurity) : base(path, mode, rights, share, bufferSize, options,
            fileSecurity)
        {
        }

        private byte[] bytes;
        private long currentByte = 0;

        public override void Write(byte[] array, int offset, int count)
        {
            bytes = new byte[array.Length];
            Array.Copy(array, bytes, array.Length);
            currentByte = 0;
        }

        public override int Read(byte[] array, int offset, int count)
        {
            Array.Copy(bytes, array, bytes.Length);
            return 0;
        }

        public override int ReadByte()
        {
            if (currentByte >= bytes.Length)
            {
                return -1;
            }

            return bytes[currentByte++];
        }
    }
}