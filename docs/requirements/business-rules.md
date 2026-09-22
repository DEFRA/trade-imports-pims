# Business Rules

All business rules are numbered sequentially. Each rule references the originating user story (US-xxx) and source Jira issues.

---

## Risk Assessment Rules

### BR-001 — Commodity Risk Level Determines Base Risk Level { #br-001 }
When an Import Record is created or updated, PIMS must evaluate the configured Commodity Risk Level rules (Country × Commodity Type → Risk Level) to determine the base risk classification (P1, P2, P3) for the Import Record.

- **Source:** [US-011](user-stories/US-011-Manage-Commodity-Risk-Levels.md)
- **Jira:** IMTA-5865, IMTA-5914, IMTA-5915, IMTA-5916

---

### BR-002 — P1 with Gold/Bronze Commodity and Bronze Place of Origin → Post Import Check Required { #br-002 }
When an Import Record is created or updated **AND** the Commodity Type + Country of Origin matches a Gold/Bronze Commodity rule **AND** the linked Place of Origin has Trust Level = **Bronze**, PIMS must set:

- Post Import Checks Required? = **Yes**
- Post Import Checks Required Reason = **Bronze Place of Origin**

- **Source:** [US-014](user-stories/US-014-Automated-Risk-Assessment-P1.md)
- **Jira:** IMTA-5866 AC-1

---

### BR-003 — P1 with Gold/Bronze Commodity and Gold Place of Origin → Inspection at 1-in-10 Frequency { #br-003 }
When an Import Record is created or updated **AND** the Commodity Type + Country of Origin matches a Gold/Bronze Commodity rule **AND** the linked Place of Origin has Trust Level = **Gold** **AND** the Number of Import Records Since Last Post Import Check ≥ 10, PIMS must set:

- Post Import Checks Required? = **Yes**
- Post Import Checks Required Reason = **Gold Place of Origin — Inspection Coverage**
- Number of Import Records Since Last Post Import Check on the Place of Origin = **0**

Otherwise:

- Post Import Checks Required? = **No**
- Post Import Checks Required Reason = **No Inspection Required — Gold Place of Origin**

- **Source:** [US-014](user-stories/US-014-Automated-Risk-Assessment-P1.md)
- **Jira:** IMTA-5866 AC-2, AC-3

---

### BR-004 — P1 with Gold/Bronze Commodity but No Verified Place of Origin → Undetermined { #br-004 }
When an Import Record is created or updated **AND** the Commodity Type + Country of Origin matches a Gold/Bronze Commodity rule **AND** no Place of Origin is linked to the Import Record, PIMS must set:

- Post Import Checks Required? = **Undetermined**
- Post Import Checks Required Reason = **Verified Place of Origin Missing**

- **Source:** [US-014](user-stories/US-014-Automated-Risk-Assessment-P1.md)
- **Jira:** IMTA-5866 AC-4

---

### BR-005 — P1 with Gold/Bronze Commodity — Manual Override Reallocates 10th Inspection { #br-005 }
When an Import Record has been flagged for a Post Import Check under BR-003 **AND** the user subsequently sets Post Import Checks Required? = No, PIMS must reallocate the missed inspection to the next Import Record associated with the same Place of Origin by adding 1 to the Number of Import Records Since Last Post Import Check counter.

- **Source:** [US-014](user-stories/US-014-Automated-Risk-Assessment-P1.md)
- **Jira:** IMTA-5866 AC-5

---

### BR-006 — P1 Inspection — Gold/Bronze Rating Not Determinable → Discretionary { #br-006 }
When the Risk Level is set to P1 **AND** the Gold/Bronze rating cannot be determined (no Gold/Bronze Commodity rule applies), the Post Import Check requirement is set to **Discretionary** with reason "Decision to inspect is discretionary".

- **Source:** [US-014](user-stories/US-014-Automated-Risk-Assessment-P1.md)
- **Jira:** IMTA-5894

---

### BR-007 — P2 Random 10% Inspection Coverage { #br-007 }
When an Import Record Risk Level is set or changed to P2:

1. If the Import Record Priority 2 Quota Counter > 0: decrement the counter by 1 and flag the record for Post Import Check.
2. Otherwise: increment the Import Record Priority 2 Counter. If the counter reaches the configured limit (10): reset the counter to 0 and flag the record for Post Import Check.

When an Import Record Risk Level is changed away from P2:

- If the record was flagged for inspection: increment the Quota Counter by 1 (to schedule a replacement).
- If the record was not flagged for inspection: decrement the Priority 2 Counter by 1 (to maintain the ratio).

- **Source:** [US-015](user-stories/US-015-Automated-Risk-Assessment-P2.md)
- **Jira:** IMTA-5895

---

### BR-008 — P3 Random 2% Inspection Coverage (ITAHC Records Only) { #br-008 }
When an Import Record of type ITAHC with a linked Primary ITAHC is created:

1. Increment the "2% All Import Records — Random" Inspection Coverage Count value.
2. If the count value equals or exceeds the configured limit: flag the Import Record for Post Import Check with reason "Random P3 Inspection" and reset the counter to 0.
3. If the count is below the limit and the Risk Level is P3: set Post Import Checks Required? = No with reason "No inspection required".

The counter is only incremented on create, not on update.

This rule only applies to Import Records with Risk Level = P3.

- **Source:** [US-016](user-stories/US-016-Automated-Risk-Assessment-P3-Random.md)
- **Jira:** IMTA-5892

---

### BR-009 — Random Inspection Counter Incremented Before Risk Assessment { #br-009 }
The Import Record Counter must be incremented before automated risk assessment rules are evaluated, to ensure accurate sequencing.

- **Source:** [US-016](user-stories/US-016-Automated-Risk-Assessment-P3-Random.md)
- **Jira:** IMTA-5867 AC-1

---

## Place of Origin Trust Level Rules

### BR-010 — New Place of Origin Defaults to Bronze Trust Level { #br-010 }
When a Place of Origin is created, its Trust Level must default to **Bronze**.

- **Source:** [US-018](user-stories/US-018-Place-of-Origin-Trust-Level-Maintenance.md)
- **Jira:** IMTA-5886 AC-1

---

### BR-011 — Gold Trust Level Awarded After 3 Consecutive Satisfactory Outcomes { #br-011 }
PIMS must promote the Trust Level of a Place of Origin from Bronze to Gold after **3 consecutive** Import Records are completed with a Post Import Check Outcome of **Satisfactory** or **Not Visited**, provided the Place of Origin is not locked to Bronze.

- **Source:** [US-018](user-stories/US-018-Place-of-Origin-Trust-Level-Maintenance.md)
- **Jira:** IMTA-5886 AC-3

---

### BR-012 — Consecutive Satisfactory Count Resets on Unsatisfactory Outcome { #br-012 }
When an Import Record is completed with a Post Import Check Outcome of **Unsatisfactory**, PIMS must set the Number of Consecutive Satisfactory Import Records for the associated Place of Origin to **0**.

- **Source:** [US-018](user-stories/US-018-Place-of-Origin-Trust-Level-Maintenance.md)
- **Jira:** IMTA-5886 AC-2

---

### BR-013 — Trust Level Revoked to Bronze on Unsatisfactory Outcome + User Confirmation { #br-013 }
When an Import Record is completed with Post Import Check Outcome = Unsatisfactory **AND** the user selects Reset Gold Trust Level to Bronze? = Yes, PIMS must set the Trust Level of the associated Place of Origin to **Bronze**.

- **Source:** [US-021](user-stories/US-021-Revoke-Gold-Trust-Level.md)
- **Jira:** IMTA-5886 AC-4, IMTA-6669

---

### BR-014 — Lock to Bronze Prevents Automatic Gold Promotion { #br-014 }
When a Place of Origin has Lock to Bronze = Yes, BR-011 must not be applied. The Trust Level remains Bronze regardless of consecutive satisfactory outcomes.

- **Source:** [US-019](user-stories/US-019-Lock-Place-of-Origin-to-Bronze.md)
- **Jira:** IMTA-5890 AC-2

---

### BR-015 — Unlock from Bronze Restores Previous Trust Level { #br-015 }
When the Lock to Bronze field is changed from Yes to No, PIMS must restore the Trust Level to the value stored in the Previous Trust Level field.

- **Source:** [US-019](user-stories/US-019-Lock-Place-of-Origin-to-Bronze.md), [US-020](user-stories/US-020-Update-Place-of-Origin-on-Import-Record.md)
- **Jira:** IMTA-6668

---

### BR-016 — Lock to Bronze Requires a Mandatory Reason { #br-016 }
When the Lock to Bronze field is set to Yes, the user must provide a non-empty value in the Locked to Bronze Reason field before the record can be saved.

- **Source:** [US-019](user-stories/US-019-Lock-Place-of-Origin-to-Bronze.md)
- **Jira:** IMTA-5890 AC-5

---

### BR-017 — Unlock from Bronze Requires a Mandatory Reason { #br-017 }
When the Lock to Bronze field is changed from Yes to No, the user must provide a non-empty value in the Unlocked from Lock to Bronze Reason field before the record can be saved.

- **Source:** [US-019](user-stories/US-019-Lock-Place-of-Origin-to-Bronze.md)
- **Jira:** IMTA-5890 AC-7

---

### BR-018 — Number of Import Records Counter on Place of Origin Incremented on Link { #br-018 }
The Number of Import Records counter on a Place of Origin must be incremented by 1 when an Import Record is linked to the Place of Origin **and** the Import Record has a Primary ITAHC. It must be decremented by 1 when the link is removed or the Primary ITAHC is cleared.

- **Source:** [US-018](user-stories/US-018-Place-of-Origin-Trust-Level-Maintenance.md)
- **Jira:** IMTA-5886 AC-5

---

### BR-019 — Number of Import Records Since Last Check Counter Incremented for Gold Places of Origin { #br-019 }
For Import Records with a Gold/Bronze Commodity **and** a Place of Origin with Trust Level = Gold: PIMS must increment the Number of Import Records Since Last Check counter on the Place of Origin by 1 each time the Import Record is completed.

- **Source:** [US-018](user-stories/US-018-Place-of-Origin-Trust-Level-Maintenance.md)
- **Jira:** IMTA-5886 AC-6

---

### BR-020 — Changing Place of Origin Updates Previous Place of Origin Counters { #br-020 }
When the Verified Place of Origin on an Import Record is changed:

1. Decrement the Number of Import Records counter on the **previous** Place of Origin by 1.
2. If the previous Place of Origin was Gold **and** the Import Record was flagged for a Gold inspection: add 1 to the previous Place of Origin's Inspection Quota.
3. Re-evaluate the Post Import Check requirement based on the new Place of Origin.

- **Source:** [US-020](user-stories/US-020-Update-Place-of-Origin-on-Import-Record.md)
- **Jira:** IMTA-6666

---

### BR-021 — Manual Early Post Import Check Resets Gold Counter { #br-021 }
When a user manually sets Post Import Check Required? = Yes on an Import Record where the previous value was No **and** the Place of Origin Trust Level = Gold **and** the Number of Import Records Since Last Post Import Check is between 1 and 9 (inclusive), PIMS must reset the counter to 0.

- **Source:** [US-022](user-stories/US-022-Defer-Post-Import-Check-Counter.md)
- **Jira:** IMTA-6680

---

## Certificate and Record Rules

### BR-022 — Unique Reference Number Format { #br-022 }
The unique reference number generated for each Import Record must follow the format:  
`GB.{YEAR}.{9-digit-sequential-number}`  
Example: `GB.2019.000000009`

The prefix (default `GB.`) must be configurable by an EU Imports Business Rules Admin.

- **Source:** [US-028](user-stories/US-028-Generate-Unique-Reference-Number.md)
- **Jira:** IMTA-6132

---

### BR-023 — Import Query Number Format { #br-023 }
The unique reference number generated for each Import Query must follow the format:  
`RMQ{YY}-{4-digit-sequential-number}`  
Example: `RMQ19-0024`

The sequence number must be global across all Import Queries (not per-Import-Record).

- **Source:** [US-025](user-stories/US-025-Import-Query-Management.md)
- **Jira:** IMTA-6185, IMTA-6255

---

### BR-024 — IV65 Response Due Date = IV65 Sent Date + 14 Calendar Days { #br-024 }
When a user changes the IV65 Sent Date field, PIMS must automatically calculate IV65 Response Due Date = IV65 Sent Date + 14 calendar days. The calculated date remains editable by caseworkers.

- **Source:** [US-030](user-stories/US-030-IV65-Due-Date-Calculation.md)
- **Jira:** IMTA-6166

---

### BR-025 — Warble Fly Treatment Declaration Received Date Only Enabled When Required { #br-025 }
The Warble Fly Treatment Declaration Received Date field must be enabled only when Warble Fly Treatment Declaration Required? = Yes. If the user changes the value back to No, the Received Date must be cleared and the field disabled.

- **Source:** [US-032](user-stories/US-032-Warble-Fly-Declaration-Date.md)
- **Jira:** IMTA-6158

---

### BR-026 — Moved to Completion Date Journalled Automatically { #br-026 }
When a user sets the Moved to Completion? field to Yes, PIMS must record the current date and time in the Moved to Completion Date field (read-only). When the field is set back to No, the date must be cleared.

- **Source:** [US-031](user-stories/US-031-Completion-Date-Recording.md)
- **Jira:** IMTA-6180

---

### BR-027 — ITAHC/DOCOM Status and Replacement Chain Must Be Tracked { #br-027 }
The Replaced By and Replaces fields on ITAHC and DOCOM records must reflect the replacement chain from TRACES. Current implementation evidence confirms the replacement links and cross-references are maintained. Explicit prevention of primary-certificate selection in every user interaction context is not yet fully evidenced and requires confirmation.

- **Source:** [US-002](user-stories/US-002-Manage-ITAHC.md)
- **Jira:** IMTA-5984

---

### BR-028 — "No ITAHC Received" Option on Import Record { #br-028 }
A user must be able to select "No ITAHC Received" in the Primary ITAHC lookup on an Import Record to allow the record to be saved without a linked ITAHC. Source stories may refer to this option as "No ITAHC Provided".

- **Source:** [US-001](user-stories/US-001-Manage-Import-Record.md)
- **Jira:** IMTA-5985

---

### BR-029 — Deleted Importer Notification Attachments Remain Active { #br-029 }
When an Importer Notification update from IPAFFS removes a related record (e.g. an Additional Permanent Address), the equivalent record in PIMS must remain active and not be deleted, as Import Records may already reference that data.

This rule applies to the implemented **Importer Notification** entity used for IPAFFS data. Legacy wording may still refer to some of these records or views as "Import Notification".

- **Source:** [US-006](user-stories/US-006-Receive-Importer-Notification-From-IPAFFS.md)
- **Jira:** IMTA-5864 AC-2

---

### BR-030 — Inspection Coverage Rules Configurable by Business Rules Admin Only { #br-030 }
Inspection Coverage Rules must only be creatable or updatable by users with the Dynamics 365 System Administrator role. All other business user security roles, including EU Imports Business Rules Admin, must be restricted to read access only.

- **Source:** [US-013](user-stories/US-013-Manage-Inspection-Coverage-Rules.md)
- **Jira:** IMTA-5891 AC-2

---

### BR-031 — Documents Attached to Import Records Cannot Be Deleted { #br-031 }
An EU Imports Caseworker must not be able to delete documents once attached to an Import Record.

- **Source:** [US-029](user-stories/US-029-Document-Attachment.md)
- **Jira:** IMTA-5913 AC-3

---

### BR-032 — Import Query Notes Can Be Added by Non-Owners { #br-032 }
An EU Imports Caseworker must be able to attach notes and files to an Import Query even if they are not the owner of that query.

- **Source:** [US-025](user-stories/US-025-Import-Query-Management.md)
- **Jira:** IMTA-6185 AC-7

---

### BR-033 — Inbound IPAFFS Messages That Fail Processing Go to Dead Letter Queue { #br-033 }
If an Importer Notification received from IPAFFS cannot be created or updated in PIMS, the failed message must be placed on the Dead Letter Queue.

- **Source:** [US-006](user-stories/US-006-Receive-Importer-Notification-From-IPAFFS.md)
- **Jira:** IMTA-5864 AC-3

---

### BR-034 — IPAFFS Commodity Identifiers Must Be Translated to D365 Commodity Classification { #br-034 }
When IPAFFS Importer Notification data is processed for downstream Import Record creation or update, PIMS must translate the IPAFFS commodity identifier to the D365 commodity classification using the configured Commodity Type Mapping.

- **Source:** [US-006](user-stories/US-006-Receive-Importer-Notification-From-IPAFFS.md)
- **Jira:** IMTA-7222

---

### BR-035 — Failed TRACES Receipts Must Be Captured, Visible and Reprocessable { #br-035 }
If an inbound ITAHC or DOCOM receipt from TRACES Classic cannot be processed into PIMS, the failure must be captured with enough detail for investigation, exposed to authorised operational users for review, support controlled retry or reprocess actions, and maintain an auditable history of the failure and its resolution.

- **Source:** [US-047](user-stories/US-047-Manage-Failed-TRACES-Receipts.md)
- **Jira:** IMTA-6626

---

## Reference Data, Notification and Triage Rules

The following rules were added after PLNT-4535 to PLNT-4542 were identified as Jira issues missing from the original corpus compilation. They have not yet been re-verified against implementation evidence; see the affected user stories' Conformance sections.

### BR-036 — Certificate and Notification Reference Fields Are Optional and Manually Entered

The GB Import Health Certificate, Traces Export Health Certificate, ITAHC Reference and DOCOM Reference fields on an Import Record must remain optional single-line text fields. PIMS must not auto-populate any of these fields; they are populated manually by the caseworker only.

- **Source:** [US-001](user-stories/US-001-Manage-Import-Record.md)
- **Jira:** PLNT-4535 AC-1

---

### BR-037 — Triage Step Must Not Clear or Update Commodity Code When Primary ITAHC Is Populated

When a user enters a value in the Primary ITAHC field during the Triage stage of the Import Record business process flow, PIMS must not remove or update the Commodity Code value on the Import Record on save. The Primary ITAHC field carries no business process logic. This rule corrects the defect reported as DEFRA incident INC0838632 ("IPAFFS - PIMS For EU Exports - Commodity Code"), under which populating Primary ITAHC previously cleared the Commodity Code value.

- **Source:** [US-001](user-stories/US-001-Manage-Import-Record.md)
- **Jira:** PLNT-4535 AC-2

---

### BR-038 — Import Record Type Value List

The Import Record Type field **on the Import Record entity** (`defraimp_importapplication`) must offer exactly the following values, with no default value pre-selected: Importer Notification, Health Certificate, ITAHC - Landbridge, CHEDA, CHEDP, DOCOM, ITAHC. The legacy values CED, CVEDA and CVEDP must be removed from this value list. The option label previously shown as "IMP" must display as "Importer Notification" wherever that shared option label is used, including on the Importer Notification Details form. This label rename is the only confirmed cross-entity effect of this rule — it does not remove CED, CVEDA or CVEDP from the Importer Notification entity's own notification type classification (see [US-006](user-stories/US-006-Receive-Importer-Notification-From-IPAFFS.md)).

- **Source:** [US-001](user-stories/US-001-Manage-Import-Record.md), [US-044](user-stories/US-044-View-Importer-Notification.md)
- **Jira:** PLNT-4536 AC-1, AC-3

---

### BR-039 — Import Record Type Set to Importer Notification When Created from an Importer Notification

When a user selects "Create Import Record" on an Importer Notification, PIMS must set the Import Record Type of the newly created Import Record to Importer Notification.

- **Source:** [US-001](user-stories/US-001-Manage-Import-Record.md)
- **Jira:** PLNT-4536 AC-2

---

### BR-040 — Date Importer Notification Received Auto-Populated from Submission Date

When an Import Record is created from an Importer Notification via the "Create Import Record" button, PIMS must auto-populate the Import Record's Date Importer Notification Received field with the value held in the Submission Date field on the associated Importer Notification.

- **Source:** [US-001](user-stories/US-001-Manage-Import-Record.md), [US-050](user-stories/US-050-Importer-Notification-Received-Date.md)
- **Jira:** PLNT-4540 AC-1

---

### BR-041 — Health Certificate Attached Auto-Flag on Importer Notification

When a document with Document Type = Health Certificate and a populated URL is attached to an Importer Notification, PIMS must set the Health Certificate Attached field to Y; otherwise the field must be N. The field remains editable by an EU Imports Caseworker after being auto-populated.

- **Source:** [US-044](user-stories/US-044-View-Importer-Notification.md)
- **Jira:** PLNT-4542 AC-1

---

### BR-042 — Health Certificate Attached After Completion Triggers Amendment and Reassignment

When a Health Certificate is attached to an Importer Notification that has already been completed, PIMS must set the Health Certificate Attached field to Y, change the Owner of the Importer Notification to the EU Imports Dynamics Application User, and change the Status to Amend.

- **Source:** [US-006](user-stories/US-006-Receive-Importer-Notification-From-IPAFFS.md), [US-044](user-stories/US-044-View-Importer-Notification.md)
- **Jira:** PLNT-4542 AC-2

---

### BR-043 — Non-Compliance Fields Default State

When the Non-Compliance tab fields are first added to an Importer Notification or Import Record, PIMS must default Contacted Due to Non-Compliance to N and Non-Compliance Status to In Progress once a Type of Non-Compliance is selected, leaving PIMS Status and Date Completed unset until a caseworker updates them.

- **Source:** [US-049](user-stories/US-049-Non-Compliance-Management.md)
- **Jira:** PLNT-4537 AC-1, AC-2

---

### BR-044 — Non-Compliance Details Mirrored onto Import Record via Quick View 

When an Importer Notification with a populated Type of Non-Compliance is linked to an Import Record, PIMS must display the Importer Notification's Non-Compliance tab fields as a read-only quick view on the Import Record's Non-Compliance tab, positioned below the Import Record's own Non-Compliance fields.

- **Source:** [US-049](user-stories/US-049-Non-Compliance-Management.md)
- **Jira:** PLNT-4537 AC-3
