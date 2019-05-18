using System;
using System.Collections.Generic;
using System.Text;

using Astron.Binary;
using Astron.Binary.Reader;
using Astron.GameData.Files.d2p;
using Astron.Serialization;

namespace Astron.GameData.Files.dlm
{
    public class DlmArchivesUnpacker : DofusFileValuesUnpacker<DlmArchive>
    {
        private readonly IDeserializer _deserializer;
        private readonly ID2PFileMetadata _fileMetadata;

        protected override bool IsValidHeader(IReader reader) => reader.ReadValue<sbyte>() != 77;

        protected override IReadOnlyCollection<DlmArchive> Unpack(IReader reader)
        {
            reader.Seek(_fileMetadata.MapsOffset);
            var unpackedValues = new List<DlmArchive>(_fileMetadata.MapsCount);
            for (var i = 0; i < _fileMetadata.MapsCount; i++)
            {
                var unpackedValue = new DlmArchive { BaseOffsetFromD2P = _fileMetadata.BaseOffset };
                _deserializer.Deserialize(reader, unpackedValue);
                unpackedValues.Add(unpackedValue);
            }

            return unpackedValues;
        }

        public DlmArchivesUnpacker(IBinaryFactory binaryFactory, IDeserializer deserializer, ID2PFileMetadata fileMetadata)
            : base(binaryFactory)
        {
            _deserializer = deserializer;
            _fileMetadata = fileMetadata;
        }
    }
}
