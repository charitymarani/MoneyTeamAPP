
namespace moneyteamApp.models
{
    public class Group
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
        public int ChamaId { get; set; }
        
        
        public Group()
        {
           
            id = ++count;
            
        }  
    }
}
