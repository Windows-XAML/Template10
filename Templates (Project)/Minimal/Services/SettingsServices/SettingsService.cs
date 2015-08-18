using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template10.Services.SettingsServices
{
    public class SettingsService
    {
        Template10.Services.SettingsService.SettingsHelper _helper;

        public SettingsService()
        {
            _helper = new Template10.Services.SettingsService.SettingsHelper();
        }

        public bool Something
        {
            get { return _helper.Read<bool>(nameof(Something), false); }
            set { _helper.Write(nameof(Something), value); }
        }
    }
}
