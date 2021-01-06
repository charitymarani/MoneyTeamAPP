

using System.Collections.Generic;
using moneyteamApp.models;
using System.IO;
using Newtonsoft.Json;


namespace moneyteamApp.DataAccess
{

    public class LocationStore : IStore<Location>
    {
        List<Location> _locations;
        string data;
        private static readonly string locationFile = "../../../DataStore/locations.txt";
        public LocationStore()
        {
            data = File.ReadAllText(locationFile);
            _locations = JsonConvert.DeserializeObject<List<Location>>(data)
                      ?? new List<Location>();

        }

        public void AddNew(Location location)
        {
            _locations.Add(location);
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
            _locations.RemoveAt(idx);
            SaveToFile();


        }

        public int GetTotalCount()
        {
            return _locations.Count;
        }

        public void RenameItem(int id, string newName)
        {
            int idx = Find(id);
            if (idx == -1)
            {
                //log error
                return;
            }

            _locations[idx].Name = newName;
            SaveToFile();

        }
        public int Find(int id)
        {
            for (int i = 0; i < _locations.Count; i++)
            {
                if (_locations[i].Id == id)
                {
                    return i;
                }
            }
            return -1;

        }
        public int FindByField(string field, string value)
        {
            for (int i = 0; i < _locations.Count; i++)
            {
                if (field == "Name")
                {
                    if (_locations[i].Name == value)
                    {
                        return i;
                    }

                }
            }
            return -1;

        }
        public List<Location> List()
        {
            return _locations;
        }

        private void SaveToFile()
        {
            string jsonData = JsonConvert.SerializeObject(_locations, Formatting.Indented);
            File.WriteAllText(locationFile, jsonData);

        }

    }
}


