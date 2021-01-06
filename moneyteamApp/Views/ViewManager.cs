using System;
namespace moneyteamApp.Views
{
    public interface IViewManager
    {
        void Invoke(IView view, string command);
    }
    public class ViewManager: IViewManager
    {
        public ViewManager()
        {
        }

        public void Invoke(IView view, string command)
        {
            view.ProcessCommand(command);
        }
    }
}
