# SupCLI

CLI-only C# port of the Sup!? command interface and P2FK contracts, intended to be a drop-in replacement for `SUP.exe` usage in `p2fk.io`.

## Goal

This repository removes the GUI layer and keeps only CLI delivery of the same core contract outputs used by the current API wrapper.

- Language: **C#**
- Runtime: **.NET 8**
- Platforms: **Windows, Linux, macOS**
- Interface: **CLI only (no GUI)**

## Contract Scope Ported

The CLI command contract mirrors the SUP command options used by the current API stack:

- `--getrootbytransactionid`
- `--getrootsbyaddress`
- `--getpublicaddressbykeyword`
- `--getkeywordbypublicaddress`
- `--getobjectbyaddress`
- `--getobjectbytransactionid`
- `--getobjectbyurn`
- `--getobjectbyfile`
- `--getobjectsbyaddress`
- `--getobjectsownedbyaddress`
- `--getobjectscreatedbyaddress`
- `--getobjectsbykeyword`
- `--getfoundobjects`
- `--getkeywordsbyaddress`
- `--getpublicmessagesbyaddress`
- `--getprivatemessagesbyaddress`
- `--getpublickeysbyaddress`
- `--getprofilebyaddress`
- `--getprofilebyurn`
- `--getinquirybyaddress`
- `--getinquirybytransactionid`
- `--getinquiriesbyaddress`
- `--getinquiriescreatedbyaddress`

Shared argument contract:

- `--versionbyte`
- `--username`
- `--password`
- `--url`
- `--address`
- `--tid`
- `--urn`
- `--keyword`
- `--filepath`
- `--skip`
- `--qty`
- `--verbose`

## Source Lineage

This project ports the CLI contract path from:

- Original SUP repository: https://github.com/embiimob/SUP
- API consumer/wrapper: https://github.com/embiimob/p2fk.io

Contract/core files are carried from SUP’s `P2FK/classes` and `P2FK/contracts` and wired to a CLI-only entrypoint.

## Prerequisites

- .NET 8 SDK: https://dotnet.microsoft.com/download/dotnet/8.0
- A synced blockchain node + RPC credentials compatible with your target chain/network
- For Windows Visual Studio workflow:
  - Visual Studio 2022 (17.8+ recommended)
  - Workload: **.NET desktop development** (or equivalent .NET build tools)

---

## Build in Windows Visual Studio (Primary Workflow)

### 1) Open project

1. Open **Visual Studio 2022**
2. **File > Open > Project/Solution**
3. Select:
   - `/home/runner/work/SupCLI/SupCLI/SupCLI.csproj` (or your local clone path)

### 2) Restore and build

From Visual Studio:

- Right-click project > **Restore NuGet Packages**
- Build > **Build Solution**

From terminal (PowerShell):

```powershell
dotnet restore
dotnet build -c Release
```

### 3) Run from source

```powershell
dotnet run -- --help
```

---

## Publish Artifacts (Produce CLI binaries)

### Windows x64 (framework-dependent)

```powershell
dotnet publish -c Release -r win-x64 --self-contained false -o .\publish\win-x64
```

### Windows x64 (self-contained single-file)

```powershell
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -o .\publish\win-x64-sc
```

### Linux x64

```bash
dotnet publish -c Release -r linux-x64 --self-contained true -p:PublishSingleFile=true -o ./publish/linux-x64
```

### macOS ARM64 (Apple Silicon)

```bash
dotnet publish -c Release -r osx-arm64 --self-contained true -p:PublishSingleFile=true -o ./publish/osx-arm64
```

### macOS x64 (Intel)

```bash
dotnet publish -c Release -r osx-x64 --self-contained true -p:PublishSingleFile=true -o ./publish/osx-x64
```

> The project assembly name is set to `SUP`, so Windows publish output is suitable for `SUP.exe` replacement usage.

---

## CLI Usage

### Show help

```bash
SUP --help
```

(or `SUP.exe --help` on Windows)

### Example commands

Get root by transaction id:

```bash
SUP --versionbyte 111 --getrootbytransactionid --password better-password --url http://127.0.0.1:18332 --tid 6d14b0dc526a431f611f16f29d684f73e6b01f0a59a0b7b3d9b8d951091c2422 --username good-user --verbose
```

Get roots by address:

```bash
SUP --address muVrFVk3ErfrnmWosLF4WixxRtDKfMx9bs --versionbyte 111 --getrootsbyaddress --password better-password --url http://127.0.0.1:18332 --username good-user
```

Get object by URN:

```bash
SUP --versionbyte 111 --getobjectbyurn --password better-password --url http://127.0.0.1:18332 --username good-user --urn twitter.com
```

Get profile by URN:

```bash
SUP --versionbyte 111 --getprofilebyurn --password better-password --url http://127.0.0.1:18332 --username good-user --urn embii4u
```

---

## Replacing SUP.exe for p2fk.io

`p2fk.io` builds command strings and executes the CLI binary path configured in `Wrapper.cs`.

To replace current CLI usage:

1. Build/publish this project for your host OS.
2. Place output binary where `p2fk.io` expects it (example: `C:\SUP\SUP.exe`).
3. Keep command-line contract unchanged (this project does that).
4. Keep RPC settings (`--url`, `--username`, `--password`, `--versionbyte`) aligned with your node config.

`p2fk.io` controller routes currently rely on command verbs listed above, so preserving these flags is required for compatibility.

---

## Notes

- This repository is intentionally **CLI-only** and does **not** include the SUP GUI.
- Contract execution still depends on chain RPC access and local root/index files produced by sync workflows.
- First-time chain sync/indexing can take substantial time and disk.
