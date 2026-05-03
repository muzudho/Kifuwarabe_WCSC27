# 05-tier3-engine-app: Upgrade Tier 3 application and migrate configuration

Upgrade Engine.csproj to net10.0 (in-place, no multi-targeting needed as this is the top-level application). Migrate App.config appSettings to appsettings.json using Microsoft.Extensions.Configuration. Update project references to consume the net10.0 targets of Entities and UseCases. Remove net48 targets from Entities and UseCases after Engine upgrade completes (consolidation phase).

Assessment notes: 4 issues (2 mandatory Project.0001/0002, 2 potential Api.0002 source compatibility). App.config contains simple appSettings section with Profile key. This is the entry-point application with no downstream consumers.

**Done when**: Engine builds on net10.0, App.config migrated to appsettings.json with Configuration API in place, all tests pass, Entities and UseCases consolidated to net10.0-only (net48 targets removed)
