using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CustomLeap
{
    [Serializable]
    public class Pointable
{
        public List<List<List<float>>> bases { get; set; }
        public List<float> btipPosition { get; set; }
        public List<float> carpPosition { get; set; }
        public List<float> dipPosition { get; set; }
        public List<float> direction { get; set; }
        public bool extended { get; set; }
        public int handId { get; set; }
        public int id { get; set; }
        public float length { get; set; }
        public List<float> mcpPosition { get; set; }
        public List<float> pipPosition { get; set; }
        public List<float> stabilizedTipPosition { get; set; }
        public float timeVisible { get; set; }
        public List<float> tipPosition { get; set; }
        public List<float> tipVelocity { get; set; }
        public bool tool { get; set; }
        public double touchDistance { get; set; }
        public string touchZone { get; set; }
        public int type { get; set; }
        public float width { get; set; }
    }
}
