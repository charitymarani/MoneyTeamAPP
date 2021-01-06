
using System;
using moneyteamApp.DataAccess;
using moneyteamApp.models;
using System.Collections.Generic;

namespace moneyteamApp.Commands.MemberCommands
{
    public class CreateNewMemberCommand : ICommand
    {
        private Person _member;
        private readonly Dictionary<string, string> _data;
        private readonly IStore<Person> _store;
        public CreateNewMemberCommand(Dictionary<string, string> data, IStore<Person> store)
        {
            _store = store;
            _data = data;
        }

        public bool CanExecute()
        {
            int email = 0;
            var phone = _store.FindByField("PhoneNumber", _data["PhoneNumber"]);
            if(_data["EmailAddress"] != null)
            {
                email = _store.FindByField("EmailAddress", _data["EmailAddress"]);
            }

            return email == -1 && phone == -1;
        }

        public void Execute()
        {
          
            _member = new Person() {
                FirstName = _data["FirstName"], LastName = _data["FirstName"], GenderId = int.Parse(_data["GenderId"]),
                PhoneNumber= _data["PhoneNumber"], EmailAddress = _data["EmailAddress"], DateOfBirth = DateTime.Parse(_data["DateOfBirth"]),
                RoleId =int.Parse( _data["RoleId"]), GroupId = int.Parse(_data["GroupId"]), LocationId = int.Parse(_data["LocationId"])
            };
            _store.AddNew(_member);

        }

        public void Undo()
        {
            _store.DeleteItem(_member.Id);
        }
    }
}


