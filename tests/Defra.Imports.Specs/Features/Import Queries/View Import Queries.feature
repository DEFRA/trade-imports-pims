Feature: View Import Queries

 @issue:62908
Scenario: A caseworker views all Import Queries
	Given I am logged in to the 'EU Imports' app as 'a caseworker'
	And I have navigated to 'Case Management' -> 'Case Management' -> 'Import Queries'
	Then I see a 'All Import Queries' view with the following columns
		| Query Number | Regarding | ITAHC | Query Sent To | Summary | Owner | Query Raised By | Date Raised | Due Date | Status Code | Date Resolved |
	And the view is sorted by the 'Query Number' column in descending order
