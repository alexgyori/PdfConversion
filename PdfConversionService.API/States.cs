using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PdfConversionService.API
{
    public enum ServerState
    {
        Offline, Waiting, Processing
    }

    public enum FileState
    {
        Queued, Converting, Converted, Missing,
        Erroneous
    }
}
