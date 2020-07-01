


using System;
using System.Collections.Generic;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using FluentAssertions;
using SeleniumExtras.PageObjects;
using TrustedBankAutomation.Core;
using System.Runtime.InteropServices;

namespace TrustedBankAutomation.Tests.Features.LoanApplication.Pages
{

    public class ApplyForLoan : BaseWebPage
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


        /// <summary>
        /// Initialize the applicant loans page using a driver
        /// </summary>
        /// <param name="currentDriver"></param>
        /// <param name="visualCue"></param>
        public ApplyForLoan(IWebDriver currentDriver, string visualCue = "My Details")
        {
            BaseWebDriver = currentDriver;
            WaitForPageToLoad(MAX_WAIT_PAGE_TIME, APPLICATION_VISUAL_CUE_TEXT);
            MaximizePage();
            PageFactory.InitElements(BaseWebDriver, this);
            BaseUrl = BaseWebDriver.Url;

        }


        /// <summary>
        ///  Applicant applies for a new loan
        /// </summary>
        /// <param name="income"></param>
        /// <param name="loanAmt"></param>
        public void applicantApply(string income, string loanAmt)
        {
            // set the yearly income
            TxtFeildYearlyIncome.Clear();
            TxtFeildYearlyIncome.SendKeys(income);

            // select the loan amount
            CmbFieldAmount.Click();
            new SelectElement(CmbFieldAmount).SelectByText(loanAmt);

            // apply for loan
            BtnApply.Click();

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



