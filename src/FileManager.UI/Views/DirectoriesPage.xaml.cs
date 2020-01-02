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

            DirectoryLocation = Path.Combine(DirectoryLocation, dir ?? string.Empty);
            
            var directoryInfo = new DirectoryInfo(DirectoryLocation);
            var subDirectories = directoryInfo.GetDirectories().Where(w => w.Attributes == FileAttributes.Directory);

            foreach (var subDirectoryInfo in subDirectories)
            {
                var directory = new DirectoryInformation { Name = subDirectoryInfo.Name, Type = 1, Location = DirectoryLocation };
                Directories.Add(directory);
            }

            directoriesList.ItemsSource = null;
            directoriesList.ItemsSource = Directories;
        }

        private static readonly string RootDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);

        public string DirectoryLocation { get; set; } = RootDirectory;
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

                //File.WriteAllBytes(Path.Combine(DirectoryLocation, fileData.FileName), fileData.DataArray); // TODO: test with large files

                var fileName = fileData.FileName;
                
                try
                {
                    var file = Path.Combine(DirectoryLocation, fileData.FileName);
                    using (var stream = File.Create(file))
                    {
                        using (var input = fileData.GetStream())
                        {
                            input.CopyTo(stream);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"File data: {ex}");
                }
                // CrossFilePicker.Current.sa

                var contents = Encoding.UTF8.GetString(fileData.DataArray);

                Console.WriteLine("File name chosen: " + fileName);
                Console.WriteLine("File data: " + contents);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception choosing file: " + ex);
            }
        }

        private void OnDirectorySelected_(object sender, EventArgs e)
        {
            var item = (DirectoryInformation)(((TappedEventArgs)e).Parameter);
            
            Initialize(item.Name);
        }

        private void OnBackClicked(object sender, EventArgs e)
        {
            if (string.Equals(DirectoryLocation, RootDirectory))
            {
                return;
            }
            
            var parent = Directory.GetParent(DirectoryLocation);
            DirectoryLocation = parent.FullName;

            Initialize();
        }
    }
}