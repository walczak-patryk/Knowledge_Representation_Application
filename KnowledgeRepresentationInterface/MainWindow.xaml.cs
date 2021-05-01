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
            }
        }

        public double Test2()
        {
            return ActualWidth / 2;
        }
    }
}
