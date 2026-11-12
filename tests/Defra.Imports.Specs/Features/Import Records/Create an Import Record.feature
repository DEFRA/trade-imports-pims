@issue:US-001
Feature: Create an Import Record

# The deployed labels are 'Import Notification' and 'CHED-A'; AC-7 says 'Importer Notification' and 'CHEDA'. Raise the label difference with the Product Analyst.
@acceptance-criteria:us-001-7
Scenario: A caseworker creates an Import Record manually
	Given I am logged in to the 'EU Imports' app as 'a caseworker'
	And I have navigated to 'Case Management' -> 'Case Management' -> 'Import Records'
	When I click to create a new record from the view
	Then I can see the following options in the 'Import Record Type' field
		| Option                |
		| Import Notification   |
		| Health Certificate    |
		| ITAHC - Landbridge    |
		| CHED-A                |
		| CHEDP                 |
		| DOCOM                 |
		| ITAHC                 |
	And the following fields are empty
		| Field              |
		| Import Record Type |

# Defect: AC-12 says manual creation offers only ITAHC, CHEDA and CHEDP, which contradicts AC-7. The deployed picklist offers the full AC-7 list. Raise with the Product Analyst.
@acceptance-criteria:us-001-12 @ignore @defect
Scenario: A caseworker creates an Import Record manually with only manual Import Record Types
	Given I am logged in to the 'EU Imports' app as 'a caseworker'
	And I have navigated to 'Case Management' -> 'Case Management' -> 'Import Records'
	When I click to create a new record from the view
	Then I can see the following options in the 'Import Record Type' field
		| Option |
		| ITAHC  |
		| CHED-A |
		| CHEDP  |
