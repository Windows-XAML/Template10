using System;
using System.Collections.ObjectModel;
using System.Linq;
using Sample.Models;
using Template10.Utils;

namespace Sample.Services.MessageService
{
    public partial class MessageService
    {
        private static ObservableCollection<Message> _messages;
        private readonly Random _random = new Random((int) DateTime.Now.Ticks);

        public ObservableCollection<Message> GetMessages()
        {
            if (_messages != null)
                return _messages;
            _messages = new ObservableCollection<Message>();
            foreach (var item in SampleData())
            {
                _messages.Add(item);
            }
            return _messages;
        }

        public ObservableCollection<Message> Search(string value) => GetMessages()
            .Where(x => x.Subject.ToLower().Contains(value?.ToLower() ?? string.Empty)
                        || x.From.ToLower().Contains(value?.ToLower() ?? string.Empty)
                        || x.Body.ToLower().Contains(value?.ToLower() ?? string.Empty))
            .ToObservableCollection();

        public void DeleteMessage(Message selected)
        {
            GetMessages().Remove(selected);
        }

        public Message GetMessage(string id) => GetMessages().FirstOrDefault(x => x.Id.Equals(id));
    }
}