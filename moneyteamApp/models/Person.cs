using System;
using System.Collections.Generic;

namespace moneyteamApp.models
{
    public class Person
    {
        private static int count=0;
        private readonly int id;
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int LocationId { get; set; }
        public int GenderId{ get; set; }
        public int RoleId { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int GroupId { get; set; }
        public string EmailAddress { get; set; }
        public int Id
        {
            get { return id; }
        }

        public Person()
        {
            id = ++count;


        }

    }
}
