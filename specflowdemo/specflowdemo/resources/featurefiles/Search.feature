Feature: Search
	

Scenario:	 Correct product and correct price is searched.
	Given I am on the Homepage
	When Product "printed" is searched
	Then Correct Productname "Printed Chiffon Dress" is displayed
	And Correct price '$16.40' is diplayed