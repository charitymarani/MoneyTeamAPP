
using System.Collections.Generic;

namespace moneyteamApp.DataAccess
{
    public interface IStore<T>
    {
        void AddNew(T chama);
        void DeleteItem(int id);
        int GetTotalCount();
        void RenameItem(int id, string newName);
        int Find(int id);
        int FindByField(string field, string value);
        List<T> List();
        
    }
}
