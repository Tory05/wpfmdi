using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Collections.Generic;

namespace WPF.MDI
{
    [ContentProperty("Content")]
    public class MdiChild : Control
    {
        private IInputElement LastFocousedElement = null;
        #region Constants

        /// <summary>
        /// Width of minimized window.
        /// </summary>
        internal const int MinimizedWidth = 160;

        /// <summary>
        /// Height of minimized window.
        /// </summary>
        internal const int MinimizedHeight = 29;

        #endregion

        #region Dependency Properties

        /// <summary>
        /// Identifies the WPF.MDI.MdiChild.ContentProperty dependency property.
        /// </summary>
        /// <returns>The identifier for the WPF.MDI.MdiChild.ContentProperty property.</returns>
        public static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register("Content", typeof(UIElement), typeof(MdiChild));

        /// <summary>
        /// Identifies the WPF.MDI.MdiChild.TitleProperty dependency property.
        /// </summary>
        /// <returns>The identifier for the WPF.MDI.MdiChild.TitleProperty property.</returns>
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(MdiChild));

        /// <summary>
        /// Identifies the WPF.MDI.MdiChild.IconProperty dependency property.
        /// </summary>
        /// <returns>The identifier for the WPF.MDI.MdiChild.IconProperty property.</returns>
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(ImageSource), typeof(MdiChild));

        /// <summary>
        /// Identifies the WPF.MDI.MdiChild.ShowIconProperty dependency property.
        /// </summary>
        /// <returns>The identifier for the WPF.MDI.MdiChild.ShowIconProperty property.</returns>
        public static readonly DependencyProperty ShowIconProperty =
            DependencyProperty.Register("ShowIcon", typeof(bool), typeof(MdiChild),
            new UIPropertyMetadata(true));

        /// <summary>
        /// Identifies the WPF.MDI.MdiChild.ResizableProperty dependency property.
        /// </summary>
        /// <returns>The identifier for the WPF.MDI.MdiChild.ResizableProperty property.</returns>
        public static readonly DependencyProperty ResizableProperty =
            DependencyProperty.Register("Resizable", typeof(bool), typeof(MdiChild),
            new UIPropertyMetadata(true));

        /// <summary>
        /// Identifies the WPF.MDI.MdiChild.FocusedProperty dependency property.
        /// </summary>
        /// <returns>The identifier for the WPF.MDI.MdiChild.FocusedProperty property.</returns>
        public static readonly DependencyProperty FocusedProperty =
            DependencyProperty.Register("Focused", typeof(bool), typeof(MdiChild),
            new UIPropertyMetadata(false, new PropertyChangedCallback(FocusedValueChanged)));

        /// <summary>
        /// Identifies the WPF.MDI.MdiChild.MinimizeBoxProperty dependency property.
        /// </summary>
        /// <returns>The identifier for the WPF.MDI.MdiChild.MinimizeBoxProperty property.</returns>
        public static readonly DependencyProperty MinimizeBoxProperty =
            DependencyProperty.Register("MinimizeBox", typeof(bool), typeof(MdiChild),
            new UIPropertyMetadata(true, new PropertyChangedCallback(MinimizeBoxValueChanged)));

        /// <summary>
        /// Identifies the WPF.MDI.MdiChild.MaximizeBoxProperty dependency property.
        /// </summary>
        /// <returns>The identifier for the WPF.MDI.MdiChild.MaximizeBoxProperty property.</returns>
        public static readonly DependencyProperty MaximizeBoxProperty =
            DependencyProperty.Register("MaximizeBox", typeof(bool), typeof(MdiChild),
            new UIPropertyMetadata(true, new PropertyChangedCallback(MaximizeBoxValueChanged)));

        /// <summary>
        /// Identifies the WPF.MDI.MdiChild.CloseBoxProperty dependency property.
        /// </summary>
        /// <returns>The identifier for the WPF.MDI.MdiChild.CloseBoxProperty property.</returns>
        public static readonly DependencyProperty CloseBoxProperty =
            DependencyProperty.Register("CloseBox", typeof(bool), typeof(MdiChild),
            new UIPropertyMetadata(true, new PropertyChangedCallback(CloseBoxValueChanged)));

        /// <summary>
        /// Identifies the WPF.MDI.MdiChild.WindowStateProperty dependency property.
        /// </summary>
        /// <returns>The identifier for the WPF.MDI.MdiChild.WindowStateProperty property.</returns>
        public static readonly DependencyProperty WindowStateProperty =
            DependencyProperty.Register("WindowState", typeof(WindowState), typeof(MdiChild),
            new UIPropertyMetadata(WindowState.Normal, new PropertyChangedCallback(WindowStateValueChanged)));

        /// <summary>
        /// Identifies the WPF.MDI.MdiChild.PositionProperty dependency property.
        /// </summary>
        /// <returns>The identifier for the WPF.MDI.MdiChild.PositionProperty property.</returns>
        public static readonly DependencyProperty PositionProperty =
            DependencyProperty.Register("Position", typeof(Point), typeof(MdiChild),
            new UIPropertyMetadata(new Point(-1, -1), new PropertyChangedCallback(PositionValueChanged)));

        /// <summary>
        /// Identifies the WPF.MDI.MdiChild.IsActiveProperty dependency property.
        /// </summary>
        /// <returns>The identifier for the WPF.MDI.MdiChild.IsActiveProperty property.</returns>
        public static readonly DependencyProperty IsActiveProperty =
            DependencyProperty.Register("IsActive", typeof(bool), typeof(MdiChild));


        #endregion

        #region Dependency Events

        /// <summary>
        /// Identifies the WPF.MDI.MdiChild.ClosingEvent routed event.
        /// </summary>
        /// <returns>The identifier for the WPF.MDI.MdiChild.ClosingEvent routed event.</returns>
        public static readonly RoutedEvent ClosingEvent =
            EventManager.RegisterRoutedEvent("Closing", RoutingStrategy.Bubble, typeof(ClosingEventArgs), typeof(MdiChild));

        /// <summary>
        /// Identifies the WPF.MDI.MdiChild.ClosedEvent routed event.
        /// </summary>
        /// <returns>The identifier for the WPF.MDI.MdiChild.ClosedEvent routed event.</returns>
        public static readonly RoutedEvent ClosedEvent =
            EventManager.RegisterRoutedEvent("Closed", RoutingStrategy.Bubble, typeof(RoutedEventArgs), typeof(MdiChild));
        /// <summary>
        /// Identifies the WPF.MDI.MdiChild.ActivatedEvent routed event.
        /// </summary>
        /// <returns>The identifier for the WPF.MDI.MdiChild.ActivatedEvent routed event.</returns>
        public static readonly RoutedEvent ActivatedEvent =
            EventManager.RegisterRoutedEvent("Activated", RoutingStrategy.Bubble, typeof(RoutedEventArgs), typeof(MdiChild));

        /// <summary>
        /// Identifies the WPF.MDI.MdiChild.DeactivatedEvent routed event.
        /// </summary>
        /// <returns>The identifier for the WPF.MDI.MdiChild.DeactivatedEvent routed event.</returns>
        public static readonly RoutedEvent DeactivatedEvent =
            EventManager.RegisterRoutedEvent("Deactivated", RoutingStrategy.Bubble, typeof(RoutedEventArgs), typeof(MdiChild));
        #endregion

        #region Property Accessors

        /// <summary>
        /// Gets or sets the content.
        /// This is a dependency property.
        /// </summary>
        /// <value>The content.</value>
        public UIElement Content
        {
            get { return (UIElement)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        /// <summary>
        /// Gets or sets the window title.
        /// This is a dependency property.
        /// </summary>
        /// <value>The window title.</value>
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        /// <summary>
        /// Gets or sets the window icon.
        /// This is a dependency property.
        /// </summary>
        /// <value>The window icon.</value>
        public ImageSource Icon
        {
            get { return (ImageSource)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to [show the window icon].
        /// This is a dependency property.
        /// </summary>
        /// <value><c>true</c> if [show the window icon]; otherwise, <c>false</c>.</value>
        public bool ShowIcon
        {
            get { return (bool)GetValue(ShowIconProperty); }
            set { SetValue(ShowIconProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the [window is resizable].
        /// This is a dependency property.
        /// </summary>
        /// <value><c>true</c> if [window is resizable]; otherwise, <c>false</c>.</value>
        public bool Resizable
        {
            get { return (bool)GetValue(ResizableProperty); }
            set { SetValue(ResizableProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the [window is focused].
        /// This is a dependency property.
        /// </summary>
        /// <value><c>true</c> if [window is focused]; otherwise, <c>false</c>.</value>
        public bool Focused
        {
            get { return (bool)GetValue(FocusedProperty); }
            set { SetValue(FocusedProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to [show the minimize box button].
        /// This is a dependency property.
        /// </summary>
        /// <value><c>true</c> if [show the minimize box button]; otherwise, <c>false</c>.</value>
        public bool MinimizeBox
        {
            get { return (bool)GetValue(MinimizeBoxProperty); }
            set { SetValue(MinimizeBoxProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to [show the maximize box button].
        /// This is a dependency property.
        /// </summary>
        /// <value><c>true</c> if [show the maximize box button]; otherwise, <c>false</c>.</value>
        public bool MaximizeBox
        {
            get { return (bool)GetValue(MaximizeBoxProperty); }
            set { SetValue(MaximizeBoxProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to [show the close box button].
        /// This is a dependency property.
        /// </summary>
        /// <value><c>true</c> if [show the close box button]; otherwise, <c>false</c>.</value>
        public bool CloseBox
        {
            get { return (bool)GetValue(CloseBoxProperty); }
            set { SetValue(CloseBoxProperty, value); }
        }

        /// <summary>
        /// Gets or sets the state of the window.
        /// This is a dependency property.
        /// </summary>
        /// <value>The state of the window.</value>
        public WindowState WindowState
        {
            get { return (WindowState)GetValue(WindowStateProperty); }
            set { SetValue(WindowStateProperty, value); }
        }

        /// <summary>
        /// Gets or sets position of top left corner of window.
        /// This is a dependency property.
        /// </summary>
        public Point Position
        {
            get { return (Point)GetValue(PositionProperty); }
            set { SetValue(PositionProperty, value); }
        }

        /// <summary>
        /// Gets buttons for window in maximized state.
        /// </summary>
        public Panel Buttons
        {
            //get { return (Panel)GetValue(ButtonsProperty); }
            //private set { SetValue(ButtonsProperty, value); }
            get;
            private set;
        }

        /// <summary>
        /// Force user not to use Margin property.
        /// </summary>
        private new Thickness Margin { set { } }

        #endregion

        #region Event Accessors

        public event RoutedEventHandler Closing
        {
            add { AddHandler(ClosingEvent, value); }
            remove { RemoveHandler(ClosingEvent, value); }
        }

        public event RoutedEventHandler Closed
        {
            add { AddHandler(ClosedEvent, value); }
            remove { RemoveHandler(ClosedEvent, value); }
        }

        public event RoutedEventHandler Activated
        {
            add { AddHandler(ActivatedEvent, value); }
            remove { RemoveHandler(ActivatedEvent, value); }
        }

        public event RoutedEventHandler Deactivated
        {
            add { AddHandler(DeactivatedEvent, value); }
            remove { RemoveHandler(DeactivatedEvent, value); }
        }
        #endregion

        #region Member Declarations

        #region Top Buttons

        private Button minimizeButton;

        private Button maximizeButton;

        private Button closeButton;

        private StackPanel buttonsPanel;

        #endregion

        /// <summary>
        /// Gets or sets the container.
        /// </summary>
        /// <value>The container.</value>
        public MdiContainer Container { get; private set; }

        /// <summary>
        /// Dimensions of window in Normal state.
        /// </summary>
        private Rect originalDimension;

        /// <summary>
        /// Position of window in Minimized state.
        /// </summary>
        private Point minimizedPosition = new Point(-1, -1);

        /// <summary>
        /// Previous non-maximized state of maximized window.
        /// </summary>
        WindowState NonMaximizedState { get; set; }

        /// <summary>
		/// Original minmum size of window in Minimized state.
		/// </summary>
		private Size originalMinimumSize = new Size();
        #endregion

        #region Constructor

        /// <summary>
        /// Initializes the <see cref="MdiChild"/> class.
        /// </summary>
        static MdiChild()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MdiChild), new FrameworkPropertyMetadata(typeof(MdiChild)));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MdiChild"/> class.
        /// </summary>
        public MdiChild()
        {
            Focusable = IsTabStop = false;

            Loaded += MdiChild_Loaded;
            GotFocus += MdiChild_GotFocus;
            KeyDown += MdiChild_KeyDown;
            LostFocus += MdiChild_LostFocus;
        }

        static void MdiChild_KeyDown(object sender, KeyEventArgs e)
        {
            MdiChild mdiChild = (MdiChild)sender;
            switch (e.Key)
            {
                case Key.F4:
                    if (e.KeyboardDevice.Modifiers == ModifierKeys.Control)
                    {
                        mdiChild.Close();
                        e.Handled = true;
                    }
                    break;
            }
        }

        #endregion

        #region Control Events

        /// <summary>
        /// Handles the Loaded event of the MdiChild control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void MdiChild_Loaded(object sender, RoutedEventArgs e)
        {
            FrameworkElement currentControl = this;

            while (currentControl != null && currentControl.GetType() != typeof(MdiContainer))
            {
                currentControl = (FrameworkElement)currentControl.Parent;
            }

            if (currentControl != null)
            {
                Container = (MdiContainer)currentControl;
            }
            //Maintain the cycle of keyboard navigation within the active MdiChild
            KeyboardNavigation.SetTabNavigation(this, KeyboardNavigationMode.Cycle);
            KeyboardNavigation.SetControlTabNavigation(this, KeyboardNavigationMode.Cycle);
            KeyboardNavigation.SetDirectionalNavigation(this, KeyboardNavigationMode.Cycle);

            /* WPF manages the KeyDown only if it has at least one control with the Focus active,
             * So set the focus on the first control within the MdiChild, to allow the KeyDown event
             */
            MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));

            //Stores reference to the control with the focus active, and then reactivates it when user return from another MdiChild
            LastFocousedElement = Keyboard.FocusedElement;
        }

        /// <summary>
        /// Handles the GotFocus event of the MdiChild control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void MdiChild_GotFocus(object sender, RoutedEventArgs e)
        {
            if (LastFocousedElement != null)
            {
                if (Keyboard.FocusedElement != LastFocousedElement && (Container.ActiveMdiChild.Name != Name))
                {
                    //When the user returns to this MdiChild from another MdiChild, set the focus to the last control used
                    Keyboard.Focus(LastFocousedElement);
                    Container.ActiveMdiChild = this;
                    RaiseEvent(new RoutedEventArgs(ActivatedEvent));
                    return;
                }

            }
            Focus();
        }

        private void MdiChild_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Content == null)
            {
                return;
            }

            //if (Content.IsKeyboardFocusWithin)
            //{
            //    //Get the last element with active focus within the MdiChild
            //    LastFocousedElement = Keyboard.FocusedElement;
            //}
            else if (!IsKeyboardFocusWithin && (Container.ActiveMdiChild.Name == Name))
            {
                SetValue(IsActiveProperty, false);
                RaiseEvent(new RoutedEventArgs(DeactivatedEvent));
            }
        }
        #endregion

        #region Control Overrides

        /// <summary>
        /// When overridden in a derived class, is invoked whenever application code or internal processes call <see cref="M:System.Windows.FrameworkElement.ApplyTemplate"/>.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();


            minimizeButton = (Button)Template.FindName("MinimizeButton", this);
            maximizeButton = (Button)Template.FindName("MaximizeButton", this);
            closeButton = (Button)Template.FindName("CloseButton", this);
            buttonsPanel = (StackPanel)Template.FindName("ButtonsPanel", this);

            if (minimizeButton != null)
            {
                minimizeButton.Click += MinimizeButton_Click;
            }

            if (maximizeButton != null)
            {
                maximizeButton.Click += MaximizeButton_Click;
            }

            if (closeButton != null)
            {
                closeButton.Click += CloseButton_Click;
            }

            Thumb dragThumb = (Thumb)Template.FindName("DragThumb", this);

            if (dragThumb != null)
            {
                dragThumb.DragStarted += Thumb_DragStarted;
                dragThumb.DragDelta += DragThumb_DragDelta;

                dragThumb.MouseDoubleClick += (sender, e) =>
                {
                    if (WindowState == WindowState.Minimized)
                    {
                        MinimizeButton_Click(null, null);
                    }
                    else if (WindowState == WindowState.Normal)
                    {
                        MaximizeButton_Click(null, null);
                    }
                };
            }

            Thumb resizeLeft = (Thumb)Template.FindName("ResizeLeft", this);
            Thumb resizeTopLeft = (Thumb)Template.FindName("ResizeTopLeft", this);
            Thumb resizeTop = (Thumb)Template.FindName("ResizeTop", this);
            Thumb resizeTopRight = (Thumb)Template.FindName("ResizeTopRight", this);
            Thumb resizeRight = (Thumb)Template.FindName("ResizeRight", this);
            Thumb resizeBottomRight = (Thumb)Template.FindName("ResizeBottomRight", this);
            Thumb resizeBottom = (Thumb)Template.FindName("ResizeBottom", this);
            Thumb resizeBottomLeft = (Thumb)Template.FindName("ResizeBottomLeft", this);

            if (resizeLeft != null)
            {
                resizeLeft.DragStarted += Thumb_DragStarted;
                resizeLeft.DragDelta += ResizeLeft_DragDelta;
            }

            if (resizeTop != null)
            {
                resizeTop.DragStarted += Thumb_DragStarted;
                resizeTop.DragDelta += ResizeTop_DragDelta;
            }

            if (resizeRight != null)
            {
                resizeRight.DragStarted += Thumb_DragStarted;
                resizeRight.DragDelta += ResizeRight_DragDelta;
            }

            if (resizeBottom != null)
            {
                resizeBottom.DragStarted += Thumb_DragStarted;
                resizeBottom.DragDelta += ResizeBottom_DragDelta;
            }

            if (resizeTopLeft != null)
            {
                resizeTopLeft.DragStarted += Thumb_DragStarted;

                resizeTopLeft.DragDelta += (sender, e) =>
                {
                    ResizeTop_DragDelta(null, e);
                    ResizeLeft_DragDelta(null, e);

                    Container.InvalidateSize();
                };
            }

            if (resizeTopRight != null)
            {
                resizeTopRight.DragStarted += Thumb_DragStarted;

                resizeTopRight.DragDelta += (sender, e) =>
                {
                    ResizeTop_DragDelta(null, e);
                    ResizeRight_DragDelta(null, e);

                    Container.InvalidateSize();
                };
            }

            if (resizeBottomRight != null)
            {
                resizeBottomRight.DragStarted += Thumb_DragStarted;

                resizeBottomRight.DragDelta += (sender, e) =>
                {
                    ResizeBottom_DragDelta(null, e);
                    ResizeRight_DragDelta(null, e);

                    Container.InvalidateSize();
                };
            }

            if (resizeBottomLeft != null)
            {
                resizeBottomLeft.DragStarted += Thumb_DragStarted;

                resizeBottomLeft.DragDelta += (sender, e) =>
                {
                    ResizeBottom_DragDelta(null, e);
                    ResizeLeft_DragDelta(null, e);

                    Container.InvalidateSize();
                };
            }

            MinimizeBoxValueChanged(this, new DependencyPropertyChangedEventArgs(MinimizeBoxProperty, true, MinimizeBox));
            MaximizeBoxValueChanged(this, new DependencyPropertyChangedEventArgs(MaximizeBoxProperty, true, MaximizeBox));
            CloseBoxValueChanged(this, new DependencyPropertyChangedEventArgs(CloseBoxProperty, true, CloseBox));
        }

        /// <summary>
        /// Invoked when an unhandled <see cref="E:System.Windows.Input.Mouse.MouseDown"/> attached event reaches an element in its route that is derived from this class. Implement this method to add class handling for this event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.Input.MouseButtonEventArgs"/> that contains the event data. This event data reports details about the mouse button that was pressed and the handled state.</param>
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);

            Focused = true;
        }

        #endregion

        #region Top Button Events

        /// <summary>
        /// Handles the Click event of the minimizeButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState == WindowState.Minimized ? WindowState.Normal : WindowState.Minimized;
        }

        /// <summary>
        /// Handles the Click event of the maximizeButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void MaximizeButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        }

        /// <summary>
        /// Handles the Click event of the closeButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            ClosingEventArgs eventArgs = new ClosingEventArgs(ClosingEvent);
            RaiseEvent(eventArgs);

            if (eventArgs.Cancel)
            {
                return;
            }

            Close();

            RaiseEvent(new RoutedEventArgs(ClosedEvent));
        }

        #endregion

        #region Thumb Events

        /// <summary>
        /// Handles the DragStarted event of the Thumb control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DragStartedEventArgs"/> instance containing the event data.</param>
        private void Thumb_DragStarted(object sender, DragStartedEventArgs e)
        {
            if (!Focused)
            {
                Focused = true;
            }
        }

        /// <summary>
        /// Handles the DragDelta event of the ResizeLeft control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DragDeltaEventArgs"/> instance containing the event data.</param>
        private void ResizeLeft_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (Width - e.HorizontalChange < MinWidth)
            {
                return;
            }

            double newLeft = e.HorizontalChange;

            if (Position.X + newLeft < 0)
            {
                newLeft = 0 - Position.X;
            }

            Width -= newLeft;
            Position = new Point(Position.X + newLeft, Position.Y);

            if (sender != null)
            {
                Container.InvalidateSize();
            }
        }

        /// <summary>
        /// Handles the DragDelta event of the ResizeTop control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DragDeltaEventArgs"/> instance containing the event data.</param>
        private void ResizeTop_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (Height - e.VerticalChange < MinHeight)
            {
                return;
            }

            double newTop = e.VerticalChange;

            if (Position.Y + newTop < 0)
            {
                newTop = 0 - Position.Y;
            }

            Height -= newTop;
            Position = new Point(Position.X, Position.Y + newTop);

            if (sender != null)
            {
                Container.InvalidateSize();
            }
        }

        /// <summary>
        /// Handles the DragDelta event of the ResizeRight control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DragDeltaEventArgs"/> instance containing the event data.</param>
        private void ResizeRight_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (Width + e.HorizontalChange < MinWidth)
            {
                return;
            }

            Width += e.HorizontalChange;

            if (sender != null)
            {
                Container.InvalidateSize();
            }
        }

        /// <summary>
        /// Handles the DragDelta event of the ResizeBottom control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DragDeltaEventArgs"/> instance containing the event data.</param>
        private void ResizeBottom_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (Height + e.VerticalChange < MinHeight)
            {
                return;
            }

            Height += e.VerticalChange;

            if (sender != null)
            {
                Container.InvalidateSize();
            }
        }

        #endregion

        #region Control Drag Event

        /// <summary>
        /// Handles the DragDelta event of the dragThumb control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DragDeltaEventArgs"/> instance containing the event data.</param>
        private void DragThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                return;
            }
            
            if (((MdiContainer)((Grid)((ScrollViewer)((Canvas)Parent).Parent).Parent).Parent).EnableScrollbar)
            {
                double newLeft = Position.X + e.HorizontalChange;
                double newTop = Position.Y + e.VerticalChange;

                if (newLeft < 0)
                {
                    newLeft = 0;
                }

                if (newTop < 0)
                {
                    newTop = 0;
                }

                Position = new Point(newLeft, newTop);

                Container.InvalidateSize();
            }
            else
            {
                ScrollViewer Scrollviewer = (ScrollViewer)((Canvas)Parent).Parent;
                double newLeft = Scrollviewer.ActualWidth >= Position.X + Width + e.HorizontalChange ? Position.X + e.HorizontalChange : Position.X;
                double newTop = Scrollviewer.ActualHeight >= Position.Y + Height + e.VerticalChange ? Position.Y + e.VerticalChange : Position.Y;
                Position = new Point(newLeft, newTop);

                Container.InvalidateSize();
            }
        }

        #endregion

        #region Dependency Property Events

        /// <summary>
        /// Dependency property event once the position value has changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void PositionValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if ((Point)e.NewValue == (Point)e.OldValue)
            {
                return;
            }

            MdiChild mdiChild = (MdiChild)sender;
            Point newPosition = (Point)e.NewValue;

            Canvas.SetTop(mdiChild, newPosition.Y < 0 ? 0 : newPosition.Y);
            Canvas.SetLeft(mdiChild, newPosition.X < 0 ? 0 : newPosition.X);
        }

        /// <summary>
        /// Dependency property event once the focused value has changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void FocusedValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue == (bool)e.OldValue)
            {
                return;
            }

            MdiChild mdiChild = (MdiChild)sender;
            if ((bool)e.NewValue)
            {
                mdiChild.Dispatcher.BeginInvoke(new Func<IInputElement, IInputElement>(Keyboard.Focus), System.Windows.Threading.DispatcherPriority.ApplicationIdle, mdiChild.Content);
                mdiChild.RaiseEvent(new RoutedEventArgs(GotFocusEvent, mdiChild));
            }
            else
            {
                if (mdiChild.WindowState == WindowState.Maximized)
                {
                    mdiChild.Unmaximize();
                }

                mdiChild.RaiseEvent(new RoutedEventArgs(LostFocusEvent, mdiChild));
            }
        }

        /// <summary>
        /// Dependency property event once the minimize box value has changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void MinimizeBoxValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            MdiChild mdiChild = (MdiChild)sender;
            bool visible = (bool)e.NewValue;

            if (visible)
            {
                bool maximizeVisible = true;

                if (mdiChild.maximizeButton != null)
                {
                    maximizeVisible = mdiChild.maximizeButton.Visibility == Visibility.Visible;
                }

                if (mdiChild.minimizeButton != null)
                {
                    mdiChild.minimizeButton.IsEnabled = true;
                }

                if (!maximizeVisible)
                {
                    if (mdiChild.maximizeButton != null)
                    {
                        mdiChild.minimizeButton.Visibility = Visibility.Visible;
                    }

                    if (mdiChild.maximizeButton != null)
                    {
                        mdiChild.maximizeButton.Visibility = Visibility.Visible;
                    }
                }
            }
            else
            {
                bool maximizeEnabled = true;

                if (mdiChild.maximizeButton != null)
                {
                    maximizeEnabled = mdiChild.maximizeButton.IsEnabled;
                }

                if (mdiChild.minimizeButton != null)
                {
                    mdiChild.minimizeButton.IsEnabled = false;
                }

                if (!maximizeEnabled)
                {
                    if (mdiChild.minimizeButton != null)
                    {
                        mdiChild.minimizeButton.Visibility = Visibility.Hidden;
                    }

                    if (mdiChild.maximizeButton != null)
                    {
                        mdiChild.maximizeButton.Visibility = Visibility.Hidden;
                    }
                }
            }
        }

        /// <summary>
        /// Dependency property event once the maximize box value has changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void MaximizeBoxValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            MdiChild mdiChild = (MdiChild)sender;
            bool visible = (bool)e.NewValue;

            if (visible)
            {
                bool minimizeVisible = true;

                if (mdiChild.minimizeButton != null)
                {
                    minimizeVisible = mdiChild.minimizeButton.Visibility == Visibility.Visible;
                }

                if (mdiChild.maximizeButton != null)
                {
                    mdiChild.maximizeButton.IsEnabled = true;
                }

                if (!minimizeVisible)
                {
                    if (mdiChild.maximizeButton != null)
                    {
                        mdiChild.minimizeButton.Visibility = Visibility.Visible;
                    }

                    if (mdiChild.maximizeButton != null)
                    {
                        mdiChild.maximizeButton.Visibility = Visibility.Visible;
                    }
                }
            }
            else
            {
                bool minimizeEnabled = true;

                if (mdiChild.minimizeButton != null)
                {
                    minimizeEnabled = mdiChild.minimizeButton.IsEnabled;
                }

                if (mdiChild.maximizeButton != null)
                {
                    mdiChild.maximizeButton.IsEnabled = false;
                }

                if (!minimizeEnabled)
                {
                    if (mdiChild.maximizeButton != null)
                    {
                        mdiChild.minimizeButton.Visibility = Visibility.Hidden;
                    }

                    if (mdiChild.maximizeButton != null)
                    {
                        mdiChild.maximizeButton.Visibility = Visibility.Hidden;
                    }
                }
            }
        }

        /// <summary>
        /// Dependency property event once the close box value has changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void CloseBoxValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            MdiChild mdiChild = (MdiChild)sender;
            bool visible = (bool)e.NewValue;

            if (visible)
            {
                if ((mdiChild.closeButton != null) && (mdiChild.closeButton.Visibility != Visibility.Visible))
                {
                    mdiChild.closeButton.Visibility = Visibility.Visible;
                }
            }
            else
            {
                if ((mdiChild.closeButton != null) && (mdiChild.closeButton.Visibility == Visibility.Visible))
                {
                    mdiChild.closeButton.Visibility = Visibility.Collapsed;
                }
            }
        }

        /// <summary>
        /// Dependency property event once the windows state value has changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void WindowStateValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            MdiChild mdiChild = (MdiChild)sender;
            MdiContainer mdiContainer = mdiChild.Container;

            WindowState previousWindowState = (WindowState)e.OldValue;
            WindowState windowState = (WindowState)e.NewValue;

            if (mdiChild.Container == null || previousWindowState == windowState)
            {
                return;
            }

            if (previousWindowState == WindowState.Maximized)
            {
                if (mdiContainer.ActiveMdiChild != null && mdiContainer.ActiveMdiChild.WindowState != WindowState.Maximized)
                {
                    for (int i = 0; i < mdiContainer.Children.Count; i++)
                    {
                        if (mdiContainer.Children[i] != mdiChild && mdiContainer.Children[i].WindowState == WindowState.Maximized && mdiContainer.Children[i].MaximizeBox)
                        {
                            mdiContainer.Children[i].WindowState = WindowState.Normal;
                        }
                    }

                    ScrollViewer sv = (ScrollViewer)((Grid)mdiContainer.Content).Children[1];
                    sv.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
                    sv.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
                }

                mdiChild.Buttons.Children.Clear();
                mdiChild.Buttons = null;
                mdiChild.buttonsPanel.Children.Add(mdiChild.minimizeButton);
                mdiChild.buttonsPanel.Children.Add(mdiChild.maximizeButton);
                mdiChild.buttonsPanel.Children.Add(mdiChild.closeButton);
            }

            if (previousWindowState == WindowState.Minimized)
            {
                mdiChild.minimizedPosition = mdiChild.Position;
                mdiChild.MinWidth = mdiChild.originalMinimumSize.Width;
                mdiChild.MinHeight = mdiChild.originalMinimumSize.Height;
            }

            switch (windowState)
            {
                case WindowState.Normal:
                    {
                        mdiChild.Position = new Point(mdiChild.originalDimension.X, mdiChild.originalDimension.Y);
                        mdiChild.Width = mdiChild.originalDimension.Width;
                        mdiChild.Height = mdiChild.originalDimension.Height;
                    }
                    break;
                case WindowState.Minimized:
                    {
                        if (previousWindowState == WindowState.Normal)
                        {
                            mdiChild.originalDimension = new Rect(mdiChild.Position.X, mdiChild.Position.Y, mdiChild.ActualWidth, mdiChild.ActualHeight);
                        }

                        mdiChild.originalMinimumSize = new Size(mdiChild.MinWidth, mdiChild.MinHeight);
                        mdiChild.MinWidth = 0;
                        mdiChild.MinHeight = 0;

                        double newLeft, newTop;
                        if (mdiChild.minimizedPosition.X >= 0 || mdiChild.minimizedPosition.Y >= 0)
                        {
                            newLeft = mdiChild.minimizedPosition.X;
                            newTop = mdiChild.minimizedPosition.Y;
                        }
                        else
                        {
                            List<Rect> minimizedWindows = new List<Rect>();
                            for (int i = 0; i < mdiContainer.Children.Count; i++)
                            {
                                if ((MdiChild)mdiContainer.Children[i] != mdiChild && mdiContainer.Children[i].WindowState == WindowState.Minimized)
                                {
                                    minimizedWindows.Add(new Rect(mdiContainer.Children[i].Position.X, mdiContainer.InnerHeight - mdiContainer.Children[i].Position.Y, mdiContainer.Children[i].Width, mdiContainer.Children[i].Height));
                                }
                            }
                            Rect newWindowPlace;
                            int count = 0,
                                capacity = Convert.ToInt32(mdiContainer.ActualWidth) / MinimizedWidth;
                            bool occupied;
                            do
                            {
                                int row = count / capacity + 1,
                                    col = count % capacity;
                                newTop = MinimizedHeight * row;
                                newLeft = MinimizedWidth * col;

                                newWindowPlace = new Rect(newLeft, newTop, MinimizedWidth, MinimizedHeight);
                                occupied = false;
                                foreach (Rect rect in minimizedWindows)
                                {
                                    rect.Intersect(newWindowPlace);
                                    if (rect != Rect.Empty && rect.Width > 0 && rect.Height > 0)
                                    {
                                        occupied = true;
                                        break;
                                    }
                                }
                                count++;

                                // TODO: handle negative Canvas coordinates somehow.
                                if (newTop < 0)
                                {
                                    // ugly workaround for now.
                                    newTop = 0;
                                    occupied = false;
                                }

                            } while (occupied);

                            newTop = mdiContainer.InnerHeight - newTop;
                        }

                        mdiChild.Position = new Point(newLeft, newTop);

                        mdiChild.Width = MinimizedWidth;
                        mdiChild.Height = MinimizedHeight;
                    }
                    break;
                case WindowState.Maximized:
                    {
                        if (previousWindowState == WindowState.Normal)
                        {
                            mdiChild.originalDimension = new Rect(mdiChild.Position.X, mdiChild.Position.Y, mdiChild.ActualWidth, mdiChild.ActualHeight);
                        }

                        mdiChild.NonMaximizedState = previousWindowState;

                        mdiChild.buttonsPanel.Children.Clear();
                        StackPanel sp = new StackPanel { Orientation = Orientation.Horizontal };
                        sp.Children.Add(mdiChild.minimizeButton);
                        sp.Children.Add(mdiChild.maximizeButton);
                        sp.Children.Add(mdiChild.closeButton);
                        mdiChild.Buttons = sp;

                        mdiChild.Position = new Point(0, 0);
                        mdiChild.Width = mdiContainer.ActualWidth;
                        mdiChild.Height = mdiContainer.ActualHeight - 2;

                        ScrollViewer sv = (ScrollViewer)((Grid)mdiContainer.Content).Children[1];
                        sv.HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden;
                        sv.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
                    }
                    break;
            }
            if (mdiContainer.ActiveMdiChild == mdiChild)
            {
                mdiContainer.Buttons = mdiChild.Buttons;
            }

            mdiContainer.InvalidateSize();
        }

        #endregion

        /// <summary>
        /// Set focus to the child window and brings into view.
        /// </summary>
        public new void Focus()
        {
            SetValue(IsActiveProperty, true);
            Container.ActiveMdiChild = this;
        }

        /// <summary>
        /// Sets WindowState to previous non-maximized value.
        /// </summary>
        internal void Unmaximize()
        {
            if (WindowState == WindowState.Maximized)
            {
                WindowState = NonMaximizedState;
            }
        }

        /// <summary>
        /// Manually closes the child window.
        /// </summary>
        public void Close()
        {
            if (Buttons != null)
            {
                Buttons.Children.Clear();
            }

            Unmaximize(); //needed, so that the next window in the container is not maximized
            Container.Children.Remove(this);
        }
    }
}