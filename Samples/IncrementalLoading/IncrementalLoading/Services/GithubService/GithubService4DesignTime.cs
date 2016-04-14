using Template10.Samples.IncrementalLoadingSample.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Template10.Samples.IncrementalLoadingSample.Services.GithubService
{
    public class GithubService4DesignTime : IGithubService
    {
        private static string[] _fakeAuthorNames = new string[] { "jerry", "andy", "bill_gates" };

        private static string[] _fakeHomeUrls = new string[] { "http://www.bing.com", "https://dev.window.com", "https://github.com/Windows-XAML/Template10" };

        private static string[] _fakeLanguages = new string[] { "C#", "TypeScript", "VB.net" };

        private static string[] _fakeProjectNames = new string[] { "Template10", "Win2D" };

        private static Random _rand = new Random();

        public Task<GithubQueryResult<Repository>> GetRepositoriesAsync(string query, int pageIndex = 1)
        {
            List<Repository> fakeRepositories = new List<Repository>();
            for (int i = 0; i < 10; i++)
            {
                fakeRepositories.Add(new Repository()
                {
                    Id = _rand.Next(),
                    FullName = string.Format("{0}/{1}", TakeRandom(_fakeAuthorNames), TakeRandom(_fakeProjectNames)),
                    Language = TakeRandom(_fakeLanguages),
                    Description = "This is a fake item for design mode",
                    HomeUrl = TakeRandom(_fakeHomeUrls),
                    Star = _rand.Next(0xFF),
                    UpdatedAt = DateTime.Now
                });
            }
            GithubQueryResult<Repository> fakeQueryResult = new GithubQueryResult<Repository>()
            {
                Items = fakeRepositories.ToArray()
            };
            return Task.FromResult(fakeQueryResult);
        }

        private static T TakeRandom<T>(T[] array)
        {
            int length = array.Length;
            return array[_rand.Next(length)];
        }
    }
}