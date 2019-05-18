using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Astron.Logging;
using Astron.Logging.Strategy;

namespace Astron.Unpacker.Logging
{
    public class FileStrategy : LoggingStrategy
    {
        private readonly object                  _lock = new object();
        private readonly ConcurrentQueue<string> _logs = new ConcurrentQueue<string>();

        public string LogPath { get; }

        public FileStrategy(string path, LogLevel minLevelAutoSave = LogLevel.Fatal)
            : base(new LogConfig(true, LogLevel.Trace, minLevelAutoSave)) => LogPath = path;

        protected override void Log(LogLevel level, string formattedHeader, string message)
            => _logs.Enqueue($"{formattedHeader} {message}");

        protected override void Log<T>(LogLevel level, string formattedHeader, string message, T instance)
        {
        }

        protected override void OnMinLevel(LogLevel level, string formattedHeader, string message)
        {
        }

        protected override void OnMaxLevel(LogLevel level, string formattedHeader, string message)
        {
            if (level == LogLevel.None)
            {
                _logs.Enqueue(""); // empty line
                return;
            }

            _logs.Enqueue($"{formattedHeader} {message}"); // log anyway
            Save();                                        // force save
        }

        public override void Save()
        {
            if (LogPath == null)
                throw new ArgumentNullException($"You must specify {nameof(Log)} using {nameof(FileStrategy)}.");

            lock (_lock)
            {
                if (!Directory.Exists(LogPath)) Directory.CreateDirectory(Path.GetDirectoryName(LogPath));

                using var file   = new FileStream(LogPath, FileMode.Append, FileAccess.Write, FileShare.Read);
                using var writer = new StreamWriter(file, Encoding.UTF8);
                while (_logs.TryDequeue(out var log)) writer.WriteLine(log);
            }
        }
    }
}
