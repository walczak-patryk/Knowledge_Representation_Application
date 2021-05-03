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


        public MainWindow()
        {
            InitializeComponent();
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
                Fluents_TreeViewItem.Items.Add(t3);
                test.Add(new ScenarioItem(i.ToString(), "2", "3","4"));
            }
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
    }

    public class ScenarioItem
    {
        public string ID { get; set; }
        public string ActionOccurence { get; set; }
        public string Observation { get; set; }
        public string Duration { get; set; }

        public ScenarioItem(string ID, string ActionOccurence, string Observation, string Duration)
        {
            this.ID = ID;
            this.ActionOccurence = ActionOccurence;
            this.Observation = Observation;
            this.Duration = Duration;
        }
    }
}
