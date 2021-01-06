
using System.Collections.Generic;

namespace moneyteamApp.Controllers
{
    public interface IController<T>
    {
        bool Add(Dictionary<string, string> data);
    }
}
