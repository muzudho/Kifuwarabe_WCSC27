# Task 02-sdk-style-conversion Progress

## What Changed

Successfully converted 3 projects from legacy .NET Framework csproj format to SDK-style format while staying on net48:

### Entities.csproj
- ✅ Converted to SDK-style format
- ✅ Migrated packages.config to PackageReference (Nett 0.15.0)
- ✅ Removed duplicate Nett package reference
- ✅ Added DefineConstants (DEBUG;TRACE) to all build configurations
- ✅ Fixed missing using directive (Grayscale.Kifuwarakei.Entities.Language) in DEBUG blocks
- ✅ Fixed undefined DEBUG variables (jibun, aite, Teban) in DoMove and UndoMove methods
- ✅ Temporarily commented out undefined Util_Tansaku.KaisiTaikyokusya references (DEBUG-only diagnostic code)

### UseCases.csproj
- ✅ Converted to SDK-style format
- ✅ Migrated packages.config to PackageReference
- ✅ Builds successfully on net48

### Engine.csproj
- ✅ Converted to SDK-style format
- ✅ Migrated packages.config to PackageReference
- ✅ Builds successfully on net48

### Cleanup
- ✅ All packages.config files removed from converted projects

## Validation Results

- **Entities build**: ✅ Success (1 warning - unreachable code in DEBUG block)
- **UseCases build**: ✅ Success (1 warning - unreachable code)
- **Engine build**: ✅ Success (1 warning - analyzer version)
- **Solution build**: ✅ Success (5 warnings total, all non-blocking)
- **Tests**: ✅ All 3 tests passed

## Issues Resolved

1. **Duplicate Nett package reference** in Entities.csproj (old-style Reference + PackageReference)
2. **Missing DefineConstants** (DEBUG;TRACE) causing conditional compilation issues
3. **Missing using directives** for Grayscale.Kifuwarakei.Entities.Language namespace in DEBUG blocks
4. **Undefined DEBUG variables** (jibun, aite, Teban) in Kyokumen.cs methods
5. **Undefined Util_Tansaku.KaisiTaikyokusya** - temporarily commented out (DEBUG diagnostic code only)

## Modified Files

- Sources/Entities/Entities.csproj
- Sources/Entities/Features/abstracts/Util_ConsoleGame.cs (added missing using)
- Sources/Entities/Features/implements/Shogiban.cs (added missing using)
- Sources/Entities/Features/implements/Kyokumen.cs (fixed DEBUG variables, commented out undefined references)
- Sources/UseCases/UseCases.csproj
- Sources/Engine/Engine.csproj
- Deleted: Sources/Entities/packages.config
- Deleted: Sources/UseCases/packages.config
- Deleted: Sources/Engine/packages.config

## Next Steps

Proceed to task 03-tier1-entities to upgrade Entities.csproj to multi-target net48;net10.0.
