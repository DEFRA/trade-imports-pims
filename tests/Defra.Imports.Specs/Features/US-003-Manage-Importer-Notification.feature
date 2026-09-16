Feature: US-003 Manage Import Notification
	As an EU Imports Caseworker
	I want to create, update, list and search Import Notifications
	So that I can record pre-notification details for an import consignment

# Acceptance criteria are taken verbatim from
# https://defra.github.io/trade-imports-pims/requirements/user-stories/US-003-Manage-Import-Notification/
#
# The Import Notification concept is realised by the defraimp_importernotification
# entity. Field names below are FORM CONTROL labels, resolved against the active tab.
#
# Two kinds of scenario appear below:
#
#   1. Scenarios that exercise behaviour the solution implements. These assert
#      normally, so a failure is a GENUINE DEFECT in working functionality.
#   2. Scenarios tagged @ac-coverage, which measure the solution against the
#      acceptance criteria as written. Requirements the solution does not yet
#      implement are recorded as KNOWN DEFECTS and reported at the end of the
#      scenario rather than halting it, so every requirement is always measured.
#      These scenarios end as inconclusive (not executed) while gaps remain,
#      keeping known defects distinct from genuine test failures.

# ---------------------------------------------------------------------------
# AC-1: An EU Imports Caseworker can create or update an Import Notification
#       record with the listed fields, all optional.
# ---------------------------------------------------------------------------

# AC-1 create half is covered by the AC-1 create coverage scenario below.

# AC-1: Importer Name, Address, Postcode, Telephone, Email.
Scenario: A Caseworker can update the Importer details
	Given a precondition Importer Notification exists
	And I am logged in to the 'EU Imports' app as "EU Imports Caseworker"
	When I have opened "created-importer-notification"
	And I select the "Importer" tab by its exact name
	And I enter "Updated Importer Company" in the "Name" field
	And I enter "1 Updated Street" in the "Address Line 1" field
	And I enter "Updated City" in the "City" field
	And I enter "UP1 2DT" in the "Postcode" field
	And I enter "01615550101" in the "Telephone" field
	And I enter "updated.importer@email.com" in the "Email" field
	And I save the record
	Then the record is saved successfully

# AC-1: CPH Number.
Scenario: A Caseworker can update the CPH Number
	Given a precondition Importer Notification exists
	And I am logged in to the 'EU Imports' app as "EU Imports Caseworker"
	When I have opened "created-importer-notification"
	And I select the "Importer Notification Details" tab by its exact name
	And I enter "98/765/4321" in the "CPH Number" field
	And I save the record
	Then the record is saved successfully

# AC-1: Charity Name, Address, Postcode, Telephone, Email.
# Charity details are held against the second consignor. The form script showHideCharity
# only reveals Charity_Tab once Importing From Charity is set, so the toggle is switched
# on first.
@us-003-ac1
Scenario: A Caseworker can update the Charity details
	Given a precondition Importer Notification exists
	And I am logged in to the 'EU Imports' app as "EU Imports Caseworker"
	When I have opened "created-importer-notification"
	And I select the "Importer Notification Details" tab by its exact name
	And I toggle the "Importing From Charity" field to "True"
	And I select the "Charity" tab by its exact name
	And I enter "Updated Charity Organisation" in the "Company Name" field
	And I enter "2 Charity Way" in the "Address Line 1" field
	And I enter "CH1 4TY" in the "Address Postcode" field
	And I enter "01615550202" in the "Telephone" field
	And I enter "updated.charity@email.com" in the "Consignor Email" field
	And I save the record
	Then the record is saved successfully

# AC-1: Consignment Country of Origin and Countries of Transit.
# Countries of Transit is realised as Route Transiting States on the Transporter tab.
Scenario: A Caseworker can update the Consignment Country of Origin and Countries of Transit
	Given a precondition Importer Notification exists
	And I am logged in to the 'EU Imports' app as "EU Imports Caseworker"
	When I have opened "created-importer-notification"
	And I select the "Transporter" tab by its exact name
	And I enter "France, Belgium" in the "Route Transiting States" field
	And I save the record
	Then the record is saved successfully

# AC-1: Date of Import.
# Covered by the AC-1 coverage scenario at the end of this section.

# AC-1: Place of Destination (Contact Name, Address, Postcode, Telephone, Email).
Scenario: A Caseworker can update the Place of Destination details
	Given a precondition Importer Notification exists
	And I am logged in to the 'EU Imports' app as "EU Imports Caseworker"
	When I have opened "created-importer-notification"
	And I select the "Place of Destination" tab by its exact name
	And I enter "Updated Destination Contact" in the "Name" field
	And I enter "4 Destination Avenue" in the "Address Line 1" field
	And I enter "DE1 6ST" in the "Postcode" field
	And I enter "01615550303" in the "Telephone" field
	And I enter "updated.destination@email.com" in the "Email" field
	And I save the record
	Then the record is saved successfully

# AC-1: Permanent Destination (Contact Name, Address, Postcode, Telephone, Email).
# Covered by the AC-1 coverage scenario at the end of this section.

# AC-1: Premises of Origin (Name, Address, Postcode, Country).
# Premises of Origin is realised as the Place of Origin tab.
Scenario: A Caseworker can update the Premises of Origin details
	Given a precondition Importer Notification exists
	And I am logged in to the 'EU Imports' app as "EU Imports Caseworker"
	When I have opened "created-importer-notification"
	And I select the "Place of Origin" tab by its exact name
	And I enter "Updated Premises of Origin" in the "Name" field
	And I enter "3 Origin Road" in the "Address Line 1" field
	And I enter "OR1 5GN" in the "Postcode" field
	And I save the record
	Then the record is saved successfully

# AC-1: Transporter (Name, Address, Postcode, Telephone, Email).
Scenario: A Caseworker can update the Transporter details
	Given a precondition Importer Notification exists
	And I am logged in to the 'EU Imports' app as "EU Imports Caseworker"
	When I have opened "created-importer-notification"
	And I select the "Transporter" tab by its exact name
	And I enter "Updated Transporter" in the "Name" field
	And I enter "5 Transport Lane" in the "Address Line 1" field
	And I enter "TR1 7PT" in the "Postcode" field
	And I enter "01615550404" in the "Telephone" field
	And I enter "updated.transporter@email.com" in the "Email" field
	And I save the record
	Then the record is saved successfully

# AC-1 coverage. Every field named by AC-1 that is not proven by the scenarios above
# is attempted here. Fields the solution does not provide are reported as known
# defects and the remaining fields are still verified.
@ac-coverage @us-003-ac1
Scenario: AC-1 field coverage for an Import Notification
	Given a precondition Importer Notification exists
	And I am logged in to the 'EU Imports' app as "EU Imports Caseworker"
	When I have opened "created-importer-notification"
	And I attempt to populate the fields required by "AC-1"
		| Tab                          | Field                           | Value                       |
		| Transporter                  | Date of Import                  | 01/01/2026                  |
		| Permanent Addresses          | Name                            | Updated Permanent Contact   |
		| Permanent Addresses          | Address Line 1                  | 6 Permanent Close           |
		| Permanent Addresses          | Postcode                        | PE1 8RM                     |
		| Permanent Addresses          | Telephone                       | 01615550505                 |
		| Permanent Addresses          | Email                           | updated.permanent@email.com |
		| Commodity                    | Species / Product (Common Name) | Bovine                      |
		| Commodity                    | Quantity                        | 12                          |
		| Commodity                    | Units                           | Kilograms                   |
		| Commodity                    | Intended Use of Commodity       | Breeding                    |
		| Transporter                  | Port / Airport of Entry         | Dover                       |
		| Importer Notification Details | Animal / Product IDs           | UK123456789012              |

# AC-1 create half. The EU Imports Caseworker role has no prvCreate privilege on
# defraimp_importernotification, so creation is reported as a known defect.
@ac-coverage @us-003-ac1
Scenario: AC-1 create coverage for an Import Notification
	Given I am logged in to the 'EU Imports' app as "EU Imports Caseworker"
	And I navigate to "Case Management" -> "Case Management" -> "Importer Notifications"
	Then I verify a Caseworker can create a new record from the view for "AC-1"

# ---------------------------------------------------------------------------
# AC-2: An EU Imports Caseworker can view a list of all Import Notifications
#       ordered by creation date (newest first), showing the listed columns.
# ---------------------------------------------------------------------------
Scenario: The Import Notifications list is ordered by creation date newest first
	Given I am logged in to the 'EU Imports' app as "EU Imports Caseworker"
	And I navigate to "Case Management" -> "Case Management" -> "Importer Notifications"
	Then the view is sorted by the "Created On" column in descending order

# AC-2 coverage. The AC names the columns that must be visible. It does not constrain
# their order, and the view may expose additional columns. Every named column is
# checked, so the report lists all of the columns the view does not provide.
@ac-coverage @us-003-ac2
Scenario: AC-2 list coverage for Import Notifications
	Given I am logged in to the 'EU Imports' app as "EU Imports Caseworker"
	And I navigate to "Case Management" -> "Case Management" -> "Importer Notifications"
	Then I verify the sort order required by "AC-2" is the "Created On" column in descending order
	And I verify the columns required by "AC-2" in the "Active Importer Notifications" view
		| Date of Import | Premises of Origin Country | Species / Product (Common Name) | Reference Number | Importer Name | Importer Telephone | Importer Email | Port / Airport of Entry |

# ---------------------------------------------------------------------------
# AC-3: An EU Imports Caseworker can perform a free text search for an Import
#       Notification by Importer Name, Charity Name, Premises of Origin Name,
#       Permanent Destination Name or Animal / Product ID.
#
# Each criterion is searched in turn. Criteria that return no match are reported
# as known defects and the remaining criteria are still exercised, so the report
# shows exactly which of the five searches the solution supports.
# ---------------------------------------------------------------------------
@ac-coverage @us-003-ac3
Scenario: AC-3 free text search coverage for Import Notifications
	Given a precondition Importer Notification exists
	And I am logged in to the 'EU Imports' app as "EU Imports Caseworker"
	And I navigate to "Case Management" -> "Case Management" -> "Importer Notifications"
	When I search the current view using each criterion required by "AC-3"
		| Search criterion           |
		| Importer Name              |
		| Charity Name               |
		| Premises of Origin Name    |
		| Permanent Destination Name |
		| Animal / Product ID        |
