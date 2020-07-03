using System;
using System.Collections.Generic;
using System.Text;
using OpenQA.Selenium;
using FluentAssertions;
using SeleniumExtras.PageObjects;
using TrustedBankAutomation.Core;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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


        /// The APPROVAL STATUS OF A CANDIDATE feild
        /// </summary>
        [CacheLookup]
        [FindsBy(How = How.CssSelector, Using = "#applications > div > div > span")]
        private IWebElement LblApprovalStatus { get; set; }

 
        /// The logout link
        /// </summary>
        [CacheLookup]
        [FindsBy(How = How.LinkText, Using = "Logout")]
        private IWebElement LinkLogOut { get; set; }


     
        /// <summary>
        /// The PASSWORD feild
        /// </summary>
        [CacheLookup]
        [FindsBy(How = How.XPath, Using = "//*[text()= 'Approve' ]")]
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
        /// <param name="currTestContext"></param>
        /// <param name="screenShotDir"></param>
        public void ApproveLoan(string userToApprove, TestContext currTestContext, string screenShotDir)
        {

                PageFactory.InitElements(BaseWebDriver, this);

                userToApprove = "\"" + userToApprove + "\"";

                var applicantCollection  =  BaseWebDriver.FindElements(By.XPath("//*[text()= " + userToApprove + " ]"));

          
                applicantCollection[0].Click();


                //Take screenshot before admin approves of a loan
                TakeScreenShot(currTestContext, screenShotDir);

                PageFactory.InitElements(BaseWebDriver, this);

                BtnApprove.Click();

                //Take screenshot after admin approves of a loan
                TakeScreenShot(currTestContext, screenShotDir);

        }


        /// <summary>
        /// Admin checks the status of a loan
        /// </summary>
        /// <param name="userToApprove"></param>

        public string CheckLoanStatus(string userToCheck)
        {

            PageFactory.InitElements(BaseWebDriver, this);

            userToCheck = "\"" + userToCheck + "\"";

            var applicantCollection = BaseWebDriver.FindElements(By.XPath("//*[text()= " + userToCheck + " ]"));

            applicantCollection[0].Click();

            PageFactory.InitElements(BaseWebDriver, this);


           return LblApprovalStatus.Text;

        }



        /// <summary>
        /// Log out the active user
        /// </summary>
        public void LogOut()
        {
            PageFactory.InitElements(BaseWebDriver, this);
            LinkLogOut.Click();
        }




    }
}



