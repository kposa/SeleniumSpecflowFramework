using NUnit.Framework;
using OpenQA.Selenium;
using SpecflowTest.Fixures;
using SpecflowTest.HTMLReporting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpecflowTest.Actions
{
    public class Actions
    {
        public void SetText(string locator,string value)
        {
            try
            {
                switch (GetElementPropertyType(locator))
             {
                    case "Name":
                        Library.Lib.Driver.FindElement(By.Name(GetElementPropertyValue(locator))).SendKeys(value);
                        
                        break;
                    case "ClassName":
                        Library.Lib.Driver.FindElement(By.ClassName(GetElementPropertyValue(locator))).SendKeys(value);
                        break;
                }
                Reporting.ReportStepDetails("Set " + value + " to " + locator, "Set " + value + " to " + locator + " success", "PASS");
            }
            catch (Exception e)
            {
                Reporting.ReportStepDetails("Set " + value + " to " + locator, "Set " + value + " to " + locator + " failed", "FAIL");
                Assert.Fail("Set Text failed to " + locator + " Exception: "+e);
            }
            
        }

        public void ClickElement(string locator)
        {
            try
            {
                switch (GetElementPropertyType(locator))
                {
                    case "ClassName":
                        Library.Lib.Driver.FindElement(By.ClassName(GetElementPropertyValue(locator))).Click();
                        break;
                    case "Id":
                        Library.Lib.Driver.FindElement(By.Id(GetElementPropertyValue(locator))).Click();
                        break;
                }
                Reporting.ReportStepDetailsWithScreenshot("Click " + locator, "Click " + locator + " success", "PASS");
            }
            catch (Exception e)
            {
                Reporting.ReportStepDetailsWithScreenshot("Click " + locator, "Click " + locator + " failed", "FAIL");
                Assert.Fail("Element click failed to " + locator + " Exception: " + e);
            }
        }

        public bool IsExists(string locator)
        {
            try
            {
                bool IsEnabled = false;
                switch (GetElementPropertyType(locator))
                {
                    case "ClassName":
                        IsEnabled = Library.Lib.Driver.FindElement(By.ClassName(GetElementPropertyValue(locator))).Enabled;
                        break;
                    case "Id":
                        IsEnabled = Library.Lib.Driver.FindElement(By.Id(GetElementPropertyValue(locator))).Enabled;
                        break;
                }
                Reporting.ReportStepDetailsWithScreenshot("Click " + locator, "Click " + locator + " success", "PASS");
                return IsEnabled;
            }
            catch (Exception e)
            {
                Reporting.ReportStepDetailsWithScreenshot("Click " + locator, "Click " + locator + " failed", "FAIL");
                Assert.Fail("Element click failed to " + locator + " Exception: " + e);
                return false;
            }
        }

        public string GetElementPropertyType(string elementLogicalName)
        {
            return elementLogicalName.Split('_')[1];
        }

        public string GetElementPropertyValue(string elementLogicalName)
        {
            return elementLogicalName.Split('_')[0];
        }
    }
}
