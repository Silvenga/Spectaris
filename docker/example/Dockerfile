FROM microsoft/dotnet-framework-samples:aspnetapp

# 'Default Web Site' MUST be in single quotes - might be due to cmd argument parsing. /shrug
RUN & "$Env:WinDir\system32\inetsrv\appcmd.exe" set config 'Default Web Site' /section:"system.web/customErrors" /mode:"Off"; \
    & "$Env:WinDir\system32\inetsrv\appcmd.exe" set config 'Default Web Site' /section:"system.webServer/httpErrors" /errorMode:"Detailed"

COPY rootfs/ /

# More batch oddities, 
# I can't string interpolation due to only having access to single quotes, dropping down to a script.
RUN $dllPath = 'C:\Spectaris.dll'; \
    & 'C:\install-module.ps1' -Assembly $dllPath
