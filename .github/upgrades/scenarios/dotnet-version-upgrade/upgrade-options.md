# Upgrade Options — Kifuwarakei

Assessment: 4 projects, 3 on .NET Framework 4.8 migrating to .NET 10.0, 3-tier dependency graph (Entities → UseCases → Engine/Test), no incompatible packages detected

## Strategy

### Upgrade Strategy
Multiple .NET Framework projects detected — Bottom-Up strategy is required to handle the Framework → modern .NET boundary with tier-by-tier validation.

| Value | Description |
|-------|-------------|
| **Bottom-Up** (selected) | Upgrade leaf-node libraries first (Entities), then work upward through dependency graph tier by tier (UseCases, then Engine). Each tier validated independently. |

## Project Structure

### Project Approach
All projects are class libraries or console apps (no System.Web dependencies) — multi-targeting allows gradual migration while keeping solution buildable.

| Value | Description |
|-------|-------------|
| **Multi-targeting** (selected) | Add new TFM alongside existing (net48;net10.0) so libraries serve both Framework and modern .NET consumers during transition. |
| In-place | Replace TFM directly — requires all consumers to migrate simultaneously. |

### Package Management
Solution has old-style .NET Framework projects crossing to modern .NET ecosystem — defer CPM until after migration stabilizes to avoid VersionOverride friction during multi-targeting.

| Value | Description |
|-------|-------------|
| **Per-Project (defer CPM to post-migration)** (selected) | Each project retains its own package versions during migration. CPM added as post-migration cleanup after all projects are SDK-style on single TFM. |
| Central Package Management (CPM) | Create Directory.Packages.props now — expect VersionOverride usage during multi-targeting. |

## Modernization

### Configuration Migration
Engine.csproj has App.config with appSettings — migrate to appsettings.json for modern .NET compatibility.

| Value | Description |
|-------|-------------|
| **Migrate to appsettings.json** (selected) | Convert App.config appSettings to appsettings.json with Microsoft.Extensions.Configuration. |
| Keep legacy App.config | Retain App.config — limited support in modern .NET, not recommended for long-term. |
