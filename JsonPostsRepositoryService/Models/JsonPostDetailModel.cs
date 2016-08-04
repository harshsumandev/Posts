using System;

namespace JsonPostsRepositoryService.Models
{
    
    /// <summary>
    /// repository for model data from the service
    /// </summary>
    [Serializable]
    public class JsonPostDetailModel
    {
        public int userId { get; set; }
        public int id { get; set; }
        public string title { get; set; }
        public string body { get; set; }

        public override string ToString()
        {
            return "id: " + id + ",\ntitle: " + title + ",\nuserId: " + userId + ",\nbody: " + body;
        }

    }
}
