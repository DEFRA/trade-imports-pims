Feature: View Import Records

@issue:62908
Scenario: A caseworker views Import Records by Owner
	Given I am logged in to the 'EU Imports' app as 'a caseworker'
	And I have navigated to 'Case Management' -> 'Case Management' -> 'Import Records'
	Then I see a 'Import Records by Owner' view with the following columns
		| Name | Primary HC | Primary Importer Notification Id | Devolved Office | Region / Area Allocated to | Commodity Code | Commodity Identifiers | Commodity Type | Purpose of Movement | Certified For | Purpose of Consignment | Internal Market Purpose | Country of Origin | Consignor Address Country (Primary Importer Notification Id) | Place of Origin - Country | Consignee Address Country (Primary Importer Notification Id) | Date of Import | Import Risk Level | Date Importer Notification Received | Place of Origin Organisation | Verified Place of Origin | Trust Level (Verified Place of Origin) | Place of Destination Organisation | Place of Destination - Address - City | Place of Destination - Postcode | Created On | Completion Summary | Created By | Owner | Quantity | OV Name (Primary HC) | Consignor Company Name (Primary Importer Notification Id) | Person Responsible Company Id (Primary Importer Notification Id) | Person Responsible Email (Primary Importer Notification Id) | Transporter Organisation | Owner (Primary Importer Notification Id) | Importer Notification Received within timescales |
	And the view is sorted by the 'Created On' column in descending order