    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Text;

using BE;
//by Neomi Mayer 328772801 and Beila Wellner 205823792
namespace DAL
{
    public interface Idal
    {
        //functions for tester
        void AddTester(Tester T);
        void DeleteTester(Tester T);
        void UpdateTester(Tester T);
        //functions for trainee
        void AddTrainee(Trainee T);
        void DeleteTrainee(Trainee T);
        void UpdateTrainee(Trainee T);
        //functions for test
        void AddTest(Test T);
        void UpdateTest(Test T);

        List<Tester> GetListOfTesters();
        List<Trainee> GetListOfTrainees();
        List<Test> GetListOfTests();
    }
}
