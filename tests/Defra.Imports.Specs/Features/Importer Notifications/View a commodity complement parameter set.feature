Feature: View a commodity complement parameter set

@issue:US-044 @acceptance-criteria:us-044-1
Scenario: A caseworker views a commodity complement parameter set
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
