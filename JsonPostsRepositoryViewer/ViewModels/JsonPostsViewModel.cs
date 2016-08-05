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
using System.Windows.Threading;
using System.Windows;

namespace JSONPlaceHolder.ViewModels
{
    /// <summary>
    /// Class that acts as a Main View Model that calls the service and gets Json Posts and its details
    /// Author : Harsh Suman
    /// 30/07/2016
    /// </summary>
    public class JsonPostsViewModel : ViewModelBase
    {
        #region Constructor
        /// <summary>
        /// Instantiates JSONPlaceHolderViewModel.
        /// </summary>
        public JsonPostsViewModel()
        {
            Initialize();
        }

        #endregion

        #region Class Level Variables

        private JsonPostViewObject _selectedJsonPlaceHolder = null;

        private ICommand _getPostCommand = null;
        private ICommand _copyTextPostCommand = null;
        private ICommand _copyJsonPostCommand = null;
        private ICommand _copyHtmlPostCommand = null;
        

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

        /// <summary>
        /// Command to get a post as per supplied criteria
        /// </summary>
        public ICommand CopyTextPostCommand
        {
            get
            {
                if (_copyTextPostCommand == null)
                {
                    _copyTextPostCommand = new DelegateCommand(CopyText, CanExecuteCopyCommand);
                }

                return _copyTextPostCommand;
            }
        }

        /// <summary>
        /// Command to get a post as per supplied criteria
        /// </summary>
        public ICommand CopyJsonPostCommand
        {
            get
            {
                if (_copyJsonPostCommand == null)
                {
                    _copyJsonPostCommand = new DelegateCommand(CopyJson, CanExecuteCopyCommand);
                }

                return _copyJsonPostCommand;
            }
        }

        /// <summary>
        /// Command to get a post as per supplied criteria
        /// </summary>
        public ICommand CopyHtmlPostCommand
        {
            get
            {
                if (_copyHtmlPostCommand == null)
                {
                    _copyHtmlPostCommand = new DelegateCommand(CopyHtml, CanExecuteCopyCommand);
                }

                return _copyHtmlPostCommand;
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

                NotifyPropertyChanged("SelectedJsonPlaceHolder");
                NotifyPropertyChanged("CopyModeEnabled");
                EnableCopyContextMenu();

                if(value != null)
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
                    if (App.Current != null)
                    {
                        App.Current.Dispatcher.BeginInvoke((Action)delegate()
                        {
                            t.Result.ForEach(v => JsonPosts.Add(v));
                            if (JsonPosts != null && JsonPosts.Count > 0)
                                SelectedJsonPlaceHolder = JsonPosts.FirstOrDefault();

                        });
                    }
                    else //Test
                    {
                      t.Result.ForEach(v => JsonPosts.Add(v));
                        if (JsonPosts != null && JsonPosts.Count > 0)
                            SelectedJsonPlaceHolder = JsonPosts.FirstOrDefault();
                    }
                }
            });

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

        private void EnableCopyContextMenu()
        {
            //Enable context Menu Items
            if (_copyTextPostCommand != null)
                (_copyTextPostCommand as DelegateCommand).RaiseCanExecuteChanged();

            if (_copyJsonPostCommand != null)
                (_copyJsonPostCommand as DelegateCommand).RaiseCanExecuteChanged();

            if (_copyHtmlPostCommand != null)
                (_copyHtmlPostCommand as DelegateCommand).RaiseCanExecuteChanged();
        }

        /// <summary>
        /// Copy Text to clipboard
        /// </summary>
        /// <param name="param"></param>
        private void CopyText(object param)
        {
            CopyToClipboard(JsonPostsRepositoryService.CopyMode.TEXT);
        }

        /// <summary>
        /// Copy Json to Clipboard
        /// </summary>
        /// <param name="param"></param>
        private void CopyJson(object param)
        {
            CopyToClipboard(JsonPostsRepositoryService.CopyMode.JSON);
        }

        /// <summary>
        /// Copy Html to Clipboard
        /// </summary>
        /// <param name="param"></param>
        private void CopyHtml(object param)
        {
            CopyToClipboard(JsonPostsRepositoryService.CopyMode.HTML);
        }

        private void CopyToClipboard(JsonPostsRepositoryService.CopyMode copyMode)
        {
            string text = string.Empty;

            switch (copyMode)
            {
                case JsonPostsRepositoryService.CopyMode.JSON:
                    text = _service.ParseModelData(ViewObjectToModelConverter.Convert(_selectedJsonPlaceHolder),
                                            JsonPostsRepositoryService.CopyMode.JSON);
                    break;
                case JsonPostsRepositoryService.CopyMode.HTML:
                    text = _service.ParseModelData(ViewObjectToModelConverter.Convert(_selectedJsonPlaceHolder),
                                            JsonPostsRepositoryService.CopyMode.HTML);
                    break;
                default:
                    text = _service.ParseModelData(ViewObjectToModelConverter.Convert(_selectedJsonPlaceHolder),
                                            JsonPostsRepositoryService.CopyMode.TEXT);
                    break;
            }

            if (!string.IsNullOrWhiteSpace(text))
                Clipboard.SetText(text);

        }


        /// <summary>
        /// Check whether the command can execute/not
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private bool CanExecuteCopyCommand(object param)
        {
            return (_selectedJsonPlaceHolder != null);
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

        /// <summary>
        /// Initializes the view model asnd instantiates dependencies
        /// </summary>
        protected override void Initialize()
        {
            //Instantiates the service
            _service = ServiceManager.Instance.GetService();

            //Get place holders models and add for rendering
            JsonPosts = new ObservableCollection<JsonPostViewObject>();

            this.PopulateJsonPosts();
        }

    }
}
