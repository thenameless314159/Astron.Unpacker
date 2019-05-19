using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Astron.Files;
using Astron.GameData.Files.d2p;
using Astron.GameData.Files.dlm;
using Astron.IoC;
using Astron.Logging;

using Konsole;

namespace Astron.Unpacker.Managers
{
    // todo: debug & trace logging
    public class D2PManager : BaseFileManager
    {
        private readonly ILogger _logger;
        private readonly string _dlmFilesFolder;
        public D2PManager(IContainer container) : base(container)
        {
            _logger = ServiceLocator.Logger;
            _dlmFilesFolder = container.GetInstance<Settings>().DlmOutputFolder;
        }

        public void UnpackAll(string[] filesPath)
        {
            _logger.Log<D2PManager>(LogLevel.Info, $"Attempting to unpack {filesPath.Length} d2p files...");

            Parallel.ForEach(filesPath, path =>
            {
                var d2PFile      = new FileAccessor(path);
                var d2pFileName = Path.GetFileName(d2PFile.FullPath);
                var metaUnpacker = new D2PFileMetadataUnpacker(_binaryFactory, _serDes);
                metaUnpacker.Unpack(d2PFile);

                var archiveUnpacker = new DlmArchivesUnpacker(_binaryFactory, _serDes, metaUnpacker.Value);
                archiveUnpacker.Unpack(d2PFile);

                var progressCount = 1;
                var progressBar   = new ProgressBar(PbStyle.SingleLine, archiveUnpacker.Values.Count);
                progressBar.Refresh(0, d2pFileName);
                foreach (var archive in archiveUnpacker.Values)
                {
                    var       filePath         = (_dlmFilesFolder + archive.RelativePath).Replace('/', '\\');
                    var       fileDirectory    = Path.GetDirectoryName(filePath);
                    using var decompressedData = new MemoryStream();
                    using var deflatedStream = new DeflateStream(new MemoryStream(archive.CompressedData),
                        CompressionMode.Decompress);

                    deflatedStream.CopyTo(decompressedData);
                    if (!Directory.Exists(fileDirectory)) Directory.CreateDirectory(fileDirectory);

                    File.WriteAllBytes(filePath, decompressedData.GetBuffer());
                    progressBar.Refresh(progressCount, filePath);
                    if(progressCount < progressBar.Max) progressCount++;
                }
                progressBar.Refresh(progressCount, d2pFileName + " done !");
            });
        }
    }
}
