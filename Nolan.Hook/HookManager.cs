using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Input;

namespace Nolan.Hook
{

    /// <summary>
    /// This class monitors mouse and keyboard operations globally and provides events accordingly.
    /// </summary>
    public static partial class HookManager
    {
        #region CUSTOM MOUSE EVENTS

        #region Mouse Move

        /// <summary>
        /// Represent the method that will handle the event MouseMove
        /// </summary>
        public delegate void MouseMoveHandler(object sender, ExtMouseEventArgs extMouseEventArgs);
        private static event MouseMoveHandler s_MouseMove;

        /// <summary>
        /// Occurs when the mouse pointer is moved. 
        /// </summary>
        public static event MouseMoveHandler MouseMove
        {
            add
            {
                EnsureSubscribedToGlobalMouseEvents();
                s_MouseMove += value;
            }

            remove
            {
                s_MouseMove -= value;
                TryUnsubscribeFromGlobalMouseEvents();
            }
        }
        #endregion

        #region Mouse Left Button Click

        /// <summary>
        /// Represent the method that will handle the event MouseLeftButtonClick
        /// </summary>
        public delegate void MouseLeftButtonClickHandler(object sender, ExtMouseEventArgs extMouseEventArgs);
        private static event MouseLeftButtonClickHandler s_MouseLeftButtonClick;

        /// <summary>
        /// Occurs when the left mouse button is clicked. 
        /// </summary>
        public static event MouseLeftButtonClickHandler MouseLeftButtonClick
        {
            add
            {
                EnsureSubscribedToGlobalMouseEvents();
                s_MouseLeftButtonClick += value;
            }

            remove
            {
                s_MouseLeftButtonClick -= value;
                TryUnsubscribeFromGlobalMouseEvents();
            }
        }
        #endregion

        #region Mouse Right Button Click

        /// <summary>
        /// Represent the method that will handle the event MouseRightButtonClick
        /// </summary>
        public delegate void MouseRightButtonClickHandler(object sender, ExtMouseEventArgs extMouseEventArgs);
        private static event MouseRightButtonClickHandler s_MouseRightButtonClick;

        /// <summary>
        /// Occurs when the right mouse button is clicked. 
        /// </summary>
        public static event MouseRightButtonClickHandler MouseRightButtonClick
        {
            add
            {
                EnsureSubscribedToGlobalMouseEvents();
                s_MouseRightButtonClick += value;
            }

            remove
            {
                s_MouseRightButtonClick -= value;
                TryUnsubscribeFromGlobalMouseEvents();
            }
        }
        #endregion

        #region Mouse Middle Button Click

        /// <summary>
        /// Represent the method that will handle the event MouseMiddleButtonClick
        /// </summary>
        public delegate void MouseMiddleButtonClickHandler(object sender, ExtMouseEventArgs extMouseEventArgs);
        private static event MouseMiddleButtonClickHandler s_MouseMiddleButtonClick;

        /// <summary>
        /// Occurs when the Middle mouse button is clicked. 
        /// </summary>
        public static event MouseMiddleButtonClickHandler MouseMiddleButtonClick
        {
            add
            {
                EnsureSubscribedToGlobalMouseEvents();
                s_MouseMiddleButtonClick += value;
            }

            remove
            {
                s_MouseMiddleButtonClick -= value;
                TryUnsubscribeFromGlobalMouseEvents();
            }
        }
        #endregion

        #region Mouse Down

        /// <summary>
        /// Represent the method that will handle the event MouseDown
        /// </summary>
        public delegate void MouseDownHandler(object sender, ExtMouseEventArgs extMouseEventArgs);
        private static event MouseDownHandler s_MouseDown;

        /// <summary>
        /// Occurs when a mouse button is pressed. 
        /// </summary>
        public static event MouseDownHandler MouseDown
        {
            add
            {
                EnsureSubscribedToGlobalMouseEvents();
                s_MouseDown += value;
            }

            remove
            {
                s_MouseDown -= value;
                TryUnsubscribeFromGlobalMouseEvents();
            }
        }

        #endregion

        #region Mouse Up

        /// <summary>
        /// Represent the method that will handle the event MouseUp
        /// </summary>
        public delegate void MouseUpHandler(object sender, ExtMouseEventArgs extMouseEventArgs);
        private static event MouseUpHandler s_MouseUp;

        /// <summary>
        /// Occurs when a mouse button is released. 
        /// </summary>
        public static event MouseUpHandler MouseUp
        {
            add
            {
                EnsureSubscribedToGlobalMouseEvents();
                s_MouseUp += value;
            }

            remove
            {
                s_MouseUp -= value;
                TryUnsubscribeFromGlobalMouseEvents();
            }
        }

        #endregion

        #region Mouse Wheel

        /// <summary>
        /// Represent the method that will handle the event MouseWheel
        /// </summary>
        public delegate void MouseWheelHandler(object sender, ExtMouseEventArgs extMouseEventArgs);
        private static event MouseWheelHandler s_MouseWheel;

        /// <summary>
        /// Occurs when the mouse wheel is rotated.
        /// </summary>
        public static event MouseWheelHandler MouseWheel
        {
            add
            {
                EnsureSubscribedToGlobalMouseEvents();
                s_MouseWheel += value;
            }

            remove
            {
                s_MouseWheel -= value;
                TryUnsubscribeFromGlobalMouseEvents();
            }
        }

        #endregion

        #region Mouse Double click
        /// <summary>
        /// Represent the method that will handle the event MouseDoubleClick
        /// </summary>
        public delegate void MouseDoubleClickHandler(object sender, ExtMouseEventArgs extMouseEventArgs);
        private static event MouseDoubleClickHandler s_MouseDoubleClick;

        //A timer to monitor the time between 2 clicks
        private static Timer s_DoubleClickTimer;

        //We remember the last button clicked because the second click also need to be the same button.
        private static MouseButton? s_PrevClickedButton;

        /// <summary>
        /// Occurs when a mouse button was double clicked.
        /// </summary>
        public static event MouseDoubleClickHandler MouseDoubleClick
        {
            add
            {
                EnsureSubscribedToGlobalMouseEvents();
                if (s_MouseDoubleClick == null)
                {
                    //When the user subscribe to the mouse double click event, we start to monitor MouseUp event.

                    //Create the timer to monitor the time between 2 MouseUp event
                    s_DoubleClickTimer = new Timer
                    {
                        // Get the double click time setting from windows
                        Interval = GetDoubleClickTime(),

                        //Not start the timer yet. It will be started when the first click occured.
                        Enabled = false
                    };

                    //When the timer elapsed without the second click, we reset the last mouse button field and stop the timer.
                    s_DoubleClickTimer.Elapsed += S_DoubleClickTimer_Elapsed;

                    //Subscribe to the MouseUp event.
                    MouseUp += OnMouseUp;
                }
                s_MouseDoubleClick += value;
            }

            remove
            {
                s_MouseDoubleClick -= value;
                if (s_MouseDoubleClick == null)
                {
                    //Stop monitoring mouse up
                    MouseUp -= OnMouseUp;

                    //Dispose the timer
                    s_DoubleClickTimer.Elapsed -= S_DoubleClickTimer_Elapsed;
                    s_DoubleClickTimer = null;
                }
                TryUnsubscribeFromGlobalMouseEvents();
            }
        }

        //Timer is alapsed and no second click occured
        private static void S_DoubleClickTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            s_DoubleClickTimer.Enabled = false;
            s_PrevClickedButton = null;
        }

        /// <summary>
        /// This method is designed to monitor mouse clicks in order to fire a double click event if interval between 
        /// clicks was short enaugh.
        /// </summary>
        /// <param name="sender">Is always null</param>
        /// <param name="e">Some information about the MouseUp Event.</param>
        private static void OnMouseUp(object sender, ExtMouseEventArgs e)
        {
            //This should not happen
            if (e.ClickCount < 1) { return; }

            //If the second click happend on the same button
            if (e.ChangedButton.HasValue && e.ChangedButton.Value.Equals(s_PrevClickedButton))
            {
                if (s_MouseDoubleClick != null)
                {
                    //Fire the double click event
                    s_MouseDoubleClick.Invoke(null, e);
                }
                //Stop the timer
                s_DoubleClickTimer.Enabled = false;

                //Reset the first click button field
                s_PrevClickedButton = null;
            }
            //It's the first click
            else
            {
                //Start the timer
                s_DoubleClickTimer.Enabled = true;

                //Remember the first clicked button.
                s_PrevClickedButton = e.ChangedButton;
            }
        }
        #endregion

        #endregion


        #region KEYBOARD EVENTS

        public delegate void KeyPressEventHandler(object sender, ExtKeyPressEventArgs e);
        private static event KeyPressEventHandler s_KeyPress;

        // <summary>
        /// Occurs when a key is pressed.
        /// </summary>
        public static event KeyPressEventHandler KeyPress
        {
            add
            {
                EnsureSubscribedToGlobalKeyboardEvents();
                s_KeyPress += value;
            }
            remove
            {
                s_KeyPress -= value;
                TryUnsubscribeFromGlobalKeyboardEvents();
            }
        }

        private static event KeyEventHandler s_KeyUp;

        /// <summary>
        /// Occurs when a key is released. 
        /// </summary>
        public static event KeyEventHandler KeyUp
        {
            add
            {
                EnsureSubscribedToGlobalKeyboardEvents();
                s_KeyUp += value;
            }
            remove
            {
                s_KeyUp -= value;
                TryUnsubscribeFromGlobalKeyboardEvents();
            }
        }

        private static event KeyEventHandler s_KeyDown;

        /// <summary>
        /// Occurs when a key is preseed. 
        /// </summary>
        public static event KeyEventHandler KeyDown
        {
            add
            {
                EnsureSubscribedToGlobalKeyboardEvents();
                s_KeyDown += value;
            }
            remove
            {
                s_KeyDown -= value;
                TryUnsubscribeFromGlobalKeyboardEvents();
            }
        }
        #endregion
    }
}