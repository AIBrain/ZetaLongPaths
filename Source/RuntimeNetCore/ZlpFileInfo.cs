﻿namespace ZetaLongPaths
{
    using Microsoft.Win32.SafeHandles;
    using Native;
    using System;
    using System.Diagnostics;
    using System.Text;
    using FileAccess = Native.FileAccess;
    using FileAttributes = Native.FileAttributes;
    using FileShare = Native.FileShare;

    // ReSharper disable once UseNameofExpression
    [DebuggerDisplay(@"{FullName}")]
    public class ZlpFileInfo :
        IZlpFileSystemInfo
    {
        public static ZlpFileInfo GetTemp() => new ZlpFileInfo(ZlpPathHelper.GetTempFilePath());

        public ZlpFileInfo(string path)
        {
            FullName = path;
        }

        public ZlpFileInfo(System.IO.FileInfo path)
        {
            FullName = path?.FullName;
        }

        public ZlpFileInfo(ZlpFileInfo path)
        {
            FullName = path?.FullName;
        }

        public static ZlpFileInfo FromOther(ZlpFileInfo path)
        {
            return new ZlpFileInfo(path);
        }

        public static ZlpFileInfo FromString(string path)
        {
            return new ZlpFileInfo(path);
        }

        public static ZlpFileInfo FromBuiltIn(System.IO.FileInfo path)
        {
            return new ZlpFileInfo(path);
        }

        public System.IO.FileInfo ToBuiltIn()
        {
            return new System.IO.FileInfo(FullName);
        }

        public ZlpFileInfo ToOther()
        {
            return Clone();
        }

        public void Refresh()
        {
        }

        public ZlpFileInfo Clone()
        {
            return new ZlpFileInfo(FullName);
        }

        public void MoveToRecycleBin() => ZlpIOHelper.MoveFileToRecycleBin(FullName);

        public string OriginalPath => FullName;

        public override string ToString() => FullName;

        public void MoveTo(
            string destinationFilePath) => ZlpIOHelper.MoveFile(FullName, destinationFilePath);

        public void MoveTo(
            ZlpFileInfo destinationFilePath)
            => ZlpIOHelper.MoveFile(FullName, destinationFilePath.FullName);

        public void MoveTo(
            string destinationFilePath,
            bool overwriteExisting) => ZlpIOHelper.MoveFile(FullName, destinationFilePath, overwriteExisting);

        public void MoveTo(
            ZlpFileInfo destinationFilePath,
            bool overwriteExisting)
            => ZlpIOHelper.MoveFile(FullName, destinationFilePath.FullName, overwriteExisting);

        /// <summary>
        /// Pass the file handle to the <see cref="System.IO.FileStream"/> constructor. 
        /// The <see cref="System.IO.FileStream"/> will close the handle.
        /// </summary>
        public SafeFileHandle CreateHandle(
            CreationDisposition creationDisposition,
            FileAccess fileAccess,
            FileShare fileShare)
        {
            return ZlpIOHelper.CreateFileHandle(FullName, creationDisposition, fileAccess, fileShare);
        }

        public void CopyTo(
            string destinationFilePath,
            bool overwriteExisting)
        {
            ZlpIOHelper.CopyFile(FullName, destinationFilePath, overwriteExisting);
        }

        public void CopyTo(
            ZlpFileInfo destinationFilePath,
            bool overwriteExisting)
        {
            ZlpIOHelper.CopyFile(FullName, destinationFilePath.FullName, overwriteExisting);
        }

        public void CopyToExact(
            string destinationFilePath,
            bool overwriteExisting)
        {
            ZlpIOHelper.CopyFileExact(FullName, destinationFilePath, overwriteExisting);
        }

        public void CopyToExact(
            ZlpFileInfo destinationFilePath,
            bool overwriteExisting)
        {
            ZlpIOHelper.CopyFileExact(FullName, destinationFilePath.FullName, overwriteExisting);
        }

        public void Delete() => ZlpIOHelper.DeleteFile(FullName);

        public void DeleteFileAfterReboot() => ZlpIOHelper.DeleteFileAfterReboot(FullName);

        public void Touch() => ZlpIOHelper.Touch(FullName);

        public string Owner => ZlpIOHelper.GetFileOwner(FullName);

        public bool Exists => ZlpIOHelper.FileExists(FullName);

        public byte[] ReadAllBytes()
        {
            return ZlpIOHelper.ReadAllBytes(FullName);
        }

        public System.IO.FileStream OpenRead()
        {
            return new System.IO.FileStream(
                ZlpIOHelper.CreateFileHandle(
                    FullName,
                    CreationDisposition.OpenAlways,
                    FileAccess.GenericRead,
                    FileShare.Read),
                System.IO.FileAccess.Read);
        }

        public System.IO.FileStream OpenWrite()
        {
            return new System.IO.FileStream(
                ZlpIOHelper.CreateFileHandle(
                    FullName,
                    CreationDisposition.OpenAlways,
                    FileAccess.GenericRead | FileAccess.GenericWrite,
                    FileShare.Read | FileShare.Write),
                System.IO.FileAccess.ReadWrite);
        }

        public System.IO.FileStream OpenCreate()
        {
            return new System.IO.FileStream(
                ZlpIOHelper.CreateFileHandle(
                    FullName,
                    CreationDisposition.CreateAlways,
                    FileAccess.GenericRead | FileAccess.GenericWrite,
                    FileShare.Read | FileShare.Write),
                System.IO.FileAccess.ReadWrite);
        }

        public string ReadAllText() => ZlpIOHelper.ReadAllText(FullName);

        public string ReadAllText(Encoding encoding) => ZlpIOHelper.ReadAllText(FullName, encoding);

        public string[] ReadAllLines() => ZlpIOHelper.ReadAllLines(FullName);

        public string[] ReadAllLines(Encoding encoding) => ZlpIOHelper.ReadAllLines(FullName, encoding);

        public void WriteAllText(string text, Encoding encoding = null)
            => ZlpIOHelper.WriteAllText(FullName, text, encoding);

        public void AppendText(string text, Encoding encoding = null)
            => ZlpIOHelper.AppendText(FullName, text, encoding);

        public void WriteAllBytes(byte[] content) => ZlpIOHelper.WriteAllBytes(FullName, content);

        public DateTime LastWriteTime
        {
            get { return ZlpIOHelper.GetFileLastWriteTime(FullName); }
            set { ZlpIOHelper.SetFileLastWriteTime(FullName, value); }
        }

        public DateTime LastAccessTime
        {
            get { return ZlpIOHelper.GetFileLastAccessTime(FullName); }
            set { ZlpIOHelper.SetFileLastAccessTime(FullName, value); }
        }

        public DateTime CreationTime
        {
            get { return ZlpIOHelper.GetFileCreationTime(FullName); }
            set { ZlpIOHelper.SetFileCreationTime(FullName, value); }
        }

        public string FullName { get; }

        public string Name => ZlpPathHelper.GetFileNameFromFilePath(FullName);

        public string NameWithoutExtension => ZlpPathHelper.GetFileNameWithoutExtension(Name);

        public ZlpDirectoryInfo Directory => new ZlpDirectoryInfo(DirectoryName);

        public string DirectoryName => ZlpPathHelper.GetDirectoryPathNameFromFilePath(FullName);

        public string Extension => ZlpPathHelper.GetExtension(FullName);

        public long Length => ZlpIOHelper.GetFileLength(FullName);

        public FileAttributes Attributes
        {
            get { return ZlpIOHelper.GetFileAttributes(FullName); }
            set { ZlpIOHelper.SetFileAttributes(FullName, value); }
        }
    }
}