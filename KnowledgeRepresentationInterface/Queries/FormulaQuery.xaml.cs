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
    /// Interaction logic for FormulaQuery.xaml
    /// </summary>
    public partial class FormulaQuery : UserControl
    {
        public FormulaQuery()
        {
            InitializeComponent();
        }

        public void Set_Fluents(List<Fluent> fluents)
        {
            Fluent_Observation_ScenarioTab.ItemsSource = fluents;
            Fluent_Observation_ScenarioTab.Items.Refresh();
        }
    }
}
