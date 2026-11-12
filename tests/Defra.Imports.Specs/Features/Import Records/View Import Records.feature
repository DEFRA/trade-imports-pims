@issue:US-001
Feature: View Import Records

# Column labels as they appear in the view, which differ from AC-2: 'Place of Destination Organisation' (Place of Destination), 'Created On' (Created On Date). The view also shows Name, Imp Type and Post Import Checks Outcome.
@acceptance-criteria:us-001-2
Scenario: A caseworker views all Import Records
	Given I am logged in to the 'EU Imports' app as 'a caseworker'
	And I have navigated to 'Case Management' -> 'Case Management' -> 'Import Records'
	Then I see a '7. All Import Records' view with the following columns
		| Name | Primary HC | Commodity Type | Imp Type | Country of Origin | Import Risk Level | Place of Origin Organisation | Place of Destination Organisation | Post Import Checks Outcome | Created On |
	And the view is sorted by the 'Created On' column in descending order
