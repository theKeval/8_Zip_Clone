using _8_Zip.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;

namespace _8_Zip.Converter
{
    public class FileFolderToImageConverter : IValueConverter
    {

        public FileFolderToImageConverter()
        {
            abc();
        }

        public Uri imageURI;
        public string filePath;

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            //throw new NotImplementedException();
            try
            {
                var obj = (FileFolderModel)value;
                if (obj.isFolder == true)
                {
                    //abc(obj.isFolder);
                    imageURI = new Uri(filePath, UriKind.Absolute);
                }
                else
                {
                    //abc(obj.isFolder);
                    imageURI = new Uri(filePath, UriKind.Absolute);
                }
            }
            catch (Exception ex)
            {
                MessageDialog ms = new MessageDialog(ex.Message);
                ms.ShowAsync();
            }

            return imageURI;

        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }

        public async void abc()     //bool isFolder
        {
            StorageFolder folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("Assets");
            StorageFile file = await folder.GetFileAsync("Folder symbol.png");
            //if (isFolder)
            //{
            //    file = await folder.GetFileAsync("Folder symbol.png");
            //}
            //else
            //{
            //    file = await folder.GetFileAsync("File symbol.png");
            //}
            
            filePath = file.Path;
        }

    }
}
