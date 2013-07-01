using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;

namespace TechFellow.CommunityR
{
    public class ForumHub : Hub
    {
        internal const string CommunityClubGroupName = "community-club";

        public Task JoinClub(string id)
        {
            return Groups.Add(Context.ConnectionId, CommunityClubGroupName + id);
        }

        public Task LeaveClub(string id)
        {
            return Groups.Remove(Context.ConnectionId, CommunityClubGroupName + id);
        }
    }
}
