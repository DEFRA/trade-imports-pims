# Defra.Imports.IntegrationTests

- [Defra.Imports.IntegrationTests](#defraimportsintegrationtests)
  - [Introduction](#introduction)
  - [Prerequisites](#prerequisites)
  - [Configuration](#configuration)
    - [environment.json](#environmentjson)
    - [User secrets](#user-secrets)

## Introduction

This document contains information on working with the integration tests.

## Prerequisites

- .NET Framework 4.7.2

## Configuration

Configuration for the tests can be stored in several places.

### environment.json

This file contains the template for the configuration. This should be pasted into the user secrets file (see below) and the secrets filled in.

### User secrets

This configuration file is used for test configuration. Copy the template from _environment.json_ and fill in the values.

To run the tests, you must have your user secrets configured with a Key Vault client secret. The remaining secrets (e.g. user credentials) are then retrieved from Key Vault.

```json
{
  "keyVault": {
    "clientSecret": "<secret>"
  }
}
```