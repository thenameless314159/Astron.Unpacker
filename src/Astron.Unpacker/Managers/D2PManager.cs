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

        public async Task UnpackAll(string[] filesPath)
        {
            _logger.Log<D2PManager>(LogLevel.Info, $"Attempting to unpack {filesPath.Length} d2p files...");

            var tasks = new List<Task>(filesPath.Length);
            tasks.AddRange(filesPath.Select(d2PFilePath => UnpackD2PFile(d2PFilePath)));

            await Task.WhenAll(tasks).ConfigureAwait(false);
        }

        public async Task UnpackD2PFile(string path)
        {
            var d2PFile      = new FileAccessor(path);
            var metaUnpacker = new D2PFileMetadataUnpacker(_binaryFactory, _serDes);
            metaUnpacker.Unpack(d2PFile);

            var archiveUnpacker = new DlmArchivesUnpacker(_binaryFactory, _serDes, metaUnpacker.Value);
            archiveUnpacker.Unpack(d2PFile);

            var progressCount = 1;
            var progressBar   = new ProgressBar(PbStyle.SingleLine, archiveUnpacker.Values.Count);
            progressBar.Refresh(0, Path.GetFileName(d2PFile.FullPath));
            await Task.Delay(10); // doesn't print all either way
            foreach (var archive in archiveUnpacker.Values)
            {
                var filePath      = (_dlmFilesFolder + archive.RelativePath).Replace('/', '\\');
                var fileDirectory = Path.GetDirectoryName(filePath);
                var deflatedStream =
                    new DeflateStream(new MemoryStream(archive.CompressedData), CompressionMode.Decompress);

                var decompressedData = new MemoryStream();
                await deflatedStream.CopyToAsync(decompressedData);
                if (!Directory.Exists(fileDirectory)) Directory.CreateDirectory(fileDirectory);

                File.WriteAllBytes(filePath, decompressedData.GetBuffer());
                progressBar.Refresh(progressCount, filePath);
                progressCount++;
            }
        }
    }
}
