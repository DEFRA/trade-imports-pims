@issue:US-049
Feature: Update an Importer Notification

@acceptance-criteria:us-049-6
Scenario: A caseworker updates an Importer Notification with a note
	Given I am logged in to the 'EU Imports' app as 'a caseworker'
	And I have opened an Importer Notification
	When I add a timeline note
	Then I see a timeline
	And I see the timeline note
