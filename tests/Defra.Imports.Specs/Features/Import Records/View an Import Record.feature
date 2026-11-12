@issue:US-001
Feature: View an Import Record

# Labels as they appear on the form, which differ from AC-1: 'Point of Entry' (Port of Entry), 'Purpose of Movement' (Purpose for Movement), 'IV65 Sent?' (IV65 Sent).
@acceptance-criteria:us-001-1 @acceptance-criteria:us-001-10 @acceptance-criteria:us-001-16
Scenario: A caseworker views an Import Record's summary details
	Given I am logged in to the 'EU Imports' app as 'a caseworker'
	And I have opened an Import Record
	When I select the 'Summary' tab
	Then I see the following fields
		| Field                                             |
		| Import Record Type                                |
		| Imp Type                                          |
		| Devolved Office                                   |
		| Cloned                                            |
		| Primary Importer Notification                     |
		| GB Import Health Certificate                      |
		| Traces Export Health Certificate                  |
		| ITAHC Reference                                   |
		| DOCOM Reference                                   |
		| Region / Area Allocated to                        |
		| Date of Import                                    |
		| MRN Number                                        |
		| General Comments                                  |
		| Commodity Type                                    |
		| Commodity Code                                    |
		| Commodity Notes                                   |
		| Quantity                                          |
		| Unit                                              |
		| Weight (KG)                                       |
		| Purpose of Movement                               |
		| Country of Origin                                 |
		| Point of Entry                                    |
		| Import Risk Level                                 |
		| Inspection Required                               |
		| IV65 Sent?                                        |
		| Warble Fly Treatment Declaration Required         |
		| Date Importer Notification Received               |
		| Importer Notification Received within timescales  |
		# Intended Use of Commodity is the Purpose of Consignment field (checked for a CHED journey below) and Horse Name is part of Commodity ID Types (checked when an Import Record is created from a notification), so neither is a gap.
		# Defect: Consignment Risk / Impact is not on the Import Record form or entity.
		# | Consignment Risk / Impact |
		# Defect: the per-animal Commodity Permanent Address Information subgrid for CHED-A records (AC-1) is not on the Import Record form.
		# | Commodity Permanent Address Information |
		# Defect: Port of Exit and Port of Exit Date are not on the Import Record form or entity, so AC-15 cannot be met.
		# | Port of Exit              |
		# | Port of Exit Date         |
	And the 'Importer Notification Received within timescales' switch is set to No

# Labels as they appear on the form, which differ from AC-1: 'Importer Organisation' (Importer Name), 'Line 1' to 'Line 3' (Address Line 1-3), 'Importer Telephone' and 'Importer Email' (Telephone, Email).
@acceptance-criteria:us-001-1
Scenario: A caseworker views an Import Record's importer details
	Given I am logged in to the 'EU Imports' app as 'a caseworker'
	And I have opened an Import Record
	When I select the 'Importer' tab
	Then I see the following fields
		| Field                 |
		| Importer Organisation |
		| Contact Name          |
		| Importer Telephone    |
		| Importer Email        |
		| Line 1                |
		| Line 2                |
		| Line 3                |
		| City                  |
		| Postcode              |
		| Country               |

@acceptance-criteria:us-001-1
Scenario: A caseworker views an Import Record's place of origin details
	Given I am logged in to the 'EU Imports' app as 'a caseworker'
	And I have opened an Import Record
	When I select the 'Place of Origin' tab
	Then I see the following fields
		| Field                        |
		| Place of Origin Organisation |
		| Contact Name                 |
		| Line 1                       |
		| Line 2                       |
		| Line 3                       |
		| City                         |
		| Postcode                     |
		| Country                      |

# Labels as they appear on the form, which differ from AC-1: the tab is 'Destination(s)' and the fields are prefixed 'Place of Destination' and 'Permanent Destination'.
@acceptance-criteria:us-001-1
Scenario: A caseworker views an Import Record's destination details
	Given I am logged in to the 'EU Imports' app as 'a caseworker'
	And I have opened an Import Record
	When I select the 'Destination(s)' tab
	Then I see the following fields
		| Field                              |
		| Place of Destination Organisation  |
		| Place of Destination Telephone     |
		| Place of Destination Email         |
		| Permanent Destination Organisation |
		| Permanent Destination Telephone    |
		| Permanent Destination Email        |

@acceptance-criteria:us-001-1 @acceptance-criteria:us-001-14
Scenario: A caseworker views an Import Record's transporter details
	Given I am logged in to the 'EU Imports' app as 'a caseworker'
	And I have opened an Import Record
	When I select the 'Transporter' tab
	Then I see the following fields
		| Field                    |
		| Transporter Organisation |
		| Transporter Telephone    |
		| Transporter Email        |
		| Type                     |
		| Line 1                   |
		| Line 2                   |
		| Line 3                   |
		| City                     |
		| Postcode                 |
		| Country                  |
		# Defect: the Transport to PoE and Transport after PoE fields required by AC-1 (ID of Transport, Estimated Arrival Date and Time, Departure Date and Time, Estimated Journey Time) are not on the Import Record form.
		# | ID of Transport               |
		# | Estimated Arrival Date        |
		# | Estimated Arrival Time        |
		# | Departure Date                |
		# | Departure Time                |
		# | Estimated Journey Time (mins) |

@acceptance-criteria:us-001-1
Scenario: A caseworker views an Import Record's transport information for a CHED-A Import Record
	Given I am logged in to the 'EU Imports' app as 'a caseworker'
	And I have opened an Import Record with an Import Record Type of 'CHED-A'
	When I select the 'Transporter' tab
	Then I see the following fields
		| Field                |
		| Means of Transport   |
		| Documentation Number |

@acceptance-criteria:us-001-1 @acceptance-criteria:us-001-4
Scenario: A caseworker views the Primary HC of an ITAHC Import Record
	Given I am logged in to the 'EU Imports' app as 'a caseworker'
	And I have opened an Import Record with an Import Record Type of 'ITAHC'
	When I select the 'Summary' tab
	Then I see the following fields
		| Field      |
		| Primary HC |

@acceptance-criteria:us-001-1
Scenario: A caseworker views an Import Record's consignment details for a CHED journey
	Given I am logged in to the 'EU Imports' app as 'a caseworker'
	And I have opened an Import Record for a CHED journey
	When I select the 'Summary' tab
	Then I see the following fields
		| Field                  |
		| Purpose of Consignment |
		| Number of Packages     |
		| Certified For          |

@acceptance-criteria:us-001-1
Scenario: A caseworker views the internal market purpose of an Import Record for the internal market
	Given I am logged in to the 'EU Imports' app as 'a caseworker'
	And I have opened an Import Record for a CHED journey with a Purpose of Consignment of 'Internal Market'
	When I select the 'Summary' tab
	Then I see the following fields
		| Field                   |
		| Internal Market Purpose |

@acceptance-criteria:us-001-8
Scenario: A caseworker views an Import Record's DOCOM details
	Given I am logged in to the 'EU Imports' app as 'a caseworker'
	And I have opened an Import Record
	When I select the 'DOCOM' tab
	Then I see the following fields
		| Field              |
		| DOCOM Category     |
		| Requested POD      |
		| Date POD Requested |
		| Reply Received     |
