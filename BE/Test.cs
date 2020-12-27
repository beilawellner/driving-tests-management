using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
//by Neomi Mayer 328772801 and Beila Wellner 205823792
namespace BE
{
    public class Test
    {
        #region private variables
        private string _testId;
        private string _testerId; 
        private string _traineeId;
        private DateTime _testDate;
        private DateTime _dateAndHourofTest; 
        private CarType _carType;
        private Address _startingPoint;
        //things to check in test
        private bool _keptDistance;
        private bool _parking;
        private bool _reverseParking;
        private bool _checkMirrors;
        private bool _usedSignal;
        private bool _keptRightofPresidence; //zchoot kdima
        private bool _stoppedAtRed;
        private bool _stoppedAtcrossWalk;
        private bool _rightTurn;
        private bool _imediateStop;

        private bool _testPassed;
        private string _remarksOnTest; //hearot
        #endregion

        #region gets and sets
        public string TestId
        {
            get { return _testId; }
            set { _testId = value; }

        }

        public string TesterId
        {
            get { return _testerId; }
            set { _testerId = value; }
        }

        public string TraineeId { get => _traineeId; set => _traineeId = value; }
        public DateTime TestDate
        { get => _testDate; set => _testDate = value; }
        public DateTime DateAndHourOfTest { get => _dateAndHourofTest; set => _dateAndHourofTest = value; }
        public bool KeptDistance { get => _keptDistance; set => _keptDistance = value; }
        public bool Parking { get => _parking; set => _parking = value; }
        public bool ReverseParking { get => _reverseParking; set => _reverseParking = value; }
        public bool CheckMirrors { get => _checkMirrors; set => _checkMirrors = value; }
        public bool UsedSignal { get => _usedSignal; set => _usedSignal = value; }
        public bool KeptRightofPresidence { get => _keptRightofPresidence; set => _keptRightofPresidence = value; }
        public bool StoppedAtRed { get => _stoppedAtRed; set => _stoppedAtRed = value; }
        public bool StoppedAtcrossWalk { get => _stoppedAtcrossWalk; set => _stoppedAtcrossWalk = value; }
        public bool RightTurn { get => _rightTurn; set => _rightTurn = value; }
        public bool ImediateStop { get => _imediateStop; set => _imediateStop = value; }
        public Address StartingPoint { get => _startingPoint; set => _startingPoint = value; }
        public bool TestPassed { get => _testPassed; set => _testPassed = value; }
        public string RemarksOnTest { get => _remarksOnTest; set => _remarksOnTest = value; }
        public CarType CarType{ get => _carType; set => _carType = value; }

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
