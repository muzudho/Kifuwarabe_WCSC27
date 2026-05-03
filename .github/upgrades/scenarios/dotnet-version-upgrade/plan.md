# .NET Framework 4.8 to .NET 10.0 Upgrade Plan

## Overview

**Target**: Upgrade Kifuwarakei solution from .NET Framework 4.8 to .NET 10.0 (LTS)
**Scope**: 3 projects (~4 source files), 3-tier dependency graph

### Selected Strategy
**Bottom-Up (Dependency-First)** — Upgrade from leaf nodes to root application, tier by tier.
**Rationale**: Multiple .NET Framework projects with clear 3-tier dependency structure. Bottom-Up ensures each tier is validated independently before consuming tiers upgrade.

### Dependency Graph
```
Tier 3: [Engine]
		 ↓
Tier 2: [UseCases]
		 ↓
Tier 1: [Entities]
```

### Per-Tier Summary

**Tier 1 (Entities)**:
- Projects: Entities.csproj
- Dependencies: None (leaf node)
- Completion criteria: Builds on net48;net10.0, all tests pass, Tier 2 still builds on net48

**Tier 2 (UseCases)**:
- Projects: UseCases.csproj
- Dependencies: Entities (Tier 1)
- Completion criteria: Builds on net48;net10.0, all tests pass, Tier 3 still builds on net48

**Tier 3 (Engine)**:
- Projects: Engine.csproj
- Dependencies: Entities, UseCases (Tiers 1-2)
- Completion criteria: Builds on net10.0, all tests pass, configuration migrated

## Tasks

### 01-prerequisites: Verify SDK and toolchain compatibility

Validate that the .NET 10.0 SDK is installed and global.json (if present) is compatible with the target framework. Ensure the development environment can target .NET 10.0 before making any project changes.

**Done when**: .NET 10.0 SDK verified installed, global.json compatibility confirmed (or no global.json present), build toolchain ready for multi-targeting

---

### 02-sdk-style-conversion: Convert projects to SDK-style format

Convert Entities, UseCases, and Engine projects from legacy .NET Framework csproj format to SDK-style format while staying on net48. This structural change must complete before TFM upgrades to separate project system concerns from API surface changes. packages.config will be migrated to PackageReference as part of this conversion.

Assessment shows all three projects are old-style with Project.0001 issues. Test project is already SDK-style.

**Done when**: All three projects use SDK-style csproj format, build successfully on net48, packages.config migrated to PackageReference, all tests pass

---

### 03-tier1-entities: Upgrade Tier 1 foundation library

Upgrade Entities.csproj to multi-target net48;net10.0. This is the foundation layer with no dependencies. Multi-targeting ensures higher tiers can continue consuming it on net48 during their upgrade phases.

Assessment notes: 2 mandatory issues (Project.0002 TFM change). No incompatible packages detected. This tier has zero project references and serves as the foundation for all other projects.

**Done when**: Entities builds on both net48 and net10.0 targets, all unit tests pass on both TFMs, Tier 2 (UseCases) still builds successfully on net48 consuming the multi-targeted Entities

---

### 04-tier2-business-logic: Upgrade Tier 2 business logic

Upgrade UseCases.csproj to multi-target net48;net10.0. This tier depends on Entities (already multi-targeted in Tier 1) and provides business logic consumed by the Engine application.

Assessment notes: 2 mandatory issues (Project.0002 TFM change). No incompatible packages detected. Depends on Entities only.

**Done when**: UseCases builds on both net48 and net10.0 targets, all unit tests pass on both TFMs, Tier 3 (Engine) still builds successfully on net48 consuming the multi-targeted UseCases

---

### 05-tier3-engine-app: Upgrade Tier 3 application and migrate configuration

Upgrade Engine.csproj to net10.0 (in-place, no multi-targeting needed as this is the top-level application). Migrate App.config appSettings to appsettings.json using Microsoft.Extensions.Configuration. Update project references to consume the net10.0 targets of Entities and UseCases. Remove net48 targets from Entities and UseCases after Engine upgrade completes (consolidation phase).

Assessment notes: 4 issues (2 mandatory Project.0001/0002, 2 potential Api.0002 source compatibility). App.config contains simple appSettings section with Profile key. This is the entry-point application with no downstream consumers.

**Done when**: Engine builds on net10.0, App.config migrated to appsettings.json with Configuration API in place, all tests pass, Entities and UseCases consolidated to net10.0-only (net48 targets removed)

---

### 06-final-validation: Solution-wide validation and deferred recommendations

Run full solution build and complete test suite to verify all projects work together on .NET 10.0. Document deferred Central Package Management recommendation (all projects are now SDK-style on single TFM — CPM can be added cleanly as post-upgrade enhancement).

**Done when**: Full solution builds without warnings on .NET 10.0, all tests pass, deferred CPM recommendation documented, upgrade artifacts (assessment, plan, execution log) finalized
