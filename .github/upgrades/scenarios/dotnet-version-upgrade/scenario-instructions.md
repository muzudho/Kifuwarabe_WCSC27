# .NET Version Upgrade

## Preferences
- **Flow Mode**: Automatic
- **Target Framework**: .NET 10.0 (LTS)

## Source Control
- **Source Branch**: master
- **Working Branch**: dotnet-version-upgrade-net10.0
- **Commit Strategy**: After Each Task

## Upgrade Options
**Source**: .github/upgrades/scenarios/dotnet-version-upgrade/upgrade-options.md

### Strategy
- Upgrade Strategy: Bottom-Up

### Project Structure
- Project Approach: Multi-targeting (Class Libraries)
- Package Management: Per-Project (defer CPM to post-migration)

### Modernization
- Configuration Migration: Migrate to appsettings.json

## Strategy
**Selected**: Bottom-Up (Dependency-First)
**Rationale**: Multiple .NET Framework projects with 3-tier dependency graph. Bottom-Up validates each tier independently before consuming tiers upgrade, ensuring stable foundation at each level.

### Execution Constraints
- Strict tier ordering: Tier N must complete and validate before Tier N+1
- Between-tier validation: after upgrading each tier, confirm higher tiers still build on old framework
- SDK-style conversion is separate from TFM upgrade — different failure modes, never merge
- Multi-targeting for libraries maintains buildability during transition
- Final consolidation phase removes net48 targets after all consumers upgraded
