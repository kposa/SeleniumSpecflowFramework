using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using SpecflowTest.HTMLReporting;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpecflowTest.Library
{
    public class Lib
    {
        public static SqlCommand command { get; set; }
        public static SqlDataReader dataReader { get; set; }
        public static SqlConnection connection { get; set; }
        public static IWebDriver Driver = null;
        String Driverpath = System.IO.Path.GetFullPath(System.IO.Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\Drivers"));
        Actions.Actions actions = new Actions.Actions();

        public int RunExe(string exeFilePath)
        {
            Process P = System.Diagnostics.Process.Start(exeFilePath);
            P.WaitForExit();
            return P.ExitCode;
        }

        public int RunExe(string exeFilePath, string parameters)
        {
            Process P = System.Diagnostics.Process.Start(exeFilePath, parameters);
            P.WaitForExit();
            return P.ExitCode;
        }

        public IWebDriver GetDriver(string Browser)
        {
            try
            {
                switch (Browser)
                {
                    case "chrome":
                    case "Chrome":
                        Driver = new ChromeDriver(Driverpath);
                        break;

                    case "firefox":
                    case "Firefox":
                        Driver = new FirefoxDriver();
                        break;

                    case "ie":
                    case "IE":
                        Driver = new InternetExplorerDriver(Driverpath);
                        break;
                }
                Driver.Manage().Window.Maximize();
                Utilities.Driver = Driver;
                return Driver;
            }
            catch (Exception e)
            {
                Reporting.ReportStepDetails(Browser+" driver failed", e.Message, "FAIL");
                return Driver;

            }
            
        }
    }

   
}