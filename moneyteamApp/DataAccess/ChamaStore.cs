using System;
using System.Collections.Generic;
using moneyteamApp.models;
using System.IO;
using Newtonsoft.Json;


namespace moneyteamApp.DataAccess
{
  
    public class ChamaStore: IStore<Chama>
    {
        List<Chama> chamas;
        string data;
        private static readonly string chamaFile = "../../../DataStore/chamas.txt";
        public ChamaStore()
        {
            data = File.ReadAllText(chamaFile);
            chamas = JsonConvert.DeserializeObject<List<Chama>>(data)
                      ?? new List<Chama>();

        }

        public void AddNew(Chama chama)
        {
            chamas.Add(chama);
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
            chamas.RemoveAt(idx);
            SaveToFile();


        }

        public int GetTotalCount()
        {
            return chamas.Count;
        }

        public List<Chama> List()
        {
            return chamas;
        }
        public void RenameItem(int id, string newName)
        {
            int idx = Find(id);
            if (idx == -1)
            {
                //log error
                return;
            }
            
            chamas[idx].Name = newName;
            SaveToFile();

        }
        public int Find(int id)
        {
            for(int i = 0; i < chamas.Count; i++)
            {
                if (chamas[i].Id == id)
                {
                    return i;
                }
            }
            return -1;

        }
        public int FindByField(string field, string value)
        {
            for (int i = 0; i < chamas.Count; i++)
            {
                if (field == "Name")
                {
                    if (chamas[i].Name == value)
                    {
                        return i;
                    }

                }
            }
            return -1;

        }

        private void SaveToFile()
        {
            string jsonData = JsonConvert.SerializeObject(chamas, Formatting.Indented);
            File.WriteAllText(chamaFile, jsonData);

        }
       
    }
}
