//using System;
//using System.Collections.Generic;
//using BE;
////by Neomi Mayer 328772801 and Beila Wellner 205823792
//namespace DS
//{

//    public static class DataSource
//    {
//        public static List<Tester> _testerList=new List<Tester>();
//        public static List<Trainee> _traineeList=new List<Trainee>();
//        public static List<Test> _testList=new List<Test>();

//        static  DataSource()
//        {
//            #region two tests

//            Test test = new Test();
//            test.TestId = "123";
//            test.CheckMirrors = true;
//            DateTime time = DateTime.Today;
//            test.DateAndHourOfTest = DateTime.Today;
//            test.ImediateStop = true;
//            test.KeptDistance = true;
//            test.KeptRightofPresidence = true;
//            test.Parking = true;
//            test.ReverseParking = true;
//            test.RightTurn = true;
//            test.StoppedAtRed = true;
//            test.StoppedAtcrossWalk = true;
//            test.TestDate = time;
//            test.TesterId = "328772801";
//            test.TraineeId = "328772801";
//            test.TestPassed = true;

//            Test test2 = new Test();
//            test2.TestId = "456";
//            test2.CheckMirrors = true;
//            DateTime time2 = DateTime.Today.AddDays(5);
//            test2.DateAndHourOfTest = DateTime.Today.AddDays(5);
//            test2.ImediateStop = true;
//            test2.KeptDistance = true;
//            test2.KeptRightofPresidence = true;
//            test2.Parking = true;
//            test2.ReverseParking = true;
//            test2.RightTurn = true;
//            test2.StoppedAtRed = true;
//            test2.StoppedAtcrossWalk = true;
//            test2.TestDate = time;
//            test2.TesterId = "319185997";
//            test2.TraineeId = "319185997";
//            test2.TestPassed = false;

//            Test test3 = new Test();
//            test3.TestId = "789";
//            test3.TestDate = DateTime.Today.AddDays(-5);
//            test3.CheckMirrors = true;
//            test3.DateAndHourOfTest = DateTime.Today.AddDays(-5);
//            test3.ImediateStop = true;
//            test3.KeptDistance = true;
//            test3.KeptRightofPresidence = true;
//            test3.Parking = true;
//            test3.ReverseParking = true;
//            test3.RightTurn = true;
//            test3.StoppedAtRed = true;
//            test3.StoppedAtcrossWalk = true;
//            test3.TestDate = time;
//            test3.TesterId = "205823792";
//            test3.TraineeId = "205823792";
//            test3.TestPassed = true;

//            _testList.Add(test);
//            _testList.Add(test2);
//            _testList.Add(test3);

//            #endregion

//            #region three testers

//            Tester tester = new Tester();
//            tester.DateOfBirth = time;
//            tester.FirstName = "Neomi";
//            tester.TesterId = "328772801";
//            tester.Testercar = CarType.Private;
//            //tester._schedual[0, 0] = true;
//            //tester._schedual[1, 0] = true;
//            //tester._schedual[2, 0] = true;
//            //tester._schedual[3, 0] = true;
//            //tester._schedual[4, 1] = true;

//            Tester tester1 = new Tester();
//            tester1.DateOfBirth = time;
//            tester1.FirstName = "Beila";
//            tester1.TesterId = "205823792";
//            tester1.PhoneNumber = "0546212193";
//            tester1.Testercar = CarType.TwoWheel;
//            tester1._schedual[0, 0] = true;
//            tester1._schedual[1, 0] = true;
//            tester1._schedual[2, 0] = true;
//            tester1._schedual[3, 0] = true;
//            tester1._schedual[4, 1] = true;

//            Tester tester2 = new Tester();
//            tester2.DateOfBirth = time;
//            tester2.FirstName = "Elisha";
//            tester2.TesterId = "319185997";
//            tester2.Testercar = CarType.Private;
//            //tester2._schedual[0, 0] = true;
//            //tester2._schedual[1, 0] = true;
//            //tester2._schedual[2, 0] = true;
//            //tester2._schedual[3, 0] = true;
//            //tester2._schedual[4, 1] = true;

//            _testerList.Add(tester);
//            _testerList.Add(tester1);
//            _testerList.Add(tester2);
//            #endregion

//            #region three trainees

//            Trainee trainee = new Trainee();
//            trainee.DateOfBirth = time;
//            trainee.FirstName = "Neomi";
//            trainee.TraineeId = "328772801";
//            trainee.LessonsPassed = (int)Configuration.MinAmmountOfLessons + 1;
//            trainee.DrivingTeacher = "Shalom";

//            Trainee trainee1 = new Trainee();
//            trainee1.DateOfBirth = time;
//            trainee1.FirstName = "Beila";
//            trainee1.TraineeId = "205823792";
//            trainee1.LessonsPassed = (int)Configuration.MinAmmountOfLessons + 1;
//            trainee1.DrivingTeacher = "Shalom";

//            Trainee trainee2 = new Trainee();
//            trainee2.DateOfBirth = time;
//            trainee2.FirstName = "Elisha";
//            trainee2.TraineeId = "319185997";
//            trainee2.DrivingTeacher = "Yosi";

//            _traineeList.Add(trainee);
//            _traineeList.Add(trainee1);
//            _traineeList.Add(trainee2);
//            #endregion

//            //205823792

//            //make netunim for stuff   
//        }
//    }
//}
