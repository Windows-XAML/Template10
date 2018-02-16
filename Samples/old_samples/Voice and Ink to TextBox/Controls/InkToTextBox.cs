using Template10.Samples.VoiceAndInkSample.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Input.Inking;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace Template10.Samples.VoiceAndInkSample.Controls
{
    [TemplatePart(Name = PART_ROOT_NAME, Type = typeof(Grid))]
    public class InkToTextBox : Control
    {
        #region private fields
        private const string PART_ROOT_NAME = "PART_ROOT";
        private const string PART_INKER_NAME = "PART_INKER";
        private const string TEXT_PROPERTY_NAME = "Text";
        private DispatcherTimer timer;
        private Grid container;
        private InkCanvas inker;
        private UIElement root;
        private UIElement contentPresenter;
        #endregion

        #region ctor
        public InkToTextBox()
        {
            this.DefaultStyleKey = typeof(InkToTextBox);
            timer = new DispatcherTimer();
            timer.Tick += Timer_Tick;
            timer.Interval = TimeSpan.FromSeconds(3);
        }
        #endregion

        #region public fields
        public Control TargetTextControl
        {
            get { return (Control)GetValue(TargetTextControlProperty); }
            set
            {
                if (value != null)
                {
                    var type = value.GetType();
                    // Get the PropertyInfo object by passing the property name.
                    PropertyInfo pInfo = type.GetProperty(TEXT_PROPERTY_NAME);

                    if (pInfo == null)
                        throw new ArgumentException("Control provided does not expose a class of type string with name Text");
                }
                SetValue(TargetTextControlProperty, value);
            }
        }

        public static readonly DependencyProperty TargetTextControlProperty =
          DependencyProperty.Register(nameof(TargetTextControl), typeof(Control), typeof(InkToTextBox), new PropertyMetadata(null));

        public Color PenColor
        {
            get { return (Color)GetValue(PenColorProperty); }
            set { SetValue(PenColorProperty, value); }
        }

        public static readonly DependencyProperty PenColorProperty =
           DependencyProperty.Register(nameof(PenColor), typeof(Color), typeof(InkToTextBox), new PropertyMetadata(Colors.Black));

        public PenTipShape PenTip
        {
            get { return (PenTipShape)GetValue(PenTipProperty); }
            set { SetValue(PenTipProperty, value); }
        }

        public static readonly DependencyProperty PenTipProperty =
           DependencyProperty.Register(nameof(PenTip), typeof(PenTipShape), typeof(InkToTextBox), new PropertyMetadata(PenTipShape.Circle));

        public Size PenSize
        {
            get { return (Size)GetValue(PenSizeProperty); }
            set { SetValue(TargetTextControlProperty, value); }
        }

        public static readonly DependencyProperty PenSizeProperty =
           DependencyProperty.Register(nameof(PenSize), typeof(Size), typeof(InkToTextBox), new PropertyMetadata(new Size(3,3)));
        #endregion

        #region override OnApplyTemplate
        protected override void OnApplyTemplate()
        {
            try
            {
                container = GetTemplateChild(PART_ROOT_NAME) as Grid;
                inker = GetTemplateChild(PART_INKER_NAME) as InkCanvas;
                if (container != null && inker != null)
                {
                    container.Visibility = Visibility.Visible;
                    InitializeInker();

                    root = VisualTreeHelperEx.FindRoot(container, false);
                    contentPresenter = VisualTreeHelperEx.FindRoot(container, true);

                    contentPresenter.PointerEntered += Element_PointerEntered;
                    contentPresenter.PointerExited += Element_PointerExited;
                    contentPresenter.PointerCanceled += Element_PointerCanceled;
                    contentPresenter.PointerReleased += Element_PointerCanceled;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }
        #endregion

        #region pointer events
        private void Element_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("PointerEntered");
            if (e.Pointer.PointerDeviceType == Windows.Devices.Input.PointerDeviceType.Pen)
            {
                root.CapturePointer(e.Pointer);

            }
            PointerProcessor(e.Pointer);
        }

        private void Element_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("PointerExited");
            if (e.Pointer.PointerDeviceType == Windows.Devices.Input.PointerDeviceType.Pen)
            {
                root.ReleasePointerCapture(e.Pointer);

            }
            PointerProcessor(e.Pointer);
        }

        private void Element_PointerCanceled(object sender, PointerRoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("PointerCanceled");
            if (e.Pointer.PointerDeviceType == Windows.Devices.Input.PointerDeviceType.Pen)
            {
                root.ReleasePointerCapture(e.Pointer);
            }
            PointerProcessor(e.Pointer);
        }
        #endregion

        #region private methods
        private void InitializeInker()
        {
            try
            {
                inker.InkPresenter.InputDeviceTypes = Windows.UI.Core.CoreInputDeviceTypes.Pen;

                var drawingAttributes = new InkDrawingAttributes();

                drawingAttributes.DrawAsHighlighter = false;
                drawingAttributes.IgnorePressure = false;

                drawingAttributes.Color = PenColor;
                drawingAttributes.PenTip = PenTip;
                drawingAttributes.Size = PenSize;

                inker.InkPresenter.UpdateDefaultDrawingAttributes(drawingAttributes);
                inker.InkPresenter.StrokesCollected += InkPresenter_StrokesCollected;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }

        }

        private void PointerProcessor(Pointer pointer)
        {
            System.Diagnostics.Debug.WriteLine("PointerProcessor");
            try
            {
                if (pointer.PointerDeviceType == Windows.Devices.Input.PointerDeviceType.Pen && container.Visibility == Visibility.Collapsed)
                {
                    // enable Inker
                    container.Visibility = Visibility.Visible;
                    System.Diagnostics.Debug.WriteLine("PointerProcessor_enable");
                }
                else if (pointer.PointerDeviceType != Windows.Devices.Input.PointerDeviceType.Pen && container.Visibility == Visibility.Visible)
                {
                    System.Diagnostics.Debug.WriteLine("PointerProcessor_diable");
                    // disable Inker
                    timer.Stop();
                    container.Visibility = Visibility.Collapsed;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }

        private void InkPresenter_StrokesCollected(InkPresenter sender, InkStrokesCollectedEventArgs args)
        {
            System.Diagnostics.Debug.WriteLine("InkPresenter_StrokesCollected");
            timer.Start();
        }

        private async void Timer_Tick(object sender, object e)
        {
            System.Diagnostics.Debug.WriteLine("Timer_Tick");
            timer.Stop();
            await RecognizeInkerText();
        }

        private async Task RecognizeInkerText()
        {
            System.Diagnostics.Debug.WriteLine("RecognizeInkerText");
            try
            {
                var inkRecognizer = new InkRecognizerContainer();
                var recognitionResults = await inkRecognizer.RecognizeAsync(inker.InkPresenter.StrokeContainer, InkRecognitionTarget.All);

                List<TextBox> textBoxes = new List<TextBox>();

                string value = string.Empty; 

                foreach (var result in recognitionResults)
                {
                    if (TargetTextControl == null)
                    {
                        Point p = new Point(result.BoundingRect.X, result.BoundingRect.Y);
                        Size s = new Size(result.BoundingRect.Width, result.BoundingRect.Height);
                        Rect r = new Rect(p, s);
                        var elements = VisualTreeHelper.FindElementsInHostCoordinates(r, contentPresenter);

                        TextBox box = elements.Where(el => el is TextBox && (el as TextBox).IsEnabled).FirstOrDefault() as TextBox;
                        if (box != null)
                        {
                            if (!textBoxes.Contains(box))
                            {
                                textBoxes.Add(box);
                                box.Text = "";
                            }
                            if (string.IsNullOrEmpty(box.Text) == false)
                                box.Text += " ";
                            box.Text += result.GetTextCandidates().FirstOrDefault().Trim();
                        }
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(value) == false)
                            value += " ";
                        value += result.GetTextCandidates().FirstOrDefault().Trim();
                    }
                        
                }

                if (TargetTextControl != null)
                {
                    var type = TargetTextControl.GetType();
                    PropertyInfo pInfo = type.GetProperty(TEXT_PROPERTY_NAME);
                    pInfo.SetValue(TargetTextControl, value);
                }

                inker.InkPresenter.StrokeContainer.Clear();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }
        #endregion
    }
}
