using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Nolan.AutoClicker.ViewModels
{
    public class AddMouseCommandDialogViewModel : INotifyPropertyChanged
    {
        #region FIELDS
        private int _WheelDelta;

        #endregion

        #region PROPERTIES


        public int WheelDelta
        {
            get => this._WheelDelta;
            set
            {
                if (value != this._WheelDelta)
                {
                    this._WheelDelta = value;
                    this.OnPropertyChanged();
                }
            }
        }

        #endregion

        #region NAVIGATION PROPERTIES



        #endregion

        #region CONSTRUCTOR

        public AddMouseCommandDialogViewModel()
        {
        }
        #endregion

        #region METHODS

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
