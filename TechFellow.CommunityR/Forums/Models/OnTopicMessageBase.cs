namespace TechFellow.CommunityR.Forums
{
    public abstract class OnTopicMessageBase : ClubContextMessage
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
