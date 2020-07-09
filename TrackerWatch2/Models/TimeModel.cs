using System;
using System.Collections.Generic;
using System.Text;

namespace TrackerWatch2.Models
{
    public class TimeModel 
    {
        public static int Hour => DateTime.Now.Hour;
        public static int Minute => DateTime.Now.Minute;
        public static DayOfWeek DayOfTheWeek => DateTime.Now.DayOfWeek;
        public static int DayOfMonth => DateTime.Now.Day;
        
    }
}
