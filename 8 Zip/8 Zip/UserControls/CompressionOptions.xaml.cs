using SharpCompress.Common;
using SharpCompress.Compressor.Deflate;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace _8_Zip.UserControls
{
    public sealed partial class CompressionOptions : UserControl
    {
        public CompressionOptions()
        {
            this.InitializeComponent();
        }

        private void cb_compressionMethod_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cb_compressionMethod.SelectedItem != null)
                {
                    switch (cb_compressionMethod.SelectedItem.ToString())
                    {
                        case "Deflate":
                            App.selectedMethod = CompressionType.Deflate;
                            break;

                        case "RAR":
                            App.selectedMethod = CompressionType.Rar;
                            break;

                        case "BZip2":
                            App.selectedMethod = CompressionType.BZip2;
                            break;

                        case "GZip":
                            App.selectedMethod = CompressionType.GZip;
                            break;

                        case "LZMA":
                            App.selectedMethod = CompressionType.LZMA;
                            break;

                        case "BCJ":
                            App.selectedMethod = CompressionType.BCJ;
                            break;

                        case "BCJ2":
                            App.selectedMethod = CompressionType.BCJ2;
                            break;

                        case "PPMD":
                            App.selectedMethod = CompressionType.PPMd;
                            break;

                        default:
                            break;
                    }
                }
            }
            catch (System.NullReferenceException nrex)
            {

            }
            catch (Exception ex)
            {
                MessageDialog ms = new MessageDialog(ex.Message);
                ms.ShowAsync();
            }
            
        }

        private void cb_compressionLevel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cb_compressionLevel.SelectedItem != null)
                {
                    switch (cb_compressionLevel.SelectedItem.ToString())
                    {
                        case "Default":
                            App.selectedLevel = CompressionLevel.Default;
                            break;

                        case "Best Compression":
                            App.selectedLevel = CompressionLevel.BestCompression;
                            break;

                        case "Best Speed":
                            App.selectedLevel = CompressionLevel.BestSpeed;
                            break;

                        case "None":
                            App.selectedLevel = CompressionLevel.None;
                            break;

                        default:
                            break;
                    }
                }
            }
            catch (System.NullReferenceException nrex)
            {

            }
            catch (Exception ex)
            {
                MessageDialog ms = new MessageDialog(ex.Message);
                ms.ShowAsync();
            }
        }
    }
}
