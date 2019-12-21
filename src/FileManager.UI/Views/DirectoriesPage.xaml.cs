using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using Plugin.FilePicker;
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
            var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var directoryname = Path.Combine(documents, "Acrobat");
            var directories = Directory.GetDirectories(documents);

            foreach (var document in directories)
            {
                DirectoryStrings += $"{document}\n";
            }
        }

        public string DirectoryStrings { get; set; }
        public List<string> Directories => new List<string>{"One", "Two", "Three", "Four", "Five"};

        private async void OnPickerClicked(object sender, EventArgs e)
        {
            try
            {
                var fileData = await CrossFilePicker.Current.PickFile();
                if (fileData == null)
                    return; // user canceled file picking

                var fileName = fileData.FileName;
                var contents = Encoding.UTF8.GetString(fileData.DataArray);

                Console.WriteLine("File name chosen: " + fileName);
                Console.WriteLine("File data: " + contents);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception choosing file: " + ex);
            }
        }
    }
}