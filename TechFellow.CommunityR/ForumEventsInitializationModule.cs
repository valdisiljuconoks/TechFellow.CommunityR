using System;
using EPiServer.Common;
using EPiServer.Community.Club;
using EPiServer.Community.Forum;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using Microsoft.AspNet.SignalR;
using InitializationModule = EPiServer.Web.InitializationModule;

namespace TechFellow.CommunityR
{
    [ModuleDependency(typeof(InitializationModule))]
    public class ForumEventsInitializationModule : IInitializableModule
    {
        public void Initialize(InitializationEngine context)
        {
            ForumHandler.Instance.TopicAdded += OnTopicAdded;
        }

        public void Uninitialize(InitializationEngine context)
        {
            ForumHandler.Instance.TopicAdded -= OnTopicAdded;
        }

        public void Preload(string[] parameters)
        {
        }

        private void OnTopicAdded(string sender, EPiServerCommonEventArgs args)
        {
            var topic = (Topic)args.Object;
            var clubId = ((Club)topic.Room.OwnedBy.Entity).ID;

            var hubContext = GlobalHost.ConnectionManager.GetHubContext<ForumHub>();
            
            // publish event to all clients
            hubContext.Clients.All.onTopicAdded(new OnTopicAddedMessage
                                                {
                                                    Topic = topic.Header,
                                                    Added = topic.Created,
                                                    Author = topic.Author.Name,
                                                    RoomId = topic.Room.ID,
                                                    RoomName = topic.Room.Header,
                                                    ForumId = topic.Room.Forum.ID,
                                                    ForumName = topic.Room.Forum.Name,
                                                });

            // publish event to club context
            hubContext.Clients.Group(ForumHub.CommunityClubGroupName + clubId).onTopicAddedInClub();
        }
    }

    public class OnTopicAddedMessage
    {
        public string Author { get; set; }
        public string Topic { get; set; }
        public DateTime Added { get; set; }
        public string RoomName { get; set; }
        public string ForumName { get; set; }
        public int RoomId { get; set; }
        public int ForumId { get; set; }
    }
}
