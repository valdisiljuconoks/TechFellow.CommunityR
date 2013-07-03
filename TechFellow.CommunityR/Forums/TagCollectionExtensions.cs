using System.Linq;
using EPiServer.Common.Tags;

namespace TechFellow.CommunityR.Forums
{
    public static class TagCollectionExtensions
    {
        public static Tag[] ToArray(this TagCollection collection)
        {
            return collection.Select(t => new Tag { Name = t.Name }).ToArray();
        }
    }
}
