using JsonPostsRepositoryService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonPostsRepositoryService
{
    /// <summary>
    /// Service interface
    /// </summary>
    public interface IJsonPostsRepositoryService
    {
        List<JsonPostDetailModel> GetPosts();
        JsonPostDetailModel GetPost(int id);
        string ParseModelData(JsonPostDetailModel model, CopyMode mode);
    }
}
