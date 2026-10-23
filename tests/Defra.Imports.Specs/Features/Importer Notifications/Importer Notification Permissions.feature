@issue:US-044
Feature: Importer Notification caseworker permissions

@acceptance-criteria:us-044-2
Scenario: A caseworker can read an Importer Notification
	Given I am logged in to the 'EU Imports' app as 'a caseworker'
	And I have opened an Importer Notification
	When I select the 'Importer Notification Details' tab
	Then I see the following fields
		| Field            |
		| Reference Number |
		| Version          |

@acceptance-criteria:us-044-2
Scenario: A caseworker can write to an Importer Notification
	Given I am logged in to the 'EU Imports' app as 'a caseworker'
	And I have opened an Importer Notification
	When I select the 'Importer Notification Details' tab
	Then I can edit the 'Health Certificate Attached' field
	And I can edit the 'Devolved Office' field

@acceptance-criteria:us-044-2
Scenario: A caseworker can write to the Non-Compliance fields of an Importer Notification
	Given I am logged in to the 'EU Imports' app as 'a caseworker'
	And I have opened an Importer Notification
	When I select the 'Non-Compliance' tab
	Then I can edit the 'Contacted Due to Non-Compliance' field
	And I can edit the 'Date Email Sent' field
	And I can edit the 'Type of Non-Compliance' field
	And I can edit the 'Date Telephone Call Made' field
	And I can edit the 'Non-Compliance Other Comments' field
	And I can edit the 'Non-Compliance Status' field
	And I can edit the 'IRMS Person Responsible' field
	And I can edit the 'Date Completed' field
	And I can edit the 'PIMS Status' field

@acceptance-criteria:us-044-2
Scenario: A caseworker can assign an Importer Notification
	Given I am logged in to the 'EU Imports' app as 'a caseworker'
	And I have opened an Importer Notification
	When I assign the current record to me
	Then 'Owner' field is populated with the current user

@acceptance-criteria:us-044-2
Scenario: A caseworker can share an Importer Notification
	Given I am logged in to the 'EU Imports' app as 'a caseworker'
	And I have opened an Importer Notification
	When I open the share menu
	Then I see the following share options
		| Option        |
		| Email link    |
		| Copy link     |
		| Manage access |

# Adding a note needs Append To on the Importer Notification and Append on the note.
@acceptance-criteria:us-044-2
Scenario: A caseworker can append a note to an Importer Notification
	Given I am logged in to the 'EU Imports' app as 'a caseworker'
	And I have opened an Importer Notification
	When I add the note 'Reviewed and ready for risk assessment.'
	Then I see a note containing 'Reviewed and ready for risk assessment.'

@acceptance-criteria:us-044-2 @ignore
Scenario: A caseworker cannot create an Importer Notification
	Given I am logged in to the 'EU Imports' app as 'a caseworker'
	And I have navigated to 'Case Management' -> 'Case Management' -> 'Importer Notifications'
	Then I cannot see the following commands on the grid
		| Command |
		| New     |
