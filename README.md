# Imports

## Introduction

This repository contains source code, data artifacts, deployment assets, and CI/CD pipelines for the Imports (PIMS) model-driven app.

## Documentation

The consolidated PIMS requirements baseline is published at <https://defra.github.io/trade-imports-pims/> and authored under [`docs/`](/docs).

To build the documentation site locally:

```sh
pip install -r docs-requirements.txt
mkdocs serve
```

## Prerequisites

The following solutions must exist in the target Dataverse environment in order to deploy the package:

| Dependency                     | Minimum Version | Description                                                      | Guidance                                                                                         |
| ------------------------------ | --------------- | ---------------------------------------------------------------- | ------------------------------------------------------------------------------------------------ |
| MicrosoftLabsAzuereBlobStorage | 1.8.0.5         | A third-party solution installed via AppSource                   | This is no longer available on AppSource. Environments must be cloned to install this dependency |
| DefraImportsDependencies       | 1.0.0.0         | A DEFRA solution installed as part of this package               |                                                                                                  |
| Dynamics 365 Customer Service  | N/A             | A first-party Dynamics 365 module installed via the Admin portal |                                                                                                  |

## Installation

The package can be installed with the following optional settings:

| Setting                                   | Type   | Description                                                                        | Guidance                                                         |
| ----------------------------------------- | ------ | ---------------------------------------------------------------------------------- | ---------------------------------------------------------------- |
| `PackageDeployer.Settings.ImportSeedData` | `bool` | Indicates whether to import seed data (user-owned data) as part of the deployment. | This should generally only be `true` for ephemeral environments. |

Note that these settings correspond to Azure Pipelines variables. To set these settings via an environment variable, replace `.` characters with `_`. For example, `PackageDeployer.Settings.ImportSeedData` becomes `PACKAGEDEPLOYER_SETTINGS_IMPORTSEEDDATA`.

## Contributing

Refer to the [CONTRIBUTING.md](/CONTRIBUTING.md) guide.
