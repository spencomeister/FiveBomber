using System;
using System.Threading;
using System.Timers;
using System.Windows.Forms;



namespace FiveBomber
{

    public partial class Form1 : Form
    {
        private readonly ProgressBar[] _progressBars;
        private readonly System.Threading.Timer _timerField;
        private System.Threading.Timer _timer;
        private readonly bool[] _isProcessing;
        private bool _isTimerDisposed = false;

        public const int MAX_VALUE = 15;
        public const int TIMER_INTERVAL = 1000;

        // This method is called when the timer fires.
        private void OnTimer(object state)
        {
            // Iterate through each ProgressBar.
            for (int i = 0; i < _progressBars.Length; i++)
            {
                // If the ProgressBar is processing and its value is less than MAX_VALUE,
                if (_isProcessing[i] && _progressBars[i].Value < MAX_VALUE)
                {
                    // Invoke the ProgressBar's Value property setter on the main thread.
                    _progressBars[i].Invoke((MethodInvoker)delegate
                    {
                        // Increment the ProgressBar's value.
                        _progressBars[i].Value++;
                    });
                }

                // If the ProgressBar's value has reached MAX_VALUE,
                if (_progressBars[i].Value == MAX_VALUE)
                {
                    // Set the processing flag for the ProgressBar to false.
                    _isProcessing[i] = false;
                    // Check if all ProgressBars have finished processing.
                    bool isAllStopped = true;
                    foreach (bool isProcessing in _isProcessing)
                    {
                        // If any ProgressBar is still processing,
                        if (isProcessing)
                        {
                            // Set the isAllStopped flag to false and break out of the loop.
                            isAllStopped = false;
                            break;
                        }
                    }

                    // If all ProgressBars have finished processing,
                    if (isAllStopped)
                    {
                        // Dispose the timer.
                        _timer.Dispose();
                        _isTimerDisposed = true;
    }
                }
            }
        }

        public Form1()
        {
            // Initialize the form.
            InitializeComponent();

            // Initialize the _progressBars array with the ProgressBar controls on the form.
            _progressBars = new[] {
                progressBar1, progressBar2, progressBar3, progressBar4, progressBar5
            };

            // Initialize all progressBars to be set maximum value.
            foreach (var progressBar in _progressBars)
            {
                progressBar.Maximum = MAX_VALUE;
            }

            // Initialize the _isProcessing array with a boolean value of true for each ProgressBar.
            _isProcessing = new bool[_progressBars.Length];
            for (int i = 0; i < _isProcessing.Length; i++)
            {
                _isProcessing[i] = true;
            }
        }

        private void btnStartStop_Click(object sender, EventArgs e)
        {
            // If the timer is not null,
            if (_timer != null)
            {
                // Stop the timer.
                if (_isTimerDisposed)
                {
                    _timer.Dispose();
                } else
                {
                    _timer.Change(Timeout.Infinite, Timeout.Infinite);
                }
                
                // Set the _timer field to null.
                _timer = null;
                _isTimerDisposed = true;
            }
            else
            {
                // Create a new System.Threading.Timer object that fires every TIMER_INTERVAL milliseconds.
                _timer = new System.Threading.Timer(OnTimer, null, TIMER_INTERVAL, TIMER_INTERVAL);
                _isTimerDisposed = false;
            }

            // Set the processing flag for each ProgressBar to true.
            for (int i = 0; i < _isProcessing.Length; i++)
            {
                _isProcessing[i] = true;
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            // Iterate through each ProgressBar.
            for (int i = 0; i < _progressBars.Length; i++)
            {
                // If the ProgressBar is processing,
                if (_isProcessing[i])
                {
                    // Set the processing flag for the ProgressBar to false.
                    _isProcessing[i] = false;

                    // Return from the method.
                    return;
                }
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            // Iterate through each ProgressBar.
            foreach (var progressBar in _progressBars)
            {
                // Set the ProgressBar's value to 0.
                progressBar.Value = 0;
            }
            // Set the processing flag for each ProgressBar to true.
            for (int i = 0; i < _isProcessing.Length; i++)
            {
                _isProcessing[i] = true;
            }
        }
    }
}
