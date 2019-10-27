using Newtonsoft.Json;
using Template10.SampleData.StarTrek;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;

namespace Template10.SampleData.Food
{
    public class Database
    {
        public Fruit[] Fruit { get; private set; }

        public bool Open { get; private set; } = false;

        public async Task<bool> OpenAsync()
        {
            if (Open)
            {
                return Open;
            }

            var root = await ReadJson();

            foreach (var fruit in Fruit = root.Fruit)
            {
                fruit.Image = UpdateImagePaths(fruit.Images);
            }

            return Open = true;

            Image UpdateImagePaths(params Image[] images)
            {
                if (images != null)
                {
                    foreach (var image in images)
                    {
                        image.Path = $"ms-appx:///SampleData/Food/Images/{image.Path}";
                    }
                    return images.FirstOrDefault();
                }
                else
                {
                    return null;
                }
            }

            async Task<JsonRoot> ReadJson()
            {
                try
                {
                    var path = new Uri("ms-appx:///SampleData/Food/Data.json");
                    var file = await StorageFile.GetFileFromApplicationUriAsync(path);
                    var json = await FileIO.ReadTextAsync(file);
                    return JsonConvert.DeserializeObject<JsonRoot>(json);
                }
                catch (Exception ex)
                {
                    Debugger.Break();
                    throw;
                }
            }
        }
    }
}
