using System;
using System.Collections.Generic;
using System.Text;

namespace Astron.Files
{
    public interface IFileValuesUnpacker<out TValue>
    {
        IReadOnlyCollection<TValue> Values { get; }
        void Unpack(IFileAccessor fileAccessor);
    }
}
