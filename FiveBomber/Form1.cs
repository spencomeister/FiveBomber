using System;
using System.Threading;
using System.Timers;
using System.Windows.Forms;

namespace FiveBomber
{
    public partial class Form1 : Form
    {
        // This field stores an array of ProgressBar controls.
        private readonly ProgressBar[] _progressBars;
        // This field stores a reference to the System.Threading.Timer object.
        private readonly System.Threading.Timer _timerField;
        // This field stores a reference to the currently running System.Threading.Timer object.
        private System.Threading.Timer _timer;
        // This field stores an array of boolean values indicating whether each ProgressBar is processing.
        private readonly bool[] _isProcessing;

        // This method is called when the timer fires.
        private void OnTimer(object state)
        {
            // Iterate through each ProgressBar.
            for (int i = 0; i < _progressBars.Length; i++)
            {
                // If the ProgressBar is processing and its value is less than 100,
                if (_isProcessing[i] && _progressBars[i].Value < 100)
                {
                    // Invoke the ProgressBar's Value property setter on the main thread.
                    _progressBars[i].Invoke((MethodInvoker)delegate
                    {
                        // Increment the ProgressBar's value.
                        _progressBars[i].Value++;
                    });
                }

                // If the ProgressBar's value has reached 100,
                if (_progressBars[i].Value == 100)
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

            // Initialize the _isProcessing array with a boolean value of true for each ProgressBar.
            _isProcessing = new bool[_progressBars.Length];
            for (int i = 0; i < _isProcessing.Length; i++)
            {
                _isProcessing[i] = true;
            }
        }

        // This method is called when the Start/Stop button is clicked.
        private void btnStartStop_Click(object sender, EventArgs e)
        {
            // If the timer is not null,
            if (_timer != null)
            {
                // Stop the timer.
                _timer.Change(Timeout.Infinite, Timeout.Infinite);
                // Dispose the timer.
                _timer.Dispose();
                // Set the _timer field to null.
                _timer = null;
            }
            else
            {
                // Create a new System.Threading.Timer object that fires every 1000 milliseconds.
                _timer = new System.Threading.Timer(OnTimer, null, 1000, 1000);
            }

            // Set the processing flag for each ProgressBar to true.
            for (int i = 0; i < _isProcessing.Length; i++)
            {
                _isProcessing[i] = true;
            }
        }

        // This method is called when the Next button is clicked.
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

        // This method is called when the Reset button is clicked.
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
