using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using FileManager.Core.Models;
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

        private void Initialize(string dir = null)
        {
            Directories.Clear();

            var root = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            DirectoryLocation = Path.Combine(root, dir ?? string.Empty);
            var directories = Directory.GetDirectories(DirectoryLocation);

            foreach (var document in directories)
            {
                var doc = document.Split('/').LastOrDefault();
                if (string.IsNullOrEmpty(doc))
                {
                    continue;
                }
                var directory = new DirectoryInformation { Name = doc, Type = 1, Location = Environment.GetFolderPath(Environment.SpecialFolder.Personal) };
                Directories.Add(directory);
            }

            directoriesList.ItemsSource = null;
            directoriesList.ItemsSource = Directories;
        }

        public string DirectoryLocation { get; set; } = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        public List<DirectoryInformation> Directories { get; } = new List<DirectoryInformation>();

        private void OnReloadClicked(object sender, EventArgs e)
        {
            Initialize();
        }

        private void OnFolderCreateClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(directoryNameStr.Text))
            {
                return;
            }

            var newDirectory = Path.Combine(DirectoryLocation, directoryNameStr.Text);
            var created = Directory.CreateDirectory(newDirectory);
            directoryNameStr.Text = string.Empty;

            

            Initialize();
        }

        private async void OnPickerClicked(object sender, EventArgs e)
        {
            try
            {
                var fileData = await CrossFilePicker.Current.PickFile();
                if (fileData == null)
                    return; // user canceled file picking

                var fileName = fileData.FileName;

                //
                //var x = fileData.FilePath;
                //var path = x.Replace($"{fileData.FileName}", "");
                //var newDirectory = Path.Combine(path, "Hello");
                //var created = Directory.CreateDirectory(newDirectory);
                //

                var contents = Encoding.UTF8.GetString(fileData.DataArray);

                Console.WriteLine("File name chosen: " + fileName);
                Console.WriteLine("File data: " + contents);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception choosing file: " + ex);
            }
        }

        private void OnDirectorySelected(object sender, SelectionChangedEventArgs e)
        {
            var item = (DirectoryInformation)e.CurrentSelection[0];

            Initialize(item.Name);
        }

        private void OnDirectorySelected_(object sender, EventArgs e)
        {
            var item = (DirectoryInformation)(((TappedEventArgs)e).Parameter);
            
            Initialize(item.Name);
        }
    }
}