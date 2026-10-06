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

## Process 6 — Matching Process (ITAHC/DOCOM to Importer Notification)

```mermaid
flowchart TD
    A([Inbound ITAHC/DOCOM <br/> or Importer Notification <br/> created or updated]) --> B[Run weighted matching <br/> algorithm against <br/> unmatched candidates <br/> within configurable window]
    B --> C{Certificate number <br/> exact match?}
    C -->|Yes| D[Score = 100%]
    C -->|No| E{First part of <br/> Destination Postcode <br/> matches?}
    E -->|No| F[No candidate match]
    E -->|Yes| G[Calculate weighted mean <br/> score across configured <br/> fields and weightings]
    D --> H{Score at or above <br/> confidence threshold?}
    G --> H
    H -->|No| F
    H -->|Yes| I[Create Match Record <br/> status Unmatched]
    I --> J[Caseworker reviews <br/> side-by-side comparison]
    J --> K{Valid match?}
    K -->|Yes| L[Complete Match step <br/> status Matched]
    K -->|No| M[Reject Match step <br/> mandatory reason <br/> status Rejected]
    L --> N{Import Record <br/> already linked?}
    N -->|No| O[Offer to create <br/> Import Record from <br/> confirmed match]
    N -->|Yes| P[Associate existing <br/> Import Record]
    O --> Q[Import Record created: <br/> ITAHC/DOCOM data + <br/> copied Importer Notification fields]
    Q --> R[Watchlist check <br/> applied — see Process 7]
    M --> S[Pair excluded from <br/> future candidate searches]
    I -.->|older than 30 days <br/> configurable, still unmatched| T[Importer Notification <br/> archived - Inactive]
```

---

## Process 7 — Watchlist Flagging

```mermaid
flowchart TD
    A([ITAHC created, or <br/> Import Record created <br/> or matched]) --> B[Identify place of origin, <br/> place of destination, <br/> consignee, transporter, <br/> veterinarian involved]
    B --> C{Any party active <br/> on the Watchlist?}
    C -->|No| D[No flag]
    C -->|Yes| E[Flag the ITAHC/Import <br/> Record — one flag per <br/> matching Watchlist entry]
    E --> F[Caseworker opens flag]
    F --> G[Watchlist record details <br/> and comments displayed]
    G --> H[Caseworker may edit <br/> record or add a comment]
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


