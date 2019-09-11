using Microsoft.Extensions.Configuration;
using System;

namespace LeaderManager.Managers
{
    internal class LeaderManager : ILeaderManager
    {
        private static readonly object _lock = new object();
        private static string _leaderServerId = null;
        private static DateTime _leaderExpirationTime = DateTime.MinValue;

        private readonly int _leadershipLengthSeconds;

        private const string LeadershipLengthSettingKey = "LeadershipLengthSeconds";

        public LeaderManager(IConfiguration configuration)
        {
            _leadershipLengthSeconds = int.Parse(configuration[LeadershipLengthSettingKey]);
        }

        public string GetCurrentLeaderId()
        {
            return _leaderServerId;
        }

        public bool TryGetOrRenewLeadership(string serverId)
        {
            try
            {
                lock (_lock)
                {
                    return TryRenewLeadership(serverId) ? true : TryGetLeadership(serverId);
                }
            }
            catch (Exception ex)
            {
                // Assume not the leader if an exception occurred. With current implementation
                // this seems unlikely, but in the future, may want to make the LeaderManager itself
                // distributed and use Redis or some other caching to store the current leader data
                Console.WriteLine($"Error in TryGetOrRenewLeadership: {ex.Message}");
                return false;
            }
        }

        private bool TryRenewLeadership(string serverId)
        {
            if (!string.Equals(serverId, _leaderServerId, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            UpdateExpirationTime();
            return true;
        }

        private bool TryGetLeadership(string serverId)
        {
            if (_leaderExpirationTime > DateTime.UtcNow)
            {
                return false;
            }

            _leaderServerId = serverId;
            UpdateExpirationTime();
            return true;
        }

        private void UpdateExpirationTime()
        {
            _leaderExpirationTime = DateTime.UtcNow.AddSeconds(_leadershipLengthSeconds);
        }
    }
}
