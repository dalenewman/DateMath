#!/usr/bin/env bash
set -euo pipefail

ROOT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
PACKAGE_DIR="$ROOT_DIR/artifacts/nuget"
SOURCE_URL="https://api.nuget.org/v3/index.json"
API_KEY=""
API_KEY_ENV=""
DRY_RUN=0

usage() {
  echo "Usage: ./scripts/nuget-push.sh [--api-key <value> | --api-key-env <name>] [--dry-run]"
}

while [[ $# -gt 0 ]]; do
  case "$1" in
    --api-key)
      API_KEY="$2"
      shift 2
      ;;
    --api-key-env)
      API_KEY_ENV="$2"
      shift 2
      ;;
    --dry-run)
      DRY_RUN=1
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

if [[ -n "$API_KEY_ENV" ]]; then
  API_KEY="${!API_KEY_ENV:-}"
fi

packages=("$PACKAGE_DIR"/*.nupkg)
if [[ ! -e "${packages[0]}" ]]; then
  echo "No .nupkg files found in: $PACKAGE_DIR" >&2
  exit 1
fi

if [[ "$DRY_RUN" -eq 1 ]]; then
  printf 'Would push: %s\n' "${packages[@]}"
  exit 0
fi

if [[ -z "$API_KEY" ]]; then
  echo "NuGet API key is required. Use --api-key or --api-key-env." >&2
  exit 1
fi

for package in "${packages[@]}"; do
  dotnet nuget push "$package" \
    --source "$SOURCE_URL" \
    --api-key "$API_KEY" \
    --skip-duplicate
done
