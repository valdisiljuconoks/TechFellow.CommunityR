namespace TechFellow.CommunityR.Forums.Models
{
    public abstract class OnTopicMessageBase : ClubContextMessage
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
