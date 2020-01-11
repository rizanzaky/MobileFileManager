using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using FileManager.Core.Models;
using MediaManager;
using MediaManager.Forms;
using MediaManager.Library;
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
            Files.Clear();

            DirectoryLocation = Path.Combine(DirectoryLocation, dir ?? string.Empty);
            
            var directoryInfo = new DirectoryInfo(DirectoryLocation);

            var a = directoryInfo.GetDirectories();
            var b = directoryInfo.GetFiles();

            var subDirectories = a.Where(w => w.Attributes == FileAttributes.Directory);

            foreach (var subDirectoryInfo in subDirectories)
            {
                var directory = new DirectoryInformation { Name = subDirectoryInfo.Name, Type = 1, Location = DirectoryLocation };
                Directories.Add(directory);
            }

            foreach (var file in b)
            {
                var type = 2;

                var extensions = new[] {".mov", ".mp4", ".avi", ".wmv"};
                var isVideo = extensions.Contains(file.Extension.ToLower());
                
                if (isVideo)
                {
                    type = 3;
                }
                
                var image = new DirectoryInformation { Name = file.Name, Type = type, Location = DirectoryLocation, FullName = file.FullName };
                Directories.Add(image);
                Files.Add(image);
            }

            directoriesList.ItemsSource = null;
            directoriesList.ItemsSource = Directories;
        }

        private static readonly string RootDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);

        public string DirectoryLocation { get; set; } = RootDirectory;
        public List<DirectoryInformation> Directories { get; } = new List<DirectoryInformation>();
        public List<DirectoryInformation> Files { get; } = new List<DirectoryInformation>();

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
                
                Initialize();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception choosing file: " + ex);
            }
        }

        private void OnDirectorySelected(object sender, EventArgs e)
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

        private async void OnImageSelected(object sender, EventArgs e)
        {
            var image = (DirectoryInformation)(((TappedEventArgs)e).Parameter);

            // if (image.Type == 3)
            // {
            //     try
            //     {
            //         // mediaPlayer.Source = null;
            //         // mediaPlayer.Source = image.FullName;
            //
            //         // CrossMediaManager.Current.MediaPlayer.VideoView =
            //         // CrossMediaManager.Current.MediaPlayer.AutoAttachVideoView = false;
            //         
            //         await CrossMediaManager.Current.Play(image.FullName);
            //     }
            //     catch (Exception exception)
            //     {
            //         Console.WriteLine(exception);
            //     }
            //     //return;
            // }

            var imageViewer = new ImageViewerPage();
            
            imageViewer.DataSource.Clear();
            imageViewer.DataSource.AddRange(Files);

            await Navigation.PushAsync(imageViewer);
        }
    }
}