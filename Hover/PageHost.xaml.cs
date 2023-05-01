using System.Threading.Tasks;
using System.Windows;
using Hover.Views;

namespace Hover
{
    #region Attribution
    // Code attribution: Original Author Luke Malpass (Angel Six)
    // Note: Few changes made
    #endregion

    /// <summary>
    /// Hosts the navigation of different pages.
    /// </summary>
    public partial class PageHost
    {
        /// <summary>
        /// Gets or sets the current page to show.
        /// </summary>
        public BasePage CurrentPage
        {
            get => (BasePage)GetValue(CurrentPageProperty);
            set => SetValue(CurrentPageProperty, value);
        }

        /// <summary>
        /// Registers <see cref="CurrentPage"/> as a dependency property.
        /// </summary>
        public static readonly DependencyProperty CurrentPageProperty =
            DependencyProperty.Register(nameof(CurrentPage), typeof(BasePage),
                typeof(PageHost), new UIPropertyMetadata(CurrentPageChanged));

        /// <summary>
        /// Initializes a new instance of the <see cref="PageHost"/> class.
        /// </summary>
        public PageHost()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Called when the <see cref="CurrentPage"/> value has changed.
        /// </summary>
        /// <param name="d">The PageHost.</param>
        /// <param name="e"> The arguments passed with the event.</param>
        private static void CurrentPageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var newPage = ((PageHost)d).NewPage;
            var oldPage = ((PageHost)d).OldPage;

            var oldContent = newPage.Content;

            newPage.Content = null;

            oldPage.Content = oldContent;

            if (oldContent is BasePage oPage)
            {
                oPage.ShouldAnimateOut = true;

                Task.Delay((int)(oPage.SlideSeconds * 1000)).ContinueWith((t) =>
                {
                    Application.Current.Dispatcher.Invoke(() => oldPage.Content = null);
                });
            }

            newPage.Content = e.NewValue;
        }

    }
}
