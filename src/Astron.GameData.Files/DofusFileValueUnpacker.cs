using System;
using System.Diagnostics;
using System.IO;

using Astron.Binary;
using Astron.Binary.Reader;
using Astron.Files;

namespace Astron.GameData.Files
{
    public abstract class DofusFileValueUnpacker<TValue> : IFileValueUnpacker<TValue>
    {
        protected readonly IBinaryFactory _binaryFactory;

        public TValue Value { get; private set; }

        protected abstract TValue Unpack(IReader reader);
        protected abstract bool IsValidHeader(IReader reader);

        protected DofusFileValueUnpacker(IBinaryFactory binaryFactory) => _binaryFactory = binaryFactory;

        public void Unpack(IFileAccessor fileAccessor)
        {
            var reader = GetReader(fileAccessor);
            if (!IsValidHeader(reader)) throw new InvalidDataException("at :" + fileAccessor.FullPath);
            Value = Unpack(reader);
        }

        protected virtual IReader GetReader(IFileAccessor accessor) => _binaryFactory.Get(accessor.Data);
    }
}
