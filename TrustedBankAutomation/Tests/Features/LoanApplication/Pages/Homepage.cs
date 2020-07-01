


using System;
using System.Collections.Generic;
using System.Text;
using OpenQA.Selenium;
using FluentAssertions;
using SeleniumExtras.PageObjects;
using TrustedBankAutomation.Core;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;

namespace TrustedBankAutomation.Tests.Features.LoanApplication.Pages
{

    public class Homepage : BaseWebPage
    {

      

        /// <summary>
        ///  initial wait time for page to load
        /// </summary>
        private const int MAX_WAIT_PAGE_TIME = 10000;


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
        public void signUpUser(string email, string password )
        {
            PageFactory.InitElements(BaseWebDriver, this);

            // set the email address
            TxtFeildEmailAddress.Clear();
            TxtFeildEmailAddress.SendKeys(email);

            // set the password
            TxtFieldPassword.Clear();
            TxtFieldPassword.SendKeys(email);

            // signs up a new user
            BtnSignUp.Click();

        }





        /// <summary>
        ///   Login a user to the application
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <param name="userProfile"></param>
        /// <returns></returns>
        public object loginUser(string email, string password, string userProfile)
        {
            PageFactory.InitElements(BaseWebDriver, this);

            // set the email address
            TxtFeildEmailAddress.Clear();
            TxtFeildEmailAddress.SendKeys(email);

            // set the password
            TxtFieldPassword.Clear();
            TxtFieldPassword.SendKeys(email);

            // signs up a new user
            BtnLogin.Click();

            if (userProfile == userType.Admin)
                return new AdminPage(BaseWebDriver);
            else
                return new ApplicantPage(BaseWebDriver);

        }

   






    }
}



