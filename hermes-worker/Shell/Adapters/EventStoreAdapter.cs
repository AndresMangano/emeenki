using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Hermes.Worker.Core.Ports;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Hermes.Worker.Shell
{
    public partial class Interpreter : IEventStore
    {
        public async Task<IEnumerable<EventDTO>> GetMissingEvents(string stream, long index)
        {
            var url = $"{_settings.EventStore}/{stream}/{index}";
            _logger.LogInformation("Request to {url}", url);
            var client = new HttpClient();
            var response = await client.GetAsync(url);

            if (response.StatusCode.Equals(HttpStatusCode.OK)) {
                var result = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<IEnumerable<EventDTO>>(result);
            }
            else {
                throw new Exception($"Failed to request {url} with {response.StatusCode}");
            }
        }
    }
}