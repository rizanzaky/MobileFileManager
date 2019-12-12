using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;

namespace FileManager.UI.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class DirectoryPage : ContentPage
    {
        public DirectoryPage()
        {
            InitializeComponent();
        }

        public List<string> Directories => new List<string>{"One", "Two", "Three", "Four", "Five"};
    }
}