using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using System.IO;
using System.Data;

namespace TrackerWatch2.Models
{
    public class WatchModel
    {
        public PersonModel Person { get; set; }
        public WatchModel(PersonModel person)
        {
            AutoAction();
        }
        public void CollectData(object sender, ElapsedEventArgs e)
        {
            Person = (PersonModel)sender;

            // Because there are int and double in the data we want to save, use non-generic collection type
            ArrayList dailyData = new ArrayList();
            dailyData.Add(Person.Steps);
            dailyData.Add(Person.MilesPerDay);
            dailyData.Add(Person.AverageHRPerMin);
            dailyData.Add(Person.MinHRPerMin);
            dailyData.Add(Person.MaxHRPerMin);

            // Save data to a file
            SaveToFile(dailyData);
        }
        private void AutoAction() 
        {
            // Creating a timer with 24 hour interval
            DateTime presentTime = DateTime.Now;
            DateTime lastSecOfDay = DateTime.Now.Date.AddDays(1).AddSeconds(-1); // defining the last nanosecond of the day   
            double remainingTime = (double)(lastSecOfDay - presentTime).TotalMilliseconds;

            Timer timing24Hour = new Timer(remainingTime); // Any given time in milliseconds between current time and the last millisecond of the day         
            timing24Hour.Enabled = true; // Enable Timer.Elapsed event
            timing24Hour.Elapsed += CollectData; // Record relevant data into a list at the end of every day
            timing24Hour.AutoReset = true; // Reset the event automatically after every recording of the events
        }
        private void SaveToFile(ArrayList items)
        {
            List<string> rowLine = new List<string>();

            // The row names arraylist must match the sequence in which the data is added in CollectData() method
            // The first data is empty string to leave space for the column header (date)
            ArrayList columnHeaders = new ArrayList()
            { "          ", "Steps", "Mileage", "AVG HR", "Min HR", "Max HR" };
            string firstRow = "";
            foreach (var item in columnHeaders)
            {
                firstRow += $" {item}";
            }
            rowLine.Add(firstRow);
                       
            // columnhead variable indicates the date of the data saved
            string newRow = DateTime.Now.Date.ToString("d");
            // Appending each data from the input to the row value        
            foreach (var item in items)
            {
                newRow += $"  {item}  ";
            }
            rowLine.Add(newRow);
            
            // Generating output           
            string filePath = "DataRecord.csv";
            File.WriteAllLines(filePath, rowLine);
        }
    }
}
