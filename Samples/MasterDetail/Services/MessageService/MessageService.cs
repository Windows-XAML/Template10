using System;
using System.Collections.ObjectModel;
using System.Linq;
using Sample.Models;
using Template10.Utils;

namespace Sample.Services.MessageService
{
    public partial class MessageService
    {
        Random _random = new Random((int)DateTime.Now.Ticks);
        static ObservableCollection<Models.Message> _messages;
        public ObservableCollection<Models.Message> GetMessages()
        {
            if (_messages != null)
                return _messages;
            _messages = new ObservableCollection<Models.Message>();
            foreach (var item in SampleData())
            {
                _messages.Add(item);
            }
            return _messages;
        }

        public ObservableCollection<Models.Message> Search(string value)
        {
            return GetMessages()
                .Where(x => x.Subject.ToLower().Contains(value.ToLower())
                || x.From.ToLower().Contains(value.ToLower())
                || x.Body.ToLower().Contains(value.ToLower()))
                .ToObservableCollection();
        }

        public void DeleteMessage(Message selected)
        {
            GetMessages().Remove(selected);
        }

        public Models.Message GetMessage(string id)
        {
            return GetMessages().FirstOrDefault(x => x.Id.Equals(id));
        }
    }
}
