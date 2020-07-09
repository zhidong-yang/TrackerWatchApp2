using System;
using System.Collections;
using System.Collections.Generic;
using TrackerWatch2.Models;

namespace TrackerWatch2
{
    class Program
    {
        static void Main(string[] args)
        {
            PersonModel person = new PersonModel();
            WatchModel watch = new WatchModel(person);
            person.Swipe(SwipeDirection.NoSwipe);
           
            // Testing:
            person.DetectSteps();
            person.DetectHeartRate();
            person.DetectSteps();
            person.DetectSteps();
            person.DetectHeartRate(); 
            person.Swipe(SwipeDirection.Up);
            person.Swipe(SwipeDirection.Down);

        }

        public static void MainDisplay(object sender, DetectNumEventArgs e)
        {
            Console.WriteLine("---------------------------");
            Console.WriteLine($"    {TimeModel.Hour} " +
                $"\n    {TimeModel.Minute}" +
                $"\n{TimeModel.DayOfTheWeek.ToString().Substring(0, 3)} {TimeModel.DayOfMonth}");
            Console.WriteLine("---------------------------");

            // Connecting diplay at the event of swiping up or down
            PersonModel person = (PersonModel)sender;            
            person.SwipingDown += DisplaySteps;
            person.SwipingUp += DisplayDailyDistance;
        }

        public static void DisplaySteps(object sender, DetectNumEventArgs e)
        {
            PersonModel person = (PersonModel)sender;
            Console.WriteLine("---------------------------");
            Console.WriteLine($"    {person.Steps} steps   ");
            Console.WriteLine("---------------------------");

            // Connecting diplay at the event of swiping up or down
            person.SwipingUp += MainDisplay;
            person.SwipingDown += DisplayHR;
        }
        public static void DisplayHR(object sender, DetectNumEventArgs e)
        {
            PersonModel person = (PersonModel)sender;

            Console.WriteLine("---------------------------"); 
            Console.WriteLine($"    {person.HeartBeat}    ");
            Console.WriteLine("---------------------------");

            // Connecting diplay at the event of swiping up or down
            person.SwipingDown += DisplayDailyDistance;
            person.SwipingUp += DisplaySteps;
        }
        public static void DisplayDailyDistance(object sender, DetectNumEventArgs e)
        {
            PersonModel person = (PersonModel)sender;
            // Connecting diplay at the event of swiping up or down
            person.SwipingUp += DisplayHR;
            person.SwipingDown += MainDisplay;

            Console.WriteLine("---------------------------");
            Console.WriteLine($"    {person.MilesPerDay:f2} Mi    ");
            Console.WriteLine("---------------------------");
        }
    }
}
