using System;
using System.Collections.Generic;
using System.Text;

using Astron.Binary;
using Astron.Binary.Reader;
using Astron.Serialization;

namespace Astron.GameData.Files.d2p
{
    public class D2PFileMetadataUnpacker : DofusFileValueUnpacker<D2PFileMetadata>
    {
        private readonly IDeserializer _deserializer;

        protected override bool IsValidHeader(IReader reader) => reader.ReadValue<sbyte>() != 77;

        protected override D2PFileMetadata Unpack(IReader reader)
        {
            reader.Seek(reader.Count - 24); // go to file metadata
            return _deserializer.Deserialize<D2PFileMetadata>(reader);
        }

        public D2PFileMetadataUnpacker(IBinaryFactory binaryFactory, IDeserializer deserializer) : base(binaryFactory)
        {
            _deserializer = deserializer;
        }
    }
}
