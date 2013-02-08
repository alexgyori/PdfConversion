using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace PdfConversion.Server.ConsoleServer
{
    class ServerConfiguration : ConfigurationSection
    {
        [ConfigurationProperty("author")]
        public String Author { get { return (String)base["author"]; } }

        [ConfigurationProperty("title-prefix")]
        public String TitlePrefix { get { return (String)base["title-prefix"]; } }

        [ConfigurationProperty("temporaryFolderPath")]
        public String TemporaryFolderPath { get { return (String)base["temporaryFolderPath"]; } }

        [ConfigurationProperty("sharedFolderPath")]
        public String SharedFolderPath { get { return (String)base["sharedFolderPath"]; } }

        [ConfigurationProperty("threads") ]
        public int ThreadsNumber { get { return int.Parse( base["threads"].ToString()); } }
    }
}
