using System;
using System.Collections.Generic;
using System.Text;

namespace Astron.GameData.Files.dlm
{
    public interface IDlmArchive
    {
        string RelativePath   { get; }
        byte[] CompressedData { get; }
    }

    public class DlmArchive : IDlmArchive
    {
        public string RelativePath   { get; set; }
        public byte[] CompressedData { get; set; }

        public int BaseOffsetFromD2P { get; set; } // must be set after d2p file metadata parsing

        public DlmArchive()
        {
        }

        public DlmArchive(string relativePath, byte[] compressedData)
        {
            RelativePath   = relativePath;
            CompressedData = compressedData;
        }
    }
}
