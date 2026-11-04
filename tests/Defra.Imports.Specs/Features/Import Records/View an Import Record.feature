@issue:US-049
Feature: View an Import Record

@acceptance-criteria:us-049-2
Scenario: A caseworker views an Import Record's non-compliance details
	Given I am logged in to the 'EU Imports' app as 'a caseworker'
	And I have opened an Import Record linked to a non-compliant Importer Notification of type 'No Health Certificate'
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
		| Other comments |
	And I see the following fields
		| Field                           | Editable |
		| Contacted Due to Non-Compliance | false    |
		| Non-Compliance Status           | false    |
