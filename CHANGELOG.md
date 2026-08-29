# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

## [1.3.1] - 2026-08-29

### Changed

- Consolidated the library and tests into modern SDK-style projects.
- Updated the test suite to .NET 10 and MSTest.Sdk 4.3.3.
- Replaced AppVeyor with cross-platform GitHub Actions build and test automation.
- Replaced the checked-in NuGet executable, batch script, and hand-authored nuspec with SDK-native pack and publish scripts.
- Added deterministic builds, nullable reference types, current analyzers, XML documentation, Source Link metadata, and NuGet symbol packages.
- Added documented terminal workflows for locally packing and publishing to NuGet.org with a session-only API key.
- Refreshed package metadata, documentation, and dependency update automation.

### Fixed

- Made tests involving `DateTime.UtcNow` resilient to clock-boundary timing.

## [1.3.0] - 2022-08-30

### Changed

- Removed obsolete package targets and retained the `netstandard2.0` target to eliminate vulnerable dependencies.

## [1.2.0] - 2020-10-14

### Fixed

- Corrected month and year arithmetic for leap years and variable-length months.

### Changed

- Updated NuGet license metadata to use the Apache-2.0 SPDX expression.

## [1.1.0] - 2019-12-23

### Added

- Added the `netstandard2.0` package target.

## [1.0.0] - 2016-12-28

### Added

- First stable release.
