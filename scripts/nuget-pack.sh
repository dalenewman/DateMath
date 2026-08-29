#!/usr/bin/env bash
set -euo pipefail

ROOT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
PROJECT="$ROOT_DIR/src/DateMath/DateMath.csproj"
OUTPUT_DIR="$ROOT_DIR/artifacts/nuget"
CONFIGURATION="Release"
PACKAGE_VERSION=""
NO_BUILD=0

usage() {
  echo "Usage: ./scripts/nuget-pack.sh [--package-version <version>] [--no-build]"
}

while [[ $# -gt 0 ]]; do
  case "$1" in
    --package-version)
      PACKAGE_VERSION="$2"
      shift 2
      ;;
    --no-build)
      NO_BUILD=1
      shift
      ;;
    --help|-h)
      usage
      exit 0
      ;;
    *)
      echo "Unknown option: $1" >&2
      usage >&2
      exit 1
      ;;
  esac
done

mkdir -p "$OUTPUT_DIR"
rm -f "$OUTPUT_DIR"/*.nupkg "$OUTPUT_DIR"/*.snupkg

command=(
  dotnet pack "$PROJECT"
  --configuration "$CONFIGURATION"
  --output "$OUTPUT_DIR"
  --nologo
  -p:ContinuousIntegrationBuild=true
)

if [[ "$NO_BUILD" -eq 1 ]]; then
  command+=(--no-build)
fi

if [[ -n "$PACKAGE_VERSION" ]]; then
  command+=(-p:PackageVersion="$PACKAGE_VERSION" -p:Version="$PACKAGE_VERSION")
fi

"${command[@]}"
echo "NuGet package output: $OUTPUT_DIR"
