using System;

namespace Astron.Files
{
    public interface IFileAccessor
    {
        /// <summary>
        /// Get the name of the file without its extension.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Get the name of the file with its extension.
        /// </summary>
        string FileName { get; }

        /// <summary>
        /// Get the full path of the file.
        /// </summary>
        string FullPath { get; }

        /// <summary>
        /// Get the data of the currently accessed file.
        /// </summary>
        ReadOnlyMemory<byte> Data { get; }
    }
}
