using System;
using System.Collections.Generic;
using System.Text;

namespace TrackerWatch2
{
    public class DetectNumEventArgs : EventArgs
    {
        public int NumberDetected { get; set; }
        public DetectNumEventArgs(int numberDetected)
        {
            NumberDetected = numberDetected;
        }
    }
}
