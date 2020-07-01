using System;
using TechTalk.SpecFlow;

namespace TrustedBankAutomation.Tests.Features.LoanApplication.StepDevs
{
    [Binding]
    public class CreateALoanApplicationSteps
    {
        [Given(@"I input a loan of ""(.*)"" with a yearly income of ""(.*)""")]
        public void GivenIInputALoanOfWithAYearlyIncomeOf(int p0, Decimal p1)
        {
            
        }
        
        [When(@"I launch TrustBank Page")]
        public void WhenILaunchTrustBankPage()
        {
           
        }
        
        [When(@"I sign-up with email ""(.*)"" and password ""(.*)""")]
        public void WhenISign_UpWithEmailAndPassword(string p0, string p1)
        {
            
        }
        
        [When(@"I login to a loan application")]
        public void WhenILoginToALoanApplication()
        {
           
        }
        
        [Then(@"I True for a loan fpr an Administrator to review")]
        public void ThenITrueForALoanFprAnAdministratorToReview()
        {
          
        }
    }
}
