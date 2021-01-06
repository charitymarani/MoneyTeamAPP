
namespace moneyteamApp.models
{
    public class Chama
    {
        private static int count = 0;
        private readonly int id;
        
        public Chama()
        {
            id = ++count;
        }
        public int Id
        {
            get { return id; }
        }
        public string Name { get; set; }
    }
}
