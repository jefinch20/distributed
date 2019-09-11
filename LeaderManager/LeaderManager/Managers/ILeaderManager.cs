namespace LeaderManager.Managers
{
    public interface ILeaderManager
    {
        string GetCurrentLeaderId();
        bool TryGetOrRenewLeadership(string serverId);
    }
}
