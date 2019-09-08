param (
    [string]$apikey = $( Read-Host "Enter nuget Api Key:" )
)

del *.nupkg
C:\Windows\Microsoft.NET\Framework\v4.0.30319\msbuild.exe ".\MetacoItBitClient\MetacoItBit.csproj" -p:Configuration=Release


.\.nuget\NuGet.exe pack .\MetacoItBitClient\MetacoItBit.csproj -Prop Configuration=Release

forfiles /m *.nupkg /c "cmd /c .\.nuget\NuGet.exe push @FILE FEipouFE987F2BBO3UD8387A64Hhh -source https://nuget.org/ -ApiKey $apikey"  
(((dir *.nupkg).Name) -match "[0-9]+?\.[0-9]+?\.[0-9]+?\.[0-9]+")
$ver = $Matches.Item(0)
