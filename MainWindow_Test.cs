using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
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
        public void TestFileOpen()
        {
            //Test with 'read' mode
            FileStream fs = null;
            String mode = "read";
            MainWindow mainWindow = new MainWindow();
            fs = mainWindow.FileOpen(mode);
            Assert.NotNull(fs);
            Assert.True(fs.CanRead);
            Assert.False(fs.CanWrite);
            fs.Close();
            //Test with 'write' mode
            FileStream fs1 = null;
            mode = "write";
            fs1 = mainWindow.FileOpen(mode);
            Assert.NotNull(fs1);
            Assert.False(fs1.CanRead);
            Assert.True(fs1.CanWrite);
            fs1.Close();
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
        [Test]
        public void TestDisplaySchedule()
        {
            //Save appointment first to have something to read from
            MainWindow mainWindow = new MainWindow();
            mainWindow.lbNotification.Content = "";
            mainWindow.cbTime.Text = "10:00";
            mainWindow.cbVehicle.Text = "Car";
            mainWindow.tbYear.Text = "1993";
            mainWindow.tbBrand.Text = "Nissan";
            mainWindow.tbModel.Text = "Sunny";
            mainWindow.cbTask.Text = "Cleanup";
            mainWindow.SaveAppointment();
            mainWindow.tblSchedule.Text = "";
            //Call DisplaySchedule
            mainWindow.DisplaySchedule();
            //Tests
            Assert.AreEqual(mainWindow.tblSchedule.Text, "Time: 10:00, This is Car which is made in 1993 by Nissan and its model is Sunny, Task: Cleanup\n\n");
        }
        [Test]
        public void TestGo()
        {
            MainWindow mainWindow = new MainWindow();
            //There is no need to call Go() function as it automatically runs when mainWindow is created
            Assert.AreEqual(mainWindow.timeSlots.Count, 13);
            Assert.AreEqual(mainWindow.cbTime.ItemsSource, mainWindow.timeSlots);
            var vehicles = Enum.GetValues(typeof(Vehicles));
            Assert.AreEqual(vehicles.Length, 4);
            Assert.AreEqual(mainWindow.cbVehicle.ItemsSource, vehicles);
        }

        [TestCase("1940", "Please input a making year between 1950-2018")]
        [TestCase("1980", "")]
        [TestCase("2019", "Please input a making year between 1950-2018")]
        [TestCase("asd", "Please input a making year between 1950-2018")]
        public void CheckYear_InputYear_ShowExpectedErrorMessage(String year, string errorMessage)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.tbYear.Text = year;
            mainWindow.CheckYear();
            Assert.AreEqual(errorMessage, mainWindow.lbYearErr.Content);
        }

        [TestCase("Car", "OilChange", "EngineTuneup", "TransmissionCleanup", "BodyTuneup")]
        [TestCase("Bus", "OilChange", "EngineTuneup", "TransmissionCleanup", "ConstantInteriorCleanup")]
        [TestCase("Truck", "OilChange", "EngineTuneup", "TransmissionCleanup", "CoverInstallation")]
        [TestCase("Tractor", "OilChange", "EngineTuneup", "TransmissionCleanup", "PTOMaintenance")]
        public void FillInTasksList_InputVehicle_ReturnExpectedResult(String vehicle, params String[] expectedTasks)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.cbVehicle.Text = vehicle;
            mainWindow.FillInTasksList();
            Assert.AreEqual(mainWindow.cbTask.ItemsSource, expectedTasks);
        }

        [TestCase(true, "t1000", "car", "1960", "nissan", "n3", "cleanup")]
        [TestCase(false, "t2000", "", "", "1943", "", "")]
        [TestCase(false, "", "bus", "", "hyundai", "n5", "")]
        public void IsCheckedBeforeSave_InputText_ShowExpectedErrorMessage(bool expectedResult, params String[] input)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.cbTime.Text = input[0];
            mainWindow.cbVehicle.Text = input[1];
            mainWindow.tbYear.Text = input[2];
            mainWindow.tbBrand.Text = input[3];
            mainWindow.tbModel.Text = input[4];
            mainWindow.cbTask.Text = input[5];
            Assert.AreEqual(expectedResult, mainWindow.IsCheckedBeforeSave());
        }
    }
}
