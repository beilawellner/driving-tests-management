using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.IO;
using BE;

namespace DAL
{
    public class FactoryDAL
    {
        public static Idal getDAL(string typeDAL)
        {
            return Dal_XML_imp.Instance;
        }
    }
    public class Dal_XML_imp : Idal
    {
        #region Singleton
        private static readonly Dal_XML_imp instance = new Dal_XML_imp();
        public static Dal_XML_imp Instance
        {
            get { return instance; }
        }

        //private Dal_XML_imp() { }
        static Dal_XML_imp() { }

        #endregion

        XElement configRoot;
        string configPath = @"Data\ConfigXml.xml";
        XElement testerRoot;
        string testerPath = @"Data\TesterXml.xml";
        XElement traineeRoot;
        string traineePath = @"Data\TraineeXml.xml";
        XElement testRoot;
        string testPath = @"Data\TestXml.xml";

        private List<Trainee> _trainees = new List<Trainee>();

        private List<Test> _tests = new List<Test>();

        private List<Tester> _testers = new List<Tester>();

        public Dal_XML_imp()
        {
            #region create/load files
            if (!File.Exists(configPath))
            {
                configRoot = new XElement("TestID", "00000000");
                configRoot.Save(configPath);
            }
            else
            {
                configRoot = loadData(configPath);
                Configuration.FirstTestId = int.Parse(configRoot.Value);
            }

            if (!File.Exists(testerPath))
            {
                testerRoot = new XElement("testers");
                testerRoot.Save(testerPath);
            }
            else testerRoot=loadData(testerPath);

            if (File.Exists(traineePath))
            {
                var file = new FileStream(traineePath, FileMode.Open);

                var xmlSerializer = new XmlSerializer(typeof(List<Trainee>));

                var list = (List<Trainee>)xmlSerializer.Deserialize(file);

                file.Close();
                _trainees = list.ToList();

            }
            else
            {
                _trainees = new List<Trainee>();
            }

            if (File.Exists(testPath))
            {
                var file = new FileStream(testPath, FileMode.Open);

                var xmlSerializer = new XmlSerializer(typeof(List<Test>));

                var list = (List<Test>)xmlSerializer.Deserialize(file);

                file.Close();
                _tests = list.ToList();

            }
            else _tests = new List<Test>();
            #endregion
        }

        private XElement loadData(string path)
        {
            try
            {
                return XElement.Load(path);
            }
            catch(Exception ex)
            {
                throw new Exception("File upload problem");
            }
        }


        #region tester
        //to xml
        public void SaveTesterList(List<Tester> testersList)
        {
            testerRoot = new XElement("testers",
                                    from t in testersList
                                    select new XElement("tester",
                                        new XElement("id", t.TesterId),
                                        new XElement("firstName", t.FirstName),
                                        new XElement("sirName", t.Surname),
                                        new XElement("dateOfBirth", t.DateOfBirth),
                                        new XElement("gender", t.TesterGender),
                                        new XElement("phoneNumber", t.PhoneNumber),
                                        new XElement("email", t.Email),
                                        new XElement("car", t.Testercar),
                                        new XElement("maxDistanceForTest", t.MaxDistanceForTest),
                                        new XElement("yearsOfExperience", t.YearsOfExperience),
                                        new XElement("maxTestInAWeek", t.MaxTestsInaWeek),

                                        new XElement("address", new XElement("street"), new XElement("buildingNumber"), new XElement("city")),

                                        //new XElement("schedule", get_schedule(t._schedual))
                                        new XElement("schedule", new XElement("sunday", get_schedule(t._schedual, 0)),
                                            new XElement("monday", get_schedule(t._schedual, 1)),
                                            new XElement("tuesday", get_schedule(t._schedual, 2)),
                                            new XElement("wednesday", get_schedule(t._schedual, 3)),
                                            new XElement("thursday", get_schedule(t._schedual, 4)))
                                        )
                                     );
            testerRoot.Save(testPath);
        }

        public string get_schedule(bool[,] scheduleMatrix, int day)
        {
            bool started = false;
            bool finished = false;
            bool empty = true;
            string start = "";
            string end = "";
            string result = "";
            for (int i = 0; i < Configuration.NumOfHoursPerDay; i++)
            {
                switch (i)
                {
                    case 0:
                        if (scheduleMatrix[i, day])
                        {
                            start = "09:00-";
                            end = "10:00";
                            started = true;
                        }
                        break;

                    case 1:
                        if (scheduleMatrix[i, day])
                        {
                            if (started)
                            {
                                end = "11:00";
                            }
                            else
                            {
                                start = "10:00-";
                                end = "11:00";
                                started = true;
                            }
                        }
                        else
                        {
                            if (result == "" && start != "")
                            {
                                result = start + end;
                                start = "";
                                end = "";
                                started = false;
                                //empty = false;
                            }
                            else
                            {
                                if (start != "")
                                {
                                    result += ",";
                                    result += start;
                                    result += end;
                                    start = "";
                                    end = "";
                                    started = false;
                                    //empty = false;
                                }
                            }
                        }
                        break;

                    case 2:
                        if (scheduleMatrix[i, day])
                        {
                            if (started)
                            {
                                end = "12:00";
                            }
                            else
                            {
                                start = "11:00-";
                                end = "12:00";
                                started = true;
                            }
                        }
                        else
                        {
                            if (result == "" && start != "")
                            {
                                result = start + end;
                                start = "";
                                end = "";
                                started = false;
                                //empty = false;
                            }
                            else
                            {
                                if (start != "")
                                {
                                    result += ",";
                                    result += start;
                                    result += end;
                                    start = "";
                                    end = "";
                                    started = false;
                                    //empty = false;
                                }
                            }
                        }
                        break;

                    case 3:
                        if (scheduleMatrix[i, day])
                        {
                            if (started)
                            {
                                end = "13:00";
                            }
                            else
                            {
                                start = "12:00-";
                                end = "13:00";
                                started = true;
                            }
                        }
                        else
                        {
                            if (result == "" && start != "")
                            {
                                result = start + end;
                                start = "";
                                end = "";
                                started = false;
                                //empty = false;
                            }
                            else
                            {
                                if (start != "")
                                {
                                    result += ",";
                                    result += start;
                                    result += end;
                                    start = "";
                                    end = "";
                                    started = false;
                                    //empty = false;
                                }
                            }
                        }
                        break;

                    case 4:
                        if (scheduleMatrix[i, day])
                        {
                            if (started)
                            {
                                end = "14:00";
                            }
                            else
                            {
                                start = "13:00-";
                                end = "14:00";
                                started = true;
                            }
                        }
                        else
                        {
                            if (result == "" && start != "")
                            {
                                result = start + end;
                                start = "";
                                end = "";
                                started = false;
                                //empty = false;
                            }
                            else
                            {
                                if (start != "")
                                {
                                    result += ",";
                                    result += start;
                                    result += end;
                                    start = "";
                                    end = "";
                                    started = false;
                                    //empty = false;
                                }
                            }
                        }
                        break;

                    case 5:
                        if (scheduleMatrix[i, day])
                        {
                            if (started)
                            {
                                end = "15:00";
                            }
                            else
                            {
                                start = "14:00-";
                                end = "15:00";
                                started = true;
                            }
                        }
                        else
                        {
                            if (result == "" && start != "")
                            {
                                result = start + end;
                                start = "";
                                end = "";
                                started = false;
                                //empty = false;
                            }
                            else
                            {
                                if (start != "")
                                {
                                    result += ",";
                                    result += start;
                                    result += end;
                                    start = "";
                                    end = "";
                                    started = false;
                                    //empty = false;
                                }
                            }
                        }
                        if (start != "")
                        {
                            if (result == "")
                                result = start + end;
                            else
                            {
                                result += ",";
                                result += start;
                                result += end;
                            }
                        }
                        break;

                }
            }
            return result;
        }

        //from xml

        public Tester GetTester(string id)
        {
            Tester tester;
            try
            {
                tester = (from t in testerRoot.Elements()
                          where t.Element("id").Value == id
                          select new Tester()
                          {
                              TesterId = t.Element("id").Value,
                              FirstName = t.Element("name").Element("firstName").Value,
                              Surname = t.Element("name").Element("sirName").Value,
                              DateOfBirth = DateTime.Parse(t.Element("DateOfBirth").Value),
                              TesterGender = (Gender)Enum.Parse(typeof(Gender), t.Element("gender").Value),
                              PhoneNumber = t.Element("PhoneNumber").Value,
                              Email = t.Element("email").Value,
                              Testercar = (CarType)Enum.Parse(typeof(CarType),t.Element("car").Value),
                              MaxDistanceForTest = int.Parse(t.Element("maxDistanceForTest").Value),
                              YearsOfExperience = int.Parse(t.Element("yearsOfExperience").Value),
                              MaxTestsInaWeek = int.Parse(t.Element("maxTestInAWeek").Value),
                              TesterAdress = new Address(t.Element("address").Element("street").Value,
                                    t.Element("address").Element("buildingNumber").Value,
                                    t.Element("address").Element("city").Value),
                              _schedual = set_schedule(t.Element("schedule").Element("sunday").Value, t.Element("schedule").Element("monday").Value,
                                    t.Element("schedule").Element("tuesday").Value, t.Element("schedule").Element("wednesday").Value, t.Element("schedule").Element("thursday").Value)
                          }).FirstOrDefault();
            }
            catch
            {
                tester = null;
            }
            return tester;
        }

     
        public void set_schedule_help(bool[,] scheduleMatrix, string dayString, int num)
        {
            string hour1 = "";
            string hour2 = "";
            int h1, h2;
            int i = 0;
            while(dayString != "")
            {
                hour1 = dayString.Substring(0, 2);
                dayString = dayString.Remove(0,6);
                hour2 = dayString.Substring(0,2);
                dayString = dayString.Remove(0,5);
                if (dayString.Length > 0) dayString = dayString.Remove(0,1);
                h1 = int.Parse(hour1);
                h2 = int.Parse(hour2);
                switch(h1)
                {
                    case 9:
                        scheduleMatrix[0, num] = true;
                        i = 1;
                        while(i <= h2 - 10)
                        {
                            scheduleMatrix[i, num] = true;
                            i++;
                        }
                        break;

                    case 10:
                        scheduleMatrix[1, num] = true;
                        i = 2;
                        while (i <= h2 - 10)
                        {
                            scheduleMatrix[i, num] = true;
                            i++;
                        }
                        break;

                    case 11:
                        scheduleMatrix[2, num] = true;
                        i = 3;
                        while (i <= h2 - 10)
                        {
                            scheduleMatrix[i, num] = true;
                            i++;
                        }
                        break;

                    case 12:
                        scheduleMatrix[3, num] = true;
                        i = 4;
                        while (i <= h2 - 10)
                        {
                            scheduleMatrix[i, num] = true;
                            i++;
                        }
                        break;

                    case 13:
                        scheduleMatrix[4, num] = true;
                        i = 5;
                        while (i <= h2 - 10)
                        {
                            scheduleMatrix[i, num] = true;
                            i++;
                        }
                        break;

                    case 14:
                        scheduleMatrix[5, num] = true;
                        break;
                }
            }
        }

        public bool[,] set_schedule(string sunday, string monday, string tuesday, string wednesday, string thursday)
        {
            bool[,] scheduleMatrix = new bool[Configuration.NumOfHoursPerDay, Configuration.NumOfWorkingDays];

            set_schedule_help(scheduleMatrix, sunday, 0);
            set_schedule_help(scheduleMatrix, monday, 1);
            set_schedule_help(scheduleMatrix, tuesday, 2);
            set_schedule_help(scheduleMatrix, wednesday, 3);
            set_schedule_help(scheduleMatrix, thursday, 4);

            return scheduleMatrix;
        }



        //functions for tester
        public void AddTester(Tester tester)
        {
            loadData( testerPath);
            var id = new XElement("id", tester.TesterId);
            var firstName = new XElement("firstName", tester.FirstName);
            var sirName = new XElement("sirName", tester.Surname);
            var dateOfBirth = new XElement("dateOfBirth", tester.DateOfBirth);
            var gender = new XElement("gender", tester.TesterGender);
            var phoneNumber = new XElement("phoneNumber", tester.PhoneNumber);
            var email = new XElement("email", tester.Email);
            var car = new XElement("car", tester.Testercar);
            var maxDistanceForTest = new XElement("maxDistanceForTest", tester.MaxDistanceForTest);
            var yearsOfExperience = new XElement("yearsOfExperience", tester.YearsOfExperience);
            var maxTestInAWeek = new XElement("maxTestInAWeek", tester.MaxTestsInaWeek);

            var address = new XElement("address", new XElement("street", tester.TesterAdress.Street),
                new XElement("buildingNumber", tester.TesterAdress.BuildingNumber),
                new XElement("city", tester.TesterAdress.City));

            var schedule = new XElement("schedule", new XElement("sunday", get_schedule(tester._schedual, 0)),
                new XElement("monday", get_schedule(tester._schedual, 1)),
                new XElement("tuesday", get_schedule(tester._schedual, 2)),
                new XElement("wednesday", get_schedule(tester._schedual, 3)),
                new XElement("thursday", get_schedule(tester._schedual, 4)));

            XElement finalTester = new XElement("tester", id, firstName, sirName, dateOfBirth, gender, phoneNumber, email, car, maxDistanceForTest,
                yearsOfExperience, maxTestInAWeek, address, schedule);

            testerRoot.Add(finalTester);
            testerRoot.Save(testerPath);
        }

        public void DeleteTester(Tester tester)
        {
            string id = tester.TesterId;
            XElement testerElement;
            try
            {
                testerElement = (from t in testerRoot.Elements()
                                 where t.Element("id").Value == id
                                 select t).FirstOrDefault();
                testerElement.Remove();
                testerRoot.Save(testerPath);
            }
            catch
            {
                throw new Exception("problem deleting tester");
            }
        }

        public void UpdateTester(Tester tester)
        {
            XElement testerElement = (from t in testerRoot.Elements()
                                      where t.Element("id").Value == tester.TesterId
                                      select t).FirstOrDefault();
            testerElement.Element("firstName").Value = tester.FirstName;
            testerElement.Element("sirName").Value = tester.Surname;
            testerElement.Element("dateOfBirth").Value = tester.DateOfBirth.ToString();
            testerElement.Element("gender").Value = tester.TesterGender.ToString();
            testerElement.Element("phoneNumber").Value = tester.PhoneNumber;
            testerElement.Element("email").Value = tester.Email;
            testerElement.Element("car").Value = tester.Testercar.ToString();
            testerElement.Element("maxDistanceForTest").Value = tester.MaxDistanceForTest.ToString();
            testerElement.Element("yearsOfExperience").Value = tester.YearsOfExperience.ToString();
            testerElement.Element("maxTestInAWeek").Value = tester.MaxTestsInaWeek.ToString();

            testerElement.Element("address").Element("street").Value = tester.TesterAdress.Street;
            testerElement.Element("address").Element("buildingNumber").Value = tester.TesterAdress.BuildingNumber;
            testerElement.Element("address").Element("city").Value = tester.TesterAdress.City;

            testerElement.Element("schedule").Element("sunday").Value = get_schedule(tester._schedual, 0);
            testerElement.Element("schedule").Element("monday").Value = get_schedule(tester._schedual, 1);
            testerElement.Element("schedule").Element("tuesday").Value = get_schedule(tester._schedual, 2);
            testerElement.Element("schedule").Element("wednesday").Value = get_schedule(tester._schedual, 3);
            testerElement.Element("schedule").Element("thursday").Value = get_schedule(tester._schedual, 4);

            testerRoot.Save(testerPath);
        }
        #endregion

        #region trianee
        public void AddTrainee(Trainee T)
        {
            if (_trainees.Exists(x => x.TraineeId == T.TraineeId))
                throw new Exception("ERROR. Trianee is already in the system.");
            _trainees.Add(T);

            var file = new FileStream(traineePath, FileMode.Create);

            var xmlSerializer = new XmlSerializer(_trainees.GetType());

            xmlSerializer.Serialize(file, _trainees);

            file.Close();
        }

        public void DeleteTrainee(Trainee T)
        {
            if (_trainees.Exists(x => x.TraineeId == T.TraineeId))
            {
                _trainees.RemoveAll(x => x.TraineeId == T.TraineeId);
            }

            var file = new FileStream(traineePath, FileMode.Create);

            var xmlSerializer = new XmlSerializer(_trainees.GetType());

            xmlSerializer.Serialize(file, _trainees);

            file.Close();

        }

        public void UpdateTrainee(Trainee T)
        {
            if (_trainees.Exists(x => x.TraineeId == T.TraineeId))
            {
                _trainees.Remove(_trainees.Find(x => x.TraineeId == T.TraineeId));
                _trainees.Add(T);
            }
            var file = new FileStream(traineePath, FileMode.Create);

            var xmlSerializer = new XmlSerializer(_trainees.GetType());

            xmlSerializer.Serialize(file, _trainees);

            file.Close();
        }


        #endregion

        #region Test


        

        //functions for test
        public void AddTest(Test T)
        {
            if (Configuration.FirstTestId < 99999999)
                T.TestId = "" + Configuration.FirstTestId.ToString("D" + 8);
            Configuration.FirstTestId += 1;
            var testid = new XElement("TestID", "" + Configuration.FirstTestId.ToString("D" + 8));
            configRoot.RemoveAll();
            configRoot.Add(testid);
            configRoot.Save(configPath);

            _tests.Add(T);

            var file = new FileStream(testPath, FileMode.Create);

            var xmlSerializer = new XmlSerializer(_tests.GetType());

            xmlSerializer.Serialize(file, _tests);

            file.Close();


        }

        public void UpdateTest(Test T)
        {
            if (_tests.Exists(x => x.TestId == T.TestId))
            {
                _tests.Remove(_tests.Find(x => x.TestId == T.TestId));
                _tests.Add(T);
            }
            var file = new FileStream(testPath, FileMode.Create);

            var xmlSerializer = new XmlSerializer(_tests.GetType());

            xmlSerializer.Serialize(file, _tests);

            file.Close();
        }

        #endregion

        #region gets
        public List<Tester> GetListOfTesters()
        {
            var testers = new List<Tester>();
            try
            {
                testers = (from t in testerRoot.Elements()
                           select new Tester()
                           {
                               TesterId = t.Element("id").Value,
                               FirstName = t.Element("firstName").Value,
                               Surname = t.Element("sirName").Value,
                               DateOfBirth = DateTime.Parse(t.Element("dateOfBirth").Value),
                               TesterGender = (Gender)Enum.Parse(typeof(Gender), t.Element("gender").Value),
                               PhoneNumber = t.Element("phoneNumber").Value,
                               Email = t.Element("email").Value,
                               Testercar = (CarType)Enum.Parse(typeof(CarType), t.Element("car").Value),
                               MaxDistanceForTest = int.Parse(t.Element("maxDistanceForTest").Value),
                               YearsOfExperience = int.Parse(t.Element("yearsOfExperience").Value),
                               MaxTestsInaWeek = int.Parse(t.Element("maxTestInAWeek").Value),
                               TesterAdress = new Address(t.Element("address").Element("street").Value,
                                    t.Element("address").Element("buildingNumber").Value,
                                    t.Element("address").Element("city").Value),
                               _schedual = set_schedule(t.Element("schedule").Element("sunday").Value, t.Element("schedule").Element("monday").Value, t.Element("schedule").Element("tuesday").Value,
                                    t.Element("schedule").Element("wednesday").Value, t.Element("schedule").Element("thursday").Value)
                           }).ToList();
            }
            catch (Exception ex)
            {
                testers = null;
            }
            return testers;
        }
        
        public List<Trainee> GetListOfTrainees()
        {
            if (_trainees.Count > 0)
            {
                FileStream file = new FileStream(traineePath, FileMode.Open);


                XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Trainee>));
                List<Trainee> list = (List<Trainee>)xmlSerializer.Deserialize(file);
                file.Close();
                return list;
            }
            else return null;
        }

        public List<Test> GetListOfTests()
        {
            if (_tests.Count > 0)
            {
                FileStream file = new FileStream(testPath, FileMode.Open);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Test>));
            List<Test> list = (List<Test>)xmlSerializer.Deserialize(file); file.Close();
            return list;
            }
            else return null;
        }
   #endregion

    }
}
