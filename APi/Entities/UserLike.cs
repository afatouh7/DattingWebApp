namespace APi.Entities
{
    public class UserLike
    {
        public AppUser SorceUser { get; set; }
        public int SorceUserId { get; set; }

        public AppUser LikeUser { get; set; }
        public int LikeUserId { get; set; }
    }
}
