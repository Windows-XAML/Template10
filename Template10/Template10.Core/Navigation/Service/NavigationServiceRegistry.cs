using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml.Controls;

namespace Template10.Navigation
{
    public class NavigationServiceRegistry
    {
        List<INavigationService> _list = new List<INavigationService>();

        internal NavigationServiceRegistry() { }

        public void Register(INavigationService service)
        {
            if (_list.Contains(service))
            {
                throw new Exception("NavigationService is already registered.");
            }
            _list.Add(service);
        }

        public void UnRegister(INavigationService service)
        {
            if (!_list.Contains(service))
            {
                throw new Exception("NavigationService is not registered.");
            }
            _list.Remove(service);
        }

        public bool Any() => _list.Any();

        public int Count => _list.Count;

        public INavigationService GetByFrameId(string frameId)
            => _list.FirstOrDefault(x => x.FrameEx.FrameId == frameId);

        public INavigationService GetByFrameFacade(IFrameEx frame)
            => _list.FirstOrDefault(x => x.FrameEx.Equals(frame));

        public INavigationService GetByFrame(Frame frame)
            => _list.FirstOrDefault(x => (x.FrameEx as IFrameEx2).Frame.Equals(frame));

        public IEnumerable<T> Select<T>(Func<INavigationService, T> predicate)
        {
            return _list.Select(predicate);
        }
    }
}
