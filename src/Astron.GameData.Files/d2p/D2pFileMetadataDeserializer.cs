using System;
using System.Collections.Generic;
using System.Text;

using Astron.Binary.Reader;
using Astron.Memory;
using Astron.Serialization;
using Astron.Serialization.Storage;

namespace Astron.GameData.Files.d2p
{
    public class D2PFileMetadataDeserializer : DeserializerStorage<D2PFileMetadata>
    {
        public D2PFileMetadataDeserializer() : base((des, reader, pol, file) =>
        {
            file.BaseOffset = (int)reader.ReadValue<uint>();
            reader.Advance(4); // skip useless "len" field
            file.MapsOffset     = (int)reader.ReadValue<uint>();
            file.MapsCount      = (int)reader.ReadValue<uint>();
            file.DlmFilesOffset = (int)reader.ReadValue<uint>();
            file.DlmFilesLength = (int)reader.ReadValue<uint>();
        })
        {
        }
    }
}
