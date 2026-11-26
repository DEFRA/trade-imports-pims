@issue:62439
Feature: Create a Post Import Check

@issue:62439 @acceptance-criteria:62439-3 @acceptance-criteria:62439-4
Scenario: A caseworker creates a Post Import Check from an Importer Notification
	Given I am logged in to the 'EU Imports' app as 'a caseworker'
	And I have opened an Importer Notification
	And I have selected the 'Post Import Checks' tab
	When I click to create a new record on the 'Post Import Checks' subgrid
	Then the 'Related Importer Notification' field is populated

@issue:62439 @acceptance-criteria:62439-6
Scenario: A caseworker attempts to create a new Post Import Check without a related importer notification or import record
	Given I am logged in to the 'EU Imports' app as 'a caseworker'
	And I am creating a new 'Post Import Check' record
	When I save the record
	Then I see an error notification on the form stating 'Please complete either the 'Related Importer Notification' or 'Related Import Record' field'
