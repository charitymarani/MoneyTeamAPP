using System;
using System.Collections.Generic;
namespace moneyteamApp.models
{
    public class Location
    {
        private static int count=0;
        private readonly int id;
        public int Id
        {
            get { return id; }
        }
        public string Name
        {
            get;
            set;
        }
        public double Latitude
        {
            get;
            set;
        }
        public double Longitude
        {
            get;
            set;

        }
        public Location()
        {
                
            id = ++count;
                
        }
        
    }
}
