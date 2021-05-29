using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Nolan.Hook
{
    public class ExtMouseEventArgs : MouseEventArgs
    {
        #region FIELDS

        #endregion

        #region PROPERTIES
        /// <summary>
        /// Get the screen based X-Coordinate  that the event occured.
        /// </summary>
        public int ScreenX { get; private set; }

        /// <summary>
        /// Get the screen based Y-Coordinate  that the event occured.
        /// </summary>
        public int ScreenY { get; private set; }

        /// <summary>
        /// Get the wheel delta which describe how much the wheel has been rotated.
        /// </summary>
        public int WheelDelta { get; private set; }


        /// <summary>
        /// Get the screen based X-coordinate of the previous mouse location
        /// </summary>
        public int PreX { get; private set; }

        /// <summary>
        /// Get the screen based Y-coordinate of the previous mouse location
        /// </summary>
        public int PreY { get; private set; }

        /// <summary>
        /// Get the changed mouse button.
        /// </summary>
        public MouseButton? ChangedButton { get; private set; }
        
        /// <summary>
        /// Get the click count.
        /// </summary>
        public int ClickCount { get; private set; }


        #endregion

        #region CONSTRUCTOR

        /// <summary>
        /// Initializes a new instance of the ExtMouseEventArgs class using the specified MouseDevice and timestamp.
        /// </summary>
        /// <param name="button">The changed button.</param>
        /// <param name="clickCount">The number of times a mouse button was pressed.</param>
        /// <param name="timestamp">The timestampt for the event.</param>
        /// <param name="screenX">The x-coordinate of the mouse click, in screen-based pixels.</param>
        /// <param name="screenY">The y-coordinate of the mouse click, in screen-based pixels.</param>
        /// <param name="wheelDelta">A signed count of the number of detents the wheel has rotated.</param>
        /// <param name="preX">The x-coordinate of the previous mouse location.</param>
        /// <param name="preY">The y-coordinate of the previous mouse location.</param>

        public ExtMouseEventArgs(MouseButton? button, int clickCount, int timestamp, int screenX, int screenY, int wheelDelta, int preX=-1, int preY=-1) 
            : base(Mouse.PrimaryDevice, timestamp)
        {
            this.ChangedButton = button;
            this.ClickCount = clickCount;
            this.ScreenX = screenX;
            this.ScreenY = screenY;
            this.WheelDelta = wheelDelta;
            this.PreX = preX;
            this.PreY = preY;
        }
        #endregion
    }
}
