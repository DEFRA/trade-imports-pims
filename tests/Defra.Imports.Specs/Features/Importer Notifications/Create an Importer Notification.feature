@issue:US-044
Feature: Create an Importer Notification

@acceptance-criteria:us-044-2
Scenario: Caseworker attempts to create an importer notification
	Given I am logged in to the 'EU Imports' app as 'a caseworker'
	And I have navigated to 'Case Management' -> 'Case Management' -> 'Importer Notifications'
	Then I cannot see the following commands on the grid
		| Command |
		| New     |
