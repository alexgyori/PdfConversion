using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PdfConversion.Server.Polling;
using PdfConversion.Server.StatusService;

namespace PdfConversion.Server.ConsoleServer
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceHost host = HostService(@"http://localhost:8081/IConversionService", typeof(IConversionService), typeof(ConversionService));

            ServerConfiguration config = Program.GetConfiguration();
            if (config == null)
            {
                return;
            }
            //This tries to recover the data if the program was closed improperly.
            //It will bring the program in a state from where it can continue.
            WorkResumer resumer = new WorkResumer(config.SharedFolderPath, config.TemporaryFolderPath, config.TitlePrefix, config.Author);
            resumer.Resume();

            Thread pollingThread = RunPollingInBackground(config);
            
            Console.WriteLine("To exit type 'exit' and <enter>");
            while (true)
            {
                string str = Console.ReadLine();
                if (str != null)
                {
                    if (str.ToLower().Equals("exit"))
                    {
                        break;
                    }
                }
            }
            Console.WriteLine(@"Please wait while the program finishes the started tasks");
            Console.WriteLine(@"Forcing a close might take the program to inconsistent states and lead to lost data");
            pollingThread.Interrupt();
            host.Close();
            
            

        }

        private static Thread RunPollingInBackground(ServerConfiguration config)
        {
            IPoller poller = PollerFactory.GetPoller(config.SharedFolderPath,
                config.TemporaryFolderPath, config.ThreadsNumber, config.TitlePrefix, config.Author);

            Thread t = new Thread(poller.Poll);
            t.Start();
            return t;
        }

        private static ServerConfiguration GetConfiguration()
        {
            ServerConfiguration config;
            try
            {

                config = ConfigurationManager.GetSection("server-settings") as ServerConfiguration;
                return config;
            }
            catch
            {
                Console.WriteLine("Configuration settings could not be retrieved");
                return null;

            }
        }
        
        private static ServiceHost HostService(string strUri, Type endpointContract, Type endpointImplementaion)
        {
            Uri baseAddress = new Uri(strUri);
            ServiceHost host = new ServiceHost(endpointImplementaion, baseAddress); 
            BasicHttpBinding binding = new BasicHttpBinding();            
            host.AddServiceEndpoint(endpointContract, binding , strUri+@"/PdfService");
            // Enable metadata publishing.
            ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
            smb.HttpGetEnabled = true;
            
            smb.MetadataExporter.PolicyVersion = PolicyVersion.Policy15;
            host.Description.Behaviors.Add(smb);
            // Open the ServiceHost to start listening for messages. Since
            // no endpoints are explicitly configured, the runtime will create
            // one endpoint per base address for each service contract implemented
            // by the service.
            host.Open();
            return host;
        }
    }
}
