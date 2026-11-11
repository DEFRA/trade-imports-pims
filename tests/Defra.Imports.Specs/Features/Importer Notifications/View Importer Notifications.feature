Feature: View Importer Notifications

@issue:US-003 @acceptance-criteria:us-003-2 @issue:US-044 @acceptance-criteria:us-044-8 @issue:62908
Scenario: A caseworker views active Importer Notifications
	Given I am logged in to the 'EU Imports' app as 'a caseworker'
	And I have navigated to 'Case Management' -> 'Case Management' -> 'Importer Notifications'
	Then I see a 'Active Importer Notifications' view with the following columns
		| Reference Number | Version | Submission Date | Created On | Status | Type | Person Responsible Name | Person Responsible Company Name | Person Responsible Email | Person Responsible Phone | Country of Origin | Region of Origin | Place of Destination Address City | Place of Destination Address Postcode | Owner | Cloned | Imp Type | Arrival Date | Health Certificate Attached |
	And the view is sorted by the 'Created On' column in descending order

@issue:62908
Scenario: A caseworker views Active CHEDAs (IRMS)
	Given I am logged in to the 'EU Imports' app as 'a caseworker'
	And I have navigated to 'Case Management' -> 'Case Management' -> 'Importer Notifications'
	Then I see a 'Active CHEDAs (IRMS)' view with the following columns
		| Reference Number | Version | Devolved Office | Arrival Date | Submission Date | Last Updated | Created On | Status | Type | Imp Type | Country of Origin | Consignor Address Country | Commodity Code | Commodity Species Name | Commodity Description | Commodities Number of Animals | Commodity ID Types | Purpose of Movement | Purpose of Consignment | Internal Market Purpose | Certified For | Consignor Company Name | Place of Destination Company Name | Place of Destination Country | Place of Destination Address City | Place of Destination Address Postcode | CPH Number | Person Responsible Name | Person Responsible Company Name | Person Responsible Phone | Person Responsible Postcode | Person Responsible Email | Transporter Company Name | Transporter Approval Number | BCP or Port of Entry | Owner | Cloned | Country Group (Country of Origin) |
	And the view is sorted by the 'Created On' column in descending order

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
