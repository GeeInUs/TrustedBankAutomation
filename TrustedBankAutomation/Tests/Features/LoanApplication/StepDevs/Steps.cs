﻿using System;
using TechTalk.SpecFlow;
using System.IO;
using TrustedBankAutomation.Tests.Features.LoanApplication.Pages;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using  TrustedBankAutomation.Core ;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Reflection;
using System.Collections;

namespace TrustedBankAutomation.Tests.Features.LoanApplication.StepDevs
{
    [Binding]

    public class Steps
    {

        /// <summary>
        /// The login directory for screenshots
        /// </summary>
        private static string screenShotDir { get; set; }



        /// <summary>
        ///  selenium server listening port
        /// </summary>
        private static string seleniumPort {get;set;}

        /// <summary>
        /// Instance of homepage reference
        /// </summary>
        private static Homepage pageObjectHomePage { get; set; }

        /// <summary>
        /// Instance of Admin page reference
        /// </summary>
        private static AdminPage pageObjectAdminPage { get; set; }

        /// <summary>
        /// Instance of Applicant page reference
        /// </summary> 
        private static ApplicantPage pageObjectApplicantPage { get; set; }

        /// <summary>
        ///Holds necessary information to generate reports
        /// </summary>
        private static Reporting reporting { get; set; }



        /// <summary>
        ///  Testing application url
        /// </summary>
        private static  string baseUrl { get; set; }

    /// <summary>
    ///  The email address of the the user that just signed-up
    /// </summary>
    private string currUserEmail { get; set; }


        /// <summary>
        ///  The password of the the user that just signed-up
        /// </summary>
        private string currUserPassword { get; set; }


        /// <summary>
        ///  Scenario context
        /// </summary>
        public static ScenarioContext scenarioContext { get; set; }

        /// <summary>
        ///  Feature context
        /// </summary>
        public static FeatureContext featureContext { get; set; }



        /// <summary>
        ///  Test context
        /// </summary>
        public static TestContext testContext { get; set; }

        /// <summary>
        /// Initialize by context injection
        /// </summary>
        /// <param name="_scenarioContext"></param>
        /// <param name="_featureContext"></param>

   
        public   Steps(ScenarioContext _scenarioContext, FeatureContext _featureContext)
        {
            scenarioContext = _scenarioContext;
            featureContext = _featureContext;
            testContext = scenarioContext.ScenarioContainer.Resolve<Microsoft.VisualStudio.TestTools.UnitTesting.TestContext>();

            InitTestProperties();
            InitReportProperties();



        }



        private static string ProductName
        {
            get
            {
                AssemblyProductAttribute myProduct =(AssemblyProductAttribute)AssemblyProductAttribute.GetCustomAttribute(Assembly.GetExecutingAssembly(),
                                 typeof(AssemblyProductAttribute));

                return myProduct.Product;
            }
        }
        /// <summary>
        /// Initializes Report run settings
        /// </summary>
        public  void InitReportProperties()
        {

            var reportDirectory = Directory.GetCurrentDirectory() + testContext.Properties["reports.dir.name"].ToString();

            if (! Directory.Exists(reportDirectory))
                 Directory.CreateDirectory(reportDirectory);
           
            reporting = new Reporting();

            reporting.BDDReports.ReportScenarioContext = scenarioContext;

            reporting.BDDReports.ReportFeatureContext = featureContext; 

            reporting.BDDReports.ReportFilePath = reportDirectory + ProductName + "\\" + featureContext.FeatureInfo.Title  + ".html";

            reporting.BDDReports.ReportTitle = "SpecFlow " + ProductName + " Reports"; ;

            reporting.BDDReports.ReportDocName = ProductName + " Document";

            reporting.InitReporting();

           
        }



        /// <summary>
        /// Initializes Test run settings
        /// </summary>
        public  static void InitTestProperties()
        {
            baseUrl = testContext.Properties["ui.base.url"].ToString();

            seleniumPort =  testContext.Properties["server.comm.port"].ToString();
           
            screenShotDir = Directory.GetCurrentDirectory() + testContext.Properties["screenshot.dir"].ToString();

            // create new screenshot directory
            if (! Directory.Exists(screenShotDir))
                Directory.CreateDirectory(screenShotDir);

        }



        [Given(@"I input a loan of ""(.*)"" with a yearly income of ""(.*)""")]
        public void GivenIInputALoanOfWithAYearlyIncomeOf(string loanAmt, string incomeAmt)
        {
              pageObjectApplicantPage.applicantApply(incomeAmt,
                loanAmt, 
                testContext,
                screenShotDir);

           
        }
        
        [When(@"I launch TrustBank Page on ""(.*)""")]
        public void WhenILaunchTrustBankPage(string browserType)
        {
            if (pageObjectHomePage == null)
                pageObjectHomePage = new Homepage(baseUrl, seleniumPort, browserType);
            else
                pageObjectHomePage.NavigateHome();

            // Take screenshot upon page launch
            pageObjectHomePage.TakeScreenShot(testContext, screenShotDir);
        }
        

        [When(@"I sign-up with email ""(.*)"" and password ""(.*)""")]
        public void WhenISign_UpWithEmailAndPassword(string email, string password)
        {
            currUserEmail = email;
            currUserPassword = password;

            //sign-up the user
            pageObjectHomePage.signUpUser(currUserEmail, currUserPassword, testContext, screenShotDir);

        }

        [When(@"As ""(.*)"", I access the loan application")]
        public void WhenILoginToALoanApplication(string userType)
        {
          
            if (userType == Homepage.userType.Admin)
                pageObjectAdminPage = (AdminPage) (pageObjectHomePage.loginUser(currUserEmail, currUserPassword, userType, testContext, screenShotDir));

            else if (userType == Homepage.userType.Applicant)
                pageObjectApplicantPage = (ApplicantPage) pageObjectHomePage.loginUser(currUserEmail, currUserPassword, userType, testContext, screenShotDir); ;

        }
      
        [Then(@"My application is ""(.*)"" for an Administrator to review")]
        public void ThenITrueForALoanFprAnAdministratorToReview(string exptedStatus)
        {

            var actualStatus = pageObjectApplicantPage.GetApplicantStatus(testContext, screenShotDir);

            exptedStatus.Should().Equals(actualStatus);
        }

        [When(@"As ""(.*)"", I logout of the application")]
        public void WhenILogoutOfTheApplication(string userType)
        {
            if (userType == Homepage.userType.Admin)
                pageObjectAdminPage.LogOut();

            else if (userType == Homepage.userType.Applicant)
                pageObjectApplicantPage.LogOut();

        }


        [When(@"I approve  ""(.*)"" application request")]
        public void WhenIApproveApplicationRequest(string email)
        {
            pageObjectAdminPage.ApproveLoan(email, testContext, screenShotDir);
        }


        [Then(@"I expect the status of ""(.*)"" to be ""(.*)""")]
        public void ThenIExpectTheStatusOfToBe(string email, string status)
        {
            status.Should().Equals(pageObjectAdminPage.CheckLoanStatus(email, testContext, screenShotDir));
        }




        /// <summary>
        /// Log each steps screenshot to reporter class
        /// </summary>
        /// <param name="objCollection"></param>
        private static void LogReportStatus(ArrayList objCollection)
        {
            if (objCollection != null && objCollection.Count >= 1)
            {
                foreach (string fname in objCollection)
                    reporting.LogStatus(fname);

                objCollection.Clear();
            }
        }


        [AfterStep]
        public static void AfterEveryStep(ScenarioContext ScenarioContext, FeatureContext ReportFeatureContext)
        {
            reporting.BDDReports.ReportScenarioContext = ScenarioContext;

            reporting.BDDReports.ReportFeatureContext = ReportFeatureContext;

            if (pageObjectAdminPage != null)  LogReportStatus(pageObjectAdminPage.ScreenshotFileCollnt);

            if (pageObjectApplicantPage != null) LogReportStatus(pageObjectApplicantPage.ScreenshotFileCollnt);

            if (pageObjectHomePage != null) LogReportStatus(pageObjectHomePage.ScreenshotFileCollnt);

        }



        [AfterFeature]
         public static void ReportIt()
            {
            try
            {
                if (reporting != null)
                    reporting.Close();
            }
            catch (Exception) { }
            finally
            {
                reporting = null;
                   
            }
        }
        
       
         [AfterTestRun]
        public static void Dereference()
        {
            try
            {
                if (pageObjectHomePage != null)
                {
                    pageObjectHomePage.ReleaseUsedReferences();
                    pageObjectHomePage.KillHost();
                    pageObjectHomePage = null;
                }
            }
            catch (Exception) { }
            finally {
               if ( reporting!= null)
                   reporting.Close();
            }

        }


    }
}
