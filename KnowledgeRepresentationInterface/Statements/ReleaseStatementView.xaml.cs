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
using Action = KR_Lib.DataStructures.Action;
using KR_Lib.DataStructures;
using KnowledgeRepresentationInterface.General;
using KR_Lib.Formulas;

namespace KnowledgeRepresentationInterface.Statements
{
    /// <summary>
    /// Interaction logic for ReleaseStatement.xaml
    /// </summary>
    public partial class ReleaseStatementView : UserControl
    {
        public ObservationCreator scenario_obs { get; set; }
        public ReleaseStatementView(List<Fluent> fluents)
        {
            this.scenario_obs = new ObservationCreator(fluents);
            InitializeComponent();
            Observation_GroupBox.Content = this.scenario_obs;
        }

        public void Refresh_Fluents()
        {
            this.scenario_obs.RefreshControl();
        }

        public IFormula Get_Formula()
        {
            if (scenario_obs.IsEmpty())
            {
                return null;
            }
            List<ObservationElement> observation = this.scenario_obs.scenarioObservation;
            observation = FormulaParser.infix_to_ONP(observation);
            IFormula formula = FormulaParser.ParseToFormula(observation);
            return formula;
        }

        public void Set_Actions_And_Fluents(List<Action> actions, List<Fluent> fluents)
        {
            ReleaseStatementActions_ComboBox.ItemsSource = actions;
            ReleaseStatementActions_ComboBox.Items.Refresh();

            ReleaseStatementFluents_ComboBox.ItemsSource = fluents;
            ReleaseStatementFluents_ComboBox.Items.Refresh();
        }
    }
}
