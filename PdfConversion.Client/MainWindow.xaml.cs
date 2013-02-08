using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using PdfConversion.Client.ConversionServiceReference;

namespace PdfConversion.Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
                
        ClientConfiguration configuration;

        List<FileStatus> files;


        private DispatcherTimer timer;


        public MainWindow()
        {          
            //do initializations
            InitializeComponent();
            InitializeConfiguration();

            this.RefreshButtonClick(null, null);

            //Configure timer to refresh and show the server status in ~real-time(~every 5 seconds)
            //Could be made less often in general. To be configured.
            this.timer = new DispatcherTimer();
            this.timer.Interval = TimeSpan.FromSeconds(10);
            timer.Tick+=timer_Tick;
            timer.Start();
            this.timer_Tick(null,null);
            if (!Directory.Exists(this.configuration.SharedFolderPath + "\\in\\"))
                Directory.CreateDirectory(this.configuration.SharedFolderPath + "\\in\\");

            listBoxFiles.MouseDoubleClick+=listBoxFiles_MouseDoubleClick;
        }

       

        private void timer_Tick(object sender, EventArgs e)
        {
            try
            {
                using (var conversionServiceClient = new ConversionServiceReference.ConversionServiceClient())
                {
                    var serverStatus = conversionServiceClient.GetServerStatus();
                    this.updateLabel(serverStatus);
                }
            }
            catch
            {
                this.updateLabel(ServerState.Offline);
            }
        }

        private void updateLabel(ConversionServiceReference.ServerState serverStatus)
        {
            switch (serverStatus)
            {
                case ServerState.Processing: { this.LabelServerStatus.Content = "Processing..."; this.LabelServerStatus.Background = new SolidColorBrush(Colors.Yellow); break; }
                case ServerState.Waiting: { this.LabelServerStatus.Content = "Waiting..."; this.LabelServerStatus.Background = new SolidColorBrush(Colors.Green); break; }
                case ServerState.Offline: { this.LabelServerStatus.Content = "Offline."; this.LabelServerStatus.Background = new SolidColorBrush(Colors.Red); break; }
            }
        }

        private void listBoxFiles_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            FileStatus item = this.listBoxFiles.SelectedItem as FileStatus;
            if (item != null)
            {
                if (item.FileState == FileState.Converted)
                {
                    var fileName = this.configuration.SharedFolderPath + "\\out\\" + item.ConvertedFileName;
                    var result = MessageBox.Show("This file is converted. Do you wish to open it?", item.FileState.ToString(), MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.Yes)
                        Process.Start(fileName);
                }
                else if (item.FileState == FileState.Erroneous)
                {
                    MessageBox.Show("There was an error converting this file. Most likely this format cannot be handled", "Erroneous File", MessageBoxButton.OK);
                }
                else
                {
                    MessageBox.Show("This file is not converted yet. The file is in state:" + item.FileState.ToString(), item.FileState.ToString());
                }
            }
        }

        private void InitializeConfiguration()
        {
            ClientConfiguration config = ConfigurationManager.GetSection("client-settings") as ClientConfiguration;
            
            if (config == null)
            {
                MessageBox.Show("The application is not properly configured. Please check that App.config contains the shared folder settings");
                Application.Current.Shutdown();
            }
            this.configuration = config;
        }

        private void UploadButtonClick(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            // Set filter for file extension and default file extension 

            dlg.DefaultExt = ".txt";

            dlg.Filter = "All Files (*.*)|*.*|JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";

            dlg.Multiselect = true;

            // Display OpenFileDialog by calling ShowDialog method 

            Nullable<bool> result = dlg.ShowDialog();
            if (result.Equals(true))
            {
                foreach (var selectedFile in dlg.FileNames)
                {
                    File.Copy(selectedFile, this.configuration.SharedFolderPath + "\\in\\" + System.IO.Path.GetFileName(selectedFile));
                }
            }


        }

        private void RefreshButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var conversionServiceClient = new ConversionServiceReference.ConversionServiceClient())
                {
                    
                    var res = conversionServiceClient.GetAvailableFiles();
                    this.files = res.ToList();
                    this.listBoxFiles.ItemsSource = files;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void ListBoxKeyPressed(object sender, KeyEventArgs e)
        {
            if (e.Key.CompareTo(Key.Enter) == 0)
            {
                this.listBoxFiles_MouseDoubleClick(sender, null);
            } 
        }
       
    }
}
