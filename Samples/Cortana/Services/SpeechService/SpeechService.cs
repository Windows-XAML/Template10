using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.Media.SpeechRecognition;
using Windows.Media.SpeechSynthesis;
using Windows.UI.Xaml.Controls;

namespace Template10.Samples.CortanaSample.Services.SpeechService
{
    class SpeechService : IDisposable
    {
        SpeechRecognizer _SpeechRecognizer;

        public async Task LoadRecognizerAsync()
        {
            var permission = await Template10.Utils.AudioUtils.RequestMicrophonePermission();
            if (permission && _SpeechRecognizer == null)
            {
                _SpeechRecognizer = new SpeechRecognizer(SpeechRecognizer.SystemSpeechLanguage);
                var constraint = new SpeechRecognitionTopicConstraint(SpeechRecognitionScenario.Dictation, "dictation");
                _SpeechRecognizer.Constraints.Add(constraint);
                var compilation = await _SpeechRecognizer.CompileConstraintsAsync();
                if (compilation.Status != SpeechRecognitionResultStatus.Success)
                    throw new Exception(compilation.Status.ToString());
            }
            else if (!permission)
            {
                throw new Exception("RequestMicrophonePermission returned false");
            }
        }

        public async Task<string> ListenAsync(string prompt, string example)
        {
            if (_SpeechRecognizer == null)
                await LoadRecognizerAsync();
            try
            {
                _SpeechRecognizer.UIOptions.ShowConfirmation = false;
                _SpeechRecognizer.UIOptions.AudiblePrompt = prompt;
                _SpeechRecognizer.UIOptions.ExampleText = example;
                var result = await _SpeechRecognizer.RecognizeWithUIAsync();
                switch (result.Status)
                {
                    case SpeechRecognitionResultStatus.Success:
                        return result.Text;
                    case SpeechRecognitionResultStatus.UserCanceled:
                        return string.Empty;
                    default:
                        throw new Exception("Speech recognition failed. Status: " + result.Status.ToString());
                }
            }
            catch (TaskCanceledException e) { throw new Exception("Cancelled", e); }
            catch (Exception e) when (e.HResult.Equals(0x80045509)) { throw new Exception("Disabled in settings", e); }
            catch { throw; }
        }

        public async Task SpeakAsync(string text)
        {
            var voice = SpeechSynthesizer.AllVoices
                .First(x => x.Gender.Equals(VoiceGender.Female) && x.Description.Contains("United States"));
            using (var speech = new SpeechSynthesizer { Voice = voice })
            {
                text = string.IsNullOrWhiteSpace(text) ? "There is no text to speak." : text;
                var stream = await speech.SynthesizeTextToStreamAsync(text);

                var media = new MediaElement { AutoPlay = true };
                media.SetSource(stream, stream.ContentType);
            }
        }

        public async void Dispose()
        {
            if (_SpeechRecognizer != null)
            {
                while (_SpeechRecognizer.State != SpeechRecognizerState.Idle)
                {
                    await Task.Delay(10);
                }
                _SpeechRecognizer.Dispose();
                _SpeechRecognizer = null;
            }
        }
    }
}
