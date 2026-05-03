# 03-tier1-entities: Upgrade Tier 1 foundation library

Upgrade Entities.csproj to multi-target net48;net10.0. This is the foundation layer with no dependencies. Multi-targeting ensures higher tiers can continue consuming it on net48 during their upgrade phases.

Assessment notes: 2 mandatory issues (Project.0002 TFM change). No incompatible packages detected. This tier has zero project references and serves as the foundation for all other projects.

**Done when**: Entities builds on both net48 and net10.0 targets, all unit tests pass on both TFMs, Tier 2 (UseCases) still builds successfully on net48 consuming the multi-targeted Entities
