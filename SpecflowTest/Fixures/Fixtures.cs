using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using SpecflowTest.HTMLReporting;
using SpecflowTest.Library;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
//using MSUnitTesting = Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnitFramework = NUnit.Framework;

namespace SpecflowTest.Fixures
{
    [Binding]
    public class Fixtures
    {
        public static IWebDriver Driver;

        [BeforeFeature("FlipKartTests")]
        public static void BeforeFeature()
        {
            try
            {
                string BrowserName = ConfigurationManager.AppSettings["BrowserName"];
                Lib Library = new Lib();
                Library.GetDriver(BrowserName);
            }
            catch (Exception e)
            {
                Reporting.ReportStepDetails("Exception Verification", e.Message, "FAIL");
                Reporting.ResultXmlFileCreation();
                Reporting.AppendModuleDetailsToXml();
                Reporting.ResultsHtmlCreation();
            }
        }

        [BeforeScenario("DBTests")]
        public static void BeforeScenarioDB()
        {
            try
            {
                var scenarioContext = NUnitFramework.TestContext.CurrentContext;

                //to capture Start time for each scenario
                Utilities.StartTime = DateTime.Now.ToString("HH:mm:ss tt");
                //to capture Start time of the Module
                Utilities.ModStartTime = DateTime.Now.ToString("HH:mm:ss tt");

                Utilities.ScenarioContext = scenarioContext;
                Reporting.ScenarioContextConfigurations();
                Reporting.ReportTestCaseDetails(scenarioContext);
                try
                {
                    string connetionString = null;

                    connetionString = "data source=.; database=Sree; integrated security=SSPI";
                    Lib.connection = new SqlConnection(connetionString);
                    Lib.connection.Open();

                }
                catch (Exception ex)
                {
                    Console.WriteLine("DB Connection Failed");
                }

            }
            catch (Exception)
            {
                
            }
        }

        [BeforeScenario("FlipKartTests")]
        public static void BeforeScenarioWeb()
        {
            try
            {
                var scenarioContext = NUnitFramework.TestContext.CurrentContext;

                //to capture Start time for each scenario
                Utilities.StartTime = DateTime.Now.ToString("HH:mm:ss tt");
                //to capture Start time of the Module
                Utilities.ModStartTime = DateTime.Now.ToString("HH:mm:ss tt");

                Utilities.ScenarioContext = scenarioContext;
                Reporting.ScenarioContextConfigurations();
                Reporting.ReportTestCaseDetails(scenarioContext);


            }
            catch (Exception)
            {

            }
        }

        [AfterScenario("DBTests")]
        [AfterScenario("FlipKartTests")]
        public static void AfterScenario()
        {
            try
            {
                Utilities.EndTime = DateTime.Now.ToString("HH:mm:ss tt");

                // calculating the time difference between Start and End time of Module
                Utilities.ModEndTime = DateTime.Now.ToString("HH:mm:ss tt");

                var scenarioContext = NUnitFramework.TestContext.CurrentContext;
                Reporting.ReportTestCaseExecutionStatus(scenarioContext);
                Reporting.ResultXmlFileCreation();
                if (Lib.dataReader!=null)
                {
                    Lib.dataReader.Close();
                }
                if (Lib.command != null)
                {
                    Lib.command.Dispose();
                }
                if (Lib.connection != null)
                {
                    Lib.connection.Close();
                }
                
                
                
            }
            catch (Exception)
            {
                
            }
        }
        [AfterFeature("DBTests")]
        [AfterFeature("FlipKartTests")]
        public static void AfterFeature()
        {
            Reporting.AppendModuleDetailsToXml();
            Reporting.ResultsHtmlCreation();
            Library.Lib.Driver.Quit();
        }
    }
}
