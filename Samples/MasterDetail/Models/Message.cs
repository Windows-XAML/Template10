using System;
using Template10.Mvvm;

namespace Samples.MasterDetail.Models
{
   public class Message : BindableBase
    {
        string _Id = default(string);
        public string Id { get { return _Id; } set { Set(ref _Id, value); } }

        string _Subject = default(string);
        public string Subject { get { return _Subject; } set { Set(ref _Subject, value); } }

        string _Body = default(string);
        public string Body { get { return _Body; } set { Set(ref _Body, value); } }

        string _From = default(string);
        public string From { get { return _From; } set { Set(ref _From, value); } }

        string _To = default(string);
        public string To { get { return _To; } set { Set(ref _To, value); } }

        DateTime _Date = default(DateTime);
        public DateTime Date { get { return _Date; } set { Set(ref _Date, value); } }

        bool _IsRead = false;
        public bool IsRead { get { return _IsRead; } set { Set(ref _IsRead, value); } }
    }
}
