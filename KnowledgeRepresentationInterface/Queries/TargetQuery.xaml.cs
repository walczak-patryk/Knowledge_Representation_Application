using KnowledgeRepresentationInterface.General;
using KR_Lib.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KnowledgeRepresentationInterface.Queries
{
    /// <summary>
    /// Interaction logic for TargetQuery.xaml
    /// </summary>
    public partial class TargetQuery : UserControl
    {
        public ObservationCreator scenario_obs { get; set; }

        public TargetQuery(List<Fluent> fluents)
        {
            this.scenario_obs = new ObservationCreator(fluents);
            InitializeComponent();
            Observation_GroupBox.Content = this.scenario_obs;
        }

        public void Refresh_Fluents()
        {
            this.scenario_obs.RefreshControl();
        }
    }
}
