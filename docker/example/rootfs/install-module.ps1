param(
    [string] $Assembly,
    [switch] $ManagedOnly
)

# https://stackoverflow.com/a/45594261/2001966
# https://stackoverflow.com/a/24136074/2001966

$name = "Spectaris"
$module = "Spectaris.HttpModule"

Write-Host "Installing [$name] into the GAC."
Write-Host "Considering path [$Assembly] for installation."

[System.Reflection.Assembly]::Load('System.EnterpriseServices, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a') | Out-Null;
(New-Object System.EnterpriseServices.Internal.Publish).GacInstall($Assembly);

Write-Host "Installing module [$module]."

$fullName = ([System.Reflection.Assembly]::LoadFile($Assembly)).FullName;

Write-Host "Full name was detected as [$fullName]."

$type = "$module, $fullName"
$preCondition = "runtimeVersionv4.0"
if ($ManagedOnly)
{
    $preCondition = "managedHandler,runtimeVersionv4.0"
}

& "$Env:WinDir\system32\inetsrv\appcmd.exe" `
    add module `
    /name:"$name" `
    /type:"$type" `
    /preCondition:"$preCondition"

Write-Host "Install completed."