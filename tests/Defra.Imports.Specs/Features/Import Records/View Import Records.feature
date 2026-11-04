@issue:US-049
Feature: View Import Records

@acceptance-criteria:us-049-5
Scenario: A caseworker views import records with active non-compliance queries
	Given I am logged in to the 'EU Imports' app as 'a caseworker'
	And I have navigated to 'Case Management' -> 'Case Management' -> 'Import Records'
	Then I see a 'Import Records with Active Non-Compliance Queries' view with the following columns
		| Name | Country of Origin | PIMS Status | Type of Non-Compliance | IRMS Person Responsible | Place of Origin - Address - City | Place of Destination - Address - City | Created On | Contacted Due to Non-Compliance | Date Email Sent | Date Telephone Call Made | Status Reason |
	And the view is sorted by the 'Created On' column in ascending order

@acceptance-criteria:us-049-5
Scenario: A caseworker views import records with closed non-compliance queries
	Given I am logged in to the 'EU Imports' app as 'a caseworker'
	And I have navigated to 'Case Management' -> 'Case Management' -> 'Import Records'
	Then I see a 'Import Records with Closed Non-Compliance Queries' view with the following columns
		| Name | Country of Origin | PIMS Status | Date Completed | Type of Non-Compliance | IRMS Person Responsible | Place of Origin - Address - City | Place of Destination - Address - City | Created On | Contacted Due to Non-Compliance | Date Email Sent | Date Telephone Call Made | Status Reason |
	And the view is sorted by the 'Created On' column in ascending order
