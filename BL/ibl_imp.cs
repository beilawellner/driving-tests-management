using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Mail;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using BE;
using DAL;
using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Xml;


//by Neomi Mayer 328772801 and Beila Wellner 205823792

namespace BL
{

    public class FactoryBL
    {
        public static IBL getDAL(string typeDAL)
        {
            return IBL_imp.Instance;
        }
    }
    public class IBL_imp : IBL
    {
        DateTime now = DateTime.Today;

        #region Singleton
        private static readonly IBL_imp instance = new IBL_imp();

        public static IBL_imp Instance
        {
            get { return instance; }
        }
        #endregion
        static Idal MyDal;

        #region Constructor

        private IBL_imp() { }

        static IBL_imp()
        {
            string TypeDAL = ConfigurationSettings.AppSettings.Get("TypeDS");
            MyDal = FactoryDAL.getDAL(TypeDAL);
        }

        private Idal dal = Dal_XML_imp.Instance;

        #endregion

        #region Functions for Tester



        public void AddTester(Tester T)
        {
            try
            {
                bool[] checkAll =
                { CheckId(T.TesterId),
                    CheckAge(T.DateOfBirth,"Tester"),
                    TesterNotInSystem(T.TesterId),
                    CheckEmail(T.Email)};

                bool clear = checkAll.All(x => x);
                if (!clear)
                    throw new Exception("Tester Not Added");

                dal.AddTester(T);

            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }



        }

        public void DeleteTester(Tester T)
        {
            bool[] checkAll =
               {CheckId(T.TesterId),
                TesterInSystem(T.TesterId)
                };

            bool clear = checkAll.All(x => x);
            if (clear)
                dal.DeleteTester(T);
        }

        public void UpdateTester(Tester T)
        {
            bool[] checkAll =
                {CheckId(T.TesterId),
                CheckAge(T.DateOfBirth,"Tester"),
                TesterInSystem(T.TesterId),
                CheckEmail(T.Email)};

            bool clear = checkAll.All(x => x);
            if (clear)
                dal.UpdateTester(T);
        }

        #endregion

        #region Functions for Trainee

        public void AddTrainee(Trainee T)
        {
            try
            {
                bool[] checkAll =
                {
                    CheckId(T.TraineeId),
                    CheckAge(T.DateOfBirth, "Trainee"),
                    TraineeNotInSystem(T.TraineeId),
                    CheckEmail(T.Email)
                     };


                bool clear = checkAll.All(x => x == true);
                if (clear)
                    dal.AddTrainee(T);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void DeleteTrainee(Trainee T)
        {
            bool[] checkAll = {
                    CheckId(T.TraineeId),
            TraineeInSystem(T.TraineeId)
                    };

            bool clear = checkAll.All(x => x);
            if (clear)
                dal.DeleteTrainee(T);
        }

        public void UpdateTrainee(Trainee T)
        {
            bool[] checkAll =
                {CheckId(T.TraineeId),CheckAge(T.DateOfBirth,"Trainee")
            ,TraineeInSystem(T.TraineeId)
                ,CheckEmail(T.Email)};

            bool clear = checkAll.All(x => x);
            if (clear)
                dal.UpdateTrainee(T);
        }

        #endregion

        #region Functions for Tests

        public void AddTest(Test T)
        {
            bool[] checkAll =
            {
                
                //TesterInSystem(T.TesterId),
                HadMinAmountOfLessons(T),
                HourInRange(T.DateAndHourOfTest.Hour),
                DayInRange((int)T.TestDate.DayOfWeek),

                NotPassedPrevTest(T),
               // AvailableTesterFound(T)!=null,
                T.TesterId!=null && T.TesterId!="",
            TraineeInSystem(T.TraineeId),
            NoConflictingTests(T)

            };
            bool clear = checkAll.All(x => x);
            if (clear)
            {
                //  T.TesterId = AvailableTesterFound(T);
                dal.AddTest(T);
            }
            else throw new Exception();

        }

        public void UpdateTest(Test T)
        {
            //UpdateTest(T);
            //try
            //{
                //checks all of the bool properties to see if any are empty
                bool emptyfield = T.GetType().GetProperties()
                    .Where(pi => pi.PropertyType == typeof(bool))
                    .Select(pi => (bool)pi.GetValue(T))
                    .Any(value => value == null);
                if (emptyfield )
                    throw new Exception("ERROR. Not all of fields for end of test filled");
                var mostFailed = T.GetType().GetProperties()
                    .Where(pi => pi.PropertyType == typeof(bool?))
                    .Select(pi => (bool?)pi.GetValue(T) == false);
                if (mostFailed.Count(x => x == true) > 5 && T.TestPassed == true)
                    throw new Exception("ERROR. Cannot pass a student if failed more then five checks." +
                                        " The test will not be updated.");
                dal.UpdateTest(T);
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e.Message);
            //}
        }

        #endregion

        #region Get Lists

        public List<Tester> GetListOfTesters()
        {
            return dal.GetListOfTesters();
        }

        public List<Trainee> GetListOfTrainees()
        {
            return dal.GetListOfTrainees();
        }

        public List<Test> GetListOfTests()
        {
            return dal.GetListOfTests();
        }



        #endregion

        #region Checks for people

        public void IsText(string text)
        {
            if (text == "" || text == null)
                throw new Exception("Warning. Field is empty.");
            //if (!Regex.IsMatch(text, @"^[a-zA-Z]+$"))
            if (text.Any(char.IsDigit))
                throw new Exception("ERROR. Text must not include numbers.");
        }

        public void IsNumber(string number)
        {
            if (number == "" || number == null)
                throw new Exception("Warning. Field is empty.");

            foreach (char c in number)
            {
                if (c < '0' || c > '9')
                    throw new Exception("ERROR. Text must only include numbers.");
            }
        }

        public bool CheckId(string id)
        {
            //try
            //{
            if (id == null || id == "")
                throw new Exception("ERROR. Field is empty.");
            IsNumber(id);
            int idcheck;
            //if (id.Length > 9)
            //    throw new Exception("ERROR. Id is too long.");
            //if (id.Length < 8)
            //    throw new Exception("ERROR. Not enough numbers in id.");

            string tempId = id;
            //check if it's all numbers- 8/9 numbers
            if (tempId.Length == 8)
                tempId = "0" + tempId;//adding '0' to id begining
            if (tempId.Length == 9)
            {
                int sum = 0;
                int calulate = 0;
                for (int i = 0; i < 9; i++)
                {
                    if (i % 2 == 0)//Multiplying the double places by 1
                    {
                        calulate = 1 * (int)Char.GetNumericValue(tempId[i]);
                    }
                    else //if(i % 2 != 0) Multiplying the double places by 2
                    {
                        calulate = 2 * (int)Char.GetNumericValue(tempId[i]);
                    }
                    if (calulate >= 10)
                    {
                        calulate = 1 + (calulate % 10);//tens digit (can only be 1) + Unity digit
                    }
                    sum += calulate;
                }
                if (sum % 10 == 0)
                {
                    return true;
                }
                else throw new Exception("ERROR. Id is invalid.");
            }
            //}
            //    catch (Exception e)
            //    {
            //        Console.WriteLine(e.Message);
            //        return false;
            //    }

            return false;
        }

        public bool CheckAge(DateTime birthday, string person)
        {
            try
            {
                //DateTime now = DateTime.Today;
                int age = now.Year - birthday.Year;
                switch (person)
                {
                    case "Tester":
                        if (age < Configuration.MinAgeOFTester)
                            throw new Exception("ERROR. Age is too young");
                        break;
                    case "Trainee":
                        if (age < Configuration.MinAgeOFTrainee)
                            throw new Exception("ERROR. Age is too young");
                        break;
                    default:
                        break;
                }
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        public bool TraineeInSystem(string TraineeId)
        {


            try
            {
                List<Trainee> traineeList = dal.GetListOfTrainees();
                if (traineeList == null)
                    return false;
                if (!traineeList.Any(x => x.TraineeId == TraineeId))
                {
                    throw new Exception("ERROR. The trainee isn't in the system.");
                }
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }

        }
        public bool TesterInSystem(string TesterId)
        {
            try
            {
                List<Tester> testerList = dal.GetListOfTesters();
                if (testerList.All(x => x.TesterId != TesterId))
                {
                    throw new Exception("ERROR. The tester isn't in the system.");
                }
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        public bool TraineeNotInSystem(string TraineeId)
        {
            try
            {
                List<Trainee> traineeList = dal.GetListOfTrainees();
                if (traineeList == null)
                    return true;
                if (traineeList.Any(x => x.TraineeId == TraineeId))
                {
                    throw new Exception("ERROR. The trainee is already in the system.");
                }
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        public bool TesterNotInSystem(string TesterId)
        {
            try
            {
                List<Tester> testerList = dal.GetListOfTesters();
                if (testerList.Any(x => x.TesterId == TesterId))
                {
                    throw new Exception("ERROR. The tester alredy is in the system.");
                }
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        public bool CheckEmail(string email)
        {
            try
            {

                if (email == "" || email == null)
                    return true;
                else
                {
                    new System.Net.Mail.MailAddress(email);
                    return true;
                }


            }
            catch (Exception e)
            {
                //throw new Exception("ERROR. Invalid email address");
                return false;
            }
        }

        #endregion

        #region checks for test
        public bool NoConflictingTests(Test T)
        {
            try
            {

                List<Test> testlist = GetListOfTests();
                //gets all of the datetimes of the tests with the same student
                if(testlist!=null)
                if (testlist.Count > 0)
                {
                    var testTime = from item in AllTestsThat(x => x.TraineeId == T.TraineeId && x.CarType == T.CarType)
                                   select item.DateAndHourOfTest;
                    if (testTime.Any())
                    {
                        //if there is a test that is less then a week 
                        if (testTime.Any(x => (T.DateAndHourOfTest - x).TotalDays < 7))
                            throw new Exception("ERROR. test dates are less than a week apart");
                        //if there are any tests with the same date and hour
                        if (testTime.Any(x => x == T.DateAndHourOfTest))
                            throw new Exception("ERROR. it is not allowed to have two tests at the same time");
                    }
                }

                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);

                return false;
            }
        }

        public bool HadMinAmountOfLessons(Test T)
        {
            try
            {
                List<Trainee> trainees = GetListOfTrainees();
                if (trainees.Find(x => x.TraineeId == T.TraineeId).LessonsPassed < Configuration.MinAmmountOfLessons)
                    throw new Exception("ERROR. The trainee has not passed the minimum amount of lessons.");
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        public bool HourInRange(int hour)
        {
            try
            {
                if (hour < Configuration.StartOfWorkDay || hour > Configuration.EndOfWorkDay)
                    throw new Exception("ERROR. Test hour out of range. Range is from 9:00 to 15:00");
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        public bool NotPassedPrevTest(Test T)
        {
            try
            {
                List<Test> testlist = dal.GetListOfTests();
                if (testlist != null)
                {
                bool passedTheTest = testlist.Where(x => x.TraineeId == T.TraineeId)
                    .Any(x => x.CarType == T.CarType && x.TestPassed == true);
                if (passedTheTest)
                    throw new Exception("ERROR. Can't add a test that already has been passed");
                }
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        public Dictionary<string, List<int>> AvailableTesterFound(Test T)
        {
            //all testers available in that hour from work schedual and other tests
            List<int> availableHours = new List<int>();
            try
            {
                //filters car type and hasnt passed max tests
                List<Tester> testers = GetListOfTesters();
                var clear = from tester in testers
                            where tester.Testercar == T.CarType && HasntPassedMaxTests(tester, T.TestDate)
                            select tester;

                List<Tester> cleanTesters = clear.ToList();
                if (cleanTesters.Count == 0)
                    throw new Exception("ERROR. No trainers with that cartype are avialable.");
                //makes a list of all avialble hours for a test
                List<Tester> filteredTesters = new List<Tester>();
                DateTime checkhour = T.TestDate;
                checkhour = checkhour.AddHours(Configuration.StartOfWorkDay);
                //makes a list of testers and their available hours 
                Dictionary<string, List<int>> TestersAndHours = new Dictionary<string, List<int>>();

                //goes through every hour of the day and adds to the dictionary
                for (int i = Configuration.StartOfWorkDay; i < Configuration.EndOfWorkDay; i++)
                {
                    //gets all avialble testers in that hour
                    filteredTesters = AvailableTesters(checkhour, cleanTesters);
                    if (filteredTesters.Any())
                    {
                        foreach (Tester t in filteredTesters)
                        {
                            //adds an hour to the tester

                            if (TestersAndHours.ContainsKey(t.TesterId))
                            {

                                TestersAndHours[t.TesterId].Add(i);
                            }
                            else
                            {
                                TestersAndHours.Add(t.TesterId, new List<int>());
                                TestersAndHours[t.TesterId].Add(i);
                            }

                        }

                        availableHours.Add(i);
                    }
                    checkhour = checkhour.AddHours(1);
                }
                //if there are no available hours then the day gets blacked out
                if (availableHours.Count == 0)
                    throw new Exception("ERROR. There are no available testers that day.");
                return TestersAndHours;
               
            }
            catch (Exception e)
            {
                return null;
            }



        }

        public bool DayInRange(int t)
        {
            // try
            // {
            bool retrunVal = true;
            if (t > Configuration.EndOfWorkWeek)
            {
                retrunVal = false;
                throw new Exception("ERROR. Test day of the week is out of range.");
            }
            return retrunVal;
            //}
            //            catch (Exception e)
            //            {
            ////                Console.WriteLine(e.Message);
            //                return false;
            //            }

        }

        public bool HasntPassedMaxTests(Tester T, DateTime DateOfTest)
        {
            List<Test> tests = dal.GetListOfTests();

            if (tests != null)
            {

            //gets the date for the beginning of the week
            int diff = (7 + (DateOfTest.DayOfWeek - DayOfWeek.Sunday)) % 7;
            DateTime weekDay = DateOfTest.AddDays(-1 * diff).Date;
            int countTests = 0;
            //goes through the week and adds the tests the tester has that week
            for (int i = 0; i < 5; i++)
            {
                var testsInDay = from test in tests
                                 where test.TestDate == weekDay && T.TesterId == test.TestId
                                 select test;
                countTests += testsInDay.Count();
                weekDay = weekDay.AddDays(1);
            }

            if (countTests < T.MaxTestsInaWeek)
                return true;
            return false;
            }

            return true;
        }

        #endregion

        #region additional functions

        public List<Tester> TestersInArea(List<Tester> testerlist, Address? a)
        {
            List<Tester> filteredTesters = new List<Tester>();
            try
            {
                //gets testers with the distance 
                Dictionary<Tester, string> testersWithDistance = new Dictionary<Tester, string>();
               
                foreach (Tester t in testerlist)
                {
                    //gets distance between tester and test address
                    string distance = adressDistance(t.TesterAdress.ToString(), a.ToString());
                    if (!distance.Contains("ERROR"))
                    {
                        //gets the distance
                        List<string> dis = distance.Split(',').ToList<string>();
                        //if the ditance is smaller then the max distance it adds the tester to the list
                        if (float.Parse(dis.First()) <= t.MaxDistanceForTest)
                        {
                            testersWithDistance.Add(t, dis.First());
                            filteredTesters.Add(t);
                        }
                    }
                    else if (distance.Contains("internet"))
                    {
                        throw new Exception("ERROR. No Internet.");
                    }


                }
                //orders the testers by the ditance to the test address
                var filter = filteredTesters.OrderBy(x => testersWithDistance[x]);
                filteredTesters = filter.ToList();
                return filteredTesters;
            }
            catch (Exception ex)
            {
                Tester Error = new Tester();
                Error.TesterId = ex.Message;
                filteredTesters.Add(Error);
                return filteredTesters;
            }
        }

        //all available testers in that hour schedual and other test wise
        public List<Tester> AvailableTesters(DateTime dateAndHour, List<Tester> testersWithCar)
        {
            List<Test> testlist = dal.GetListOfTests();
            

            List<Tester> filteredTesters = new List<Tester>();
            int dayOfWeek = (int)dateAndHour.DayOfWeek;
            int hour = dateAndHour.Hour;
            //if the day and hour is in range
            if (dayOfWeek < 5 && hour >= Configuration.StartOfWorkDay && hour <= Configuration.EndOfWorkDay)
                foreach (Tester t in testersWithCar)
                {
                    //gets all hours in a day 
                    var colum = Enumerable.Range(0, t._schedual.GetLength(0))
                        .Select(x => t._schedual[x, dayOfWeek])
                        .ToArray();
                    if(testlist!=null)
                    {
                        //if there is another test in that hour
                        bool noOtherTest =
                        testlist.Where(x => x.TestDate == dateAndHour.Date && x.TesterId == t.TesterId)
                        .All(delegate (Test x) { return x.DateAndHourOfTest.Hour != hour; });

                    
                    //if the tester is available in that hour and doesnt have any other tests 
                    if (colum[hour - Configuration.StartOfWorkDay] != false && noOtherTest)
                        filteredTesters.Add(t);
                    }
                    else
                    {
                        if (colum[hour - Configuration.StartOfWorkDay] != false)
                            filteredTesters.Add(t);
                    }
                }    
            return filteredTesters;
        }

        public IEnumerable<Test> AllTestsThat(Func<Test, bool> predicate = null)
        {
            List<Test> testsList = dal.GetListOfTests();

            if (predicate == null)
                return testsList;

            var all = from test in testsList
                      where predicate(test)
                      select test;
            return all;

        }

        public List<Trainee> AllTraineesThat(Func<Trainee, bool> predicate = null)
        {
            List<Trainee> traineeList = dal.GetListOfTrainees();

            if (predicate == null)
                return traineeList;

            var all = from trainee in traineeList
                      where predicate(trainee)
                      select trainee;
            return all.ToList();
        }

        public int NumberOfTests(Trainee T)
        {
            List<Test> testList = dal.GetListOfTests();
            var tests = from test in testList
                        where test.TraineeId == T.TraineeId
                        select test;
            return tests.Count();
        }

        public bool CanGetLicence(Trainee T)
        {
            List<Test> testList = dal.GetListOfTests();
            if (testList != null)
            {

            var tests = from test in testList
                        where test.TestPassed==true && test.TraineeId == T.TraineeId
                        select test;
            if (tests.Any())
                return true;
            }
            return false;

        }

        public List<Test> TestsByDate()
        {
            List<Test> testList = dal.GetListOfTests();

            var tests = testList.OrderByDescending(x => x.DateAndHourOfTest);
            return tests.ToList();
        }

        public List<Trainee> readyTrainees()
        {

            List<Trainee> trainees = GetListOfTrainees();
            if(trainees==null)
                return null;
            var filter = from trainee in trainees
                         where HasntPassedAnyTest(trainee) && trainee.LessonsPassed >= Configuration.MinAmmountOfLessons
                         select trainee;
            List<Trainee> filterbytime = filter.ToList();
            foreach (Trainee train in filter)
            {
                if (GetListOfTests() != null)
                {
                var testTime = from item in GetListOfTests().Where(x => x.TraineeId == train.TraineeId && x.CarType == train.Traineecar)
                               select item.DateAndHourOfTest;
                if (testTime.Any() && testTime.Count() > 1)
                    if (testTime.Any(x => (now - x).TotalDays < 7))
                        filterbytime.Remove(train);
                }

            }
            return filterbytime;
        }

        public bool HasntPassedAnyTest(Trainee T)
        {
            List<Test> testList = dal.GetListOfTests();
            if (testList != null)
            {

            var tests = from test in testList
                        where test.TestPassed==true && test.TraineeId == T.TraineeId
                        select test;
            if (!tests.Any())
                return true;
            return false;
            }

            return true;
        }

        #endregion

        #region Grouping

        public IEnumerable<IGrouping<CarType, Tester>> TestersByCarType(bool orderList = false)
        {
            List<Tester> testerList = dal.GetListOfTesters();
            if (orderList)
            {
                var testersInOrder = from tester in testerList
                                     orderby tester.Testercar
                                     group tester by tester.Testercar;
                return testersInOrder;


            }
            else
            {
                var testers = from tester in testerList
                              group tester by tester.Testercar;
                return testers;
            }
        }

        public IEnumerable<IGrouping<string, Trainee>> TraineesByDrivingSchool(bool orderList = false)
        {
            List<Trainee> traineeList = dal.GetListOfTrainees();
            if (orderList)
            {
                var traineesInOrder = from trainee in traineeList
                                      orderby trainee.DrivingSchool
                                      group trainee by trainee.DrivingSchool;
                return traineesInOrder;


            }
            else
            {
                var trainees = from trainee in traineeList
                               group trainee by trainee.DrivingSchool;
                return trainees;

            }
        }

        public IEnumerable<IGrouping<string, Trainee>> TraineesByTeachers(bool orderList = false)
        {

            List<Test> testList = dal.GetListOfTests();
            List<Trainee> traineeList = dal.GetListOfTrainees();
            List<Tester> testerList = dal.GetListOfTesters();
            if (orderList)
            {
                var traineesInOrder = from trainee in traineeList
                                      orderby trainee.DrivingTeacher
                                      group trainee by trainee.DrivingTeacher;
                return traineesInOrder;


            }
            else
            {
                var trainees = from trainee in traineeList
                               group trainee by trainee.DrivingTeacher;
                return trainees;

            }

        }

        public IEnumerable<IGrouping<int, Trainee>> TraineesByNumTestsDone(bool orderList = false)
        {

            List<Trainee> traineeList = dal.GetListOfTrainees();
            if (orderList)
            {
                var traineesInOrder = from trainee in traineeList
                                      let numTests = NumberOfTests(trainee)
                                      orderby numTests
                                      group trainee by numTests;
                return traineesInOrder;


            }
            else
            {
                var trainees = from trainee in traineeList
                               group trainee by NumberOfTests(trainee);
                return trainees;

            }
        }

        #endregion
        //returns the distance between two addresses
        public string adressDistance(string origin, string destination)
        {

            //origin = "pisga 45 st. jerusalem"; //or "תקווה פתח 100 העם אחד "etc.
            //destination = "gilgal 78 st. ramat-gan";//or "גן רמת 10 בוטינסקי'ז "etc.
            string KEY = @"oIomr8087DVAi6VGLGABq1jox21hylQh";
            string url = @"https://www.mapquestapi.com/directions/v2/route" +
                         @"?key=" + KEY +
                         @"&from=" + origin +
                         @"&to=" + destination +
                         @"&outFormat=xml" +
                         @"&ambiguities=ignore&routeType=fastest&doReverseGeocode=false" +
                         @"&enhancedNarrative=false&avoidTimedConditions=false";
            //request from MapQuest service the distance between the 2 addresses
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            WebResponse response = request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            StreamReader sreader = new StreamReader(dataStream);
            string responsereader = sreader.ReadToEnd();
            response.Close();
            //the response is given in an XML format
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml(responsereader);
            if (xmldoc.GetElementsByTagName("statusCode")[0].ChildNodes[0].InnerText == "0")
            //we have the expected answer
            {
                //display the returned distance
                XmlNodeList distance = xmldoc.GetElementsByTagName("distance");
                double distInMiles = Convert.ToDouble(distance[0].ChildNodes[0].InnerText);
                string dis = "" + (distInMiles * 1.609344);
                //  Console.WriteLine("Distance In KM: " + distInMiles * 1.609344);
                //display the returned driving time
                XmlNodeList formattedTime = xmldoc.GetElementsByTagName("formattedTime");
                string fTime = formattedTime[0].ChildNodes[0].InnerText;
                // Console.WriteLine("Driving Time: " + fTime);
                return dis + "," + fTime;
            }
            else if (xmldoc.GetElementsByTagName("statusCode")[0].ChildNodes[0].InnerText == "402")
            //we have an answer that an error occurred, one of the addresses is not found
            {
                return "ERROR. One of the addresses is not found. Try again.";
            }
            else //busy network or other error...
            {
                return "ERROR. No answer recieved. Please check your internet connection";
            }

        }
    }
}

