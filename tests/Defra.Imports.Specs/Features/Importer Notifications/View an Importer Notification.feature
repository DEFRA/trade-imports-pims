@issue:US-003
Feature: View an Importer Notification

# Possible defect: there are more controls visible on tab than mentioned in requirements (assumed defect in requirements).
@acceptance-criteria:us-003-1 @possible-defect
Scenario: A caseworker views an Importer Notification's Importer Notification Details
	Given I am logged in to the 'EU Imports' app as 'a caseworker'
	And I have opened an Importer Notification
	When I select the 'Importer Notification Details' tab
	Then I see the following fields
		| Field                   |
		| Devolved Office         |
		| MRN Number              |
		| Cloned                  |
		| Imp Type                |
		| Inspection Required     |
		| CPH Number              |
		| Country of Origin       |
		| Arrival Date            |
		| Purpose of Movement     |
		| Purpose of Consignment  |
		| Internal Market Purpose |
		| Certified For           |
		| Number of Packages      |
		| Weight (KG)             |

# Possible defect: there are more controls visible on tab than mentioned in requirements (assumed defect in requirements).
@acceptance-criteria:us-003-1 @possible-defect
Scenario: A caseworker views an Importer Notification's commodity details
	Given I am logged in to the 'EU Imports' app as 'a caseworker'
	And I have opened an Importer Notification
	When I select the 'Commodity ' tab
	Then I see the following fields
		| Field              |
		| Commodity ID Types |
		| Commodity Code     |

# Possible defect: subgrid is visible for all types.
@acceptance-criteria:us-003-6 @possible-defect
Scenario: A caseworker views an Importer Notification's commodity permanent address information (CVEDA)
	Given I am logged in to the 'EU Imports' app as 'a caseworker'
	And I have opened an Importer Notification of type 'CVEDA'
	When I select the 'Commodity ' tab
	Then I see a 'Commodity Permanent Address information <Identifiers>' subgrid with the following columns
		| Animal ID | Passport | Microchip | Tattoo | Address Type | Address Line1 | Address Line2 | Address Line3 | City | Post Code | Telephone | Email |

# Possible defect: deployed Charity tab labels the email field 'Consignor Email' (not 'Email') and the postcode field 'Address Postcode' (not 'Postcode') as required.
# Possible defect: there are more controls visible on tab than mentioned in requirements (assumed defect in requirements).
@acceptance-criteria:us-003-1 @possible-defect
Scenario: A caseworker views an Importer Notification's charity details
	Given I am logged in to the 'EU Imports' app as 'a caseworker'
	And I have opened an Importer Notification which is importing from a charity
	When I select the 'Charity' tab
	Then I see the following fields
		| Field           |
		| Individual Name |
		| Company Name    |
		| Email           |
		| Telephone       |
		| Address Line 1  |
		| Postcode        |

@acceptance-criteria:us-003-1
Scenario: A caseworker views an Importer Notification's person responsible details
	Given I am logged in to the 'EU Imports' app as 'a caseworker'
	And I have opened an Importer Notification
	When I select the 'Person Responsible' tab
	Then I see the following fields
		| Field   |
		| Name    |
		| Email   |
		| Phone   |
		| Address |
		| Country |

# Possible defect: there are more controls visible on tab than mentioned in requirements (assumed defect in requirements).
@acceptance-criteria:us-003-1 @possible-defect
Scenario: A caseworker views an Importer Notification's importer details
	Given I am logged in to the 'EU Imports' app as 'a caseworker'
	And I have opened an Importer Notification
	When I select the 'Importer' tab
	Then I see the following fields
		| Field          |
		| Name           |
		| Email          |
		| Telephone      |
		| Address Line 1 |
		| Postcode       |
		| Country        |

# Possible defect: there are more controls visible on tab than mentioned in requirements (assumed defect in requirements).
@acceptance-criteria:us-003-1 @possible-defect
Scenario: A caseworker views an Importer Notification's place of origin details
	Given I am logged in to the 'EU Imports' app as 'a caseworker'
	When I select the 'Place of Origin' tab
	Then I see the following fields
		| Field          |
		| Name           |
		| Email          |
		| Telephone      |
		| Address Line 1 |
		| Postcode       |
		| Country        |

# Possible defect: there are more controls visible on tab than mentioned in requirements (assumed defect in requirements).
@acceptance-criteria:us-003-1 @possible-defect
Scenario: A caseworker views an Importer Notification's place of destination details
	Given I am logged in to the 'EU Imports' app as 'a caseworker'
	When I select the 'Place of Destination' tab
	Then I see the following fields
		| Field          |
		| Name           |
		| Email          |
		| Telephone      |
		| Address Line 1 |
		| Postcode       |
		| Country        |

@acceptance-criteria:us-003-1
Scenario: A caseworker views an Importer Notification's permanent address details
	Given I am logged in to the 'EU Imports' app as 'a caseworker'
	When I select the 'Permanent Addresses' tab
	Then I see the following fields
		| Field                           |
		| Permanent Addresses <Subgrid_3> |

# Possible defect: there are more controls visible on tab than mentioned in requirements (assumed defect in requirements).
@acceptance-criteria:us-003-1 @possible-defect
Scenario: A caseworker views an Importer Notification's transporter details
	Given I am logged in to the 'EU Imports' app as 'a caseworker'
	When I select the 'Transporter' tab
	Then I see the following fields
		| Field                  |
		| Name                   |
		| Email                  |
		| Telephone              |
		| Address Line 1         |
		| Postcode               |
		| BCP or Port of Entry   |
		| Means of Transport     |
		| Estimated Arrival Date |
		| Departure Date         |
		| Destination Country    |
