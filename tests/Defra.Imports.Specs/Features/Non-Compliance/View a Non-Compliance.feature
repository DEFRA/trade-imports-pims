@issue:US-049
Feature: View a Non-Compliance

# Known product defect from US-049 AC-2 note: tab visibility depends on linked Importer Notification non-compliance.
@acceptance-criteria:us-049-2 @ignore @defect
Scenario: A caseworker updates an Import Record as non-compliant due to no importer notification
	Given I am logged in to the 'EU Imports' app as 'a caseworker'
	And I have opened an 'Import Record'
	When I select the 'Non-Compliance' tab
	And I enter 'No Importer Notification' in the 'Type of Non-Compliance' field
	And I save the record
	Then I see the following field values
		| Field                  | Value                    |
		| Type of Non-Compliance | No Importer Notification |
	And I see the following fields
		| Field                           | Editable |
		| Contacted Due to Non-Compliance | false    |
		| Non-Compliance Status           | false    |

