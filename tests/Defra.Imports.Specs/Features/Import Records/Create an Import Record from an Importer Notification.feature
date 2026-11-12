@issue:US-001
Feature: Create an Import Record from an Importer Notification

# The deployed Import Record Type label is 'Import Notification'; AC-7, AC-9 and AC-17 call it 'Importer Notification'.
# AC-17 also copies Commodity Type, Countries of Transit, IV65 Sent, Warble Fly, Commodity Identifiers and Watch Flags. See the defect comment in the first scenario.
# Labels as they appear on the form, which differ from AC-17: 'Point of Entry' (Port of Entry), 'Purpose of Movement' (Purpose), 'IPAFFS Species ID' (Species ID).
@acceptance-criteria:us-001-7 @acceptance-criteria:us-001-13 @acceptance-criteria:us-001-17
Scenario: A caseworker creates an Import Record from an Importer Notification
	Given I am logged in to the 'EU Imports' app as 'a caseworker'
	And I have opened an Importer Notification
	When I click the 'Create Import Record From Notification' command
	And I open the Import Record created from the Importer Notification
	Then I see a value of 'Import Notification' in the 'Import Record Type' field
	And the following fields are populated
		| Field                         |
		| Primary Importer Notification |
		| Devolved Office               |
		| Commodity Code                |
		| Purpose of Movement           |
		| Country of Origin             |
		| Point of Entry                |
		| Imp Type                      |
		| Quantity                      |
		| IPAFFS Species ID             |
		| Commodity ID Types            |
		# Defect: the 'Create Import Record From Notification' workflow has no step that copies these AC-17 fields, so they cannot be asserted. Confirm with a developer whether another plug-in or flow should set them.
		# | Commodity Type         |
		# | Countries of Transit   |
		# | IV65 Sent              |
		# | Warble Fly             |
		# | Commodity Identifiers  |
		# | Watch Flags            |

@acceptance-criteria:us-001-14
Scenario: A caseworker creates an Import Record from an Importer Notification and the transporter type is copied
	Given I am logged in to the 'EU Imports' app as 'a caseworker'
	And I have opened an Importer Notification
	When I click the 'Create Import Record From Notification' command
	And I open the Import Record created from the Importer Notification
	And I select the 'Transporter' tab
	Then the following fields are populated
		| Field |
		| Type  |

@acceptance-criteria:us-001-9
Scenario: A caseworker creates an Import Record from an Importer Notification and the submission date is copied
	Given I am logged in to the 'EU Imports' app as 'a caseworker'
	And I have opened an Importer Notification
	When I click the 'Create Import Record From Notification' command
	And I open the Import Record created from the Importer Notification
	Then I see the Importer Notification's Submission Date in the 'Date Importer Notification Received' field
