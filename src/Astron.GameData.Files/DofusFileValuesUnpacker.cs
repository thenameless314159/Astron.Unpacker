using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Astron.Binary;
using Astron.Binary.Reader;
using Astron.Files;

namespace Astron.GameData.Files
{
    public abstract class DofusFileValuesUnpacker<TValue> : IFileValuesUnpacker<TValue>
    {
        protected readonly IBinaryFactory _binaryFactory;

        public IReadOnlyCollection<TValue> Values { get; private set; }

        protected abstract IReadOnlyCollection<TValue> Unpack(IReader reader);
        protected abstract bool IsValidHeader(IReader reader);

        protected DofusFileValuesUnpacker(IBinaryFactory binaryFactory) => _binaryFactory = binaryFactory;

        public void Unpack(IFileAccessor fileAccessor)
        {
            var reader = GetReader(fileAccessor);
            if (!IsValidHeader(reader)) throw new InvalidDataException("at :" + fileAccessor.FullPath);
            Values = Unpack(reader);
        }

        protected virtual IReader GetReader(IFileAccessor accessor) => _binaryFactory.Get(accessor.Data);
    }
}
