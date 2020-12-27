using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
//by Neomi Mayer 328772801 and Beila Wellner 205823792

namespace BE
{
    public struct Address
    {
        private string _street;
        private string _buildingNumber;
        private string _city;

        public string Street { get => _street; set => _street = value; }
        public string BuildingNumber { get => _buildingNumber; set => _buildingNumber = value; }
        public string City { get => _city; set => _city = value; }


        public Address(string st,string bn, string c)
        {
            _street = st;
            _buildingNumber = bn;
            _city = c;
        }

        public override string ToString()
        {
            //only makes a to string if all things are not null
            if (Street != null && City != null && BuildingNumber != null)
                return Street + " " + BuildingNumber + " st. " + City;

            else return null;

        }
        //comparison operators for address
        public static  bool operator ==(Address a,Address b)
        {
            return a.Street == null || a.City == null || a.BuildingNumber == null;
        }
        public static bool operator !=(Address a, Address b)
        {
            return a.Street != null && a.City != null && a.BuildingNumber != null;
        }


    }


}


