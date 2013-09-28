namespace TechFellow.CommunityR.Forums
{
    public abstract class OnReplyMessageBase : ClubContextMessage
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int TopicId { get; set; }
        public int TopicAuthorId { get; set; }
    }
}
