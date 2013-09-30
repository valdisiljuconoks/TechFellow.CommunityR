namespace TechFellow.CommunityR.Forums.Models
{
    public class OnReplyAddedMessage : OnReplyMessageBase
    {
        public int AuthorId { get; set; }
        public string Author { get; set; }
    }
}
