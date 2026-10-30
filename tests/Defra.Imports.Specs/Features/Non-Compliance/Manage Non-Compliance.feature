@issue:US-049
Feature: Manage Non-Compliance

@acceptance-criteria:us-049-1
Scenario: A caseworker views and updates non-compliance fields on an Importer Notification
	Given I am logged in to the 'EU Imports' app as 'a caseworker'
	And I have opened a non-compliant Importer Notification
	When I select the 'Non-Compliance' tab
	Then I see the following fields
		| Field                             |
		| Contacted Due to Non-Compliance   |
		| Date Email Sent                   |
		| Date Telephone Call Made          |
		| Type of Non-Compliance            |
		| IRMS Person Responsible           |
		| Non-Compliance Status             |
		| PIMS Status                       |
		| Date Completed                    |
	And I do not see the following fields
		| Field            |
		| Other comments <defraimp_noncomplianceothercomments> |

@acceptance-criteria:us-049-1
Scenario: A caseworker sees default non-compliance values on an Importer Notification
	Given I am logged in to the 'EU Imports' app as 'a caseworker'
	And I have opened a non-compliant Importer Notification
	When I select the 'Non-Compliance' tab
	Then I see the following field values
		| Field                           | Value       |
		| Contacted Due to Non-Compliance | No          |
		| Non-Compliance Status           | In Progress |
	And the following fields are empty
		| Field          |
		| Date Completed |

@acceptance-criteria:us-049-2
Scenario: A caseworker views and updates non-compliance fields on an Import Record linked to a non-compliant Importer Notification
	Given I am logged in to the 'EU Imports' app as 'a caseworker'
	And I have opened an Import Record linked to a non-compliant Importer Notification
	When I select the 'Non-Compliance' tab
	Then I see the following fields
		| Field                             |
		| Contacted Due to Non-Compliance   |
		| Date Email Sent                   |
		| Date Telephone Call Made          |
		| Type of Non-Compliance            |
		| IRMS Person Responsible           |
		| Non-Compliance Status             |
		| PIMS Status                       |
		| Date Completed                    |
	And I do not see the following fields
		| Field            |
		| Other comments <defraimp_noncomplianceothercomments> |

# Known product defect from US-049 AC-2 note: tab visibility depends on linked Importer Notification non-compliance.
@acceptance-criteria:us-049-2 @ignore @defect
Scenario: A caseworker can classify non-compliance as no importer notification directly on an Import Record
	Given I am logged in to the 'EU Imports' app as 'a caseworker'
	And I have opened an 'Import Record'
	When I select the 'Non-Compliance' tab
	Then I see the following fields
		| Field                  |
		| Type of Non-Compliance |

@acceptance-criteria:us-049-3
Scenario: A caseworker sees importer-notification non-compliance details on the Import Record
	Given I am logged in to the 'EU Imports' app as 'a caseworker'
	And I have opened an Import Record linked to a non-compliant Importer Notification
	When I select the 'Non-Compliance' tab
	Then I see the following fields
		| Field                           | Editable |
		| Contacted Due to Non-Compliance | true    |
		| Non-Compliance Status           | true    |

# Known environment gap: expected non-compliance query Importer Notification views are not present in CIT test view selector.
@acceptance-criteria:us-049-4 @ignore @defect
Scenario: A caseworker views importer notifications with active non-compliance queries
	Given I am logged in to the 'EU Imports' app as 'a caseworker'
	And I have navigated to 'Case Management' -> 'Case Management' -> 'Importer Notifications'
	Then I see a 'Importer Notifications with Active Non-Compliance Queries' view with the following columns
		| Reference Number | Submission Date | PIMS Status | Type of Non-Compliance | IRMS Person Responsible | Country of Origin | Place of Destination Address Postcode | Contacted Due to Non-Compliance | Date Email Sent | Date Telephone Call Made | Status |
	And the view is sorted by the 'Reference Number' column in ascending order

# Known environment gap: expected non-compliance query Importer Notification views are not present in CIT test view selector.
@acceptance-criteria:us-049-4 @ignore @defect
Scenario: A caseworker views importer notifications with closed non-compliance queries
	Given I am logged in to the 'EU Imports' app as 'a caseworker'
	And I have navigated to 'Case Management' -> 'Case Management' -> 'Importer Notifications'
	Then I see a 'Importer Notifications with Closed Non-Compliance Queries' view with the following columns
		| Reference Number | Submission Date | PIMS Status | Date Completed | Type of Non-Compliance | IRMS Person Responsible | Country of Origin | Place of Destination Address Postcode | Contacted Due to Non-Compliance | Date Email Sent | Date Telephone Call Made | Status |
	And the view is sorted by the 'Reference Number' column in ascending order

@acceptance-criteria:us-049-5
Scenario: A caseworker views import records with active non-compliance queries
	Given I am logged in to the 'EU Imports' app as 'a caseworker'
	And I have navigated to 'Case Management' -> 'Case Management' -> 'Import Records'
	Then I see a 'Import Records with Active Non-Compliance Queries' view with the following columns
		| Name | Country of Origin | PIMS Status | Type of Non-Compliance | IRMS Person Responsible | Place of Origin | Place of Destination | Created On | Contacted Due to Non-Compliance | Date Email Sent | Date Telephone Call Made | Status |
	And the view is sorted by the 'Created On' column in ascending order

@acceptance-criteria:us-049-5
Scenario: A caseworker views import records with closed non-compliance queries
	Given I am logged in to the 'EU Imports' app as 'a caseworker'
	And I have navigated to 'Case Management' -> 'Case Management' -> 'Import Records'
	Then I see a 'Import Records with Closed Non-Compliance Queries' view with the following columns
		| Name | Country of Origin | PIMS Status | Date Completed | Type of Non-Compliance | IRMS Person Responsible | Place of Origin | Place of Destination | Created On | Contacted Due to Non-Compliance | Date Email Sent | Date Telephone Call Made | Status |
	And the view is sorted by the 'Created On' column in ascending order

@acceptance-criteria:us-049-6
Scenario: A caseworker records chronological non-compliance notes on an Importer Notification
	Given I am logged in to the 'EU Imports' app as 'a caseworker'
	And I have opened a non-compliant Importer Notification
	When I add a non-compliance timeline note
	Then I see a non-compliance timeline
	And I see the non-compliance timeline note