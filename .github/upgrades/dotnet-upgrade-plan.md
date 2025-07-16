# .NET 9.0 Upgrade Plan

## Execution Steps

## Settings

This section contains settings and data used by execution steps.

### Excluded projects

Table below contains projects that do belong to the dependency graph for selected projects and should not be included in the upgrade.

| Project name                                   | Description                 |
|:-----------------------------------------------|:---------------------------:|

### Aggregate NuGet packages modifications across all projects

NuGet packages used across all selected projects or their dependencies that need version update in projects that reference them.

| Package Name                        | Current Version | New Version | Description                         |
|:------------------------------------|:---------------:|:-----------:|:------------------------------------:|

### Project upgrade details
This section contains details about each project upgrade and modifications that need to be done in the project.

#### Voorbeeld.WebApplication modifications

Project properties changes:
  - Target framework should be changed from `net6.0` to `net9.0`

NuGet packages changes:
  - Newtonsoft.Json should be updated from `13.0.1` to `13.0.2` (*recommended for .NET 9.0*)
  - System.IdentityModel.Tokens.Jwt should be updated from `6.15.0` to `6.16.0` (*recommended for .NET 9.0*)

Other changes:
  - None

## Execution Steps

1. Validate that an .NET 9.0 SDK required for this upgrade is installed on the machine and if not, help to get it installed.
2. Ensure that the SDK version specified in global.json files is compatible with the .NET 9.0 upgrade.
3. Upgrade Voorbeeld.WebApplication.csproj
4. Run unit tests to validate upgrade in the projects listed below:

