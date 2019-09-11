using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using DistributedProcess.Models;
using Microsoft.Extensions.Configuration;

namespace DistributedProcess.Clients
{
    internal class LeaderManagerClient : ILeaderManagerClient
    {
        private readonly HttpClient _client;


        private const string TryGetOrRenewLeadershipEndpoint = "api/leadership";

        public LeaderManagerClient(HttpClient httpClient)
        {
            _client = httpClient;
            _client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<bool> TryGetOrRenewLeadership(TryGetOrRenewLeadershipCommand command)
        {
            var response = await _client.PostAsJsonAsync(TryGetOrRenewLeadershipEndpoint, command);
            var leadershipResponse = await response.Content.ReadAsAsync<TryGetOrRenewLeadershipResponse>();
            return leadershipResponse.IsLeader;
        }
    }
}
