# Template10.Samples.CortanaSample (and Speech)

This project demonstrates how to use Template10.Samples.CortanaSample's speech functionality and Template10's audio utilities in your application.

## Speech Service

The speech service uses the SpeechRecognizer class from the Windows.Media.SpeechRecognition namespace to "speak" text, show Cortant-related UI dialogs and listen to the user's speech.

### Validate Microphone Access

Before initializing the SpeechRecognizer instance, the static AudioUtils.RequestMicrophonePermission method is invoked to determine if the application has access to the microphone.

    var permission = await Template10.Utils.AudioUtils.RequestMicrophonePermission();

### Listen to User Speech

This example shows a built-in dialog that prompts the user to speak.  The result of this speech is returned in a SpeechRecognitionResult value.

    _SpeechRecognizer.UIOptions.ShowConfirmation = false;
    _SpeechRecognizer.UIOptions.AudiblePrompt = prompt;
    _SpeechRecognizer.UIOptions.ExampleText = example;
    var result = await _SpeechRecognizer.RecognizeWithUIAsync();
    if (result.Status == SpeechRecognitionResultStatus.Success)
    {
        return result.Text;
    }
    
### Speak Text

This example creates a SpeechSynthesizer instance with a female US voice and converts a string (text) to a Stream.  The Stream is then played using the built-in MediaElement.

    var voice = SpeechSynthesizer.AllVoices
        .First(x => x.Gender.Equals(VoiceGender.Female) && x.Description.Contains("United States"));
    using (var speech = new SpeechSynthesizer { Voice = voice })
    {
        text = string.IsNullOrWhiteSpace(text) ? "There is no text to speak." : text;
        var stream = await speech.SynthesizeTextToStreamAsync(text);

        var media = new MediaElement { AutoPlay = true };
        media.SetSource(stream, stream.ContentType);
    }

## Template10.Samples.CortanaSample XML

This project also includes a sample XML file illustrating the natural language voice capabilities with Template10.Samples.CortanaSample.

    <VoiceCommands xmlns="http://schemas.microsoft.com/voicecommands/1.2">
      <CommandSet xml:lang="en-us" Name="FreeText_en-us">
        <CommandPrefix> TemplateTen </CommandPrefix>
        <Example> Handle my text with Template 10. </Example>
        <Command Name="FreeTextCommand">
          <Example> Add statement my text </Example>
          <ListenFor RequireAppName="BeforeOrAfterPhrase"> [please] add statement {textPhrase} [with] </ListenFor>
          <Feedback> Adding statement for you now. </Feedback>
          <Navigate />
        </Command>
        <PhraseTopic Label="textPhrase" Scenario="Natural Language" />
      </CommandSet>
    </VoiceCommands>

This sample file implements a phrase "Handle {spoken phrase}" with optional words.  Here are some examples of phrases you can say:
- *TemplateTen* add statement **hello world**
- *TemplateTen* please add statement **hello world**
- Please add statement **hello world** *TemplateTen*
- Add statement **hello world** *TemplateTen*

More details about natural language voice commands can be found here: https://msdn.microsoft.com/en-us/library/windows/apps/xaml/dn997786.aspx

