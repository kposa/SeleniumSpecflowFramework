using NUnit.Framework;
using OpenQA.Selenium;
using SpecflowTest.Library;
using SpecflowTest.Library.Pages;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;

namespace SpecflowTest.StepDefinitions
{
    [Binding]
    public sealed class FlipkartSteps
    {
        Actions.Actions actions = new Actions.Actions();
        ResultsPage resultPage = new ResultsPage();
        HomePage homePage = new HomePage();

        Library.Lib lib = new Library.Lib();
        [Given(@"I launch Browser")]
        public void GivenILaunchBrowser()
        {
            Assert.IsNotNull(Library.Lib.Driver);
        }

        [Given(@"I navigate to ""(.*)"" web site")]
        public void GivenINavigateToWebSite(string url)
        {
            Library.Lib.Driver.Navigate().GoToUrl(new Uri(url));
        }

        [When(@"I perform search with ""(.*)""")]
        public void WhenIPerformSearchWith(string searchString)
        {
            homePage.PerformSearch(searchString);
        }

        [Then(@"The result should be displayed")]
        public void ThenTheResultShouldBeDisplayed()
        {
            Assert.IsNotNull(Library.Lib.Driver.FindElement(By.Id("searchCount")));
        }

        [When(@"I perform search and results are displayed")]
        public void WhenIPerformSearchAndResultsAreDisplayed(Table table)
        {
            foreach (var row in table.Rows)
            {
                homePage.PerformSearch(row["SearchString"]);
                Assert.IsNotNull(Library.Lib.Driver.FindElement(By.Id("searchCount")));
            }

        }

        [When(@"Click ""(.*)"" button")]
        public void WhenClickButton(string p0)
        {
            resultPage.ClickChooseFileButton();
        }

        [When(@"Enter file path and Click Open button")]
        public void WhenEnterFilePathAndClickOpenButton()
        {
            string autoItResourcePath = System.IO.Path.GetFullPath(System.IO.Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\Utilities\AutoIT"));
            string DocumntsFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string path = DocumntsFolder + "\\VerifySearchOperationIphone_Smoke_2016.xml";
            int exitCode = lib.RunExe(autoItResourcePath + "\\ImportXMLFile.exe", path);
            if (exitCode == 1)
            {
                Assert.IsTrue(true, "XML file is imported");
            }
            else
            {
                Assert.Fail("XML file is not imported");
            }
        }

        [Then(@"file should be imported")]
        public void ThenFileShouldBeImported()
        {
            System.Threading.Thread.Sleep(10000);
            resultPage.VerifyImportedFile();
        }
        [Given(@"I connect to SQL Database")]
        public void GivenIConnectToSQLDatabase()
        {
            Assert.IsNotNull(Lib.connection);
        }
        [When(@"I execute SQL Query")]
        public void WhenIExecuteSQLQuery()
        {
            string sql = "select * from Employee";
            Lib.command = new SqlCommand(sql, Lib.connection);
            Lib.command.Connection = Lib.connection;
            Lib.dataReader = Lib.command.ExecuteReader();
        }

        [Then(@"I should get the results")]
        public void ThenIShouldGetTheResults(Table table)
        {
            foreach (var row in table.Rows)
            {
                if (Lib.dataReader.Read())
                {
                    Assert.AreEqual(row["EmpId"],Lib.dataReader.GetValue(0).ToString().Trim());
                    Assert.AreEqual(row["EmpName"],Lib.dataReader.GetValue(1).ToString().Trim());
                    Assert.AreEqual(row["EmpSal"] ,Lib.dataReader.GetValue(2).ToString().Trim());
                    Assert.AreEqual(row["EmpCity"] ,Lib.dataReader.GetValue(3).ToString().Trim());
                }
            }
            
        }


    }

}

