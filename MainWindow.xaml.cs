using System;
using System.Collections.Generic;
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
using System.IO;

namespace PROG8145_Assignment4
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    enum TimeSlots
    {
        t1000, t1030, t1100, t1130, t1200, t1230, t1300, t1330, t1400, t1430, t1500, t1530, t1600
    }
    enum Vehicles
    {
        Car, Bus, Truck, Tractor
    }
    enum Tasks
    {
        OilChange, EngineTuneup, TransmissionCleanup
    }
    public partial class MainWindow : Window
    {
        public int numOfApps = 0;
        public List<string> timeSlots = new List<string>();
        public MainWindow()
        {
            InitializeComponent();
            Go();
        }

        public void Go()
        {
            //Get a list of timeslots from enum
            var enumTimeSlots = Enum.GetValues(typeof(TimeSlots));
            string timeSlot = String.Empty;
            for (int i = 0; i < enumTimeSlots.Length; i++)
            {
                timeSlot = enumTimeSlots.GetValue(i).ToString();
                timeSlot = ExtractTimeSlot(timeSlot);
                timeSlots.Add(timeSlot);
            }
            cbTime.ItemsSource = timeSlots;
            //Get a list of vehicles from enum
            var vehicles = Enum.GetValues(typeof(Vehicles));
            cbVehicle.ItemsSource = vehicles;
        }

        public string ExtractTimeSlot(string timeSlot)
        {
            string strHour = timeSlot.Substring(1, 2);
            string strMinute = timeSlot.Substring(timeSlot.Length - 2, 2);
            return strHour + ":" + strMinute;
        }

        public List<string> GetVehicleTasks()
        {
            var tasks = Enum.GetValues(typeof(Tasks));
            List<string> tasksList = new List<string>();
            for (int i = 0; i < tasks.Length; i++)
            {
                string task = tasks.GetValue(i).ToString();
                tasksList.Add(task);
            }
            return tasksList;
        }

        private void cbTime_DropDownClosed(object sender, EventArgs e)
        {
            //Validate time slot
            if (cbTime.Text.Equals(""))
            {
                lbTimeErr.Content = "Please choose a time slot!";
            } else
            {
                lbTimeErr.Content = "";
            }
        }

        private void cbVehicle_DropDownClosed(object sender, EventArgs e)
        {
            //Validate vehicle
            if (cbVehicle.Text.Equals(""))
            {
                lbVehicleErr.Content = "Please choose a vehicle!";
            }
            else
            {
                lbVehicleErr.Content = "";
            }
        }

        private void tbYear_LostFocus(object sender, RoutedEventArgs e)
        {
            //Validate making year
            int year = 0;
            if (!int.TryParse(tbYear.Text, out year) || year < 1950 || year > 2018)
            {
                lbYearErr.Content = "Please input a making year between 1950-2018";
            } else
            {
                lbYearErr.Content = "";
            }
        }

        private void tbBrand_LostFocus(object sender, RoutedEventArgs e)
        {
            //Validate brand
            if (tbBrand.Text.Equals(""))
            {
                lbBrandErr.Content = "Please enter your vehicle's brand!";
            }
            else
            {
                lbBrandErr.Content = "";
            }
        }

        private void tbModel_LostFocus(object sender, RoutedEventArgs e)
        {
            //Validate model
            if (tbModel.Text.Equals(""))
            {
                lbModelErr.Content = "Please enter your vehicle's model!";
            }
            else
            {
                lbModelErr.Content = "";
            }
        }

        private void cbTask_DropDownClosed(object sender, EventArgs e)
        {
            //Validate task
            if (cbTask.Text.Equals(""))
            {
                lbTaskErr.Content = "Please choose your vehicle's task!";
            }
            else
            {
                lbTaskErr.Content = "";
            }
        }

        private void cbTask_DropDownOpened(object sender, EventArgs e)
        {
            //Get a list of vehicle's tasks
            List<string> tasks = new List<string>();
            switch (cbVehicle.Text)
            {
                case nameof(Vehicles.Car):
                    tasks = GetVehicleTasks();
                    string task = "BodyTuneup";
                    tasks.Add(task);
                    break;
                case nameof(Vehicles.Bus):
                    tasks = GetVehicleTasks();
                    string task1 = "ConstantInteriorCleanup";
                    tasks.Add(task1);
                    break;
                case nameof(Vehicles.Truck):
                    tasks = GetVehicleTasks();
                    string task2 = "CoverInstallation";
                    tasks.Add(task2);
                    break;
                case nameof(Vehicles.Tractor):
                    tasks = GetVehicleTasks();
                    string task3 = "PTOMaintenance";
                    tasks.Add(task3);
                    break;
                default:
                    break;
            }
            cbTask.ItemsSource = tasks;
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            ClearAll();
            lbNotification.Content = "";
            tblSchedule.Text = "";
        }

        public void ClearAll()
        {
            cbTime.Text = "";
            cbVehicle.Text = "";
            tbYear.Text = "";
            tbBrand.Text = "";
            tbModel.Text = "";
            cbTask.Text = "";
        }

        public FileStream FileOpen(string mode)
        {
            string directory = @"..\..\Database";
            string file = "schedule.txt";
            string filepath = directory + @"\" + file;
            FileStream fs = null;
            try
            {
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                if (!File.Exists(filepath))
                {
                    fs = File.Create(filepath);
                } else
                {
                    if (mode.Equals("read"))
                    {
                        fs = File.Open(filepath, FileMode.Open, FileAccess.Read);
                    } else if (mode.Equals("write"))
                    {
                        if (numOfApps > 0)
                        {
                            fs = File.Open(filepath, FileMode.Append, FileAccess.Write);
                        } else
                        {
                            fs = File.Open(filepath, FileMode.Open, FileAccess.Write);
                            fs.SetLength(0);
                        }
                    }
                }
            }
            catch (IOException e)
            {
                Console.WriteLine(e);
            }
            return fs;
        }

        public void SaveAppointment()
        {
            FileStream fs = FileOpen("write");
            try
            {
                BinaryWriter bw = new BinaryWriter(fs);
                bw.Write(cbTime.Text);
                bw.Write(cbVehicle.Text);
                bw.Write(int.Parse(tbYear.Text));
                bw.Write(tbBrand.Text);
                bw.Write(tbModel.Text);
                bw.Write(cbTask.Text);
                bw.Close();
                numOfApps++;
                timeSlots.Remove(cbTime.Text);
                cbTime.ItemsSource = new List<string>();
                cbTime.ItemsSource = timeSlots;
                ClearAll();
                lbNotification.Foreground = Brushes.Blue;
                lbNotification.Content = "Appointment is saved successfully";
            } catch (IOException e)
            {
                Console.WriteLine(e);
            } finally
            {
                fs.Close();
            }
        }

        private void DisplaySchedule()
        {
            FileStream fs = FileOpen("read");
            Schedule schedule = new Schedule();
            try
            {
                BinaryReader br = new BinaryReader(fs);
                for (int i = 0; i < numOfApps; i++)
                {
                    string time = br.ReadString();
                    string strVehicle = br.ReadString();
                    int makingYear = br.ReadInt32();
                    string brand = br.ReadString();
                    string model = br.ReadString();
                    string task = br.ReadString();
                    Vehicle vehicle = null;
                    switch (strVehicle)
                    {
                        case nameof(Vehicles.Car):
                            vehicle = new Car(makingYear, brand, model);
                            break;
                        case nameof(Vehicles.Bus):
                            vehicle = new Bus(makingYear, brand, model);
                            break;
                        case nameof(Vehicles.Truck):
                            vehicle = new Truck(makingYear, brand, model);
                            break;
                        case nameof(Vehicles.Tractor):
                            vehicle = new Tractor(makingYear, brand, model);
                            break;
                        default:
                            break;
                    }
                    Appointment appointment = new Appointment(time, vehicle, task);
                    schedule.Add(appointment);
                }
                schedule.Sort();
                for (int i = 0; i < schedule.Count(); i++)
                {
                    tblSchedule.Text += schedule[i].ToString();
                }
                br.Close();
            }
            catch (IOException e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                fs.Close();
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            int year = 0;
            //Check if all input are correct
            if (cbTime.Text.Equals(""))
            {
                lbTimeErr.Content = "Please choose a time slot!";
            }
            else if (cbVehicle.Text.Equals(""))
            {
                lbVehicleErr.Content = "Please choose a vehicle!";
            }
            else if (!int.TryParse(tbYear.Text, out year) || year < 1950 || year > 2018)
            {
                lbYearErr.Content = "Please input a making year between 1950-2018";
            }
            else if (tbBrand.Text.Equals(""))
            {
                lbBrandErr.Content = "Please enter your vehicle's brand!";
            }
            else if (tbModel.Text.Equals(""))
            {
                lbModelErr.Content = "Please enter your vehicle's model!";
            }
            else if (cbTask.Text.Equals(""))
            {
                lbTaskErr.Content = "Please choose your vehicle's task!";
            }
            else
            {
                SaveAppointment();
            }
        }

        private void btnView_Click(object sender, RoutedEventArgs e)
        {
            tblSchedule.Text = "";
            DisplaySchedule();
        }
    }
}
