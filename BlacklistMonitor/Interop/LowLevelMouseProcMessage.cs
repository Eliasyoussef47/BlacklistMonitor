using System;
using System.Collections.Generic;
using System.Text;

namespace BlacklistMonitor.Interop
{
    enum LowLevelMouseProcMessage
    {
        WM_MOUSEMOVE = 0x0200,
        WM_LBUTTONDOWN = 0x0201,
        WM_RBUTTONDOWN = 0x0204,
        WM_RBUTTONUP = 0x0205,
        WM_MOUSEWHEEL = 0x020A,
        WM_MOUSEHWHEEL = 0x020E
    }
}
