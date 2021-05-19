using KnowledgeRepresentationInterface.General;
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

namespace KnowledgeRepresentationInterface.Queries
{
    /// <summary>
    /// Interaction logic for FormulaQuery.xaml
    /// </summary>
    public partial class FormulaQuery : UserControl
    {
        public ObservationCreator scenario_obs { get; set; }

        public FormulaQuery(List<Fluent> fluents)
        {
            this.scenario_obs = new ObservationCreator(fluents);
            InitializeComponent();
            Observation_GroupBox.Content = this.scenario_obs;
        }

        public void Refresh_Fluents()
        {
            this.scenario_obs.RefreshControl();
        }

        private void Duration_UIntUpDown_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (Duration_UIntUpDown.Text == "")
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
                Duration_UIntUpDown.Background = textImageBrush;
            }
            else
            {

                Duration_UIntUpDown.Background = null;
            }
        }
    }
}
