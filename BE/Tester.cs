using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;

//by Neomi Mayer 328772801 and Beila Wellner 205823792
namespace BE
{
    public class Tester //can it be public?S
    {
        
       #region private variables
        private string _testerId="";
        private string _surname="";
        private string _firstName = "";
        private DateTime _dateOfBirth;
        private Gender _testerGender;
        private string _phoneNumber = "";
        [XmlIgnore]//to not be automatic by serialise
        public bool[,] _schedual = new bool[6, 5];

        private Address _testerAdress = new Address("","","");
        private int _yearsOfExperience = 0;
        private int _maxTestsInaWeek =0;

        private CarType _testercar;
        double _maxDistanceForTest = 0; //in kilometers
        //things I added
        private string _email ="";

        #endregion

        #region gets and sets

        public string TesterId
        {
            get { return _testerId; }
            set { _testerId = value; }
        }
        public string Surname { get => _surname; set => _surname = value; }
        public string FirstName { get => _firstName; set => _firstName = value; }
        public DateTime DateOfBirth { get => _dateOfBirth; set => _dateOfBirth = value; }
        public Gender TesterGender { get => _testerGender; set => _testerGender = value; } 
        public string PhoneNumber { get => _phoneNumber; set => _phoneNumber = value; }
        public int YearsOfExperience { get => _yearsOfExperience; set => _yearsOfExperience = value; }
        public int MaxTestsInaWeek { get => _maxTestsInaWeek; set => _maxTestsInaWeek = value; }
        public Address TesterAdress { get => _testerAdress; set => _testerAdress = value; }
        public double MaxDistanceForTest { get => _maxDistanceForTest; set => _maxDistanceForTest = value; }

        public CarType Testercar { get => _testercar; set => _testercar = value; }
        //public bool[,] Schedule { get; set; } = new bool[5, 6];
        public string Email { get => _email; set => _email = value; }

        public void setSchedual(bool[] SundayArr, bool[] MondayArr, bool[] TuesdayArr, bool[] WednesdayArr, bool[] ThursdayArr)
        {
            for(int i = 0; i < Configuration.NumOfWorkingDays; i++)
            {
                for(int j = 0; j < Configuration.NumOfHoursPerDay; j++)
                {
                    switch(i)
                    {
                        case 0:
                            _schedual[j, i] = SundayArr[j];
                            break;
                        case 1:
                            _schedual[j, i] = MondayArr[j];
                            break;
                        case 2:
                            _schedual[j, i] = TuesdayArr[j];
                            break;
                        case 3:
                            _schedual[j, i] = WednesdayArr[j];
                            break;
                        case 4:
                            _schedual[j, i] = ThursdayArr[j];
                            break;
                    }
                }
            }
        }
        public void getSchedual(bool[] SundayArr, bool[] MondayArr, bool[] TuesdayArr, bool[] WednesdayArr, bool[] ThursdayArr)
        {
            for(int i = 0; i < Configuration.NumOfWorkingDays; i++)
            {
                for (int j = 0; j < Configuration.NumOfHoursPerDay; j++)
                {
                    switch (i)
                    {
                        case 0:
                            SundayArr[j] = _schedual[j, i];
                            break;
                        case 1:
                            MondayArr[j] = _schedual[j, i];
                            break;
                        case 2:
                            TuesdayArr[j] = _schedual[j, i];
                            break;
                        case 3:
                            WednesdayArr[j] = _schedual[j, i];
                            break;
                        case 4:
                            ThursdayArr[j] = _schedual[j, i];
                            break;
                    }
                }
            }
        }
        #endregion

        public override string ToString()
        {
          //  format of tostring is : "property name: property value"
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
