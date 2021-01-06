
namespace moneyteamApp.models
{
    public class Role
    {
        private static int count = 0;
        private readonly int id;
        
        public int Id
         {
            get { return id; }
         }
        public string Name
        {
            get;
            set;
         }
        public Role()
        {


            id = ++count;
        }      
            
    }
}
