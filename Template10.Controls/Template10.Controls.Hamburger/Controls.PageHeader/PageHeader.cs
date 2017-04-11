using Template10.Utils;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Shapes;

// The Templated Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234235

namespace Template10.Controls
{
    [ContentProperty(Name = nameof(PrimaryCommands))]
    public sealed class PageHeader : CommandBar
    {
        private HamburgerMenu deRegHamburgerEvents; // Avoids unnecessary deregistration for pages without PageHeader

        public PageHeader()
        {
            DefaultStyleKey = typeof(PageHeader);

            // behaviors
            var behavior = new Behaviors.EllipsisBehavior();
            RegisterPropertyChangedCallback(EllipsisVisibilityProperty, (s, e) => behavior.Visibility = EllipsisVisibility);
            var collection = new Microsoft.Xaml.Interactivity.BehaviorCollection();
            collection.Add(behavior);
            SetValue(Microsoft.Xaml.Interactivity.Interaction.BehaviorsProperty, collection);

            TabIndex = 5000;

            Loaded += (s, e) =>
        {
            RegisterHamburgerMenuChanges();
            UpdateSpacingToFitHamburgerMenu();
            };

            Unloaded += (s, e) =>
            {
                if (deRegHamburgerEvents != null)
                {
                    deRegHamburgerEvents.IsOpenChanged -= HamburgerMenu_IsOpenChanged;
                    deRegHamburgerEvents.DisplayModeChanged -= HamburgerMenu_DisplayModeChanged;
                    deRegHamburgerEvents.HamburgerButtonVisibilityChanged -= HamburgerMenu_HamburgerButtonVisibilityChanged;
                }
            };
        }

        private void RegisterHamburgerMenuChanges()
        {
            deRegHamburgerEvents = null; // default on PageHeader startup

            var hamburgerMenu = ParentHamburgerMenu;

            if (hamburgerMenu != null)
            {
                deRegHamburgerEvents = hamburgerMenu; // remember to deregister on UnLoaded event
                hamburgerMenu.IsOpenChanged += HamburgerMenu_IsOpenChanged;
                hamburgerMenu.DisplayModeChanged += HamburgerMenu_DisplayModeChanged;
                hamburgerMenu.HamburgerButtonVisibilityChanged += HamburgerMenu_HamburgerButtonVisibilityChanged;
            }
        }

        private void HamburgerMenu_IsOpenChanged(object sender, Common.ChangedEventArgs<bool> e)
        {
            UpdateSpacingToFitHamburgerMenu();
        }

        private void HamburgerMenu_DisplayModeChanged(object sender, Common.ChangedEventArgs<SplitViewDisplayMode> e)
        {
            UpdateSpacingToFitHamburgerMenu();
        }

        private void HamburgerMenu_HamburgerButtonVisibilityChanged(object sender, Common.ChangedEventArgs<Visibility> e)
        {
            UpdateSpacingToFitHamburgerMenu();
        }

        private void UpdateSpacingToFitHamburgerMenu()
        {
            if (EnableHamburgerMenuAutoLayout && spacer != null)
            {
                var hamburgerMenu = ParentHamburgerMenu;
                if (hamburgerMenu == null)
                {
                    spacer.Visibility = Visibility.Collapsed;
                }
                else
                {
                    switch (hamburgerMenu.DisplayMode)
                    {
                        case SplitViewDisplayMode.Inline:
                        case SplitViewDisplayMode.Overlay:
                            {
                                var buttonVisible = hamburgerMenu.HamburgerButtonVisibility == Visibility.Visible;
                                spacer.Visibility = buttonVisible ? Visibility.Visible : Visibility.Collapsed;
                            }
                            break;
                        case SplitViewDisplayMode.CompactOverlay:
                        case SplitViewDisplayMode.CompactInline:
                            {
                                spacer.Visibility = Visibility.Collapsed;
                            }
                            break;
                    }
                }
            }
        }

        private Page ParentPage => this.FirstAncestor<Page>();
        private HamburgerMenu ParentHamburgerMenu => ParentPage?.Frame?.FirstAncestor<HamburgerMenu>();

        public Behaviors.EllipsisBehavior.Visibilities EllipsisVisibility
        {
            get { return (Behaviors.EllipsisBehavior.Visibilities)GetValue(EllipsisVisibilityProperty); }
            set { SetValue(EllipsisVisibilityProperty, value); }
        }
        public static readonly DependencyProperty EllipsisVisibilityProperty =
            DependencyProperty.Register(nameof(EllipsisVisibility), typeof(Behaviors.EllipsisBehavior.Visibilities),
                typeof(PageHeader), new PropertyMetadata(Behaviors.EllipsisBehavior.Visibilities.Auto));

        public Visibility PrimaryCommandsVisibility
        {
            get { return (Visibility)GetValue(PrimaryCommandsVisibilityProperty); }
            set { SetValue(PrimaryCommandsVisibilityProperty, value); }
        }
        public static readonly DependencyProperty PrimaryCommandsVisibilityProperty =
            DependencyProperty.Register(nameof(PrimaryCommandsVisibility), typeof(Visibility),
                typeof(PageHeader), new PropertyMetadata(Visibility.Visible));

        public Visibility BackButtonVisibility
        {
            get { return (Visibility)GetValue(BackButtonVisibilityProperty); }
            set { SetValue(BackButtonVisibilityProperty, value); }
        }
        public static readonly DependencyProperty BackButtonVisibilityProperty =
            DependencyProperty.Register(nameof(BackButtonVisibility), typeof(Visibility),
                typeof(PageHeader), new PropertyMetadata(Visibility.Collapsed));

        public Symbol BackButtonContent
        {
            get { return (Symbol)GetValue(BackButtonContentProperty); }
            set { SetValue(BackButtonContentProperty, value); }
        }
        public static readonly DependencyProperty BackButtonContentProperty =
            DependencyProperty.Register(nameof(BackButtonContent), typeof(Symbol), typeof(PageHeader),
                new PropertyMetadata(Symbol.Back));

        public Frame Frame { get { return (Frame)GetValue(FrameProperty); } set { SetValue(FrameProperty, value); } }
        public static readonly DependencyProperty FrameProperty =
            DependencyProperty.Register(nameof(Frame), typeof(Frame), typeof(PageHeader),
                new PropertyMetadata(default(Frame), OnFramePropertyChanged));
        private static void OnFramePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var frame = (Frame)e.NewValue;
            if (frame.FlowDirection == FlowDirection.LeftToRight)
                d.SetValue(BackButtonContentProperty, Symbol.Back);
            else
                d.SetValue(BackButtonContentProperty, Symbol.Forward);
        }

        public bool EnableHamburgerMenuAutoLayout
        {
            get { return (bool)GetValue(EnableHamburgerMenuAutoLayoutProperty); }
            set { SetValue(EnableHamburgerMenuAutoLayoutProperty, value); }
        }
        public static readonly DependencyProperty EnableHamburgerMenuAutoLayoutProperty =
            DependencyProperty.Register(nameof(EnableHamburgerMenuAutoLayout), typeof(bool),
                typeof(PageHeader), new PropertyMetadata(true));

        public double VisualStateNarrowMinWidth
        {
            get { return (double)GetValue(VisualStateNarrowMinWidthProperty); }
            set { SetValue(VisualStateNarrowMinWidthProperty, value); }
        }
        public static readonly DependencyProperty VisualStateNarrowMinWidthProperty =
            DependencyProperty.Register(nameof(VisualStateNarrowMinWidth), typeof(double),
                typeof(PageHeader), new PropertyMetadata((double)-1));

        public double VisualStateNormalMinWidth
        {
            get { return (double)GetValue(VisualStateNormalMinWidthProperty); }
            set { SetValue(VisualStateNormalMinWidthProperty, value); }
        }
        public static readonly DependencyProperty VisualStateNormalMinWidthProperty =
            DependencyProperty.Register(nameof(VisualStateNormalMinWidth), typeof(double),
                typeof(PageHeader), new PropertyMetadata(0d));

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(PageHeader), new PropertyMetadata(string.Empty, (d, e) =>
            {
                (d as PageHeader).Content = e.NewValue;
            }));


        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            spacer = GetTemplateChild<Rectangle>("Spacer");
            moreButton = GetTemplateChild<Button>("MoreButton");
            UpdateSpacingToFitHamburgerMenu();
        }

        private T GetTemplateChild<T>(string name) where T : DependencyObject
        {
            var child = GetTemplateChild(name) as T;
            if (child == null && !Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                throw new Common.TemplatePartNotFoundException($"Control part {name} not found in Template.");
            }
            return child;
        }

        private Button moreButton;
        private Rectangle spacer;
        internal Button GetMoreButton() => moreButton;
    }
}
