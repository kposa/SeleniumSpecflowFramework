using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpecflowTest.HTMLReporting
{
    public static class Utilities
    {
        public static string StartTime { get; set; }
        public static string ModStartTime { get; set; }
        public static TestContext ScenarioContext { get; set; }
        public static string TestDeploymentDirectory { get; set; }
        public static string execTime { get; set; }
        public static string TestScenarioName { get; set; }
        public static string TestReportName { get; set; }
        public static string TestFileName { get; set; }
        public static List<TestStepDetails> testReport { get; set; }
        public static int TotalTCs { get; set; }
        public static string TestCaseName { get; set; }
        public static string TestCaseDescription { get; set; }
        public static string TestCaseId { get; set; }
        public static float tempStepCount = 0;
        public static float stepCount = 0;
        public static float valueToInc = 1;
        public static string resultFlag = "Pass";
        public static string Fail = "Warning";
        public static string TestResultFilePath { get; set; }
        public static string TestResultFileLocationAndName { get; set; }
        public static string EndTime { get; set; }
        public static string ModEndTime { get; set; }
        public static int FailedTests { get; set; }
        public static int PassedTests { get; set; }
        public static int DefectTests { get; set; }
        public static string dateStamp = DateTime.Now.ToString("ddMMyyyy", System.Globalization.CultureInfo.InvariantCulture);
        public static readonly string TimeStamp = System.DateTime.Now.ToString("mmddyyyyhhmmss");
        public static IWebDriver Driver { get; set; }

        public static void CaptureScreenShot()
        {
            string snapshotfilepath = "Screenshots\\" + dateStamp + "\\" + TimeStamp + "_" + ScenarioContext.Test.Name.Split('(')[0] + "_" + stepCount + ".png";
            try
            {
                if (!System.IO.Directory.Exists("Screenshots"))
                {
                    System.IO.Directory.CreateDirectory("Screenshots");
                }

                if (!System.IO.Directory.Exists("Screenshots\\" + dateStamp))
                {
                    System.IO.Directory.CreateDirectory("Screenshots\\" + dateStamp);
                }

                ITakesScreenshot ssdriver = Driver as ITakesScreenshot;
                Screenshot screenshot = ssdriver.GetScreenshot();
                screenshot.SaveAsFile(snapshotfilepath, ImageFormat.Png);
            }
            catch (FileNotFoundException ex)
            {
                //unable to take snapshots if alert is open. So Added this if condition to take the snapshot even if alert is open.
                if (ex.Message.Contains("unexpected alert open"))
                {
                    ITakesScreenshot ssdriver = Driver as ITakesScreenshot;
                    Screenshot screenshot = ssdriver.GetScreenshot();
                    screenshot.SaveAsFile(snapshotfilepath, ImageFormat.Png);
                }
                else
                {
                    ITakesScreenshot ssdriver = Driver as ITakesScreenshot;
                    Screenshot screenshot = ssdriver.GetScreenshot();
                    screenshot.SaveAsFile(snapshotfilepath, ImageFormat.Png);
                }
            }
            catch (Exception e)
            {
                //unable to take snapshots if alert is open. So Added this if condition to take the snapshot even if alert is open.
                if (e.Message.Contains("unexpected alert open"))
                {
                    ITakesScreenshot ssdriver = Driver as ITakesScreenshot;
                    Screenshot screenshot = ssdriver.GetScreenshot();
                    screenshot.SaveAsFile(snapshotfilepath, ImageFormat.Png);
                }
                else
                {
                    ITakesScreenshot ssdriver = Driver as ITakesScreenshot;
                    Screenshot screenshot = ssdriver.GetScreenshot();
                    screenshot.SaveAsFile(snapshotfilepath, ImageFormat.Png);
                }
            }
        }

        public static string GetModuleStatus()
        {
            string GetModuleStatus = "PASS";
            if (FailedTests > 0)
            {
                GetModuleStatus = "FAIL";
            }
            return GetModuleStatus;
        }
        public static string GetScenarioExecutionTime()
        {
            TimeSpan scenarioTimeDiff = DateTime.Parse(Utilities.EndTime).Subtract(DateTime.Parse(Utilities.StartTime));
            string GetScenarioExecutionTime = Math.Round(scenarioTimeDiff.TotalMinutes, 2).ToString();
            return GetScenarioExecutionTime;
        }

        public static string GetTestExecutionStatus()
        {
            string status = "PASS";
            if (testReport.Any(a => a.Status == "FAIL"))
            {
                status = "FAIL";
            }
            else if (testReport.Any(a => a.Status == "DEFECT"))
            {
                status = "DEFECT";
            }
            else
            {
                status = "PASS";
            }
            return status;
        }

        public static string GetModuleExecutionTime()
        {
            TimeSpan modTimeDiff = DateTime.Parse(Utilities.ModEndTime).Subtract(DateTime.Parse(Utilities.ModStartTime));
            string GetModuleExecutionTime = Math.Round(modTimeDiff.TotalMinutes, 2).ToString();
            return GetModuleExecutionTime;
        }

        public static string GetApplicationType()
        {
            return "Web Page";
        }

        public static string GetEnvironmentDetails()
        {
            string GetEnvironmentDetails = "Windows 7";
                //GetEnvironmentDetails = ReadConfig.GetPropertyFromConfigInUppercase(Utilities.EnvironmentDetails, Constants.URL);
            
            return GetEnvironmentDetails;
        }

    }


}
