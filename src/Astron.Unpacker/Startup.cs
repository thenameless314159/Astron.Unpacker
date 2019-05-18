using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

using Astron.Binary;
using Astron.Files;
using Astron.GameData.Files.d2p;
using Astron.GameData.Files.dlm;
using Astron.IoC;
using Astron.Logging;
using Astron.Memory;
using Astron.Serialization;
using Astron.Size;
using Astron.Unpacker.Binary;
using Astron.Unpacker.Logging;
using Astron.Unpacker.Sizing;

using Newtonsoft.Json;

namespace Astron.Unpacker
{
    public class Startup
    {
        public static IContainer ConfigureServices(IContainerBuilder builder)
        {
            var sw = Stopwatch.StartNew();

            var policy = new HeapAllocationPolicy();
            var sizing = new SizingBuilder().Register(new Utf8SizeStorage()).Build();
            var binaryFactory = new BinaryBuilder(sizing, policy, Endianness.BigEndian)
                .Register(new Utf8BinaryStorage())
                .Build();

            var serDes = new SerDesBuilder(policy)
                .Register(new D2PFileMetadataDeserializer())
                .Register(new DlmArchiveDeserializer())
                .Build();

            builder
                .Register(sizing)
                .Register(serDes)
                .Register(binaryFactory)
                .Register<ISerializer>(serDes)
                .Register<IDeserializer>(serDes)
                .Register<IMemoryPolicy>(policy);

            var container = builder.Build();
            ServiceLocator.Container = container;
            ServiceLocator.GetLoggerOf<Startup>().Log(LogLevel.Info, $"IoC container successfully configured in {sw.ElapsedMilliseconds}ms.");
            return container;
        }
    }
}
