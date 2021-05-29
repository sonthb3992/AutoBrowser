using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Nolan.Hook
{
    public static partial class HookManager
    {

        /// <summary>
        /// The point structure defines the X- and Y- coordinates of a point.
        /// </summary>
        /// <remarks>
        /// DON'T use System.Drawing.Point, the order of the fields in System.Drawing.Point isn't guaranteed to stay the same.
        /// </remarks>
        [StructLayout(LayoutKind.Sequential)]
        public struct Point
        {
            /// <summary>
            /// The x-coordinate of the point.
            /// </summary>
            public int X;

            /// <summary>
            /// The y-coordinate of the point.
            /// </summary>
            public int Y;

        }

        /// <summary>
        /// The MouseLLHookStruct contains information about a low-level mouse input event.
        /// </summary>
        /// <remarks>
        /// https://docs.microsoft.com/en-us/windows/win32/api/winuser/ns-winuser-msllhookstruct
        /// </remarks>
        [StructLayout(LayoutKind.Sequential)]
        public struct MouseLLHookStruct
        {
            /// <summary>
            /// The x- and y-coordinates of the cursor, in per-monitor-aware screen coordinates.
            /// </summary>
            public Point Point;


            /// <summary>
            /// If the message is WM_MOUSEWHEEL, the high-order word of this member is the wheel delta. 
            /// The low-order word is reserved. 
            /// A positive value indicates that the wheel was rotated forward, away from the user; 
            /// A negative value indicates that the wheel was rotated backward, toward the user. 
            /// One wheel click is defined as WHEEL_DELTA, which is 120.
            /// 
            /// If the message is WM_XBUTTONDOWN, WM_XBUTTONUP, WM_XBUTTONDBLCLK, WM_NCXBUTTONDOWN, WM_NCXBUTTONUP, 
            /// or WM_NCXBUTTONDBLCLK, the high-order word specifies which X button was pressed or released, 
            /// and the low-order word is reserved. This value can be one or more of the following values. 
            /// Otherwise, mouseData is not used.
            /// XBUTTON1 0x0001 The first X button was pressed or released.
            /// XBUTTON2 0x0002 The second X button was pressed or released.
            /// </summary>
            public int MouseData;


            /// <summary>
            /// The event-injected flags. An application can use the following values to test the flags. 
            /// Testing LLMHF_INJECTED (bit 0) will tell you whether the event was injected. 
            /// If it was, then testing LLMHF_LOWER_IL_INJECTED (bit 1) will tell you whether or not the event was injected 
            /// from a process running at lower integrity level.
            /// </summary>
            public int Flags;


            /// <summary>
            /// The time stamp for this message.
            /// </summary>
            public int Time;


            /// <summary>
            /// Additional information associated with the message.
            /// </summary>
            public IntPtr ExtraInfo;

        }


        /// <summary>
        /// The KeyboardLLHookStruct structure contains information about a low-level keyboard input event. 
        /// </summary>
        /// <remarks>
        /// http://msdn.microsoft.com/library/default.asp?url=/library/en-us/winui/winui/windowsuserinterface/windowing/hooks/hookreference/hookstructures/cwpstruct.asp
        /// </remarks>
        [StructLayout(LayoutKind.Sequential)]
        private struct KeyboardLLHookStruct
        {
            /// <summary>
            /// Specifies a virtual-key code. The code must be a value in the range 1 to 254. 
            /// </summary>
            public int VirtualKeyCode;

            /// <summary>
            /// Specifies a hardware scan code for the key. 
            /// </summary>
            public int ScanCode;

            /// <summary>
            /// Specifies the extended-key flag, event-injected flag, context code, and transition-state flag.
            /// </summary>
            public int Flags;

            /// <summary>
            /// Specifies the Time stamp for this message.
            /// </summary>
            public int Time;

            /// <summary>
            /// Specifies extra information associated with the message. 
            /// </summary>
            public int ExtraInfo;
        }
    }

}
