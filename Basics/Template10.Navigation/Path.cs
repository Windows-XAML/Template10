using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Template10.Navigation
{
    public class Path
    {
        public Path(string page, bool clear)
        {
            Page = page;
            Clear = clear;
        }

        public Path(string page, bool clear, params (string Name, string Value)[] parameters)
        {
            Page = page;
            Clear = clear;
            AddParameter(parameters);
        }

        private bool Clear { get; set; }

        private Path Parent { get; set; }

        private Path Child { get; set; }

        public string Page { get; }

        public IEnumerable<(string Name, string Value)> Parameters => _parameters.Select(x => x).ToList().AsReadOnly();

        List<(string Name, string Value)> _parameters = new List<(string Name, string Value)>();

        public Path AddParameter(params (string Name, string Value)[] parameters)
        {
            _parameters.AddRange(parameters);
            return this;
        }

        public Path AddPage(string page)
        {
            return (Child = new Path(page, false) { Parent = this });
        }

        public override string ToString()
        {
            var page = this;
            while (page.Parent != null)
            {
                page = page.Parent;
            }
            var sb = new StringBuilder();
            if (page.Clear)
            {
                sb.Append("/");
            }
            Append(page, ref sb);
            while (page.Child != null)
            {
                page = page.Child;
                sb.Append("/");
                Append(page, ref sb);
            }
            return sb.ToString();
        }

        private void Append(Path path, ref StringBuilder sb)
        {
            sb.Append(path.Page ?? "null");
            if (path._parameters.Any())
            {
                sb.Append("?");
                foreach (var item in path._parameters)
                {
                    var name = WebUtility.UrlEncode(item.Name);
                    var value = WebUtility.UrlEncode(item.Value);
                    sb.Append($"{name}={value}&");
                }
            }
        }
    }
}
