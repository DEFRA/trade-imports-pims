Feature: View Post Import Check

@issue:62908
Scenario: A caseworker views PIC by Allocated Area
	Given I am logged in to the 'EU Imports' app as 'a caseworker'
	And I have navigated to 'Case Management' -> 'Case Management' -> 'Post Import Checks'
	Then I see a 'PIC by Allocated Area' view with the following columns
		| Related Import Record | Related ITAHC | Import Risk Level (Related Import Record) | Country of Origin (Related Import Record) | Place of Origin - Country (Related Import Record) | Commodity Code (Related Import Record) | Commodity Type (Related Import Record) | Quantity (Related Import Record) | Devolved Office (Related Import Record) | Region / Area Allocated to | Place of Origin Trust Level (Related Import Record) | SAM Work Schedule Number | Samples / Tests Required | Sampling Required? | Number of Animals Tested | Customer Number | CPH Number | Date of Import (Related Import Record) | Date Visit Allocated | Reply from AHO Due | Date of Visit | IV17 Received Date | Post Import Check Outcome | Reason Not Visited | Reason For Not Visiting | Created On | Created By | Owner | Transporter Organisation (Related Import Record) | Place of Destination Organisation (Related Import Record) | Place of Destination - Address - Line 1 (Related Import Record) | Place of Destination - Postcode (Related Import Record) | BCP or Port of Entry (Related Import Record) | Importer Contact Name (Related Import Record) | Importer Organisation (Related Import Record) | Place of Origin Organisation (Related Import Record) | Place of Origin Contact Name (Related Import Record) | Created By (Delegate) |
	And the view is sorted by the 'Created On' column in descending order
