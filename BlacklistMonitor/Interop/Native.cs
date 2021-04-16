using BlacklistMonitor.Interop.Exceptions;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace BlacklistMonitor.Interop
{
    /// <summary>
    /// Contains the functions that don't need wrapping and the wrapped functions.
    /// </summary>
    static class Native
    {
        [DllImport("USER32.dll")]
        public static extern short GetKeyState(VirtualKeyStates nVirtKey);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern bool ClipCursor(RECT rcClip);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetCursorPos(int x, int y);
        /// <summary>
        /// A callback function used with the SetWindowsHookEx function. The system calls this function every time a 
        /// new mouse input event is about to be posted into a thread input queue.
        /// </summary>
        /// <param name="nCode">A code the hook procedure uses to determine how to process the message. If nCode is 
        /// less than zero, the hook procedure must pass the message to the CallNextHookEx function without further 
        /// processing and should return the value returned by CallNextHookEx. This parameter can be one of the 
        /// following values.</param>
        /// <param name="mouseMessageId">The identifier of the mouse message. This parameter can be one of the 
        /// following messages: WM_LBUTTONDOWN, WM_LBUTTONUP, WM_MOUSEMOVE, WM_MOUSEWHEEL, WM_MOUSEHWHEEL, 
        /// WM_RBUTTONDOWN, or WM_RBUTTONUP.</param>
        /// <param name="mouseEventInfo">A pointer to an MSLLHOOKSTRUCT structure.</param>
        /// <returns>If nCode is less than zero, the hook procedure must return the value returned by CallNextHookEx. 
        /// If nCode is greater than or equal to zero, and the hook procedure did not process the message, it is 
        /// highly recommended that you call CallNextHookEx and return the value it returns; otherwise, other 
        /// applications that have installed WH_MOUSE_LL hooks will not receive hook notifications and may behave 
        /// incorrectly as a result. If the hook procedure processed the message, it may return a nonzero value to 
        /// prevent the system from passing the message to the rest of the hook chain or the target window procedure.
        /// </returns>
        public delegate int LowLevelMouseHookProc(int nCode, IntPtr mouseMessageId, IntPtr mouseEventInfo);
        /// <summary>
        /// Installs an application-defined hook procedure into a hook chain. You would install a hook procedure to 
        /// monitor the system for certain types of events. These events are associated either with a specific thread 
        /// or with all threads in the same desktop as the calling thread.
        /// </summary>
        /// <param name="hookType">The type of hook procedure to be installed.</param>
        /// <param name="hookProcedure">A HookProc wrapping the method to be called by the hook. If the threadId 
        /// parameter is zero or specifies the identifier of a thread created by a different process, the 
        /// hookProcedure parameter must point to a hook procedure in a DLL. Otherwise, hookProcedure can point to a 
        /// hook procedure in the code associated with the current process.</param>
        /// <param name="instanceHandle">A handle to the DLL containing the hook procedure pointed to by the 
        /// hookProcedure parameter. The instanceHandle parameter must be set to (IntPtr)0 if the threadId parameter 
        /// specifies a thread created by the current process and if the hook procedure is within the code associated 
        /// with the current process.</param>
        /// <param name="threadId">The identifier of the thread with which the hook procedure is to be associated. For 
        /// desktop apps, if this parameter is zero, the hook procedure is associated with all existing threads 
        /// running in the same desktop as the calling thread.</param>
        /// <returns>If the function succeeds, the return value is the handle to the hook procedure. If the function 
        /// fails, the return value is IntPtr.Zero</returns>
        [DllImport("User32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern int SetWindowsHookEx(int hookType, LowLevelMouseHookProc hookProcedure, IntPtr instanceHandle, int threadId);
        /// <summary>
        /// Removes a hook procedure installed in a hook chain by the SetWindowsHookEx function.
        /// </summary>
        /// <param name="hookId">A handle to the hook to be removed. This parameter is a hook handle obtained by a 
        /// previous call to SetWindowsHookEx.</param>
        /// <returns>If the function succeeds, the return value is nonzero. If the function fails, the return value is 
        /// zero.</returns>
        [DllImport("User32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern bool UnhookWindowsHookEx(int hookId);
        /// <summary>
        /// Passes the hook information to the next hook procedure in the current hook chain. A hook procedure can 
        /// call this function either before or after processing the hook information.
        /// </summary>
        /// <param name="hookId">This parameter is ignored.</param>
        /// <param name="nCode">The hook code passed to the current hook procedure. The next hook procedure uses this 
        /// code to determine how to process the hook information.</param>
        /// <param name="wParam">The wParam value passed to the current hook procedure. The meaning of this parameter 
        /// depends on the type of hook associated with the current hook chain.</param>
        /// <param name="lParam">The lParam value passed to the current hook procedure. The meaning of this parameter 
        /// depends on the type of hook associated with the current hook chain.</param>
        /// <returns>This value is returned by the next hook procedure in the chain. The current hook procedure must 
        /// also return this value. The meaning of the return value depends on the hook type. For more information, 
        /// see the descriptions of the individual hook procedures.</returns>
        [DllImport("User32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern int CallNextHookEx(int hookId, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr GetModuleHandle(string moduleName);

        public static POINT GetCursorPos()
        {
            POINT w32Mouse = new POINT();
            bool succes = NativeBase.GetCursorPos(ref w32Mouse);
            if (!succes)
            {
                throw new GetCursorPosFailedException();
            }
            return w32Mouse;
        }

        public static RECT GetClipCursor()
        {
            RectStruct rectStruct = new RectStruct();
            bool succes = NativeBase.GetClipCursor(ref rectStruct);
            if (!succes)
            {
                throw new GetClipCursorFailedException();
            }
            return rectStruct;
        }

    }

    /// <summary>
    /// Contains the functions that need to be wrapped.
    /// </summary>
    static class NativeBase
    {
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetCursorPos(ref POINT pt);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern bool GetClipCursor(ref RectStruct lprect);
    }
}
