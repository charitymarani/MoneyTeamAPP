
using System.Collections.Generic;
using moneyteamApp.models;
using System.IO;
using Newtonsoft.Json;


namespace moneyteamApp.DataAccess
{

    public class MemberStore : IStore<Person>
    {
        List<Person> _members;
        string data;
        private static readonly string locationFile = "../../../DataStore/members.txt";
        public MemberStore()
        {
            data = File.ReadAllText(locationFile);
            _members = JsonConvert.DeserializeObject<List<Person>>(data)
                      ?? new List<Person>();

        }

        public void AddNew(Person member)
        {
            _members.Add(member);
            SaveToFile();

        }

        public void DeleteItem(int id)
        {
            int idx = Find(id);
            if (idx == -1)
            {
                //log error
                return;
            }
            _members.RemoveAt(idx);
            SaveToFile();


        }

        public int GetTotalCount()
        {
            return _members.Count;
        }

        public void RenameItem(int id, string newName)
        {
            int idx = Find(id);
            if (idx == -1)
            {
                //log error
                return;
            }

            _members[idx].FirstName = newName;
            SaveToFile();

        }
        public int Find(int id)
        {
            for (int i = 0; i < _members.Count; i++)
            {
                if (_members[i].Id == id)
                {
                    return i;
                }
            }
            return -1;

        }
        public int FindByField(string field,  string value)
        {
            for (int i = 0; i < _members.Count; i++)
            {
                if (field == "PhoneNumber")
                {
                    if (_members[i].PhoneNumber == value)
                    {
                        return i;
                    }

                }
                if (field == "EmailAddress")
                {
                    if (_members[i].EmailAddress == value)
                    {
                        return i;
                    }

                }
            }
            return -1;

        }
        public List<Person> List()
        {
            return _members;
        }
        private void SaveToFile()
        {
            string jsonData = JsonConvert.SerializeObject(_members, Formatting.Indented);
            File.WriteAllText(locationFile, jsonData);

        }

    }
}



