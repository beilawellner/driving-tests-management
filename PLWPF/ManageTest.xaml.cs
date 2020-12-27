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
using System.Windows.Shapes;
using BE;
using BL;

namespace PLWPF
{   
    /// <summary>
    /// Interaction logic for ManageTest.xaml
    /// </summary>
    public partial class ManageTest : Window
    {
        //private BL.IBL bl;
        //private BE.Test TestForPL;
        //private List<Test> TestListForPL;
        public ManageTest()
        {
            InitializeComponent();
        }
            
        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {

            System.Windows.Data.CollectionViewSource testViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("testViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // testViewSource.Source = [generic data source]
        }      
        
    }
}
