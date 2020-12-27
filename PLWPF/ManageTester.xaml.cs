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
        //arrays for schedule
        bool[] SundayArr = new bool[6];
        bool[] MondayArr = new bool[6];
        bool[] TuesdayArr = new bool[6];
        bool[] WednesdayArr = new bool[6];
        bool[] ThursdayArr = new bool[6];

        public TestersWindow()
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            TesterGrid.Visibility = Visibility.Hidden;
            bl = IBL_imp.Instance;
            TesterForPL = new Tester();
            this.TesterGrid.DataContext = TesterForPL;
            this.TesterComboBox.DataContext = TesterListForPL;

            //manage calendar
            dateOfBirthDatePicker.SelectedDate = DateTime.Now.AddYears(-1 * (int)BE.Configuration.MaxAgeOFTester);
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
            if (bl.GetListOfTesters() == null)
            {
                UpdateTester.IsEnabled = false;
                DeleteTester.IsEnabled = false;
            }
            else if (bl.GetListOfTesters().Count == 0)
            {
                UpdateTester.IsEnabled = false;
                DeleteTester.IsEnabled = false;
            }
        }

        #region manage buttons
        private void BackToMainMenue_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AddTester_Click(object sender, RoutedEventArgs e)
        {
            winCondition = "Add";
            removeWarnings();
            //TesterComboBox.ItemsSource = bl.GetListOfTesters().Select(x => x.TesterId);
            TesterForPL = new Tester();
            openAll();
            TesterGrid.DataContext = TesterForPL;
            dateOfBirthDatePicker.SelectedDate = DateTime.Now.AddYears(-1 * (int)BE.Configuration.MaxAgeOFTester);
            dateOfBirthDatePicker.DisplayDateEnd = DateTime.Now.AddYears(-1 * (int)BE.Configuration.MinAgeOFTester);
            dateOfBirthDatePicker.DisplayDateStart = DateTime.Now.AddYears(-1 * (int)BE.Configuration.MaxAgeOFTester);
            IdErrors.Text = "";
            testerIdTextBox.Visibility = Visibility.Visible;
            TesterGrid.Visibility = Visibility.Visible;
            TesterComboBox.Visibility = Visibility.Hidden;
            TesterGrid.IsEnabled = true;
            Save.Content = "Check";
        }

        private void UpdateTester_Click(object sender, RoutedEventArgs e)
        {
            winCondition = "Update";
            removeWarnings();
            try
            {
                TesterForPL = new Tester();
                TesterComboBox.SelectedItem = null;
                closeAlmostAll();
                TesterGrid.DataContext = TesterForPL;
                IdErrors.Text = "First Select ID";
                IdErrors.Foreground = Brushes.DarkBlue;
                TesterListForPL = bl.GetListOfTesters();
                if (TesterListForPL ==null)
                    throw new Exception("There are no Testers to update.");
                if (TesterListForPL.Count == 0)
                    throw new Exception("There are no Testers to update.");
                dateOfBirthDatePicker.DisplayDateEnd = DateTime.Now.AddYears(-1 * (int)BE.Configuration.MinAgeOFTester);
                dateOfBirthDatePicker.DisplayDateStart = DateTime.Now.AddYears(-1 * (int)BE.Configuration.MaxAgeOFTester);
                TesterComboBox.ItemsSource = bl.GetListOfTesters().Select(x => x.TesterId);
                TesterGrid.Visibility = Visibility.Visible;
                TesterGrid.IsEnabled = true;                
                Save.Content = "Check";
                TesterComboBox.Visibility = Visibility.Visible;
                testerIdTextBox.Visibility = Visibility.Hidden;
            }
            catch (Exception exception)
            {
                if (exception.Message != "Object reference not set to an instance of an object.")
                    MessageBox.Show(exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteTester_Click(object sender, RoutedEventArgs e)
        {
            winCondition = "Delete";
            removeWarnings();
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
                if (TesterListForPL == null)
                    throw new Exception("There are no testers to delete.");
                if (TesterListForPL.Count == 0)
                    throw new Exception("There are no testers to delete.");
                TesterComboBox.ItemsSource = bl.GetListOfTesters().Select(x => x.TesterId);
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
                    TesterForPL.TesterAdress = new Address(Street.Text, BuidingNumber.Text, City.Text);
                    TesterForPL.setSchedual(SundayArr, MondayArr, TuesdayArr, WednesdayArr, ThursdayArr);
                    bl.AddTester(TesterForPL);
                    TesterGrid.Visibility = Visibility.Hidden;
                    MessageBox.Show("Tester saved successfully", "", MessageBoxButton.OK, MessageBoxImage.Information);
                    UpdateTester.IsEnabled = true;
                    DeleteTester.IsEnabled = true;
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    TesterGrid.Visibility = Visibility.Visible;
                }
            }

            if (Save.Content == "Update")
            {
                TesterForPL.TesterAdress = new Address(Street.Text, BuidingNumber.Text, City.Text);
                TesterForPL.setSchedual(SundayArr, MondayArr, TuesdayArr, WednesdayArr, ThursdayArr);
                bl.UpdateTester(TesterForPL);
                TesterForPL = new Tester();
                TesterComboBox.SelectedItem = null;
                this.TesterGrid.DataContext = TesterForPL;
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
                    if (bl.GetListOfTesters() == null)
                    {
                        UpdateTester.IsEnabled = false;
                        DeleteTester.IsEnabled = false;
                    }
                    else if (bl.GetListOfTesters().Count == 0)
                    {
                        UpdateTester.IsEnabled = false;
                        DeleteTester.IsEnabled = false;
                    }
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
                isWarnings();//empty filed
                switch (winCondition)
                {
                    case "Add":
                        if(noErrors())
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
                        else
                        {
                            MessageBox.Show("Can't add Tester. Fill ID " +
                                            "and fix errors.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        break;

                    case "Update":
                        if(noErrors()) Save.Content = "Update";
                        break;

                    case "Delete":
                        if(noErrors()) Save.Content = "Delete";
                        break;
                }

               
            }
        }
        #endregion

        #region additional functions
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

        public void isWarnings()
        {
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
            if (string.IsNullOrWhiteSpace(City.Text))
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
            if (string.IsNullOrWhiteSpace(BuidingNumber.Text))
            {
                AddressErrors.Text = "Warning. Filed is empty";
                AddressErrors.Foreground = Brushes.Orange;
                BuidingNumber.BorderBrush = Brushes.Orange;
            }
            if (!SundayArr.Any(x => x) && !MondayArr.Any(x => x) && !TuesdayArr.Any(x => x) && !WednesdayArr.Any(x => x) && !ThursdayArr.Any(x => x))//if schedule is empty
            {
                ScheduleError.Text = "Warning, No times selected";
                ScheduleError.Foreground = Brushes.Orange;
            }
        }

        public void removeWarnings()
        {
            IdErrors.Text = "";
            NameErrors.Text = "";
            SirNameErrors.Text = "";
            GenderError.Text = "";
            PhoneNumberErrors.Text = "";
            EmailErrors.Text = "";
            CarError.Text = "";
            DistanceError.Text = "";
            ExperienceErrors.Text = "";
            MaxTestsError.Text = "";
            AddressErrors.Text = "";
            ScheduleError.Text = "";
            testerIdTextBox.BorderBrush = Brushes.Gray;
            firstNameTextBox.BorderBrush = Brushes.Gray;
            sirnameTextBox.BorderBrush = Brushes.Gray;
            phoneNumberTextBox.BorderBrush = Brushes.Gray;
            emailTextBox.BorderBrush = Brushes.Gray;
            maxTestsInaWeekTextBox.BorderBrush = Brushes.Gray;
            yearsOfExperienceTextBox.BorderBrush = Brushes.Gray;
            maxDistanceForTestTextBox.BorderBrush = Brushes.Gray;
            City.BorderBrush = Brushes.Gray;
            Street.BorderBrush = Brushes.Gray;
            BuidingNumber.BorderBrush = Brushes.Gray;
        }

        //for numbers only
        private void TextBox_PreviewTextInputNumbers(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("^[a-zA-Z]+$").IsMatch(e.Text);
        }

        //for letters only
        private void TextBox_PreviewTextInputLetters(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^A-z]+").IsMatch(e.Text);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            System.Windows.Data.CollectionViewSource testerViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("testerViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // testerViewSource.Source = [generic data source]
        }
        #endregion

        #region Schedual

        private void SchedualListBox_MouseLeave(object sender, MouseEventArgs e)
        {
            try
            {
                //save the user choose in arr
                switch (dayLabel.Content)
                {
                    case "Sunday":
                        setHoursInArr(SundayArr);
                        break;

                    case "Monday":
                        setHoursInArr(MondayArr);
                        break;

                    case "Tuesday":
                        setHoursInArr(TuesdayArr);
                        break;

                    case "Wednesday":
                        setHoursInArr(WednesdayArr);
                        break;

                    case "Thursday":
                        setHoursInArr(ThursdayArr);
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("There is some problem", "Oops", MessageBoxButton.OK, MessageBoxImage.Hand);
            }

        }

        private void NextDayButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                switch (dayLabel.Content)
                {
                    case "Sunday":
                        dayLabel.Content = "Monday";//change the label
                        showDailyHours(MondayArr);//show previes selections
                        break;

                    case "Monday":
                        dayLabel.Content = "Tuesday";
                        showDailyHours(TuesdayArr);
                        break;

                    case "Tuesday":
                        dayLabel.Content = "Wednesday";
                        showDailyHours(WednesdayArr);
                        break;

                    case "Wednesday":
                        dayLabel.Content = "Thursday";
                        showDailyHours(ThursdayArr);
                        break;

                    case "Thursday":
                        dayLabel.Content = "Sunday";
                        showDailyHours(SundayArr);
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("There is some problem", "Oops" ,MessageBoxButton.OK, MessageBoxImage.Hand);
            }            
        }

        private void PreviousDayButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                switch (dayLabel.Content)
                {
                    case "Sunday":
                        dayLabel.Content = "Thursday";//change the label
                        showDailyHours(ThursdayArr);
                        break;

                    case "Monday":
                        dayLabel.Content = "Sunday";
                        showDailyHours(SundayArr);
                        break;

                    case "Tuesday":
                        dayLabel.Content = "Monday";
                        showDailyHours(MondayArr);
                        break;

                    case "Wednesday":
                        dayLabel.Content = "Tuesday";
                        showDailyHours(TuesdayArr);
                        break;

                    case "Thursday":
                        dayLabel.Content = "Wednesday";
                        showDailyHours(WednesdayArr);
                        break;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("There is some problem", "Oops", MessageBoxButton.OK, MessageBoxImage.Hand);
            }            
        }

        public void showDailyHours(bool[] dayArr)
        {
            if (dayArr[0]) item0.IsSelected = true;
            else item0.IsSelected = false;
            if (dayArr[1]) item1.IsSelected = true;
            else item1.IsSelected = false;
            if (dayArr[2]) item2.IsSelected = true;
            else item2.IsSelected = false;
            if (dayArr[3]) item3.IsSelected = true;
            else item3.IsSelected = false;
            if (dayArr[4]) item4.IsSelected = true;
            else item4.IsSelected = false;
            if (dayArr[5]) item5.IsSelected = true;
            else item5.IsSelected = false;
        }

        public void setHoursInArr(bool[] dayArr)
        {
            if (item0.IsSelected) dayArr[0] = true;
            else dayArr[0] = false;
            if (item1.IsSelected) dayArr[1] = true;
            else dayArr[1] = false;
            if (item2.IsSelected) dayArr[2] = true;
            else dayArr[2] = false;
            if (item3.IsSelected) dayArr[3] = true;
            else dayArr[3] = false;
            if (item4.IsSelected) dayArr[4] = true;
            else dayArr[4] = false;
            if (item5.IsSelected) dayArr[5] = true;
            else dayArr[5] = false;
        }
        #endregion

        #region checks

        #region id
        private void TesterIdTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.CheckId(TesterForPL.TesterId);
                if(bl.TesterInSystem(TesterForPL.TesterId))
                {
                    throw new Exception("ERROR. Tester exist in system");
                }
                //hoursFromSchedualArr = TesterForPL.getSchedual();
                //showTesterTime(0);
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

        private void TesterComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TesterComboBox.SelectedIndex != -1)
            {
                IdErrors.Text = "";
                if (Save.Content == "Check")
                {
                    openAll();
                }
                string id = (string)TesterComboBox.SelectedItem;
                TesterForPL = bl.GetListOfTesters().FirstOrDefault(a => a.TesterId == id);
                this.TesterGrid.DataContext = TesterForPL;

                testerGenderComboBox.SelectedItem = TesterForPL.TesterGender;
                if (TesterForPL.TesterGender == Gender.Male)
                    testerGenderComboBox.SelectedIndex = 0;
                else if (TesterForPL.TesterGender == Gender.Female)
                    testerGenderComboBox.SelectedIndex = 1;
                else testerGenderComboBox.SelectedIndex = 2;
                testercarComboBox.SelectedItem = TesterForPL.Testercar;
                City.Text = TesterForPL.TesterAdress.City;
                Street.Text = TesterForPL.TesterAdress.Street;
                BuidingNumber.Text = TesterForPL.TesterAdress.BuildingNumber;

                //schedual
                TesterForPL.getSchedual(SundayArr, MondayArr, TuesdayArr, WednesdayArr, ThursdayArr);
                showDailyHours(SundayArr);//working hours in sunday

//this.TesterGrid.DataContext = TesterForPL;
               }
            }
        #endregion

        #region names
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
        #endregion

        #region others
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
        #endregion

        #region address
        private void City_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            City.BorderBrush = Brushes.Black;
        }

        private void Street_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            Street.BorderBrush = Brushes.Black;
        }

        private void BuidingNumber_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            BuidingNumber.BorderBrush = Brushes.Black;
        }
        #endregion

        #region combobox

             private void GenderComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
                {
            if (testerGenderComboBox.SelectedIndex != -1)
                TesterForPL.TesterGender = (Gender)Enum.Parse(typeof(Gender), testerGenderComboBox.SelectedItem.ToString());
        }

        private void TestercarComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (testercarComboBox.SelectedIndex != -1)
                TesterForPL.Testercar = (CarType)Enum.Parse(typeof(CarType), testercarComboBox.SelectedItem.ToString());
        }

       

        #endregion

        #endregion


    }
}
