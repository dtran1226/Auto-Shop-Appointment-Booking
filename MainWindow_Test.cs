using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using NUnit.Framework;

namespace PROG8145_Assignment4
{
    [TestFixture, RequiresSTA]
    public class TestMainWindow
    {
        [Test]
        public void TestExtractTimeSlots()
        {
                String timeSlot = "t1000";
                MainWindow mainWindow = new MainWindow();
                String expectedTimeSlot = mainWindow.ExtractTimeSlot(timeSlot);
                Assert.AreEqual("10:00", expectedTimeSlot);
                timeSlot = "t1340";
                expectedTimeSlot = mainWindow.ExtractTimeSlot(timeSlot);
                Assert.AreEqual("13:40", expectedTimeSlot);
        }
        [Test]
        public void TestGetVehicleTasks()
        {
            MainWindow mainWindow = new MainWindow();
            List<string> sampleList = new List<string>();
            sampleList.Add("OilChange");
            sampleList.Add("EngineTuneup");
            sampleList.Add("TransmissionCleanup");
            List<string> expectedList = mainWindow.GetVehicleTasks();
            Assert.AreEqual(expectedList.Count, 3);
            Assert.AreEqual(expectedList[1], "EngineTuneup");
            Assert.AreEqual(sampleList, expectedList);
        }
        [Test]
        public void TestClearAll()
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.cbTime.Text = "10:30";
            mainWindow.cbVehicle.Text = "car";
            mainWindow.tbYear.Text = "1993";
            mainWindow.tbBrand.Text = "nissan";
            mainWindow.tbModel.Text = "sunny";
            mainWindow.cbTask.Text = "cleanup";
            Assert.AreEqual(mainWindow.cbTime.Text, "10:30");
            Assert.AreEqual(mainWindow.cbVehicle.Text, "car");
            Assert.AreEqual(mainWindow.tbYear.Text, "1993");
            Assert.AreEqual(mainWindow.tbBrand.Text, "nissan");
            Assert.AreEqual(mainWindow.tbModel.Text, "sunny");
            Assert.AreEqual(mainWindow.cbTask.Text, "cleanup");
            mainWindow.ClearAll();
            Assert.AreEqual(mainWindow.cbTime.Text, "");
            Assert.AreEqual(mainWindow.cbVehicle.Text, "");
            Assert.AreEqual(mainWindow.tbYear.Text, "");
            Assert.AreEqual(mainWindow.tbBrand.Text, "");
            Assert.AreEqual(mainWindow.tbModel.Text, "");
            Assert.AreEqual(mainWindow.cbTask.Text, "");
        }
        [Test]
        public void TestSaveAppointment()
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.lbNotification.Content = "";
            mainWindow.cbTime.Text = "10:00";
            mainWindow.cbVehicle.Text = "car";
            mainWindow.tbYear.Text = "1993";
            mainWindow.tbBrand.Text = "nissan";
            mainWindow.tbModel.Text = "sunny";
            mainWindow.cbTask.Text = "cleanup";
            //Assert before calling the function to test
            Assert.AreEqual(mainWindow.numOfApps, 0);
            Assert.AreEqual(mainWindow.timeSlots.Count, 13);
            Assert.AreEqual(mainWindow.timeSlots[0], "10:00");
            Assert.AreNotEqual(mainWindow.lbNotification.Foreground, System.Windows.Media.Brushes.Blue);
            Assert.AreEqual(mainWindow.lbNotification.Content, "");
            //Save too file
            mainWindow.SaveAppointment();
            //Read from file
            FileStream fs = mainWindow.FileOpen("read");
            try
            {
                BinaryReader br = new BinaryReader(fs);
                string time = br.ReadString();
                string strVehicle = br.ReadString();
                int makingYear = br.ReadInt32();
                string brand = br.ReadString();
                string model = br.ReadString();
                string task = br.ReadString();
                br.Close();
                //Assert after calling the function to test
                Assert.AreEqual(time, "10:00");
                Assert.AreEqual(strVehicle, "car");
                Assert.AreEqual(makingYear, 1993);
                Assert.AreEqual(brand, "nissan");
                Assert.AreEqual(model, "sunny");
                Assert.AreEqual(task, "cleanup");
                Assert.AreEqual(mainWindow.numOfApps, 1);
                Assert.AreEqual(mainWindow.timeSlots.Count, 12);
                Assert.AreEqual(mainWindow.timeSlots[0], "10:30");
                Assert.AreEqual(mainWindow.lbNotification.Foreground, System.Windows.Media.Brushes.Blue);
                Assert.AreEqual(mainWindow.lbNotification.Content, "Appointment is saved successfully");
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
    }
}
