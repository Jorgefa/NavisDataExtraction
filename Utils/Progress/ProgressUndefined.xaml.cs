using System;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace NavisDataExtraction.Utils.Progress
{
    public partial class ProgressUndefined : Window
    {
        public ProgressUndefined()
        {
            InitializeComponent();
        }


        public void UpdateProgress(string text)
        {
            Dispatcher.Invoke(new Action<string>(
                delegate(string t) { ProgressText.Text = t; }), DispatcherPriority.Background, text);
        }

        private void BorderMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }
    }


    public static class ProgressUtilUndefined
    {
        private static ProgressUndefined _window;
        private static EventWaitHandle _eventWaitHandle;

        /// <summary>
        /// Starts new CPU thread with progress window.
        /// </summary>
        public static void Start()
        {
            using (_eventWaitHandle = new AutoResetEvent(false))
            {
                var newProgressWindowThread = new Thread(ShowProgressWindow);
                newProgressWindowThread.SetApartmentState(ApartmentState.STA);
                newProgressWindowThread.IsBackground = true;
                newProgressWindowThread.Start();

                _eventWaitHandle.WaitOne();
            }
        }

        /// <summary>
        /// Closes progress window after finishing updates.
        /// </summary>
        public static void Finish()
        {
            _window.Dispatcher.Invoke(_window.Close);
        }

        /// <summary>
        /// Updates progress window.
        /// </summary>
        /// <param name="updateMessage">Update window text</param>
        public static void Update(string updateMessage)
        {
            _window.UpdateProgress(updateMessage);
        }

        /// <summary>
        /// Shows new progress window
        /// </summary>
        private static void ShowProgressWindow()
        {
            _window = new ProgressUndefined();
            _window.Show();

            _window.Closed += WindowOnClosed;
            _eventWaitHandle.Set();

            Dispatcher.Run();
        }

        /// <summary>
        /// Calls CPU thread shutdown when window closes
        /// </summary>
        private static void WindowOnClosed(object sender, EventArgs e)
        {
            Dispatcher.CurrentDispatcher.InvokeShutdown();
        }
    }
}