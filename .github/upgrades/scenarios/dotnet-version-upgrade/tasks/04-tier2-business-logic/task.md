# 04-tier2-business-logic: Upgrade Tier 2 business logic

Upgrade UseCases.csproj to multi-target net48;net10.0. This tier depends on Entities (already multi-targeted in Tier 1) and provides business logic consumed by the Engine application.

Assessment notes: 2 mandatory issues (Project.0002 TFM change). No incompatible packages detected. Depends on Entities only.

**Done when**: UseCases builds on both net48 and net10.0 targets, all unit tests pass on both TFMs, Tier 3 (Engine) still builds successfully on net48 consuming the multi-targeted UseCases
