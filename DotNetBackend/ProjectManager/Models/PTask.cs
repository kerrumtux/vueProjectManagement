using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManager.Models
{
    public class PTask
    {
        public int id { get; set; }
        public string text { get; set; }
        public bool active { get; set; }
        public int projectId { get; set; }
    }
}
