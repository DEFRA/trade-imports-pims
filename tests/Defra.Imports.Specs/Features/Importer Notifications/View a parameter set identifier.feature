Feature: View a parameter set identifier

@issue:US-044 @acceptance-criteria:us-044-1
Scenario: A caseworker views a parameter set identifier
	Given I am logged in to the 'EU Imports' app as 'a caseworker'
	And I have opened a parameter set identifier of an Importer Notification
	Then I see the following fields
		| Field                                          |
		| Commodity Complement Parameter Set             |
		| Species Number                                 |
		| Data                                           |
		| Importer Notification Id                       |
		| Permanent Address                              |
		| Is Place Of Destination The Permanent Address  |
		| Type                                           |
		| Address Line1                                  |
		| Address Line2                                  |
		| city                                           |
		| Postal Zip Code                                |
		| Email                                          |
		| Telephone                                      |
