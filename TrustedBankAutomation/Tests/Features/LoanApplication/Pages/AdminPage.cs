


using System;
using System.Collections.Generic;
using System.Text;
using OpenQA.Selenium;
using FluentAssertions;
using SeleniumExtras.PageObjects;
using TrustedBankAutomation.Core;
using System.Runtime.InteropServices;

namespace TrustedBankAutomation.Tests.Features.LoanApplication.Pages
{

    public class AdminPage : BaseWebPage
    {

        /// <summary>
        ///  initial wait time for page to load
        /// </summary>
        private const int MAX_WAIT_PAGE_TIME = 10000;


        /// <summary>
        ///  text to  look for when inital page is loaded 
        /// </summary>
        private string APPLICATION_VISUAL_CUE_TEXT { get; set; }

        /// The EMAIL-ADDRESS feild
        /// </summary>
        [CacheLookup]
        [FindsBy(How = How.Id, Using = "emailAddress")]
        private IWebElement TxtFeildEmailAddress { get; set; }


        /// The logout link
        /// </summary>
        [CacheLookup]
        [FindsBy(How = How.LinkText, Using = "Logout")]
        private IWebElement LinkLogOut { get; set; }


        
        /// <summary>
        /// The PASSWORD feild
        /// </summary>
        [CacheLookup]
        [FindsBy(How = How.Id, Using = "btnApprove-2")]
        private IWebElement BtnApprove { get; set; }



        /// <summary>
        /// Initialize the Admin Administration page using a driver
        /// </summary>
        /// <param name="currentDriver"></param>
        /// <param name="pageVisualCue"></param>
        public AdminPage(IWebDriver currentDriver, string pageVisualCue = "Applications")
        {
            APPLICATION_VISUAL_CUE_TEXT = pageVisualCue;
            BaseWebDriver = currentDriver;

            WaitForPageToLoad(MAX_WAIT_PAGE_TIME, APPLICATION_VISUAL_CUE_TEXT);
            MaximizePage();
            PageFactory.InitElements(BaseWebDriver, this);
            BaseUrl = BaseWebDriver.Url;

        }

        /// <summary>
        /// Admin Approves of a loan
        /// </summary>
        /// <param name="userToApprove"></param>
      public void ApproveLoan(string userToApprove)
        {
            userToApprove = "\\" +  userToApprove + "\\" ;
            var applicantCollection  =  BaseWebDriver.FindElements(By.XPath("//*[text()= " + userToApprove + " ]"));

            if (applicantCollection.Count == 1)
            {
                applicantCollection[0].Click();

                BtnApprove.Click();
            }
        }


        /// <summary>
        /// Log out the active user
        /// </summary>
        public void LogOut()
        {
            LinkLogOut.Click();
        }




    }
}



