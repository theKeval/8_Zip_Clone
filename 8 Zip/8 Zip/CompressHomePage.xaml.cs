using SharpCompress.Common;
using SharpCompress.Writer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace _8_Zip
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class CompressHomePage : _8_Zip.Common.LayoutAwarePage
    {
        public CompressHomePage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="navigationParameter">The parameter value passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested.
        /// </param>
        /// <param name="pageState">A dictionary of state preserved by this page during an earlier
        /// session.  This will be null the first time a page is visited.</param>
        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="pageState">An empty dictionary to be populated with serializable state.</param>
        protected override void SaveState(Dictionary<String, Object> pageState)
        {
        }


        StorageFile ZipToWrite;
        //public List<StorageFile> OC_selectedFiles;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            try
            {
                ZipToWrite = (StorageFile)e.Parameter;
            }
            catch (Exception ex)
            {
                MessageDialog ms = new MessageDialog(ex.Message);
                ms.ShowAsync();
            }

        }

        private void BtnGoUp_Tapped(object sender, TappedRoutedEventArgs e)
        {

        }

        private void gv_contentViewer_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {

        }

        public ArchiveType archiveType;
        private async void Btn_AddFiles_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var FilesPicker = new FileOpenPicker();
            FilesPicker.CommitButtonText = "Compress";
            FilesPicker.FileTypeFilter.Add("*");
            FilesPicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            
            var OC_selectedFiles = await FilesPicker.PickMultipleFilesAsync();

            using (var zip = await ZipToWrite.OpenStreamForWriteAsync())
            {
                var type = ZipToWrite.FileType;
                
                switch (type)
                {
                    case ".zip":
                        archiveType = ArchiveType.Zip;
                        break;

                    case ".rar":
                        archiveType = ArchiveType.Rar;
                        break;

                    case ".gzip":
                        archiveType = ArchiveType.GZip;
                        break;

                    case ".tar":
                        archiveType = ArchiveType.Tar;
                        break;

                    case ".sevenzip":
                        archiveType = ArchiveType.SevenZip;
                        break;

                    default:
                        break;
                }

                using (var zipWriter = WriterFactory.Open(zip, archiveType, CompressionType.None))
                {
                    foreach (var singleFile in OC_selectedFiles)
                    {
                        zipWriter.Write(singleFile.Name, await singleFile.OpenStreamForReadAsync());
                        //zipWriter.Write(singleFile.Name, singleFile.OpenStreamForReadAsync);
                        //zipWriter.Write(Path.GetFileName(file), filePath);
                    }
                }
            }

        }

        private void Btn_AddFolder_Tapped(object sender, TappedRoutedEventArgs e)
        {

        }

    }
}
