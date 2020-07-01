Selenium-Webdriver
Test Framework: Selenium WEBDRIVER is used to drive the browser instance of the application we wish to test. The selenium server on the other is responsible for translating/interpreting SELENESE commands passed from the test program, and acts as an HTTP proxy, intercepting and verifying HTTP messages passed between the browser and the application under test.

UI Automation Prerequisite:
On the build server, euazbuild01, I have created a set of scripts that will ensure that the Selenium Server is running and an active session is maintained on the build server to run UI tests in interactive mode.

It is therefore imperative that the server is up and running prior to executing any UI tests. Please refer to an article which I have written trouble shooting failing tests discussing the need for WEBDRIVER tests running in interactive mode for tests to run successfully.

In lieu of this fact, I have created scripts that will address the need for UI tests to run interactive and also launch the selenium server for testing.

Framework: A Deeper look!
IDE – VS C#.NET MSTEST Browsers Supported – Chrome,

Running tests
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
Test Execution
I would prefer that solution in integrated to MS Azure for CI

Test files * *GBI.CustomerPortal.UI.Tests.dll !*TestAdapter.dll !**\obj**

Test results folder $(Agent.TempDirectory)\TestResults

Test filter criteria TestCategory=TESTENV_REGRESSION

Settings file \RunSettings\test.runsettings

from Developers console
lauch Developer Console in VS
CD to bin directory of ..\GBI.CustomerPortal.UI.Tests\bin\Debug
run the below command vstest.console.exe "GBI.CustomerPortal.UI.Tests.dll" /TestCaseFilter:"TestCategory=TESTENV_REGRESSION" /Settings:Tests\RunSettings\test.runsettings
from VS IDE
