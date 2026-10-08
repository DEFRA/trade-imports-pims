Feature: View Importer Notifications

@issue:US-003 @acceptance-criteria:us-003-2
Scenario: A caseworker views active Importer Notifications
	Given I am logged in to the 'EU Imports' app as 'a caseworker'
	And I have navigated to 'Case Management' -> 'Case Management' -> 'Importer Notifications'
	Then I see a 'Active Importer Notifications' view with the following columns
		| Reference Number | Version | Submission Date | Created On | Status | Type | Person Responsible Name | Person Responsible Company Name | Person Responsible Email | Person Responsible Phone | Country of Origin | Region of Origin | Place of Destination Address City | Place of Destination Address Postcode | Owner | Cloned | Imp Type |
	And the view is sorted by the 'Created On' column in descending order

# Possible defect: Deployed 'All POAO/HRFNAO Importer Notifications' view differs from the requirement: it has no separate 'Status (Active/Inactive)' column.
@issue:US-003 @acceptance-criteria:us-003-5 @possible-defect
Scenario: A caseworker views all POAO/HRFNAO Importer Notifications
	Given I am logged in to the 'EU Imports' app as 'a caseworker'
	And I have navigated to 'Case Management' -> 'Case Management' -> 'Importer Notifications'
	Then I see a 'All POAO/HRFNAO Importer Notifications' view with the following columns
		| Reference Number | Version | Submission Date | Created On | Status | Status | Type | Person Responsible Name | Person Responsible Company Name | Person Responsible Email | Person Responsible Phone | Country of Origin | Region of Origin | Place of Destination Address City | Place of Destination Address Postcode | Owner | Imp Type | Commodity Description | Commodity Code |
	And the view is sorted by the 'Reference Number' column in ascending order

@issue:US-055 @acceptance-criteria:us-055-1 @todo:implement
Scenario: A caseworker views Importer Notifications flagged for multiple commodity codes
	Given I am logged in to the 'EU Imports' app as 'a caseworker'
	And I have navigated to 'Case Management' -> 'Case Management' -> 'Importer Notifications'
	Then I see a 'Importer Notifications with >1 Commodity Codes - No Caseworker intervention' view with the following columns
		| Reference Number | Version | Submission Date | Created On | Status | Type | Person Responsible Name | Person Responsible Company Name | Person Responsible Email | Person Responsible Phone | Country of Origin | Region of Origin |
	And the view is sorted by the 'Created On' column in descending order

# Possible defect: Deployed quick find configuration only searches Reference Number; it does not search Importer Name, Charity Name, Premises of Origin Name, Permanent Destination Name or Animal / Product ID as required.
@issue:US-003 @acceptance-criteria:us-003-3 @possible-defect @todo:implement
Scenario: A caseworker searches for an Importer Notification by related party or commodity details
	Given I am logged in to the 'EU Imports' app as 'a caseworker'
	# TODO: Implement non-parameterised binding that handles Importer Notification only in ImporterNotificationSteps.cs. Use ImporterNotificationScenario.Builder to create an import notification with no specific configuration and adds it to the context with a given key.
	And an Importer Notification has been created
	And I have navigated to 'Case Management' -> 'Case Management' -> 'Importer Notifications'
	# TODO: Implement binding that gets entity list page data set to perform search for created binding by selecting one of those field values at random from the Importer Notification in the context in ImporterNotificationSteps.cs.
	When I search for the Importer Notification using one the following fields
		| Field                      |
		| Importer Name              |
		| Charity Name               |
		| Premises of Origin Name    |
		| Permanent Destination Name |
		| Animal / Product ID        |
	# TODO: Implement binding that uses the data set control to assert visibility of the Importer Notification in the context by using the reference number in ImporterNotificationSteps.cs.
	Then I see the matching Importer Notification in the search results
