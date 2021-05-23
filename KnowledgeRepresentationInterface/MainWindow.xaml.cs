using KnowledgeRepresentationInterface.Queries;
using KnowledgeRepresentationInterface.Statements;
using KnowledgeRepresentationInterface.General;
using KR_Lib.DataStructures;
using KR_Lib.Formulas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Action = KR_Lib.DataStructures.Action;
using KR_Lib;

namespace KnowledgeRepresentationInterface
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        IEngine engine;

        PossibleScenarioQuery PSQ;
        ActionQuery AQ;
        FormulaQuery FQ;
        TargetQuery TQ;
        ScenarioGUI scenario;
        List<Action> actions;
        List<Fluent> fluents;
        List<ScenarioGUI> scenarios;
        ObservationCreator scenario_obs;
        

        //statements
        CauseStatement CS;
        ImpossibleAtStatement IAS;
        ImpossibleIfStatement IIS;
        InvokeStatement IS;
        ReleaseStatement RS;
        TriggerStatement TS;

        public MainWindow()
        {
            this.engine = new Engine();
            InitializeComponent();
        
            this.scenario = new ScenarioGUI();
            this.scenarios = new List<ScenarioGUI>();
            this.actions = new List<Action>();
            this.fluents = new List<Fluent>();
            this.scenario_obs = new ObservationCreator(this.fluents);
            Initialize_Query_Types();
            Initialize_Statement_Types();
            Scenario_ListView.ItemsSource = this.scenario.items;
            Query_GroupBox.Content = this.PSQ;

            this.actions.Add(new Action("action1"));
            this.actions.Add(new Action("action2"));
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

        private void Initialize_Statement_Types()
        {
            this.CS = new CauseStatement();
            this.IAS = new ImpossibleAtStatement();
            this.IIS = new ImpossibleIfStatement();
            this.IS = new InvokeStatement();
            this.RS = new ReleaseStatement();
            this.TS = new TriggerStatement();
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
            if(Query_Scenario_ComboBox.SelectedIndex<0)
            {
                MessageBox.Show("You have to select a scenario!");
                return;
            }
            switch(Query_Type_ComboBox.SelectedIndex)
            {
                case 0:
                    QueryType qt = QueryType.Always;
                    if (this.PSQ.Type_ComboBox.SelectedIndex==1)
                    {
                        qt = QueryType.Ever;
                    }
                    KR_Lib.Queries.PossibleScenarioQuery query = new KR_Lib.Queries.PossibleScenarioQuery(qt, Guid.NewGuid());
                    break;
                case 1:

                    break;
                case 2:

                    break;
                case 3:

                    break;
            }
            Query_Scenario_ComboBox.SelectedIndex = -1;
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

        private void Statement_Type_ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Statement_GroupBox == null)
            {
                return;
            }
            switch (Statement_Type_ComboBox.SelectedIndex)
            {
                case 0:
                    Statement_GroupBox.Content = this.CS;
                    break;
                case 1:
                    Statement_GroupBox.Content = this.IAS;
                    break;
                case 2:
                    Statement_GroupBox.Content = this.IIS;
                    break;
                case 3:
                    Statement_GroupBox.Content = this.IS;
                    break;
                case 4:
                    Statement_GroupBox.Content = this.RS;
                    break;
                case 5:
                    Statement_GroupBox.Content = this.TS;
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
                if(item.ActionOccurence.Length==0)
                {
                    new_subitem.Header = "Observation: " + item.Observation;
                }
                else
                {
                    new_subitem.Header = "Action occurence: " + item.ActionOccurence;
                }
                
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
            int moment = 0;
            if(Action_Occurences_Moment_UIntUpDown.Value != null)
            {
                moment = (int)Action_Occurences_Moment_UIntUpDown.Value;
            }

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

            List<ObservationElement> observation = this.scenario_obs.scenarioObservation;
            observation = FormulaParser.infix_to_ONP(observation);
            IFormula formula = FormulaParser.ParseToFormula(observation);
            if(formula==null)
            {
                MessageBox.Show("The expression is not valid!");
                return;
            }
            int moment = 0;
            if (Observations_UIntUpDown.Value != null)
            {
                moment = (int)Observations_UIntUpDown.Value;
            }
            this.scenario.items.Add(new ScenarioItem(moment.ToString(), "", this.scenario_obs.GetContent(), ""));
            Scenario_ListView.Items.Refresh();
            this.scenario_obs.Clear_Control();
        }

        private void ScenarioName_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (ScenarioName_TextBox.Text == "")
            {
                // Create an ImageBrush.
                ImageBrush textImageBrush = new ImageBrush();
                textImageBrush.ImageSource =
                    new BitmapImage(
                        new Uri(@"../../Backgrounds/Scenario.bmp", UriKind.Relative)
                    );
                textImageBrush.AlignmentX = AlignmentX.Left;
                textImageBrush.AlignmentY = AlignmentY.Top;
                textImageBrush.Stretch = Stretch.Uniform;
                // Use the brush to paint the button's background.
                ScenarioName_TextBox.Background = textImageBrush;
            }
            else
            {

                ScenarioName_TextBox.Background = null;
            }
        }

        private void Observations_UIntUpDown_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (Observations_UIntUpDown.Text == "")
            {
                // Create an ImageBrush.
                ImageBrush textImageBrush = new ImageBrush();
                textImageBrush.ImageSource =
                    new BitmapImage(
                        new Uri(@"../../Backgrounds/Moment.bmp", UriKind.Relative)
                    );
                textImageBrush.AlignmentX = AlignmentX.Left;
                textImageBrush.AlignmentY = AlignmentY.Top;
                textImageBrush.Stretch = Stretch.Uniform;
                // Use the brush to paint the button's background.
                Observations_UIntUpDown.Background = textImageBrush;
            }
            else
            {

                Observations_UIntUpDown.Background = null;
            }
        }

        private void Action_Occurences_Moment_UIntUpDown_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (Action_Occurences_Moment_UIntUpDown.Text == "")
            {
                // Create an ImageBrush.
                ImageBrush textImageBrush = new ImageBrush();
                textImageBrush.ImageSource =
                    new BitmapImage(
                        new Uri(@"../../Backgrounds/Moment.bmp", UriKind.Relative)
                    );
                textImageBrush.AlignmentX = AlignmentX.Left;
                textImageBrush.AlignmentY = AlignmentY.Top;
                textImageBrush.Stretch = Stretch.Uniform;
                // Use the brush to paint the button's background.
                Action_Occurences_Moment_UIntUpDown.Background = textImageBrush;
            }
            else
            {
                Action_Occurences_Moment_UIntUpDown.Background = null;
            }
        }

        private void Action_Occurences_Duration_UIntUpDown_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (Action_Occurences_Duration_UIntUpDown.Text == "")
            {
                // Create an ImageBrush.
                ImageBrush textImageBrush = new ImageBrush();
                textImageBrush.ImageSource =
                    new BitmapImage(
                        new Uri(@"../../Backgrounds/Duration.bmp", UriKind.Relative)
                    );
                textImageBrush.AlignmentX = AlignmentX.Left;
                textImageBrush.AlignmentY = AlignmentY.Top;
                textImageBrush.Stretch = Stretch.Uniform;
                // Use the brush to paint the button's background.
                Action_Occurences_Duration_UIntUpDown.Background = textImageBrush;
            }
            else
            {

                Action_Occurences_Duration_UIntUpDown.Background = null;
            }
        }

        private void TimeInfinity_UpDown_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            engine.SetMaxTime((int)TimeInfinity_UpDown.Value);
        }

        private void AddFluentButton_Click(object sender, RoutedEventArgs e)
        {
            string name = fluentName.Text;
            Fluent fluent = new Fluent(name);
            fluents.Add(fluent);

            engine.AddFluent(fluent);
        }

        private void AddActionButton_Click(object sender, RoutedEventArgs e)
        {
            string name = actionNameTextBox.Text;
            Action action = new Action(name);

            actions.Add(action);
            engine.AddAction(action);
        }
    }
}
