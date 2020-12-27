using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup.Localizer;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BE;
using BL;

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for traineesByTeachers.xaml
    /// </summary>
    public partial class traineesByTeachers : Window
    {
        private BL.IBL bl;
        public traineesByTeachers()
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            bl = IBL_imp.Instance;
            IEnumerable<IGrouping<string, Trainee>> TraineesByTeachers = bl.TraineesByTeachers(true);
            foreach (var teachergroup in TraineesByTeachers)
            {
                var teacher = teachergroup.Key;
                TeacherComboBox.Items.Add((string)teacher);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            System.Windows.Data.CollectionViewSource traineeViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("traineeViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // traineeViewSource.Source = [generic data source]
        }

        private void BackToMainMenue_Click(object sender, RoutedEventArgs e)
        {
        this.Close();
        }

        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var trainees = bl.TraineesByTeachers(true).Single(g => g.Key == (string)TeacherComboBox.SelectedItem);
            traineeDataGrid.DataContext = trainees;
        }
    }
}
