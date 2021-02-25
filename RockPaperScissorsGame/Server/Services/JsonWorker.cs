using System;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Server.Services
{
    public class JsonWorker<T> where T: new()
    {
        private readonly string _path;
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1,1);

        public JsonWorker(string path)
        {
            _path = path;
        }

        public async Task WriteAllAsync(T accounts)
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
                //_logger.LogError(e, "Error occurred during writing to file");
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task<T> ReadAllAsync()
        {
            await _semaphore.WaitAsync();

            try
            {
                await using var stream = File.OpenRead(_path);

                return await JsonSerializer.DeserializeAsync<T>(stream, new JsonSerializerOptions()
                {
                    WriteIndented = true
                });
            }
            catch (Exception e)
            {
                //_logger.LogError(e, "Error occurred during reading from file");
            }
            finally
            {
                _semaphore.Release();
            }

            return new T();
        }
    }
}