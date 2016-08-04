using JsonPostsRepositoryService.Models;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Web.Script.Serialization;
using System.Xml;
using System.Xml.Serialization;

namespace JsonPostsRepositoryService
{
    /// <summary>
    /// Class to mock service, this makes a webrequest to the server
    /// </summary>
    public class JsonPostsRepositoryService : IJsonPostsRepositoryService
    {
        /// <summary>
        /// Fetches the posts from http://jsonplaceholder.typicode.com/posts and deserializes them to a Model object
        /// </summary>
        /// <returns></returns>
        List<JsonPostDetailModel> IJsonPostsRepositoryService.GetPosts()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Constants.POSTSURL);

            List<JsonPostDetailModel> jsonPlaceHolders = null;
             
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    string html = reader.ReadToEnd();
                    //This deserializes the data from http://jsonplaceholder.typicode.com/posts to JsonPlaceHolderData
                    JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
                    jsonPlaceHolders = new List<JsonPostDetailModel>();
                    jsonPlaceHolders = jsonSerializer.Deserialize<List<JsonPostDetailModel>>(html);
                }
            }

            return jsonPlaceHolders;
        }

        /// <summary>
        /// Fetches the relavant JSON Post from http://jsonplaceholder.typicode.com/posts/ and deserializes it to a Model object
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonPostDetailModel GetPost(int id)
        {
            string url = Constants.POSTSURL + "/" + id.ToString();
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            JsonPostDetailModel jsonPlaceHolderModel = null;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    string result = reader.ReadToEnd();
                    //This deserializes the data from http://jsonplaceholder.typicode.com/posts/1 to JsonPlaceHolderData
                    JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
                    jsonPlaceHolderModel = new JsonPostDetailModel();
                    jsonPlaceHolderModel = jsonSerializer.Deserialize<JsonPostDetailModel>(result);

                }
            }

            return jsonPlaceHolderModel;
        }

        /// <summary>
        /// Method that parses the Model object into plain texe, Json, Html
        /// </summary>
        /// <param name="model"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public string ParseModelData(JsonPostDetailModel model, CopyMode mode)
        {
            string data = string.Empty;
            
            switch (mode)
            {
                case CopyMode.JSON:
                    var jsSerializer = new JavaScriptSerializer();
                    data = jsSerializer.Serialize(model);
                    break;
                case CopyMode.HTML:
                    var htmlSerializer = new XmlSerializer(typeof(JsonPostDetailModel));
                    using(var srtWriter = new StringWriter())
                    {
                        using (var writer = XmlWriter.Create(srtWriter))
                        {
                            htmlSerializer.Serialize(writer, model);
                            data = srtWriter.ToString();

                            //Just Convert the object to Html string representation
                            XmlDocument document = new XmlDocument();
                            document.LoadXml(data);
                            XmlNode node = document.SelectSingleNode(Constants.JSONPOSTDETAILMODEL);

                            if (node != null)
                                data = node.InnerXml.ToString();
                        }
                    }
                    break;
                default:
                    return model.ToString();
                    
            }

            return data;
        }
    }
}
