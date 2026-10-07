@issue:US-003
Feature: View an Importer Notification

@acceptance-criteria:us-003-1 @acceptance-criteria:us-003-2 @issue:US-044 @acceptance-criteria:us-044-4 @acceptance-criteria:us-044-5 @acceptance-criteria:us-044-6 @issue:US-053 @acceptance-criteria:us-053-3 @issue:US-029 @acceptance-criteria:us-029-7
Scenario: A caseworker views an Importer Notification's Importer Notification Details
	Given I am logged in to the 'EU Imports' app as 'a caseworker'
	# TODO: Implement non-parameterised binding that handles Importer Notification only. Use ImporterNotificationScenario.Builder to create an import notification with no specific configuration and navigate to form with `ActivePage.ClientApi`.
	And I have opened an Importer Notification
	When I select the 'Importer Notification Details' tab
	# 'Submitted By Display Name', 'Last Updated', 'Last Updated By Display Name', 'Submitted By Is Control User?' and 'Veterinary Document' were not traced back to requirements but inferred from the implementation
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
		| Purpose of Consignment        |
		| Internal Market Purpose       |
		| Certified For                 |
		| Number of Packages            |
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
	# Watch Flags subgrid columns were not itemised in requirements (US-058 AC-4 confirms flagging/viewing behaviour but not the exact column set) but inferred from the implementation
	And I see a 'Watch Flags <Subgrid_WatchFlags>' subgrid with the following columns
		| Name | Watch List | Created On |
	And I see an 'IPAFFS Documents (Importer Notification) <documents_Subgrid>' subgrid with the following columns
		| Document Type | Document Reference | Document Issue Date | Document URL |

@acceptance-criteria:us-003-1 @issue:US-056 @acceptance-criteria:us-056-3
Scenario: A caseworker views an Importer Notification's commodity details
	Given I am logged in to the 'EU Imports' app as 'a caseworker'
	And I have opened an Importer Notification
	When I select the 'Commodity ' tab
	Then I see the following fields
		| Field              |
		| Commodity ID Types |
		| Commodity Code     |
	And I see a 'Commodity details <commodity_details>' subgrid with the following columns
		| Importer Notification | Commodity Species Id | Commodity Species Name | Number of Animals | Number of Packages | Created On |

# Possible defect: subgrid is visible for all types.
@acceptance-criteria:us-003-6 @possible-defect
Scenario: A caseworker views an Importer Notification's commodity permanent address information (CVEDA)
	Given I am logged in to the 'EU Imports' app as 'a caseworker'
	# TODO: Implement non-parameterised binding that handles Importer Notification of a given type only. Parameter should be early-bound model enum. Use ImporterNotificationScenario.Builder to create an import notification with a builder method added for type and navigate to form with `ActivePage.ClientApi`.
	And I have opened an Importer Notification of type 'CVEDA'
	When I select the 'Commodity ' tab
	Then I see a 'Commodity Permanent Address information <Identifiers>' subgrid with the following columns
		| Animal ID | Passport | Microchip | Tattoo | Address Type | Address Line1 | Address Line2 | Address Line3 | City | Post Code | Telephone | Email |

@acceptance-criteria:us-003-1
Scenario: A caseworker views an Importer Notification's charity details
	Given I am logged in to the 'EU Imports' app as 'a caseworker'
	# TODO: Implement non-parameterised binding that handles Importer Notification importing from a charity only. Use ImporterNotificationScenario.Builder to create an import notification with a builder method added for importing from charity which causes the builder to set additional expected fields for charity and navigate to form with `ActivePage.ClientApi`.
	And I have opened an Importer Notification which is importing from a charity
	When I select the 'Charity' tab
	# 'UK Telephone', 'International Telephone' and 'Other Identifier' were not traced back to requirements but inferred from the implementation
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

@acceptance-criteria:us-003-1
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

@acceptance-criteria:us-003-1
Scenario: A caseworker views an Importer Notification's place of origin details
	Given I am logged in to the 'EU Imports' app as 'a caseworker'
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

@acceptance-criteria:us-003-1
Scenario: A caseworker views an Importer Notification's place of destination details
	Given I am logged in to the 'EU Imports' app as 'a caseworker'
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

@acceptance-criteria:us-003-1
Scenario: A caseworker views an Importer Notification's permanent address details
	Given I am logged in to the 'EU Imports' app as 'a caseworker'
	When I select the 'Permanent Addresses' tab
	Then I see the following fields
		| Field                           |
		| Permanent Addresses <Subgrid_3> |

@acceptance-criteria:us-003-1 @issue:US-001 @acceptance-criteria:us-001-2
Scenario: A caseworker views an Importer Notification's transporter details
	Given I am logged in to the 'EU Imports' app as 'a caseworker'
	When I select the 'Transporter' tab
	# 'Port of Exit' and 'Port of Exit Date' were not traced back to requirements but inferred from the implementation
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
