using System;
using FileManager.Core.Models;
using Xamarin.Forms;

namespace FileManager.UI.Common.TemplateSelectors
{
    public class FileDirectorySelector : DataTemplateSelector
    {
        public DataTemplate DirectoryTemplate { get; set; }
        public DataTemplate FileTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            return ((DirectoryInformation)item).Type == 1 ? DirectoryTemplate : FileTemplate;
        }
    }
}
