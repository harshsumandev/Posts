using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows.Input;
using JSONPlaceHolder.Commands;
using System.ComponentModel;
using JsonPostsRepositoryService;
using JsonPostsRepositoryService.Models;
using JSONPlaceHolder.ViewModels.Converters;

namespace JSONPlaceHolder.ViewModels
{
    /// <summary>
    /// Class that acts as a Main View Model that calls the service and gets Json Posts and its details
    /// Author : Harsh Suman
    /// 30/07/2016
    /// </summary>
    public class JsonPostsViewModel : INotifyPropertyChanged
    {
        #region Constructor
        /// <summary>
        /// Instantiates JSONPlaceHolderViewModel.
        /// </summary>
        public JsonPostsViewModel()
        {
            this.PopulateJsonPosts();
        }

        #endregion

        #region Class Level Variables

        private JsonPostViewObject _selectedJsonPlaceHolder = null;

        private ICommand _getPostCommand = null;
        public event PropertyChangedEventHandler PropertyChanged;

        IJsonPostsRepositoryService _service = null;
        private CopyMode _copyMode = CopyMode.TEXT;
        private string _postContent = string.Empty;
        private bool _enableGetPost = false;


        #endregion

        #region Properties

        /// <summary>
        /// Json Posts
        /// </summary>
        public ObservableCollection<JsonPostViewObject> JsonPosts { get; set; }

        /// <summary>
        /// Command to get a post as per supplied criteria
        /// </summary>
        public ICommand GetPostCommand
        {
            get
            {
                if (_getPostCommand == null)
                {
                    _getPostCommand = new DelegateCommand(GetSelectedPost, CanExecuteGetSelectedPost);
                }

                return _getPostCommand;
            }
        }

        public CopyMode CopyMode
        {
            get { return _copyMode; }
            set 
            { 
                _copyMode = value;
                RenderPostContentFormat();
            }
        }

        public bool EnableGetPost
        {
            get { return _enableGetPost; }
            set
            {
                _enableGetPost = value;
                if(_getPostCommand!= null)
                    (_getPostCommand as DelegateCommand).RaiseCanExecuteChanged();
            }

        }

        public string PostContent
        {
            get { return _postContent; }
            set 
            {
                _postContent = value;
                NotifyPropertyChanged("PostContent");
            }
        }

        public bool CopyModeEnabled
        {
            get { return _selectedJsonPlaceHolder != null; }
        }

        /// <summary>
        /// Currently selected Json Post
        /// </summary>
        public JsonPostViewObject SelectedJsonPlaceHolder
        {
            get { return _selectedJsonPlaceHolder; }
            set
            {
                _selectedJsonPlaceHolder = value;
                //(_getPostCommand as DelegateCommand).RaiseCanExecuteChanged();
                NotifyPropertyChanged("SelectedJsonPlaceHolder");
                NotifyPropertyChanged("CopyModeEnabled");
                RenderPostContentFormat();

            }
        }

        #endregion

        #region private methods

        /// <summary>
        /// Method that calls service to polulate Json Posts
        /// </summary>
        private void PopulateJsonPosts()
        {
            //Instantiate the service
            _service = ServiceManager.Instance.GetService();

            //Get place holders models and add for rendering
            JsonPosts = new ObservableCollection<JsonPostViewObject>();

            //This makes an async call to the service
            Task<List<JsonPostViewObject>>.Factory.StartNew(() =>
            {
                return ModelToViewObjectConverter.Convert(_service.GetPosts());
            }).ContinueWith((t) =>
            {
                if (t != null && t.Result != null &&
                    t.Result.Count > 0)
                {
                    //Polulate results, pump up on UI thread
                    App.Current.Dispatcher.BeginInvoke((Action)delegate()
                    {
                        t.Result.ForEach(v=> JsonPosts.Add(v));
                        if (JsonPosts != null && JsonPosts.Count > 0)
                            SelectedJsonPlaceHolder = JsonPosts.FirstOrDefault();

                    });
                }
            });

        }

        /// <summary>
        /// Raise property change
        /// </summary>
        /// <param name="property"></param>
        private void NotifyPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        /// <summary>
        /// Just fetch Json Post as per supplied criteria
        /// </summary>
        /// <param name="param"></param>
        private void GetSelectedPost(object param)
        {
            if (_service != null && _selectedJsonPlaceHolder != null)
            {
                JsonPostDetailModel jsonPlaceHolderModel = _service.GetPost(_selectedJsonPlaceHolder.Id);

                if (jsonPlaceHolderModel != null)
                {
                    //Just populate fetched object after service call
                    SelectedJsonPlaceHolder = ModelToViewObjectConverter.Convert(jsonPlaceHolderModel);
                    NotifyPropertyChanged("CopyModeEnabled");
                    RenderPostContentFormat();
                }
            }
        }

        /// <summary>
        /// Display content of the post in HTML, JSON, PlainText
        /// </summary>
        private void RenderPostContentFormat()
        {
            PostContent =_service.ParseModelData(ViewObjectToModelConverter.Convert(_selectedJsonPlaceHolder),
                CopyModeConverter.Convert(_copyMode));
        }

        /// <summary>
        /// Check whether the command can execute/not
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private bool CanExecuteGetSelectedPost(object param)
        {
            return _enableGetPost;
        }

        #endregion

    }
}
