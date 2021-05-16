using KnowledgeRepresentationInterface.Queries;
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

        public MainWindow()
        {
            Initialize_Query_Types();
            InitializeComponent();
            Query_GroupBox.Content = this.PSQ;
            List<ScenarioItem> test = new List<ScenarioItem>();
            Scenario_ListView.ItemsSource = test;
            TreeViewItem t = new TreeViewItem();
            t.Header = "Fluent II";
            TreeViewItem t2 = new TreeViewItem();
            t2.Header = "Option";
            t.Items.Add(t2);
            Fluents_TreeViewItem.Items.Add(t);
            for(int i=0;i<50;i++) 
            {
                TreeViewItem t3 = new TreeViewItem();
                t3.Header = "Fluent " + i.ToString();
                ScenarioItem item = new ScenarioItem("2", "3", "4");
                t3.Tag = item.ID;
                Fluents_TreeViewItem.Items.Add(t3);
                test.Add(item);
            }
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
                    return;
                }
            }
            foreach (TreeViewItem item in Fluents_TreeViewItem.Items)
            {
                if (item.IsSelected)
                {
                    Fluents_TreeViewItem.Items.Remove(item);
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
                    return;
                }
            }

        }

        private void Delete_Scenario_ListView_Click(object sender, RoutedEventArgs e)
        {
            List<ScenarioItem> tmp_ListView = (List<ScenarioItem>)Scenario_ListView.ItemsSource;
            foreach(ScenarioItem item in Scenario_ListView.SelectedItems)
            {
                tmp_ListView.Remove(item);
            }
            Scenario_ListView.ItemsSource = tmp_ListView;
            Scenario_ListView.Items.Refresh();

        }

        private void Panel_TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //do actions on tab change
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
                    break;
                case 2:
                    Query_GroupBox.Content = this.FQ;
                    break;
                case 3:
                    Query_GroupBox.Content = this.TQ;
                    break;
            }
        }
    }

    public class ScenarioItem
    {
        public Guid ID { get; set; }
        public string ActionOccurence { get; set; }
        public string Observation { get; set; }
        public string Duration { get; set; }

        public ScenarioItem(string ActionOccurence, string Observation, string Duration)
        {
            this.ID = new Guid();
            this.ActionOccurence = ActionOccurence;
            this.Observation = Observation;
            this.Duration = Duration;
        }

    }
}
