
using System.Collections.Generic;
using moneyteamApp.models;
using System.IO;
using Newtonsoft.Json;


namespace moneyteamApp.DataAccess
{

    public class NoticeStore : IStore<Notice>
    {
        List<Notice> _notices;
        string _data;
        private static readonly string noticeFile = "../../../DataStore/notices.txt";
        public NoticeStore()
        {
            _data = File.ReadAllText(noticeFile);
            _notices = JsonConvert.DeserializeObject<List<Notice>>(_data)
                      ?? new List<Notice>();

        }

        public void AddNew(Notice notice)
        {
            _notices.Add(notice);
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
            _notices.RemoveAt(idx);
            SaveToFile();


        }

        public int GetTotalCount()
        {
            return _notices.Count;
        }

        public List<Notice> List()
        {
            return _notices;
        }
        public void RenameItem(int id, string newMessage)
        {
            int idx = Find(id);
            if (idx == -1)
            {
                //log error
                return;
            }

            _notices[idx].Message = newMessage;
            SaveToFile();

        }
        public int Find(int id)
        {
            for (int i = 0; i < _notices.Count; i++)
            {
                if (_notices[i].Id == id)
                {
                    return i;
                }
            }
            return -1;

        }
        public int FindByField(string field, string value)
        {
            for (int i = 0; i < _notices.Count; i++)
            {
                if (field == "ReceiverId")
                {
                    if (_notices[i].ReceiverId == int.Parse(value))
                    {
                        return i;
                    }

                }
            }
            return -1;

        }

        private void SaveToFile()
        {
            string jsonData = JsonConvert.SerializeObject(_notices, Formatting.Indented);
            File.WriteAllText(noticeFile, jsonData);

        }

    }
}
