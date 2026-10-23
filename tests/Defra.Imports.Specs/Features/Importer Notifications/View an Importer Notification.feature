@issue:US-003
Feature: View an Importer Notification

@acceptance-criteria:us-003-1 @acceptance-criteria:us-003-2 @issue:US-044 @acceptance-criteria:us-044-4 @acceptance-criteria:us-044-5 @issue:US-029 @acceptance-criteria:us-029-7 @issue:US-006 @acceptance-criteria:us-044-1
Scenario: A caseworker views an Importer Notification's importer notification details
	Given I am logged in to the 'EU Imports' app as 'a caseworker'
	And I have opened an Importer Notification
	When I select the 'Importer Notification Details' tab
	# 'Submitted By Is Control User?' and 'Veterinary Document' were not traced back to requirements but inferred from the implementation
	Then I see the following fields
		| Field                         |
		| Devolved Office               |
		| MRN Number                    |
		| Cloned                        |
		| Imp Type                      |
		| Inspection Required           |
		| CPH Number                    |
		| Country of Origin             |
		| Arrival Date                  |
		| Purpose of Movement           |
		| Weight (KG)                   |
		| Reference Number              |
		| Version                       |
		| Submission Date               |
		| Submitted By Display Name     |
		| Last Updated                  |
		| Last Updated By Display Name  |
		| Submitted By Is Control User? |
		| Type                          |
		| Health Certificate Attached   |
		| Veterinary Document           |
		| Importing From Charity        |
		| Region of Origin              |
		| Arrival Time                  |
		| Commodities Number of Animals |
	And I do not see the following fields
		| Field                  |
		| Purpose of Consignment |
		| Certified For          |
		| Number of Packages     |
	And I see a 'Watch Flags <Subgrid_WatchFlags>' subgrid with the following columns
		| Name | Watch List | Created On |
	And I see an 'IPAFFS Documents (Importer Notification) <documents_Subgrid>' subgrid with the following columns
		| Document Type | Document Reference | Document Issue Date | Document URL |

@acceptance-criteria:us-003-1 @acceptance-criteria:us-003-2 @issue:US-044 @acceptance-criteria:us-044-1 @acceptance-criteria:us-044-7
Scenario: A caseworker views an Importer Notification's importer notification details (CHEDA/CVEDA)
	Given I am logged in to the 'EU Imports' app as 'a caseworker'
	And I have opened an Importer Notification of type CHEDA or CVEDA
	When I select the 'Importer Notification Details' tab
	Then I see the following fields
		| Field                  |
		| Purpose of Consignment |
		| Certified For          |
		| Number of Packages     |
	And I do not see the following fields
		| Field               |
		| Purpose of Movement |
		| Weight (KG)         |
		| Imp Type            |

# Defect: the Type field on the Importer Notification Details tab shows 'IMP' rather than 'Importer Notification' as required by US-044 AC-5 (BR-038).
@issue:US-044 @acceptance-criteria:us-044-5 @ignore @defect
Scenario: A caseworker views the IMP notification type labelled as Importer Notification
	Given I am logged in to the 'EU Imports' app as 'a caseworker'
	And I have opened an Importer Notification of type 'IMP'
	When I select the 'Importer Notification Details' tab
	Then I see a value of 'Importer Notification' in the 'Type' field

@issue:US-044 @acceptance-criteria:us-044-4
Scenario: A caseworker views Health Certificate Attached as Yes when a health certificate document with a URL is attached
	Given I am logged in to the 'EU Imports' app as 'a caseworker'
	And I have opened an Importer Notification with a health certificate document
	When I select the 'Importer Notification Details' tab
	Then the 'Health Certificate Attached' checkbox is checked

@issue:US-044 @acceptance-criteria:us-044-4
Scenario: A caseworker views Health Certificate Attached as No when no health certificate document is attached
	Given I am logged in to the 'EU Imports' app as 'a caseworker'
	And I have opened an Importer Notification without a health certificate document
	When I select the 'Importer Notification Details' tab
	Then the 'Health Certificate Attached' checkbox is unchecked

@issue:US-044 @acceptance-criteria:us-044-4
Scenario: A caseworker can edit the Health Certificate Attached field
	Given I am logged in to the 'EU Imports' app as 'a caseworker'
	And I have opened an Importer Notification
	When I select the 'Importer Notification Details' tab
	Then I can edit the 'Health Certificate Attached' field

@issue:US-044 @acceptance-criteria:us-044-4
Scenario: A caseworker views Health Certificate Attached immediately beneath Imp Type
	Given I am logged in to the 'EU Imports' app as 'a caseworker'
	And I have opened an Importer Notification
	When I select the 'Importer Notification Details' tab
	Then the 'Health Certificate Attached' field is immediately beneath the 'Imp Type' field

@acceptance-criteria:us-003-1
Scenario: A caseworker views an Importer Notification's internal market purpose (CVED/CHEDA)
	Given I am logged in to the 'EU Imports' app as 'a caseworker'
	And I have opened an Importer Notification with a Purpose of Consignment of 'Internal Market'
	When I select the 'Importer Notification Details' tab
	Then I see the following fields
		| Field                   |
		| Internal Market Purpose |

@acceptance-criteria:us-003-1 @issue:US-056 @acceptance-criteria:us-056-3 @issue:US-044 @acceptance-criteria:us-044-1
Scenario: A caseworker views an Importer Notification's commodity details
	Given I am logged in to the 'EU Imports' app as 'a caseworker'
	And I have opened an Importer Notification
	When I select the 'Commodity' tab
	Then I see the following fields
		| Field              |
		| Commodity ID Types |
		| Commodity Code     |
	And I see a 'Commodity details <commodity_details>' subgrid with the following columns
		| Importer Notification | Commodity Species Id | Commodity Species Name | Number of Animals | Number of Packages | Created On |

# Possible defect: subgrid is visible for all types.
@acceptance-criteria:us-003-6 @possible-defect @issue:US-044 @acceptance-criteria:us-044-1
Scenario: A caseworker views an Importer Notification's commodity permanent address information (CHEDA/CVEDA)
	Given I am logged in to the 'EU Imports' app as 'a caseworker'
	And I have opened an Importer Notification of type CHEDA or CVEDA
	When I select the 'Commodity' tab
	Then I see a 'Commodity Permanent Address information <Identifiers>' subgrid with the following columns
		| Animal ID | Passport | Microchip | Tattoo | Address Type | Address Line1 | Address Line2 | Address Line3 | City | Post Code | Telephone | Email |

@acceptance-criteria:us-003-1
Scenario: A caseworker views an Importer Notification's charity details
	Given I am logged in to the 'EU Imports' app as 'a caseworker'
	And I have opened an Importer Notification which is importing from a charity
	When I select the 'Charity' tab
	# 'UK Telephone', 'International Telephone' and 'Other Identifier' were not explicitly traced back to individual requirements but inferred from the implementation (should be verified against D365 schema reference in US-044 AC-1)
	Then I see the following fields
		| Field                   |
		| Individual Name         |
		| Company Name            |
		| Consignor Email         |
		| Telephone               |
		| Address Line 1          |
		| Address Postcode        |
		| UK Telephone            |
		| International Telephone |
		| Address Line 2          |
		| Address Line 3          |
		| Address City            |
		| Address Country         |
		| Approval Number         |
		| Status                  |
		| Type                    |
		| Other Identifier        |

@acceptance-criteria:us-003-1 @issue:US-044 @acceptance-criteria:us-044-1
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

@acceptance-criteria:us-003-1 @issue:US-044 @acceptance-criteria:us-044-1
Scenario: A caseworker views an Importer Notification's importer details
	Given I am logged in to the 'EU Imports' app as 'a caseworker'
	And I have opened an Importer Notification
	When I select the 'Importer' tab
	Then I see the following fields
		| Field           |
		| Name            |
		| Email           |
		| Telephone       |
		| Address Line 1  |
		| Postcode        |
		| Country         |
		| Approval Number |
		| Status          |
		| Type            |
		| Address Line 2  |
		| Address Line 3  |
		| City            |

@acceptance-criteria:us-003-1 @issue:US-044 @acceptance-criteria:us-044-1
Scenario: A caseworker views an Importer Notification's place of origin details
	Given I am logged in to the 'EU Imports' app as 'a caseworker'
	And I have opened an Importer Notification
	When I select the 'Place of Origin' tab
	Then I see the following fields
		| Field           |
		| Name            |
		| Email           |
		| Telephone       |
		| Address Line 1  |
		| Postcode        |
		| Country         |
		| Approval Number |
		| Status          |
		| Type            |
		| Address Line 2  |
		| Address Line 3  |
		| City            |

@acceptance-criteria:us-003-1 @issue:US-044 @acceptance-criteria:us-044-1
Scenario: A caseworker views an Importer Notification's place of destination details
	Given I am logged in to the 'EU Imports' app as 'a caseworker'
	And I have opened an Importer Notification
	When I select the 'Place of Destination' tab
	Then I see the following fields
		| Field              |
		| Name               |
		| Email              |
		| Telephone          |
		| Address Line 1     |
		| Postcode           |
		| Country            |
		| Approval Number    |
		| Status             |
		| Type               |
		| Address Line 2     |
		| Address Line 3     |
		| City               |
		| Permanent Address? |

@acceptance-criteria:us-003-1 @issue:US-044 @acceptance-criteria:us-044-1
Scenario: A caseworker views an Importer Notification's permanent address details
	Given I am logged in to the 'EU Imports' app as 'a caseworker'
	And I have opened an Importer Notification
	When I select the 'Permanent Addresses' tab
	# Subgrid columns were not itemised in requirements (US-003 AC-1 confirms the Permanent Destination field group but not this subgrid's column set) but inferred from the implementation
	Then I see a 'Permanent Addresses <Subgrid_3>' subgrid with the following columns
		| Name | Company Name | Email | Telephone | Address Line 1 | Address Line 2 | Address Line 3 | Address City | Address Postcode | Address Country | Approval Number | Type | Status |

@acceptance-criteria:us-003-1 @acceptance-criteria:us-003-9 @issue:US-001 @acceptance-criteria:us-001-2 @issue:US-044 @acceptance-criteria:us-044-1
Scenario: A caseworker views an Importer Notification's transporter details
	Given I am logged in to the 'EU Imports' app as 'a caseworker'
	And I have opened an Importer Notification
	When I select the 'Transporter' tab
	Then I see the following fields
		| Field                                                            |
		| Name                                                             |
		| Email                                                            |
		| Telephone                                                        |
		| Address Line 1                                                   |
		| Postcode                                                         |
		| BCP or Port of Entry                                             |
		| Means of Transport                                               |
		| Estimated Arrival Date                                           |
		| Departure Date                                                   |
		| Destination Country                                              |
		| Approval Number                                                  |
		| Status                                                           |
		| Type                                                             |
		| ID of Transport <defraimp_meansoftransporttoentrypointid>        |
		| Document <defraimp_meansoftransporttoentrypointdocument>         |
		| Estimated Arrival Time                                           |
		| Estimated Journey Time (mins)                                    |
		| Means of Transport <defraimp_meansoftransportfromentrypointtype> |
		| ID of Transport <defraimp_meansoftransportfromentrypointid>      |
		| Document <defraimp_meansoftransportfromentrypointdocument>       |
		| Departure Time                                                   |
		| Address Line 2                                                   |
		| Address Line 3                                                   |
		| City                                                             |
		| Country                                                          |
		| Port of Exit                                                     |
		| Port of Exit Date                                                |
		| Exit Border Control Post                                         |
		| Exit Border Control Post Date                                    |
		| Route Transiting States                                          |
	And I see a 'Countries Of Transit <subgrid_countries_of_transit>' subgrid with the following columns
		| Official Country Name |

@issue:US-044 @acceptance-criteria:us-044-1
Scenario: A caseworker views an Importer Notification's permanent address entry
	Given I am logged in to the 'EU Imports' app as 'a caseworker'
	And I have opened a permanent address of an Importer Notification
	Then I see the following fields
		| Field                    |
		| Individual Name          |
		| Importer Notification    |
		| Id                       |
		| Type                     |
		| Status                   |
		| Approval Number          |
		| Other Identifier         |
		| Company Name             |
		| Address Line 1           |
		| Address Line 2           |
		| Address Line 3           |
		| Address City             |
		| Address Post             |
		| Address Country          |
		| Email                    |
		| Telephone                |
		| UK Telephone             |
		| International Phone Code |
		| International Telephone  |

@issue:US-044 @acceptance-criteria:us-044-1
Scenario: A caseworker views an Importer Notification's commodity species details
	Given I am logged in to the 'EU Imports' app as 'a caseworker'
	And I have opened a commodity species of an Importer Notification
	Then I see the following fields
		| Field                 |
		| Complement Id         |
		| Complement Name       |
		| Commodity Id          |
		| Commodity Description |
		| Importer Notification |
		| Number of Animals     |
		| Number of Packages    |
		| Species Id            |
		| Species Name          |
		| Species Nomination    |
		| Species Type Name     |
		| Species Type          |
		| Species Class Name    |
		| Species Class         |
		| Species Family Name   |
		| Species Family        |
		| Species Common Name   |

@issue:US-044 @acceptance-criteria:us-044-1
Scenario: A caseworker views an Importer Notification's parameter set identifier details
	Given I am logged in to the 'EU Imports' app as 'a caseworker'
	And I have opened a parameter set identifier of an Importer Notification
	Then I see the following fields
		| Field                                          |
		| Commodity Complement Parameter Set             |
		| Species Number                                 |
		| Data                                           |
		| Importer Notification Id                       |
		| Permanent Address                              |
		| Is Place Of Destination The Permanent Address  |
		| Type                                           |
		| Address Line1                                  |
		| Address Line2                                  |
		| city                                           |
		| Postal Zip Code                                |
		| Email                                          |
		| Telephone                                      |

@issue:US-044 @acceptance-criteria:us-044-1
Scenario: A caseworker views an Importer Notification's parameter set key data pair details
	Given I am logged in to the 'EU Imports' app as 'a caseworker'
	And I have opened a parameter set key data pair of an Importer Notification
	Then I see the following fields
		| Field                              |
		| Commodity Complement Parameter Set |
		| Key                                |
		| Data                               |

@issue:US-044 @acceptance-criteria:us-044-1
Scenario: A caseworker views an Importer Notification's commodity complement parameter set details
	Given I am logged in to the 'EU Imports' app as 'a caseworker'
	And I have opened a commodity complement parameter set of an Importer Notification
	Then I see the following fields
		| Field                 |
		| Complement Id         |
		| Species Id            |
		| Importer Notification |
		| Species Name          |
		| Species Quantity      |
	And I can see the 'Parameter Set Identifiers (Commodity Complement Parameter Set) <Identifiers>' subgrid
	And I can see the 'Parameter Set Key Data Pairs (Commodity Complement Parameter Set) <KeyDataPairs>' subgrid

# Possible defect: the D365 schema (IMTA-7201) marks the fields below as displayed on the Importer Notification form, but the form does not have them.
@issue:US-044 @acceptance-criteria:us-044-1 @possible-defect
Scenario: A caseworker does not see the schema fields missing from the Person Responsible tab
	Given I am logged in to the 'EU Imports' app as 'a caseworker'
	And I have opened an Importer Notification
	When I select the 'Person Responsible' tab
	Then I do not see the following fields
		| Field |
		| Type  |
		| Fax   |

# Possible defect: see the Person Responsible scenario.
@issue:US-044 @acceptance-criteria:us-044-1 @possible-defect
Scenario Outline: A caseworker does not see the schema fields missing from the <Tab> tab
	Given I am logged in to the 'EU Imports' app as 'a caseworker'
	And I have opened an Importer Notification
	When I select the '<Tab>' tab
	Then I do not see the following fields
		| Field                   |
		| Individual Name         |
		| UK Telephone            |
		| International Telephone |
		| Other Identifier        |

	Examples:
		| Tab                  |
		| Importer             |
		| Place of Origin      |
		| Place of Destination |
		| Transporter          |

# Possible defect: see the Person Responsible scenario.
@issue:US-044 @acceptance-criteria:us-044-1 @possible-defect
Scenario: A caseworker does not see the schema commodity fields missing from the Importer Notification Details tab
	Given I am logged in to the 'EU Imports' app as 'a caseworker'
	And I have opened an Importer Notification
	When I select the 'Importer Notification Details' tab
	Then I do not see the following fields
		| Field                           |
		| Commodities Total Gross Weight  |
		| Commodities Total Net Weight    |
		| Commodity Intended For          |
		| Consigned Country               |

# Ignored: the test data's document URL is a random placeholder, and the real URL comes from the IPAFFS base URL configuration plus the reference number, so whether it resolves can't be checked without real IPAFFS documents.
@issue:US-044 @acceptance-criteria:us-044-10 @ignore
Scenario: A caseworker sees a document URL and opens it from the Importer Notification
	Given I am logged in to the 'EU Imports' app as 'a caseworker'
	And I have opened an Importer Notification
	When I select the 'Importer Notification Details' tab
	Then I see a document URL link in the documents subgrid
	When I click the first document URL in the documents subgrid
	Then the document opens in a new tab
