Feature: View/Approve An Applicants Application
		As an Administrator, 
		I want to to be able to login  
		so that an I can view the status of a New Applicant's loan .

Scenario: Admin Approves a Client's Application
	When I launch TrustBank Page on "Chrome"
	And I sign-up with email "Approve134021@gmail.com" and password "Password1!"
	And As "Applicant", I access the loan application
	Given I input a loan of "£5000" with a yearly income of "80,000"
	Then My application is "True" for an Administrator to review
	When As "Applicant", I logout of the application
	And As "Admin", I access the loan application
	When I approve  "Approve134021@gmail.com" application request
	Then I expect the status of "Approve134021@gmail.com" to be "Approved"