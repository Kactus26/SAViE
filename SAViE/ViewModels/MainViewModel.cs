using SAViE.Commands;
using SAViE.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SAViE.ViewModels
{
    class MainViewModel : ViewModelBase
    {
        public string title = "SAViE";
        public string Title 
        { 
            get => title;
            set => Set(ref title, value);
        }

        public ICommand OpenKonspCommand { get; }
        private bool CanOpenKonspCommandExecute(object p) => true;
        private void OnOpenKonspCommandExecuted(object p)
        {
            KonspektWindow konspektWindow = new KonspektWindow();
            konspektWindow.Show();
            var window = Application.Current.Windows[0];
            if (window != null)
                window.Close();
        }   

        public MainViewModel()
        {
            OpenKonspCommand = new RelayCommand(OnOpenKonspCommandExecuted, CanOpenKonspCommandExecute);
        }
    }
}
