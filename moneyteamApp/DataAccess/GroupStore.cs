
using System.Collections.Generic;
using moneyteamApp.models;
using System.IO;
using Newtonsoft.Json;


namespace moneyteamApp.DataAccess
{
    
    public class GroupStore : IStore<Group>
    {
        List<Group> _groups;
        string data;
        private static readonly string groupFile = "../../../DataStore/groups.txt";
        public GroupStore()
        {
            data = File.ReadAllText(groupFile);
            _groups = JsonConvert.DeserializeObject<List<Group>>(data)
                      ?? new List<Group>();

        }

        public void AddNew(Group group)
        {
            _groups.Add(group);
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
            _groups.RemoveAt(idx);
            SaveToFile();


        }

        public int GetTotalCount()
        {
            return _groups.Count;
        }

        public void RenameItem(int id, string newName)
        {
            int idx = Find(id);
            if (idx == -1)
            {
                //log error
                return;
            }

            _groups[idx].Name = newName;
            SaveToFile();

        }
        public int Find(int id)
        {
            for (int i = 0; i < _groups.Count; i++)
            {
                if (_groups[i].Id == id)
                {
                    return i;
                }
            }
            return -1;

        }
        public int FindByField(string field, string value)
        {
            for (int i = 0; i < _groups.Count; i++)
            {
                if (field == "Name")
                {
                    if (_groups[i].Name == value)
                    {
                        return i;
                    }

                }
            }
            return -1;

        }
        public List<Group> List()
        {
            return _groups;
        }
        private void SaveToFile()
        {
            string jsonData = JsonConvert.SerializeObject(_groups, Formatting.Indented);
            File.WriteAllText(groupFile, jsonData);

        }

    }
}
