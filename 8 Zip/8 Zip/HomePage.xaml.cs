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
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
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
        private string _rootNamespace;

        public string RootNamespace
        {
            get { return _rootNamespace; }
            set { _rootNamespace = value; }
        }

        private FileActivatedEventArgs _fileEventArgs = null;
        public FileActivatedEventArgs FileEvent
        {
            get { return _fileEventArgs; }
            set { _fileEventArgs = value; }
        }

        private ProtocolActivatedEventArgs _protocolEventArgs = null;
        public ProtocolActivatedEventArgs ProtocolEvent
        {
            get { return _protocolEventArgs; }
            set { _protocolEventArgs = value; }
        }



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

        public static StorageFile ArchivedFile;

        public static bool flag = true;
        public static StorageFile ReceivedZip;
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            bool flag = false;
            if (e.Parameter != "")
            {
                if (e.Parameter != null && HomePage.flag)
                {
                    try
                    {
                        ReceivedZip = (StorageFile)e.Parameter;
                        ArchivedFile = ReceivedZip;
                        //var ZipFile = await StorageFile.GetFileFromPathAsync(pathOfZip);

                        if (ReceivedZip != null)
                        {
                            if (ReceivedZip.FileType != ".gz")
                            {
                                //await extractCompressedFile_ArchiveFactory(abc);

                                //await extractCompressedFile_ReaderFactory(abc, xyz);

                                flag = false;
                                await extractInLocalFolder_ReaderFactory(ReceivedZip);
                            }
                            else if (ReceivedZip.FileType == ".gz")
                            {
                                flag = false;
                                await extractInLocalFolder_GZipReader(ReceivedZip);
                            }
                        }

                        isBusy.IsBusy = false;
                    }
                    catch (NotSupportedException nse)
                    {
                        isBusy.IsBusy = false;

                        MessageDialog ms = new MessageDialog("Archive you selected is not supported.", "RAR Extractor");
                        ms.ShowAsync();
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message == "The specified path, file name, or both are too long. The fully qualified file name must be less than 260 characters, and the directory name must be less than 248 characters.")
                        {
                            flag = true;
                        }

                        isBusy.IsBusy = false;

                        MessageDialog ms = new MessageDialog(ex.Message);
                        ms.ShowAsync();

                    }
                    finally
                    {
                        isBusy.IsBusy = false;
                    }
                    if (flag)
                    {
                        await (await Windows.Storage.ApplicationData.Current.LocalFolder.GetFolderAsync(RootFolderName)).DeleteAsync();
                    }
                }
            }
            

        }


        public static StorageFile abc;
        //public static Type type;
        private async void OpenArchiveTapped_sp(object sender, TappedRoutedEventArgs e)
        {
            bool flag = false;
            try
            {
                //tb.Text = "please wait...";
                isBusy.IsBusy = true;

                var filePicker = new FileOpenPicker();
                filePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
                filePicker.FileTypeFilter.Add("*");
                //filePicker.FileTypeFilter.Add(".rar");
                abc = await filePicker.PickSingleFileAsync();
                
                ArchivedFile = abc;

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

            catch (NotSupportedException nse)
            {
                isBusy.IsBusy = false;

                MessageDialog ms = new MessageDialog("Archive you selected is not supported.", "RAR Extractor");
                ms.ShowAsync();
            }

            catch (Exception ex)
            {
                if (ex.Message == "The specified path, file name, or both are too long. The fully qualified file name must be less than 260 characters, and the directory name must be less than 248 characters.")
                {
                    flag = true;
                }

                isBusy.IsBusy = false;

                MessageDialog ms = new MessageDialog(ex.Message);
                ms.ShowAsync();
            }
            finally
            {
                isBusy.IsBusy = false;
            }
            if (flag)
            {
                await (await Windows.Storage.ApplicationData.Current.LocalFolder.GetFolderAsync(RootFolderName)).DeleteAsync();
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
        public ObservableCollection<FileFolderModel> OC_rootFolderContent = new ObservableCollection<FileFolderModel>();
        public string sizeText;
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

                        sizeText = (Reader.Entry.CompressedSize).ToString() + " Bytes";
                        selectedFolder = folder;

                        if (!Reader.Entry.IsDirectory)
                        {
                            #region
                            if (Reader.Entry.FilePath.Contains("/"))
                            {
                                string[] splitedPath = Reader.Entry.FilePath.Split('/');
                                string FileName = splitedPath[(splitedPath.Count() - 1)];

                                foreach (var item in splitedPath)
                                {
                                    if (item == splitedPath.First() && !(item == splitedPath.Last()))
                                    {
                                        bool flag;
                                        try
                                        {
                                            selectedFolder = await selectedFolder.GetFolderAsync(item);
                                            flag = false;
                                        }
                                        catch (FileNotFoundException fnfe)
                                        {
                                            flag = true;
                                        }

                                        if (flag == true)
                                        {
                                            StorageFolder sf = await selectedFolder.CreateFolderAsync(item, CreationCollisionOption.OpenIfExists);
                                            selectedFolder = sf;

                                            var obj = new FileFolderModel();
                                            obj.File = sf;
                                            obj.isFolder = true;
                                            obj.name = sf.Name;
                                            //obj.Size = sizeText;
                                            obj.IsRoot = true;
                                            if (Reader.Entry.Size != 0)
                                            {
                                                obj.Ratio = "Ratio: " + (100 * (Reader.Entry.CompressedSize / Reader.Entry.Size)).ToString() + " %";
                                            }

                                            OC_content.Add(obj);
                                        }

                                    }
                                    else if (!(item == splitedPath.Last()))
                                    {
                                        bool flag;
                                        try
                                        {
                                            selectedFolder = await selectedFolder.GetFolderAsync(item);
                                            flag = false;
                                        }
                                        catch (FileNotFoundException fnfe)
                                        {
                                            flag = true;
                                        }
                                        if (flag == true)
                                        {
                                            StorageFolder sf1 = await selectedFolder.CreateFolderAsync(item, CreationCollisionOption.OpenIfExists);
                                            selectedFolder = sf1;

                                            var obj = new FileFolderModel();
                                            obj.File = sf1;
                                            obj.isFolder = true;
                                            obj.name = sf1.Name;
                                            //obj.Size = sizeText;
                                            obj.IsRoot = false;
                                            if (Reader.Entry.Size != 0)
                                            {
                                                obj.Ratio = "Ratio: " + (100 * (Reader.Entry.CompressedSize / Reader.Entry.Size)).ToString() + " %";
                                            }

                                            OC_content.Add(obj);
                                        }
                                    }
                                    else if (item == splitedPath.Last())
                                    {
                                        bool flag;
                                        try
                                        {
                                            var check = await selectedFolder.GetFileAsync(FileName);
                                            flag = false;
                                        }
                                        catch (FileNotFoundException fnfe)
                                        {
                                            flag = true;
                                        }

                                        if (flag == true)
                                        {
                                            StorageFile file = await selectedFolder.CreateFileAsync(FileName, CreationCollisionOption.OpenIfExists);

                                            var obj = new FileFolderModel();
                                            obj.File = file;
                                            obj.isFolder = false;
                                            obj.name = file.Name;
                                            obj.Size = sizeText;
                                            obj.IsRoot = false;
                                            if (Reader.Entry.Size != 0)
                                            {
                                                obj.Ratio = "Ratio: " + (100 * (Reader.Entry.CompressedSize / Reader.Entry.Size)).ToString() + " %";
                                            }

                                            OC_content.Add(obj);
                                        }

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
                                bool flag;
                                try
                                {
                                    var check = await selectedFolder.GetFileAsync(Reader.Entry.FilePath);
                                    flag = false;
                                }
                                catch (FileNotFoundException fnfe)
                                {
                                    flag = true;
                                }
                                if (flag == true)
                                {
                                    StorageFile file = await selectedFolder.CreateFileAsync(Reader.Entry.FilePath, CreationCollisionOption.OpenIfExists);

                                    var obj = new FileFolderModel();
                                    obj.File = file;
                                    obj.isFolder = false;
                                    obj.name = file.Name;
                                    obj.Size = sizeText;
                                    obj.IsRoot = true;
                                    if (Reader.Entry.Size != 0)
                                    {
                                        obj.Ratio = "Ratio: " + (100 * (Reader.Entry.CompressedSize / Reader.Entry.Size)).ToString() + " %";
                                    }

                                    OC_content.Add(obj);
                                }

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
                            #endregion
                        }
                        else if (Reader.Entry.IsDirectory)
                        {
                            #region
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
                                        bool flag;
                                        try
                                        {
                                            selectedFolder = await selectedFolder.GetFolderAsync(item);
                                            flag = false;
                                        }
                                        catch (FileNotFoundException fnfe)
                                        {
                                            flag = true;
                                        }
                                        if (flag == true)
                                        {
                                            StorageFolder sf = await selectedFolder.CreateFolderAsync(item, CreationCollisionOption.OpenIfExists);
                                            selectedFolder = sf;

                                            var obj = new FileFolderModel();
                                            obj.File = sf;
                                            obj.isFolder = true;
                                            obj.name = sf.Name;
                                            //obj.Size = sizeText;
                                            obj.IsRoot = true;
                                            if (Reader.Entry.Size != 0)
                                            {
                                                obj.Ratio = "Ratio: " + (100 * (Reader.Entry.CompressedSize / Reader.Entry.Size)).ToString() + " %";
                                            }

                                            OC_content.Add(obj);
                                        }

                                    }
                                    else if (item != splitedPathFinal.Last())
                                    {
                                        bool flag;
                                        try
                                        {
                                            selectedFolder = await selectedFolder.GetFolderAsync(item);
                                            flag = false;
                                        }
                                        catch (FileNotFoundException fnfe)
                                        {
                                            flag = true;
                                        }
                                        if (flag == true)
                                        {
                                            StorageFolder sf1 = await selectedFolder.CreateFolderAsync(item, CreationCollisionOption.OpenIfExists);
                                            selectedFolder = sf1;

                                            var obj = new FileFolderModel();
                                            obj.File = sf1;
                                            obj.isFolder = true;
                                            obj.name = sf1.Name;
                                            //obj.Size = sizeText;
                                            obj.IsRoot = false;
                                            if (Reader.Entry.Size != 0)
                                            {
                                                obj.Ratio = "Ratio: " + (100 * (Reader.Entry.CompressedSize / Reader.Entry.Size)).ToString() + " %";
                                            }

                                            OC_content.Add(obj);
                                        }

                                    }
                                    else if (item == splitedPathFinal.Last())
                                    {
                                        bool flag;
                                        try
                                        {
                                            flag = false;
                                        }
                                        catch (FileNotFoundException fnfe)
                                        {
                                            flag = true;
                                        }
                                        if (flag == true)
                                        {
                                            StorageFolder sfLast = await selectedFolder.CreateFolderAsync(FolderName, CreationCollisionOption.OpenIfExists);

                                            var obj = new FileFolderModel();
                                            obj.File = sfLast;
                                            obj.isFolder = true;
                                            obj.name = sfLast.Name;
                                            //obj.Size = sizeText;
                                            obj.IsRoot = false;
                                            if (Reader.Entry.Size != 0)
                                            {
                                                obj.Ratio = "Ratio: " + (100 * (Reader.Entry.CompressedSize / Reader.Entry.Size)).ToString() + " %";
                                            }

                                            OC_content.Add(obj);
                                        }

                                    }
                                }
                            }

                            //StorageFolder newFolder = await folder.CreateFolderAsync(Reader.Entry.FilePath, CreationCollisionOption.ReplaceExisting);
                            #endregion
                        }

                        #endregion
                    }

                    #endregion
                }

                RootFolder = await ApplicationData.Current.LocalFolder.GetFolderAsync(RootFolderName);

                //foreach (var item in await RootFolder.GetFoldersAsync())
                //{
                //    OC_content.Add(new FileFolderModel
                //    {
                //        name = item.Name,
                //        isFolder = true
                //    });
                //}

                //foreach (var item in await RootFolder.GetFilesAsync())
                //{
                //    OC_content.Add(new FileFolderModel
                //    {
                //        name = item.Name,
                //        isFolder = false
                //    });
                //}

                this.Frame.Navigate(typeof(ExtractHomePage), OC_content);
                //this.Frame.Navigate(typeof(ExtractHomePage), new { OC_content, abc });

            }
            catch (SharpCompress.Common.CryptographicException cryptoEx)
            {
                this.cryptoEx = cryptoEx;
            }
            catch (System.NullReferenceException nullEx)
            {
                this.cryptoEx = nullEx;
            }
            //catch (System.DivideByZeroException dbzex)
            //{

            //}

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

                        var obj = new FileFolderModel();
                        obj.File = file;
                        obj.isFolder = false;
                        obj.name = file.Name;
                        obj.Size = (gZipReader.Entry.CompressedSize).ToString() + " Bytes";
                        obj.IsRoot = true;
                        if (gZipReader.Entry.Size != 0)
                        {
                            obj.Ratio = "Ratio: " + (100 * (gZipReader.Entry.CompressedSize / gZipReader.Entry.Size)).ToString() + " %";
                        }

                        OC_content.Add(obj);
                    }
                }

                RootFolder = await ApplicationData.Current.LocalFolder.GetFolderAsync(RootFolderName);
                //foreach (var item in await RootFolder.GetFoldersAsync())
                //{
                //    OC_content.Add(new FileFolderModel
                //    {
                //        name = item.Name,
                //        isFolder = true
                //    });
                //}

                //foreach (var item in await RootFolder.GetFilesAsync())
                //{
                //    OC_content.Add(new FileFolderModel
                //    {
                //        name = item.Name,
                //        isFolder = false
                //    });
                //}

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

            using (Stream fileStream = await ArchivedFile.OpenStreamForReadAsync())
            {
                RootFolderName = ArchivedFile.DisplayName;
                StorageFolder folder = await ApplicationData.Current.LocalFolder.CreateFolderAsync(ArchivedFile.DisplayName, CreationCollisionOption.ReplaceExisting);

                switch (ArchivedFile.FileType)
                {
                    case ".zip":

                        var zipArchive = ZipArchive.Open(fileStream, password);
                        foreach (var itemRoot in zipArchive.Entries)
                        {
                            sizeText = (itemRoot.CompressedSize).ToString() + " Bytes";

                            #region while for every entry

                            selectedFolder = folder;
                            if (!itemRoot.IsDirectory)
                            {
                                #region
                                if (itemRoot.FilePath.Contains("/"))
                                {
                                    string[] splitedPath = itemRoot.FilePath.Split('/');
                                    string FileName = splitedPath[(splitedPath.Count() - 1)];

                                    foreach (var item in splitedPath)
                                    {
                                        if (item == splitedPath.First() && !(item == splitedPath.Last()))
                                        {
                                            bool flag;
                                            try
                                            {
                                                selectedFolder = await selectedFolder.GetFolderAsync(item);
                                                flag = false;
                                            }
                                            catch (FileNotFoundException fnfe)
                                            {
                                                flag = true;
                                            }
                                            if (flag == true)
                                            {
                                                StorageFolder sf = await selectedFolder.CreateFolderAsync(item, CreationCollisionOption.OpenIfExists);
                                                selectedFolder = sf;

                                                var obj = new FileFolderModel();
                                                obj.File = sf;
                                                obj.isFolder = true;
                                                obj.name = sf.Name;
                                                //obj.Size = sizeText;
                                                obj.IsRoot = true;
                                                if (itemRoot.Size != 0)
                                                {
                                                    obj.Ratio = "Ratio: " + (100 * (itemRoot.CompressedSize / itemRoot.Size)).ToString() + " %";
                                                }

                                                OC_content.Add(obj);
                                            }

                                        }
                                        else if (!(item == splitedPath.Last()))
                                        {
                                            bool flag;
                                            try
                                            {
                                                selectedFolder = await selectedFolder.GetFolderAsync(item);
                                                flag = false;
                                            }
                                            catch (FileNotFoundException fnfe)
                                            {
                                                flag = true;
                                            }
                                            if (flag == true)
                                            {
                                                StorageFolder sf1 = await selectedFolder.CreateFolderAsync(item, CreationCollisionOption.OpenIfExists);
                                                selectedFolder = sf1;

                                                var obj = new FileFolderModel();
                                                obj.File = sf1;
                                                obj.isFolder = true;
                                                obj.name = sf1.Name;
                                                //obj.Size = sizeText;
                                                obj.IsRoot = false;
                                                if (itemRoot.Size != 0)
                                                {
                                                    obj.Ratio = "Ratio: " + (100 * (itemRoot.CompressedSize / itemRoot.Size)).ToString() + " %";
                                                }

                                                OC_content.Add(obj);
                                            }

                                        }
                                        else if (item == splitedPath.Last())
                                        {
                                            bool flag;
                                            try
                                            {
                                                var check = await selectedFolder.GetFileAsync(FileName);
                                                flag = false;
                                            }
                                            catch (FileNotFoundException fnfe)
                                            {
                                                flag = true;
                                            }
                                            if (flag == true)
                                            {
                                                StorageFile file = await selectedFolder.CreateFileAsync(FileName, CreationCollisionOption.OpenIfExists);

                                                var obj = new FileFolderModel();
                                                obj.File = file;
                                                obj.isFolder = false;
                                                obj.name = file.Name;
                                                obj.Size = sizeText;
                                                obj.IsRoot = false;
                                                if (itemRoot.Size != 0)
                                                {
                                                    obj.Ratio = "Ratio: " + (100 * (itemRoot.CompressedSize / itemRoot.Size)).ToString() + " %";
                                                }

                                                OC_content.Add(obj);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    bool flag;
                                    try
                                    {
                                        var check = await selectedFolder.GetFileAsync(itemRoot.FilePath);
                                        flag = false;
                                    }
                                    catch (FileNotFoundException fnfe)
                                    {
                                        flag = true;
                                    }
                                    if (flag == true)
                                    {
                                        StorageFile file = await selectedFolder.CreateFileAsync(itemRoot.FilePath, CreationCollisionOption.OpenIfExists);

                                        var obj = new FileFolderModel();
                                        obj.File = file;
                                        obj.isFolder = false;
                                        obj.name = file.Name;
                                        obj.Size = sizeText;
                                        obj.IsRoot = true;
                                        if (itemRoot.Size != 0)
                                        {
                                            obj.Ratio = "Ratio: " + (100 * (itemRoot.CompressedSize / itemRoot.Size)).ToString() + " %";
                                        }

                                        OC_content.Add(obj);
                                    }
                                }
                                #endregion
                            }
                            else if (itemRoot.IsDirectory)
                            {
                                #region
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
                                            bool flag;
                                            try
                                            {
                                                selectedFolder = await selectedFolder.GetFolderAsync(item);
                                                flag = false;
                                            }
                                            catch (FileNotFoundException fnfe)
                                            {
                                                flag = true;
                                            }
                                            if (flag == true)
                                            {
                                                StorageFolder sf = await selectedFolder.CreateFolderAsync(item, CreationCollisionOption.OpenIfExists);
                                                selectedFolder = sf;

                                                var obj = new FileFolderModel();
                                                obj.File = sf;
                                                obj.isFolder = true;
                                                obj.name = sf.Name;
                                                //obj.Size = sizeText;
                                                obj.IsRoot = true;
                                                if (itemRoot.Size != 0)
                                                {
                                                    obj.Ratio = "Ratio: " + (100 * (itemRoot.CompressedSize / itemRoot.Size)).ToString() + " %";
                                                }

                                                OC_content.Add(obj);
                                            }
                                        }
                                        else if (item != splitedPathFinal.Last())
                                        {
                                            bool flag;
                                            try
                                            {
                                                selectedFolder = await selectedFolder.GetFolderAsync(item);
                                                flag = false;
                                            }
                                            catch (FileNotFoundException fnfe)
                                            {
                                                flag = true;
                                            }
                                            if (flag == true)
                                            {
                                                StorageFolder sf1 = await selectedFolder.CreateFolderAsync(item, CreationCollisionOption.OpenIfExists);
                                                selectedFolder = sf1;

                                                var obj = new FileFolderModel();
                                                obj.File = sf1;
                                                obj.isFolder = true;
                                                obj.name = sf1.Name;
                                                //obj.Size = sizeText;
                                                obj.IsRoot = false;
                                                if (itemRoot.Size != 0)
                                                {
                                                    obj.Ratio = "Ratio: " + (100 * (itemRoot.CompressedSize / itemRoot.Size)).ToString() + " %";
                                                }

                                                OC_content.Add(obj);
                                            }
                                        }
                                        else if (item == splitedPathFinal.Last())
                                        {
                                            bool flag;
                                            try
                                            {
                                                var check = await selectedFolder.GetFolderAsync(FolderName);
                                                flag = false;
                                            }
                                            catch (FileNotFoundException fnfe)
                                            {
                                                flag = true;
                                            }
                                            if (flag == true)
                                            {
                                                StorageFolder sfLast = await selectedFolder.CreateFolderAsync(FolderName, CreationCollisionOption.OpenIfExists);

                                                var obj = new FileFolderModel();
                                                obj.File = sfLast;
                                                obj.isFolder = true;
                                                obj.name = sfLast.Name;
                                                //obj.Size = sizeText;
                                                obj.IsRoot = false;
                                                if (itemRoot.Size != 0)
                                                {
                                                    obj.Ratio = "Ratio: " + (100 * (itemRoot.CompressedSize / itemRoot.Size)).ToString() + " %";
                                                }

                                                OC_content.Add(obj);
                                            }
                                        }
                                    }
                                }
                                #endregion
                            }
                            #endregion
                        }

                        break;


                    case ".rar":

                        var rarReader = RarReader.Open(fileStream, password);
                        while (rarReader.MoveToNextEntry())
                        {
                            sizeText = (rarReader.Entry.CompressedSize).ToString() + " Bytes";

                            #region while for every entry

                            selectedFolder = folder;
                            if (!rarReader.Entry.IsDirectory)
                            {
                                #region
                                if (rarReader.Entry.FilePath.Contains("/"))
                                {
                                    string[] splitedPath = rarReader.Entry.FilePath.Split('/');
                                    string FileName = splitedPath[(splitedPath.Count() - 1)];

                                    foreach (var item in splitedPath)
                                    {
                                        if (item == splitedPath.First() && !(item == splitedPath.Last()))
                                        {
                                            bool flag;
                                            try
                                            {
                                                selectedFolder = await selectedFolder.GetFolderAsync(item);
                                                flag = false;
                                            }
                                            catch (FileNotFoundException fnfe)
                                            {
                                                flag = true;
                                            }
                                            if (flag == true)
                                            {
                                                StorageFolder sf = await selectedFolder.CreateFolderAsync(item, CreationCollisionOption.OpenIfExists);
                                                selectedFolder = sf;

                                                var obj = new FileFolderModel();
                                                obj.File = sf;
                                                obj.isFolder = true;
                                                obj.name = sf.Name;
                                                //obj.Size = sizeText;
                                                obj.IsRoot = true;
                                                if (rarReader.Entry.Size != 0)
                                                {
                                                    obj.Ratio = "Ratio: " + (100 * (rarReader.Entry.CompressedSize / rarReader.Entry.Size)).ToString() + " %";
                                                }

                                                OC_content.Add(obj);
                                            }

                                        }
                                        else if (!(item == splitedPath.Last()))
                                        {
                                            bool flag;
                                            try
                                            {
                                                selectedFolder = await selectedFolder.GetFolderAsync(item);
                                                flag = false;
                                            }
                                            catch (FileNotFoundException fnfe)
                                            {
                                                flag = true;
                                            }
                                            if (flag == true)
                                            {
                                                StorageFolder sf1 = await selectedFolder.CreateFolderAsync(item, CreationCollisionOption.OpenIfExists);
                                                selectedFolder = sf1;

                                                var obj = new FileFolderModel();
                                                obj.File = sf1;
                                                obj.isFolder = true;
                                                obj.name = sf1.Name;
                                                //obj.Size = sizeText;
                                                obj.IsRoot = false;
                                                if (rarReader.Entry.Size != 0)
                                                {
                                                    obj.Ratio = "Ratio: " + (100 * (rarReader.Entry.CompressedSize / rarReader.Entry.Size)).ToString() + " %";
                                                }

                                                OC_content.Add(obj);
                                            }

                                        }
                                        else if (item == splitedPath.Last())
                                        {
                                            bool flag;
                                            try
                                            {
                                                var check = await selectedFolder.GetFileAsync(FileName);
                                                flag = false;
                                            }
                                            catch (FileNotFoundException fnfe)
                                            {
                                                flag = true;
                                            }
                                            if (flag == true)
                                            {
                                                StorageFile file = await selectedFolder.CreateFileAsync(FileName, CreationCollisionOption.OpenIfExists);

                                                var obj = new FileFolderModel();
                                                obj.File = file;
                                                obj.isFolder = false;
                                                obj.name = file.Name;
                                                obj.Size = sizeText;
                                                obj.IsRoot = false;
                                                if (rarReader.Entry.Size != 0)
                                                {
                                                    obj.Ratio = "Ratio: " + (100 * (rarReader.Entry.CompressedSize / rarReader.Entry.Size)).ToString() + " %";
                                                }

                                                OC_content.Add(obj);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    bool flag;
                                    try
                                    {
                                        var check = selectedFolder.GetFileAsync(rarReader.Entry.FilePath);
                                        flag = false;
                                    }
                                    catch (FileNotFoundException fnfe)
                                    {
                                        flag = true;
                                    }
                                    if (flag == true)
                                    {
                                        StorageFile file = await selectedFolder.CreateFileAsync(rarReader.Entry.FilePath, CreationCollisionOption.OpenIfExists);

                                        var obj = new FileFolderModel();
                                        obj.File = file;
                                        obj.isFolder = false;
                                        obj.name = file.Name;
                                        obj.Size = sizeText;
                                        obj.IsRoot = true;
                                        if (rarReader.Entry.Size != 0)
                                        {
                                            obj.Ratio = "Ratio: " + (100 * (rarReader.Entry.CompressedSize / rarReader.Entry.Size)).ToString() + " %";
                                        }

                                        OC_content.Add(obj);
                                    }
                                }
                                #endregion
                            }
                            else if (rarReader.Entry.IsDirectory)
                            {
                                #region
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
                                            bool flag;
                                            try
                                            {
                                                selectedFolder = await selectedFolder.GetFolderAsync(item);
                                                flag = false;
                                            }
                                            catch (FileNotFoundException fnfe)
                                            {
                                                flag = true;
                                            }
                                            if (flag == true)
                                            {
                                                StorageFolder sf = await selectedFolder.CreateFolderAsync(item, CreationCollisionOption.OpenIfExists);
                                                selectedFolder = sf;

                                                var obj = new FileFolderModel();
                                                obj.File = sf;
                                                obj.isFolder = true;
                                                obj.name = sf.Name;
                                                //obj.Size = sizeText;
                                                obj.IsRoot = true;
                                                if (rarReader.Entry.Size != 0)
                                                {
                                                    obj.Ratio = "Ratio: " + (100 * (rarReader.Entry.CompressedSize / rarReader.Entry.Size)).ToString() + " %";
                                                }

                                                OC_content.Add(obj);
                                            }
                                        }
                                        else if (item != splitedPathFinal.Last())
                                        {
                                            bool flag;
                                            try
                                            {
                                                selectedFolder = await selectedFolder.GetFolderAsync(item);
                                                flag = false;
                                            }
                                            catch (FileNotFoundException fnfe)
                                            {
                                                flag = true;
                                            }
                                            if (flag == true)
                                            {
                                                StorageFolder sf1 = await selectedFolder.CreateFolderAsync(item, CreationCollisionOption.OpenIfExists);
                                                selectedFolder = sf1;

                                                var obj = new FileFolderModel();
                                                obj.File = sf1;
                                                obj.isFolder = true;
                                                obj.name = sf1.Name;
                                                //obj.Size = sizeText;
                                                obj.IsRoot = false;
                                                if (rarReader.Entry.Size != 0)
                                                {
                                                    obj.Ratio = "Ratio: " + (100 * (rarReader.Entry.CompressedSize / rarReader.Entry.Size)).ToString() + " %";
                                                }

                                                OC_content.Add(obj);
                                            }
                                        }
                                        else if (item == splitedPathFinal.Last())
                                        {
                                            bool flag;
                                            try
                                            {
                                                var check = await selectedFolder.GetFolderAsync(FolderName);
                                                flag = false;
                                            }
                                            catch (FileNotFoundException fnfe)
                                            {
                                                flag = true;
                                            }
                                            if (flag == true)
                                            {
                                                StorageFolder sfLast = await selectedFolder.CreateFolderAsync(FolderName, CreationCollisionOption.OpenIfExists);

                                                var obj = new FileFolderModel();
                                                obj.File = sfLast;
                                                obj.isFolder = true;
                                                obj.name = sfLast.Name;
                                                //obj.Size = sizeText;
                                                obj.IsRoot = false;
                                                if (rarReader.Entry.Size != 0)
                                                {
                                                    obj.Ratio = "Ratio: " + (100 * (rarReader.Entry.CompressedSize / rarReader.Entry.Size)).ToString() + " %";
                                                }

                                                OC_content.Add(obj);
                                            }
                                        }
                                    }
                                }
                                #endregion
                            }
                            #endregion
                        }

                        break;

                    default:
                        break;
                }


            }

            RootFolder = await ApplicationData.Current.LocalFolder.GetFolderAsync(RootFolderName);

            #region commented
            //foreach (var item in await RootFolder.GetFoldersAsync())
            //{
            //    foreach (var subItem in OC_content)
            //    {
            //        if (subItem.isFolder == true && subItem.name == item.Name)
            //        {
            //            OC_rootFolderContent.Add(new FileFolderModel
            //    {
            //        name = item.Name,
            //                isFolder = true,
            //                size = subItem.size,
            //                compressedSize = subItem.compressedSize,
            //                ratio = subItem.ratio
            //    });
            //}
            //    }
            //}

            //foreach (var item in await RootFolder.GetFilesAsync())
            //{
            //    foreach (var subItem in OC_content)
            //    {
            //        if (subItem.isFolder == false && subItem.name == item.Name)
            //        {
            //            OC_rootFolderContent.Add(new FileFolderModel
            //    {
            //        name = item.Name,
            //                isFolder = false,
            //                size = subItem.size,
            //                compressedSize = subItem.compressedSize,
            //                ratio = subItem.ratio
            //    });
            //}
            //    }
            //}
            #endregion

            isBusy.IsBusy = false;

            this.Frame.Navigate(typeof(ExtractHomePage), OC_content);

        }

        private async void snappedViewOpenArchive_sp(object sender, TappedRoutedEventArgs e)
        {
            ApplicationView.TryUnsnap();

            try
            {
                isBusy.IsBusy = true;

                var filePicker = new FileOpenPicker();
                filePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
                filePicker.FileTypeFilter.Add("*");
                
                abc = await filePicker.PickSingleFileAsync();

                if (abc != null)
                {
                    if (abc.FileType != ".gz")
                    {
                        await extractInLocalFolder_ReaderFactory(abc);
                    }
                    else if (abc.FileType == ".gz")
                    {
                        await extractInLocalFolder_GZipReader(abc);
                    }
                }

                isBusy.IsBusy = false;

            }

            catch (NotSupportedException nse)
            {
                isBusy.IsBusy = false;

                MessageDialog ms = new MessageDialog("Archive you selected is not supported.", "RAR Extractor");
                ms.ShowAsync();
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

        private async void snappedViewCreateArchive_sp(object sender, TappedRoutedEventArgs e)
        {
            ApplicationView.TryUnsnap();

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




    }
}
