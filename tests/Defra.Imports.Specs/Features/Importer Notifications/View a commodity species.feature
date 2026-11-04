@issue:US-044
Feature: View a commodity species

@acceptance-criteria:us-044-1
Scenario: A caseworker views a commodity species
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
