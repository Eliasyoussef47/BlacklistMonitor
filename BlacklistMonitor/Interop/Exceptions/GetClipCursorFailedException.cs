using System;
using System.Collections.Generic;
using System.Text;

namespace BlacklistMonitor.Interop.Exceptions
{
    class GetClipCursorFailedException : Exception
    {
        public GetClipCursorFailedException() : base("GetClipCursor has failed.")
        {
        }
    }
}
