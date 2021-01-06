using System;
namespace moneyteamApp.models
{
    public class Notice
    {
        private static int count =0;
        private int id;
        public int Id
        {
            get { return id; }
        }
        public string Message { get; set; }
        public int ReceiverId { get; set; }

        public Notice()
        {
            id = ++count;

        }
    }
}
