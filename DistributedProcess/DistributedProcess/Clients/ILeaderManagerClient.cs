using System.Threading.Tasks;
using DistributedProcess.Models;

namespace DistributedProcess.Clients
{
    public interface ILeaderManagerClient
    {
        Task<bool> TryGetOrRenewLeadership(TryGetOrRenewLeadershipCommand command);
    }
}
