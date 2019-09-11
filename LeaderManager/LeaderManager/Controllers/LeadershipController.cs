using LeaderManager.Managers;
using LeaderManager.Models;
using Microsoft.AspNetCore.Mvc;

namespace LeaderManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeadershipController : ControllerBase
    {
        private readonly ILeaderManager _leaderManager;

        public LeadershipController(ILeaderManager leaderManager)
        {
            _leaderManager = leaderManager;
        }

        [HttpGet]
        public string Get()
        {
            var leaderId = _leaderManager.GetCurrentLeaderId();
            return leaderId != null
                ? $"The current leader is {leaderId}"
                : "No leader exists";
        }

        [HttpPost]
        public TryGetOrRenewLeadershipResponse Post([FromBody] TryGetOrRenewLeadershipCommand command)
        {
            var isLeader = _leaderManager.TryGetOrRenewLeadership(command.ServerId);
            return new TryGetOrRenewLeadershipResponse
            {
                IsLeader = isLeader
            };
        }
    }
}
