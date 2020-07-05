# Overview
To write automated tests that will run test functionality of the TrustedBank Loan Management Application. 
The TrustedBank Loan Management Application relies on a database that holds information about users and the loan application they made
Testing is specification by design. Cucumber files should be written to drive tests.


# User Stores
- All user stories are defined in MS Azure. 
- You must login as July.Test2020@outlook.com to view all related stories and work items supporting this project.
- Credentials for this user will be sent in a seperate email.
- As July.Test2020@outlook.com, you will be able to see
- Acceptance criteria for TrustedBank Loan Management Application 
-	All Work -items and Iteration to which work was done.

The work items of iterest are 

| ID             | Story                                                       |
| -------------  | ----------------------------------------------------------- |
| 3              | [Manual Test:: New Applicant signs-up for a new account](https://dev.azure.com/GeeInUs/TrustedBankTests/_workitems/edit/3/?triage=true) |
| 18             | [Manual Test:: New Applicant signs-up for a new account](https://dev.azure.com/GeeInUs/TrustedBankTests/_workitems/edit/18/?triage=true) |
| 19             | [Manual Test:: New Applicant signs-up for a new account](https://dev.azure.com/GeeInUs/TrustedBankTests/_workitems/edit/19/?triage=true) |
| 20             | [Manual Test:: New Applicant signs-up for a new account](https://dev.azure.com/GeeInUs/TrustedBankTests/_workitems/edit/20/?triage=true) |
| 21             | [Manual Test:: New Applicant signs-up for a new account](https://dev.azure.com/GeeInUs/TrustedBankTests/_workitems/edit/21/?triage=true) |
| 57             | [Manual Test:: New Applicant signs-up for a new account](https://dev.azure.com/GeeInUs/TrustedBankTests/_workitems/edit/57/?triage=true) |
| 61             | [Manual Test:: New Applicant signs-up for a new account](https://dev.azure.com/GeeInUs/TrustedBankTests/_workitems/edit/61/?triage=true) |



# Test Framework Stack
  - ## Fluet Assertion 
    * To verify Acceptance criteria
  - ## Git 
    * I am using Github for source control. Acccess to my code  is public. 
    * You can [Clone my code]( https://github.com/GeeInUs/TrustedBankAutomation. 
  - ## Selenium +  SpecFlow
    * Chrome Driver browser is used to drive TrustedBank Loan Management 
    * SpecFlow For test Specification and to drive code implementation 
    * To launch an instance of selenum server
  - ## Windows (Windows Server) 
    * IIS on server is used to host TrustedBank Aapplication
    * Has all Framework and libarary references
    * SqlServer (Management.SqlIaaSAgent) to host TrustedBank Aapplication data repositoty
    
# TEST PLAN 
  - ##  TrustedBank Loan Management Test Suites 
      * All test cases are written in [Click here to see MS Azure test cases](https://dev.azure.com/GeeInUs/TrustedBankTests/_testPlans/execute?planId=13)
   - ## [Manual Execution](http://dev.azure.com/GeeInUs/TrustedBankTests/_testManagement/analytics/progressreport)
  
      

# Test Execution
I would prefer that solution in integrated to MS Azure for CI


# Retrospective
RETROSPECTIVE
Overview
- ## User experience problems. 
	*  By default the system sets the administrator credentials. This is a security breach.
	*  No way to tell if sign-up for a loan application is successful or not.  Confirmation label help the applicant know action is completed successful/failed in submitting
	 Admin is not given the option to reject/accept an application with comments. This can be handy for applicant.
	 Would be good if Admin has paging/filter functionality to view a specific applicant or a small range of applicant.
         Applicant sees insipid UI layout when I login to  loan application using IE Browser

		- 		-
- ## Incomplete specs. 
	 As an Administrator:: Reject Functionality is missing
         Failure to meet acceptance criteria requirement that permits a user to apply after 30 days
	

- ## broken functionality
	 Yearly income input using decimals crashes the system.
	Yearly income input  of Negative amounts can be input to the system
	Invalid email criteria is not met. No defence programming here
	Invalid password complexity is not met. A user can sign-up with just spaces as password
	Sign-up with a password that has spaces can crash system
	Buffer flow exception when large yearly income is inputted.
	No warning/error messages to notify user when field is validated incorrectly


- ##  Info-sec Issues:
	Database has password in plain text. This should be encrypted.
	Admin credentials is set on redirect. Anyone can login an approve a loan
	System will permit a user with the same credentials sign up
	User Locale not considered. A French citizen with accentuated characters and different number formatting (Language and Region Settings ) can be potentially bring down database
















 
