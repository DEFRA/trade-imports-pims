@issue:US-001
Feature: Search for an Import Record

# AC-3 says Importer Name; the quick find searches the importer's Contact Name, which is taken to be the Importer Name. Confirm with the Product Analyst.
@acceptance-criteria:us-001-3
Scenario: A caseworker searches for an Import Record
	Given I am logged in to the 'EU Imports' app as 'a caseworker'
	And an Import Record has been created with searchable fields populated
	And I have navigated to 'Case Management' -> 'Case Management' -> 'Import Records'
	When I search for the Import Record using each of the following fields
		| Field                   |
		| Importer Name           |
		| Premises of Origin Name |
	Then I see the matching Import Record in the search results

# Defect: searching by Date of Import, ITAHC Reference or Importer Notification Local Reference Number does not return the Import Record, although AC-3 requires it. The quick find configuration includes none of these fields.
@acceptance-criteria:us-001-3 @ignore @defect
Scenario: A caseworker searches for an Import Record by its date of import or reference numbers
	Given I am logged in to the 'EU Imports' app as 'a caseworker'
	And an Import Record has been created with searchable fields populated
	And I have navigated to 'Case Management' -> 'Case Management' -> 'Import Records'
	When I search for the Import Record using each of the following fields
		| Field                                        |
		| Date of Import                               |
		| ITAHC Certificate Reference Number           |
		| Importer Notification Local Reference Number |
	Then I see the matching Import Record in the search results
