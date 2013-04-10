using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace ControlLibrary
{
    public class TitleImage
    {
        public string ImageUri
        { 
            get;
            set;
        }

        public string Title
        {
            get;
            set;
        }

        public string Content
        {
            get;
            set;
        }

        internal ImageSource ImageSource
        {
            get;
            set;
        }
    }
    
    public class TitleImageHubTile : HubTileBase
    {
        private static readonly string isPrefix = "is://";
        private StorageFile storageFile = null;

        private Collection<TitleImage> imageSources = new Collection<TitleImage>();
        private Random rand = new Random();

        /// <summary>
        /// Gets Or Sets the ImageSources collection. Images are randomly chosen from this
        /// collection when an image needs to be displayed.
        /// </summary>
        public Collection<TitleImage> ImageSources
        {
            get
            {
                return this.imageSources;
            }
            set
            {
                this.imageSources = value;
                //后台赋值调用可override的OnUpLoadSources虚方法
                OnUpLoadSources();
            }
        }

        //继承的子类调用OnUpLoadSources为了ImageSources集合赋值 刷新到Ui
        protected virtual void OnUpLoadSources()
        {

        }

        /// <summary>
        /// A callback that if set will be used to generate an ImageSource based on the provided string uri.
        /// </summary>
        public Func<string, TitleImage> CreateImageSource
        {
            get;
            set;
        }

        /// <summary>
        /// Creates an ImageSource from a randomly chosen URI from the ImageSources collection and returns it.
        /// </summary>
        /// <returns>Returns an ImageSource with a randomly chosen URI from the ImageSources collection.</returns>
        protected async Task<TitleImage> GetSource()
        {
            if (this.ImageSources.Count == 0)
            {
                return null;
                //return new TitleImage();
            }

            int index = this.rand.Next(this.ImageSources.Count);
            while (!this.IsNewIndexValid(index))
            {
                index = index = this.rand.Next(this.ImageSources.Count);
            }

            string uri = this.imageSources[index].ImageUri;

            if (this.CreateImageSource != null)
            {
                return this.CreateImageSource(uri);
            }

            ImageSource imageSource = await this.CreateDefaultImageSource(uri);

            TitleImage titleImage = new TitleImage();
            titleImage = this.imageSources[index];
            titleImage.ImageSource = imageSource;
            return titleImage;
        }

        /// <summary>
        /// Should be overridden in descendant classes to indicate if the same image can be displayed
        /// many times in a row.
        /// </summary>
        /// <param name="index">The index of the new image.</param>
        /// <returns>Returns true if the image can be repeated and false otherwise.</returns>
        protected virtual bool IsNewIndexValid(int index)
        {
            return true;
        }

        private async Task<ImageSource> CreateDefaultImageSource(string uri)
        {
            BitmapImage result = new BitmapImage();
            try
            {
                if (uri.StartsWith(isPrefix))
                {
                    uri = uri.Substring(isPrefix.Length);

                    StorageFolder folder = ApplicationData.Current.LocalFolder;

                    //using (Stream str = await folder.OpenStreamForReadAsync(uri))
                    //{
                    //    IRandomAccessStream iRandomAccessStream = MicrosoftStreamExtensions.AsRandomAccessStream(str);
                    //    result.SetSource(iRandomAccessStream);
                    //}

                    storageFile = await folder.GetFileAsync(uri);
                    using (IRandomAccessStream raStream = await storageFile.OpenAsync(FileAccessMode.Read))
                    {
                        result.SetSource(raStream);
                    }
                    return result;
                }

                var reg = @"http(s)?://([\w-]+\.)+[\w-]+(/[\w-./?%&=]*)?";
                Regex regex = new Regex(reg, RegexOptions.IgnoreCase);
                if (regex.IsMatch(uri))
                {
                    result.UriSource = new Uri(uri, UriKind.RelativeOrAbsolute);
                }
                else
                {
                    result.UriSource = new Uri(this.BaseUri, uri);
                }
                return result;
            }
            catch
            {
                return result = null;
            }
        }
    }
}
