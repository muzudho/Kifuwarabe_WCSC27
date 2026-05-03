# Projects and dependencies analysis

This document provides a comprehensive overview of the projects and their dependencies in the context of upgrading to .NETCoreApp,Version=v10.0.

## Table of Contents

- [Executive Summary](#executive-Summary)
  - [Highlevel Metrics](#highlevel-metrics)
  - [Projects Compatibility](#projects-compatibility)
  - [Package Compatibility](#package-compatibility)
  - [API Compatibility](#api-compatibility)
- [Aggregate NuGet packages details](#aggregate-nuget-packages-details)
- [Top API Migration Challenges](#top-api-migration-challenges)
  - [Technologies and Features](#technologies-and-features)
  - [Most Frequent API Issues](#most-frequent-api-issues)
- [Projects Relationship Graph](#projects-relationship-graph)
- [Project Details](#project-details)

  - [Sources\Engine\Engine.csproj](#sourcesengineenginecsproj)
  - [Sources\Entities\Entities.csproj](#sourcesentitiesentitiescsproj)
  - [Sources\UseCases\UseCases.csproj](#sourcesusecasesusecasescsproj)
  - [Test\Test.csproj](#testtestcsproj)


## Executive Summary

### Highlevel Metrics

| Metric | Count | Status |
| :--- | :---: | :--- |
| Total Projects | 4 | 3 require upgrade |
| Total NuGet Packages | 6 | All compatible |
| Total Code Files | 91 |  |
| Total Code Files with Incidents | 4 |  |
| Total Lines of Code | 29919 |  |
| Total Number of Issues | 8 |  |
| Estimated LOC to modify | 2+ | at least 0.0% of codebase |

### Projects Compatibility

| Project | Target Framework | Difficulty | Package Issues | API Issues | Est. LOC Impact | Description |
| :--- | :---: | :---: | :---: | :---: | :---: | :--- |
| [Sources\Engine\Engine.csproj](#sourcesengineenginecsproj) | net48 | 🟢 Low | 0 | 2 | 2+ | ClassicDotNetApp, Sdk Style = False |
| [Sources\Entities\Entities.csproj](#sourcesentitiesentitiescsproj) | net48 | 🟢 Low | 0 | 0 |  | ClassicClassLibrary, Sdk Style = False |
| [Sources\UseCases\UseCases.csproj](#sourcesusecasesusecasescsproj) | net48 | 🟢 Low | 0 | 0 |  | ClassicClassLibrary, Sdk Style = False |
| [Test\Test.csproj](#testtestcsproj) | net10.0 | ✅ None | 0 | 0 |  | DotNetCoreApp, Sdk Style = True |

### Package Compatibility

| Status | Count | Percentage |
| :--- | :---: | :---: |
| ✅ Compatible | 6 | 100.0% |
| ⚠️ Incompatible | 0 | 0.0% |
| 🔄 Upgrade Recommended | 0 | 0.0% |
| ***Total NuGet Packages*** | ***6*** | ***100%*** |

### API Compatibility

| Category | Count | Impact |
| :--- | :---: | :--- |
| 🔴 Binary Incompatible | 0 | High - Require code changes |
| 🟡 Source Incompatible | 2 | Medium - Needs re-compilation and potential conflicting API error fixing |
| 🔵 Behavioral change | 0 | Low - Behavioral changes that may require testing at runtime |
| ✅ Compatible | 30494 |  |
| ***Total APIs Analyzed*** | ***30496*** |  |

## Aggregate NuGet packages details

| Package | Current Version | Suggested Version | Projects | Description |
| :--- | :---: | :---: | :--- | :--- |
| coverlet.collector | 6.0.4 |  | [Test.csproj](#testtestcsproj) | ✅Compatible |
| Microsoft.CodeAnalysis.NetAnalyzers | 5.0.1 |  | [Engine.csproj](#sourcesengineenginecsproj) | ✅Compatible |
| Microsoft.NET.Test.Sdk | 17.14.1 |  | [Test.csproj](#testtestcsproj) | ✅Compatible |
| Nett | 0.15.0 |  | [Engine.csproj](#sourcesengineenginecsproj)<br/>[Entities.csproj](#sourcesentitiesentitiescsproj)<br/>[UseCases.csproj](#sourcesusecasesusecasescsproj) | ✅Compatible |
| xunit | 2.9.3 |  | [Test.csproj](#testtestcsproj) | ✅Compatible |
| xunit.runner.visualstudio | 3.1.4 |  | [Test.csproj](#testtestcsproj) | ✅Compatible |

## Top API Migration Challenges

### Technologies and Features

| Technology | Issues | Percentage | Migration Path |
| :--- | :---: | :---: | :--- |
| Legacy Configuration System | 2 | 100.0% | Legacy XML-based configuration system (app.config/web.config) that has been replaced by a more flexible configuration model in .NET Core. The old system was rigid and XML-based. Migrate to Microsoft.Extensions.Configuration with JSON/environment variables; use System.Configuration.ConfigurationManager NuGet package as interim bridge if needed. |

### Most Frequent API Issues

| API | Count | Percentage | Category |
| :--- | :---: | :---: | :--- |
| T:System.Configuration.ConfigurationManager | 1 | 50.0% | Source Incompatible |
| P:System.Configuration.ConfigurationManager.AppSettings | 1 | 50.0% | Source Incompatible |

## Projects Relationship Graph

Legend:
📦 SDK-style project
⚙️ Classic project

```mermaid
flowchart LR
    P1["<b>⚙️&nbsp;Engine.csproj</b><br/><small>net48</small>"]
    P2["<b>⚙️&nbsp;Entities.csproj</b><br/><small>net48</small>"]
    P3["<b>⚙️&nbsp;UseCases.csproj</b><br/><small>net48</small>"]
    P4["<b>📦&nbsp;Test.csproj</b><br/><small>net10.0</small>"]
    P1 --> P2
    P1 --> P3
    P3 --> P2
    P4 --> P2
    P4 --> P3
    click P1 "#sourcesengineenginecsproj"
    click P2 "#sourcesentitiesentitiescsproj"
    click P3 "#sourcesusecasesusecasescsproj"
    click P4 "#testtestcsproj"

```

## Project Details

<a id="sourcesengineenginecsproj"></a>
### Sources\Engine\Engine.csproj

#### Project Info

- **Current Target Framework:** net48
- **Proposed Target Framework:** net10.0
- **SDK-style**: False
- **Project Kind:** ClassicDotNetApp
- **Dependencies**: 2
- **Dependants**: 0
- **Number of Files**: 3
- **Number of Files with Incidents**: 2
- **Lines of Code**: 864
- **Estimated LOC to modify**: 2+ (at least 0.2% of the project)

#### Dependency Graph

Legend:
📦 SDK-style project
⚙️ Classic project

```mermaid
flowchart TB
    subgraph current["Engine.csproj"]
        MAIN["<b>⚙️&nbsp;Engine.csproj</b><br/><small>net48</small>"]
        click MAIN "#sourcesengineenginecsproj"
    end
    subgraph downstream["Dependencies (2"]
        P2["<b>⚙️&nbsp;Entities.csproj</b><br/><small>net48</small>"]
        P3["<b>⚙️&nbsp;UseCases.csproj</b><br/><small>net48</small>"]
        click P2 "#sourcesentitiesentitiescsproj"
        click P3 "#sourcesusecasesusecasescsproj"
    end
    MAIN --> P2
    MAIN --> P3

```

### API Compatibility

| Category | Count | Impact |
| :--- | :---: | :--- |
| 🔴 Binary Incompatible | 0 | High - Require code changes |
| 🟡 Source Incompatible | 2 | Medium - Needs re-compilation and potential conflicting API error fixing |
| 🔵 Behavioral change | 0 | Low - Behavioral changes that may require testing at runtime |
| ✅ Compatible | 1112 |  |
| ***Total APIs Analyzed*** | ***1114*** |  |

#### Project Technologies and Features

| Technology | Issues | Percentage | Migration Path |
| :--- | :---: | :---: | :--- |
| Legacy Configuration System | 2 | 100.0% | Legacy XML-based configuration system (app.config/web.config) that has been replaced by a more flexible configuration model in .NET Core. The old system was rigid and XML-based. Migrate to Microsoft.Extensions.Configuration with JSON/environment variables; use System.Configuration.ConfigurationManager NuGet package as interim bridge if needed. |

<a id="sourcesentitiesentitiescsproj"></a>
### Sources\Entities\Entities.csproj

#### Project Info

- **Current Target Framework:** net48
- **Proposed Target Framework:** net10.0
- **SDK-style**: False
- **Project Kind:** ClassicClassLibrary
- **Dependencies**: 0
- **Dependants**: 3
- **Number of Files**: 84
- **Number of Files with Incidents**: 1
- **Lines of Code**: 26179
- **Estimated LOC to modify**: 0+ (at least 0.0% of the project)

#### Dependency Graph

Legend:
📦 SDK-style project
⚙️ Classic project

```mermaid
flowchart TB
    subgraph upstream["Dependants (3)"]
        P1["<b>⚙️&nbsp;Engine.csproj</b><br/><small>net48</small>"]
        P3["<b>⚙️&nbsp;UseCases.csproj</b><br/><small>net48</small>"]
        P4["<b>📦&nbsp;Test.csproj</b><br/><small>net10.0</small>"]
        click P1 "#sourcesengineenginecsproj"
        click P3 "#sourcesusecasesusecasescsproj"
        click P4 "#testtestcsproj"
    end
    subgraph current["Entities.csproj"]
        MAIN["<b>⚙️&nbsp;Entities.csproj</b><br/><small>net48</small>"]
        click MAIN "#sourcesentitiesentitiescsproj"
    end
    P1 --> MAIN
    P3 --> MAIN
    P4 --> MAIN

```

### API Compatibility

| Category | Count | Impact |
| :--- | :---: | :--- |
| 🔴 Binary Incompatible | 0 | High - Require code changes |
| 🟡 Source Incompatible | 0 | Medium - Needs re-compilation and potential conflicting API error fixing |
| 🔵 Behavioral change | 0 | Low - Behavioral changes that may require testing at runtime |
| ✅ Compatible | 25674 |  |
| ***Total APIs Analyzed*** | ***25674*** |  |

<a id="sourcesusecasesusecasescsproj"></a>
### Sources\UseCases\UseCases.csproj

#### Project Info

- **Current Target Framework:** net48
- **Proposed Target Framework:** net10.0
- **SDK-style**: False
- **Project Kind:** ClassicClassLibrary
- **Dependencies**: 1
- **Dependants**: 2
- **Number of Files**: 3
- **Number of Files with Incidents**: 1
- **Lines of Code**: 2770
- **Estimated LOC to modify**: 0+ (at least 0.0% of the project)

#### Dependency Graph

Legend:
📦 SDK-style project
⚙️ Classic project

```mermaid
flowchart TB
    subgraph upstream["Dependants (2)"]
        P1["<b>⚙️&nbsp;Engine.csproj</b><br/><small>net48</small>"]
        P4["<b>📦&nbsp;Test.csproj</b><br/><small>net10.0</small>"]
        click P1 "#sourcesengineenginecsproj"
        click P4 "#testtestcsproj"
    end
    subgraph current["UseCases.csproj"]
        MAIN["<b>⚙️&nbsp;UseCases.csproj</b><br/><small>net48</small>"]
        click MAIN "#sourcesusecasesusecasescsproj"
    end
    subgraph downstream["Dependencies (1"]
        P2["<b>⚙️&nbsp;Entities.csproj</b><br/><small>net48</small>"]
        click P2 "#sourcesentitiesentitiescsproj"
    end
    P1 --> MAIN
    P4 --> MAIN
    MAIN --> P2

```

### API Compatibility

| Category | Count | Impact |
| :--- | :---: | :--- |
| 🔴 Binary Incompatible | 0 | High - Require code changes |
| 🟡 Source Incompatible | 0 | Medium - Needs re-compilation and potential conflicting API error fixing |
| 🔵 Behavioral change | 0 | Low - Behavioral changes that may require testing at runtime |
| ✅ Compatible | 3708 |  |
| ***Total APIs Analyzed*** | ***3708*** |  |

<a id="testtestcsproj"></a>
### Test\Test.csproj

#### Project Info

- **Current Target Framework:** net10.0✅
- **SDK-style**: True
- **Project Kind:** DotNetCoreApp
- **Dependencies**: 2
- **Dependants**: 0
- **Number of Files**: 3
- **Lines of Code**: 106
- **Estimated LOC to modify**: 0+ (at least 0.0% of the project)

#### Dependency Graph

Legend:
📦 SDK-style project
⚙️ Classic project

```mermaid
flowchart TB
    subgraph current["Test.csproj"]
        MAIN["<b>📦&nbsp;Test.csproj</b><br/><small>net10.0</small>"]
        click MAIN "#testtestcsproj"
    end
    subgraph downstream["Dependencies (2"]
        P2["<b>⚙️&nbsp;Entities.csproj</b><br/><small>net48</small>"]
        P3["<b>⚙️&nbsp;UseCases.csproj</b><br/><small>net48</small>"]
        click P2 "#sourcesentitiesentitiescsproj"
        click P3 "#sourcesusecasesusecasescsproj"
    end
    MAIN --> P2
    MAIN --> P3

```

### API Compatibility

| Category | Count | Impact |
| :--- | :---: | :--- |
| 🔴 Binary Incompatible | 0 | High - Require code changes |
| 🟡 Source Incompatible | 0 | Medium - Needs re-compilation and potential conflicting API error fixing |
| 🔵 Behavioral change | 0 | Low - Behavioral changes that may require testing at runtime |
| ✅ Compatible | 0 |  |
| ***Total APIs Analyzed*** | ***0*** |  |

