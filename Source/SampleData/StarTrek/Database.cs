using Newtonsoft.Json;
using SampleData.Data.StarTrek;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;

namespace SampleData
{
    public class Database
    {
        public Member[] Members { get; private set; }

        public string[] Species { get; private set; }

        public string[] Genders { get; private set; }

        public Ship[] Ships { get; private set; }

        public Show[] Shows { get; private set; }

        public bool Open { get; private set; } = false;

        public async Task<bool> OpenAsync()
        {
            if (Open)
            {
                return Open;
            }

            var root = await ReadJson();

            Species = root.Members.Select(x => x.Species).Distinct().OrderBy(x => x).ToArray();

            Genders = root.Members.Select(x => x.Gender).Distinct().OrderBy(x => x).ToArray();

            foreach (var ship in Ships = root.Ships)
            {
                UpdateImagePaths(ship.Show, ship.Images);
                ship.Image = ship.Images.First();
            }

            foreach (var member in Members = root.Members)
            {
                UpdateImagePaths(member.Show, member.Images);
                member.Image = member.Images.First();
            }

            foreach (var show in Shows = root.Shows)
            {
                UpdateImagePaths(show.Abbreviation, show.Images);
                show.Image = Ships.First(x => x.Show == show.Abbreviation).Images.First();
            }

            return Open = true;

            Image UpdateImagePaths(string show, params Image[] images)
            {
                foreach (var image in images)
                {
                    image.Path = $"ms-appx:///SampleData/StarTrek/Images/{show}/{image.Path}";
                }
                return images.FirstOrDefault();
            }

            async Task<JsonRoot> ReadJson()
            {
                var path = new Uri("ms-appx:///SampleData/StarTrek/Data.json");
                var file = await StorageFile.GetFileFromApplicationUriAsync(path);
                var json = await FileIO.ReadTextAsync(file);
                return JsonConvert.DeserializeObject<JsonRoot>(json);
            }
        }
    }
}
