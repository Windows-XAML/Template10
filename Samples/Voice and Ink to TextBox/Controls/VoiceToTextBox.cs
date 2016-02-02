using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.SpeechRecognition;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MPC.Controls
{
    [TemplatePart(Name = PART_ROOT_NAME, Type = typeof(StackPanel))]
    [TemplatePart(Name = PART_TEXT_NAME, Type = typeof(TextBox))]
    [TemplatePart(Name = PART_BUTTON_NAME, Type = typeof(Button))]
    public class VoiceToTextBox : Control
    {
        #region private fields
        private const string PART_ROOT_NAME = "PART_ROOT";
        private const string PART_TEXT_NAME = "PART_TEXT";
        private const string PART_BUTTON_NAME = "PART_BUTTON";  
        private TextBox textBox;
        private Button button;
        private SpeechRecognizer speechRecognizer;
        private CoreDispatcher dispatcher;
        #endregion

        #region public fields
        // Text
        public static readonly DependencyProperty TextProperty = 
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(VoiceToTextBox), new PropertyMetadata(DependencyProperty.UnsetValue));
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty PlaceholderTextProperty = 
            DependencyProperty.Register(nameof(PlaceholderText), typeof(string), typeof(VoiceToTextBox), new PropertyMetadata("Type or Speech"));
        public string PlaceholderText
        {
            get { return (string)GetValue(PlaceholderTextProperty); }
            set { SetValue(PlaceholderTextProperty, value); }
        }
        #endregion

        #region ctor
        public VoiceToTextBox()
        {
            this.DefaultStyleKey = typeof(VoiceToTextBox);
        }
        #endregion

        #region override OnApplyTemplate
        protected override void OnApplyTemplate()
        { 
            textBox = GetTemplateChild(PART_TEXT_NAME) as TextBox;
            button = GetTemplateChild(PART_BUTTON_NAME) as Button;
            InitEvents();
            
        }
        #endregion

        #region private methods
        private void InitEvents()
        {
            if (button != null)
                button.Click += Button_Click;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            await InitSpeech();

            VisualStateManager.GoToState(this, "Listening", true);

            try
            {
                SpeechRecognitionResult speechRecognitionResult = await speechRecognizer.RecognizeAsync();
                if (speechRecognitionResult.Status == SpeechRecognitionResultStatus.Success)
                {
                    Text = speechRecognitionResult.Text;
                }
                else
                {
                    Text = string.Format("Speech Recognition Failed, Status: {0}", speechRecognitionResult.Status.ToString());
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                Text = string.Format("Speech Recognition Failed");
            }
            finally
            {
                VisualStateManager.GoToState(this, "NotListening", true);
            }
        }

        private async Task InitSpeech()
        {
            dispatcher = CoreWindow.GetForCurrentThread().Dispatcher;

            bool permissionGained = await Template10.Utils.AudioUtils.RequestMicrophonePermission();
            if (permissionGained)
            {
                button.IsEnabled = true;

                if (speechRecognizer != null)
                {
                    this.speechRecognizer.Dispose();
                    this.speechRecognizer = null;
                }

                speechRecognizer = new SpeechRecognizer();

                var dictationConstraint = new SpeechRecognitionTopicConstraint(SpeechRecognitionScenario.Dictation, "dictation");
                speechRecognizer.Constraints.Add(dictationConstraint);
                SpeechRecognitionCompilationResult compilationResult = await speechRecognizer.CompileConstraintsAsync();

                speechRecognizer.HypothesisGenerated += SpeechRecognizer_HypothesisGenerated;

                if (compilationResult.Status != SpeechRecognitionResultStatus.Success)
                    button.IsEnabled = false;

            }
            else
            {
                Text = string.Format("Permission to access mic denied by the user");
                button.IsEnabled = false;
            }

            await Task.Yield();
        }

        private async void SpeechRecognizer_HypothesisGenerated(SpeechRecognizer sender, SpeechRecognitionHypothesisGeneratedEventArgs args)
        {
            await textBox.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                Text = args.Hypothesis.Text;
            });
        }
        #endregion
    }
}
