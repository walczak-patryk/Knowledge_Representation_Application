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
    /// Interaction logic for CauseStatement.xaml
    /// </summary>
    public partial class CauseStatementView : UserControl
    {
        public static int numberOfCouseStatements = 0;

        public ObservationCreator scenario_obs { get; set; }
        public ObservationCreator scenario_obs2 { get; set; }
        public CauseStatementView()
        {
            InitializeComponent();
        }

        public CauseStatementView(List<Fluent> fluents)
        {
            this.scenario_obs = new ObservationCreator(fluents);
            this.scenario_obs2 = new ObservationCreator(fluents);
            InitializeComponent();
            Observation_GroupBox.Content = this.scenario_obs;
            Observation_GroupBox2.Content = this.scenario_obs2;
        }
        public void Refresh_Fluents()
        {
            this.scenario_obs.RefreshControl();
            this.scenario_obs2.RefreshControl();
        }

        public IFormula Get_First_Formula()
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
        public IFormula Get_Second_Formula()
        {
            if (scenario_obs2.IsEmpty())
            {
                return null;
            }
            List<ObservationElement> observation = this.scenario_obs2.scenarioObservation;
            observation = FormulaParser.infix_to_ONP(observation);
            IFormula formula = FormulaParser.ParseToFormula(observation);
            return formula;
        }

        public void Set_Actions(List<Action> actions)
        {
            CauseStatement_ComboBox.ItemsSource = actions;
            CauseStatement_ComboBox.Items.Refresh();
        }
    }
}
