Feature: View Importer Notifications

@issue:US-003 @acceptance-criteria:us-003-2 @issue:US-044 @acceptance-criteria:us-044-8
Scenario: A caseworker views active Importer Notifications
	Given I am logged in to the 'EU Imports' app as 'a caseworker'
	And I have navigated to 'Case Management' -> 'Case Management' -> 'Importer Notifications'
	Then I see a 'Active Importer Notifications' view with the following columns
		| Reference Number | Version | Submission Date | Created On | Status | Type | Person Responsible Name | Person Responsible Company Name | Person Responsible Email | Person Responsible Phone | Country of Origin | Region of Origin | Place of Destination Address City | Place of Destination Address Postcode | Owner | Cloned | Imp Type |
	And the view is sorted by the 'Created On' column in descending order

# Defect: deployed 'All POAO/HRFNAO Importer Notifications' view differs from the requirement: it has no separate 'Status' column for Active/Inactive state code (only the custom Status column).
@issue:US-003 @acceptance-criteria:us-003-5 @ignore @defect
Scenario: A caseworker views all POAO/HRFNAO Importer Notifications
	Given I am logged in to the 'EU Imports' app as 'a caseworker'
	And I have navigated to 'Case Management' -> 'Case Management' -> 'Importer Notifications'
	Then I see a 'All POAO/HRFNAO Importer Notifications' view with the following columns
		| Reference Number | Version | Submission Date | Created On | Status | Status | Type | Person Responsible Name | Person Responsible Company Name | Person Responsible Email | Person Responsible Phone | Country of Origin | Region of Origin | Place of Destination Address City | Place of Destination Address Postcode | Owner | Imp Type | Commodity Description | Commodity Code |
	And the view is sorted by the 'Reference Number' column in ascending order

@issue:US-055 @acceptance-criteria:us-055-1
Scenario: A caseworker views Importer Notifications flagged for multiple commodity codes
	Given I am logged in to the 'EU Imports' app as 'a caseworker'
	And I have navigated to 'Case Management' -> 'Case Management' -> 'Importer Notifications'
	Then I see a 'Importer Notifications with >1 Commodity Codes - No Caseworker intervention' view with the following columns
		| Reference Number | Version | Submission Date | Created On | Status | Type | Person Responsible Name | Person Responsible Company Name | Person Responsible Email | Person Responsible Phone | Country of Origin | Region of Origin |
	And the view is sorted by the 'Created On' column in descending order

@issue:US-044 @acceptance-criteria:us-044-6
# US-044 AC-6 names the last-but-one column 'Port of Entry'; the deployed view labels it 'BCP or Port of Entry'.
Scenario: A caseworker views amended Importer Notifications where Health Certificate Attached and Owner changed to EU Imports Dynamics
	Given I am logged in to the 'EU Imports' app as 'a caseworker'
	And I have navigated to 'Case Management' -> 'Case Management' -> 'Importer Notifications'
	Then I see a 'Amended Notifications where Health Certificate Attached and Owner changed to EU Imports Dynamics' view with the following columns
		| Reference Number | Version | PIMS Status | Type of Non-Compliance | IRMS Person Responsible | Devolved Office | Arrival Date | Submission Date | Created On | Status | Type | Country of Origin | Commodity Code | Commodity Species Name | Commodities Number of Animals | Commodity ID Types | Purpose of Movement | Place of Original Harvest Company Name | Place of Original Harvest Individual Name | Place of Destination Address Postcode | Place of Destination Company Name | Place of Destination Address City | Person Responsible Name | Person Responsible Company Name | Transporter Company Name | BCP or Port of Entry | Owner |
	And the view is sorted by the 'Created On' column in ascending order

# Defect: US-044 AC-6 requires column widths to show the whole title, but 'Type of Non-Compliance', 'Submission Date', 'Commodity Species Name', 'Commodities Number of Animals', 'Commodity ID Types' and 'BCP or Port of Entry' are cut off in the deployed view.
@issue:US-044 @acceptance-criteria:us-044-6 @ignore @defect
Scenario: A caseworker sees every column title in the amended Importer Notifications view
	Given I am logged in to the 'EU Imports' app as 'a caseworker'
	And I have navigated to 'Case Management' -> 'Case Management' -> 'Importer Notifications'
	When I switch to the 'Amended Notifications where Health Certificate Attached and Owner changed to EU Imports Dynamics' view
	Then no column title in the view is cut off

@issue:US-044 @acceptance-criteria:us-044-6
Scenario: A caseworker finds an amended Importer Notification with a health certificate in the Amended Notifications view
	Given I am logged in to the 'EU Imports' app as 'a caseworker'
	And an amended Importer Notification with a health certificate has been created
	And I have navigated to 'Case Management' -> 'Case Management' -> 'Importer Notifications'
	When I switch to the 'Amended Notifications where Health Certificate Attached and Owner changed to EU Imports Dynamics' view
	And I search for the Importer Notification by its reference number
	Then I see the matching Importer Notification in the search results

# Defect: deployed quick find configuration only searches Reference Number; it does not search Importer Name, Charity Name, Premises of Origin Name, Permanent Destination Name or Animal / Product ID as required.
@issue:US-003 @acceptance-criteria:us-003-3 @ignore @defect
Scenario: A caseworker searches for an Importer Notification by related party or commodity details
	Given I am logged in to the 'EU Imports' app as 'a caseworker'
	And an Importer Notification has been created with searchable fields populated
	And I have navigated to 'Case Management' -> 'Case Management' -> 'Importer Notifications'
	When I search for the Importer Notification using one the following fields
		| Field                      |
		| Importer Name              |
		| Charity Name               |
		| Premises of Origin Name    |
		| Permanent Destination Name |
		| Animal / Product ID        |
	Then I see the matching Importer Notification in the search results
