using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BE;
using BL;
using MessageBox = System.Windows.MessageBox;
using MouseEventArgs = System.Windows.Input.MouseEventArgs;

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for ManageTrainee.xaml
    /// </summary>
    public partial class ManageTrainee : Window
    {
        private BL.IBL bl;
        private BE.Trainee TraineeForPL;
        private List<Trainee> TraineeListForPL;
        public ManageTrainee()
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            TraineeGrid.Visibility = Visibility.Hidden;
            bl = IBL_imp.Instance;
            TraineeForPL = new Trainee();
            //City.DataContext = TraineeForPL.TraineeAddress.City;
            //Street.DataContext = TraineeForPL.TraineeAddress.Street;
            //BuidingNumber.DataContext = TraineeForPL.TraineeAddress.BuildingNumber;
            this.TraineeGrid.DataContext = TraineeForPL;
            this.TraineeComboBox.DataContext = TraineeListForPL;
            this.traineeGenderComboBox.ItemsSource = Enum.GetValues(typeof(BE.Gender));
            this.traineeGearComboBox.ItemsSource = Enum.GetValues(typeof(BE.GearType));
            this.traineecarComboBox.ItemsSource = Enum.GetValues(typeof(BE.CarType));
            dateOfBirthDatePicker.DisplayDateEnd = DateTime.Now.AddYears(-1*(int)BE.Configuration.MinAgeOFTrainee);
            dateOfBirthDatePicker.DisplayDateStart = DateTime.Now.AddYears(-1 * (int)BE.Configuration.MaxAgeOFTrainee);
            TraineeComboBox.Visibility = Visibility.Hidden;
            if (bl.GetListOfTrainees() == null)
            {
                UpdateTrainee.IsEnabled = false;
                DeleteTrainee.IsEnabled = false;
            }
            

        }

        private void BackToMainMenue_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        #region manage buttons
        private void AddTrainee_Click(object sender, RoutedEventArgs e)
        {
            removewarnings();
           // TraineeComboBox.ItemsSource = bl.GetListOfTrainees().Select(x => x.TraineeId);
            TraineeForPL =new Trainee();
            openAll();
            TraineeGrid.DataContext = TraineeForPL;
            IdErrors.Text = "";
            traineeIdTextBox.Visibility = Visibility.Visible;
            TraineeGrid.Visibility = Visibility.Visible;
            TraineeComboBox.Visibility = Visibility.Hidden;
            TraineeGrid.IsEnabled = true;
            Save.Content = "Check";
            Save.IsEnabled = true;
        }

        private void UpdateTrainee_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                removewarnings();
                TraineeForPL = new Trainee();
                Save.IsEnabled = false;
                closeAlmostAll();
                TraineeGrid.DataContext = TraineeForPL;
                IdErrors.Text = "First Select ID";
                IdErrors.Foreground=Brushes.DarkBlue;
                TraineeListForPL = bl.GetListOfTrainees();
                if ( TraineeListForPL == null)
                    throw new Exception("There are no trainees to update.");
                if (TraineeListForPL.Count==0)
                    throw new Exception("There are no trainees to update.");
                TraineeComboBox.ItemsSource = bl.GetListOfTrainees().Select(x=>x.TraineeId);
                TraineeGrid.Visibility = Visibility.Visible;
                TraineeGrid.IsEnabled = true;
               
                Save.Content = "Check";
                TraineeComboBox.Visibility = Visibility.Visible;
                traineeIdTextBox.Visibility = Visibility.Hidden;
                TraineeComboBox.SelectedItem = null;

            }
            catch (Exception exception)
            {
                if(exception.Message!= "Object reference not set to an instance of an object.")
                MessageBox.Show(exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteTrainee_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                removewarnings();
                Save.Content = "Delete";
                TraineeForPL = new Trainee();
                TraineeComboBox.Visibility = Visibility.Visible;
                traineeIdTextBox.Visibility = Visibility.Hidden;
                TraineeComboBox.SelectedItem = null;
                TraineeGrid.DataContext = TraineeForPL;
                closeAlmostAll();
                Save.IsEnabled = false;
                IdErrors.Text = "First Select ID";
                TraineeListForPL = bl.GetListOfTrainees();
                if (TraineeListForPL == null)
                    throw new Exception("There are no trainees to delete.");
                if (TraineeListForPL.Count == 0)
                    throw new Exception("There are no trainees to delete.");
                TraineeGrid.Visibility = Visibility.Visible;
                TraineeComboBox.ItemsSource = bl.GetListOfTrainees().Select(x => x.TraineeId);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
        #endregion
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            System.Windows.Data.CollectionViewSource traineeViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("traineeViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // traineeViewSource.Source = [generic data source]
           
        }


        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (Save.Content == "Add")
            {
                try
                {
                    TraineeForPL.TraineeAddress = new Address(Street.Text, BuidingNumber.Text, City.Text);
                    bl.AddTrainee(TraineeForPL);
                    //TraineeForPL = new Trainee();
                    TraineeGrid.Visibility = Visibility.Hidden;
                    //this.TraineeGrid.DataContext = TraineeForPL;
                    MessageBox.Show("Trainee saved successfully", "", MessageBoxButton.OK, MessageBoxImage.Information);
                    UpdateTrainee.IsEnabled = true;
                    DeleteTrainee.IsEnabled = true;
                }

                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    TraineeGrid.Visibility = Visibility.Visible;
                }

            }
            if (Save.Content == "Update")
            {
                TraineeForPL.TraineeAddress = new Address(Street.Text, BuidingNumber.Text, City.Text);
                
                bl.UpdateTrainee(TraineeForPL);
                TraineeForPL = new Trainee();
                TraineeGrid.Visibility = Visibility.Hidden;
                TraineeComboBox.SelectedItem = null;
                this.TraineeGrid.DataContext = TraineeForPL;
                MessageBox.Show("Trainee saved successfully", "", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            if (Save.Content == "Delete")
            {
                MessageBoxResult dialogResult = MessageBox.Show("Are you sure you want to delete?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                if (dialogResult == MessageBoxResult.Yes)
                {
                    bl.DeleteTrainee(TraineeForPL);
                    TraineeGrid.Visibility = Visibility.Hidden;
                    MessageBox.Show("Trainee successfully deleted.", "", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                    traineeIdTextBox.Visibility = Visibility.Visible;
                    TraineeForPL = new Trainee();
                    TraineeGrid.DataContext = TraineeForPL;
                    if (bl.GetListOfTrainees() == null)
                    {
                        UpdateTrainee.IsEnabled = false;
                        DeleteTrainee.IsEnabled = false;
                    }
                }
                else if (dialogResult == MessageBoxResult.No)
                {
                    traineeIdTextBox.Visibility = Visibility.Visible;
                    TraineeForPL = new Trainee();
                    TraineeGrid.DataContext = TraineeForPL;
                    TraineeGrid.Visibility = Visibility.Hidden;
                    MessageBox.Show("Trainee not deleted.", "", MessageBoxButton.OK, MessageBoxImage.Asterisk);

                }
            }

            if (Save.Content == "Check")
            {

                if (noErrors() && TraineeComboBox.Visibility == Visibility.Hidden)
                {
                    if (bl.TraineeInSystem(TraineeForPL.TraineeId))
                    {
                        MessageBoxResult dialogResult = MessageBox.Show("Trainee alredy exists in the system! Do you want to update?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                        if (dialogResult == MessageBoxResult.Yes)
                        {
                            TraineeComboBox.Visibility = Visibility.Visible;
                            traineeIdTextBox.Visibility = Visibility.Hidden;
                            TraineeComboBox.SelectedValue = (object)TraineeForPL.TraineeId;
                            TraineeForPL = bl.GetListOfTrainees()
                                .FirstOrDefault(x => x.TraineeId == traineeIdTextBox.Text);
                        }
                        else if (dialogResult == MessageBoxResult.No)
                        {
                            TraineeForPL=new Trainee();
                            TraineeGrid.DataContext = TraineeForPL;
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
                    MessageBox.Show("Can't add Trainee. Fill ID " +
                                    "and fix errors.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

           

        }

        #region id checks
        private void TraineeIdTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            throw new NotImplementedException();
        }
        private void TraineeComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TraineeComboBox.SelectedIndex != -1)
            {
                IdErrors.Text = "";
                if (Save.Content == "Check")
                {
                    openAll();

                }

                Save.IsEnabled = true;
                string id = (string) TraineeComboBox.SelectedItem;
                TraineeForPL = bl.GetListOfTrainees().FirstOrDefault(a => a.TraineeId == id);
                this.TraineeGrid.DataContext = TraineeForPL;

                traineeGearComboBox.SelectedItem = TraineeForPL.TraineeGear;
                traineeGenderComboBox.SelectedItem = TraineeForPL.TraineeGender;
                traineecarComboBox.SelectedItem = TraineeForPL.Traineecar;
                City.Text = TraineeForPL.TraineeAddress.Value.City;
                Street.Text = TraineeForPL.TraineeAddress.Value.Street;
                BuidingNumber.Text = TraineeForPL.TraineeAddress.Value.BuildingNumber;
            }
        }
        private void TraineeIdTextBox_OnLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            try
            {
                bl.CheckId(TraineeForPL.TraineeId);
                //make a check if in system- make a messege box option for update/delete then "select" the combox option.
                if (bl.TraineeInSystem(TraineeForPL.TraineeId))
                {
                    MessageBoxResult dialogResult =
                        MessageBox.Show("Trainee alredy exists in the system! Do you want to update?", "Warning",
                            MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                    if (dialogResult == MessageBoxResult.Yes)
                    {
                        TraineeComboBox.Visibility = Visibility.Visible;
                        traineeIdTextBox.Visibility = Visibility.Hidden;
                        TraineeComboBox.SelectedValue = (object) TraineeForPL.TraineeId;
                        TraineeForPL = bl.GetListOfTrainees()
                            .FirstOrDefault(x => x.TraineeId == traineeIdTextBox.Text);
                    }
                    else if (dialogResult == MessageBoxResult.No)
                    {
                        TraineeForPL = new Trainee();
                        TraineeGrid.DataContext = TraineeForPL;
                    }

                }
            }
            catch (Exception ex)
            {

                IdErrors.Text = ex.Message;
                IdErrors.Foreground = Brushes.Red;
                traineeIdTextBox.BorderBrush = Brushes.Red;
            }

        }

        private void TraineeIdTextBox_OnGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            IdErrors.Text = "";
            IdErrors.Foreground = Brushes.Black;
            traineeIdTextBox.BorderBrush = Brushes.Black;
        }

        #endregion
        #region Name checks
        private void FirstNameTextBox_OnLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            try
            {
                bl.IsText(TraineeForPL.FirstName);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("ERROR"))
                {
                    NameErrors.Foreground = Brushes.Red;
                    firstNameTextBox.BorderBrush = Brushes.Red;

                }
                else
                {
                    NameErrors.Foreground = Brushes.Orange;
                    firstNameTextBox.BorderBrush = Brushes.Orange;
                }
                NameErrors.Text = ex.Message;

            }
        }

        private void FirstNameTextBox_OnGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            NameErrors.Text = "";
            NameErrors.Foreground = Brushes.Black;
            firstNameTextBox.BorderBrush = Brushes.Black;
        }

        private void SirnameTextBox_OnLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            try
            {
                bl.IsText(TraineeForPL.Sirname);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("ERROR"))
                {
                    SirNameErrors.Foreground = Brushes.Red;
                    sirnameTextBox.BorderBrush = Brushes.Red;

                }
                else
                {
                    SirNameErrors.Foreground = Brushes.Orange;
                    sirnameTextBox.BorderBrush = Brushes.Orange;
                }

                SirNameErrors.Text = ex.Message;

            }
        }

        private void SirnameTextBox_OnGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            SirNameErrors.Text = "";
            SirNameErrors.Foreground = Brushes.Black;
            sirnameTextBox.BorderBrush = Brushes.Black;
        }
        #endregion
        #region phone Number

        private void PhoneNumberTextBox_OnLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            try
            {
                bl.IsNumber(TraineeForPL.PhoneNumber);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("ERROR"))
                {
                    PhoneNumberErrors.Foreground = Brushes.Red;
                    phoneNumberTextBox.BorderBrush = Brushes.Red;
                }
                else
                {
                    PhoneNumberErrors.Foreground = Brushes.Orange;
                    phoneNumberTextBox.BorderBrush = Brushes.Orange;
                }

                PhoneNumberErrors.Text = ex.Message;

            }
        }

        private void PhoneNumberTextBox_OnGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            PhoneNumberErrors.Text = "";
            PhoneNumberErrors.Foreground = Brushes.Black;
            phoneNumberTextBox.BorderBrush = Brushes.Black;
        }
        #endregion

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
                else
                {
                    AddressErrors.Foreground = Brushes.Orange;
                    City.BorderBrush = Brushes.Orange;
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
                else
                {
                    AddressErrors.Foreground = Brushes.Orange;
                    Street.BorderBrush = Brushes.Orange;
                }
                AddressErrors.Text = ex.Message;

            }
        }
        private void City_OnGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            City.BorderBrush=Brushes.Black;
            
        }
        private void Street_OnGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
         Street.BorderBrush = Brushes.Black;
            
        }
        #endregion
        #region driving teacher and driving school

        private void DrivingSchoolTextBox_OnLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            try
            {
                bl.IsText(TraineeForPL.DrivingSchool);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("ERROR"))
                {
                    DrivingSchoolErrors.Foreground = Brushes.Red;
                    drivingSchoolTextBox.BorderBrush = Brushes.Red;

                }
                else
                {
                    DrivingSchoolErrors.Foreground = Brushes.Orange;
                    drivingSchoolTextBox.BorderBrush = Brushes.Orange;
                }
                DrivingSchoolErrors.Text = ex.Message;

            }
        }
        private void DrivingTeacherTextBox_OnLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            try
            {
                
                //DrivingTeacherErrors.Text = "";
                //drivingTeacherTextBox.BorderBrush = Brushes.Black;
              //  bl.CheckId(TraineeForPL.DrivingTeacher);
                
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("ERROR. Field is empty."))
                {
                    DrivingTeacherErrors.Foreground = Brushes.Orange;
                    drivingTeacherTextBox.BorderBrush = Brushes.Orange;
                    DrivingTeacherErrors.Text = "Warning. Field is empty.";
                }

                else if(ex.Message.Contains("ERROR"))
                {
                    DrivingTeacherErrors.Foreground = Brushes.Red;
                    drivingTeacherTextBox.BorderBrush = Brushes.Red;
                    DrivingTeacherErrors.Text = ex.Message;

                }

            }
        }

        #endregion

        #region email

        private void EmailTextBox_OnLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            try
            {
                if(TraineeForPL.Email==null|| TraineeForPL.Email=="")
                    throw new Exception("Waring. Empty Field");
                if(!bl.CheckEmail(TraineeForPL.Email))
                    throw new Exception("ERROR. Invalid Email");

            }
            catch (Exception ex)
            {
                if(ex.Message.Contains("ERROR"))
                {
                    EmailErrors.Foreground = Brushes.Red;
                    emailTextBox.BorderBrush = Brushes.Red;
                }
                else
                {
                    EmailErrors.Foreground = Brushes.Orange;
                    emailTextBox.BorderBrush = Brushes.Orange;
                }

                EmailErrors.Text = ex.Message;

            }

        }

        private void EmailTextBox_OnGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            EmailErrors.Text = "";
            EmailErrors.Foreground = Brushes.Black;
            emailTextBox.BorderBrush = Brushes.Black;
        }
        #endregion

        #region lessson passed buttons

        private void Plus_On_Click(object sender, RoutedEventArgs e)
        {
            TraineeForPL.LessonsPassed++;
            lessonsPassedTextBox.Text = "" + TraineeForPL.LessonsPassed;

        }

        private void Minus_Click(object sender, RoutedEventArgs e)
        {
            if(TraineeForPL.LessonsPassed>=1)
            TraineeForPL.LessonsPassed--;
            lessonsPassedTextBox.Text = "" + TraineeForPL.LessonsPassed;
        }
        #endregion

        #region comboboxes
        private void TraineeGenderComboBox_OnMouseLeave(object sender, MouseEventArgs e)
        {
            if(traineeGenderComboBox.SelectedItem == null)
            {
                GenderErrors.Text = "Warning. Field is empty.";
                GenderErrors.Foreground = Brushes.Orange;
                traineeGenderComboBox.BorderBrush = Brushes.Orange;

            }

            else
            {

                GenderErrors.Text = "";
                GenderErrors.Foreground = Brushes.Black;
                traineeGenderComboBox.BorderBrush = Brushes.Black;
            }

        }
        private void TraineeGenderComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(traineeGenderComboBox.SelectedIndex!=-1)
            TraineeForPL.TraineeGender = (Gender)Enum.Parse(typeof(Gender), traineeGenderComboBox.SelectedItem.ToString()); 
        }

        private void TraineecarComboBox_OnMouseLeave(object sender, MouseEventArgs e)
        {
            if (traineecarComboBox.SelectedItem == null)
            {
                CarTypeErrors.Text = "Warning. Field is empty.";
                CarTypeErrors.Foreground = Brushes.Orange;
                traineecarComboBox.BorderBrush = Brushes.Orange;

            }

            else
            {

                CarTypeErrors.Text = "";
                CarTypeErrors.Foreground = Brushes.Black;
                traineecarComboBox.BorderBrush = Brushes.Black;
            }
        }

        private void TraineecarComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(traineecarComboBox.SelectedIndex!=-1)
            TraineeForPL.Traineecar = (CarType)Enum.Parse(typeof(CarType), traineecarComboBox.SelectedItem.ToString());
        }
        private void TraineeGearComboBox_OnMouseLeave(object sender, MouseEventArgs e)
        {
            if (traineeGearComboBox.SelectedItem == null)
            {
                GearTypeErrors.Text = "Warning. Field is empty.";
                GearTypeErrors.Foreground = Brushes.Orange;
                traineeGearComboBox.BorderBrush = Brushes.Orange;

            }

            else
            {

                GearTypeErrors.Text = "";
                GearTypeErrors.Foreground = Brushes.Black;
                traineeGearComboBox.BorderBrush = Brushes.Black;
            }
        }

        private void TraineeGearComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(traineeGearComboBox.SelectedIndex!=-1)
            TraineeForPL.TraineeGear = (GearType)Enum.Parse(typeof(GearType), traineeGearComboBox.SelectedItem.ToString());
        }

        #endregion

        #region opens and closes
        public bool noErrors()
        {

            try
            {
                if(TraineeForPL.TraineeId == null)
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
                if (DrivingSchoolErrors.Text.Contains("ERROR"))
                    throw new Exception();
                if (DrivingTeacherErrors.Text.Contains("ERROR"))
                    throw new Exception();
                if(AddressErrors.Text.Contains("ERROR"))
                    throw new Exception();
                return true;
            }
            catch (Exception exception)
            {
                return false;
            }

            
        }

        public void closeAlmostAll()
        {
            traineeIdTextBox.IsEnabled = false;
            firstNameTextBox.IsEnabled = false;
            sirnameTextBox.IsEnabled = false;
            dateOfBirthDatePicker.IsEnabled = false;
            traineeGenderComboBox.IsEnabled = false;
            phoneNumberTextBox.IsEnabled = false;
            emailTextBox.IsEnabled = false;
            drivingSchoolTextBox.IsEnabled = false;
            drivingTeacherTextBox.IsEnabled = false;
            traineecarComboBox.IsEnabled = false;
            traineeGearComboBox.IsEnabled = false;
            City.IsEnabled = false;
            Street.IsEnabled = false;
            BuidingNumber.IsEnabled = false;
            plus.IsEnabled = false;
            minus.IsEnabled = false;
        }

        public void openAll()
        {
            traineeIdTextBox.IsEnabled = true;
            firstNameTextBox.IsEnabled = true;
            sirnameTextBox.IsEnabled = true;
            dateOfBirthDatePicker.IsEnabled = true;
            traineeGenderComboBox.IsEnabled = true; 
            phoneNumberTextBox.IsEnabled = true;
            emailTextBox.IsEnabled = true; 
            drivingSchoolTextBox.IsEnabled = true; 
            drivingTeacherTextBox.IsEnabled = true; 
            traineecarComboBox.IsEnabled = true; 
            traineeGearComboBox.IsEnabled = true; 
            City.IsEnabled = true; 
            Street.IsEnabled = true; 
            BuidingNumber.IsEnabled = true; 
            plus.IsEnabled = true; 
            minus.IsEnabled = true; 
        }

        public void removewarnings()
        {
            IdErrors.Text = "";
            NameErrors.Text = "";
            SirNameErrors.Text = "";
            PhoneNumberErrors.Text = "";
            EmailErrors.Text = "";
            DateErrors.Text = "";
            GenderErrors.Text = "";
            DrivingSchoolErrors.Text = "";
            DrivingTeacherErrors.Text = "";
            CarTypeErrors.Text = "";
            GearTypeErrors.Text = "";
            AddressErrors.Text = "";


        }

        #endregion

    }
}
