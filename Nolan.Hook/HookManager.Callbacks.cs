using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Nolan.Hook
{
    public static partial class HookManager
    {
        #region MOUSE HOOK
        /// <summary>
        /// This field is not objectively needed but we need to keep a reference on a delegate which will be 
        /// passed to unmanaged code. To avoid GC to clean it up.
        /// When passing delegates to unmanaged code, they must be kept alive by the managed application 
        /// until it is guaranteed that they will never be called.
        /// </summary>
        private static HookProc s_MouseDelegate;

        /// <summary>
        /// Store the handle to the mouse hook procedure.
        /// </summary>
        private static int s_MouseHookHandle;

        /// <summary>
        /// Store the last mouse location.
        /// </summary>
        private static int m_OldX;
        private static int m_OldY;

        /// <summary>
        /// A HookProc delegate representing the hook procedure method. 
        /// Used as the value passed into lpfn (long pointer to function).
        /// </summary>
        private delegate int HookProc(int nCode, int wParam, IntPtr lParam);


        /// <summary>
        /// A callback function which will be called every time a mouse activity detected. 
        /// </summary>
        /// <param name="nCode">
        /// [in] Specifies whether the hook procedure must process the message. 
        /// If nCode is HC_ACTION, the hook procedure must process the message. 
        /// If nCode is less than zero, the hook procedure must pass the message to the 
        /// CallNextHookEx function without further processing and must return the 
        /// value returned by CallNextHookEx.
        /// </param>
        /// <param name="wParam">
        /// [in] Specifies whether the message was sent by the current thread. 
        /// If the message was sent by the current thread, it is nonzero; otherwise, it is zero. 
        /// </param>
        /// <param name="lParam">
        /// [in] Pointer to a CWPSTRUCT structure that contains details about the message. 
        /// </param>
        /// <returns>
        /// If nCode is less than zero, the hook procedure must return the value returned by CallNextHookEx. 
        /// If nCode is greater than or equal to zero, it is highly recommended that you call CallNextHookEx 
        /// and return the value it returns; otherwise, other applications that have installed WH_CALLWNDPROC 
        /// hooks will not receive hook notifications and may behave incorrectly as a result. If the hook 
        /// procedure does not call CallNextHookEx, the return value should be zero. 
        /// </returns>
        private static int MouseHookProc(int nCode, int wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                MouseLLHookStruct mouseHookStruct = (MouseLLHookStruct)Marshal.PtrToStructure(lParam, typeof(MouseLLHookStruct));
                MouseButton? button = null;
                short mouseDelta = 0;
                int clickCount = 0;
                bool mouseDown = false;
                bool mouseUp = false;

                switch (wParam)
                {
                    case WM_LBUTTONDOWN:
                        mouseDown = true;
                        button = MouseButton.Left;
                        clickCount = 1;
                        break;
                    case WM_LBUTTONUP:
                        mouseUp = true;
                        button = MouseButton.Left;
                        clickCount = 1;
                        break;
                    case WM_LBUTTONDBLCLK:
                        button = MouseButton.Left;
                        clickCount = 2;
                        break;
                    case WM_RBUTTONDOWN:
                        mouseDown = true;
                        button = MouseButton.Right;
                        clickCount = 1;
                        break;
                    case WM_RBUTTONUP:
                        mouseUp = true;
                        button = MouseButton.Right;
                        clickCount = 1;
                        break;
                    case WM_RBUTTONDBLCLK:
                        button = MouseButton.Right;
                        clickCount = 2;
                        break;
                }

                ExtMouseEventArgs e = new ExtMouseEventArgs(
                                                   button,
                                                   clickCount,
                                                   mouseHookStruct.Time,
                                                   mouseHookStruct.Point.X,
                                                   mouseHookStruct.Point.Y,
                                                   mouseDelta,
                                                   m_OldX,
                                                   m_OldY);

                //Mouse up
                if (s_MouseUp != null && mouseUp)
                {
                    s_MouseUp.Invoke(null, e);
                }


                //Mouse down
                if (s_MouseDown != null && mouseDown)
                {
                    s_MouseDown.Invoke(null, e);
                }

                //If someone listens to left click and a click is heppened
                if (s_MouseLeftButtonClick != null && button == MouseButton.Left && clickCount > 0)
                {
                    s_MouseLeftButtonClick.Invoke(null, e);
                }


                //If someone listens to right button click and a click is heppened
                if (s_MouseRightButtonClick != null && button == MouseButton.Right && clickCount > 0)
                {
                    s_MouseRightButtonClick.Invoke(null, e);
                }

                //If someone listens to left click and a click is heppened
                if (s_MouseMiddleButtonClick != null && button == MouseButton.Middle && clickCount > 0)
                {
                    s_MouseMiddleButtonClick.Invoke(null, e);
                }

                //If someone listens to double click and a click is heppened
                if (s_MouseDoubleClick != null && clickCount == 2)
                {
                    s_MouseDoubleClick.Invoke(null, e);
                }

                //Wheel was moved
                if (s_MouseWheel != null && mouseDelta != 0)
                {
                    s_MouseWheel.Invoke(null, e);
                }

                //If someone listens to move and there was a change in coordinates raise move event
                if ((s_MouseMove != null) && (m_OldX != mouseHookStruct.Point.X || m_OldY != mouseHookStruct.Point.Y))
                {
                    m_OldX = mouseHookStruct.Point.X;
                    m_OldY = mouseHookStruct.Point.Y;
                    if (s_MouseMove != null)
                    {
                        s_MouseMove.Invoke(null, e);
                    }
                }

                if (e.Handled)
                {
                    return -1;
                }
            }

            //call next hook
            return CallNextHookEx(s_MouseHookHandle, nCode, wParam, lParam);
        }

        /// <summary>
        /// This method install Keyboard hook only if it is not installed and must be installed
        /// </summary>
        private static void EnsureSubscribedToGlobalMouseEvents()
        {
            // install Mouse hook only if it is not installed and must be installed
            if (s_MouseHookHandle == 0)
            {
                //See comment of this field. To avoid GC to clean it up.
                s_MouseDelegate = MouseHookProc;

                //install hook
                s_MouseHookHandle = SetWindowsHookEx(
                    WH_MOUSE_LL,
                    s_MouseDelegate,
                    GetModuleHandle(Process.GetCurrentProcess().MainModule.ModuleName), 
                    0);

                //If SetWindowsHookEx fails.
                if (s_MouseHookHandle == 0)
                {
                    //Returns the error code returned by the last unmanaged function called using platform invoke that has the DllImportAttribute.SetLastError flag set. 
                    int errorCode = Marshal.GetLastWin32Error();
                    //do cleanup

                    //Initializes and throws a new instance of the Win32Exception class with the specified error. 
                    throw new Win32Exception(errorCode);
                }
            }
        }


        /// <summary>
        /// This method checks whether any subsribers are registered to the hook.
        /// If there's no subscriber then unsubscribe from hook
        /// </summary>
        private static void TryUnsubscribeFromGlobalMouseEvents()
        {
            //if no subsribers are registered unsubsribe from hook
            if (s_MouseLeftButtonClick == null &&
                s_MouseRightButtonClick == null &&
                s_MouseMiddleButtonClick == null &&
                s_MouseDown == null &&
                s_MouseMove == null &&
                s_MouseUp == null &&
                s_MouseWheel == null)
            {
                ForceUnsunscribeFromGlobalMouseEvents();
            }
        }

        /// <summary>
        /// Phương thức này hủy cài đặt Hook theo dõi chuột.
        /// </summary>
        private static void ForceUnsunscribeFromGlobalMouseEvents()
        {
            if (s_MouseHookHandle != 0)
            {
                //uninstall hook
                int result = UnhookWindowsHookEx(s_MouseHookHandle);
                //reset invalid handle
                s_MouseHookHandle = 0;
                //Free up for GC
                s_MouseDelegate = null;
                //if failed and exception must be thrown
                if (result == 0)
                {
                    //Returns the error code returned by the last unmanaged function called using platform invoke that has the DllImportAttribute.SetLastError flag set. 
                    int errorCode = Marshal.GetLastWin32Error();
                    //Initializes and throws a new instance of the Win32Exception class with the specified error. 
                    throw new Win32Exception(errorCode);
                }
            }
        }


        #endregion

        #region KEYBOARD HOOK PROCESSING

        /// <summary>
        /// This field is not objectively needed but we need to keep a reference on a delegate which will be 
        /// passed to unmanaged code. To avoid GC to clean it up.
        /// When passing delegates to unmanaged code, they must be kept alive by the managed application 
        /// until it is guaranteed that they will never be called.
        /// </summary>
        private static HookProc s_KeyboardDelegate;

        /// <summary>
        /// Stores the handle to the Keyboard hook procedure.
        /// </summary>
        private static int s_KeyboardHookHandle;

        /// <summary>
        /// A callback function which will be called every Time a keyboard activity detected.
        /// </summary>
        /// <param name="nCode">
        /// [in] Specifies whether the hook procedure must process the message. 
        /// If nCode is HC_ACTION, the hook procedure must process the message. 
        /// If nCode is less than zero, the hook procedure must pass the message to the 
        /// CallNextHookEx function without further processing and must return the 
        /// value returned by CallNextHookEx.
        /// </param>
        /// <param name="wParam">
        /// [in] Specifies whether the message was sent by the current thread. 
        /// If the message was sent by the current thread, it is nonzero; otherwise, it is zero. 
        /// </param>
        /// <param name="lParam">
        /// [in] Pointer to a CWPSTRUCT structure that contains details about the message. 
        /// </param>
        /// <returns>
        /// If nCode is less than zero, the hook procedure must return the value returned by CallNextHookEx. 
        /// If nCode is greater than or equal to zero, it is highly recommended that you call CallNextHookEx 
        /// and return the value it returns; otherwise, other applications that have installed WH_CALLWNDPROC 
        /// hooks will not receive hook notifications and may behave incorrectly as a result. If the hook 
        /// procedure does not call CallNextHookEx, the return value should be zero. 
        /// </returns>
        private static int KeyboardHookProc(int nCode, Int32 wParam, IntPtr lParam)
        {
            //indicates if any of underlaing events set e.Handled flag
            bool handled = false;

            if (nCode >= 0)
            {
                //read structure KeyboardHookStruct at lParam
                KeyboardLLHookStruct MyKeyboardHookStruct = (KeyboardLLHookStruct)Marshal.PtrToStructure(lParam, typeof(KeyboardLLHookStruct));

                //Get the key
                var keyData = KeyInterop.KeyFromVirtualKey(MyKeyboardHookStruct.VirtualKeyCode);

                //raise KeyDown
                if (s_KeyDown != null && (wParam == WM_KEYDOWN || wParam == WM_SYSKEYDOWN))
                {
                    KeyEventArgs e = new KeyEventArgs(
                        Keyboard.PrimaryDevice,
                        Keyboard.PrimaryDevice.ActiveSource,
                        MyKeyboardHookStruct.Time,
                        keyData);
                    s_KeyDown.Invoke(null, e);
                    handled = e.Handled;
                }

                // raise KeyPress
                if (s_KeyPress != null && wParam == WM_KEYDOWN)
                {
                    bool isDownShift = ((GetKeyState(VK_SHIFT) & 0x80) == 0x80 ? true : false);
                    bool isDownCapslock = (GetKeyState(VK_CAPITAL) != 0 ? true : false);

                    byte[] keyState = new byte[256];
                    GetKeyboardState(keyState);
                    byte[] inBuffer = new byte[2];
                    if (ToAscii(MyKeyboardHookStruct.VirtualKeyCode,
                              MyKeyboardHookStruct.ScanCode,
                              keyState,
                              inBuffer,
                              MyKeyboardHookStruct.Flags) == 1)
                    {
                        char key = (char)inBuffer[0];
                        if ((isDownCapslock ^ isDownShift) && Char.IsLetter(key)) key = Char.ToUpper(key);


                        ExtKeyPressEventArgs e = new ExtKeyPressEventArgs(Keyboard.PrimaryDevice,
                        Keyboard.PrimaryDevice.ActiveSource,
                        MyKeyboardHookStruct.Time,
                        keyData,
                        key);


                        s_KeyPress.Invoke(null, e);
                        handled = handled || e.Handled;
                    }
                }

                // raise KeyUp
                if (s_KeyUp != null && (wParam == WM_KEYUP || wParam == WM_SYSKEYUP))
                {
                    KeyEventArgs e = new KeyEventArgs(
                       Keyboard.PrimaryDevice,
                       Keyboard.PrimaryDevice.ActiveSource,
                       MyKeyboardHookStruct.Time,
                       keyData);
                    handled = handled || e.Handled;
                }

            }

            //if event handled in application do not handoff to other listeners
            if (handled)
                return -1;

            //forward to other application
            return CallNextHookEx(s_KeyboardHookHandle, nCode, wParam, lParam);
        }


        private static void EnsureSubscribedToGlobalKeyboardEvents()
        {
            // install Keyboard hook only if it is not installed and must be installed
            if (s_KeyboardHookHandle == 0)
            {
                //See comment of this field. To avoid GC to clean it up.
                s_KeyboardDelegate = KeyboardHookProc;
                //install hook
                s_KeyboardHookHandle = SetWindowsHookEx(
                    WH_KEYBOARD_LL,
                    s_KeyboardDelegate,
                    GetModuleHandle(Process.GetCurrentProcess().MainModule.ModuleName),
                    0);
                //If SetWindowsHookEx fails.
                if (s_KeyboardHookHandle == 0)
                {
                    //Returns the error code returned by the last unmanaged function called using platform invoke that has the DllImportAttribute.SetLastError flag set. 
                    int errorCode = Marshal.GetLastWin32Error();
                    //do cleanup

                    //Initializes and throws a new instance of the Win32Exception class with the specified error. 
                    throw new Win32Exception(errorCode);
                }
            }
        }

        private static void TryUnsubscribeFromGlobalKeyboardEvents()
        {
            //if no subsribers are registered unsubsribe from hook
            if (s_KeyDown == null &&
                s_KeyUp == null &&
                s_KeyPress == null)
            {
                ForceUnsunscribeFromGlobalKeyboardEvents();
            }
        }

        private static void ForceUnsunscribeFromGlobalKeyboardEvents()
        {
            if (s_KeyboardHookHandle != 0)
            {
                //uninstall hook
                int result = UnhookWindowsHookEx(s_KeyboardHookHandle);
                //reset invalid handle
                s_KeyboardHookHandle = 0;
                //Free up for GC
                s_KeyboardDelegate = null;
                //if failed and exception must be thrown
                if (result == 0)
                {
                    //Returns the error code returned by the last unmanaged function called using platform invoke that has the DllImportAttribute.SetLastError flag set. 
                    int errorCode = Marshal.GetLastWin32Error();
                    //Initializes and throws a new instance of the Win32Exception class with the specified error. 
                    throw new Win32Exception(errorCode);
                }
            }
        }
        #endregion
    }

}
