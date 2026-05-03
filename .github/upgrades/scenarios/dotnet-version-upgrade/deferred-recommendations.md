# Deferred Recommendations

## Central Package Management (CPM)

### Current State

All projects in the solution are now SDK-style projects targeting .NET 10.0 with `PackageReference` format. Package versions are managed per-project:

- **Entities.csproj**: Nett 0.15.0
- **UseCases.csproj**: Nett 0.15.0
- **Engine.csproj**: 
  - Nett 0.15.0
  - Microsoft.CodeAnalysis.NetAnalyzers 10.0.100
  - Microsoft.Extensions.Configuration 9.0.1
  - Microsoft.Extensions.Configuration.Json 9.0.1
- **Test.csproj**: xunit, xunit.runner.visualstudio, coverlet.collector

### Recommendation

Consider adopting **NuGet Central Package Management (CPM)** to centralize package version management across all projects in the solution.

#### Benefits

1. **Single source of truth**: All package versions defined in one file (`Directory.Packages.props`)
2. **Version consistency**: Ensures all projects use the same version of shared packages (e.g., Nett 0.15.0 is used in 3 projects)
3. **Easier upgrades**: Update package versions in one place instead of editing multiple project files
4. **Prevents version drift**: No risk of different projects using different versions unintentionally

#### How to Implement

1. Create `Directory.Packages.props` at solution root
2. Move all `PackageReference` versions from project files to the props file
3. Set `<ManagePackageVersionsCentrally>true</ManagePackageVersionsCentrally>` in props file
4. Remove `Version` attributes from project file `<PackageReference>` elements (keep only `Include` attribute)

#### Example Structure

**Directory.Packages.props**:
```xml
<Project>
  <PropertyGroup>
	<ManagePackageVersionsCentrally>true</ManagePackageVersionsCentrally>
  </PropertyGroup>
  <ItemGroup>
	<PackageVersion Include="Nett" Version="0.15.0" />
	<PackageVersion Include="Microsoft.CodeAnalysis.NetAnalyzers" Version="10.0.100" />
	<PackageVersion Include="Microsoft.Extensions.Configuration" Version="9.0.1" />
	<PackageVersion Include="Microsoft.Extensions.Configuration.Json" Version="9.0.1" />
	<PackageVersion Include="xunit" Version="..." />
	<!-- etc -->
  </ItemGroup>
</Project>
```

**Project files** (Entities.csproj, UseCases.csproj, Engine.csproj):
```xml
<ItemGroup>
  <!-- Version attribute removed - managed centrally -->
  <PackageReference Include="Nett" />
</ItemGroup>
```

#### Why Deferred

CPM is best implemented as a **post-upgrade enhancement** for these reasons:

1. **Separation of concerns**: Framework upgrade and package management modernization are independent changes
2. **Reduced complexity**: Completing the TFM upgrade first ensures a stable baseline before introducing CPM
3. **Clean adoption**: All projects are now SDK-style on a single TFM, making CPM adoption straightforward with no multi-targeting complications

#### Next Steps (Optional, Post-Upgrade)

1. Review current package versions and decide on consolidation strategy
2. Create `Directory.Packages.props` with centralized versions
3. Update all project files to remove version attributes
4. Test build and verify all projects resolve packages correctly
5. Commit CPM adoption as a separate, focused change

### References

- [NuGet Central Package Management](https://learn.microsoft.com/en-us/nuget/consume-packages/central-package-management)
- [Directory.Packages.props schema](https://learn.microsoft.com/en-us/nuget/reference/directory-packages-props)
