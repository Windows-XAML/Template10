using Microsoft.Advertising.WinRT.UI;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Template10.Services.AdService
{
    public class AdHelper
    {
        private string _appId;
        private string _unitId;
        private bool _busy = false;
        private static bool _shown = false;
        private InterstitialAd _ad = new InterstitialAd();

        public AdHelper(string appId, string unitId)
        {
            _appId = appId;
            _unitId = unitId;
        }

        public enum Results { Complete, Canceled, Error };

        public void Preload()
        {
            _ad.RequestAd(AdType.Video, _appId, _unitId);
        }

        public void Hide()
        {
            _ad.Close();
        }

        public void Show(Action<Results> callback, bool allowRepeat = false)
        {
            if (_busy) { return; }
            else { _busy = true; }

            // allow repeat
            if (!allowRepeat && _shown)
            {
                callback(Results.Complete);
                return;
            }

            // setup
            _ad.Completed += (s, e) => { callback(Results.Complete); };
            _ad.Cancelled += (s, e) => { callback(Results.Canceled); };
            _ad.ErrorOccurred += (s, e) => { callback(Results.Error); };
            _ad.AdReady += (s, e) => { _shown = true; _ad.Show(); };

            // invoke
            if (_ad.State == InterstitialAdState.Ready) { _shown = true; _ad.Show(); }
            else { Preload(); }
        }

    }
}
