using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.VoiceCommands;
using Windows.Storage;

namespace Template10.Samples.CortanaSample
{
    sealed partial class App : Template10.Common.BootStrapper
    {
        public App()
        {
            InitializeComponent();
        }

        public override async Task OnInitializeAsync(IActivatedEventArgs args)
        {
            var url = new Uri("ms-appx:///Cortana.xml");
            var file = await StorageFile.GetFileFromApplicationUriAsync(url);
            await VoiceCommandDefinitionManager.InstallCommandDefinitionsFromStorageFileAsync(file);
        }

        public override Task OnStartAsync(StartKind startKind, IActivatedEventArgs args)
        {
            var e = args as VoiceCommandActivatedEventArgs;
            if (e != null)
            {
                var result = e.Result;
                var properties = result.SemanticInterpretation.Properties
                    .ToDictionary(x => x.Key, x => x.Value);

                var command = result.RulePath.First();
                if (command.Equals("FreeTextCommand"))
                {
                    // get spoken text
                    var text = properties.First(x => x.Key.Equals("textPhrase")).Value[0];

                    // remember to handle response appropriately
                    var mode = properties.First(x => x.Key.Equals("commandMode")).Value;
                    if (mode.Equals("voice")) { /* okay to speak */ }
                    else { /* not okay to speak */ }

                    // update value
                    if (ViewModels.MainPageViewModel.Instance == null)
                        NavigationService.Navigate(typeof(Views.MainPage), text);
                    else
                        ViewModels.MainPageViewModel.Instance.Value = text;
                }
                else { /* unexpected command */ }
            }
            else
            {
                NavigationService.Navigate(typeof(Views.MainPage));
            }
            return Task.CompletedTask;
        }
    }
}
