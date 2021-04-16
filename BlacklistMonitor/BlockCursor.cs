using BlacklistMonitor.Interop;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace BlacklistMonitor
{
    class BlockCursor
    {
        private static int HookId = 0;
        private Native.LowLevelMouseHookProc MouseLLProcedure;
        private int MaxMousePosY = 400;
        private int MaxMousePosX = 1000;
        private Point CurrentCursorPos;
        public List<Screen> BlacklistedMonitors;

        public void Start()
        {
            MouseLLProcedure = new Native.LowLevelMouseHookProc(MouseBlockProc);
            HookId = Native.SetWindowsHookEx((int)HookType.WH_MOUSE_LL, MouseLLProcedure, (IntPtr)0, 0);
        }

        private bool CursorInScreen(Screen screen)
        {
            Point CursorPos = Native.GetCursorPos();
            return screen.Bounds.Contains(CursorPos);
        }
        

        /// <summary>
        /// The procedure that does the actual blocking of the cursor. It does this by checking if the cursor has 
        /// crossed onto the blocked section and if so it moves the cursor back out of the blocked section.
        /// </summary>
        /// <param name="nCode"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        private int MouseBlockProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                MSLLHOOKSTRUCT mouseInfo = new MSLLHOOKSTRUCT();
                mouseInfo = (MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, mouseInfo.GetType());
                Point mousePoint = mouseInfo.pt;

                if (mousePoint.X < MaxMousePosX && mousePoint.Y > MaxMousePosY)
                {
                    int newMousePosX = mousePoint.X;
                    int newMousePosY = mousePoint.Y;
                    if (CurrentCursorPos.Y <= MaxMousePosY)
                    {
                        newMousePosY = MaxMousePosY;
                    }
                    if (CurrentCursorPos.X >= MaxMousePosX)
                    {
                        newMousePosX = MaxMousePosX;
                    }
                    Native.SetCursorPos(newMousePosX, newMousePosY);
                    return 1;
                }
                CurrentCursorPos = mousePoint;
            }
            return nCode < 0 ? Native.CallNextHookEx(0, nCode, wParam, lParam) : 0;
        }

        public void Stop()
        {
            if (HookId != 0)
            {
                Native.UnhookWindowsHookEx(HookId);
            }
        }
    }
}
