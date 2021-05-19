using KR_Lib.DataStructures;
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

namespace KnowledgeRepresentationInterface.General
{
    /// <summary>
    /// Interaction logic for ObservationCreator.xaml
    /// </summary>
    public partial class ObservationCreator : UserControl
    {
        public List<ObservationElement> scenarioObservation { get; set; }
        List<Fluent> fluents;

        public ObservationCreator(List<Fluent> fluents)
        {
            scenarioObservation = new List<ObservationElement>();
            this.fluents = fluents;
            InitializeComponent();
            Fluent_Observation_ScenarioTab.ItemsSource = this.fluents;
        }

        private void And_Scenario_Click(object sender, RoutedEventArgs e)
        {
            Observations_TextBox.Text += "AND ";
            scenarioObservation.Add(new ObservationElement(false, null, 4, "AND"));
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
            if (scenarioObservation.Count == 0)
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

        public void RefreshControl()
        {
            Fluent_Observation_ScenarioTab.Items.Refresh();
        }

        public bool IsEmpty()
        {
            return this.scenarioObservation.Count == 0;
        }

        public string GetContent()
        {
            return Observations_TextBox.Text;
        }

    }
}
