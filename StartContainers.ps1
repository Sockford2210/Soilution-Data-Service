[CmdletBinding()]
param(
    [switch]$Rebuild,
    [switch]$IncludeWebApp
)

$Dockerfile = ".\docker-compose-db-only.yml"

if ($IncludeWebApp){
    $Dockerfile = ".\docker-compose-full.yml"
}

if ($Rebuild){
    docker compose -f $Dockerfile build
}

docker compose -f $Dockerfile up -d

Write-Host "Environment running, press 'q' to stop"
$StopKey = 81 #q key
While ($KeyInfo.VirtualKeyCode -ne $StopKey) {
    $KeyInfo = $Host.UI.RawUI.ReadKey("NoEcho, IncludeKeyDown")
 }

 docker compose -f $Dockerfile down