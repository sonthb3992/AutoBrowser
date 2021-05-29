using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Nolan.Hook
{
    public class ExtKeyPressEventArgs : KeyEventArgs

    {
        #region CONSTRUCTOR

        public ExtKeyPressEventArgs(KeyboardDevice keyboardDevice, System.Windows.PresentationSource presentationSource, int time, Key key, Char KeyChar)
            : base(Keyboard.PrimaryDevice, Keyboard.PrimaryDevice.ActiveSource, time, key)

        {
            this.KeyChar = KeyChar;
        }

        public char KeyChar { get; private set; }
        #endregion
    }
}
