using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Nolan.AutoBrowser
{
    public class CommandEntry : INotifyPropertyChanged
    {
        #region FIELDS
        private Guid _Guid;
        #endregion

        #region PROPERTIES

        /// <summary>
        /// Get or set the Guid of this command.
        /// Guid is used to jump to command.
        /// </summary>
        public Guid Guid
        {
            get { return _Guid; }
            set
            {
                if (value != null || value != _Guid) _Guid = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region NAVIGATION PROPERTIES

        #endregion

        #region CONSTRUCTOR

        /// <summary>
        /// Create new instance of command entry.
        /// </summary>
        public CommandEntry()
        {
            this.Guid = Guid.NewGuid();
        }


        #endregion

        #region METHODS

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



        #endregion
    }
}
