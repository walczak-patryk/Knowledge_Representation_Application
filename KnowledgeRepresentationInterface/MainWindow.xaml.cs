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
using KR_Lib.Statements;

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
        List<IStatement> statements;
        List<ScenarioGUI> scenarios;
        ObservationCreator scenario_obs;
        

        //statements
        CauseStatementView CS;
        ImpossibleAtStatementView IAS;
        ImpossibleIfStatementView IIS;
        InvokeStatementView IS;
        ReleaseStatementView RS;
        TriggerStatementView TS;

        public MainWindow()
        {
            this.engine = new Engine();
            InitializeComponent();
            this.FontSize = 14;
            this.scenario = new ScenarioGUI();
            this.scenarios = new List<ScenarioGUI>();
            this.actions = new List<Action>();
            this.fluents = new List<Fluent>();
            this.statements = new List<IStatement>();
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
            this.CS = new CauseStatementView(this.fluents);
            this.IAS = new ImpossibleAtStatementView();
            this.IIS = new ImpossibleIfStatementView(this.fluents);
            this.IS = new InvokeStatementView(this.fluents);
            this.RS = new ReleaseStatementView(this.fluents);
            this.TS = new TriggerStatementView(this.fluents);
        }

        private void Delete_TreeView_Click(object sender, RoutedEventArgs e)
        {
            foreach(TreeViewItem item in Actions_TreeViewItem.Items)
            {
                if(item.IsSelected)
                {
                    Actions_TreeViewItem.Items.Remove(item);
                    Action to_remove = null;
                    foreach(var elem in this.actions)
                    {
                        if(elem.Id.ToString()==item.Tag.ToString())
                        {
                            to_remove = elem;
                            break;
                        }
                    }
                    
                    Guid action_to_remove_guid = to_remove.Id;
                    this.engine.RemoveAction(to_remove.Id);
                    this.actions.RemoveAll(x => x.Id.ToString() == item.Tag.ToString());
                    List<ScenarioGUI> scenarios_to_remove = new List<ScenarioGUI>();
                    foreach (ScenarioGUI scenario_tmp in scenarios)
                    {
                        foreach (ScenarioItem item_tmp in scenario_tmp.items)
                        {
                            if (item_tmp.ActionOccurence != null)
                            {
                                if (item_tmp.ActionOccurence_engine.Id.ToString() == item.Tag.ToString())
                                {
                                    scenarios_to_remove.Add(scenario_tmp);
                                    break;
                                }
                            }
                        }
                    }
                    foreach(var scenario_tmp in scenarios_to_remove)
                    {
                        TreeViewItem scenario_to_remove = null;
                        foreach (TreeViewItem scenario_tv in Scenarios_TreeViewItem.Items)
                        {
                            if (scenario_tv.Tag.ToString() == scenario_tmp.Id.ToString())
                            {
                                scenario_to_remove = scenario_tv;
                                break;
                            }
                        }
                        Scenarios_TreeViewItem.Items.Remove(scenario_to_remove);
                        this.scenarios.RemoveAll(x => x.Id.ToString() == scenario_tmp.Id.ToString());
                        Query_Scenario_ComboBox.ItemsSource = this.scenarios;
                        Query_Scenario_ComboBox.Items.Refresh();
                        this.engine.RemoveScenario(scenario_tmp.Id);
                    }

                    List<TreeViewItem> statementTreeViewItemsToRemove = new List<TreeViewItem>();
                    foreach(TreeViewItem statementItem in Statements_TreeViewItem.Items)
                    {
                        foreach(TreeViewItem statementParameter in statementItem.Items)
                        {
                            if(action_to_remove_guid.ToString() == statementParameter.Tag.ToString())
                            {
                                statementTreeViewItemsToRemove.Add(statementItem);

                                this.engine.RemoveStatement(Guid.Parse(statementItem.Tag.ToString()));
                                this.statements.RemoveAll(x => x.GetId().ToString() == statementItem.Tag.ToString());
                                break;
                            } 
                        }
                    }
                    foreach (TreeViewItem statementTreeViewItems in statementTreeViewItemsToRemove) {
                        Statements_TreeViewItem.Items.Remove(statementTreeViewItems);
                    }
                    if (statementTreeViewItemsToRemove.Count > 0)
                    {
                        Statements_TreeViewItem.Items.Refresh();
                        Statements_TreeViewItem.UpdateLayout();
                    }

                    Action_Occurences_ComboBox.Items.Refresh();
                    this.AQ.Set_Actions(this.actions);
                    this.CS.Set_Actions(this.actions);
                    return;
                }
            }
            foreach (TreeViewItem item in Fluents_TreeViewItem.Items)
            {
                if (item.IsSelected)
                {
                    Fluents_TreeViewItem.Items.Remove(item);
                    Fluent to_remove = null;
                    
                    foreach (var elem in this.fluents)
                    {
                        if(elem.Id.ToString()==item.Tag.ToString())
                        {
                            to_remove = elem;
                        }
                    }
                    Guid fluent_to_remove_guid = to_remove.Id;
                    this.engine.RemoveFluent(to_remove.Id);
                    this.fluents.RemoveAll(x => x.Id.ToString() == item.Tag.ToString());

                    List<ScenarioGUI> scenarios_to_remove = new List<ScenarioGUI>();
                    foreach (ScenarioGUI scenario_tmp in scenarios)
                    {
                        foreach (ScenarioItem item_tmp in scenario_tmp.items)
                        {
                            bool found = false;
                            if(found)
                            {
                                break;
                            }
                            if (item_tmp.observationElements != null)
                            {
                                foreach (var obsElem in item_tmp.observationElements)
                                {
                                    if (obsElem.isFluent)
                                    {
                                        if (obsElem.fluent.Id.ToString() == item.Tag.ToString())
                                        {
                                            scenarios_to_remove.Add(scenario_tmp);
                                            found = true;
                                            break;
                                        }
                                    }

                                }
                            }

                        }
                    }
                    foreach (var scenario_tmp in scenarios_to_remove)
                    {
                        TreeViewItem scenario_to_remove = null;
                        foreach (TreeViewItem scenario_tv in Scenarios_TreeViewItem.Items)
                        {
                            if (scenario_tv.Tag.ToString() == scenario_tmp.Id.ToString())
                            {
                                scenario_to_remove = scenario_tv;
                                break;
                            }
                        }
                        Scenarios_TreeViewItem.Items.Remove(scenario_to_remove);
                        this.scenarios.RemoveAll(x => x.Id.ToString() == scenario_tmp.Id.ToString());
                        Query_Scenario_ComboBox.ItemsSource = this.scenarios;
                        Query_Scenario_ComboBox.Items.Refresh();
                        this.engine.RemoveScenario(scenario_tmp.Id);
                    }

                    List<TreeViewItem> statementTreeViewItemsToRemove = new List<TreeViewItem>();
                    foreach (TreeViewItem statementItem in Statements_TreeViewItem.Items)
                    {
                        foreach (TreeViewItem statementParameter in statementItem.Items)
                        {
                            if (fluent_to_remove_guid.ToString() == statementParameter.Tag.ToString())
                            {
                                statementTreeViewItemsToRemove.Add(statementItem);

                                this.engine.RemoveStatement(Guid.Parse(statementItem.Tag.ToString()));
                                this.statements.RemoveAll(x => x.GetId().ToString() == statementItem.Tag.ToString());
                                break;
                            } 
                            if(statementParameter.Header.ToString().Contains(item.Header.ToString()))
                            {
                                statementTreeViewItemsToRemove.Add(statementItem);

                                this.engine.RemoveStatement(Guid.Parse(statementItem.Tag.ToString()));
                                this.statements.RemoveAll(x => x.GetId().ToString() == statementItem.Tag.ToString());
                                break;
                            }

                        }
                    }
                    foreach (TreeViewItem statementTreeViewItems in statementTreeViewItemsToRemove)
                    {
                        Statements_TreeViewItem.Items.Remove(statementTreeViewItems);
                    }
                    if (statementTreeViewItemsToRemove.Count > 0)
                    {
                        Statements_TreeViewItem.Items.Refresh();
                        Statements_TreeViewItem.UpdateLayout();
                    }


                    this.scenario_obs.RefreshControl();
                    this.TQ.Refresh_Fluents();
                    this.FQ.Refresh_Fluents();

                    this.CS.Refresh_Fluents();
                    this.IIS.Refresh_Fluents();
                    this.IS.Refresh_Fluents();
                    this.RS.Refresh_Fluents();
                    this.TS.Refresh_Fluents();
                    return;
                }
            }
            foreach (TreeViewItem item in Statements_TreeViewItem.Items)
            {
                if (item.IsSelected)
                {
                    Statements_TreeViewItem.Items.Remove(item);
                    Statements_TreeViewItem.Items.Refresh();
                    Statements_TreeViewItem.UpdateLayout();

                    this.engine.RemoveStatement(Guid.Parse(item.Tag.ToString()));
                    this.statements.RemoveAll(x => x.GetId().ToString() == item.Tag.ToString());
                    return;
                }
            }
            foreach (TreeViewItem item in Scenarios_TreeViewItem.Items)
            {
                if (item.IsSelected)
                {
                    Scenarios_TreeViewItem.Items.Remove(item);
                    ScenarioGUI scenario_to_remove = null;
                    foreach (ScenarioGUI scenario_tmp in scenarios)
                    {
                        if (scenario_tmp.Id.ToString() == item.Tag.ToString())
                        {
                            scenario_to_remove = scenario_tmp;
                            break;
                        }
                    }
                    this.engine.RemoveScenario(scenario_to_remove.Id);
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
            Statement_Type_ComboBox_SelectionChanged(sender, e);
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
                    this.CS.Set_Actions(this.actions);
                    break;
                case 1:
                    Statement_GroupBox.Content = this.IAS;
                    this.IAS.Set_Actions(this.actions);
                    break;
                case 2:
                    Statement_GroupBox.Content = this.IIS;
                    this.IIS.Set_Actions(this.actions);
                    break;
                case 3:
                    Statement_GroupBox.Content = this.IS;
                    this.IS.Set_Actions(this.actions);
                    break;
                case 4:
                    Statement_GroupBox.Content = this.RS;
                    this.RS.Set_Actions_And_Fluents(this.actions, this.fluents);
                    break;
                case 5:
                    Statement_GroupBox.Content = this.TS;
                    this.TS.Set_Actions(this.actions);
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
            //new_scenario.Tag = scenario.Id.ToString();
            foreach(var item in this.scenario.items)
            {
                TreeViewItem new_subitem = new TreeViewItem();
                if(item.ActionOccurence == null)
                {
                    new_subitem.Header = "Observation: " + item.Observation + " M: " + item.Moment;
                }
                else
                {
                    new_subitem.Header = "Action occurrence: " + item.ActionOccurence + " D: " + item.Duration + " M: " + item.Moment;
                }
                
                new_subitem.Tag = item.Id;
                new_scenario.Items.Add(new_subitem);
            }

            Scenario engine_scenario = new Scenario(this.scenario.name);
            this.scenario.Id = engine_scenario.Id;
            new_scenario.Tag = scenario.Id.ToString();
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
            this.scenario.items.Add(new ScenarioItem(action.Name,action,moment,duration, "", null, null));
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
            this.scenario.items.Add(new ScenarioItem(null,null,moment,0,this.scenario_obs.GetContent(), formula, observation));
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
            if (this.IsNameInFluents(name))
            {
                MessageBox.Show($"Fluent name '{name}' already used. Type another name.");
                return;
            }

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

            this.CS.Refresh_Fluents();
            this.IIS.Refresh_Fluents();
            this.IS.Refresh_Fluents();
            this.RS.Refresh_Fluents();
            this.TS.Refresh_Fluents();
        }

        private void AddActionButton_Click(object sender, RoutedEventArgs e)
        {
            string name = actionNameTextBox.Text;

            if (this.IsNameInActions(name))
            {
                MessageBox.Show($"Action name '{name}' already used. Type another name.");
                return;
            }
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

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Niniejszy program jest systemem realizującym scenariusze działań z wykorzystaniem rozszerzenia języka AL.\n\n" +
                "Autorzy:\nJoanna Frankiewicz *\nPatryk Walczak *\nAlicja Danilczuk\nKacper Gąsior\nPamela Krzypkowska\nKornel Mrozowski\n" + 
                "Martin Mrugała\nKacper Skoczek\nFilip Szymczak\nDamian Wysokiński");
        }
        private bool IsNameInActions(string name)
        {
            foreach (Action action in this.actions)
            {
                var actionName = action.Name;
                if(actionName == name)
                {
                    return true;
                }
            }
            return false;
        }

        private bool IsNameInFluents(string name)
        {
            foreach (Fluent fluent in this.fluents)
            {
                var fluentName = fluent.Name;
                if (fluentName == name)
                {
                    return true;
                }
            }
            return false;
        }

        private void AddStatementButton_Click(object sender, RoutedEventArgs e)
        {           
            if (Statement_Type_ComboBox.SelectedIndex == -1)
            {
                MessageBox.Show($"No statement type chosen.");
                return;
            }
            switch (Statement_Type_ComboBox.SelectedIndex)
            {
                case 0: //CS

                    if (this.CS.CauseStatement_ComboBox.SelectedIndex == -1)
                    {
                        MessageBox.Show($"No action chosen for 'cause' statement type.");
                        return;
                    }

                    if(this.CS.Get_First_Formula() == null)
                    {
                        MessageBox.Show($"Incorrect first formula");
                        return;
                    }

                    if (this.CS.Get_Second_Formula() == null && !this.CS.HorizonstalToggleSwitchForExpression.IsChecked)
                    {
                        MessageBox.Show($"Incorrect second formula");
                        return;
                    }

                    Action action = (Action)this.CS.CauseStatement_ComboBox.SelectedItem;
                    int time = Convert.ToInt32(TimeInfinity_UpDown.Value);

                    ActionTime actionTime = new ActionTime(action, time);

                    IStatement statement;
                    if (this.CS.HorizonstalToggleSwitchForExpression.IsChecked) {
                    statement =  new CauseStatement(actionTime, this.CS.Get_First_Formula());
                    }
                    else
                    {
                        statement = new CauseStatement(actionTime, this.CS.Get_First_Formula(), this.CS.Get_Second_Formula());
                    }

                    TreeViewItem tv_elem = new TreeViewItem();
                    tv_elem.Header = "Cause Statement " + CauseStatementView.numberOfCouseStatements;
                    tv_elem.Tag = statement.GetId();

                    TreeViewItem tv_elem_action = new TreeViewItem();
                    tv_elem_action.Header = "Action: " + this.CS.CauseStatement_ComboBox.Text;
                    tv_elem_action.Tag = action.Id.ToString();
                    tv_elem_action.Visibility = Visibility.Collapsed;

                    TreeViewItem tv_elem_formula1 = new TreeViewItem();
                    tv_elem_formula1.Header = "Formula1: " + this.CS.scenario_obs.Observations_TextBox.Text;
                    tv_elem_formula1.Tag = this.CS.Get_First_Formula().ToString();
                    tv_elem_formula1.Visibility = Visibility.Collapsed;

                    string treeViewStatementFormulaSummary = 
                        this.CS.CauseStatement_ComboBox.Text + " CAUSES ( " +
                        this.CS.scenario_obs.Observations_TextBox.Text + ")";

                    if (this.CS.HorizonstalToggleSwitchForExpression.IsChecked == false)
                    {
                        TreeViewItem tv_elem_formula2 = new TreeViewItem();
                        tv_elem_formula2.Header = "Formula2: " + this.CS.scenario_obs2.Observations_TextBox.Text;
                        tv_elem_formula2.Visibility = Visibility.Collapsed;
                        tv_elem_formula2.Tag = this.CS.Get_Second_Formula().ToString();
                        tv_elem.Items.Add(tv_elem_formula2);

                        treeViewStatementFormulaSummary += " IF ( " + this.CS.scenario_obs2.Observations_TextBox.Text + ")";
                    }
                    TreeViewItem tv_statement_summary = new TreeViewItem();
                    tv_statement_summary.Header = treeViewStatementFormulaSummary;
                    tv_statement_summary.Tag = "";

                    CauseStatementView.numberOfCouseStatements += 1;

                    tv_elem.Items.Add(tv_elem_action);
                    tv_elem.Items.Add(tv_elem_formula1);
                    tv_elem.Items.Add(tv_statement_summary);

                    tv_elem.Tag = statement.GetId();
                    Statements_TreeViewItem.Items.Add(tv_elem);

                    statements.Add(statement);
                    engine.AddStatement(statement);
                    

                    break;
                case 1: //IAS

                    if (this.IAS.ImpossibleAtStatement_ComboBox.SelectedIndex == -1)
                    {
                        MessageBox.Show("No Action chosen for 'impossible at' statement type.");
                        return;
                    }
                    if(this.IAS.Action_Duration_UIntUpDown.Value == null || this.IAS.Action_Duration_UIntUpDown.Value == 0)
                    {
                        MessageBox.Show("Specify time for 'impossible at' statement type.");
                        return;
                    }

                    Action actionIAS = (Action)this.IAS.ImpossibleAtStatement_ComboBox.SelectedItem;
                    int timeIAS = Convert.ToInt32(this.IAS.Action_Duration_UIntUpDown.Value);

                    IStatement statementIAS = new ImpossibleAtStatement(actionIAS, timeIAS);
                    TreeViewItem tv_elemIAS = new TreeViewItem();
                    tv_elemIAS.Header = "Imposssible At Statement " + ImpossibleAtStatementView.numberOfImpossibleAtStatements;
                    tv_elemIAS.Tag = statementIAS.GetId();

                    TreeViewItem tv_elem_actionIAS = new TreeViewItem();
                    tv_elem_actionIAS.Header = "Action: " + this.IAS.ImpossibleAtStatement_ComboBox.Text;
                    tv_elem_actionIAS.Tag = actionIAS.Id.ToString();

                    TreeViewItem tv_elem_timeIAS = new TreeViewItem();
                    tv_elem_timeIAS.Header = "Time: " + timeIAS;
                    tv_elem_timeIAS.Tag = timeIAS.ToString();

                    tv_elemIAS.Items.Add(tv_elem_actionIAS);
                    tv_elemIAS.Items.Add(tv_elem_timeIAS);

                    ImpossibleAtStatementView.numberOfImpossibleAtStatements += 1;

                    Statements_TreeViewItem.Items.Add(tv_elemIAS);

                    statements.Add(statementIAS);
                    engine.AddStatement(statementIAS);

                    break;
                case 2: //IIS
                    if (this.IIS.ImpossibleIfStatement_ComboBox.SelectedIndex == -1)
                    {
                        MessageBox.Show("No action chosen for 'impossible if' statement type.");
                        return;
                    }

                    if (this.IIS.Get_Formula()== null)
                    {
                        MessageBox.Show($"Incorrect second formula");
                        return;
                    }

                    Action actionIIS = (Action)this.IIS.ImpossibleIfStatement_ComboBox.SelectedItem;

                    IStatement statementIIS = new ImpossibleIfStatement(actionIIS, this.IIS.Get_Formula());
                    TreeViewItem tv_elemIIS = new TreeViewItem();
                    tv_elemIIS.Header = "Impossible If Statement " + ImpossibleIfStatementView.numberOfImpossibleifStatements; 
                    tv_elemIIS.Tag = statementIIS.GetId();

                    TreeViewItem tv_elemIIS_action = new TreeViewItem();
                    tv_elemIIS_action.Header = "Action: " + this.IIS.ImpossibleIfStatement_ComboBox.Text;
                    tv_elemIIS_action.Tag = actionIIS.Id.ToString();


                    TreeViewItem tv_elem_formulaIIS = new TreeViewItem();
                    tv_elem_formulaIIS.Header = "Formula: " + this.IIS.scenario_obs.Observations_TextBox.Text;
                    tv_elem_formulaIIS.Tag = this.IIS.Get_Formula().ToString();

                    ImpossibleIfStatementView.numberOfImpossibleifStatements += 1;

                    tv_elemIIS.Items.Add(tv_elemIIS_action);
                    tv_elemIIS.Items.Add(tv_elem_formulaIIS);

                    Statements_TreeViewItem.Items.Add(tv_elemIIS);



                    statements.Add(statementIIS);
                    engine.AddStatement(statementIIS);

                    break;
                case 3: //IS
                    if (this.IS.InvokeStatementFirst_ComboBox.SelectedIndex == -1)
                    {
                        MessageBox.Show("No action chosen for 'invoke' statement type.");
                        return;
                    }
                    if (this.IS.InvokeStatementSecend_ComboBox.SelectedIndex == -1)
                    {
                        MessageBox.Show("No action chosen for 'invoke' statement type.");
                        return;
                    }
                    if (this.IS.Get_Formula() == null)
                    {
                        MessageBox.Show($"Incorrect formula");
                        return;
                    }

                    Action actionIS1 = (Action)this.IS.InvokeStatementFirst_ComboBox.SelectedItem;
                    Action actionIS2 = (Action)this.IS.InvokeStatementSecend_ComboBox.SelectedItem;

                    int timeIS = Convert.ToInt32(TimeInfinity_UpDown.Value);

                    ActionTime actionTimeIS1 = new ActionTime(actionIS1, timeIS);
                    ActionTime actionTimeIS2 = new ActionTime(actionIS2, timeIS);

                    int waitTimeValue = Convert.ToInt32(this.IS.Action_Duration_UIntUpDown.Value);

                    IStatement statementIS = new InvokeStatement(actionTimeIS1, actionTimeIS2, this.IS.Get_Formula(), waitTimeValue);
                    TreeViewItem tv_elemIS = new TreeViewItem();

                    tv_elemIS.Header = "Invoke Statement " + InvokeStatementView.numberOfIvokeStatements;
                    tv_elemIS.Tag = statementIS.GetId();

                    TreeViewItem tv_elemIS_action1 = new TreeViewItem();
                    tv_elemIS_action1.Header = "Action1: " + this.IS.InvokeStatementFirst_ComboBox.Text;
                    tv_elemIS_action1.Tag = actionIS1.Id.ToString();

                    TreeViewItem tv_elemIS_action2 = new TreeViewItem();
                    tv_elemIS_action2.Header = "Action2: " + this.IS.InvokeStatementSecend_ComboBox.Text;
                    tv_elemIS_action2.Tag = actionIS2.Id.ToString();

                    TreeViewItem tv_elem_formulaIS = new TreeViewItem();
                    tv_elem_formulaIS.Header = "Formula: " + this.IS.scenario_obs.Observations_TextBox.Text;
                    tv_elem_formulaIS.Tag = this.IS.Get_Formula().ToString();

                    tv_elemIS.Items.Add(tv_elemIS_action1);
                    tv_elemIS.Items.Add(tv_elemIS_action2);
                    tv_elemIS.Items.Add(tv_elem_formulaIS);

                    InvokeStatementView.numberOfIvokeStatements += 1;
                    Statements_TreeViewItem.Items.Add(tv_elemIS);

                    statements.Add(statementIS);
                    engine.AddStatement(statementIS);

                    break;
                case 4: //RS
                    if(this.RS.ReleaseStatementActions_ComboBox.SelectedIndex == -1)
                    {
                        MessageBox.Show("No action chosen for 'release' statement type.");
                        return;
                    }
                    if (this.RS.ReleaseStatementFluents_ComboBox.SelectedIndex == -1)
                    {
                        MessageBox.Show("No fluent chosen for 'release' statement type.");
                        return;
                    }

                    if (this.RS.Get_Formula() == null)
                    {
                        MessageBox.Show($"Incorrect formula");
                        return;
                    }

                    Action actionRS = (Action)this.RS.ReleaseStatementActions_ComboBox.SelectedItem;
                    Fluent fluentRS = (Fluent)this.RS.ReleaseStatementFluents_ComboBox.SelectedItem;

                    IStatement statementRS = new ReleaseStatement(actionRS, fluentRS, this.RS.Get_Formula());


                    TreeViewItem tv_elemRS = new TreeViewItem();

                    tv_elemRS.Header = "Release Statement " + ReleaseStatementView.numberOfReleaseStatements; 
                    tv_elemRS.Tag = statementRS.GetId();

                    TreeViewItem tv_elemRS_action = new TreeViewItem();
                    tv_elemRS_action.Header = "Action: " + this.RS.ReleaseStatementActions_ComboBox.Text;
                    tv_elemRS_action.Tag = actionRS.Id.ToString();

                    TreeViewItem tv_elemRS_fluent = new TreeViewItem();
                    tv_elemRS_fluent.Header = "Fluent: " + this.RS.ReleaseStatementFluents_ComboBox.Text;
                    tv_elemRS_fluent.Tag = fluentRS.Id.ToString();

                    TreeViewItem tv_elem_formulaRS = new TreeViewItem();
                    tv_elem_formulaRS.Header = "Formula: " + this.RS.scenario_obs.Observations_TextBox.Text;
                    tv_elem_formulaRS.Tag = this.RS.Get_Formula().ToString();

                    tv_elemRS.Items.Add(tv_elemRS_action);
                    tv_elemRS.Items.Add(tv_elemRS_fluent);
                    tv_elemRS.Items.Add(tv_elem_formulaRS);
                    ReleaseStatementView.numberOfReleaseStatements += 1;

                    Statements_TreeViewItem.Items.Add(tv_elemRS);

                    statements.Add(statementRS);
                    engine.AddStatement(statementRS);

                    break;
                case 5: //TS
                    
                    if(this.TS.TriggerStatementAction_ComboBox.SelectedIndex == -1)
                    {
                        MessageBox.Show("No action choosen for 'trigger' statement type.");
                        return;
                    }

                    if (this.TS.Get_Formula() == null)
                    {
                        MessageBox.Show($"Incorrect formula");
                        return;
                    }
                    int time2 = -1;
                    if (this.TS.TriggerStatementAction_Numeric.Value == null)
                    {
                        MessageBox.Show($"Incorrect action duration");
                        return;
                    }
                    time2 = (int)this.TS.TriggerStatementAction_Numeric.Value;
                    Action actionTS = (Action)this.TS.TriggerStatementAction_ComboBox.SelectedItem;
                    var actionT = new ActionTime(actionTS, time2);
                    IStatement statementTS = new TriggerStatement(actionT, this.TS.Get_Formula());

                    TreeViewItem tv_elemTS = new TreeViewItem();

                    tv_elemTS.Header = "Trigger Statement " + TriggerStatementView.numberOfTriggerStatements;
                    tv_elemTS.Tag = statementTS.GetId();

                    TreeViewItem tv_elemTS_action = new TreeViewItem();
                    tv_elemTS_action.Header = "Action: " + this.TS.TriggerStatementAction_ComboBox.Text;
                    tv_elemTS_action.Tag = actionTS.Id.ToString();

                    TreeViewItem tv_elem_formulaTS = new TreeViewItem();
                    tv_elem_formulaTS.Header = "Formula: " + this.TS.scenario_obs.Observations_TextBox.Text;
                    tv_elem_formulaTS.Tag = this.TS.Get_Formula().ToString();

                    tv_elemTS.Items.Add(tv_elemTS_action);
                    tv_elemTS.Items.Add(tv_elem_formulaTS);
                    TriggerStatementView.numberOfTriggerStatements += 1;

                    Statements_TreeViewItem.Items.Add(tv_elemTS);

                    statements.Add(statementTS);
                    engine.AddStatement(statementTS);

                    break;
            }
        }
    }
}
