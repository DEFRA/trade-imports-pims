Feature: View Importer Notifications

# Possible defect: Deployed 'Active Importer Notifications' view differs from the requirement: it additionally shows Cloned and Imp Type columns, and labels columns 'Created On'/'Person Responsible Company Name' rather than 'Created on'/'Person Responsible Company'.
@possible-defect
Scenario: A caseworker views active Importer Notifications
	Given I am logged in to the 'EU Imports' app as 'a caseworker'
	And I have navigated to 'Case Management' -> 'Case Management' -> 'Importer Notifications'
	Then I see a 'Active Importer Notifications' view with the following columns:
		| Reference Number | Version | Submission Date | Created on | Status | Type | Person Responsible Name | Person Responsible Company | Person Responsible Email | Person Responsible Phone | Country of Origin | Region of Origin | Place of Destination Address City | Place of Destination Address Postcode | Owner |
	And the view is sorted by the 'Created On' column in descending order

# Possible defect: Deployed 'All POAO/HRFNAO Importer Notifications' view differs from the requirement: it has no separate 'Status (Active/Inactive)' column, omits 'Cloned', labels columns 'Created On'/'Person Responsible Company Name', and sorts ascending by Reference Number rather than by creation date.
@possible-defect
Scenario: A caseworker views all POAO/HRFNAO Importer Notifications
	Given I am logged in to the 'EU Imports' app as 'a caseworker'
	And I have navigated to 'Case Management' -> 'Case Management' -> 'Importer Notifications'
	Then I see a 'All POAO/HRFNAO Importer Notifications' view with the following columns:
		| Reference Number | Version | Submission Date | Created on | Status | Type | Person Responsible Name | Person Responsible Company | Person Responsible Email | Person Responsible Phone | Country of Origin | Region of Origin | Place of Destination Address City | Place of Destination Address Postcode | Owner | Status (Active/Inactive) | Imp Type | Commodity Description | Commodity Code |
	And the view is sorted by the 'Created On' column in descending order

Scenario: A caseworker views Importer Notifications flagged for multiple commodity codes
	Given I am logged in to the 'EU Imports' app as 'a caseworker'
	And I have navigated to 'Case Management' -> 'Case Management' -> 'Importer Notifications'
	Then I see a 'Importer Notifications with >1 Commodity Codes - No Caseworker intervention' view with the following columns:
		| Reference Number | Version | Submission Date | Created On | Status | Type | Person Responsible Name | Person Responsible Company Name | Person Responsible Email | Person Responsible Phone | Country of Origin | Region of Origin |
	And the view is sorted by the 'Created On' column in descending order

# Possible defect: Deployed quick find configuration only searches Reference Number; it does not search Importer Name, Charity Name, Premises of Origin Name, Permanent Destination Name or Animal / Product ID as required.
@possible-defect
Scenario: A caseworker searches for an Importer Notification by related party or commodity details
	Given I am logged in to the 'EU Imports' app as 'a caseworker'
	And I have navigated to 'Case Management' -> 'Case Management' -> 'Importer Notifications'
	When I search for an Importer Notification using one the following fields:
		| Field                      |
		| Importer Name              |
		| Charity Name               |
		| Premises of Origin Name    |
		| Permanent Destination Name |
		| Animal / Product ID        |
	Then I see the matching Importer Notification in the search results
