using JsonPostsRepositoryService.Models;
namespace JSONPlaceHolder.ViewModels
{
    public class ViewObjectToModelConverter
    {
        /// <summary>
        /// Create a Model object from a View Object
        /// </summary>
        /// <param name="viewObject"></param>
        /// <returns></returns>
        public static JsonPostDetailModel Convert(JsonPostViewObject viewObject)
        {
            return GetJsonPostDetailModel(viewObject);
        }

        /// <summary>
        /// Returns JsonPostViewObject from a JsonPostDetailModel object
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private static JsonPostDetailModel GetJsonPostDetailModel(JsonPostViewObject viewObject)
        {
            return new JsonPostDetailModel { id = viewObject.Id, title = viewObject.Title, body = viewObject.Body, userId = viewObject.UserId };
        }
        
    }
}
