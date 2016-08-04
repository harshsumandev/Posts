using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JSONPlaceHolder.ViewModels;
using JSONPlaceHolder;
using System.Threading;
using System.Linq;
using JsonPostsRepositoryService.Models;

namespace JsonPostsRepositoryTests
{
    /// <summary>
    /// Test class to test JsonPostsViewModel, loading of JsonPosts, SelectedPost etc
    /// </summary>
    [TestClass]
    public class JsonPostsViewModelTest
    {
        JsonPostsViewModel _jsonPostsViewModel = null;
        private const int _postsCount = 100;
        
        /// <summary>
        /// Initiaalizes the test case and instantiates JsonPostsViewModel. 
        /// </summary>
        [TestInitialize]
        public void Init()
        {
            _jsonPostsViewModel = new JsonPostsViewModel();
            Thread.Sleep(5000);
        }

        /// <summary>
        /// Verifies that all JsonPosts have been loaded
        /// Also verifies that First Item for JsonPosts have been set to SelectedJsonPlaceHolder
        /// </summary>
        [TestMethod]
        public void LoadJsonPosts_Test()
        {
            if (_jsonPostsViewModel != null)
            {
                //Check whether JsonPosts to render have been populated
                Assert.IsTrue((_jsonPostsViewModel.JsonPosts != null &&
                    _jsonPostsViewModel.JsonPosts.Count == _postsCount));

                Assert.AreEqual(_jsonPostsViewModel.SelectedJsonPlaceHolder.ToString(),
                    _jsonPostsViewModel.JsonPosts.FirstOrDefault().ToString());
            }
        }

        /// <summary>
        /// Test to verify the SelectedPost which polulates when JsonPosts are loaded initially.
        /// i.e. the SelectedPost will be first one from JsonPosts.
        /// Tests get/set for SelectedJsonPlaceHolderProperty
        /// </summary>
        [TestMethod]
        public void SelectedJsonPlaceHolderProperty_Test()
        {
            if (_jsonPostsViewModel != null)
            {
                //set it to null
                _jsonPostsViewModel.SelectedJsonPlaceHolder = null;
                //Assert for setter
                Assert.AreEqual(_jsonPostsViewModel.SelectedJsonPlaceHolder, null);

                //Set it to first view object
                _jsonPostsViewModel.SelectedJsonPlaceHolder = _jsonPostsViewModel.JsonPosts.FirstOrDefault();
                Assert.AreEqual(_jsonPostsViewModel.SelectedJsonPlaceHolder.ToString(), _jsonPostsViewModel.JsonPosts.FirstOrDefault().ToString());

            }

        }

        /// <summary>
        /// Property test for PostContent property, validates and checks for set, get
        /// </summary>
        [TestMethod]
        public void PostContentProperty_Test()
        {
            if (_jsonPostsViewModel != null)
            {
                JsonPostDetailModel jsonPostDetailModel = ViewObjectToModelConverter.Convert(_jsonPostsViewModel.JsonPosts.FirstOrDefault());
                
                //During initial loading _jsonPostsViewModel.PostContent was set, verify its validity
                Assert.AreEqual(_jsonPostsViewModel.PostContent, jsonPostDetailModel.ToString());

                //Intentionally set the value
                _jsonPostsViewModel.PostContent = string.Empty;
                _jsonPostsViewModel.PostContent = jsonPostDetailModel.ToString();

                Assert.AreEqual(_jsonPostsViewModel.PostContent, jsonPostDetailModel.ToString());
            }
        }
    }
}
