﻿using SharpCompress.Archive.GZip;
using SharpCompress.Archive.Rar;
using SharpCompress.Common;
using SharpCompress.Writer;
using SharpCompress.Writer.GZip;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Search;
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
            isBusy.IsBusy = true;

            try
            {
                using (var zipStream = await ZipToWrite.OpenStreamForWriteAsync())
                {
                    var type = ZipToWrite.FileType;

                    switch (type)
                    {
                        case ".zip":
                            archiveType = ArchiveType.Zip;
                            break;

                        case ".gz":
                            archiveType = ArchiveType.GZip;
                            break;

                        case ".bz2":
                            archiveType = ArchiveType.Zip;
                            break;

                        case ".tar":
                            archiveType = ArchiveType.Tar;
                            break;

                        //case ".rar":
                        //    archiveType = ArchiveType.Rar;
                        //    break;

                        //case ".sevenzip":
                        //    archiveType = ArchiveType.SevenZip;
                        //    break;

                        default:
                            break;
                    }

                    if (archiveType != ArchiveType.GZip)
                    {
                        var FilesPicker = new FileOpenPicker();
                        FilesPicker.CommitButtonText = "Compress";
                        FilesPicker.FileTypeFilter.Add("*");
                        FilesPicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;

                        var OC_selectedFiles = await FilesPicker.PickMultipleFilesAsync();

                        if (OC_selectedFiles.Count != 0)
                        {
                            if (archiveType != ArchiveType.Tar)
                            {
                                using (var zipWriter = WriterFactory.Open(zipStream, archiveType, CompressionType.Deflate))  //(zip, archiveType, CompressionType.None))
                                {
                                    foreach (var singleFile in OC_selectedFiles)
                                    {

                                        zipWriter.Write(singleFile.Name, await singleFile.OpenStreamForReadAsync());

                                        //zipWriter.Write(singleFile.Name, singleFile.OpenStreamForReadAsync);
                                        //zipWriter.Write(Path.GetFileName(file), filePath);
                                    }
                                }
                            }
                            else if (archiveType == ArchiveType.Tar)
                            {
                                using (var tarWriter = WriterFactory.Open(zipStream, archiveType, CompressionType.None))
                                {
                                    foreach (var singleFile in OC_selectedFiles)
                                    {
                                        tarWriter.Write(singleFile.Name, await singleFile.OpenStreamForReadAsync());
                                    }
                                }
                            }

                            isBusy.IsBusy = false;
                            this.Frame.Navigate(typeof(HomePage));
                        }
                    }
                    else if (archiveType == ArchiveType.GZip)
                    {
                        var filePicker = new FileOpenPicker();
                        filePicker.CommitButtonText = "Compress";
                        filePicker.FileTypeFilter.Add("*");
                        filePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;

                        var selectedFile = await filePicker.PickSingleFileAsync();

                        if (selectedFile != null)
                        {
                            using (GZipWriter writer = new GZipWriter(zipStream))
                            {
                                writer.Write(selectedFile.Name, await selectedFile.OpenStreamForReadAsync());
                            }

                            isBusy.IsBusy = false;
                            this.Frame.Navigate(typeof(HomePage));
                        }

                        #region commented - tries
                        //using (var gzipWriter = GZipWriter)
                        //{

                        //}

                        //using (var gzipArchive = GZipArchive.Open(zip))
                        //{
                        //    gzipArchive.AddEntry(selectedFile.Path, await selectedFile.OpenStreamForReadAsync());
                        //    //gzipArchive.SaveTo(await selectedFile.OpenStreamForReadAsync());
                        //    //gzipArchive.AddEntry(selectedFile.Path, await selectedFile.OpenStreamForReadAsync());
                        //    //gzipArchive.AddEntry(
                        //}
                        #endregion
                    }
                }
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

        private async void Btn_AddFolder_Tapped(object sender, TappedRoutedEventArgs e)
        {
            isBusy.IsBusy = true;

            try
            {
                using (var zip = await ZipToWrite.OpenStreamForWriteAsync())
                {
                    var type = ZipToWrite.FileType;

                    switch (type)
                    {
                        case ".zip":
                            archiveType = ArchiveType.Zip;
                            break;

                        case ".gz":
                            archiveType = ArchiveType.GZip;
                            break;

                        case ".bz2":
                            archiveType = ArchiveType.Zip;
                            break;

                        case ".tar":
                            archiveType = ArchiveType.Tar;
                            break;

                        default:
                            break;
                    }

                    if (archiveType != ArchiveType.GZip)
                    {
                        var folderPicker = new FolderPicker();
                        folderPicker.CommitButtonText = "select folder";
                        folderPicker.FileTypeFilter.Add("*");
                        folderPicker.SuggestedStartLocation = PickerLocationId.ComputerFolder;

                        var selectedFolder = await folderPicker.PickSingleFolderAsync();

                        if (selectedFolder != null)
                        {
                            string[] splitedPathOfFolder = selectedFolder.Path.Split('\\');
                            int folderNameLenght = splitedPathOfFolder.Last().Length;
                            int selectedFolderPathLenght = selectedFolder.Path.Length;
                            int substringLenght = selectedFolderPathLenght - folderNameLenght;

                            if (archiveType != ArchiveType.Tar)
                            {
                                using (var zipWriter = WriterFactory.Open(zip, archiveType, CompressionType.Deflate))  //(zip, archiveType, CompressionType.None))
                                {
                                    foreach (var singleFile in await selectedFolder.GetFilesAsync(CommonFileQuery.OrderByName))
                                    {
                                        string subStringToConsider = singleFile.Path.Substring(substringLenght - 1);

                                        zipWriter.Write(subStringToConsider, await singleFile.OpenStreamForReadAsync());
                                        //zipWriter.Write(singleFile.Name, singleFile.OpenStreamForReadAsync);
                                        //zipWriter.Write(Path.GetFileName(file), filePath);
                                    }
                                }
                            }
                            else if (archiveType == ArchiveType.Tar)
                            {
                                using (var zipWriter = WriterFactory.Open(zip, archiveType, CompressionType.None))  //(zip, archiveType, CompressionType.None))
                                {
                                    foreach (var singleFile in await selectedFolder.GetFilesAsync(CommonFileQuery.OrderByName))
                                    {
                                        string subStringToConsider = singleFile.Path.Substring(substringLenght - 1);

                                        zipWriter.Write(subStringToConsider, await singleFile.OpenStreamForReadAsync());
                                        //zipWriter.Write(singleFile.Name, singleFile.OpenStreamForReadAsync);
                                        //zipWriter.Write(Path.GetFileName(file), filePath);
                                    }
                                }
                            }

                            isBusy.IsBusy = false;
                            this.Frame.Navigate(typeof(HomePage));
                        }
                    }

                    #region GZip can not contain folder - commented
                    //else if (archiveType == ArchiveType.GZip)
                    //{
                    //    var filePicker = new FileOpenPicker();
                    //    filePicker.CommitButtonText = "Compress";
                    //    filePicker.FileTypeFilter.Add("*");
                    //    filePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;

                    //    var selectedFile = await filePicker.PickSingleFileAsync();

                    //    using (GZipWriter writer = new GZipWriter(zip))
                    //    {
                    //        writer.Write(selectedFile.Name, await selectedFile.OpenStreamForReadAsync());
                    //    }
                    //}
                    #endregion
                }
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

    }
}
