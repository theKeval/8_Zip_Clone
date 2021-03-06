﻿using _8_Zip.Helper;
using SharpCompress.Archive.Zip;
using SharpCompress.Reader;
using SharpCompress.Reader.GZip;
using SharpCompress.Reader.Rar;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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

//using System.Runtime.InteropServices;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace _8_Zip
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class ExtractHomePage : _8_Zip.Common.LayoutAwarePage
    {
        public ExtractHomePage()
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

        //private int nIndex = 0;
        public static StorageFolder myDestinationFolder;
        public static StorageFolder openedFolder;
        public static ObservableCollection<StorageFolder> previousFolder = new ObservableCollection<StorageFolder>();
        public ObservableCollection<FileFolderModel> parameterContent;
        public static int sizeOfPathToRemove;
        //public ObservableCollection<FileFolderModel> parameterContent_1 = new ObservableCollection<FileFolderModel>();
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            try
            {
                parameterContent = (ObservableCollection<FileFolderModel>)e.Parameter;

                gv_contentViewer.ItemsSource = parameterContent.Where(x => x.IsRoot == true);
                lv_contentViewer.ItemsSource = parameterContent.Where(x => x.IsRoot == true);

                openedFolder = HomePage.RootFolder;

                var a = openedFolder.Path.ToCharArray();
                var b = a.Count();
                var c = openedFolder.Name.ToCharArray();
                var d = c.Count();
                sizeOfPathToRemove = b - d;

                var pathToDisplay = openedFolder.Path.Remove(0, sizeOfPathToRemove - 1);
                tBlock_openedFolderPath.Text = pathToDisplay;
            }
            catch (Exception ex)
            {
                MessageDialog ms = new MessageDialog(ex.Message);
                ms.ShowAsync();
            }
        }


        public ObservableCollection<FileFolderModel> OC_display = new ObservableCollection<FileFolderModel>();
        private async void gv_contentViewer_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (gv_contentViewer.SelectedItem != null)
                {
                    var selectedFileFolder = (FileFolderModel)gv_contentViewer.SelectedItem;
                    

                    if (selectedFileFolder.isFolder)
                    {
                        openedFolder = await openedFolder.GetFolderAsync(selectedFileFolder.name);
                        previousFolder.Add(openedFolder);

                        #region
                        OC_display.Clear();

                        foreach (var item in await openedFolder.GetFoldersAsync())
                        {
                            foreach (var otherItem in parameterContent.Where(x=> x.isFolder == true && x.File.Path == item.Path))
                            {
                                OC_display.Add(new FileFolderModel
                                {
                                    name = otherItem.name,
                                    isFolder = true,
                                    Size = otherItem.Size,
                                    Ratio = otherItem.Ratio,
                                    File = otherItem.File
                                });
                            }

                            //parameterContent.Add(new FileFolderModel
                            //{
                            //    name = item.Name,
                            //    isFolder = true
                            //});
                        }

                        foreach (var item in await openedFolder.GetFilesAsync())
                        {
                            foreach (var otherItem in parameterContent.Where(x=> x.isFolder == false && x.File.Path == item.Path))
                            {
                                OC_display.Add(new FileFolderModel
                                {
                                    name = otherItem.name,
                                    isFolder = false,
                                    Size = otherItem.Size,
                                    Ratio = otherItem.Ratio,
                                    File = otherItem.File
                                });
                            }

                            //parameterContent.Add(new FileFolderModel
                            //{
                            //    name = item.Name,
                            //    isFolder = false
                            //});
                        }
                        #endregion

                        //foreach (var item in parameterContent.Where(x => x.File.Path == openedFolder.Path))
                        //{
                            
                        //}

                        var path = openedFolder.Path.Remove(0, sizeOfPathToRemove);
                        tBlock_openedFolderPath.Text = path;
                        gv_contentViewer.ItemsSource = OC_display;        // .Where(x=> x.File.Path == openedFolder.Path)
                        lv_contentViewer.ItemsSource = OC_display;
                        BtnGoUp.IsEnabled = true;

                    }
                    else
                    {

                    }
                }
                else if (lv_contentViewer.SelectedItem != null)
                {
                    var selectedFileFolder = (FileFolderModel)lv_contentViewer.SelectedItem;

                    if (selectedFileFolder.isFolder)
                    {
                        openedFolder = await openedFolder.GetFolderAsync(selectedFileFolder.name);
                        previousFolder.Add(openedFolder);

                        //parameterContent.Clear();


                        #region

                        OC_display.Clear();

                        foreach (var item in await openedFolder.GetFoldersAsync())
                        {
                            foreach (var otherItem in parameterContent.Where(x=> x.isFolder == true && x.File.Path == item.Path))
                            {
                                OC_display.Add(new FileFolderModel
                                {
                                    name = otherItem.name,
                                    isFolder = true,
                                    Size = otherItem.Size,
                                    Ratio = otherItem.Ratio,
                                    File = otherItem.File
                                });
                            }

                            //parameterContent.Add(new FileFolderModel
                            //{
                            //    name = item.Name,
                            //    isFolder = true
                            //});
                        }

                        foreach (var item in await openedFolder.GetFilesAsync())
                        {
                            foreach (var otherItem in parameterContent.Where(x=> x.isFolder == false && x.File.Path == item.Path))
                            {
                                OC_display.Add(new FileFolderModel
                                {
                                    name = otherItem.name,
                                    isFolder = false,
                                    Size = otherItem.Size,
                                    Ratio = otherItem.Ratio,
                                    File = otherItem.File
                                
                                });
                            }

                            //parameterContent.Add(new FileFolderModel
                            //{
                            //    name = item.Name,
                            //    isFolder = false
                            //});
                        }

                        #endregion


                        var path = openedFolder.Path.Remove(0, sizeOfPathToRemove);
                        tBlock_openedFolderPath.Text = path;
                        gv_contentViewer.ItemsSource = OC_display;
                        lv_contentViewer.ItemsSource = OC_display;
                        BtnGoUp.IsEnabled = true;

                    }
                }

            }
            catch (Exception ex)
            {
                MessageDialog ms = new MessageDialog(ex.Message);
                ms.ShowAsync();
            }
        }

        private async void Btn_ExtractAll_Tapped_1(object sender, TappedRoutedEventArgs e)
        {
            try
            {
                isBusy.IsBusy = true;

                var folderPicker = new FolderPicker();
                folderPicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
                folderPicker.FileTypeFilter.Add("*");
                //folderPicker.FileTypeFilter.Add(".jpg");
                //folderPicker.FileTypeFilter.Add(".txt");
                //folderPicker.FileTypeFilter.Add(".zip");
                //folderPicker.FileTypeFilter.Add(".rar");
                StorageFolder xyz = await folderPicker.PickSingleFolderAsync();
                myDestinationFolder = xyz;

                if (xyz != null)
                {
                    StorageFile abc = HomePage.ArchivedFile;

                    if (abc.FileType != ".gz")
                    {
                        await extractCompressedFile_ReaderFactory(abc, xyz);

                        MessageDialog ms = new MessageDialog("Archive extracted successfully.", "RAR Extractor");
                        await ms.ShowAsync();
                    }
                    else if (abc.FileType == ".gz")
                    {
                        await extractCompressedFile_GZipReader(abc, xyz);

                        MessageDialog ms = new MessageDialog("Archive extracted successfully.", "RAR Extractor");
                        await ms.ShowAsync();
                    }
                }

                isBusy.IsBusy = false;
            }
            catch (Exception ex)
            {
                if (ex.Message == "The specified path, file name, or both are too long. The fully qualified file name must be less than 260 characters, and the directory name must be less than 248 characters.")
                {
                    previousFolder.Clear();

                    HomePage.RootFolder.DeleteAsync();
                }

                isBusy.IsBusy = false;

                MessageDialog ms = new MessageDialog(ex.Message);
                ms.ShowAsync();

                this.Frame.Navigate(typeof(HomePage));
            }
            finally
            {
                isBusy.IsBusy = false;
            }
        }

        public Exception cryptoEx;
        public StorageFolder selectedFolder;
        private async Task extractCompressedFile_ReaderFactory(StorageFile sourceCompressedFile, StorageFolder destinationFolder)      //, StorageFolder destinationFolder
        {
            try
            {
                using (Stream fileStream = await sourceCompressedFile.OpenStreamForReadAsync())
                {
                    var Reader = ReaderFactory.Open(fileStream);
                    StorageFolder folder = await destinationFolder.CreateFolderAsync(sourceCompressedFile.DisplayName, CreationCollisionOption.ReplaceExisting);

                    //foreach (var entry in archive.Entries)
                    while (Reader.MoveToNextEntry())
                    {
                        selectedFolder = folder;
                        if (!Reader.Entry.IsDirectory)
                        {
                            if (Reader.Entry.FilePath.Contains("/"))
                            {
                                string[] splitedPath = Reader.Entry.FilePath.Split('/');
                                string FileName = splitedPath[(splitedPath.Count() - 1)];

                                foreach (var item in splitedPath)
                                {
                                    if (item == splitedPath.First() && !(item == splitedPath.Last()))
                                    {
                                        StorageFolder sf = await selectedFolder.CreateFolderAsync(item, CreationCollisionOption.OpenIfExists);
                                        selectedFolder = sf;
                                    }
                                    else if (!(item == splitedPath.Last()))
                                    {
                                        StorageFolder sf1 = await selectedFolder.CreateFolderAsync(item, CreationCollisionOption.OpenIfExists);
                                        selectedFolder = sf1;
                                        //StorageFolder sf1 = await sf.createfol
                                    }
                                    else if (item == splitedPath.Last())
                                    {
                                        StorageFile file = await selectedFolder.CreateFileAsync(FileName, CreationCollisionOption.OpenIfExists);
                                        Stream newFileStream = await file.OpenStreamForWriteAsync();

                                        MemoryStream streamEntry = new MemoryStream();
                                        Reader.WriteEntryTo(streamEntry);
                                        //entry.WriteTo(streamEntry);
                                        // buffer for extraction data
                                        byte[] data = streamEntry.ToArray();

                                        newFileStream.Write(data, 0, data.Length);
                                        newFileStream.Flush();
                                        newFileStream.Dispose();
                                    }
                                }
                            }
                            else
                            {
                                StorageFile file = await selectedFolder.CreateFileAsync(Reader.Entry.FilePath, CreationCollisionOption.OpenIfExists);
                                Stream newFileStream = await file.OpenStreamForWriteAsync();

                                MemoryStream streamEntry = new MemoryStream();
                                Reader.WriteEntryTo(streamEntry);
                                //entry.WriteTo(streamEntry);
                                // buffer for extraction data
                                byte[] data = streamEntry.ToArray();

                                newFileStream.Write(data, 0, data.Length);
                                newFileStream.Flush();
                                newFileStream.Dispose();
                            }
                            #region commented old code
                            //StorageFile newFile = await folder.CreateFileAsync(Reader.Entry.FilePath, CreationCollisionOption.FailIfExists);
                            //Stream newFileStream = await newFile.OpenStreamForWriteAsync();

                            //MemoryStream streamEntry = new MemoryStream();
                            //Reader.WriteEntryTo(streamEntry);
                            ////entry.WriteTo(streamEntry);
                            //// buffer for extraction data
                            //byte[] data = streamEntry.ToArray();

                            //newFileStream.Write(data, 0, data.Length);
                            //newFileStream.Flush();
                            //newFileStream.Dispose();
                            #endregion
                        }
                        else if (Reader.Entry.IsDirectory)
                        {
                            if (Reader.Entry.FilePath.Contains("/"))
                            {
                                string[] splitedPathWithBlank = Reader.Entry.FilePath.Split('/');
                                string FolderName = splitedPathWithBlank[splitedPathWithBlank.Length - 2];    // can also be written as splitedPath[splitedPath.count() - 2]
                                List<string> splitedPath = new List<string>();
                                List<string> splitedPathFinal = new List<string>();

                                foreach (var item in splitedPathWithBlank)
                                {
                                    if (!(item == splitedPathWithBlank.Last()))
                                    {
                                        splitedPath.Add(item);
                                    }
                                }

                                var i = 1;
                                foreach (var item in splitedPath)
                                {
                                    if (!(i == splitedPath.Count()))
                                    {
                                        splitedPathFinal.Add(item);
                                        //item = item + ".last";
                                    }
                                    else
                                    {
                                        splitedPathFinal.Add(item + ".last");
                                    }
                                    i++;
                                }

                                foreach (var item in splitedPathFinal)
                                {
                                    if (item == splitedPathFinal.First() && item != splitedPathFinal.Last())
                                    {
                                        StorageFolder sf = await selectedFolder.CreateFolderAsync(item, CreationCollisionOption.OpenIfExists);
                                        selectedFolder = sf;
                                    }
                                    else if (item != splitedPathFinal.Last())
                                    {
                                        StorageFolder sf1 = await selectedFolder.CreateFolderAsync(item, CreationCollisionOption.OpenIfExists);
                                        selectedFolder = sf1;
                                    }
                                    else if (item == splitedPathFinal.Last())
                                    {
                                        StorageFolder sfLast = await selectedFolder.CreateFolderAsync(FolderName, CreationCollisionOption.OpenIfExists);
                                    }
                                }
                            }

                            //StorageFolder newFolder = await folder.CreateFolderAsync(Reader.Entry.FilePath, CreationCollisionOption.ReplaceExisting);
                        }
                    }
                }
            }
            catch (SharpCompress.Common.CryptographicException cryptoEx)
            {
                this.cryptoEx = cryptoEx;
            }
            catch (System.NullReferenceException nullEx)
            {
                this.cryptoEx = nullEx;
            }

            if (this.cryptoEx != null)
            {
                CustomDialogForpassword.IsOpen = true;

                //var dialog = new InputDialog();
                //var password = await dialog.ShowAsync("8 Zip Clone", "This Zip is password protected.\nEnter Password:", "Submit", "Cancel");
            }

        }

        private async Task extractCompressedFile_GZipReader(StorageFile sourceCompressedFile, StorageFolder destinationFolder)
        {
            using (Stream fileStream = await sourceCompressedFile.OpenStreamForReadAsync())
            {
                var gZipReader = GZipReader.Open(fileStream);
                StorageFolder folder = await destinationFolder.CreateFolderAsync(sourceCompressedFile.DisplayName, CreationCollisionOption.GenerateUniqueName);

                while (gZipReader.MoveToNextEntry())
                {
                    selectedFolder = folder;

                    StorageFile file = await selectedFolder.CreateFileAsync(gZipReader.Entry.FilePath, CreationCollisionOption.OpenIfExists);
                    Stream newFileStream = await file.OpenStreamForWriteAsync();

                    MemoryStream streamEntry = new MemoryStream();
                    gZipReader.WriteEntryTo(streamEntry);
                    //entry.WriteTo(streamEntry);
                    // buffer for extraction data
                    byte[] data = streamEntry.ToArray();

                    newFileStream.Write(data, 0, data.Length);
                    newFileStream.Flush();
                    newFileStream.Dispose();
                }
            }
        }

        private async void BtnGoUp_Tapped(object sender, TappedRoutedEventArgs e)
        {
            //var parentFolderPath = openedFolder.Path;

            if (previousFolder.Count <= 1)
            {
                openedFolder = HomePage.RootFolder;
            }
            else
            {
                openedFolder = previousFolder[previousFolder.Count - 2];
            }

            previousFolder.RemoveAt(previousFolder.Count - 1);
            OC_display.Clear();

            foreach (var item in await openedFolder.GetFoldersAsync())
            {
                foreach (var otherItem in parameterContent.Where(x=> x.isFolder == true && x.File.Path == item.Path))
                {
                    OC_display.Add(new FileFolderModel
                    {
                        name = otherItem.name,
                        isFolder = true,
                        Size = otherItem.Size,
                        Ratio = otherItem.Ratio,
                        File = otherItem.File
                    });
                }

                //parameterContent.Add(new FileFolderModel
                //{
                //    name = item.Name,
                //    isFolder = true
                //});
            }

            foreach (var item in await openedFolder.GetFilesAsync())
            {
                foreach (var otherItem in parameterContent.Where(x => x.isFolder == false && x.File.Path == item.Path))
                {
                    OC_display.Add(new FileFolderModel
                    {
                        name = otherItem.name,
                        isFolder = false,
                        Size = otherItem.Size,
                        Ratio = otherItem.Ratio,
                        File = otherItem.File
                    });
                }

                //parameterContent.Add(new FileFolderModel
                //{
                //    name = item.Name,
                //    isFolder = false
                //});
            }

            tBlock_openedFolderPath.Text = openedFolder.Path.Remove(0, sizeOfPathToRemove);
            gv_contentViewer.ItemsSource = OC_display;
            lv_contentViewer.ItemsSource = OC_display;

            if (previousFolder.Count == 0)
            {
                BtnGoUp.IsEnabled = false;
            }

        }

        protected async override void GoBack(object sender, RoutedEventArgs e)
        {
            base.GoBack(sender, e);

            previousFolder.Clear();

            await HomePage.RootFolder.DeleteAsync();
        }



        private void CustomDialogForpassword_BackButtonClicked(object sender, RoutedEventArgs e)
        {
            CustomDialogForpassword.IsOpen = false;
        }

        private void DialogCancelClicked(object sender, RoutedEventArgs e)
        {
            CustomDialogForpassword.IsOpen = false;
        }

        private async void btnPassOK_Click(object sender, RoutedEventArgs e)
        {
            CustomDialogForpassword.IsOpen = false;

            isBusy.IsBusy = true;

            var password = tBox_password.Text;

            using (Stream fileStream = await HomePage.ArchivedFile.OpenStreamForReadAsync())
            {

                StorageFolder folder = await myDestinationFolder.CreateFolderAsync(HomePage.ArchivedFile.DisplayName, CreationCollisionOption.ReplaceExisting);

                switch (HomePage.ArchivedFile.FileType)
                {
                    case ".zip":

                        var zipArchive = ZipArchive.Open(fileStream, password);
                        foreach (var itemRoot in zipArchive.Entries)
                        {
                            #region process on every entry

                            selectedFolder = folder;
                            if (!itemRoot.IsDirectory)
                            {
                                if (itemRoot.FilePath.Contains("/"))
                                {
                                    string[] splitedPath = itemRoot.FilePath.Split('/');
                                    string FileName = splitedPath[(splitedPath.Count() - 1)];

                                    foreach (var item in splitedPath)
                                    {
                                        if (item == splitedPath.First() && !(item == splitedPath.Last()))
                                        {
                                            StorageFolder sf = await selectedFolder.CreateFolderAsync(item, CreationCollisionOption.OpenIfExists);
                                            selectedFolder = sf;
                                        }
                                        else if (!(item == splitedPath.Last()))
                                        {
                                            StorageFolder sf1 = await selectedFolder.CreateFolderAsync(item, CreationCollisionOption.OpenIfExists);
                                            selectedFolder = sf1;
                                            //StorageFolder sf1 = await sf.createfol
                                        }
                                        else if (item == splitedPath.Last())
                                        {
                                            StorageFile file = await selectedFolder.CreateFileAsync(FileName, CreationCollisionOption.OpenIfExists);
                                            if (itemRoot.Size != 0)
                                            {
                                                Stream newFileStream = await file.OpenStreamForWriteAsync();

                                                MemoryStream streamEntry = new MemoryStream();
                                                itemRoot.WriteTo(streamEntry);

                                                byte[] data = streamEntry.ToArray();

                                                newFileStream.Write(data, 0, data.Length);
                                                newFileStream.Flush();
                                                newFileStream.Dispose();
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    StorageFile file = await selectedFolder.CreateFileAsync(itemRoot.FilePath, CreationCollisionOption.OpenIfExists);
                                    if (itemRoot.Size != 0)
                                    {
                                        Stream newFileStream = await file.OpenStreamForWriteAsync();
                                        MemoryStream streamEntry = new MemoryStream();
                                        itemRoot.WriteTo(streamEntry);
                                        byte[] data = streamEntry.ToArray();
                                        newFileStream.Write(data, 0, data.Length);
                                        newFileStream.Flush();
                                        newFileStream.Dispose();
                                    }
                                }
                            }
                            else if (itemRoot.IsDirectory)
                            {
                                if (itemRoot.FilePath.Contains("/"))
                                {
                                    string[] splitedPathWithBlank = itemRoot.FilePath.Split('/');
                                    string FolderName = splitedPathWithBlank[splitedPathWithBlank.Length - 2];    // can also be written as splitedPath[splitedPath.count() - 2]
                                    List<string> splitedPath = new List<string>();
                                    List<string> splitedPathFinal = new List<string>();

                                    foreach (var item in splitedPathWithBlank)
                                    {
                                        if (!(item == splitedPathWithBlank.Last()))
                                        {
                                            splitedPath.Add(item);
                                        }
                                    }

                                    var i = 1;
                                    foreach (var item in splitedPath)
                                    {
                                        if (!(i == splitedPath.Count()))
                                        {
                                            splitedPathFinal.Add(item);
                                            //item = item + ".last";
                                        }
                                        else
                                        {
                                            splitedPathFinal.Add(item + ".last");
                                        }
                                        i++;
                                    }

                                    foreach (var item in splitedPathFinal)
                                    {
                                        if (item == splitedPathFinal.First() && item != splitedPathFinal.Last())
                                        {
                                            StorageFolder sf = await selectedFolder.CreateFolderAsync(item, CreationCollisionOption.OpenIfExists);
                                            selectedFolder = sf;
                                        }
                                        else if (item != splitedPathFinal.Last())
                                        {
                                            StorageFolder sf1 = await selectedFolder.CreateFolderAsync(item, CreationCollisionOption.OpenIfExists);
                                            selectedFolder = sf1;
                                        }
                                        else if (item == splitedPathFinal.Last())
                                        {
                                            StorageFolder sfLast = await selectedFolder.CreateFolderAsync(FolderName, CreationCollisionOption.OpenIfExists);
                                        }
                                    }
                                }

                                //StorageFolder newFolder = await folder.CreateFolderAsync(Reader.Entry.FilePath, CreationCollisionOption.ReplaceExisting);
                            }

                            #endregion
                        }

                        break;


                    case ".rar":

                        var rarReader = RarReader.Open(fileStream, password);
                        while (rarReader.MoveToNextEntry())
                        {
                            #region process on every entry

                            selectedFolder = folder;
                            if (!rarReader.Entry.IsDirectory)
                            {
                                if (rarReader.Entry.FilePath.Contains("/"))
                                {
                                    string[] splitedPath = rarReader.Entry.FilePath.Split('/');
                                    string FileName = splitedPath[(splitedPath.Count() - 1)];

                                    foreach (var item in splitedPath)
                                    {
                                        if (item == splitedPath.First() && !(item == splitedPath.Last()))
                                        {
                                            StorageFolder sf = await selectedFolder.CreateFolderAsync(item, CreationCollisionOption.OpenIfExists);
                                            selectedFolder = sf;
                                        }
                                        else if (!(item == splitedPath.Last()))
                                        {
                                            StorageFolder sf1 = await selectedFolder.CreateFolderAsync(item, CreationCollisionOption.OpenIfExists);
                                            selectedFolder = sf1;
                                            //StorageFolder sf1 = await sf.createfol
                                        }
                                        else if (item == splitedPath.Last())
                                        {
                                            StorageFile file = await selectedFolder.CreateFileAsync(FileName, CreationCollisionOption.OpenIfExists);
                                            Stream newFileStream = await file.OpenStreamForWriteAsync();

                                            MemoryStream streamEntry = new MemoryStream();
                                            rarReader.WriteEntryTo(streamEntry);
                                            //entry.WriteTo(streamEntry);
                                            // buffer for extraction data
                                            byte[] data = streamEntry.ToArray();

                                            newFileStream.Write(data, 0, data.Length);
                                            newFileStream.Flush();
                                            newFileStream.Dispose();
                                        }
                                    }
                                }
                                else
                                {
                                    StorageFile file = await selectedFolder.CreateFileAsync(rarReader.Entry.FilePath, CreationCollisionOption.OpenIfExists);
                                    Stream newFileStream = await file.OpenStreamForWriteAsync();

                                    MemoryStream streamEntry = new MemoryStream();
                                    rarReader.WriteEntryTo(streamEntry);
                                    //entry.WriteTo(streamEntry);
                                    // buffer for extraction data
                                    byte[] data = streamEntry.ToArray();

                                    newFileStream.Write(data, 0, data.Length);
                                    newFileStream.Flush();
                                    newFileStream.Dispose();
                                }
                            }
                            else if (rarReader.Entry.IsDirectory)
                            {
                                if (rarReader.Entry.FilePath.Contains("/"))
                                {
                                    string[] splitedPathWithBlank = rarReader.Entry.FilePath.Split('/');
                                    string FolderName = splitedPathWithBlank[splitedPathWithBlank.Length - 2];    // can also be written as splitedPath[splitedPath.count() - 2]
                                    List<string> splitedPath = new List<string>();
                                    List<string> splitedPathFinal = new List<string>();

                                    foreach (var item in splitedPathWithBlank)
                                    {
                                        if (!(item == splitedPathWithBlank.Last()))
                                        {
                                            splitedPath.Add(item);
                                        }
                                    }

                                    var i = 1;
                                    foreach (var item in splitedPath)
                                    {
                                        if (!(i == splitedPath.Count()))
                                        {
                                            splitedPathFinal.Add(item);
                                            //item = item + ".last";
                                        }
                                        else
                                        {
                                            splitedPathFinal.Add(item + ".last");
                                        }
                                        i++;
                                    }

                                    foreach (var item in splitedPathFinal)
                                    {
                                        if (item == splitedPathFinal.First() && item != splitedPathFinal.Last())
                                        {
                                            StorageFolder sf = await selectedFolder.CreateFolderAsync(item, CreationCollisionOption.OpenIfExists);
                                            selectedFolder = sf;
                                        }
                                        else if (item != splitedPathFinal.Last())
                                        {
                                            StorageFolder sf1 = await selectedFolder.CreateFolderAsync(item, CreationCollisionOption.OpenIfExists);
                                            selectedFolder = sf1;
                                        }
                                        else if (item == splitedPathFinal.Last())
                                        {
                                            StorageFolder sfLast = await selectedFolder.CreateFolderAsync(FolderName, CreationCollisionOption.OpenIfExists);
                                        }
                                    }
                                }

                            }
                            #endregion
                        }

                        break;


                    default:
                        break;
                }


            }

            isBusy.IsBusy = false;

        }

    }

}
