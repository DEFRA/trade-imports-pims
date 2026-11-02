Feature: View a permanent address

@issue:US-044 @acceptance-criteria:us-044-1
Scenario: A caseworker views a permanent address
	Given I am logged in to the 'EU Imports' app as 'a caseworker'
	And I have opened a permanent address of an Importer Notification
	Then I see the following fields
		| Field                    |
		| Individual Name          |
		| Importer Notification    |
		| Id                       |
		| Type                     |
		| Status                   |
		| Approval Number          |
		| Other Identifier         |
		| Company Name             |
		| Address Line 1           |
		| Address Line 2           |
		| Address Line 3           |
		| Address City             |
		| Address Post             |
		| Address Country          |
		| Email                    |
		| Telephone                |
		| UK Telephone             |
		| International Phone Code |
		| International Telephone  |
