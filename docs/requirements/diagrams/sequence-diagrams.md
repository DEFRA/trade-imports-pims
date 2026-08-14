# Sequence Diagrams

End-to-end sequence diagrams for core PIMS processes. These complement the process flowcharts by showing message timing and actor interaction.

## Process 1 - TRACES ITAHC Ingest to Import Record Creation

```mermaid
sequenceDiagram
    autonumber
    actor TRACES as TRACES Classic
    participant CIT as CIT Team Mailbox
    participant AZ as Azure Integration
    participant SB as Azure Service Bus
    participant PIMS as PIMS D365
    participant WF as PIMS Workflows
    participant TEAM as Regional Team Assignment
    actor CW as Caseworker

    TRACES->>CIT: Send ITAHC notification email
    CIT->>AZ: Forward/route inbound payload
    AZ->>SB: Publish normalized ITAHC message
    SB->>PIMS: Deliver ITAHC message
    PIMS->>WF: Create ITAHC record
    WF->>WF: Validate and map required fields
    WF->>PIMS: Auto-create linked Import Record
    WF->>TEAM: Determine APHA region/team
    TEAM-->>PIMS: Return assigned team
    PIMS-->>CW: Import Record available for review
    CW->>PIMS: Review and continue case handling
```

## Process 2 - IPAFFS Importer Notification Receipt and Update

```mermaid
sequenceDiagram
    autonumber
    actor IMP as Importer
    participant IPAFFS as IPAFFS
    participant SB as Azure Service Bus
    participant PIMS as PIMS D365
    participant WF as PIMS Workflows
    actor CW as Caseworker

    IMP->>IPAFFS: Submit/amend/cancel notification
    IPAFFS->>SB: Push notification JSON event
    SB->>PIMS: Deliver event
    PIMS->>WF: Trigger notification processing
    alt Notification does not exist
        WF->>PIMS: Create Importer Notification
    else Notification exists
        WF->>PIMS: Update existing Importer Notification
    end
    WF->>PIMS: Link or propose match to Import Record/ITAHC
    PIMS-->>CW: Show notification and match context
    CW->>PIMS: Review and confirm/adjust match
```

## Process 3 - Automated Risk Assessment and Inspection Decision

```mermaid
sequenceDiagram
    autonumber
    participant PIMS as PIMS Import Record
    participant RULE as Risk Rules Engine
    participant CATALOG as Commodity Risk Configuration
    participant PO as Place of Origin
    participant COUNTER as Inspection Counter Service
    participant PIC as Post Import Check Scheduler
    actor CW as Caseworker

    PIMS->>RULE: Start risk assessment on create/update
    RULE->>CATALOG: Load commodity and coverage rules
    CATALOG-->>RULE: Return applicable risk metadata
    RULE->>PO: Get trust level and lock status
    PO-->>RULE: Return Gold/Bronze + lock details
    RULE->>COUNTER: Check random inspection threshold (P2/P3)
    COUNTER-->>RULE: Return counter state and threshold outcome
    RULE->>PIMS: Set risk level, reason, and status
    alt Post Import Check required
        RULE->>PIC: Create/schedule Post Import Check
        PIC-->>PIMS: Store PIC required flag and reason
    else No Post Import Check required
        RULE->>PIMS: Set PIC required = No
    end
    PIMS-->>CW: Display calculated risk assessment outcome
```

## Process 4 - Post Import Check Outcome and Trust Level Maintenance

```mermaid
sequenceDiagram
    autonumber
    actor CW as Caseworker
    participant PIC as Post Import Check
    participant PIMS as PIMS D365
    participant PO as Place of Origin
    participant COUNTER as Trust/Inspection Counters
    participant AUDIT as Audit History

    CW->>PIC: Record Post Import Check outcome
    PIC->>PIMS: Persist outcome against Import Record
    PIMS->>PO: Evaluate trust level impact
    alt Outcome is Satisfactory or Not Visited
        PO->>COUNTER: Increment consecutive satisfactory counter
        COUNTER-->>PO: Updated value
        alt Threshold reached and not locked to Bronze
            PO->>PO: Promote trust level to Gold
        else Threshold not reached or lock active
            PO->>PO: Keep current trust level
        end
    else Outcome is Unsatisfactory
        PO->>COUNTER: Reset satisfactory counter
        COUNTER-->>PO: Counter reset complete
        PO->>PO: Revoke/retain trust level per policy
    end
    PO->>AUDIT: Record trust level decision and rationale
    AUDIT-->>PIMS: Audit trail available on record history
```

## Process 5 - Import Query Lifecycle

```mermaid
sequenceDiagram
    autonumber
    actor CW as Caseworker
    actor EXT as Importer or Third Party
    participant PIMS as PIMS Import Query Module
    participant AUTO as Auto Number Service
    participant SLA as Due Date Tracker
    participant AUDIT as Audit History

    CW->>PIMS: Create Import Query from Import Record
    PIMS->>AUTO: Request next Query Number (RMQYY-NNNN)
    AUTO-->>PIMS: Return unique Query Number
    PIMS->>SLA: Calculate due date/status clocks
    PIMS-->>EXT: Send query request
    loop Until response received
        SLA->>PIMS: Mark approaching/overdue status
        PIMS-->>CW: Show query in active/overdue views
        CW-->>EXT: Follow up if overdue
    end
    EXT-->>PIMS: Submit response/evidence
    CW->>PIMS: Record notes and close query as resolved
    PIMS->>AUDIT: Write closure and timestamp history
```

## Process 6 - Failed TRACES Receipt Handling and Reprocessing

```mermaid
sequenceDiagram
    autonumber
    participant TRACES as TRACES Classic
    participant AZ as Azure Integration
    participant DLQ as Dead Letter Queue
    participant PIMS as PIMS Failed Receipt View
    actor CW as Caseworker
    participant REPROC as Reprocess Workflow

    TRACES->>AZ: Send ITAHC/DOCOM payload
    AZ->>AZ: Attempt transform and route
    alt Processing failure
        AZ->>DLQ: Store failed message and error details
        DLQ->>PIMS: Expose failure in Failed TRACES Receipts
        PIMS-->>CW: Show failure details and retry option
        CW->>REPROC: Trigger reprocess
        REPROC->>AZ: Re-submit corrected/original payload
        AZ->>PIMS: Create/update target records if successful
    else Processing success
        AZ->>PIMS: Create/update target records directly
    end
```
