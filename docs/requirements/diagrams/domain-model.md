# Domain Model

This diagram shows the core entity relationships in PIMS.

```mermaid
classDiagram
    class ImportRecord {
        +String uniqueReferenceNumber
        +String importRecordType
        +String devolvedOffice
        +String importerName
        +String countryOfOrigin
        +Date dateOfImport
        +String commodityType
        +String importRiskLevel
        +Boolean postImportChecksRequired
        +String postImportChecksRequiredReason
        +Boolean movedToCompletion
        +Date movedToCompletionDate
        +String regionAreaAllocatedTo
    }

    class ITAHC {
        +String certificateReferenceNumber
        +Date tracesNotificationReceivedDate
        +String officialVeterinarianOrInspector
        +String localVeterinaryUnit
        +String localReference
        +String devolvedOffice
        +Boolean relatedImportRecord
        +String tracesSpeciesId
        +String tracesCommodityCode
    }

    class DOCOM {
        +String certificateReferenceNumber
        +String localReferenceNumber
        +String receivingCategory
        +String purpose
        +String sealNumber
        +String containerNumber
        +String devolvedOffice
        +String aphaAbpApprovalNumber
        +Date dateOfDecision
    }

    class CVED {
        +String certificateReferenceNumber
    }

    class ImporterNotification {
        +String importerName
        +Date dateOfImport
        +String premisesOfOriginCountry
        +String speciesProduct
        +String portAirportOfEntry
        +String type
        +String status
        +Date receivedDate
        +String replaces
        +String replacedBy
        +String devolvedOffice
        +String impType
        +String inspectionRequired
        +String mrnNumber
        +Boolean cloned
        +String commodityCode
        +String ipaffsSpeciesId
        +String ipaffsCommodityCode
    }

    class PlaceOfOrigin {
        +String organisationName
        +String address
        +String postcode
        +String country
        +String trustLevel
        +Boolean lockToBronze
        +Integer numberOfImportRecords
        +Integer numberOfConsecutiveSatisfactoryRecords
        +Integer numberOfImportRecordsSinceLastCheck
    }

    class PostImportCheck {
        +Date scheduledDate
        +String outcome
        +Date iv17ReceivedDate
    }

    class ImportQuery {
        +String queryNumber
        +String querySentTo
        +String summary
        +Date dateRaised
        +Date dateDueToBeResolved
        +String status
        +Date resolutionDate
    }

    class CommodityRiskLevel {
        +Lookup country
        +Lookup commodityType
        +String riskLevel
    }

    class GoldBronzeCommodity {
        +String name
        +Lookup commodityType
    }

    class InspectionCoverageRule {
        +String ruleName
        +String riskLevel
        +Integer numberOfRecordsUntilInspection
    }

    class CounterHistory {
        +String counterHistoryType
        +String operation
        +String reason
        +Integer previousValue
        +Integer currentValue
    }

    class GeographicTeam {
        +String name
    }

    class MatchRecord {
        +String statusReason
        +Decimal probabilityOfMatch
        +String isValidMatch
        +String rejectedReason
    }

    class Watchlist {
        +String watchType
        +Date startDate
        +Date endDate
        +String comments
    }

    class WatchlistComment {
        +String comment
        +Date createdOn
    }

    class CommodityTypeMapping {
        +Lookup commodityType
        +String tracesSpeciesId
        +String tracesCommodityCode
        +String ipaffsSpeciesId
        +String ipaffsCommodityCode
    }

    ImportRecord "1" --> "0..1" ITAHC : primaryITAHC
    ImportRecord "1" --> "0..1" DOCOM : primaryDOCOM
    ImportRecord "1" --> "0..1" ImporterNotification : primaryImporterNotification
    ImportRecord "1" --> "0..1" PlaceOfOrigin : verifiedPlaceOfOrigin
    ImportRecord "1" --> "*" PostImportCheck : hasPostImportChecks
    ImportRecord "1" --> "*" ImportQuery : hasQueries
    ImportRecord "*" --> "1" GeographicTeam : assignedTo
    ImportRecord "1" --> "*" CounterHistory : counterChanges

    ITAHC "0..1" --> "0..1" ITAHC : replacedBy
    ITAHC "0..1" --> "0..1" ITAHC : replaces

    DOCOM "0..1" --> "0..1" DOCOM : replacedBy
    DOCOM "0..1" --> "0..1" DOCOM : replaces

    PlaceOfOrigin "1" --> "*" ImportRecord : linkedImportRecords
    PlaceOfOrigin "1" --> "*" CounterHistory : counterChanges

    GoldBronzeCommodity "*" --> "*" CommodityRiskLevel : appliesToCountries
    InspectionCoverageRule "1" --> "*" ImportRecord : governsInspectionOf
    ImporterNotification "0..1" --> "0..1" ImportRecord : matchedTo
    ImporterNotification "0..1" --> "0..1" ImporterNotification : replacedBy
    ImporterNotification "0..1" --> "0..1" ImporterNotification : replaces

    MatchRecord "*" --> "0..1" ITAHC : candidateITAHC
    MatchRecord "*" --> "0..1" DOCOM : candidateDOCOM
    MatchRecord "*" --> "0..1" ImporterNotification : candidateImporterNotification
    MatchRecord "*" --> "0..1" ImportRecord : resultingImportRecord
    Watchlist "1" --> "*" WatchlistComment : hasComments
    Watchlist "1" --> "*" ITAHC : flags
    Watchlist "1" --> "*" ImportRecord : flags
    CommodityTypeMapping "*" --> "1" ImportRecord : determinesCommodityTypeOf
```
