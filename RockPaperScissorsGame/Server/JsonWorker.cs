using System;
using System.Collections.Concurrent;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Server
{
    public class JsonWorker
    {
        private readonly string _path;
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1,1);
        private readonly ILogger _logger;

        public JsonWorker(string path)
        {
            _path = path;
        }

        public async Task WriteAllAsync(ConcurrentBag<Account> accounts)
        {
            await _semaphore.WaitAsync();

            try
            {
                await using var stream = new MemoryStream();
                await JsonSerializer.SerializeAsync(stream, accounts, accounts.GetType(), new JsonSerializerOptions()
                {
                    WriteIndented = true
                });
                stream.Position = 0;
                using var reader = new  StreamReader(stream);
                var json = await reader.ReadToEndAsync();

                await File.WriteAllTextAsync(_path, json);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error occurred during writing to file");
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task<ConcurrentBag<Account>> ReadAllAsync()
        {
            await _semaphore.WaitAsync();

            try
            {
                var json = await File.ReadAllTextAsync(_path);

                await using var stream = new MemoryStream();
                return await JsonSerializer.DeserializeAsync<ConcurrentBag<Account>>(stream, new JsonSerializerOptions() { WriteIndented = true });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error occurred during reading from file");
            }
            finally
            {
                _semaphore.Release();
            }
            return new ConcurrentBag<Account>();
        }
    }
}