Feature: Create a loan application
	As a New applicant 
    I want to to be able to login
    So that I can apply for a new loan applicaion

Scenario Outline: New Applicant applies for a loan that is likely to be accepted
	When I launch TrustBank Page on <Browser>
	And I sign-up with email <Email> and password <Password>
	And As <UserType>, I access the loan application
	Given I input a loan of <LoanAmount> with a yearly income of <YearIncome>  
	Then  My application is <Sent> for an Administrator to review 
	Examples: 
	| Browser  | Email             | Password     | UserType    | LoanAmount | YearIncome                      | Sent    |
	| "Chrome" | "CreateTest1@gmail.com" | "Password1!" | "Applicant" | "£5000"    | "80,000"                        | "True"  |
	| "Chrome" | "CreateTest2@gmail.com" | "Password1!" | "Applicant" | "£40000"   | "20,000"                        | "True"  |
	| "Chrome" | "CreateTest3@gmail.com" | "Password1!" | "Applicant" | "£40000"   | "0"                             | "True"  |
	| "Chrome" | "CreateTest4@gmail.com" | "Password1!" | "Applicant" | "£5000"    | "11111111111111111111111111111" | "False" |
	| "Chrome" | "CreateTest5@gmail.com" | "Password1!" | "Applicant" | "£10000"   | "-1"                            | "False" |
	| "Chrome" | "CreateTest6@gmail.com" | "Password1!" | "Applicant" | "£10000"   | "41,000.567"                    | "False" |	