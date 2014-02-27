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
//using WinRTXamlToolkit.Controls;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace _8_Zip
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class HomePage : _8_Zip.Common.LayoutAwarePage
    {
        public HomePage()
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

        public static StorageFile abc;
        //public static Type type;
        private async void OpenArchiveTapped_sp(object sender, TappedRoutedEventArgs e)
        {
            try
            {
                //tb.Text = "please wait...";
                isBusy.IsBusy = true;

                var filePicker = new FileOpenPicker();
                filePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
                filePicker.FileTypeFilter.Add("*");
                //filePicker.FileTypeFilter.Add(".rar");
                abc = await filePicker.PickSingleFileAsync();

                //var folderPicker = new FolderPicker();
                //folderPicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
                //folderPicker.FileTypeFilter.Add(".png");
                //folderPicker.FileTypeFilter.Add(".jpg");
                //folderPicker.FileTypeFilter.Add(".txt");
                //StorageFolder xyz = await folderPicker.PickSingleFolderAsync();

                if (abc != null)
                {
                    if (abc.FileType != ".gz")
                    {
                        //await extractCompressedFile_ArchiveFactory(abc);

                        //await extractCompressedFile_ReaderFactory(abc, xyz);

                        await extractInLocalFolder_ReaderFactory(abc);
                    }
                    else if (abc.FileType == ".gz")
                    {
                        await extractInLocalFolder_GZipReader(abc);
                    }
                }


                //tb.Text = "Done.";
                isBusy.IsBusy = false;

            }
            catch (Exception ex)
            {
                isBusy.IsBusy = false;

                MessageDialog ms = new MessageDialog(ex.Message);
                ms.ShowAsync();
            }
            finally
            {
                isBusy.IsBusy = false;
            }
        }

        private async void CreateArchiveTapped_sp(object sender, TappedRoutedEventArgs e)
        {
            FileSavePicker savePicker = new FileSavePicker();
            savePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            savePicker.SuggestedFileName = "New Archive.zip";
            savePicker.DefaultFileExtension = ".zip";
            savePicker.FileTypeChoices.Add("Zip", new List<string> { ".zip" });
            savePicker.FileTypeChoices.Add("GZip", new List<string> { ".gz" });
            savePicker.FileTypeChoices.Add("BZip2", new List<string> { ".bz2" });
            savePicker.FileTypeChoices.Add("Tar", new List<string> { ".tar" });

            //savePicker.FileTypeChoices.Add("FileTypeFilter", new List<string> { ".zip", ".rar" });
            savePicker.CommitButtonText = "Save";

            StorageFile FileToSave = await savePicker.PickSaveFileAsync();

            if (FileToSave != null)
            {
                this.Frame.Navigate(typeof(CompressHomePage), FileToSave);
            }

        }

        #region commented - extracting archive with writing in file - now moved to ExtractHomePage code behind

        //private async Task extractCompressedFile_ReaderFactory(StorageFile sourceCompressedFile, StorageFolder destinationFolder)      //, StorageFolder destinationFolder
        //{
        //    using (Stream fileStream = await sourceCompressedFile.OpenStreamForReadAsync())
        //    {
        //        var Reader = ReaderFactory.Open(fileStream);
        //        StorageFolder folder = await destinationFolder.CreateFolderAsync(sourceCompressedFile.DisplayName, CreationCollisionOption.OpenIfExists);

        //        //foreach (var entry in archive.Entries)
        //        while (Reader.MoveToNextEntry())
        //        {
        //            selectedFolder = folder;
        //            if (!Reader.Entry.IsDirectory)
        //            {
        //                if (Reader.Entry.FilePath.Contains("/"))
        //                {
        //                    string[] splitedPath = Reader.Entry.FilePath.Split('/');
        //                    string FileName = splitedPath[(splitedPath.Count() - 1)];

        //                    foreach (var item in splitedPath)
        //                    {
        //                        if (item == splitedPath.First() && !(item == splitedPath.Last()))
        //                        {
        //                            StorageFolder sf = await selectedFolder.CreateFolderAsync(item, CreationCollisionOption.OpenIfExists);
        //                            selectedFolder = sf;
        //                        }
        //                        else if (!(item == splitedPath.Last()))
        //                        {
        //                            StorageFolder sf1 = await selectedFolder.CreateFolderAsync(item, CreationCollisionOption.OpenIfExists);
        //                            selectedFolder = sf1;
        //                            //StorageFolder sf1 = await sf.createfol
        //                        }
        //                        else if (item == splitedPath.Last())
        //                        {
        //                            StorageFile file = await selectedFolder.CreateFileAsync(FileName, CreationCollisionOption.OpenIfExists);
        //                            Stream newFileStream = await file.OpenStreamForWriteAsync();

        //                            MemoryStream streamEntry = new MemoryStream();
        //                            Reader.WriteEntryTo(streamEntry);
        //                            //entry.WriteTo(streamEntry);
        //                            // buffer for extraction data
        //                            byte[] data = streamEntry.ToArray();

        //                            newFileStream.Write(data, 0, data.Length);
        //                            newFileStream.Flush();
        //                            newFileStream.Dispose();
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    StorageFile file = await selectedFolder.CreateFileAsync(Reader.Entry.FilePath, CreationCollisionOption.OpenIfExists);
        //                    Stream newFileStream = await file.OpenStreamForWriteAsync();

        //                    MemoryStream streamEntry = new MemoryStream();
        //                    Reader.WriteEntryTo(streamEntry);
        //                    //entry.WriteTo(streamEntry);
        //                    // buffer for extraction data
        //                    byte[] data = streamEntry.ToArray();

        //                    newFileStream.Write(data, 0, data.Length);
        //                    newFileStream.Flush();
        //                    newFileStream.Dispose();
        //                }
        //                #region commented old code
        //                //StorageFile newFile = await folder.CreateFileAsync(Reader.Entry.FilePath, CreationCollisionOption.FailIfExists);
        //                //Stream newFileStream = await newFile.OpenStreamForWriteAsync();

        //                //MemoryStream streamEntry = new MemoryStream();
        //                //Reader.WriteEntryTo(streamEntry);
        //                ////entry.WriteTo(streamEntry);
        //                //// buffer for extraction data
        //                //byte[] data = streamEntry.ToArray();

        //                //newFileStream.Write(data, 0, data.Length);
        //                //newFileStream.Flush();
        //                //newFileStream.Dispose();
        //                #endregion
        //            }
        //            else if (Reader.Entry.IsDirectory)
        //            {
        //                if (Reader.Entry.FilePath.Contains("/"))
        //                {
        //                    string[] splitedPathWithBlank = Reader.Entry.FilePath.Split('/');
        //                    string FolderName = splitedPathWithBlank[splitedPathWithBlank.Length - 2];    // can also be written as splitedPath[splitedPath.count() - 2]
        //                    List<string> splitedPath = new List<string>();
        //                    List<string> splitedPathFinal = new List<string>();

        //                    foreach (var item in splitedPathWithBlank)
        //                    {
        //                        if (!(item == splitedPathWithBlank.Last()))
        //                        {
        //                            splitedPath.Add(item);
        //                        }
        //                    }

        //                    var i = 1;
        //                    foreach (var item in splitedPath)
        //                    {
        //                        if (!(i == splitedPath.Count()))
        //                        {
        //                            splitedPathFinal.Add(item);
        //                            //item = item + ".last";
        //                        }
        //                        else
        //                        {
        //                            splitedPathFinal.Add(item + ".last");
        //                        }
        //                        i++;
        //                    }

        //                    foreach (var item in splitedPathFinal)
        //                    {
        //                        if (item == splitedPathFinal.First() && item != splitedPathFinal.Last())
        //                        {
        //                            StorageFolder sf = await selectedFolder.CreateFolderAsync(item, CreationCollisionOption.OpenIfExists);
        //                            selectedFolder = sf;
        //                        }
        //                        else if (item != splitedPathFinal.Last())
        //                        {
        //                            StorageFolder sf1 = await selectedFolder.CreateFolderAsync(item, CreationCollisionOption.OpenIfExists);
        //                            selectedFolder = sf1;
        //                        }
        //                        else if (item == splitedPathFinal.Last())
        //                        {
        //                            StorageFolder sfLast = await selectedFolder.CreateFolderAsync(FolderName, CreationCollisionOption.OpenIfExists);
        //                        }
        //                    }
        //                }

        //                //StorageFolder newFolder = await folder.CreateFolderAsync(Reader.Entry.FilePath, CreationCollisionOption.ReplaceExisting);
        //            }
        //        }
        //    }
        //}

        #endregion


        public Exception cryptoEx;
        public StorageFolder selectedFolder;
        public string RootFolderName;
        public static StorageFolder RootFolder;
        public ObservableCollection<FileFolderModel> OC_content = new ObservableCollection<FileFolderModel>();
        private async Task extractInLocalFolder_ReaderFactory(StorageFile sourceCompressedFile)      //, StorageFolder destinationFolder
        {
            try
            {
                using (Stream fileStream = await sourceCompressedFile.OpenStreamForReadAsync())
                {
                    #region process on selected Zip File

                    var Reader = ReaderFactory.Open(fileStream);
                    RootFolderName = sourceCompressedFile.DisplayName;
                    StorageFolder folder = await ApplicationData.Current.LocalFolder.CreateFolderAsync(sourceCompressedFile.DisplayName, CreationCollisionOption.ReplaceExisting);

                    //SharpCompress.Archive.Zip.ZipArchive.Open(
                    //SharpCompress.Archive.Rar.RarArchive.Open(
                    //SharpCompress.Reader.Rar.RarReader.Open(

                    //List<string> lstFileName = new List<string>();
                    //while (Reader.MoveToNextEntry())
                    //{
                    //    lstFileName.Add(Reader.Entry.FilePath);
                    //}

                    while (Reader.MoveToNextEntry())
                    {
                        #region while for every entry

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

                                        #region for writing in file - commented
                                        //Stream newFileStream = await file.OpenStreamForWriteAsync();

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
                                }
                            }
                            else
                            {
                                StorageFile file = await selectedFolder.CreateFileAsync(Reader.Entry.FilePath, CreationCollisionOption.OpenIfExists);

                                #region for writing in file - commented
                                //Stream newFileStream = await file.OpenStreamForWriteAsync();

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
                        #endregion
                    }

                    #endregion
                }

                RootFolder = await ApplicationData.Current.LocalFolder.GetFolderAsync(RootFolderName);

                foreach (var item in await RootFolder.GetFoldersAsync())
                {
                    OC_content.Add(new FileFolderModel
                    {
                        name = item.Name,
                        isFolder = true
                    });
                }

                foreach (var item in await RootFolder.GetFilesAsync())
                {
                    OC_content.Add(new FileFolderModel
                    {
                        name = item.Name,
                        isFolder = false
                    });
                }

                this.Frame.Navigate(typeof(ExtractHomePage), OC_content);
                //this.Frame.Navigate(typeof(ExtractHomePage), new { OC_content, abc });

            }
            catch (SharpCompress.Common.CryptographicException cryptoEx)
            {
                this.cryptoEx = cryptoEx;
            }
            catch(System.NullReferenceException nullEx)
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

        private async Task extractInLocalFolder_GZipReader(StorageFile sourceCompressedFile)
        {
            using (Stream fileStream = await sourceCompressedFile.OpenStreamForReadAsync())
            {
                var gZipReader = GZipReader.Open(fileStream);
                RootFolderName = sourceCompressedFile.DisplayName;
                StorageFolder folder = await ApplicationData.Current.LocalFolder.CreateFolderAsync(sourceCompressedFile.DisplayName, CreationCollisionOption.ReplaceExisting);

                while (gZipReader.MoveToNextEntry())
                {
                    selectedFolder = folder;
                    {
                        StorageFile file = await selectedFolder.CreateFileAsync(gZipReader.Entry.FilePath, CreationCollisionOption.OpenIfExists);
                    }
                }

                RootFolder = await ApplicationData.Current.LocalFolder.GetFolderAsync(RootFolderName);
                foreach (var item in await RootFolder.GetFoldersAsync())
                {
                    OC_content.Add(new FileFolderModel
                    {
                        name = item.Name,
                        isFolder = true
                    });
                }

                foreach (var item in await RootFolder.GetFilesAsync())
                {
                    OC_content.Add(new FileFolderModel
                    {
                        name = item.Name,
                        isFolder = false
                    });
                }

                this.Frame.Navigate(typeof(ExtractHomePage), OC_content);

            }
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

            using (Stream fileStream = await abc.OpenStreamForReadAsync())
            {
                RootFolderName = abc.DisplayName;
                StorageFolder folder = await ApplicationData.Current.LocalFolder.CreateFolderAsync(abc.DisplayName, CreationCollisionOption.ReplaceExisting);

                switch (abc.FileType)
                {
                    case ".zip":
                        
                        var zipArchive = ZipArchive.Open(fileStream, password);
                        foreach (var itemRoot in zipArchive.Entries)
                        {
                            #region while for every entry

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
                                        }
                                        else if (item == splitedPath.Last())
                                        {
                                            StorageFile file = await selectedFolder.CreateFileAsync(FileName, CreationCollisionOption.OpenIfExists);
                                        }
                                    }
                                }
                                else
                                {
                                    StorageFile file = await selectedFolder.CreateFileAsync(itemRoot.FilePath, CreationCollisionOption.OpenIfExists);
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


                    case ".rar":
                        
                        var rarReader = RarReader.Open(fileStream, password);
                        while (rarReader.MoveToNextEntry())
                        {
                            #region while for every entry

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
                                        }
                                    }
                                }
                                else
                                {
                                    StorageFile file = await selectedFolder.CreateFileAsync(rarReader.Entry.FilePath, CreationCollisionOption.OpenIfExists);
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

            RootFolder = await ApplicationData.Current.LocalFolder.GetFolderAsync(RootFolderName);

            foreach (var item in await RootFolder.GetFoldersAsync())
            {
                OC_content.Add(new FileFolderModel
                {
                    name = item.Name,
                    isFolder = true
                });
            }

            foreach (var item in await RootFolder.GetFilesAsync())
            {
                OC_content.Add(new FileFolderModel
                {
                    name = item.Name,
                    isFolder = false
                });
            }

            isBusy.IsBusy = false;

            this.Frame.Navigate(typeof(ExtractHomePage), OC_content);

        }



    }
}
