


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

        /// The EMAIL-ADDRESS feild
        /// </summary>
        [CacheLookup]
        [FindsBy(How = How.Id, Using = "emailAddress")]
        private IWebElement TxtFeildEmailAddress { get; set; }


        /// <summary>
        /// The PASSWORD feild
        /// </summary>
        [CacheLookup]
        [FindsBy(How = How.Id, Using = "loginPassword")]
        private IWebElement TxtFieldPassword { get; set; }

        /// <summary>
        /// The PASSWORD  RESET feild
        /// </summary>
        [CacheLookup]
        [FindsBy(How = How.CssSelector, Using = "input.btn.reset-password.ai-tracker")]
        private IWebElement BtnPasswordReset { get; set; }



        /// <summary>
        ///  Base constructor.opens browser, load base url and maximize page 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="serverPort"></param>
        /// <param name="browserType"></param>
        /// <param name="cueText "></param>
        public Homepage(string url, string serverPort, string browserType, string cueText = "") : base(url, browserType, serverPort)
        {
            HOMEPAGE_VISUAL_CUE_TEXT = cueText;
            WaitForPageToLoad(MAX_WAIT_PAGE_TIME, HOMEPAGE_VISUAL_CUE_TEXT);
            MaximizePage();
            PageFactory.InitElements(BaseWebDriver, this);

            BaseUrl = BaseWebDriver.Url;


        }


        /// <summary>
        /// reloads the Homepage and initializes cached page objects
        /// </summary>
        private void ReloadPage()
        {
            NavigateHome();

            PageFactory.InitElements(BaseWebDriver, this);
        }






    }
}



