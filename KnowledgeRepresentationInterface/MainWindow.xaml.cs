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
        Scenario scenario;
        List<Action> actions;
        List<Fluent> fluents;
        List<Scenario> scenarios;
        List<ObservationElement> scenarioObservation;

        public MainWindow()
        {
            Initialize_Query_Types();
            InitializeComponent();
            this.scenario = new Scenario();
            this.scenarios = new List<Scenario>();
            this.actions = new List<Action>();
            this.fluents = new List<Fluent>();
            Scenario_ListView.ItemsSource = this.scenario.items;
            Query_GroupBox.Content = this.PSQ;
            scenarioObservation = new List<ObservationElement>();
            //List<ScenarioItem> test = new List<ScenarioItem>();
            //Scenario_ListView.ItemsSource = test;
            //TreeViewItem t = new TreeViewItem();
            //t.Header = "Fluent II";
            //TreeViewItem t2 = new TreeViewItem();
            //t2.Header = "Option";
            //t.Items.Add(t2);
            //Fluents_TreeViewItem.Items.Add(t);
            //for(int i=0;i<50;i++) 
            //{
            //    TreeViewItem t3 = new TreeViewItem();
            //    t3.Header = "Fluent " + i.ToString();
            //    ScenarioItem item = new ScenarioItem("1","2", "3", "4");
            //    t3.Tag = item.Id;
            //    Fluents_TreeViewItem.Items.Add(t3);
            //    test.Add(item);
            //}
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
            Fluent_Observation_ScenarioTab.ItemsSource = this.fluents;
            Query_Scenario_ComboBox.ItemsSource = this.scenarios;
        }

        private void Initialize_Query_Types()
        {
            this.PSQ = new PossibleScenarioQuery();
            this.AQ = new ActionQuery();
            this.FQ = new FormulaQuery();
            this.TQ = new TargetQuery();
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
                    Fluent_Observation_ScenarioTab.Items.Refresh();
                    this.TQ.Set_Fluents(this.fluents);
                    this.FQ.Set_Fluents(this.fluents);
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
            //List<ScenarioItem> tmp_ListView = (List<ScenarioItem>)Scenario_ListView.ItemsSource;
            foreach(ScenarioItem item in Scenario_ListView.SelectedItems)
            {
                this.scenario.items.RemoveAll(x => x.Id == item.Id);
                //tmp_ListView.Remove(item);
            }
            //Scenario_ListView.ItemsSource = tmp_ListView;
            //Scenario_ListView.ItemsSource = this.scenario.items;
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
                    this.FQ.Set_Fluents(this.fluents);
                    break;
                case 3:
                    Query_GroupBox.Content = this.TQ;
                    this.TQ.Set_Fluents(this.fluents);
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
            this.scenario = new Scenario();
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
            if (scenarioObservation.Count==0)
            {
                MessageBox.Show("The observation is empty!");
                return;
            }
            int moment = (int)Observations_UIntUpDown.Value;

            this.scenario.items.Add(new ScenarioItem(moment.ToString(), "", Observations_TextBox.Text, ""));
            List<ObservationElement> observation = infix_to_ONP(this.scenarioObservation);
            Scenario_ListView.Items.Refresh();
        }

        private void And_Scenario_Click(object sender, RoutedEventArgs e)
        {
            Observations_TextBox.Text += "AND ";
            scenarioObservation.Add(new ObservationElement(false, null, 4,  "AND"));
        }

        private void Or_Scenario_Click(object sender, RoutedEventArgs e)
        {
            Observations_TextBox.Text += "OR ";
            scenarioObservation.Add(new ObservationElement(false, null, 3, "OR"));
        }

        private void Not_Scenario_Click(object sender, RoutedEventArgs e)
        {
            Observations_TextBox.Text += "NOT ";
            scenarioObservation.Add(new ObservationElement(false, null, 4, "NOT"));
        }

        private void Im_Scenario_Click(object sender, RoutedEventArgs e)
        {
            Observations_TextBox.Text += "=> ";
            scenarioObservation.Add(new ObservationElement(false, null, 3, "=>"));
        }

        private void Eq_Scenario_Click(object sender, RoutedEventArgs e)
        {
            Observations_TextBox.Text += "<=> ";
            scenarioObservation.Add(new ObservationElement(false, null, 4, "<=>"));
        }

        private void Left_Scenario_Click(object sender, RoutedEventArgs e)
        {
            Observations_TextBox.Text += "( ";
            scenarioObservation.Add(new ObservationElement(false, null, 2, "("));
        }

        private void Right_Scenario_Click(object sender, RoutedEventArgs e)
        {
            Observations_TextBox.Text += ") ";
            scenarioObservation.Add(new ObservationElement(false, null, 2, ")"));
        }

        private void Erase_Scenario_Click(object sender, RoutedEventArgs e)
        {
            if(scenarioObservation.Count==0)
            {
                return;
            }
            ObservationElement element = scenarioObservation[scenarioObservation.Count - 1];
            Observations_TextBox.Text = Observations_TextBox.Text.Remove(Observations_TextBox.Text.Length - element.length, element.length);
            scenarioObservation.RemoveAt(scenarioObservation.Count - 1);
        }

        private void Add_Fluent_Observation_ScenarioTab_Click(object sender, RoutedEventArgs e)
        {
            int index = (int)Fluent_Observation_ScenarioTab.SelectedIndex;
            if (index < 0)
            {
                return;
            }
            
            Observations_TextBox.Text += this.fluents[index].ToString() + " ";
            scenarioObservation.Add(new ObservationElement(true, this.fluents[index], this.fluents[index].ToString().Length + 1, null));
        }

        void print_obs(List<ObservationElement> observation)
        {
            StringBuilder sb = new StringBuilder();
            foreach(var elem in observation)
            {
                if(elem.isFluent)
                {
                    sb.Append(elem.fluent.Name + " ");
                }
                else
                {
                    sb.Append(elem.operator_ + " ");
                }
                
            }
            MessageBox.Show(sb.ToString());
        }

        
    }

    public class ScenarioItem
    {
        public Guid Id { get; set; }
        public string Moment { get; set; }
        public string ActionOccurence { get; set; }
        public (Action,int) ActionDetails { get; set; }
        public string Observation { get; set; }
        public string Duration { get; set; }

        public ScenarioItem(string Moment, string ActionOccurence, string Observation, string Duration)
        {
            this.Id = Guid.NewGuid();
            this.Moment = Moment;
            this.ActionOccurence = ActionOccurence;
            this.Observation = Observation;
            this.Duration = Duration;
        }
    }

    public class Scenario
    {
        public Guid Id { get; set; }
        public string name { get; set; }
        public List<ScenarioItem> items { get; set; }

        public Scenario()
        {
            this.Id = Guid.NewGuid();
            this.items = new List<ScenarioItem>();
        }

        public override string ToString()
        {
            return name;
        }
    }

    public class ObservationElement
    {
        public bool isFluent { get; set; }
        public Fluent fluent { get; set; }
        public int length { get; set; }
        public string operator_ { get; set; }

        public ObservationElement(bool isFluent, Fluent fluent, int length, string operator_)
        {
            this.isFluent = isFluent;
            this.fluent = fluent;
            this.length = length;
            this.operator_ = operator_;
        }
    }
}
