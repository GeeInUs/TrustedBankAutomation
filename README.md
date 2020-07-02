# Overview
To write automated tests that will run test functionality of the TrustedBank Loan Management Application. 
The TrustedBank Loan Management Application relies on a database that holds information about users and the loan application they made
Testing is specification by design. Cucumber files should be written to drive tests.

#Test Framework Stack
--SpecFlow 
.NET write BDD tests
## Selenium WEBDRIVER 
is used to drive the browser instance of the application we wish to test. The selenium server on the other is responsible for translating/interpreting SELENESE commands passed from the test program, and acts as an HTTP proxy, intercepting and verifying HTTP messages passed between the browser and the application under test. 
## Fluet Assertion 
to validate test expected conditions vs actual conditions

# Running tests
The Project is set-up testing in a data-driven approach to testing

Tests are organized into the major features with each feature having data and properties mimicking examples of how the features will work in reality.

To exemplify, the Registration Feature is further divided into Environments that hold test data that will drive testing of the feature.

The data held in each feature instructs the program how tests will be executed on a given page.

  "Registration": [

    {
      "Description": "Cancel registration",
      "Title": "Lady",
      "FName": "TestUser6",
      "LName": "TestUser6",
      "Password": "Testing6!",
      "PasswordConfirm": "Testing6!",
      "RandomizeEmail": true,
      "Email": "Test11@hotmail.com",
      "EmailConfirm": "Test11@hotmail.com",
      "ReceiveMarketUpdate": true,
      "AcceptTermsAndConditions": true,
      "CancelRegistration": true,
      "ViewPrivacyPolicy": false,
      "ViewTermsAndConditions": false,
      "SubmitRegistration": false,
      "ExpectedMessage": "Are you sure you want to cancel your registration? Your details will not be saved.",
      "ExpectedErrors": "",
      "ExpectSuccessfulRegister": false
    }


# Test Execution
I would prefer that solution in integrated to MS Azure for CI

Test files *
**\*GBI.CustomerPortal.UI.Tests.dll
!**\*TestAdapter.dll
!**\obj\**


Test results folder
$(Agent.TempDirectory)\TestResults


Test filter criteria
TestCategory=TESTENV_REGRESSION





 
