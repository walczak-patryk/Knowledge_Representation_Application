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
using KR_Lib.DataStructures;

namespace KnowledgeRepresentationInterface.Statements
{
    /// <summary>
    /// Interaction logic for ReleaseStatement.xaml
    /// </summary>
    public partial class ReleaseStatement : UserControl
    {
        public ReleaseStatement()
        {
            InitializeComponent();
        }

        public void Set_Actions_And_Fluents(List<Action> actions, List<Fluent> fluents)
        {
            ReleaseStatementActions_ComboBox.ItemsSource = actions;
            ReleaseStatementActions_ComboBox.Items.Refresh();

            ReleaseStatementFluents_ComboBox.ItemsSource = fluents;
            ReleaseStatementFluents_ComboBox.Items.Refresh();
        }
    }
}
