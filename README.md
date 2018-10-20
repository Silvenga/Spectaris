# Spectaris

[![AppVeyor](https://img.shields.io/appveyor/ci/Silvenga/spectaris.svg?logo=appveyor&maxAge=3600&style=flat-square)](https://ci.appveyor.com/project/Silvenga/spectaris)
[![License](https://img.shields.io/github/license/silvenga/spectaris.svg?maxAge=86400&style=flat-square)](https://github.com/Silvenga/spectaris/blob/master/LICENSE)

An experiment in IIS request instrumentation.

## Usage

Built releases can be downloaded from the releases [page](https://github.com/Silvenga/Spectaris/releases). 

The easiest way to use the module is to install it into the GAC. The [`install-module.ps1`](./install-module.ps1) script can be used for this purpose. For example, in PowerShell:

```powershell
# Download the installer:
Invoke-WebRequest https://raw.githubusercontent.com/Silvenga/Spectaris/master/install-module.ps1 -OutFile install-module.ps1

# Download the module (in this case, version 1.0.0):
Invoke-WebRequest https://github.com/Silvenga/Spectaris/releases/download/1.0.0/Spectaris.dll -OutFile Spectaris.dll

# Install the module to instrument managed requests (an absolute path is required):
./install-module.ps1 -Assembly (Resolve-Path .\Spectaris.dll).Path -ManagedOnly
```