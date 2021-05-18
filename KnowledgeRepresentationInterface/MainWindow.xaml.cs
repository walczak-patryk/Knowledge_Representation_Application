using KnowledgeRepresentationInterface.General;
using KnowledgeRepresentationInterface.Queries;
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
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Action = KR_Lib.DataStructures.Action;

namespace KnowledgeRepresentationInterface
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        PossibleScenarioQuery PSQ;
        ActionQuery AQ;
        FormulaQuery FQ;
        TargetQuery TQ;
        ScenarioGUI scenario;
        List<Action> actions;
        List<Fluent> fluents;
        List<ScenarioGUI> scenarios;
        ObservationCreator scenario_obs;
        

        public MainWindow()
        {
            InitializeComponent();
            this.scenario = new ScenarioGUI();
            this.scenarios = new List<ScenarioGUI>();
            this.actions = new List<Action>();
            this.fluents = new List<Fluent>();
            this.scenario_obs = new ObservationCreator(this.fluents);
            Initialize_Query_Types();
            Scenario_ListView.ItemsSource = this.scenario.items;
            Query_GroupBox.Content = this.PSQ;

            this.actions.Add(new Action("action1",0,2));
            this.actions.Add(new Action("action2",0,4));
            this.fluents.Add(new Fluent("fluent1", true));
            this.fluents.Add(new Fluent("fluent2", true));
            foreach(var elem in this.actions)
            {
                TreeViewItem ele = new TreeViewItem();
                ele.Header = elem.Name;
                ele.Tag = elem.Id;
                Actions_TreeViewItem.Items.Add(ele);
            }
            foreach (var elem in this.fluents)
            {
                TreeViewItem ele = new TreeViewItem();
                ele.Header = elem.Name;
                ele.Tag = elem.Id;
                Fluents_TreeViewItem.Items.Add(ele);
            }
            Action_Occurences_ComboBox.ItemsSource = this.actions;
            Query_Scenario_ComboBox.ItemsSource = this.scenarios;
            Observation_Scenario_GroupBox.Content = this.scenario_obs;
        }

        private void Initialize_Query_Types()
        {
            this.PSQ = new PossibleScenarioQuery();
            this.AQ = new ActionQuery();
            this.FQ = new FormulaQuery(this.fluents);
            this.TQ = new TargetQuery(this.fluents);
        }

        private void Delete_TreeView_Click(object sender, RoutedEventArgs e)
        {
            foreach(TreeViewItem item in Actions_TreeViewItem.Items)
            {
                if(item.IsSelected)
                {
                    Actions_TreeViewItem.Items.Remove(item);
                    this.actions.RemoveAll(x => x.Id.ToString() == item.Tag.ToString());
                    Action_Occurences_ComboBox.Items.Refresh();
                    this.AQ.Set_Actions(this.actions);
                    return;
                }
            }
            foreach (TreeViewItem item in Fluents_TreeViewItem.Items)
            {
                if (item.IsSelected)
                {
                    Fluents_TreeViewItem.Items.Remove(item);
                    this.fluents.RemoveAll(x => x.Id.ToString() == item.Tag.ToString());
                    this.scenario_obs.RefreshControl();
                    this.TQ.Refresh_Fluents();
                    this.FQ.Refresh_Fluents();
                    return;
                }
            }
            foreach (TreeViewItem item in Statements_TreeViewItem.Items)
            {
                if (item.IsSelected)
                {
                    Statements_TreeViewItem.Items.Remove(item);
                    return;
                }
            }
            foreach (TreeViewItem item in Scenarios_TreeViewItem.Items)
            {
                if (item.IsSelected)
                {
                    Scenarios_TreeViewItem.Items.Remove(item);
                    this.scenarios.RemoveAll(x => x.Id.ToString() == item.Tag.ToString());
                    Query_Scenario_ComboBox.ItemsSource = this.scenarios;
                    Query_Scenario_ComboBox.Items.Refresh();
                    return;
                }
            }

        }

        private void Delete_Scenario_ListView_Click(object sender, RoutedEventArgs e)
        {
            foreach(ScenarioItem item in Scenario_ListView.SelectedItems)
            {
                this.scenario.items.RemoveAll(x => x.Id == item.Id);
            }
            Scenario_ListView.Items.Refresh();

        }

        private void Panel_TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ExecuteQuery_Click(object sender, RoutedEventArgs e)
        {
        }

        private void Query_Type_ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(Query_GroupBox == null)
            {
                return;
            }
            switch(Query_Type_ComboBox.SelectedIndex)
            {
                case 0:
                    Query_GroupBox.Content = this.PSQ;
                    break;
                case 1:
                    Query_GroupBox.Content = this.AQ;
                    this.AQ.Set_Actions(this.actions);
                    break;
                case 2:
                    Query_GroupBox.Content = this.FQ;
                    this.FQ.Refresh_Fluents();
                    break;
                case 3:
                    Query_GroupBox.Content = this.TQ;
                    this.TQ.Refresh_Fluents();
                    break;
            }
        }

        private void Finish_Scenario_Click(object sender, RoutedEventArgs e)
        {
            if (ScenarioName_TextBox.Text.Length==0)
            {
                MessageBox.Show("The scenario name cannot be empty!");
                return;
            }
            if (!ScenarioName_TextBox.Text.All(char.IsLetterOrDigit))
            {
                MessageBox.Show("The scenario name should contain only alphanumeric symbols!");
                return;
            }
            foreach (var elem in this.scenarios)
            {
                if(elem.name == ScenarioName_TextBox.Text)
                {
                    MessageBox.Show("A scenario with this name already exists!");
                    return;
                }
            }
            this.scenario.name = ScenarioName_TextBox.Text;
            this.scenarios.Add(this.scenario);
            TreeViewItem new_scenario = new TreeViewItem();
            new_scenario.Header = ScenarioName_TextBox.Text;
            new_scenario.Tag = scenario.Id.ToString();
            foreach(var item in this.scenario.items)
            {
                TreeViewItem new_subitem = new TreeViewItem();
                new_subitem.Header = item.Id;
                new_subitem.Tag = item.Id;
                new_scenario.Items.Add(new_subitem);
            }
            this.scenario = new ScenarioGUI();
            Scenario_ListView.ItemsSource = this.scenario.items;
            Scenario_ListView.Items.Refresh();
            Scenarios_TreeViewItem.Items.Add(new_scenario);
            Query_Scenario_ComboBox.Items.Refresh();

        }

        private void Action_Occurences_Button_Click(object sender, RoutedEventArgs e)
        {
            Action action = (Action)Action_Occurences_ComboBox.SelectedItem;
            if(action == null)
            {
                MessageBox.Show("You have to select an action to add!");
                return;
            }
            int moment = (int)Action_Occurences_UIntUpDown.Value;

            this.scenario.items.Add(new ScenarioItem(moment.ToString(),action.Name, "", ""));
            Scenario_ListView.Items.Refresh();
        }

        private void Observations_Button_Click(object sender, RoutedEventArgs e)
        {
            if (scenario_obs.IsEmpty())
            {
                MessageBox.Show("The observation is empty!");
                return;
            }
            int moment = (int)Observations_UIntUpDown.Value;

            this.scenario.items.Add(new ScenarioItem(moment.ToString(), "", this.scenario_obs.GetContent(), ""));
            //List<ObservationElement> observation = infix_to_ONP(this.scenarioObservation);
            Scenario_ListView.Items.Refresh();
        }        
    }
}
