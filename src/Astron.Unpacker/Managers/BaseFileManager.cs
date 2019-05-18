using System;
using System.Collections.Generic;
using System.Text;

using Astron.Binary;
using Astron.IoC;
using Astron.Serialization;

namespace Astron.Unpacker.Managers
{
    public abstract class BaseFileManager
    {
        protected readonly IBinaryFactory _binaryFactory;
        protected readonly ISerDes _serDes;

        protected BaseFileManager(IContainer container)
        {
            _binaryFactory = container.GetInstance<IBinaryFactory>();
            _serDes = container.GetInstance<ISerDes>();
        }
    }
}
