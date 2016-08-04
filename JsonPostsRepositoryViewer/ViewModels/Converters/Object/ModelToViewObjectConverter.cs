using JsonPostsRepositoryService.Models;
using System.Collections.Generic;

namespace JSONPlaceHolder.ViewModels
{
    public class ModelToViewObjectConverter
    {
        /// <summary>
        /// Create a ViewObject used for rendering from the model object
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static JsonPostViewObject Convert(JsonPostDetailModel model)
        {
            return GetJsonPostViewObject(model);
        }

        /// <summary>
        /// Creates a ViewObject list used for rendering from Model Objects
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static List<JsonPostViewObject> Convert(List<JsonPostDetailModel> models)
        {
            List<JsonPostViewObject> jsonPlaceHolderData = null;

            if (models != null && models.Count > 0)
            {
                jsonPlaceHolderData = new List<JsonPostViewObject>();

                models.ForEach(m =>
                    {
                        jsonPlaceHolderData.Add(GetJsonPostViewObject(m));
                    });
            }
            return jsonPlaceHolderData;
        }

        /// <summary>
        /// Returns JsonPostViewObject from a JsonPostDetailModel object
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private static JsonPostViewObject GetJsonPostViewObject(JsonPostDetailModel model)
        {
            return new JsonPostViewObject { Id = model.id, UserId = model.userId, Title = model.title, Body = model.body };
        }
    }
}
