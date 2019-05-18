using System;
using System.Collections.Generic;
using System.Text;

using Astron.Binary.Reader;
using Astron.Binary.Storage;
using Astron.Binary.Writer;

namespace Astron.Unpacker.Binary
{
    public class Utf8BinaryStorage : IBinaryStorage<string>
    {
        public Func<IReader, string> ReadValue => reader =>
        {
            var length = reader.ReadValue<short>();

            if (length < 1) return string.Empty;
            var encodedStr = reader.GetSlice(length);
            reader.Advance(length);
            return Encoding.UTF8.GetString(encodedStr.Span);
        };

        public Action<IWriter, string> WriteValue => (writer, value) =>
        {
            if (value == string.Empty) return;

            var encodedStr = Encoding.UTF8.GetBytes(value);
            writer.WriteValue(encodedStr.Length);
            writer.WriteValues(encodedStr);
        };
    }
}
