Feature: Create a loan application
	As a New applicant 
    I want to to be able to login
    So that I can apply for a new loan applicaion

Scenario Outline: New Applicant applies for a loan that is likely to be acceptd
	When I launch TrustBank Page
	And I sign-up with email <Email> and password <Password>
	And I login to a loan application
	Given I input a loan of <LoanAmount> with a yearly income of <YearIncome>  
	Then  I <CanApply> for a loan fpr an Administrator to review 
	Examples: 
	| Email             | Password     | LoanAmount | YearIncome                      | CanApply |
	| "Test1@gmail.com" | "Password1!" | "400"      | "80,000"                        | True     |
	| "Test2@gmail.com" | "Password1!" | "40000"    | "20,000"                        | True     |
	| "Test3@gmail.com" | "Password1!" | "40000"    | "0"                             | True     |
	| "Test4@gmail.com" | "Password1!" | "5000"     | "11111111111111111111111111111" | False    |
	| "Test5@gmail.com" | "Password1!" | "10000"    | "-1"                            | False    |
	| "Test6@gmail.com" | "Password1!" | "10000"    | "35,000.89"                     | False    |