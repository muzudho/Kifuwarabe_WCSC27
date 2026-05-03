# 02-sdk-style-conversion: Convert projects to SDK-style format

Convert Entities, UseCases, and Engine projects from legacy .NET Framework csproj format to SDK-style format while staying on net48. This structural change must complete before TFM upgrades to separate project system concerns from API surface changes. packages.config will be migrated to PackageReference as part of this conversion.

Assessment shows all three projects are old-style with Project.0001 issues. Test project is already SDK-style.

**Done when**: All three projects use SDK-style csproj format, build successfully on net48, packages.config migrated to PackageReference, all tests pass
