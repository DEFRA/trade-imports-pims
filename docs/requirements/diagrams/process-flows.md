# Process Flows

Key process flows in PIMS, derived from the source Jira stories.

---

## Process 1 — TRACES ITAHC Ingest and Import Record Creation

```mermaid
flowchart TD
    A([TRACES Classic <br/> Creates ITAHC]) --> B[Email notification <br/> to CIT team]
    B --> C[Azure Integration <br/> Intercepts and routes to <br/> Service Bus Queue]
    C --> D{Message <br/> Processed <br/> Successfully?}
    D -->|No| E[Capture failed receipt <br/> Make visible for <br/> investigation and reprocess]
    D -->|Yes| F[Create ITAHC Record <br/> in PIMS D365]
    F --> G[Auto-create <br/> Import Record <br/> linked to ITAHC]
    G --> H[Auto-assign to <br/> Regional Team]
    H --> I[Caseworker Reviews <br/> Import Record]
    I --> J[Risk Assessment Applied <br/> Automatically]
    J --> K{Post Import Check <br/> Required?}
    K -->|Yes| L[Post Import Check <br/> Scheduled]
    K -->|No| M[Import Record <br/> Completed]
    L --> N[Post Import Check <br/> Outcome Recorded]
    N --> O[Trust Level <br/> Updated on <br/> Place of Origin]
    N --> M
```

---

## Process 2 — IPAFFS Importer Notification Receipt

```mermaid
flowchart TD
    A([Importer submits <br/> notification in IPAFFS]) --> B{Status?}
    B -->|Submitted| C[IPAFFS pushes <br/> JSON to Service Bus]
    B -->|Amended| C
    B -->|Cancelled| C
    C --> D{Processed <br/> successfully?}
    D -->|No| E[Route to <br/> Dead Letter Queue]
    D -->|Yes| F{Importer Notification <br/> exists in PIMS?}
    F -->|No| G[Create Importer <br/> Notification in PIMS]
    F -->|Yes| H[Update Importer <br/> Notification in PIMS]
    G --> I[Trigger downstream <br/> Import Record create <br/> or update workflow <br/> where applicable]
    H --> I
    I --> J[Caseworker Views <br/> Importer Notification]
    J --> K[Match to ITAHC <br/> and Import Record]
```

---

## Process 6 — Match Inbound Records to Import Records

```mermaid
flowchart TD
    A([Inbound ITAHC <br/> Importer Notification <br/> IV66-related data created or updated]) --> B[Create or update <br/> Match Record]
    B --> C[Run matching logic <br/> against candidate <br/> Import Records]
    C --> D[Populate Match View <br/> with related Import <br/> Records and context]
    D --> E[Show Work Schedule <br/> Number where available]
    E --> F[Caseworker reviews <br/> likely matches]
```

---

## Process 3 — Automated Risk Assessment (P1 Gold/Bronze)

```mermaid
flowchart TD
    A([Import Record <br/> Created or Updated]) --> B{Gold/Bronze <br/> Commodity Rule <br/> Applies?}
    B -->|No| C[Set Risk Level per <br/> Commodity Risk Level Rule]
    C --> D[Post Import Check <br/> Required = Discretionary]
    B -->|Yes| E{Place of Origin <br/> Linked?}
    E -->|No| F[Post Import Check <br/> Required = Undetermined <br/> Reason: Verified Place of Origin Missing]
    E -->|Yes| G{Place of Origin <br/> Trust Level?}
    G -->|Bronze| H[Post Import Check <br/> Required = Yes <br/> Reason: Bronze Place of Origin]
    G -->|Gold| I{Import Records Since <br/> Last Check ≥ 10?}
    I -->|Yes| J[Post Import Check <br/> Required = Yes <br/> Reason: Gold Place of Origin — Inspection Coverage <br/> Reset counter to 0]
    I -->|No| K[Post Import Check <br/> Required = No <br/> Reason: No Inspection Required — Gold Place of Origin]
```

---

## Process 4 — Place of Origin Trust Level Lifecycle

```mermaid
flowchart TD
    A([Place of Origin <br/> Created]) --> B[Trust Level = Bronze]
    B --> C{Post Import <br/> Check Outcome}
    C -->|Satisfactory or <br/> Not Visited| D[Increment Consecutive <br/> Satisfactory Count]
    D --> E{Count ≥ 3 AND <br/> Lock to Bronze = No?}
    E -->|Yes| F[Promote Trust Level <br/> to Gold]
    E -->|No| G[Remain Bronze]
    C -->|Unsatisfactory| H[Reset Consecutive <br/> Satisfactory Count to 0]
    H --> I{Caseworker: <br/> Revoke Gold?}
    I -->|Yes| J[Trust Level <br/> set to Bronze]
    I -->|No| K[Trust Level <br/> remains Gold]
    F --> L{Caseworker <br/> Locks to Bronze?}
    L -->|Yes| M[Lock to Bronze = Yes <br/> Mandatory Reason Required]
    M --> N[Trust Level stays Bronze <br/> regardless of outcomes]
    L -->|No| F
    M --> O{Caseworker <br/> Unlocks?}
    O -->|Yes| P[Restore Previous <br/> Trust Level <br/> Mandatory Reason Required]
```

---

## Process 5 — Import Query Lifecycle

```mermaid
flowchart TD
    A([Caseworker identifies <br/> query on Import Record]) --> B[Create Import Query <br/> Auto-generate Query Number <br/> RMQYY-NNNN]
    B --> C[Assign Query to <br/> Caseworker]
    C --> D[Query Sent to <br/> Importer <br/> Third Party]
    D --> E{Response <br/> Received?}
    E -->|No, overdue| F[Query appears in <br/> Overdue Queries view]
    F --> G[Caseworker <br/> Follows Up]
    G --> E
    E -->|Yes| H[Caseworker records <br/> Notes <br/> Files on Query]
    H --> I[Caseworker closes <br/> Query as Resolved]
    I --> J[Resolution Date <br/> Auto-recorded]
```


