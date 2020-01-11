using System;
using FileManager.Core.Models;
using Xamarin.Forms;

namespace FileManager.UI.Common.TemplateSelectors
{
    public class MediaViewSelector : DataTemplateSelector
    {
        public DataTemplate ImageViewTemplate { get; set; }
        public DataTemplate VideoViewTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            return ((DirectoryInformation)item).Type == 2 ? ImageViewTemplate : VideoViewTemplate;
        }
    }
}
