using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
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
using BL;
using BE;
using Calendar = System.Globalization.Calendar;

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for AddTestUserControl.xaml
    /// </summary>
    public partial class AddTestUserControl : UserControl
    {
        #region private vars
        private BL.IBL bl;
        private List<Trainee> AddTraineeListForPL; 
        private Test AddTestForPL;
        private Dictionary<string, List<int>> AvilabletestersForPL;
        private Address? traineeAdd;
       private BackgroundWorker sortTestersByAddress;
      private List<Tester> TesterByDistance;
        private string testDistance;
        #endregion
        public AddTestUserControl()
        {
            InitializeComponent();
            try
            {
                bl = IBL_imp.Instance;
                #region make the tab ready
                hours.Visibility = Visibility.Hidden; 
                TesterComboBox.IsEnabled = false;
                AddDateErrors.Foreground = Brushes.Red;
                HoursErrors.Foreground = Brushes.Red;
                TesterErrors.Foreground = Brushes.Red;
                AddTestForPL = new Test();
                TestAddGrid.DataContext = AddTestForPL;
                dateAndHourOfTestTextBlock.DataContext = AddTestForPL.DateAndHourOfTest.ToString();
                AddTraineeListForPL = bl.readyTrainees();
                emptyAddTab();
                if (AddTraineeListForPL.Count == 0)
                    throw new Exception("ERROR. There are no trainees ready for a test.");
                this.AddTraineeIdComboBox.ItemsSource = AddTraineeListForPL.Select(x => x.TraineeId);
               AddTestCalender.DisplayDateStart = DateTime.Today;
                AddTestCalender.IsEnabled = false;
                hours.IsEnabled = false;
                testIdTextBlock.Text = Configuration.FirstTestId.ToString("D" + 8);
                if(DateTime.Now.Hour>BE.Configuration.EndOfWorkDay-1)
                    AddTestCalender.BlackoutDates.Add(new CalendarDateRange(DateTime.Today));
                 #endregion
            }
            catch (Exception exception)
            {
                emptyAddTab();
                TestAddGrid.IsEnabled = false;
                calenderAndHours.IsEnabled = false;
                TesterErrors.Text = exception.Message;
                TesterErrors.Visibility = Visibility.Visible;
                TesterErrors.Foreground = Brushes.Red;
            }

        }


        #region empty
        public void emptyAddTab()
        {
            blErrors.Visibility = Visibility.Collapsed;
            blErrors.Text = "";
            AddTestCalender.IsEnabled = false;
            Save.IsEnabled = false;
            dateAndHourOfTestTextBlock.Text = "";
            TestAddressErrors.Visibility = Visibility.Collapsed;
            TestAddressErrors.Text = "";
            testerAddress.Text = "";
            carTypeTextBlock.Text = "";
            street.Text = "";
            stNumber.Text = "";
            city.Text = "";
            hours.SelectedItem = null;
            traineeAddress.Text = "";
            findTesters.IsEnabled = false;
            emptyErrors();
            TesterComboBox.SelectedIndex = -1;
        }

        public void emptyErrors()
        {
            AddDateErrors.Text = "";
            AddDateErrors.Visibility = Visibility.Collapsed;
            HoursErrors.Visibility = Visibility.Collapsed;
            HoursErrors.Text = "";
            TesterErrors.Text = "";
            TesterErrors.Visibility = Visibility.Collapsed;

        }
        #endregion

        #region calender

        private void AddTestCalender_OnDisplayDateChanged(object sender, CalendarDateChangedEventArgs e)
        {
            //blacks out fridays and saterdays when month is changed
            blackoutFridaysAndSaterdays((DateTime) AddTestCalender.DisplayDate,
                ((DateTime) AddTestCalender.DisplayDate).AddDays(60));
            Blackoutdays(1, AddTestCalender.DisplayDate.Month);

        }

        private void AddTestCalender_OnSelectedDatesChanged(object sender,
            SelectionChangedEventArgs selectionChangedEventArgs)
        {
            try
            {
                TesterComboBox.Visibility = Visibility.Hidden;
                TestAddressBlock.IsEnabled = true;
                findTesters.IsEnabled = true;
                testerAddress.Text = "";
                hours.Items.Clear();
                hours.Visibility = Visibility.Hidden;
                AddDateErrors.Text = "";
                TesterComboBox.SelectedIndex=-1;
                AddTestForPL.TestDate = (DateTime) AddTestCalender.SelectedDate;
                AvilabletestersForPL = bl.AvailableTesterFound(AddTestForPL);
                TesterComboBox.ItemsSource = AvilabletestersForPL.Keys.ToList();
                TesterComboBox.SelectedItem = null;
                AddDateErrors.Visibility = Visibility.Collapsed;
                AddDateErrors.Text = "";
                TimeSpan ts = new TimeSpan(9, 0, 0);
                if ((DateTime) AddTestCalender.SelectedDate == DateTime.Today)
                {
                    ts = new TimeSpan(DateTime.Now.Hour + 1, 0, 0);
                }

                AddTestForPL.TestDate = (DateTime) AddTestCalender.SelectedDate;
                AddTestForPL.DateAndHourOfTest = (DateTime) AddTestCalender.SelectedDate + ts;
                dateAndHourOfTestTextBlock.Text = AddTestForPL.DateAndHourOfTest.ToString();
                hours.SelectedIndex = 0;
                HoursErrors.Visibility = Visibility.Collapsed;
                HoursErrors.Text = "";
                hours.IsEnabled = true;
                AddDateErrors.Visibility = Visibility.Collapsed;
                AddDateErrors.Text = "";
            }
            catch (Exception exception)
            {
                AddDateErrors.Visibility = Visibility.Visible;
                AddDateErrors.Text = exception.Message;
                AddDateErrors.Foreground = Brushes.Red;
            }
        }

        public void blackoutFridaysAndSaterdays(DateTime startdate, DateTime enddate)
        {
            try
            {


                // step forward to the first friday
                while (startdate.DayOfWeek != DayOfWeek.Friday)
                    startdate = startdate.AddDays(1);

                while (startdate < enddate)
                {

                    AddTestCalender.BlackoutDates.Add(new CalendarDateRange(startdate));
                    AddTestCalender.BlackoutDates.Add(new CalendarDateRange(startdate.AddDays(1)));
                    startdate = startdate.AddDays(7);
                }

                AddDateErrors.Visibility = Visibility.Collapsed;
                AddDateErrors.Text = "";
            }
            catch (Exception exception)
            {
                AddDateErrors.Visibility = Visibility.Visible;
                AddDateErrors.Text = exception.Message;

            }
        }

        public void Blackoutdays(int start, int startMonth)
        {
            try
            {
                if (DateTime.Now.Hour >= Configuration.EndOfWorkDay)
                {
                    AddTestCalender.BlackoutDates.Add(new CalendarDateRange(
                        DateTime.Today));
                }

                for (;
                    start <= DateTime.DaysInMonth(AddTestCalender.DisplayDate.Year, startMonth);
                    start++)
                {
                    AddTestForPL.TestDate = new DateTime(AddTestCalender.DisplayDate.Year,
                        AddTestCalender.DisplayDate.Month, start);
                    //if there are no available testers in that hour
                    if (bl.AvailableTesterFound(AddTestForPL) == null)
                        AddTestCalender.BlackoutDates.Add(new CalendarDateRange(
                            new DateTime(AddTestCalender.DisplayDate.Year, startMonth, start)));

                }
            }
            catch (Exception exception)
            {
                AddDateErrors.Text = exception.Message;
                AddDateErrors.Visibility = Visibility.Visible;
            }
        }

        #endregion

        #region combobox
        private void TraineeIdComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                TesterByDistance = new List<Tester>();
                hours.Visibility = Visibility.Hidden;
                if (AddTraineeIdComboBox.SelectedIndex != -1)
                {
                    emptyAddTab();
                    if(sortTestersByAddress!=null)
                        if(sortTestersByAddress.IsBusy)
                        sortTestersByAddress.CancelAsync();

                    AddTestCalender.BlackoutDates.Clear();
                    AddTestForPL.TraineeId = AddTraineeIdComboBox.SelectedItem.ToString();
                    AddTestForPL.CarType = bl.GetListOfTrainees().Where(x => x.TraineeId == AddTestForPL.TraineeId)
                        .Select(x => x.Traineecar).FirstOrDefault();

                if (AddTestForPL.CarType == null)
                    throw new Exception("ERROR. Add a car type to the trainee first");
                carTypeTextBlock.Text = AddTestForPL.CarType.ToString();
                if(bl.GetListOfTesters().All(x=>x.Testercar!=AddTestForPL.CarType))
                    throw new Exception("ERROR. there are no testers with that car type.");

                Blackoutdays(1, AddTestCalender.DisplayDate.Month);
                AddTestCalender.DisplayDate = new DateTime(AddTestCalender.DisplayDate.Year,AddTestCalender.DisplayDate.Month,1);
               blackoutFridaysAndSaterdays(AddTestCalender.DisplayDate, AddTestCalender.DisplayDate.AddDays(60));

                    traineeAdd = bl.GetListOfTrainees().Where(x => x.TraineeId == AddTestForPL.TraineeId)
                    .Select(x => (x.TraineeAddress)).FirstOrDefault();
                    string address = traineeAdd.ToString();
                    if (traineeAdd != null)
                    {
                        traineeAddress.Text = address;
                        city.Text = traineeAdd.Value.City;
                        street.Text = traineeAdd.Value.Street;
                        stNumber.Text = traineeAdd.Value.BuildingNumber;
                    }
                    else traineeAddress.Text = "Address not found";


                AddTestCalender.IsEnabled = true;
                    TestAddressBlock.IsEnabled = false;
                    findTesters.IsEnabled = false;

                }

            }
            catch (Exception exception)
            {
                TesterErrors.Text = exception.Message;
                TesterErrors.Visibility = Visibility.Visible;
                TesterErrors.Foreground = Brushes.Red;
                AddTestCalender.IsEnabled = false;
                
            }

        }

        private void TesterComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                TestAddressErrors.Visibility = Visibility.Collapsed;
                TestAddressErrors.Text = "";
                hours.Items.Clear();
                hours.Visibility = Visibility.Hidden;
                AddTestForPL.TesterId = TesterComboBox.SelectedItem.ToString();
                string address = bl.GetListOfTesters().Where(x => x.TesterId == AddTestForPL.TesterId)
                    .Select(x => (x.TesterAdress.ToString())).FirstOrDefault();
                if (address != null)
                {
                    testerAddress.Text = address;
                }
                else testerAddress.Text = "Address not found";
                List<int> hoursOfTester = AvilabletestersForPL[AddTestForPL.TesterId];
                TimeSpan ts = new TimeSpan(hoursOfTester.First(), 0, 0);
                AddTestForPL.DateAndHourOfTest = AddTestForPL.DateAndHourOfTest.Date;
                AddTestForPL.DateAndHourOfTest= AddTestForPL.DateAndHourOfTest + ts;
                dateAndHourOfTestTextBlock.Text = AddTestForPL.DateAndHourOfTest.ToString();


                if (hoursOfTester != null)
                {
                    hours.Visibility = Visibility.Visible;
                    foreach (var time in hoursOfTester)
                    {
                        if (AddTestForPL.TestDate.Date != DateTime.Today)
                        {
                            string timeframe = "" + time + ":00-" + (time + 1) + ":00";
                            hours.Items.Add(timeframe);
                            
                        }
                         else if (DateTime.Now.Hour < time)
                        {
                            string timeframe = "" + time + ":00-" + (time + 1) + ":00";
                            hours.Items.Add(timeframe);

                        }
                    }


                }
            }
            catch (Exception exception)
            {

            }

        }
        #endregion
        
        private void Hours_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                TimeSpan ts;
                if(hours.Items.Count!=null)
                switch (hours.SelectedItem.ToString())
                {
                    case "9:00-10:00": //nine 
                        ts = new TimeSpan(9, 0, 0);
                        AddTestForPL.DateAndHourOfTest = AddTestForPL.DateAndHourOfTest.Date + ts;
                        dateAndHourOfTestTextBlock.Text = AddTestForPL.DateAndHourOfTest.ToString();
                        break;
                    case "10:00-11:00": //ten
                        ts = new TimeSpan(10, 0, 0);
                        AddTestForPL.DateAndHourOfTest = AddTestForPL.DateAndHourOfTest.Date + ts;
                        dateAndHourOfTestTextBlock.Text = AddTestForPL.DateAndHourOfTest.ToString();
                        break;
                    case "11:00-12:00": //eleven
                        ts = new TimeSpan(11, 0, 0);
                        AddTestForPL.DateAndHourOfTest = AddTestForPL.DateAndHourOfTest.Date + ts;
                        dateAndHourOfTestTextBlock.Text = AddTestForPL.DateAndHourOfTest.ToString();
                        break;
                    case "12:00-13:00": //twelve
                        ts = new TimeSpan(12, 0, 0);
                        AddTestForPL.DateAndHourOfTest = AddTestForPL.DateAndHourOfTest.Date + ts;
                        dateAndHourOfTestTextBlock.Text = AddTestForPL.DateAndHourOfTest.ToString();
                        break;
                    case "13:00-14:00": //one
                        ts = new TimeSpan(13, 0, 0);
                        AddTestForPL.DateAndHourOfTest = AddTestForPL.DateAndHourOfTest.Date + ts;
                        dateAndHourOfTestTextBlock.Text = AddTestForPL.DateAndHourOfTest.ToString();
                        break;
                    case "14:00-15:00": //two
                        ts = new TimeSpan(14, 0, 0);
                        AddTestForPL.DateAndHourOfTest = AddTestForPL.DateAndHourOfTest.Date + ts;
                        dateAndHourOfTestTextBlock.Text = AddTestForPL.DateAndHourOfTest.ToString();
                        break;

                }
                Save.IsEnabled=true;
                HoursErrors.Visibility = Visibility.Collapsed;
                HoursErrors.Text = "";

            }
            catch (Exception exception)
            {
                if (exception.Message != "Object reference not set to an instance of an object.")
                {

                HoursErrors.Visibility = Visibility.Visible;
                HoursErrors.Text = exception.Message;
                HoursErrors.Foreground = Brushes.Red;
                }
            }


        }

        #region test address

        private void sortTestersByAddress_DoWork(object sender, DoWorkEventArgs e)
        {
            
            List<Tester> TestersWithcar = bl.GetListOfTesters().Where(x => x.Testercar == AddTestForPL.CarType && x.TesterAdress != null && AvilabletestersForPL.ContainsKey(x.TesterId)).ToList();
            if (TestersWithcar.Count > 0)
                e.Result = bl.TestersInArea(TestersWithcar, AddTestForPL.StartingPoint);
            

        }

        private void sortTestersByAddress_Complete(object sender, RunWorkerCompletedEventArgs e)
        {
            TesterByDistance = new List<Tester>();
            TesterByDistance = (List<Tester>)e.Result;
            List<string> fileredDistance = new List<string>();

            foreach (var tester in TesterByDistance)
            {
                if (AvilabletestersForPL.ContainsKey(tester.TesterId))
                    fileredDistance.Add(tester.TesterId);
            }

            
            if (fileredDistance.Count != 0)
            {
            TesterComboBox.ItemsSource = fileredDistance;
            TesterComboBox.IsEnabled = true;
            TesterComboBox.Visibility = Visibility.Visible;
                TesterErrors.Text = "";
                TesterErrors.Visibility = Visibility.Collapsed;
            }
            else
            {
                TesterErrors.Text = "ERROR. No testers found. Check internet and Tester addresses";
                TesterErrors.Foreground = Brushes.Red;
                TesterErrors.Visibility = Visibility.Visible;


            }
            findTesters.Content = "Find Testers";
        }

        private void FindTesters_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if(bl.GetListOfTests()!=null)
                if (!bl.NoConflictingTests(AddTestForPL))
                {
                 throw new Exception("ERROR. can't add dates that are less than a week apart.");
                }

                TesterComboBox.Visibility = Visibility.Hidden;
                TestAddressErrors.Visibility = Visibility.Collapsed;
                checkAddress();

                sortTestersByAddress = new BackgroundWorker();
                sortTestersByAddress.DoWork += sortTestersByAddress_DoWork;
                sortTestersByAddress.RunWorkerCompleted += sortTestersByAddress_Complete;
                sortTestersByAddress.RunWorkerAsync();
                findTesters.Content = "Waiting";
                TestAddressErrors.Text = "";
                TestAddressErrors.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("not include numbers"))
                {
                    TestAddressErrors.Text = "ERROR. street/city must not include numbers.";
                }
                else if (ex.Message.Contains("only include numbers"))
                {
                    TestAddressErrors.Text = "ERROR. street number must only include numbers.";
                }
                else
                {
                TestAddressErrors.Text = ex.Message;
                }
                TestAddressErrors.Visibility = Visibility.Visible;
                TestAddressErrors.Foreground = Brushes.Red;


            }

        }
        
        void checkAddress()
        {

                bl.IsText(city.Text);
                bl.IsText(street.Text);
                bl.IsNumber(stNumber.Text);
                AddTestForPL.StartingPoint=new Address(street.Text,stNumber.Text,city.Text);          
        }

        #endregion

        #region checks
        private void checkErrors()
        {
            if(AddDateErrors.Text!="" && AddDateErrors.Text != null)
                throw new Exception();
            if (TesterErrors.Text != "" && TesterErrors.Text != null)
                throw new Exception();
            if (HoursErrors.Text != "" && HoursErrors.Text != null)
                throw new Exception();
            if (TestAddressErrors.Text != "" && TestAddressErrors.Text != null)
                throw new Exception();
            


        }

        private void checkFields()
        {
            if(AddTestForPL.DateAndHourOfTest==null)
                throw new Exception();
            if (AddTestForPL.TesterId == null)
                throw new Exception();
            if (AddTestForPL.TestId == null)
                throw new Exception();
            if (AddTestForPL.TraineeId == null)
                throw new Exception();
            if (AddTestForPL.TestDate == null)
                throw new Exception();
            if (AddTestForPL.StartingPoint == null)
                throw new Exception();
        }
        #endregion
            
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {  
                    
                    checkErrors();
                    checkFields();
                    
                    bl.AddTest(AddTestForPL);
                    AddTestForPL = new Test();
                    TestAddGrid.DataContext = AddTestForPL;
                        testIdTextBlock.Text = (Configuration.FirstTestId).ToString("D" + 8);
                        AddTestCalender.DisplayDate = new DateTime(AddTestCalender.DisplayDate.Year, AddTestCalender.DisplayDate.Month, 1);
                TesterComboBox.IsEnabled = false;
                        MessageBox.Show("Test successfully added.", "", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                emptyAddTab();
                

            }
            catch (Exception ex)
            {
                MessageBox.Show("Can't add test. Check Errors and empty Fields.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                
            }
        }

    }
}
