using System;

namespace TechFellow.CommunityR.Forums
{
    public class OnTopicAddedMessage : OnTopicMessageBase
    {
        public int AuthorId { get; set; }
        public string Author { get; set; }
        public DateTime Added { get; set; }
        public string RoomName { get; set; }
        public string ForumName { get; set; }
        public int RoomId { get; set; }
        public int ForumId { get; set; }
        public Tag[] Tags { get; set; }
    }
}
