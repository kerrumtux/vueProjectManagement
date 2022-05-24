using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManager.Models
{
    public class Time
    {
        public int id { get; set; }
        public string text { get; set; }
        public int numberOfHours { get; set; }
        public string date { get; set; }
        public int taskId { get; set; }
    }
}
