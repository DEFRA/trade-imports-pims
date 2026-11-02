@issue:US-044
Feature: Update an Importer Notification

@acceptance-criteria:us-044-4
Scenario: A caseworker updates an Importer Notification's Health Certificate Attached field
	Given I am logged in to the 'EU Imports' app as 'a caseworker'
	And I have opened an Importer Notification
	When I select the 'Importer Notification Details' tab
	Then I see the following fields
		| Field                       | Editable |
		| Health Certificate Attached | true     |

@acceptance-criteria:us-044-2
Scenario: A caseworker updates an Importer Notification's Devolved Office
	Given I am logged in to the 'EU Imports' app as 'a caseworker'
	And I have opened an Importer Notification
	When I select the 'Importer Notification Details' tab
	And I enter 'IRMS - Scotland' in the 'Devolved Office' field
	And I save the record
	Then the record is saved successfully

@acceptance-criteria:us-044-2
Scenario: A caseworker updates an Importer Notification's non-compliance details
	Given I am logged in to the 'EU Imports' app as 'a caseworker'
	And I have opened an Importer Notification
	When I select the 'Non-Compliance' tab
	Then I see the following fields
		| Field                            | Editable |
		| Contacted Due to Non-Compliance  | true     |
		| Date Email Sent                  | true     |
		| Type of Non-Compliance           | true     |
		| Date Telephone Call Made         | true     |

# 'Non-Compliance Other Comments' is only shown once the type is Other.
@acceptance-criteria:us-044-2
Scenario: A caseworker updates an Importer Notification's non-compliance details when the type is Other
	Given I am logged in to the 'EU Imports' app as 'a caseworker'
	And I have opened an Importer Notification
	When I select the 'Non-Compliance' tab
	And I enter 'Other' in the 'Type of Non-Compliance' field
	Then I see the following fields
		| Field                         | Editable |
		| Non-Compliance Other Comments | true     |
		| Non-Compliance Status         | true     |
		| IRMS Person Responsible       | true     |
		| Date Completed                | true     |
		| PIMS Status                   | true     |

@acceptance-criteria:us-044-2
Scenario: A caseworker updates an importer notification with a note
	Given I am logged in to the 'EU Imports' app as 'a caseworker'
	And I have opened an Importer Notification
	When I add the note 'Reviewed and ready for risk assessment.'
	Then I see a note containing 'Reviewed and ready for risk assessment.'
