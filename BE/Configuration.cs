using System;
using System.Collections.Generic;
using System.Text;
//by Neomi Mayer 328772801 and Beila Wellner 205823792S

namespace BE
{
    public class Configuration
    {
        //the current running test
        public static int FirstTestId = 0;
        public static float MinAgeOFTester = 40;
        public static float MaxAgeOFTester = 70;
        public static float MinAgeOFTrainee = 17;
        public static float MaxAgeOFTrainee = 120;
        public static float MinAmmountOfLessons = 20;
        public static int MinHoursBetweenTests=1;
        public static int StartOfWorkDay = 9;
        public static int EndOfWorkDay = 15;
        public static int EndOfWorkWeek = 4;
        public static int NumOfWorkingDays = 5;
        public static int NumOfHoursPerDay = 6;
    }
}
