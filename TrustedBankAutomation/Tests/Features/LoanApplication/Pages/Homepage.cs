using System;
using System.Collections.Generic;
using System.Text;
using OpenQA.Selenium;
using FluentAssertions;
using SeleniumExtras.PageObjects;
using TrustedBankAutomation.Core;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TrustedBankAutomation.Tests.Features.LoanApplication.Pages
{

    public class Homepage : BaseWebPage
    {

        /// <summary>
        ///  initial wait time for page to load
        /// </summary>
        private const int MAX_WAIT_PAGE_TIME = 10000;


        private const string adminUserName = "admin@trustedbank.org";
        private const string adminPassword = "abcd123";


        /// <summary>
        ///  text to  look for when inital page is loaded 
        /// </summary>
        private string HOMEPAGE_VISUAL_CUE_TEXT { get; set; }


        public struct userType
        {

            public const string Admin = "Admin";
            public const string Applicant = "Applicant";
        }

        /// The EMAIL-ADDRESS feild
        /// </summary>
        [CacheLookup]
        [FindsBy(How = How.Id, Using = "email")]
        private IWebElement TxtFeildEmailAddress { get; set; }


        /// <summary>
        /// The PASSWORD feild
        /// </summary>
        [CacheLookup]
        [FindsBy(How = How.Id, Using = "password")]
        private IWebElement TxtFieldPassword { get; set; }

        /// <summary>
        /// The login button 
        /// </summary>
        [CacheLookup]
        [FindsBy(How = How.XPath, Using = "//*[text()= \"Login\" ]")]
        private IWebElement BtnLogin { get; set; }


        /// <summary>
        /// The Sign-up button
        /// </summary>
        [CacheLookup]
        [FindsBy(How = How.XPath, Using = "//*[text()= \"Sign-Up\" ]")]
        private IWebElement BtnSignUp { get; set; }




        /// <summary>
        ///  Base constructor.opens browser, load base url and maximize page 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="serverPort"></param>
        /// <param name="browserType"></param>
        /// <param name="cueText "></param>
        public Homepage(string url, string serverPort, string browserType, string cueText = "Email address:") : base(url, browserType, serverPort)
        {
            HOMEPAGE_VISUAL_CUE_TEXT = cueText;
            WaitForPageToLoad(MAX_WAIT_PAGE_TIME, HOMEPAGE_VISUAL_CUE_TEXT);
            MaximizePage();
            PageFactory.InitElements(BaseWebDriver, this);

            BaseUrl = BaseWebDriver.Url;


        }


        /// <summary>
        ///  Signs up a new applicant to be applicable for loan registration
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <param name="currTestContext"></param>
        /// <param name="screenShotDir"></param>
        public void signUpUser(string email, string password, TestContext currTestContext, string screenShotDir )
        {
            PageFactory.InitElements(BaseWebDriver, this);

            // set the email address
            TxtFeildEmailAddress.Clear();
            TxtFeildEmailAddress.SendKeys(email);

            // set the password
            TxtFieldPassword.Clear();
            TxtFieldPassword.SendKeys(password);

            //Take screenshot before sign-up
            TakeScreenShot(currTestContext, screenShotDir);

            // signs up a new user
            BtnSignUp.Click();

        }



        /// <summary>
        ///   Login a user to the application
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <param name="userProfile"></param>
        /// <param name="currTestContext"></param>
        /// <param name="screenShotDir"></param>
        /// <returns></returns>
        public object loginUser(string email, string password, string userProfile, TestContext currTestContext, string screenShotDir)
        {
            PageFactory.InitElements(BaseWebDriver, this);

            if (userProfile == userType.Admin)
            {
                email = adminUserName;
                password = adminPassword;

            }

            // set the email address
            TxtFeildEmailAddress.Clear();
            TxtFeildEmailAddress.SendKeys(email);

            // set the password
            TxtFieldPassword.Clear();
            TxtFieldPassword.SendKeys(password);

            //Take screenshot before login
            TakeScreenShot(currTestContext, screenShotDir);

            // signs up a new user
            BtnLogin.Click();

            if (userProfile == userType.Admin)
                return new AdminPage(BaseWebDriver);
            else
                return new ApplicantPage(BaseWebDriver);

           

        }

   






    }
}



