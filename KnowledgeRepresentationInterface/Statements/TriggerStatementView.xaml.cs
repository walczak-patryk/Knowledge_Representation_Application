using KnowledgeRepresentationInterface.General;
using KR_Lib.DataStructures;
using KR_Lib.Formulas;
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

namespace KnowledgeRepresentationInterface.Statements
{
    /// <summary>
    /// Interaction logic for TriggerStatement.xaml
    /// </summary>
    public partial class TriggerStatementView : UserControl
    {
        public static int numberOfTriggerStatements = 0;
        public ObservationCreator scenario_obs { get; set; }
        public TriggerStatementView(List<Fluent> fluents)
        {
            this.scenario_obs = new ObservationCreator(fluents);
            InitializeComponent();
            Observation_GroupBox.Content = this.scenario_obs;
        }

        public void Set_Actions(List<Action> actions)
        {
            TriggerStatementAction_ComboBox.ItemsSource = actions;
            TriggerStatementAction_ComboBox.Items.Refresh();
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
    }
}
