using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;
using Prism.Mvvm;

namespace Sample.Models
{
    public class DataItem : BindableBase
    {
        public DataItem()
        {
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                Title = "Sample item";
                Text = "The quick brown fox jumps over the lazy dog.";
                Image = "ms-appx:///Images/pears.jpg";
            }
        }

        public int Id { get; }

        private string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private string _text;
        public string Text
        {
            get => _text;
            set => SetProperty(ref _text, value);
        }

        private string _image;
        public string Image
        {
            get => _image;
            set => SetProperty(ref _image, value);
        }
    }
}