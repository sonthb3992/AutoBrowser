using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Nolan.AutoClicker.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        #region FIELDS

        #endregion

        #region PROPERTIES

        #endregion

        #region COMMANDS


        public Commands.ActionCommand AddMouseCommand { get; set; }
        #endregion

        #region CONSTRUCTOR

        public MainWindowViewModel()
        {
            this.AddMouseCommand = new Commands.ActionCommand(this.OpenAddMouseCommandDialog);
        }


        #endregion

        #region METHODS
        private void OpenAddMouseCommandDialog(object obj)
        {
            Views.AddMouseCommandDialog dialog = new Views.AddMouseCommandDialog();
            dialog.ShowDialog();
        }

        #endregion


        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Helper method
        /// </summary>
        private void OnPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

    }
}
