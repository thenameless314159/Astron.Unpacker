using System;
using System.Collections.Generic;
using System.Text;

namespace Astron.GameData.Files.d2p
{
    public interface ID2PFileMetadata
    {
        int BaseOffset     { get; }
        int MapsOffset     { get; }
        int MapsCount      { get; }
        int DlmFilesOffset { get; }
        int DlmFilesLength { get; }
    }

    public class D2PFileMetadata : ID2PFileMetadata
    {
        public int BaseOffset     { get; set; }
        public int MapsOffset     { get; set; }
        public int MapsCount      { get; set; }
        public int DlmFilesOffset { get; set; }
        public int DlmFilesLength { get; set; }

        public D2PFileMetadata()
        {
        }

        public D2PFileMetadata(int baseOffset, int mapsOffset, int mapsCount, int dlmFilesOffset,
                               int dlmFilesLength)
        {
            BaseOffset     = baseOffset;
            MapsOffset     = mapsOffset;
            MapsCount      = mapsCount;
            DlmFilesOffset = dlmFilesOffset;
            DlmFilesLength = dlmFilesLength;
        }
    }
}
