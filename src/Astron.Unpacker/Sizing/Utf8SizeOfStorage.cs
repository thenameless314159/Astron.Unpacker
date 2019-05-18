using System;
using System.Collections.Generic;
using System.Text;

using Astron.Size;
using Astron.Size.Storage;

namespace Astron.Unpacker.Sizing
{
    public class Utf8SizeStorage : ISizeOfStorage<string>
    {
        public Func<ISizing, string, int> Calculate => (s, v) => 2 + v.Length;
    }
}
