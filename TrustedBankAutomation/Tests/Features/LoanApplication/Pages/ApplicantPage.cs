using System;
using System.Collections.Generic;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using FluentAssertions;
using SeleniumExtras.PageObjects;
using TrustedBankAutomation.Core;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;

namespace TrustedBankAutomation.Tests.Features.LoanApplication.Pages
{

    public class ApplicantPage : BaseWebPage
    {

        /// <summary>
        ///  initial wait time for page to load
        /// </summary>
        private const int MAX_WAIT_PAGE_TIME = 10000;


        /// <summary>
        ///  text to  look for when inital page is loaded 
        /// </summary>
        private string APPLICATION_VISUAL_CUE_TEXT { get; set; }

        /// The yearly income feild
        /// </summary>
        [CacheLookup]
        [FindsBy(How = How.Id, Using = "txtIncome")]
        private IWebElement TxtFeildYearlyIncome { get; set; }


        /// <summary>
        /// The amount of loan feild
        /// </summary>
        [CacheLookup]
        [FindsBy(How = How.Id, Using = "cmbAmount")]
        private IWebElement CmbFieldAmount { get; set; }

        /// <summary>
        /// The apply button
        /// </summary>
        [CacheLookup]
        [FindsBy(How = How.CssSelector, Using = "#application > div.float-right > button")]
        private IWebElement BtnApply { get; set; }

        /// The logout link
        /// </summary>
        [CacheLookup]
        [FindsBy(How = How.LinkText, Using = "Logout")]
        private IWebElement LinkLogOut { get; set; }

        /// The STATUS OF A CANDIDATE feild
        /// </summary>
        [CacheLookup]
        [FindsBy(How = How.CssSelector, Using = "#application > div > div:nth-child(4)")]
        private IWebElement LblCandidateStatus { get; set; }


        /// <summary>
        /// holds a Collection of screenshot filenames per test step 
        /// </summary>
        public ArrayList ScreenshotFileCollnt { get; set; }


        /// <summary>
        /// Initialize the applicant loans page using a driver
        /// </summary>
        /// <param name="currentDriver"></param>
        /// <param name="visualCue"></param>
        public ApplicantPage(IWebDriver currentDriver, string visualCue = "My Details")
        {
            APPLICATION_VISUAL_CUE_TEXT = visualCue;
             BaseWebDriver = currentDriver;
            ScreenshotFileCollnt = null;
            ScreenshotFileCollnt = new ArrayList();
            WaitForPageToLoad(MAX_WAIT_PAGE_TIME, APPLICATION_VISUAL_CUE_TEXT);
            MaximizePage();
            PageFactory.InitElements(BaseWebDriver, this);
            BaseUrl = BaseWebDriver.Url;

        }

        /// <summary>
        /// Applicant applies for a new loan
        /// </summary>
        /// <param name="income"></param>
        /// <param name="loanAmt"></param>
        /// <param name="currTestContext"></param>
        /// <param name="screenShotDir"></param>
        /// <returns>True if application was sent successfully, false otherwise</returns>
        public bool applicantApply(string income, string loanAmt, 
            TestContext currTestContext, string screenShotDir)
        {
            bool appliedSuccessfully = false;
            try
            {
                // set the yearly income
                TxtFeildYearlyIncome.Clear();
                TxtFeildYearlyIncome.SendKeys(income);

                // select the loan amount
                CmbFieldAmount.Click();
                new SelectElement(CmbFieldAmount).SelectByText(loanAmt);


                //Take screenshot before applicant applies for loan
                var fileName = TakeScreenShot(currTestContext, screenShotDir);

                // store filename
                if (fileName != null) ScreenshotFileCollnt.Add(fileName);

                // apply for loan
                BtnApply.Click();

                // Take screenshot after applicant applies for loan
                fileName = TakeScreenShot(currTestContext, screenShotDir);

                // store filename
                if (fileName != null) ScreenshotFileCollnt.Add(fileName);

                appliedSuccessfully = true; 

            } catch(Exception) {
                appliedSuccessfully = false;
            }

            return appliedSuccessfully;

}



        /// <summary>
        /// Returns status of a loan
        /// </summary>
        /// <param name="currTestContext"></param>
        /// <param name="screenShotDir"></param>
        public string GetApplicantStatus( TestContext currTestContext, string screenShotDir)
        {
            try
            {

                Refresh();

                PageFactory.InitElements(BaseWebDriver, this);

                //Take screenshot after admin approves of a loan
                var fileName = TakeScreenShot(currTestContext, screenShotDir);

                // store filename
                if (fileName != null) ScreenshotFileCollnt.Add(fileName);

            }
            catch (Exception) { }

            return  (LblCandidateStatus != null & LblCandidateStatus.Displayed ) ? LblCandidateStatus.Text.Split("\"")[1] :"";

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



