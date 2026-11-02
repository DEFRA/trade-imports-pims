Feature: View a parameter set key data pair

@issue:US-044 @acceptance-criteria:us-044-1
Scenario: A caseworker views a parameter set key data pair
	Given I am logged in to the 'EU Imports' app as 'a caseworker'
	And I have opened a parameter set key data pair of an Importer Notification
	Then I see the following fields
		| Field                              |
		| Commodity Complement Parameter Set |
		| Key                                |
		| Data                               |
