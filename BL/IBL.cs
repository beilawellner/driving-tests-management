using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;


//by Neomi Mayer 328772801 and Beila Wellner 205823792
namespace BL
{
    public interface IBL
    {
        #region fuctions for tester, trainee and test
        //functions for tester
        void AddTester(Tester T);
        void DeleteTester(Tester T);
        void UpdateTester(Tester T);
        //fuctions for trainee
        void AddTrainee(Trainee T);
        void DeleteTrainee(Trainee T);
        void UpdateTrainee(Trainee T);
        //fuctions for test
        void AddTest(Test T);
        void UpdateTest(Test T);

        #endregion

        //get lists
        List<Tester> GetListOfTesters();
        List<Trainee> GetListOfTrainees();
        List<Test> GetListOfTests();

        #region  checks for trainer and trainee

        void IsText(string text);
        void IsNumber(string number);
        bool CheckId(string id);
        /// <summary>
        /// Checks if the person is the right age to to something
        /// </summary>
        /// <param name="birthday"></param>
        /// <param name="person"> if it is a trainee or tester</param>
        /// <returns></returns>
        bool CheckAge(DateTime birthday, string person);
        /// <summary>
        /// for when you need to update a person
        /// </summary>
        /// <param name="TraineeId">check by id</param>
        /// <returns></returns>
        bool TraineeInSystem(string TraineeId);
        bool TesterInSystem(string TesterId);
        /// <summary>
        /// for when you need to add a person
        /// </summary>
        /// <param name="TraineeId">check by id</param>
        /// <returns></returns>
        bool TraineeNotInSystem(string TraineeId);
        bool TesterNotInSystem(string TesterId);

        bool CheckEmail(string email);
        #endregion

        #region  checks for test
        /// <summary>
        /// checks if there are no other tests at that time
        /// and that the new test is at least a week apart
        /// </summary>
        /// <param name="T"></param>
        /// <returns></returns>
        bool NoConflictingTests(Test T);
        bool HadMinAmountOfLessons(Test T);
        /// <summary>
        /// hour for test should be in the working hours
        /// </summary>
        /// <param name="hour"></param>
        /// <returns></returns>
        bool HourInRange(int hour);
        /// <summary>
        /// you cant add a test if he already passed a previous test
        /// </summary>
        /// <param name="T"></param>
        /// <returns></returns>
        bool NotPassedPrevTest(Test T);
        /// <summary>
        /// finds an available tester
        /// </summary>
        /// <param name="T"></param>
        /// <returns>available tester id if found or null if not found</returns>
        Dictionary<string, List<int>> AvailableTesterFound(Test T);
        /// <summary>
        /// checks that the day is in the days where the testers work
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        bool DayInRange(int d);
        /// <summary>
        /// checks if a tester hasn't passed his max amount of tests to od per week
        /// </summary>
        /// <param name="T"></param>
        /// <param name="DateOfTest"></param>
        /// <returns></returns>
        bool HasntPassedMaxTests(Tester T,DateTime DateOfTest);
        #endregion

        #region  additional functions

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a">an address</param>
        /// <returns> a list of all testers near an address</returns>
        List<Tester> TestersInArea(List<Tester> testerlist, Address? a);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dateAndHour"></param>
        /// <returns>returns a list of testers that are potentially availeble in that hour</returns>
        List<Tester> AvailableTesters(DateTime dateAndHour, List<Tester> testersWithCar);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns>returns a list of all tests that the predicate returns true</returns>
        IEnumerable<Test> AllTestsThat(Func<Test,bool> predicate=null);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns>returns a list of all trainees that the predicate returns true</returns>
        List<Trainee> AllTraineesThat(Func<Trainee, bool> predicate= null);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="T"></param>
        /// <returns>returns the number of tests a trainee has done</returns>
        int NumberOfTests(Trainee T);
        /// <summary>
        /// checks if a trainee can get their license 
        /// </summary>
        /// <param name="T"></param>
        /// <returns></returns>
        bool CanGetLicence(Trainee T);
        /// <summary>
        /// 
        /// </summary>
        /// <returns>returns a list of tests that happen on that date</returns>
        List<Test> TestsByDate();

        List<Trainee> readyTrainees();
        bool HasntPassedAnyTest(Trainee T);


        #endregion

        #region grouping functions
        IEnumerable<IGrouping<CarType, Tester>> TestersByCarType(bool orderList = false);
        IEnumerable<IGrouping<string, Trainee>> TraineesByDrivingSchool(bool orderList = false);
        IEnumerable<IGrouping<string, Trainee>> TraineesByTeachers(bool orderList = false);
        IEnumerable<IGrouping<int, Trainee>> TraineesByNumTestsDone(bool orderList = false);
        #endregion

        /// <summary>
        /// gives back the distance between two addresses
        /// </summary>
        /// <returns></returns>
        string adressDistance(string origin, string destination);


    }
}
