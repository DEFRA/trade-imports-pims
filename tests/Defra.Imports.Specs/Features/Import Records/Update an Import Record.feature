@issue:US-001
Feature: Update an Import Record

# AC-11 (a note keeps its original creator after reassignment) is not automated yet: it needs a step that assigns a record to a different user and a step that reads the creator of a note. Agree the approach with the maintainer.

# Defect: no 'No ITAHC Received' ITAHC record exists in the environment (BR-028), so it cannot be selected in Primary HC. Primary HC is shown on the form only for ITAHC and ITAHC - Landbridge Import Records, and in the Triage stage of the business process flow.
@acceptance-criteria:us-001-4 @ignore @defect
Scenario: A caseworker saves an Import Record without a linked ITAHC
	Given I am logged in to the 'EU Imports' app as 'a caseworker'
	And I have opened an Import Record with an Import Record Type of 'ITAHC'
	When I enter the following details
		| Field      | Value             |
		| Primary HC | No ITAHC Received |
	And I save the record
	Then the record is saved successfully

@acceptance-criteria:us-001-6
Scenario: A caseworker selects a Primary HC during triage
	Given I am logged in to the 'EU Imports' app as 'a caseworker'
	And I have opened an Import Record at the Triage stage with a Commodity Code value
	When I select the ITAHC created for the scenario in the following fields of the current business process stage
		| Field      |
		| Primary HC |
	And I save the record
	Then I see a value of '0102210000' in the 'Commodity Code' field

# AC-18 calls the fields Phone and Email; on the form they are Importer Telephone and Importer Email, which may be the renamed fields. Confirm with the Product Analyst.
# Defect: AC-18 requires CPH (optional), Phone (mandatory) and Email (mandatory), but there is no CPH field on the form and Importer Telephone and Importer Email are not mandatory.
@acceptance-criteria:us-001-18
Scenario: A caseworker records an importer's contact details on an Import Record
	Given I am logged in to the 'EU Imports' app as 'a caseworker'
	And I have opened an Import Record
	When I select the 'Importer' tab
	And I enter the following details
		| Field              | Value                  |
		| Importer Telephone | 07123456789            |
		| Importer Email     | caseworker@test.gov.uk |
	And I save the record
	Then the record is saved successfully
	# And the 'Importer Telephone' field is mandatory
	# And the 'Importer Email' field is mandatory
	# And the 'CPH' field is optional

@acceptance-criteria:us-001-18
Scenario: A caseworker records a transporter's address on an Import Record
	Given I am logged in to the 'EU Imports' app as 'a caseworker'
	And I have opened an Import Record
	When I select the 'Transporter' tab
	And I enter the following details
		| Field    | Value         |
		| Line 1   | 1 Test Street |
		| Line 2   | Test Area     |
		| Line 3   | Test District |
		| City     | London        |
		| Postcode | SW1A 1AA      |
	And I save the record
	Then the record is saved successfully

@acceptance-criteria:us-001-1
Scenario: A caseworker records that an IV65 has been sent on an Import Record
	Given I am logged in to the 'EU Imports' app as 'a caseworker'
	And I have opened an Import Record
	When I select the 'Summary' tab
	And I enter the following details
		| Field      | Value |
		| IV65 Sent? | true  |
	Then I see the following fields
		| Field                  |
		| IV65 Sent Date         |
		| IV65 Response Due Date |

@acceptance-criteria:us-001-1
Scenario: A caseworker records that a Warble Fly treatment declaration is required on an Import Record
	Given I am logged in to the 'EU Imports' app as 'a caseworker'
	And I have opened an Import Record
	When I select the 'Summary' tab
	And I enter the following details
		| Field                                     | Value |
		| Warble Fly Treatment Declaration Required | true  |
	Then I see the following fields
		| Field                                          |
		| Warble Fly Treatment Declaration Received Date |
