# Task 04-tier2-business-logic Progress

## What Changed

Successfully upgraded UseCases.csproj (Tier 2 business logic) to multi-target net48;net10.0.

### UseCases.csproj
- ✅ Changed `TargetFramework` (singular) to `TargetFrameworks` (plural)
- ✅ Added multi-targeting: `net10.0;net48` (newest to oldest)
- ✅ Removed duplicate Nett package reference (old-style Reference)
- ✅ Made .NET Framework-specific references conditional:
  - System.Configuration
  - System.Data.DataSetExtensions
  - Microsoft.CSharp
  - System.Net.Http
  - These references now only apply when `TargetFramework == 'net48'`

## Validation Results

- **UseCases net48 build**: ✅ Success (1 warning - unreachable code)
- **UseCases net10.0 build**: ✅ Success (1 warning - unreachable code)
- **Between-tier validation**: ✅ Engine (Tier 3) still builds successfully on net48, consuming UseCases net48 target
- **Tests**: ✅ All 3 tests passed on net10.0

## Issues Resolved

1. **Duplicate Nett package reference** - Removed old-style `<Reference Include="Nett...">` (kept PackageReference only)
2. **MSB3245 warnings** - .NET Framework-specific assembly references not found in net10.0 target
   - **Resolution**: Added `Condition="'$(TargetFramework)' == 'net48'"` to .NET Framework-specific `<Reference>` elements

## Modified Files

- Sources/UseCases/UseCases.csproj

## Next Steps

Proceed to task 05-tier3-engine-app to upgrade Engine.csproj to net10.0 (in-place, no multi-targeting) and migrate App.config to appsettings.json.
