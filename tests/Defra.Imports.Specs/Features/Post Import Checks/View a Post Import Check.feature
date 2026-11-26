@issue:62439
Feature: View a Post Import Check

@issue:62439 @acceptance-criteria:62439-1 @acceptance-criteria:62439-5
Scenario: A caseworker views a Post Import Check's related record fields
	Given I am logged in to the 'EU Imports' app as 'a caseworker'
	And I have opened a 'Post Import Check'
	Then I see the following fields
		| Field                          |
		| Related Importer Notification |
	And the following fields allow me to navigate to a related record
		| Field                          |
		| Related Importer Notification |
	And the 'Related Import Record' field is not mandatory
