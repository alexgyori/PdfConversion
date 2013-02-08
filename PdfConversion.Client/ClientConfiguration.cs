using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfConversion.Client
{
    class ClientConfiguration : ConfigurationSection
    {
        [ConfigurationProperty("sharedFolderPath")]
        public String SharedFolderPath { get { return (String)base["sharedFolderPath"]; } }
    }
}
