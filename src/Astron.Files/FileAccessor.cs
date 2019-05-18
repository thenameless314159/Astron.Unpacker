using System;
using System.IO;

namespace Astron.Files
{
    public class FileAccessor : IFileAccessor
    {
        public string Name { get; } 
        public string FileName { get; } 
        public string FullPath { get; }
        public ReadOnlyMemory<byte> Data { get; }

        public FileAccessor(string filePath)
        {
            if (!File.Exists(filePath)) throw new FileNotFoundException(filePath);

            var fileData = File.ReadAllBytes(filePath);
            if (fileData.Length < 1) throw new InvalidOperationException($"File is empty : {filePath}");

            Name = Path.GetFileNameWithoutExtension(filePath);
            FileName = Path.GetFileName(filePath);
            FullPath = filePath;
            Data = fileData;
        }
    }
}
