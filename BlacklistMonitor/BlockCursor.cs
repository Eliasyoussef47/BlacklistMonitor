using BlacklistMonitor.Interop;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        //private BlockedBounds BlockedBounds = Screen.AllScreens[0].Bounds;
        private Point OldCursorPos;
        public List<Screen> BlacklistedScreens;
        public List<BlockedBounds> BlockedBoundsList;

        public void Start()
        {
            BlacklistedScreens = new List<Screen>();
            BlacklistedScreens.Add(Screen.AllScreens[1]);
            BlacklistedScreens.Add(Screen.AllScreens[2]);

            BlockedBoundsList = new List<BlockedBounds>();
            foreach (Screen BlacklistedScreen in BlacklistedScreens)
            {
                BlockedBoundsList.Add(BlacklistedScreen.Bounds);
            }

            MouseLLProcedure = new Native.LowLevelMouseHookProc(CursorBlockProc);
            HookId = Native.SetWindowsHookEx((int)HookType.WH_MOUSE_LL, MouseLLProcedure, (IntPtr)0, 0);
            Trace.WriteLine("Screens count: " + Screen.AllScreens.Length);
        }

        /// <summary>
        /// The procedure that does the actual blocking of the cursor. It does this by checking if the cursor has 
        /// crossed onto the blocked section and if so it moves the cursor back out of the blocked section.
        /// </summary>
        /// <param name="nCode"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        private int CursorBlockProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                MSLLHOOKSTRUCT mouseInfo = Marshal.PtrToStructure<MSLLHOOKSTRUCT>(lParam);
                Point mousePoint = mouseInfo.pt;
                // Check if the cursor is in the blocked bounds.
                foreach (BlockedBounds BlockedBounds in BlockedBoundsList)
                {
                    if (BlockedBounds.Contains(mousePoint))
                    {
                        // Determine where to put the cursor back
                        int newMousePosX = mousePoint.X; // 900
                        int newMousePosY = mousePoint.Y; // 399
                                                         // Check if the old Y pos was outside of the block
                        if (OldCursorPos.Y <= BlockedBounds.Top)
                        {
                            newMousePosY = BlockedBounds.Top;
                        }
                        if (OldCursorPos.Y >= BlockedBounds.Bottom)
                        {
                            newMousePosY = BlockedBounds.Bottom;
                        }
                        // Check if the old X pos was outside of the block
                        if (OldCursorPos.X >= BlockedBounds.Right)
                        {
                            newMousePosX = BlockedBounds.Right;
                        }
                        if (OldCursorPos.X <= BlockedBounds.Left)
                        {
                            newMousePosX = BlockedBounds.Left;
                        }
                        Native.SetCursorPos(newMousePosX, newMousePosY);
                        return 1;
                    }
                }
                OldCursorPos = mousePoint;
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
