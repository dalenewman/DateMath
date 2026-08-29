# NuGet Packaging and Publishing

DateMath uses cross-platform `dotnet` CLI scripts for packaging and publishing. Publishing is performed locally; no NuGet API key is stored in GitHub.

## Prerequisites

- The .NET SDK selected by [`global.json`](../global.json).
- A NuGet.org account that owns the `DateMath` package.
- A NuGet.org API key scoped to the `DateMath` package when publishing.

Run all commands from the repository root.

## Build and test

Restore, build, and test the release configuration before packaging:

```bash
dotnet restore
dotnet build --configuration Release --no-restore
dotnet test --configuration Release --no-build
```

## Package locally

Create the package using the version in [`DateMath.csproj`](../src/DateMath/DateMath.csproj):

```bash
./scripts/nuget-pack.sh --no-build
```

Omit `--no-build` when the release build has not already been created:

```bash
./scripts/nuget-pack.sh
```

To create a package with a temporary version override:

```bash
./scripts/nuget-pack.sh --package-version 1.3.1-preview.1
```

Packages are written to `artifacts/nuget`:

- `DateMath.<version>.nupkg` contains the library, XML documentation, package metadata, and README.
- `DateMath.<version>.snupkg` contains portable debugging symbols.

Inspect the package before publishing:

```bash
unzip -l artifacts/nuget/DateMath.1.3.1.nupkg
unzip -p artifacts/nuget/DateMath.1.3.1.nupkg DateMath.nuspec
```

Replace `1.3.1` with the version being released.

## Preview the publish

Confirm which package would be sent to NuGet.org without making any network changes:

```bash
./scripts/nuget-push.sh --dry-run
```

The publishing script only selects `.nupkg` files. The `dotnet` CLI also publishes the matching `.snupkg` when it is present in the same directory.

## Publish from a terminal

Keep the API key in an environment variable for the current terminal session. Do not add it to a repository file or persistent shell profile.

For zsh:

```zsh
read -rs "NUGET_API_KEY?NuGet API key: "
export NUGET_API_KEY
echo
./scripts/nuget-push.sh --api-key-env NUGET_API_KEY
unset NUGET_API_KEY
```

For bash:

```bash
read -rsp "NuGet API key: " NUGET_API_KEY
export NUGET_API_KEY
echo
./scripts/nuget-push.sh --api-key-env NUGET_API_KEY
unset NUGET_API_KEY
```

The script publishes to `https://api.nuget.org/v3/index.json` and uses `--skip-duplicate`, making an accidental rerun safe when the same package version already exists.

## Release checklist

1. Update `Version` and `FileVersion` in [`DateMath.csproj`](../src/DateMath/DateMath.csproj).
2. Add the release date and notes to [`CHANGELOG.md`](../CHANGELOG.md).
3. Restore, build, and run the tests in Release configuration.
4. Review and commit the release metadata so Source Link identifies the release commit.
5. Pack and inspect the `.nupkg` and `.snupkg` files.
6. Run the publishing script with `--dry-run`.
7. Publish using a session-only `NUGET_API_KEY` environment variable.
8. Confirm that the package and symbols finish validating on NuGet.org.
9. Tag the release commit and push the tag.

## GitHub Actions

The [`.NET` workflow](../.github/workflows/dotnet.yml) builds and tests on Linux, Windows, and macOS. It also uploads `.nupkg` and `.snupkg` artifacts for inspection. It does not publish packages and requires no NuGet.org secret.
