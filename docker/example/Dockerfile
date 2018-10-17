FROM microsoft/dotnet-framework-samples:aspnetapp

# 'Default Web Site' MUST be in single quotes - might be due to cmd argument parsing. /shrug
RUN & "$Env:WinDir\system32\inetsrv\appcmd.exe" set config 'Default Web Site' /section:"system.web/customErrors" /mode:"Off"; \
    & "$Env:WinDir\system32\inetsrv\appcmd.exe" set config 'Default Web Site' /section:"system.webServer/httpErrors" /errorMode:"Detailed"

# Docker support is amazing!
# https://www.richard-banks.org/2017/02/debug-net-in-windows-container.html
EXPOSE 4020 4021
RUN $downloadUrl = 'https://download.visualstudio.microsoft.com/download/pr/7dc739dc-7ac6-4d3e-9a1a-8545ec7b4b2b/5ad9ec4cc15ad30e7560dfb8620ad4bf/vs_remotetools.exe'; \
    Invoke-WebRequest -OutFile c:\vs_remotetools.exe -Uri $downloadUrl; \
    & 'c:\vs_remotetools.exe' /install /quiet \
    rm c:\vs_remotetools.exe

COPY rootfs/ /

# More batch oddities, 
# I can't string interpolation due to only having access to single quotes, dropping down to a script.
RUN $dllPath = 'C:\Spectaris.dll'; \
    & 'C:\install-module.ps1' -Assembly $dllPath
