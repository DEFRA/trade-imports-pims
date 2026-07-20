# Contributing

Please ensure that you've read this document before contributing to this repository.

## Table of contents

- [Contributing](#contributing)
  - [Table of contents](#table-of-contents)
  - [Structure](#structure)
  - [Prerequisites](#prerequisites)
  - [Branching and pull requests](#branching-and-pull-requests)
  - [Behaviour-driven development](#behaviour-driven-development)
  - [Development](#development)
    - [Developer tools](#developer-tools)
      - [Visual Studio](#visual-studio)
      - [Visual Studio Code](#visual-studio-code)
    - [Platform guidance](#platform-guidance)
    - [Configuration over code](#configuration-over-code)
    - [Future-proofing](#future-proofing)
    - [Data](#data)
      - [Categories](#categories)
    - [Build tasks](#build-tasks)
      - [Create a development environment](#create-a-development-environment)
      - [Sync solution metadata](#sync-solution-metadata)
    - [Solution versioning](#solution-versioning)
      - [Commit message](#commit-message)
      - [Git tag](#git-tag)
  - [Deployments](#deployments)
    - [Updates and upgrades](#updates-and-upgrades)
  - [Testing](#testing)
    - [Test setup](#test-setup)
      - [Fakers](#fakers)
      - [Scenario builder](#scenario-builder)
        - [Accessing a builder](#accessing-a-builder)
        - [Caching scenarios](#caching-scenarios)
        - [Incremental scenarios](#incremental-scenarios)
    - [Writing acceptance tests](#writing-acceptance-tests)

## Structure

- `data/`: Data packages and extracted records used for import, export, seed, and deletion operations.
- `deploy/`: Packaging and deployment project for releasing solution artifacts.
- `pipelines/`: Azure DevOps YAML pipelines for build, validation, packaging, environment management, and deployment.
- `src/`: Core application source code, including business logic, repositories, plugins, workflows, and solution assets.
- `webresources/`: Client-facing assets such as scripts, images, and static data used in the model-driven app.
- `tests/`: Unit and integration test projects, fixtures, and test data.

## Prerequisites

- Visual Studio and .NET SDKs (version aligned to project configuration)
- Access to required Dataverse environments
- Access to Azure DevOps

## Branching and pull requests

Familiarise yourself with Microsoft's [Git branching guidance](https://docs.microsoft.com/en-us/azure/devops/repos/git/git-branching-guidance?view=azure-devops) and [Release Flow](https://docs.microsoft.com/en-us/azure/devops/learn/devops-at-microsoft/release-flow) branching strategy. The summary is:

> Use feature branches for all new features and bug fixes

and

> Feature branches isolate work in progress from the completed work in the main branch.

Completed means all of the following are included in the pull request for the feature branch (when applicable):

- Configuration & customistaion
- Deployment logic (incl. data)
- Documentation
- Automated tests

This allows for holistic code reviews, cleaner git history, and a more stable package. Completed also means sufficient testing has been carried out _before_ creating a pull request i.e. impacted tests have ran successfully.

The pull request should be linked to the user story or bug for traceability and to track its build and deployment status. 

Tasks are created arbitrarily and the build or deployment status of these isn't relevant relative to the parent user story or bug - therefore, linking work items at this level is not required. If you link individual commits to tasks for greater granularity of tracking, please remove this from the work items automatically linked to the pull request.

## Behaviour-driven development

We employ BDD which is an extension of TDD (test-driven development). Refer to [Reqnroll](https://reqnroll.net/) for more detail.

The Cucumber website provides a good description of [who does what](https://cucumber.io/docs/bdd/who-does-what/). In summary, we expect that the BA, tester, and developer (the 'Three Amigos') have collaboratively produced Gherkin scenarios for a user story _prior to development_.

## Development

### Developer tools

#### Visual Studio

The recommended IDE for .NET development e.g. code activities, plug-ins, package deployer logic, integration tests and acceptance tests.

#### Visual Studio Code

The recommended text editor for other development (e.g. web resources, YAML pipelines, documentation)

### Platform guidance

For guidance relating to the platform, refer to the Microsoft Power Apps [documentation](https://docs.microsoft.com/en-us/powerapps/) - the advice given is generally accurate. If the answer cannot be found here, defer to the team.

### Configuration over code

We are delivering a low-code solution and need to be aware of when it is or isn't appropriate to introduce code. A good rule of thumb is that any code written should be **highly reusable** and **unlikely to change** (e.g. platform extensions rather than business processes).

This means primarily relying on workflows, actions, business process flows, or flows - occasionally supplemented with custom workflow activities. Actions are a useful way of grouping steps together and introducing more 'verbs' beyond create, read, and update etc. They can be called from workflows, flows, or business process flows as well as via the organisation service or web API.

You may rarely require plug-in handlers to alter the behaviour of managed actions or trigger logic on messages not supported by workflows or flows. Plug-in steps don't offer the scoping functionality of workflows and flows (especially important when dealing with out-of-the-box components - see [Future-proofing](#Future-proofing)) or the input & ouput functionality found in actions and custom workflow activities (meaning less flexible and reusable). Placing most of your business logic in plug-ins also introduces a contention issue in shared development environments due to a common dependency on a single solution component.

### Future-proofing

An environment may have many solutions deployed to it over it's lifetime. For this reason, we must ensure that our solution is future-proof and compatible with other solutions that may be introduced. This principle can be applied in countless different ways, but some examples might be:

- Avoid organisation scoped processes or plug-in handlers on out-of-the-box actions for out-of-the-box entities as this may prevent these entities from being reused by other parts of the business
- Avoid customising managed forms or views as other solutions can also introduce changes to these, create new forms for your app instead

### Data

#### Categories

Data is currently broken down into three categories:

| Category | Owner            | Dependencies allowed | Deployment                                                 | Location  | Notes                                                          |
| -------- | ---------------- | -------------------- | ---------------------------------------------------------- | --------- | -------------------------------------------------------------- |
| Core     | Development team | Yes                  | Imported with solutions as part of the package deployment. | data\core | Solutions and tests can be dependent on this data.             |
| Seed     | Users            | No                   | Imported independently on-demand.                          | data\seed | Updates made by users in production should not be overwritten. |

The only category that end users can create or update is seed data. End users should not be able to perform CRUD operations on tables and columns that fall under the solution and external data schemas. Care should be taken when importing seed data to avoid overwriting end user activity in production. This includes undoing updates or re-adding deleted records.

### Build tasks

#### Create a development environment

You can create a development environment by executing the _imports.environment.create.dev_ pipeline.

#### Sync solution metadata

You can sync solution metadata by executing the _imports.solution.sync_ pipeline.

### Solution versioning

#### Commit message

Solutions are versioned automatically based on the Git history. Ensure that you write commit and pull request titles that conform to Conventional Commits. A Git commit where there are changes within a solution metadata folder counts as a version increment for that solution. 

| Commit message                | Increment | Explanation                                                                    |
| ----------------------------- | --------- | ------------------------------------------------------------------------------ |
| feat!: my breaking feature    | Major     | Exclamation is present after the commit type.                                  |
| feat: my non-breaking feature | Minor     | No exclamation is present after the commit type and commit type is 'feat'.     |
| fix: my non-breaking bug fix  | Patch     | No exclamation is present after the commit type and commit type is not 'feat'. |

If you're making changes for a solution (e.g. plug-in or web resource changes) that sit outside the solution metadata folder, you must increment the version for the solution manually by including one of the following lines in the commit body:

`+solutionVer(<solutionName>): major`

`+solutionVer(<solutionName>): minor`

`+solutionVer(<solutionName>): patch`

Where `<solutionName>` is replaced by the unique name of the solution.

#### Git tag

It's possible to overide the version that will be calculated at build time by using Git tags. This can be done by adding a tag with the following format:

`<solution unique name>/<major>.<minor>.<patch>`.

For example:

`defra_Imports/1.1.0`

## Deployments

### Updates and upgrades

The solutions will be either updated or upgraded based on the difference in version between the solution being deployed and the solution existing in an environment. By default, it will be a solution update _unless_ the major version of the incoming solution is greater.

## Testing

There are three test projects - unit, integration~, and acceptance~. Ensure that your feature branch updates the test projects in order to verify your changes:

- Unit when writing custom code (e.g. workflow activities or plugin handlers)
- Integration when making changes that impact the API (e.g. workflows and actions)
~- Acceptance when making changes that impact end users~

Note that it is likely that you will require a combination of all three kinds of automated testing to be employed on a single ticket.

### Test setup

~A common library (*Defra.Imports.TestCommon*) sits between both the integration and acceptance test libraries. This is where we can implement logic that is common across the test layers. An example of this is test setup - the combination of data generation and API request execution that allow the tests to setup a given test scenario.~ 

#### Fakers

We are using the *Bogus* library to allow us to generate lifelike data. For more information on the `Faker` objects that this library allows us to make use of, refer to the [documentation](https://github.com/bchavez/Bogus). 

When we create `Faker` classes, we should ensure that they:
- Have a default set of rules which will create valid records
- Use the `en_GB` locale.

#### Scenario builder

The Bogus library provides us with a builder pattern for generating objects with lifelike data, but this is only a small part of test data setup - it still needs to be sent to the Dataverse API. The resulting records may then need to have further actions taken on them via the API. 

To abstract the API requests needed for the tests to simulate a given series of user actions, we have introduced another set of classes using a builder pattern. These can be found in the `Defra.Imports.Scenarios` namespace. 
The scenario builder is chain of builder classes that implement `AsyncScenarioStepBuilder<TScenario>`. Each of these classes represents a single step in the General Certificate process. For example:

- A user registering
- A user submitting an application
- An inspector assigning a work order
- An inspector recording their time for task
- An inspector completing a task

##### Accessing a builder

A scenario builder instance is available to integration tests via the `ScenarioBuilder` property. Acceptance tests can access an instance by using Reqnroll [context injection](https://docs.Reqnroll.org/projects/Reqnroll/en/latest/Bindings/Context-Injection.html) - for example, adding an `IImportsScenarioBuilder` parameter to a binding class constructor.

##### Caching scenarios

Read-only scenarios (i.e. scenarios where we don't make subsequent requests except for read requests on the data that gets created) can be pass a `cacheKey` to the `BuildAsync` method. If there is a hit on the cache key, then a previously constructed scenario that used the same key will be returned. If there is a cache miss, the scenario will be cached for future tests. This can greatly improve test performance and reduce the amount of test data generated.

##### Incremental scenarios

Integration tests can build scenarios with a single call to `Build`. This is not the case for acceptance tests, as these may have many `Given` steps chained together. For this reason, the scenario builder supports incremental building. 

The scenario returned by calling `Build` can be passed into future calls to `Build` and only the steps that haven't yet run will execute. To assist with this, there is an overload of `Build` that returns the scenario as well as the last built step as a `out` parameter. Both the step and the scenario can then be passed to other `Given` step bindings through the `ScenarioContext`.

### Writing acceptance tests

Acceptance tests are implemented using a BDD approach (see [Behaviour-driven development](#Behaviour-driven-development)). Bear the following in mind:

- Reqnroll scenarios are produced by the 'Three Amigos' before development
- Reqnroll scenarios primarily document system behaviour - tests are a by-product
- Do not try to achieve too much in a single scenario - split out negative testing and happy path
- Shorter scenarios are generally better
- Scenario steps should ideally be steps in a business process rather than individual UI interactions
- Make use of `Given` steps to quickly and reliably set up pre-conditions via the API(s)
- Use existing Reqnroll bindings where possible