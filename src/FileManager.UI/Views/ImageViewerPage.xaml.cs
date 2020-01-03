using System;
using System.Collections.Generic;
using FileManager.Core.Models;
using Xamarin.Forms;

namespace FileManager.UI.Views
{
    public partial class ImageViewerPage : ContentPage
    {
        public ImageViewerPage()
        {
            InitializeComponent();
        }
        
        public List<DirectoryInformation> DataSource { get; } = new List<DirectoryInformation>();
    }
}
