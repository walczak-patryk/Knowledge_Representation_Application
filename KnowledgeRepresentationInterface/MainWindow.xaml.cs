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
using KR_Lib.Scenarios;
using KR_Lib.Queries;

namespace KnowledgeRepresentationInterface
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        IEngine engine;

        PossibleScenarioQueryView PSQ;
        ActionQueryView AQ;
        FormulaQueryView FQ;
        TargetQueryView TQ;
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
            this.FontSize = 14;
            this.scenario = new ScenarioGUI();
            this.scenarios = new List<ScenarioGUI>();
            this.actions = new List<Action>();
            this.fluents = new List<Fluent>();
            this.scenario_obs = new ObservationCreator(this.fluents);
            Initialize_Query_Types();
            Initialize_Statement_Types();
            Scenario_ListView.ItemsSource = this.scenario.items;
            Query_GroupBox.Content = this.PSQ;

            Action_Occurences_ComboBox.ItemsSource = this.actions;
            Query_Scenario_ComboBox.ItemsSource = this.scenarios;
            Observation_Scenario_GroupBox.Content = this.scenario_obs;
        }

        private void Initialize_Query_Types()
        {
            this.PSQ = new PossibleScenarioQueryView();
            this.AQ = new ActionQueryView();
            this.FQ = new FormulaQueryView(this.fluents);
            this.TQ = new TargetQueryView(this.fluents);
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

            ScenarioGUI selected_scenario = (ScenarioGUI)Query_Scenario_ComboBox.SelectedItem;
            IQuery query;
            bool result = false;
            switch (Query_Type_ComboBox.SelectedIndex)
            {
                case 0:
                    QueryType qt_PSQ = QueryType.Always;
                    if (this.PSQ.Type_ComboBox.SelectedIndex==1)
                    {
                        qt_PSQ = QueryType.Ever;
                    }
                    query = new KR_Lib.Queries.PossibleScenarioQuery(qt_PSQ, selected_scenario.Id);
                    result = this.engine.ExecuteQuery(query);
                    break;
                case 1:
                    if(this.AQ.Moment_UIntUpDown.Value==null)
                    {
                        MessageBox.Show("The moment value cannot be empty!");
                        return;
                    }
                    if (this.AQ.Actions_ComboBox.SelectedItem == null)
                    {
                        MessageBox.Show("You have to select an action!");
                        return;
                    }
                    int time_AQ = (int)this.AQ.Moment_UIntUpDown.Value;
                    Action action_AQ = (Action)this.AQ.Actions_ComboBox.SelectedItem;
                    query = new KR_Lib.Queries.ActionQuery(time_AQ, action_AQ, selected_scenario.Id);
                    result = this.engine.ExecuteQuery(query);
                    break;
                case 2:
                    if (this.FQ.Moment_UIntUpDown.Value == null)
                    {
                        MessageBox.Show("The moment value cannot be empty!");
                        return;
                    }
                    IFormula formula_FQ = this.FQ.Get_Formula();
                    if (formula_FQ == null)
                    {
                        MessageBox.Show("The formula is not valid!");
                        return;
                    }
                    int time_FQ = (int)this.FQ.Moment_UIntUpDown.Value;
                    query = new KR_Lib.Queries.FormulaQuery(time_FQ, formula_FQ, selected_scenario.Id);
                    result = this.engine.ExecuteQuery(query);
                    break;
                case 3:
                    IFormula formula_TQ = this.TQ.Get_Formula();
                    if (formula_TQ == null)
                    {
                        MessageBox.Show("The formula is not valid!");
                        return;
                    }
                    QueryType qt_TQ = QueryType.Always;
                    if (this.PSQ.Type_ComboBox.SelectedIndex == 1)
                    {
                        qt_TQ = QueryType.Ever;
                    }
                    query = new KR_Lib.Queries.TargetQuery(formula_TQ, qt_TQ, selected_scenario.Id);
                    result = this.engine.ExecuteQuery(query);
                    break;
            }

            if(result)
            {
                Result_label.Content = "TRUE";
            }
            else
            {
                Result_label.Content = "FALSE";
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
            TreeViewItem new_scenario = new TreeViewItem();
            new_scenario.Header = ScenarioName_TextBox.Text;
            new_scenario.Tag = scenario.Id.ToString();
            foreach(var item in this.scenario.items)
            {
                TreeViewItem new_subitem = new TreeViewItem();
                if(item.ActionOccurence == null)
                {
                    new_subitem.Header = "Observation: " + item.Observation + " M: " + item.Moment;
                }
                else
                {
                    new_subitem.Header = "Action occurence: " + item.ActionOccurence + " D: " + item.Duration + " M: " + item.Moment;
                }
                
                new_subitem.Tag = item.Id;
                new_scenario.Items.Add(new_subitem);
            }

            Scenario engine_scenario = new Scenario(this.scenario.name);
            this.scenario.Id = engine_scenario.Id;
            this.engine.AddScenario(engine_scenario);
            foreach(var elem in this.scenario.items)
            {
                if(elem.ActionOccurence != null)
                {
                    engine_scenario.ActionOccurrences.Add(elem.ActionOccurence_engine);
                }
                else
                {
                    this.engine.AddObservation(engine_scenario.Id,elem.formula, elem.Moment_int);
                }
            }

            this.scenarios.Add(this.scenario);
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
            if(Action_Occurences_Moment_UIntUpDown.Value == null)
            {
                MessageBox.Show("The moment value cannot be empty!");
                return;
            }
            if (Action_Occurences_Duration_UIntUpDown.Value == null)
            {
                MessageBox.Show("The duration value cannot be empty!");
                return;
            }
            int moment = (int)Action_Occurences_Moment_UIntUpDown.Value;
            int duration = (int)Action_Occurences_Duration_UIntUpDown.Value;
            this.scenario.items.Add(new ScenarioItem(action.Name,action,moment,duration, "", null));
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
            if (Observations_UIntUpDown.Value == null)
            {
                MessageBox.Show("The moment value cannot be empty!");
                return;
            }
            int moment = (int)Observations_UIntUpDown.Value;
            this.scenario.items.Add(new ScenarioItem(null,null,moment,0,this.scenario_obs.GetContent(), formula));
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
            TreeViewItem tv_elem = new TreeViewItem();
            tv_elem.Header = name;
            tv_elem.Tag = fluent.Id;
            Fluents_TreeViewItem.Items.Add(tv_elem);
            engine.AddFluent(fluent);
            scenario_obs.RefreshControl();
            this.FQ.Refresh_Fluents();
            this.TQ.Refresh_Fluents();
        }

        private void AddActionButton_Click(object sender, RoutedEventArgs e)
        {
            string name = actionNameTextBox.Text;
            Action action = new Action(name);
            TreeViewItem tv_elem = new TreeViewItem();
            tv_elem.Header = name;
            tv_elem.Tag = action.Id;
            Actions_TreeViewItem.Items.Add(tv_elem);
            actions.Add(action);
            engine.AddAction(action);
            Action_Occurences_ComboBox.Items.Refresh();
            this.AQ.Actions_ComboBox.Items.Refresh();
        }
    }
}
