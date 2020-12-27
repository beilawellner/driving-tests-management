using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
//using System.Windows.Forms;
using BE;
using BL;

/*
 to find out:
 selections items to previues day
 */
namespace PLWPF
{
    /// <summary>
    /// Interaction logic for TestersWindow.xaml
    /// </summary>
    public partial class TestersWindow : Window
    {
        private BL.IBL bl;
        private BE.Tester TesterForPL;
        private List<Tester> TesterListForPL;
        string winCondition;
        //lists for schedule
        bool[] SundayArr = new bool[6];
        bool[] MondayArr = new bool[6];
        bool[] TuesdayArr = new bool[6];
        bool[] WednesdayArr = new bool[6];
        bool[] ThursdayArr = new bool[6];
        //List<int> SundayHours = new List<int>();
        //List<int> MondayHours = new List<int>();
        //List<int> TuesdayHours = new List<int>();
        //List<int> WednesdayHours = new List<int>();
        //List<int> ThursdayHours = new List<int>();
        bool[,] hoursFromSchedualArr = new bool[6, 5];
        bool notAllIsFalse = false;
        public TestersWindow()
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            TesterGrid.Visibility = Visibility.Hidden;
            bl = IBL_imp.Instance;
            TesterForPL = new Tester();
            this.TesterGrid.DataContext = TesterForPL;
            this.TesterComboBox.DataContext = TesterListForPL;
            //Save.IsEnabled = false;
            //manage calendar
            dateOfBirthDatePicker.DisplayDateEnd = DateTime.Now.AddYears(-1 * (int)BE.Configuration.MinAgeOFTester);
            dateOfBirthDatePicker.DisplayDateStart = DateTime.Now.AddYears(-1 * (int)BE.Configuration.MaxAgeOFTester);
            //enums
            this.testerGenderComboBox.ItemsSource = Enum.GetValues(typeof(BE.Gender));
            this.testercarComboBox.ItemsSource = Enum.GetValues(typeof(BE.CarType));
            //for numbers only
            this.testerIdTextBox.PreviewTextInput += TextBox_PreviewTextInputNumbers;
            this.phoneNumberTextBox.PreviewTextInput += TextBox_PreviewTextInputNumbers;
            this.maxDistanceForTestTextBox.PreviewTextInput += TextBox_PreviewTextInputNumbers;
            this.yearsOfExperienceTextBox.PreviewTextInput += TextBox_PreviewTextInputNumbers;
            this.maxTestsInaWeekTextBox.PreviewTextInput += TextBox_PreviewTextInputNumbers;
            //for letters only
            this.sirnameTextBox.PreviewTextInput += TextBox_PreviewTextInputLetters;
            this.firstNameTextBox.PreviewTextInput += TextBox_PreviewTextInputLetters;
            this.City.PreviewTextInput += TextBox_PreviewTextInputLetters;
        }

        #region manage buttons
        private void BackToMainMenue_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            //MainWindow.Show();
        }
        private void AddTester_Click(object sender, RoutedEventArgs e)
        {
            winCondition = "add";
            TesterComboBox.ItemsSource = bl.GetListOfTesters().Select(x => x.TesterId);
            TesterForPL = new Tester();
            openAll();
            TesterGrid.DataContext = TesterForPL;
            IdErrors.Text = "";
            testerIdTextBox.Visibility = Visibility.Visible;
            TesterGrid.Visibility = Visibility.Visible;
            TesterComboBox.Visibility = Visibility.Hidden;
            TesterGrid.IsEnabled = true;
            Save.Content = "Check";
        }

        private void UpdateTester_Click(object sender, RoutedEventArgs e)
        {
            winCondition = "update";
            try
            {
                TesterForPL = new Tester();
                TesterComboBox.SelectedItem = null;
                closeAlmostAll();
                TesterGrid.DataContext = TesterForPL;
                IdErrors.Text = "First Select ID";
                IdErrors.Foreground = Brushes.DarkBlue;
                TesterListForPL = bl.GetListOfTesters();
                TesterComboBox.ItemsSource = bl.GetListOfTesters().Select(x => x.TesterId);
                if (TesterListForPL.Count == 0)
                    throw new Exception("There are no Testers to update.");
                TesterGrid.Visibility = Visibility.Visible;
                TesterGrid.IsEnabled = true;
                
                Save.Content = "Check";
                TesterComboBox.Visibility = Visibility.Visible;
                testerIdTextBox.Visibility = Visibility.Hidden;

            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteTester_Click(object sender, RoutedEventArgs e)
        {
            winCondition = "delete";
            try
            {
                Save.Content = "Delete";
                TesterForPL = new Tester();
                TesterGrid.Visibility = Visibility.Visible;
                TesterComboBox.Visibility = Visibility.Visible;
                TesterComboBox.SelectedItem = null;
                TesterGrid.DataContext = TesterForPL;
                closeAlmostAll();
                IdErrors.Text = "First Select ID";
                TesterListForPL = bl.GetListOfTesters();
                TesterComboBox.ItemsSource = bl.GetListOfTesters().Select(x => x.TesterId);
                if (TesterListForPL.Count == 0)
                    throw new Exception("There are no testers to update.");
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (Save.Content == "Add")
            {
                try
                {
                    //TesterForPL.TesterAddress = new Address(Street.Text, BuidingNumber.Text, City.Text);
                    TesterForPL.setSchedual(SundayArr, MondayArr, TuesdayArr, WednesdayArr, ThursdayArr);
                    bl.AddTester(TesterForPL);
                    TesterGrid.Visibility = Visibility.Hidden;
                    MessageBox.Show("Tester saved successfully", "", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    TesterGrid.Visibility = Visibility.Visible;
                }

            }
            if (Save.Content == "Update")
            {
                //TesterForPL.TesterAddress = new Address(Street.Text, BuidingNumber.Text, City.Text);

                bl.UpdateTester(TesterForPL);
                TesterGrid.Visibility = Visibility.Hidden;
                MessageBox.Show("Tester saved successfully", "", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            if (Save.Content == "Delete")
            {
                MessageBoxResult dialogResult = MessageBox.Show("Are you sure you want to delete?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                if (dialogResult == MessageBoxResult.Yes)
                {
                    bl.DeleteTester(TesterForPL);
                    TesterGrid.Visibility = Visibility.Hidden;
                    MessageBox.Show("Tester successfully deleted.", "", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                    TesterForPL = new Tester();
                    TesterGrid.DataContext = TesterForPL;
                }
                else if (dialogResult == MessageBoxResult.No)
                {
                    TesterForPL = new Tester();
                    TesterGrid.DataContext = TesterForPL;
                    TesterGrid.Visibility = Visibility.Hidden;
                    MessageBox.Show("Tester not deleted.", "", MessageBoxButton.OK, MessageBoxImage.Asterisk);

                }
            }

            if (Save.Content == "Check")
            {
                //empty filed
                if (string.IsNullOrWhiteSpace(firstNameTextBox.Text))
                {
                    NameErrors.Text = "Warning. Filed is empty";
                    NameErrors.Foreground = Brushes.Orange;
                    firstNameTextBox.BorderBrush = Brushes.Orange;
                }
                if (string.IsNullOrWhiteSpace(sirnameTextBox.Text))
                {
                    SirNameErrors.Text = "Warning. Filed is empty";
                    SirNameErrors.Foreground = Brushes.Orange;
                    sirnameTextBox.BorderBrush = Brushes.Orange;
                }
                if (testerGenderComboBox.SelectedItem == null)
                {
                    GenderError.Text = "Warning. No item selected";
                    GenderError.Foreground = Brushes.Orange;
                    testerGenderComboBox.BorderBrush = Brushes.Orange;
                }
                if (string.IsNullOrWhiteSpace(phoneNumberTextBox.Text))
                {
                    PhoneNumberErrors.Text = "Warning. Filed is empty";
                    PhoneNumberErrors.Foreground = Brushes.Orange;
                    phoneNumberTextBox.BorderBrush = Brushes.Orange;
                }
                if (string.IsNullOrWhiteSpace(phoneNumberTextBox.Text))
                {
                    PhoneNumberErrors.Text = "Warning. Filed is empty";
                    PhoneNumberErrors.Foreground = Brushes.Orange;
                    phoneNumberTextBox.BorderBrush = Brushes.Orange;
                }
                if (string.IsNullOrWhiteSpace(emailTextBox.Text))
                {
                    EmailErrors.Text = "Warning. Filed is empty";
                    EmailErrors.Foreground = Brushes.Orange;
                    emailTextBox.BorderBrush = Brushes.Orange;
                }
                if (testercarComboBox.SelectedItem == null)
                {
                    CarError.Text = "Warning. No item selected";
                    CarError.Foreground = Brushes.Orange;
                    testercarComboBox.BorderBrush = Brushes.Orange;
                }
                if (maxDistanceForTestTextBox.Text == "0" || maxDistanceForTestTextBox.Text == null)
                {
                    DistanceError.Text = "Warning. Filed is empty";
                    DistanceError.Foreground = Brushes.Orange;
                    maxDistanceForTestTextBox.BorderBrush = Brushes.Orange;
                }
                if (yearsOfExperienceTextBox.Text == "0" || yearsOfExperienceTextBox.Text == null)
                {
                    ExperienceErrors.Text = "Warning. Filed is empty";
                    ExperienceErrors.Foreground = Brushes.Orange;
                    yearsOfExperienceTextBox.BorderBrush = Brushes.Orange;
                }
                if (maxTestsInaWeekTextBox.Text == "0" || maxTestsInaWeekTextBox.Text == null)
                {
                    MaxTestsError.Text = "Warning. Filed is empty";
                    MaxTestsError.Foreground = Brushes.Orange;
                    maxTestsInaWeekTextBox.BorderBrush = Brushes.Orange;
                }
                if(string.IsNullOrWhiteSpace(City.Text))
                {
                    AddressErrors.Text = "Warning. Filed is empty";
                    AddressErrors.Foreground = Brushes.Orange;
                    City.BorderBrush = Brushes.Orange;
                }
                if (string.IsNullOrWhiteSpace(Street.Text))
                {
                    AddressErrors.Text = "Warning. Filed is empty";
                    AddressErrors.Foreground = Brushes.Orange;
                    Street.BorderBrush = Brushes.Orange;
                }
                if (!SundayArr.Any(x => x) && !MondayArr.Any(x => x) && !TuesdayArr.Any(x => x) && !WednesdayArr.Any(x => x) && !ThursdayArr.Any(x => x))//if schedule is empty
                {
                    ScheduleError.Text = "Warning, No times selected";
                    ScheduleError.Foreground = Brushes.Orange;
                }
                





                if (noErrors() && TesterComboBox.Visibility == Visibility.Hidden)
                {
                    if (bl.TesterInSystem(TesterForPL.TesterId))
                    {
                        MessageBoxResult dialogResult = MessageBox.Show("Tester alredy exists in the system! Do you want to update?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                        if (dialogResult == MessageBoxResult.Yes)
                        {
                            TesterComboBox.Visibility = Visibility.Visible;
                            TesterComboBox.SelectedValue = (object)TesterForPL.TesterId;
                            TesterForPL = bl.GetListOfTesters()
                                .FirstOrDefault(x => x.TesterId == testerIdTextBox.Text);
                        }
                        else if (dialogResult == MessageBoxResult.No)
                        {
                            TesterForPL = new Tester();
                            TesterGrid.DataContext = TesterForPL;
                        }

                    }
                    else Save.Content = "Add";
                }
                else if (noErrors())
                {
                    Save.Content = "Update";
                }
                else
                {
                    MessageBox.Show("Can't add Tester. Fill ID " +
                                    "and fix errors.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        #endregion

        #region additions
        public void closeAlmostAll()
        {
            testerIdTextBox.IsEnabled = false;
            firstNameTextBox.IsEnabled = false;
            sirnameTextBox.IsEnabled = false;
            dateOfBirthDatePicker.IsEnabled = false;
            testerGenderComboBox.IsEnabled = false;
            phoneNumberTextBox.IsEnabled = false;
            emailTextBox.IsEnabled = false;
            testercarComboBox.IsEnabled = false;
            maxDistanceForTestTextBox.IsEnabled = false;
            yearsOfExperienceTextBox.IsEnabled = false;
            maxTestsInaWeekTextBox.IsEnabled = false;
            //schedualListBox.IsEnabled = false;
            //nextDayButton.IsEnabled = false;
            //previousDayButton.IsEnabled = false;
            City.IsEnabled = false;
            Street.IsEnabled = false;
            BuidingNumber.IsEnabled = false;
            scheduleGrid.IsEnabled = false;
        }

        public void openAll()
        {
            testerIdTextBox.IsEnabled = true;
            firstNameTextBox.IsEnabled = true;
            sirnameTextBox.IsEnabled = true;
            dateOfBirthDatePicker.IsEnabled = true;
            testerGenderComboBox.IsEnabled = true;
            phoneNumberTextBox.IsEnabled = true;
            emailTextBox.IsEnabled = true;
            testercarComboBox.IsEnabled = true;
            maxDistanceForTestTextBox.IsEnabled = true;
            yearsOfExperienceTextBox.IsEnabled = true;
            maxTestsInaWeekTextBox.IsEnabled = true;
            City.IsEnabled = true;
            Street.IsEnabled = true;
            BuidingNumber.IsEnabled = true;
            scheduleGrid.IsEnabled = true;
        }

        public bool noErrors()
        {

            try
            {
                if (TesterForPL.TesterId == null)
                    throw new Exception();
                if (IdErrors.Text != "")
                    throw new Exception();
                if (NameErrors.Text.Contains("ERROR"))
                    throw new Exception();
                if (SirNameErrors.Text.Contains("ERROR"))
                    throw new Exception();
                if (PhoneNumberErrors.Text.Contains("ERROR"))
                    throw new Exception();
                if (EmailErrors.Text.Contains("ERROR"))
                    throw new Exception();
                return true;
            }
            catch (Exception exception)
            {
                return false;
            }
        }

        //for numbers only
        private void TextBox_PreviewTextInputNumbers(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("^[a-zA-Z]+$").IsMatch(e.Text);
        }

        //for numbers only
        private void TextBox_PreviewTextInputLetters(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^A-z]+").IsMatch(e.Text);
        }
        #endregion

        private void TesterComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            IdErrors.Text = "";
            if (Save.Content == "Check")
            {
                openAll();
            }

            string id = (string)TesterComboBox.SelectedItem;
            TesterForPL = bl.GetListOfTesters().FirstOrDefault(a => a.TesterId == id);
            {//schedual
                hoursFromSchedualArr = TesterForPL.getSchedual();
                for (int i = 0; i < 6; i++)//put selections hour for sunday
                {
                    if (hoursFromSchedualArr[i, 0])
                    {
                        schedualListBox.SelectedIndex = i;
                    }
                }

            }
            this.TesterGrid.DataContext = TesterForPL;
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            System.Windows.Data.CollectionViewSource testerViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("testerViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // testerViewSource.Source = [generic data source]
        }

        #region Schedual
        private void NextDayButton_Click(object sender, RoutedEventArgs e)
        {
            foreach(object ob in schedualListBox.Items)
            {

            }
            foreach (ListBoxItem SelectedItem in schedualListBox.SelectedItems)
            {
                SelectedItem.IsSelected = false;
            }
            if (winCondition == "add")
            {                
                switch (dayLabel.Content)
                {                    
                    case "Sunday":
                        dayLabel.Content = "Monday";//change the label
                        for (int i = 0; i < 6; i++)//show previes selections
                        {
                            if (MondayArr[i])
                            {
                                schedualListBox.SelectedIndex = i;
                            }
                        }

                        break;

                    case "Monday":
                        dayLabel.Content = "Tuesday";
                        for (int i = 0; i < 6; i++)
                        {
                            if (TuesdayArr[i])
                            {
                                schedualListBox.SelectedIndex = i;
                            }
                        }
                        break;

                    case "Tuesday":
                        dayLabel.Content = "Wednesday";
                        for (int i = 0; i < 6; i++)
                        {
                            if (WednesdayArr[i])
                            {
                                schedualListBox.SelectedIndex = i;
                            }
                        }
                        break;

                    case "Wednesday":
                        dayLabel.Content = "Thursday";
                        for (int i = 0; i < 6; i++)
                        {
                            if (ThursdayArr[i])
                            {
                                schedualListBox.SelectedIndex = i;
                            }
                        }
                        break;

                    case "Thursday":
                        dayLabel.Content = "Sunday";
                        for (int i = 0; i < 6; i++)
                        {
                            if (SundayArr[i])
                            {
                                schedualListBox.SelectedIndex = i;
                            }
                        }
                        break;
                }
            }
            else//update or delete
            {
                switch (dayLabel.Content)//change the Label
                {

                    case "Sunday":
                        dayLabel.Content = "Monday";
                        for (int i = 0; i < 6; i++)
                        {
                            if (hoursFromSchedualArr[i, 1])
                            {
                                schedualListBox.SelectedIndex = i;
                            }
                        }

                        break;

                    case "Monday":
                        dayLabel.Content = "Tuesday";
                        for (int i = 0; i < 6; i++)
                        {
                            if (hoursFromSchedualArr[i, 2])
                            {
                                schedualListBox.SelectedIndex = i;
                            }
                        }
                        break;

                    case "Tuesday":
                        dayLabel.Content = "Wednesday";
                        for (int i = 0; i < 6; i++)
                        {
                            if (hoursFromSchedualArr[i, 3])
                            {
                                schedualListBox.SelectedIndex = i;
                            }
                        }
                        break;

                    case "Wednesday":
                        dayLabel.Content = "Thursday";
                        for (int i = 0; i < 6; i++)
                        {
                            if (hoursFromSchedualArr[i, 4])
                            {
                                schedualListBox.SelectedIndex = i;
                            }
                        }
                        break;

                    case "Thursday":
                        dayLabel.Content = "Sunday";
                        for (int i = 0; i < 6; i++)
                        {
                            if (hoursFromSchedualArr[i, 0])
                            {
                                schedualListBox.SelectedIndex += i;
                            }
                        }
                        break;
                }
            }
        }

        private void PreviousDayButton_Click(object sender, RoutedEventArgs e)
        {
            if (winCondition == "add")
            {
                switch (dayLabel.Content)
                {

                    case "Sunday":
                        dayLabel.Content = "Thursday";//change the label
                        for (int i = 0; i < 6; i++)//show previes selection
                        {
                            if (ThursdayArr[i])
                            {
                                schedualListBox.SelectedIndex = i;
                            }
                        }
                        break;

                    case "Monday":
                        dayLabel.Content = "Sunday";
                        for (int i = 0; i < 6; i++)
                        {
                            if (SundayArr[i])
                            {
                                schedualListBox.SelectedIndex = i;
                            }
                        }
                        break;

                    case "Tuesday":
                        dayLabel.Content = "Monday";
                        for (int i = 0; i < 6; i++)
                        {
                            if (MondayArr[i])
                            {
                                schedualListBox.SelectedIndex = i;
                            }
                        }
                        break;

                    case "Wednesday":
                        dayLabel.Content = "Tuesday";
                        for (int i = 0; i < 6; i++)
                        {
                            if (TuesdayArr[i])
                            {
                                schedualListBox.SelectedIndex = i;
                            }
                        }
                        break;

                    case "Thursday":
                        dayLabel.Content = "Wednesday";
                        for (int i = 0; i < 6; i++)
                        {
                            if (WednesdayArr[i])
                            {
                                schedualListBox.SelectedIndex = i;
                            }
                        }
                        break;
                }
            }
            else//update or delete
            {
                switch (dayLabel.Content)//change the Label
                {

                    case "Sunday":
                        dayLabel.Content = "Thursday";
                        for (int i = 0; i < 6; i++)
                        {
                            if (hoursFromSchedualArr[i, 4])
                            {
                                schedualListBox.SelectedIndex = i;
                            }
                        }
                        break;

                    case "Monday":
                        dayLabel.Content = "Sunday";
                        for (int i = 0; i < 6; i++)
                        {
                            if (hoursFromSchedualArr[i, 0])
                            {
                                schedualListBox.SelectedIndex += i;
                            }
                        }
                        break;

                    case "Tuesday":
                        dayLabel.Content = "Monday";
                        for (int i = 0; i < 6; i++)
                        {
                            if (hoursFromSchedualArr[i, 1])
                            {
                                schedualListBox.SelectedIndex = i;
                            }
                        }
                        break;

                    case "Wednesday":
                        dayLabel.Content = "Tuesday";
                        for (int i = 0; i < 6; i++)
                        {
                            if (hoursFromSchedualArr[i, 2])
                            {
                                schedualListBox.SelectedIndex = i;
                            }
                        }
                        break;

                    case "Thursday":
                        dayLabel.Content = "Wednesday";
                        for (int i = 0; i < 6; i++)
                        {
                            if (hoursFromSchedualArr[i, 3])
                            {
                                schedualListBox.SelectedIndex = i;
                            }
                        }
                        break;
                }
            }
        }

        private void SchedualListBox_MouseLeave(object sender, MouseEventArgs e)
        {
            switch (dayLabel.Content)//change the hours in list
            {
                case "Sunday":
                    notAllIsFalse = SundayArr.Any(x => x);
                    if (notAllIsFalse)//if there is values from previous time we delete it
                        Array.Clear(SundayArr, 0, SundayArr.Length);
                    foreach (ListBoxItem SelectedItem in schedualListBox.SelectedItems)
                    {
                        SundayArr[schedualListBox.Items.IndexOf(SelectedItem)] = true;
                    }
                    break;

                case "Monday":
                    notAllIsFalse = MondayArr.Any(x => x);
                    if (notAllIsFalse)
                        Array.Clear(MondayArr, 0, MondayArr.Length);
                    foreach (ListBoxItem SelectedItem in schedualListBox.SelectedItems)
                    {
                        MondayArr[schedualListBox.Items.IndexOf(SelectedItem)] = true;
                    }
                    break;

                case "Tuesday":
                    notAllIsFalse = TuesdayArr.Any(x => x);
                    if (notAllIsFalse)
                        Array.Clear(TuesdayArr, 0, TuesdayArr.Length);
                    foreach (ListBoxItem SelectedItem in schedualListBox.SelectedItems)
                    {
                        TuesdayArr[schedualListBox.Items.IndexOf(SelectedItem)] = true;
                    }
                    break;

                case "Wednesday":
                    notAllIsFalse = WednesdayArr.Any(x => x);
                    if (notAllIsFalse)
                        Array.Clear(WednesdayArr, 0, WednesdayArr.Length);
                    foreach (ListBoxItem SelectedItem in schedualListBox.SelectedItems)
                    {
                        WednesdayArr[schedualListBox.Items.IndexOf(SelectedItem)] = true;
                    }
                    break;

                case "Thursday":
                    notAllIsFalse = ThursdayArr.Any(x => x);
                    if (notAllIsFalse)
                        Array.Clear(ThursdayArr, 0, ThursdayArr.Length);
                    foreach (ListBoxItem SelectedItem in schedualListBox.SelectedItems)
                    {
                        ThursdayArr[schedualListBox.Items.IndexOf(SelectedItem)] = true;
                    }
                    break;
            }
        }
        #endregion

        #region checks
        //id
        private void TesterIdTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.CheckId(TesterForPL.TesterId);
                if(bl.TesterInSystem(TesterForPL.TesterId))
                {
                    throw new Exception("ERROR. Tester exist in system");
                }
            }
            catch(Exception ex)
            {
                IdErrors.Text = ex.Message;
                IdErrors.Foreground = Brushes.Red;
                testerIdTextBox.BorderBrush = Brushes.Red;
            }
        }

        private void TesterIdTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            IdErrors.Text = "";
            IdErrors.Foreground = Brushes.Black;
            testerIdTextBox.BorderBrush = Brushes.Black;
        }

        //first name
        private void FirstNameTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            NameErrors.Text = "";
            NameErrors.Foreground = Brushes.Black;
            firstNameTextBox.BorderBrush = Brushes.Black;
        }

        //last name
        private void SirnameTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            SirNameErrors.Text = "";
            SirNameErrors.Foreground = Brushes.Black;
            sirnameTextBox.BorderBrush = Brushes.Black;
        }

        //phon number
        private void PhoneNumberTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            PhoneNumberErrors.Text = "";
            PhoneNumberErrors.Foreground = Brushes.Black;
            phoneNumberTextBox.BorderBrush = Brushes.Black;
        }

        //email
        private void EmailTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!bl.CheckEmail(TesterForPL.Email))
            {
                EmailErrors.Text = "ERROR. Invalid Email";
                EmailErrors.Foreground = Brushes.Red;
                emailTextBox.BorderBrush = Brushes.Red;
            }
        }

        private void EmailTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            EmailErrors.Text = "";
            EmailErrors.Foreground = Brushes.Black;
            emailTextBox.BorderBrush = Brushes.Black;
        }

        private void MaxDistanceForTestTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            DistanceError.Text = "";
            DistanceError.Foreground = Brushes.Black;
            maxDistanceForTestTextBox.BorderBrush = Brushes.Black;
        }

        private void YearsOfExperienceTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            ExperienceErrors.Text = "";
            ExperienceErrors.Foreground = Brushes.Black;
            yearsOfExperienceTextBox.BorderBrush = Brushes.Black;
        }

        private void MaxTestsInaWeekTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            MaxTestsError.Text = "";
            MaxTestsError.Foreground = Brushes.Black;
            maxTestsInaWeekTextBox.BorderBrush = Brushes.Black;
        }

        private void ScheduleGrid_GotFocus(object sender, RoutedEventArgs e)
        {
            ScheduleError.Text = "";
            ScheduleError.Foreground = Brushes.Black;
        }

        #region address
        private void City_OnLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            try
            {
                bl.IsText(City.Text);

                AddressErrors.Text = "";
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("ERROR"))
                {
                    AddressErrors.Foreground = Brushes.Red;
                    City.BorderBrush = Brushes.Red;

                }
                AddressErrors.Text = ex.Message;
            }
        }
        private void Street_OnLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            try
            {
                bl.IsText(Street.Text);
                AddressErrors.Text = "";
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("ERROR"))
                {
                    AddressErrors.Foreground = Brushes.Red;
                    Street.BorderBrush = Brushes.Red;

                }
                AddressErrors.Text = ex.Message;
            }
        }
        private void City_OnGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            City.BorderBrush = Brushes.Black;

        }
        private void Street_OnGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            Street.BorderBrush = Brushes.Black;
        }
        #endregion

        #endregion

    }
}
