using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CustomLeap
{
    [Serializable]
    public class CustomFrame
{
        public float currentFrameRate { get; set; }
        public List<object> devices { get; set; }
        public List<object> gestures { get; set; }
        public List<Hand> hands { get; set; }
        public long id { get; set; }
        public InteractionBox interactionBox { get; set; }
        public List<Pointable> pointables { get; set; }
        public List<List<double>> r { get; set; }
        public double s { get; set; }
        public List<double> t { get; set; }
        public long timestamp { get; set; }
    }
}
