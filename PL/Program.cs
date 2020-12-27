using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BE;
using BL;
//by Neomi Mayer 328772801 and Beila Wellner 205823792
namespace PL
{
    class Program
    {
        static void Main(string[] args)
        {
            BL.ibl_imp ibl = new ibl_imp();
            List<Tester> _testerList = ibl.GetListOfTesters();
            List<Trainee> _traineeList = ibl.GetListOfTrainees();
            List<Test> _testList = ibl.GetListOfTests();
            DateTime time = DateTime.Today;

            #region tester
            //wrong id1
            Tester tester = new Tester();
            tester.DateOfBirth = new DateTime(1960, 12, 12, 0, 0, 0);
            tester.FirstName = "Neomi";
            tester.TesterId = "gfdgsg";
            tester.YearsOfExperience = 10;
            tester.MaxTestsInaWeek = 5;
            ibl.AddTester(tester);
            //_testerList.Add(tester);

            //wrong id2
            tester.TesterId = "205823795";
            ibl.AddTester(tester);
            //_testerList.Add(tester);

            //wrong id3
            tester.TesterId = "2058";
            ibl.AddTester(tester);
            //_testerList.Add(tester);

            //too young
            tester.TesterId = "328772801";
            tester.DateOfBirth = time;
            ibl.AddTester(tester);
            //_testerList.Add(tester);

            //correct
            tester.DateOfBirth = new DateTime(1960, 12, 12, 0, 0, 0);
            ibl.AddTester(tester);
            //_testerList.Add(tester);

            //exist
            Tester tester1 = new Tester();
            tester1.DateOfBirth = new DateTime(1960, 12, 12, 0, 0, 0);
            tester1.FirstName = "Neomi";
            tester1.TesterId = "328772801";
            ibl.AddTester(tester1);
            //_testerList.Add(tester);

            //update tester
            tester.Sirname = "Mayer";
            tester.TesterGender = Gender.Female;
            tester.PhoneNumber = "0584797944";
            tester.MaxDistanceForTest = 5;
            tester.Testercar = CarType.Private;
            ibl.UpdateTester(tester);

            foreach (Tester t in _testerList)
            {
                System.Console.WriteLine(t);
            }

            //add tester to delete after
            tester1.DateOfBirth = new DateTime(1960, 12, 12, 0, 0, 0);
            tester1.FirstName = "Beila";
            tester1.TesterId = "205823792";
            tester1.YearsOfExperience = 10;
            tester1.MaxTestsInaWeek = 5;
            ibl.AddTester(tester1);
            ibl.DeleteTester(tester1);

            foreach (Tester t in _testerList)
            {
                System.Console.WriteLine(t);
            }
            #endregion

            #region trainee
            //too young
            Trainee trainee = new Trainee();
            trainee.DateOfBirth = time;
            trainee.FirstName = "Beila";
            trainee.TraineeId = "205823792";
            ibl.AddTrainee(trainee);

            //correct
            trainee.DateOfBirth = new DateTime(1994, 12, 30, 0, 0, 0);
            ibl.AddTrainee(trainee);

            //exist
            Trainee trainee1 = new Trainee();
            trainee1.DateOfBirth = new DateTime(1994, 12, 30, 0, 0, 0);
            trainee1.FirstName = "Beila";
            trainee1.TraineeId = "205823792";
            ibl.AddTrainee(trainee1);

            //apdate
            trainee.Sirname = "Wellner";
            trainee.DrivingSchool = "Beit Sefer Lenehiga";
            trainee.DrivingTeacher = "Moshe";
            trainee.LessonsPassed = 17;
            trainee.PhoneNumber = "0546212193";
            trainee.TraineeGear = GearType.Automatic;
            trainee.TraineeGender = Gender.Female;
            ibl.UpdateTrainee(trainee);

            //add to delete after
            trainee1.DateOfBirth = new DateTime(1995, 12, 30, 0, 0, 0);
            trainee1.FirstName = "Neomi";
            trainee1.TraineeId = "328772801";
            ibl.AddTrainee(trainee1);
            ibl.DeleteTrainee(trainee1);

            foreach (Trainee t in _traineeList)
            {
                System.Console.WriteLine(t);
            }
            #endregion

            #region test
            //wrong time and not enough lessons to trainee (and wrong id to tester and trainee)
            Test test = new Test();
            test.TestDate = new DateTime(2019, 1, 12, 0, 0, 0);

            test.DateAndHourOfTest = new DateTime(2019, 1, 12, 0, 0, 0);
            test.CarType = CarType.Private;
            test.CheckMirrors = true;
            test.ImediateStop = true;
            test.KeptDistance = true;
            test.KeptRightofPresidence = true;
            test.Parking = true;
            test.ReverseParking = true;
            test.RightTurn = true;
            test.StoppedAtRed = true;
            test.StoppedAtcrossWalk = true;
            test.UsedSignal = true;
            test.TestPassed = false;
            test.RemarksOnTest = "the trainee was graet";
            test.TesterId = "204233852";
            test.TraineeId = "205823792";
            //test.TraineeId = "319185997";
            ibl.AddTest(test);

            //still wrong id to tester and trainee
            trainee.LessonsPassed = 21;
            ibl.UpdateTrainee(trainee);
            test.DateAndHourOfTest = new DateTime(2019, 1, 12, 12, 0, 0);
            ibl.AddTest(test);

            //correct
            test.TesterId = "328772801";
            //test.TraineeId = "205823792";
            ibl.AddTest(test);

            foreach (Test t in _testList)
            {
                System.Console.WriteLine(t);
            }
            #endregion



            /*
            delete things
            NoConflictingTests?
            HadMinAmountOfLessons need check if trainee id exist
            test id after adding test
            */
            System.Console.ReadKey();
        }
    }
}
