
using System.Collections.Generic;
using moneyteamApp.models;
using System.IO;
using Newtonsoft.Json;


namespace moneyteamApp.DataAccess
{

    public class RoleStore : IStore<Role>
    {
        List<Role> _roles;
        string data;
        private static readonly string roleFile = "../../../DataStore/roles.txt";
        public RoleStore()
        {
            data = File.ReadAllText(roleFile);
            _roles = JsonConvert.DeserializeObject<List<Role>>(data)
                      ?? new List<Role>();

        }

        public void AddNew(Role role)
        {
            _roles.Add(role);
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
            _roles.RemoveAt(idx);
            SaveToFile();


        }

        public int GetTotalCount()
        {
            return _roles.Count;
        }

        public void RenameItem(int id, string newName)
        {
            int idx = Find(id);
            if (idx == -1)
            {
                //log error
                return;
            }

            _roles[idx].Name = newName;
            SaveToFile();

        }
        public int Find(int id)
        {
            for (int i = 0; i < _roles.Count; i++)
            {
                if (_roles[i].Id == id)
                {
                    return i;
                }
            }
            return -1;

        }
        public int FindByField(string field, string value)
        {
            for (int i = 0; i < _roles.Count; i++)
            {
                if (field == "Name")
                {
                    if (_roles[i].Name == value)
                    {
                        return i;
                    }

                }
            }
            return -1;

        }
        public List<Role> List()
        {
            return _roles;
        }

        private void SaveToFile()
        {
            string jsonData = JsonConvert.SerializeObject(_roles, Formatting.Indented);
            File.WriteAllText(roleFile, jsonData);

        }

    }
}

