
namespace JSONPlaceHolder.ViewModels
{
    /// <summary>
    /// Class used as a View Object
    /// </summary>
    public class JsonPostViewObject
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public int UserId { get; set; }

        public override string ToString()
        {
            return Id + "," + Title + "," + Body + "," + UserId;
        }
    }
}
