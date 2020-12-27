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
    /// Interaction logic for TestersByCarType.xaml
    /// </summary>
    public partial class TestersByCarType : Window
    {
        private BL.IBL bl;
        public TestersByCarType()
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            bl = IBL_imp.Instance;
            IEnumerable<IGrouping<CarType, Tester>> TestersByCarType = bl.TestersByCarType(true);
            foreach (var cargroup in TestersByCarType)
            {
                var car = cargroup.Key;
                CarComboBox.Items.Add((CarType)car);
            }
            //this.testercarComboBox.ItemsSource = Enum.GetValues(typeof(BE.CarType));
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            System.Windows.Data.CollectionViewSource testerViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("testerViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // testerViewSource.Source = [generic data source]
        }

        private void BackToMainMenue_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var testers = bl.TestersByCarType(true).Single(g => g.Key == (CarType)CarComboBox.SelectedItem);
            testerDataGrid.DataContext = testers;
        }
    }
}
