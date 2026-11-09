@issue:US-044
Feature: Assign an Importer Notification

@acceptance-criteria:us-044-2
Scenario: A caseworker assigns an importer notification
	Given I am logged in to the 'EU Imports' app as 'a caseworker'
	And I have opened an Importer Notification
	When I assign the current record to me
	Then 'Owner' field is populated with the current user
