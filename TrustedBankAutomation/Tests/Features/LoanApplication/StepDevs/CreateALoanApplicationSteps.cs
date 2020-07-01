using System;
using TechTalk.SpecFlow;
using System.IO;
using TrustedBankAutomation.Tests.Features.LoanApplication.Pages;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TrustedBankAutomation.Tests.Features.LoanApplication.StepDevs
{
    [Binding]

    public class CreateALoanApplicationSteps
    {

        /// <summary>
        /// The login directory for screenshots
        /// </summary>
        private static string screenShotDir { get; set; }



        /// <summary>
        ///  selenium server listening port
        /// </summary>
        private static string seleniumPort { get; set; }

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
        ///  Testing application url
        /// </summary>
        private string baseUrl { get; set; }

        /// <summary>
        ///  The email address of the the user that just signed-up
        /// </summary>
        private string currUserEmail { get; set; }


        /// <summary>
        ///  The password of the the user that just signed-up
        /// </summary>
        private string currUserPassword { get; set; }

        /// <summary>
        ///  If the applicant submitted application for loan 
        /// </summary>
        private bool applicationSentOk { get; set; }


        /// <summary>
        ///  Scenario context
        /// </summary>
        private ScenarioContext scenarioContext { get; set; }

        /// <summary>
        ///  Feature context
        /// </summary>
        private FeatureContext featureContext { get; set; }


        /// <summary>
        ///  Test context
        /// </summary>
        private TestContext testContext { get; set; }

        /// <summary>
        /// Initialize by context injection
        /// </summary>
        /// <param name="_scenarioContext"></param>
        /// <param name="_featureContext"></param>

        public CreateALoanApplicationSteps(ScenarioContext _scenarioContext, FeatureContext _featureContext)
        {
            scenarioContext = _scenarioContext;

            featureContext = _featureContext;

            testContext = scenarioContext.ScenarioContainer.Resolve<Microsoft.VisualStudio.TestTools.UnitTesting.TestContext>();

            Init();
        }

        [AfterFeature]
        public static void Dereference()
        {
            if (pageObjectHomePage != null)
            {
                pageObjectHomePage.ReleaseUsedReferences();
                pageObjectHomePage = null;
            }

        }

        /// <summary>
        /// Initializes Test run settings
        /// </summary>
        private void Init()
        {
         
            screenShotDir = Directory.GetCurrentDirectory() + testContext.Properties["screenshot.dir"];

            seleniumPort =  ( testContext.Properties["server.comm.port"]).ToString() ;

            baseUrl =  (testContext.Properties["ui.base.url"]).ToString();

            if (!Directory.Exists(screenShotDir))
                Directory.CreateDirectory(screenShotDir);


        }



        [Given(@"I input a loan of ""(.*)"" with a yearly income of ""(.*)""")]
        public void GivenIInputALoanOfWithAYearlyIncomeOf(string loanAmt, string incomeAmt)
        {
            applicationSentOk = pageObjectApplicantPage.applicantApply(incomeAmt, loanAmt);
        }
        
        [When(@"I launch TrustBank Page on ""(.*)""")]
        public void WhenILaunchTrustBankPage(string browserType)
        {
            if (pageObjectHomePage == null)
                pageObjectHomePage = new Homepage(baseUrl, seleniumPort, browserType);
            else
                pageObjectHomePage.NavigateHome();

        }
        

        [When(@"I sign-up with email ""(.*)"" and password ""(.*)""")]
        public void WhenISign_UpWithEmailAndPassword(string email, string password)
        {
            currUserEmail = email;
            currUserPassword = password;

            //sign-up the user
            pageObjectHomePage.signUpUser(currUserEmail, currUserPassword);

        }

        [When(@"As a ""(.*)"", I access the loan application")]
        public void WhenILoginToALoanApplication(string userType)
        {
          
            if (userType == Homepage.userType.Admin)
                pageObjectAdminPage = (AdminPage) (pageObjectHomePage.loginUser(currUserEmail, currUserPassword, userType));

            else if (userType == Homepage.userType.Applicant)
                pageObjectApplicantPage = (ApplicantPage) pageObjectHomePage.loginUser(currUserEmail, currUserPassword, userType);

        }
        
        [Then(@"I ""(.*)"" for a loan for an Administrator to review")]
        public void ThenITrueForALoanFprAnAdministratorToReview(string canApply)
        {
            applicationSentOk.Should().Equals( bool.Parse( canApply) );
        }
    }
}
