using System;
using System.Collections.Concurrent;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TPW.Data
{
    public class DiagnosticLogger
    {
        private readonly BlockingCollection<string> _logQueue = new(new ConcurrentQueue<string>());
        private readonly string _logFilePath;
        private readonly CancellationTokenSource _cts = new();

        public DiagnosticLogger(string logFilePath)
        {
            _logFilePath = logFilePath;
            Task.Factory.StartNew(ProcessQueue, TaskCreationOptions.LongRunning);
        }

        public void Log(string message)
        {
            string timestamped = $"{DateTime.UtcNow:O} - {message}";
            _logQueue.Add(timestamped);
        }

        private async Task ProcessQueue()
        {
            try
            {
                using var writer = new StreamWriter(_logFilePath, append: true, Encoding.ASCII);
                while (!_cts.IsCancellationRequested)
                {
                    if (_logQueue.TryTake(out var log, Timeout.Infinite, _cts.Token))
                    {
                        await writer.WriteLineAsync(log);
                        await writer.FlushAsync();
                    }
                }
            }
            catch (OperationCanceledException)
            {
                // Normal during shutdown
            }
        }

        public void Shutdown()
        {
            _cts.Cancel();
        }
    }
}
