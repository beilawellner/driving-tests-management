using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
//by Neomi Mayer 328772801 and Beila Wellner 205823792
namespace BE
{
    public class Trainee
    {
        #region private variables
        private string _traineeId;
        private string _surname;
        private string _firstName;
        private DateTime _dateOfBirth=DateTime.Now.AddYears(-1*(int)BE.Configuration.MinAgeOFTrainee);
        private Gender _traineeGender;
        private string _phoneNumber;
        private Address? _traineeAddress;
        private CarType _traineecar;
        private GearType _traineeGear;
        private string _drivingSchool;
        private string _drivingTeacher;
        private int _lessonsPassed=0;
        //things i added
        private string _email;



        #endregion

        #region gets and sets
        public string TraineeId
        {
            get { return _traineeId; }
            set { _traineeId = value; }
        }
        public string Sirname { get => _surname; set => _surname = value; }
        public string FirstName { get => _firstName; set => _firstName = value; }
        public DateTime DateOfBirth { get => _dateOfBirth; set => _dateOfBirth = value; }
        public Gender TraineeGender { get => _traineeGender; set => _traineeGender = value; }
        public string PhoneNumber { get => _phoneNumber; set => _phoneNumber = value; }
        public Address? TraineeAddress { get => _traineeAddress; set => _traineeAddress = value; }
        public GearType TraineeGear { get => _traineeGear; set => _traineeGear = value; }
        public string DrivingSchool { get => _drivingSchool; set => _drivingSchool = value; }
        public string DrivingTeacher { get => _drivingTeacher; set => _drivingTeacher = value; }
        public int LessonsPassed { get => _lessonsPassed; set => _lessonsPassed = value; }
        public CarType Traineecar { get => _traineecar; set => _traineecar = value; }
        public string Email { get => _email; set => _email = value; }
        #endregion

        public override string ToString()
        {
            //format of tostring is : "property name: property value" 
            PropertyInfo[] _PropertyInfos = this.GetType().GetProperties(); ;

            var sb = new StringBuilder();

            foreach (var info in _PropertyInfos)
            {
                var value = info.GetValue(this, null) ?? "(null)";
                //puts spaces between the property words
                StringBuilder builder = new StringBuilder();
                foreach (char c in info.Name)
                {
                    if (Char.IsUpper(c) && builder.Length > 0) builder.Append(' ');
                    builder.Append(c);
                }

                sb.AppendLine(builder.ToString() + ": " + value.ToString());
            }

            return sb.ToString();
        }
    }
}
