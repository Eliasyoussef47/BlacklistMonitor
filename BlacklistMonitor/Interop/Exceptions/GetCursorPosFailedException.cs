using System;
using System.Collections.Generic;
using System.Text;

namespace BlacklistMonitor.Interop.Exceptions
{
    class GetCursorPosFailedException : Exception
    {
        public GetCursorPosFailedException() : base("GetCursorPos has failed.")
        {
        }
    }
}
