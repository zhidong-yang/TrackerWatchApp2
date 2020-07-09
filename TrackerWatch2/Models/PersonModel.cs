using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Timers;

namespace TrackerWatch2.Models
{
    public class PersonModel 
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        // property Steps are set by the event subscriber in the constructor and the event raiser in DetectSteps() method
        public int Steps { get; private set; }
        
        //private double _milesPerDay;
        public double MilesPerDay { get; private set; }

        public int HeartBeat { get; private set; }
        public List<int> DailyHRDataList()
        {
            List<int> output = new List<int>();

            // Every time when a heart rate is detected, the data is added to this list
            DetectingHeartRate += RecordingHeartRate;
            output.Add(HeartBeat);
            // Remove all the non-zero values sicne zero indicates heart rate is not properly detected
            output = output.Where(x => x != 0).ToList(); 

            return output;
        }
        
        private int _averageHRPerMin;
        public int AverageHRPerMin
        {
            get { return _averageHRPerMin; }
            set
            {
                _averageHRPerMin = Convert.ToInt32(DailyHRDataList().Average());
            }
        }
        private int _minHRPerMin;

        public int MinHRPerMin
        {
            get { return _minHRPerMin; }
            set
            {
                _minHRPerMin = DailyHRDataList().Min();
            }
        }

        private int _maxHRPerMin;
        public int MaxHRPerMin
        {
            get { return _maxHRPerMin; }
            set
            {
                _maxHRPerMin = DailyHRDataList().Max();
            }
        }

        // ctor: Using it to connect the event raiser to the property of the PersonModel instance
        public PersonModel()
        {
            NoSwiping += Program.MainDisplay; // initiate the default display on the watch
            SetOneDayTimer(); // Clear the steps to zero at the end of the day
            DetectingSteps += IncreasingSteps;
            DetectingHeartRate += RecordingHeartRate;
        }

        // EventHandlers to detect steps and heart rate (both properties), and swipe() method which affects the UI
        public event EventHandler<DetectNumEventArgs> DetectingSteps;
        public event EventHandler<DetectNumEventArgs> DetectingHeartRate;
        public event EventHandler<DetectNumEventArgs> SwipingUp;
        public event EventHandler<DetectNumEventArgs> SwipingDown;
        public event EventHandler<DetectNumEventArgs> NoSwiping;
        public int DetectSteps()
        {
            Console.Write("How many steps are detected: ");
            int output = Convert.ToInt32(Console.ReadLine());
            DetectingSteps?.Invoke(this, new DetectNumEventArgs(output));

            return output;
        }
        public void IncreasingSteps(object sender, DetectNumEventArgs e)
        {           
            Steps += e.NumberDetected;
            MilesPerDay += 0.000435606 * Steps; // An adult's stride is 2.3 feet per step, which is 0.0004356 mile
        }
        public int DetectHeartRate()
        {
            Console.Write("What is the heart rate in the past minute: ");
            int output = Convert.ToInt32(Console.ReadLine());
            DetectingHeartRate?.Invoke(this, new DetectNumEventArgs(output));

            return output;
        }
        public void RecordingHeartRate(object sender, DetectNumEventArgs e)
        {
            HeartBeat = e.NumberDetected;
        }
        public void Swipe(SwipeDirection direction)
        {
            int num = (int)direction;
            if (direction == SwipeDirection.Up)
            {
                SwipingUp?.Invoke(this, new DetectNumEventArgs(num));
            }
            else if (direction == SwipeDirection.Down)
            {
                SwipingDown?.Invoke(this, new DetectNumEventArgs(num));
            }
            else
            {
                NoSwiping?.Invoke(this, new DetectNumEventArgs(num));
            }
        }
        private static void ClearToZero(object sender, EventArgs e)
        {
            PersonModel person = (PersonModel)sender;
            person.Steps = 0; // Clear person instance's step to zero at the start of the day, using Timer.Elapsed event below            
        }
        private static void SetOneDayTimer()
        {
            // Creating a timer with 24 hour interval
            DateTime presentTime = DateTime.Now;
            DateTime startOfNextDay = DateTime.Now.Date.AddDays(1); // defining the firstsecond nanosecond of the day   
            double remainingTime = (double)(startOfNextDay - presentTime).TotalMilliseconds;

            Timer timing24Hour = new Timer(remainingTime); // Any given time in milliseconds between current time and the first second of the next day         
            timing24Hour.Enabled = true; // Enable Timer.Elapsed event
            timing24Hour.Elapsed += ClearToZero; // Record relevant data into a list at the end of every day
            timing24Hour.AutoReset = true; // Reset the event automatically after every recording of the events
        }



    }
}
