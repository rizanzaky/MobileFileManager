using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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

            Initialize();
        }

        private void Initialize()
        {
            var documents = Environment.GetFolderPath(Environment.SpecialFolder.Resources);
            var directoryname = Path.Combine(documents, "Acrobat");

            var directories = Directory.GetDirectories(documents);

            foreach (var document in directories)
            {
                DirectoryStrings += $"{document}\n";
            }
        }

        public string DirectoryStrings { get; set; }
        public List<string> Directories => new List<string>{"One", "Two", "Three", "Four", "Five"};
    }
}