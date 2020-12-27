using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BE;
using BL;
using CheckBox = System.Windows.Controls.CheckBox;
using MessageBox = System.Windows.MessageBox;
using UserControl = System.Windows.Controls.UserControl;

//hi
// how to make an xml file in configuration. 
//read on serialise
//read on how to make schedual to xml
//read on background worker
namespace PLWPF
{
    public partial class UpdateTest : UserControl
    {
        private BL.IBL bl;
        private BE.Test TestForPL;
        private List<Test> TestListForPL;
        public UpdateTest()
        {
            InitializeComponent();   
            try
            {
                testerIdTextBox.IsReadOnly = true;
                traineeIdTextBox.IsReadOnly = true;

                checkMirrorsCheckBox.Checked += CheckBox_Checked;
                imediateStopCheckBox.Checked += CheckBox_Checked;
                keptDistanceCheckBox.Checked += CheckBox_Checked;
                keptRightofPresidenceCheckBox.Checked += CheckBox_Checked;
                parkingCheckBox.Checked += CheckBox_Checked;
                reverseParkingCheckBox.Checked += CheckBox_Checked;
                rightTurnCheckBox.Checked += CheckBox_Checked;
                stoppedAtcrossWalkCheckBox.Checked += CheckBox_Checked;
                stoppedAtRedCheckBox.Checked += CheckBox_Checked;
                usedSignalCheckBox.Checked += CheckBox_Checked;

                bl = IBL_imp.Instance;
                TestForPL = new Test();
                this.TestDetailsGrid.DataContext = TestForPL;
                this.commentsGroupBox.DataContext = TestForPL;
                this.testPassedGrid.DataContext = TestForPL;
                this.BoolItemsGrid.DataContext = TestForPL;
                TestListForPL = bl.GetListOfTests();
                if(TestListForPL==null)
                   throw new Exception("There are no Tests to update");
                this.DataContext = TestForPL;
                this.testIdComboBox.ItemsSource = bl.GetListOfTests().OrderByDescending(x=>x.TestId);
                closeAlmostAll();
                if (TestListForPL.Count == 0)
                    throw new Exception("There are no Tests to update");
            }
            catch (Exception ex)
            {
                testIdComboBox.IsEnabled = false;
                noTests.Visibility = Visibility.Visible;
            }
        }

        int counter = 0;
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox check = sender as CheckBox;
            if (check.IsChecked == true)
                counter++;
            if (counter > 5)
                testPassedCheckBox.IsChecked = true;
            //throw new NotImplementedException();
        }

        #region function
        public void closeAlmostAll()
        {
            testDateDatePicker.IsEnabled = false;
            testerIdTextBox.IsEnabled = false;
            traineeIdTextBox.IsEnabled = false;
            commentsGroupBox.IsEnabled = false;
            testPassedCheckBox.IsEnabled = false;
            BoolItemsGrid.IsEnabled = false;
            saveButton.IsEnabled = false;
            checkMirrorsCheckBox.IsChecked = false;
            imediateStopCheckBox.IsChecked = false;
            keptRightofPresidenceCheckBox.IsChecked = false;
            parkingCheckBox.IsChecked = false;
            reverseParkingCheckBox.IsChecked = false;
            rightTurnCheckBox.IsChecked = false;
            stoppedAtcrossWalkCheckBox.IsChecked = false;
            stoppedAtRedCheckBox.IsChecked = false;
            usedSignalCheckBox.IsChecked = false;
        }

        public void openAll()
        {
            testerIdTextBox.IsEnabled = true;
            traineeIdTextBox.IsEnabled = true;
            commentsGroupBox.IsEnabled = true;
            BoolItemsGrid.IsEnabled = true;
            saveButton.IsEnabled = true;
        }

        void setChecks()
        {
            
            checkMirrorsCheckBox.IsChecked = TestForPL.CheckMirrors;
            imediateStopCheckBox.IsChecked = TestForPL.ImediateStop;
            keptRightofPresidenceCheckBox.IsChecked = TestForPL.KeptRightofPresidence;
            parkingCheckBox.IsChecked = TestForPL.Parking;
            reverseParkingCheckBox.IsChecked = TestForPL.ReverseParking;
            rightTurnCheckBox.IsChecked = TestForPL.RightTurn;
            stoppedAtcrossWalkCheckBox.IsChecked = TestForPL.StoppedAtcrossWalk;
            stoppedAtRedCheckBox.IsChecked = TestForPL.StoppedAtRed;
            usedSignalCheckBox.IsChecked = TestForPL.UsedSignal;
        }

        void fillFields()
        {
            if (keptDistanceCheckBox.IsChecked == true)
                TestForPL.KeptDistance = true;
            else
                TestForPL.KeptDistance = false;

            if (parkingCheckBox.IsChecked == true)
                TestForPL.Parking = true;
            else TestForPL.Parking = false;

            if (reverseParkingCheckBox.IsChecked == true)
                TestForPL.ReverseParking = true;
            else TestForPL.ReverseParking = false;

            if (checkMirrorsCheckBox.IsChecked == true)
                TestForPL.CheckMirrors = true;
            else TestForPL.CheckMirrors = false;

            if (usedSignalCheckBox.IsChecked == true)
                TestForPL.UsedSignal = true;
            else TestForPL.UsedSignal = false;

            if (keptRightofPresidenceCheckBox.IsChecked == true)
                TestForPL.KeptRightofPresidence = true;
            else TestForPL.KeptRightofPresidence = false;

            if (stoppedAtRedCheckBox.IsChecked == true)
                TestForPL.StoppedAtRed = true;
            else TestForPL.StoppedAtRed = false;

            if (stoppedAtcrossWalkCheckBox.IsChecked == true)
                TestForPL.StoppedAtcrossWalk = true;
            else TestForPL.StoppedAtcrossWalk = false;

            if (rightTurnCheckBox.IsChecked == true)
                TestForPL.RightTurn = true;
            else TestForPL.RightTurn = false;

            if (imediateStopCheckBox.IsChecked == true)
                TestForPL.ImediateStop = true;
            else TestForPL.ImediateStop = false;

            if (testPassedCheckBox.IsChecked == true)
                TestForPL.TestPassed = true;
            else TestForPL.TestPassed = false;

            TestForPL.RemarksOnTest = commentsTextBox.Text;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            System.Windows.Data.CollectionViewSource testViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("testViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // testViewSource.Source = [generic data source]
        }
        #endregion

        private void TestIdComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(testIdComboBox.SelectedItem != null)
            {
                string id = ((Test)testIdComboBox.SelectedItem).TestId;
                TestForPL = bl.GetListOfTests().FirstOrDefault(a => a.TestId == id);
                setChecks();
                testPassedCheckBox.IsChecked = TestForPL.TestPassed;
                testerIdTextBox.Text = TestForPL.TesterId;
                traineeIdTextBox.Text = TestForPL.TraineeId;
                testDateDatePicker.Text = TestForPL.TestDate.ToString();
                openAll();

            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                switch(saveButton.Content.ToString())
                {
                    case "Save":
                        fillFields();
                        bl.UpdateTest(TestForPL);
                        TestForPL = new Test();
                        this.TestDetailsGrid.DataContext = TestForPL;
                        this.commentsGroupBox.DataContext = TestForPL;
                        MessageBox.Show("Test saved successfully", "", MessageBoxButton.OK, MessageBoxImage.Information);
                        keptDistanceCheckBox.IsChecked = false;
                        testPassedCheckBox.IsChecked = false;
                        closeAlmostAll();
                        break;

                    case "Check":
                        saveButton.Content = "Save";
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message+" Test not saved.", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                saveButton.Content = "Check";
            }
        }
    }
}
