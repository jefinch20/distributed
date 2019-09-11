using DistributedProcess.Clients;
using DistributedProcess.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading;

namespace DistributedProcess.Managers
{
    internal class MasterManager : IMasterManager, IDisposable
    {
        private static readonly Guid _serverId = Guid.NewGuid();
        private static bool _isLeader = false;

        private readonly Timer _refreshTimer;
        private readonly ILeaderManagerClient _leaderManagerClient;

        private const string LeadershipRefreshPeriodSettingKey = "LeadershipRefreshPeriodSeconds";

        public MasterManager(IConfiguration configuration, ILeaderManagerClient leaderManagerClient)
        {
            _leaderManagerClient = leaderManagerClient;

            var refreshPeriodSeconds = int.Parse(configuration[LeadershipRefreshPeriodSettingKey]);
            _refreshTimer = new Timer(TryGetRefreshLeadership, null, TimeSpan.Zero, TimeSpan.FromSeconds(refreshPeriodSeconds));
        }

        public void Dispose()
        {
            _refreshTimer.Dispose();
        }

        public string GetMasterStatus()
        {
            return _isLeader ? $"I am server {_serverId}" : $"I, {_serverId}, am not the master";
        }

        private void TryGetRefreshLeadership(object o)
        {
            try
            {
                _isLeader = _leaderManagerClient.TryGetOrRenewLeadership(new TryGetOrRenewLeadershipCommand
                {
                    ServerId = _serverId.ToString()
                }).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error trying to get or renew leadership: {ex.Message}");
                _isLeader = false;
            }
        }
    }
}
