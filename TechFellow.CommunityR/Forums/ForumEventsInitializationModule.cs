using EPiServer.Common;
using EPiServer.Community.Club;
using EPiServer.Community.Forum;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using TechFellow.CommunityR.Forums.Models;
using InitializationModule = EPiServer.Web.InitializationModule;

namespace TechFellow.CommunityR.Forums
{
    [ModuleDependency(typeof(InitializationModule))]
    public class ForumEventsInitializationModule : IInitializableModule
    {
        public void Initialize(InitializationEngine context)
        {
            // forum
            ForumHandler.Instance.ForumAdded += OnForumAdded;
            ForumHandler.Instance.ForumRemoved += OnForumRemoved;

            // room
            ForumHandler.Instance.RoomAdded += OnRoomAdded;
            ForumHandler.Instance.RoomRemoved += OnRoomRemoved;

            // topic
            ForumHandler.Instance.TopicAdded += OnTopicAdded;
            ForumHandler.Instance.TopicMoved += OnTopicMoved;
            ForumHandler.Instance.TopicRemoved += OnTopicRemoved;
            ForumHandler.Instance.TopicUpdated += OnTopicUpdated;

            // reply
            ForumHandler.Instance.ReplyAdded += OnReplyAdded;
            ForumHandler.Instance.ReplyRemoved += OnReplyRemoved;
            ForumHandler.Instance.ReplyUpdated += OnReplyUpdated;
        }

        public void Uninitialize(InitializationEngine context)
        {
            // forum
            ForumHandler.Instance.ForumAdded -= OnForumAdded;
            ForumHandler.Instance.ForumRemoved -= OnForumRemoved;

            // room
            ForumHandler.Instance.RoomAdded -= OnRoomAdded;
            ForumHandler.Instance.RoomRemoved -= OnRoomRemoved;

            // topic
            ForumHandler.Instance.TopicAdded -= OnTopicAdded;
            ForumHandler.Instance.TopicMoved -= OnTopicMoved;
            ForumHandler.Instance.TopicRemoved -= OnTopicRemoved;
            ForumHandler.Instance.TopicUpdated += OnTopicUpdated;

            // reply
            ForumHandler.Instance.ReplyAdded -= OnReplyAdded;
            ForumHandler.Instance.ReplyRemoved -= OnReplyRemoved;
            ForumHandler.Instance.ReplyUpdated += OnReplyUpdated;
        }

        public void Preload(string[] parameters)
        {
        }

        private void OnForumAdded(string sender, EPiServerCommonEventArgs args)
        {
            var forum = (Forum)args.Object;
            SendMessageToAll<ForumHub>("onForumAdded", new OnForumAddedMessage { Id = forum.ID, Name = forum.Name });
        }

        private void OnForumRemoved(string sender, EPiServerCommonEventArgs args)
        {
            var forum = (Forum)args.Object;
            SendMessageToAll<ForumHub>("onForumRemoved", new OnForumRemovedMessage { Id = forum.ID, Name = forum.Name });
        }

        private void OnReplyAdded(string sender, EPiServerCommonEventArgs args)
        {
            var reply = (Reply)args.Object;
            var message = new OnReplyAddedMessage
                          {
                                  Id = reply.ID,
                                  Name = reply.Header,
                                  AuthorId = reply.Author.ID,
                                  Author = reply.Author.Name,
                                  TopicId = reply.Topic.ID,
                                  TopicAuthorId = reply.Topic.Author.ID,
                          };

            var club = reply.Topic.Room.OwnedBy.Entity as Club;
            if (club == null)
            {
                // publish event to all clients
                SendMessageToAll<ForumHub>("onReplyAdded", message);
            }
            else
            {
                // publish event to club context
                message.ClubId = club.ID;
                SendMessageToGroup<ForumHub>(ForumHub.CommunityClubGroupName + club.ID, "onReplyAddedInClub", message);
            }
        }

        private void OnReplyRemoved(string sender, EPiServerCommonEventArgs args)
        {
            var reply = (Reply)args.Object;
            var message = new OnReplyRemovedMessage
                          {
                                  Id = reply.ID,
                                  Name = reply.Header,
                                  TopicId = reply.Topic.ID,
                                  TopicAuthorId = reply.Topic.Author.ID,
                          };

            var club = reply.Topic.Room.OwnedBy.Entity as Club;
            if (club == null)
            {
                // publish event to all clients
                SendMessageToAll<ForumHub>("onReplyRemoved", message);
            }
            else
            {
                // publish event to club context
                message.ClubId = club.ID;
                SendMessageToGroup<ForumHub>(ForumHub.CommunityClubGroupName + club.ID, "onReplyRemovedInClub", message);
            }
        }

        private void OnReplyUpdated(string sender, EPiServerCommonEventArgs args)
        {
            var reply = (Reply)args.Object;
            var message = new OnReplyUpdatedMessage
                          {
                                  Id = reply.ID,
                                  Name = reply.Header,
                                  TopicId = reply.Topic.ID,
                                  TopicAuthorId = reply.Topic.Author.ID,
                          };

            var club = reply.Topic.Room.OwnedBy.Entity as Club;
            if (club == null)
            {
                // publish event to all clients
                SendMessageToAll<ForumHub>("onReplyUpdated", message);
            }
            else
            {
                // publish event to club context
                message.ClubId = club.ID;
                SendMessageToGroup<ForumHub>(ForumHub.CommunityClubGroupName + club.ID, "onReplyUpdatedInClub", message);
            }
        }

        private void OnRoomAdded(string sender, EPiServerCommonEventArgs args)
        {
        }

        private void OnRoomRemoved(string sender, EPiServerCommonEventArgs args)
        {
        }

        private void OnTopicAdded(string sender, EPiServerCommonEventArgs args)
        {
            var topic = (Topic)args.Object;
            var message = new OnTopicAddedMessage
                          {
                                  Name = topic.Header,
                                  Added = topic.Created,
                                  Tags = topic.Tags.ToArray(),
                                  AuthorId = topic.Author.ID,
                                  Author = topic.Author.Name,
                                  RoomId = topic.Room.ID,
                                  RoomName = topic.Room.Header,
                                  ForumId = topic.Room.Forum.ID,
                                  ForumName = topic.Room.Forum.Name,
                          };

            var club = topic.Room.OwnedBy.Entity as Club;
            if (club == null)
            {
                SendMessageToAll<ForumHub>("onTopicAdded", message);
            }
            else
            {
                // publish event to club context
                message.ClubId = club.ID;
                SendMessageToGroup<ForumHub>(ForumHub.CommunityClubGroupName + club.ID, "onTopicAddedInClub", message);
            }
        }

        private void OnTopicMoved(string sender, EPiServerCommonEventArgs args)
        {
            var topic = (Topic)args.Object;
            var message = new OnTopicMovedMessage
                          {
                                  Id = topic.ID,
                                  Name = topic.Header,
                          };

            var club = topic.Room.OwnedBy.Entity as Club;
            if (club == null)
            {
                SendMessageToAll<ForumHub>("onTopicMoved", message);
            }
            else
            {
                // publish event to club context
                message.ClubId = club.ID;
                SendMessageToGroup<ForumHub>(ForumHub.CommunityClubGroupName + club.ID, "onTopicMovedInClub", message);
            }
        }

        private void OnTopicRemoved(string sender, EPiServerCommonEventArgs args)
        {
            var topic = (Topic)args.Object;
            var message = new OnTopicRemovedMessage
                          {
                                  Id = topic.ID,
                                  Name = topic.Header,
                          };

            var club = topic.Room.OwnedBy.Entity as Club;
            if (club == null)
            {
                SendMessageToAll<ForumHub>("onTopicRemoved", message);
            }
            else
            {
                // publish event to club context
                message.ClubId = club.ID;
                SendMessageToGroup<ForumHub>(ForumHub.CommunityClubGroupName + club.ID, "onTopicRemovedInClub", message);
            }
        }

        private void OnTopicUpdated(string sender, EPiServerCommonEventArgs args)
        {
            var topic = (Topic)args.Object;
            var message = new OnTopicUpdatedMessage
                          {
                                  Id = topic.ID,
                                  Name = topic.Header,
                          };

            var club = topic.Room.OwnedBy.Entity as Club;
            if (club == null)
            {
                // publish event to all clients
                SendMessageToAll<ForumHub>("onTopicUpdated", message);
            }
            else
            {
                // publish event to club context
                message.ClubId = club.ID;
                SendMessageToGroup<ForumHub>(ForumHub.CommunityClubGroupName + club.ID, "onTopicUpdatedInClub", message);
            }
        }

        private void SendMessageToAll<THub>(string method, params object[] args) where THub : IHub
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<THub>();
            IClientProxy proxy = hubContext.Clients.All;
            proxy.Invoke(method, args);
        }

        private void SendMessageToGroup<THub>(string groupName, string method, params object[] args) where THub : IHub
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<THub>();
            IClientProxy proxy = hubContext.Clients.Group(groupName);
            proxy.Invoke(method, args);
        }
    }
}
