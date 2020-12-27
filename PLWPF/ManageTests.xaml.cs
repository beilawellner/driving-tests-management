using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class ManageTests : Window
    {
        private BL.IBL bl;
        private List<Trainee> AddTraineeListForPL;
        private Test AddTestForPL;

        public ManageTests()
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;

            bl = IBL_imp.Instance;
           
            //if (bl.GetListOfTests() == null)
            //{
            //    Update.Visibility = Visibility.Hidden;
            //}

            //else if (bl.GetListOfTests().Count == 0)
            //{
            //    Update.Visibility = Visibility.Hidden;
            //}



        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            System.Windows.Data.CollectionViewSource testViewSource =
                ((System.Windows.Data.CollectionViewSource) (this.FindResource("testViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // testViewSource.Source = [generic data source]
        }

        private void AddTestUserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }

        //private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    //if (Update.IsSelected)
        //    //{
        //      try
        //        {
        //    //        AddTestForPL = new Test();
        //    //        TestAddGrid.DataContext = AddTestForPL;
        //    //        AddTraineeListForPL = bl.readyTrainees();
        //    //        emptyAddTab();
        //    //        AddTestCalender.SelectedDate = null;
        //    //        hours.IsEnabled = false;
        //        }
        //        catch (Exception exception)
        //       {
        //    //        MessageBox.Show(exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        //        }


        // //   }
        //}

    }
}
