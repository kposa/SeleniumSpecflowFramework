using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace SpecflowTest.HTMLReporting
{
    public static class Reporting
    {
        public static void ReportStepDetailsWithScreenshot(string verification, string description, string status)
        {
            Utilities.stepCount += Utilities.valueToInc;
            Utilities.CaptureScreenShot();
            // new varible is created to link snapshots to this location
            string screenshotpath = "Screenshots\\" + Utilities.dateStamp;
            Utilities.testReport.Add(new TestStepDetails
            {
                StepNumber = (float)Math.Round(Utilities.stepCount, 2, MidpointRounding.AwayFromZero),
                Verification = verification,
                Description = description,
                Status = status,
                ScreenLink = screenshotpath + "\\" + Utilities.TimeStamp + "_" + Utilities.ScenarioContext.Test.Name.Split('(')[0] + "_" + Utilities.stepCount + ".png"
                //DateTime = DateTime.Now.ToString("yyyy-MM-ddThh:mm:sszzz")
            });
            if ((status.ToUpper(System.Globalization.CultureInfo.InvariantCulture) == "FAIL" || status.ToUpper(System.Globalization.CultureInfo.InvariantCulture).Contains("DEFECT") || status.ToUpper(System.Globalization.CultureInfo.InvariantCulture).Contains("WARN")))
            {
                Utilities.resultFlag = Utilities.Fail;
                //MicrosoftAssert.Assert.IsTrue(false, "Report Generation Failed");
            }
        }
        public static void ResultsHtmlCreation()
        {
            //Location of base HTML file location
            string htmlReportBaseFileLocation = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\..\")) + @"Utilities";
            string htmlReportFileName = (Utilities.TestResultFileLocationAndName).Replace("xml", "html"); //this.TestReportName + ".html";

            string finalHtmlReportFilePath = Directory.GetCurrentDirectory();
            string sourceFile = Path.Combine(htmlReportBaseFileLocation, "ReportUI.html");
            string destFile = Path.Combine(finalHtmlReportFilePath, htmlReportFileName);

            // To copy a folder's contents to a new location:
            // Create a new target folder, if necessary.
            if (!Directory.Exists(finalHtmlReportFilePath))
            {
                Directory.CreateDirectory(finalHtmlReportFilePath);
            }
            //code to move the report file to results location
            File.Copy(sourceFile, destFile, true);

            //code to move the folder from one location to another
            //File.Copy(htmlReportBaseFileLocation, finalHtmlReportFilePath,true);

        }

        public static void ReportTestCaseExecutionStatus(TestContext scenarioContext)
        {
            if (scenarioContext.Result.Status.ToString() == "Passed")
            {
                Utilities.PassedTests += 1;
            }
            else
            {
                Utilities.FailedTests += 1;
            }
            Utilities.stepCount = Utilities.tempStepCount;
            Utilities.valueToInc = 1;
        }

        public static void AppendModuleDetailsToXml()
        {
            var moduelName = "Demo";

            /*commented out this variable for SONAR violation*/
            //var moduleStatus = (TestContext == null) ? this.ScenarioContext.Result.Status.ToString() : Utilities.ConvertToUppercase((this.TestContext.CurrentTestOutcome).ToString()).Replace("ED", string.Empty);
            try
            {
                XDocument document;
                using (var stream = File.Open(Utilities.TestResultFilePath, FileMode.Open))
                {
                    document = XDocument.Load(stream);
                }
                document.Descendants("Module").Last().Add(
                    new XElement("ModuleName", moduelName),
                    new XElement("BuildID", "NA"),
                    new XElement("OS", "Windows 7"),
                    new XElement("HostName", Environment.MachineName),
                    new XElement("Browser", ConfigurationManager.AppSettings["BrowserName"]),
                    new XElement("ExecutedBy", Environment.UserName),
                    new XElement("AutomationTool", "*Specflow*"),
                    new XElement("ModuleStatus", Utilities.GetModuleStatus()),
                    new XElement("ModuleStartTime", Utilities.ModStartTime),
                    new XElement("ModuleEndTime", Utilities.ModEndTime),
                    new XElement("TotalScenarios", "    " + Utilities.TotalTCs),
                    new XElement("PassedScenarios", "    " + Utilities.PassedTests),
                    new XElement("FailedScenarios", "    " + (Utilities.FailedTests + Utilities.DefectTests)),
                    new XElement("IncompleteScenarios", "    " + (Utilities.TotalTCs - (Utilities.PassedTests + Utilities.FailedTests + Utilities.DefectTests)))
                    );
                document.Save(Utilities.TestResultFilePath);
            }
            catch (XmlException e)
            {
                ReportStepDetails("Failed to create XML report", e.Message, Utilities.Fail);
            }
            catch (InvalidOperationException e)
            {
                ReportStepDetails("Failed to create XML report", e.Message, Utilities.Fail);
            }
            catch (Exception e)
            {
                ReportStepDetails("Failed to create XML report", e.Message, Utilities.Fail);
            }
        }

        public static void ScenarioContextConfigurations()
        {
            Utilities.TestDeploymentDirectory = TestContext.CurrentContext.TestDirectory;
            Utilities.execTime = DateTime.Now.ToString("yyyy-MM-dd_hh-mm-ss", System.Globalization.CultureInfo.InvariantCulture);
            string fullyQualTestClassName = TestContext.CurrentContext.Test.FullName;
            string testClassName = fullyQualTestClassName.Split('(')[0].Split('.')[fullyQualTestClassName.Split('(')[0].Split('.').Length - 1];
            Utilities.TestScenarioName = testClassName;
            Utilities.TestReportName = Utilities.TestScenarioName + "_" + Utilities.execTime;
            if (Utilities.TestFileName == null)
            {
                Utilities.TestFileName = Utilities.TestReportName;
            }
            Utilities.testReport = new List<TestStepDetails>();
        }

        public static void ReportTestCaseDetails(TestContext scenarioContext)
        {
            Utilities.TotalTCs += 1;
            string testMethodName = scenarioContext.Test.Name;
            Utilities.TestCaseName = testMethodName;
            Utilities.TestCaseDescription = SplitCamelCase(Utilities.TestCaseName);
            Utilities.TestCaseId = "TC_" + Utilities.TestScenarioName + "_" + Utilities.TotalTCs;
            Utilities.tempStepCount = Utilities.stepCount;
            Utilities.valueToInc = 1F;
        }

        public static string SplitCamelCase(string input)
        {
            string output = System.Text.RegularExpressions.Regex.Replace(input, "(?<!^)([A-Z][a-z]|(?<=[a-z])[A-Z])", " $1", System.Text.RegularExpressions.RegexOptions.Compiled).Trim();
            return output;
        }

        public static void ReportStepDetails(string verification, string description, string status)
        {
            Utilities.stepCount += Utilities.valueToInc;
            Utilities.testReport.Add(new TestStepDetails
            {
                StepNumber = (float)Math.Round(Utilities.stepCount, 2, MidpointRounding.AwayFromZero),
                Verification = verification,
                Description = description,
                Status = status,
                DateTime = DateTime.Now.ToString("yyyy-MM-ddThh:mm:sszzz", System.Globalization.CultureInfo.InvariantCulture)
            });
            if (status.ToUpper(System.Globalization.CultureInfo.InvariantCulture) == "FAIL")
            {
                Utilities.resultFlag = Utilities.Fail;
                //MicrosoftAssert.Assert.IsTrue(false, "Report Generation Failed");
            }
        }

        public static string ConvertToUppercase(string testExecutionType)
        {
            return testExecutionType.ToUpper(System.Globalization.CultureInfo.InvariantCulture);
        }
        public static void ResultXmlFileCreation()
        {
            string FullyQualifiedTestClassName = Utilities.ScenarioContext.Test.Name.Split('(')[0];
            string TestName = FullyQualifiedTestClassName;
            string CurrentTestOutcome = ConvertToUppercase(Utilities.ScenarioContext.Result.Status.ToString()).Replace("ED", string.Empty);

            //creating an instance of the XML Document
            XDocument document;
            XElement tagRegistry = null;

            object c = from testStepReport in Utilities.testReport
                       select new XElement("Step", new XAttribute("id", testStepReport.StepNumber),
                           new XElement("Description", testStepReport.Description),
                           new XElement("Action", testStepReport.Verification),
                           new XElement("Status", ConvertToUppercase(testStepReport.Status)),
                           new XElement("ScreenLink", testStepReport.ScreenLink));

            Utilities.TestResultFilePath = Utilities.TestFileName + ".xml";
            // assigning the path to a utility variable so that it could be used in Send Email
            Utilities.TestResultFileLocationAndName = Utilities.TestResultFilePath;
            if (File.Exists(Utilities.TestResultFilePath))
            {
                using (var stream = File.Open(Utilities.TestResultFilePath, FileMode.Open))
                {
                    document = XDocument.Load(stream);
                }
                tagRegistry = document.Descendants("Scenario").FirstOrDefault();
            }
            else
            {
                document = new XDocument(new XDeclaration("1.0", "ISO-8859-1", "yes"));
            }

            if (tagRegistry == null)
            {
                tagRegistry = new XElement("Project",
                       new XElement("ApplicationName", "Flipkart Demo"),
                       new XElement("ApplicationType", Utilities.GetApplicationType()),
                       new XElement("TestEnvironment", "QA"),
                       new XElement("TestSuiteType", "REGRESSION TESTING"),
                       new XElement("ExecutionDate", Utilities.StartTime),
                       new XElement("ExecutionTime", Utilities.EndTime));
                document.Add(tagRegistry);
                // adding modules
                XElement moduletag = new XElement("Modules",
                new XElement("Module", new XAttribute("id", FullyQualifiedTestClassName),
                    new XElement("Scenarios", new XElement("Scenario", new XAttribute("id", TestName),
                    new XElement("Description", Utilities.TestCaseDescription),
                    new XElement("StartTime", Utilities.ModStartTime),
                    new XElement("EndTime", Utilities.ModEndTime),
                    new XElement("Duration", Utilities.GetModuleExecutionTime() + " min"),
                    new XElement("Status", ConvertToUppercase(Utilities.GetTestExecutionStatus()).Replace("ED", string.Empty)),
                    new XElement("Steps", c)))));
                tagRegistry.Add(moduletag);
            }
            else
            {
                XElement newmoduletag = new XElement("Scenario",
                   new XAttribute("id", TestName),
                   new XElement("Description", Utilities.TestCaseDescription),
                   new XElement("StartTime", Utilities.StartTime),
                   new XElement("EndTime", Utilities.EndTime),
                   new XElement("Duration", Utilities.GetScenarioExecutionTime() + " min"),
                   new XElement("Status", ConvertToUppercase((CurrentTestOutcome)).Replace("ED", string.Empty)),
                   new XElement("Steps", c));
                document.Descendants("Scenarios").Single().Add(newmoduletag);
            }
            System.Diagnostics.Debug.Write("Test results file path:" + Utilities.TestResultFilePath);
            document.Save(Utilities.TestResultFilePath);
        }
    }

    public class TestStepDetails
    {
        /// <summary>
        /// Gets or sets the step number.
        /// </summary>
        /// <value>
        /// The step number.
        /// </value>
        public float StepNumber { get; set; }

        /// <summary>
        /// Gets or sets the verification.
        /// </summary>
        /// <value>
        /// The verification.
        /// </value>
        public string Verification { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the ScreenLink.
        /// </summary>
        /// <value>
        /// The ScreenLink.
        /// </value>
        public string ScreenLink { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the date time.
        /// </summary>
        /// <value>
        /// The date time.
        /// </value>
        public string DateTime { get; set; }
    }
}
