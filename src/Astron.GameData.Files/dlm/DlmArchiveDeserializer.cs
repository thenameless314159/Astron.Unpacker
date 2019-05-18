using System;
using System.Collections.Generic;
using System.Text;

using Astron.Serialization.Storage;

namespace Astron.GameData.Files.dlm
{
    public class DlmArchiveDeserializer : DeserializerStorage<DlmArchive>
    {
        public DlmArchiveDeserializer() : base((des, reader, pol, archive) =>
        {
            archive.RelativePath = reader.ReadValue<string>();
            var fileIndex           = reader.ReadValue<int>() + archive.BaseOffsetFromD2P;
            var fileSize            = reader.ReadValue<int>();
            var nextArchivePosition = reader.Position;

            reader.Seek(fileIndex + 2);
            archive.CompressedData = reader.GetSlice(fileSize).ToArray();
            reader.Seek(nextArchivePosition);
        })
        {
        }
    }
}
