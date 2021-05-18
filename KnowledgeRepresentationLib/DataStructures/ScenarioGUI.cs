using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KR_Lib.DataStructures
{
    public class ScenarioGUI
    {
        public Guid Id { get; set; }
        public string name { get; set; }
        public List<ScenarioItem> items { get; set; }

        public ScenarioGUI()
        {
            this.Id = Guid.NewGuid();
            this.items = new List<ScenarioItem>();
        }

        public override string ToString()
        {
            return name;
        }
    }
}
