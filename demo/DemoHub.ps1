param(
    [string] $HubName = "DemoHub",
    [int] $NumberOfAirDevices = 1,
    [int] $ReadingIntervalSeconds = 5,
    [string] $HostName = "http://localhost:5000",
    [int] $NumberOfReadings = 100
)

function Send-AirQualityReadings{
    param(
        [string] $Url,
        [string] $DeviceName
    )

    $humidity = Get-Random -Maximum 50 -Minimum 40
    $temperature = Get-Random -Maximum 25 -Minimum 23
    $c02 = Get-Random -Maximum 1000 -Minimum 900

    $timestamp = (Get-Date).ToString("yyyy-MM-ddThh:mm:ss")

    $body = @{
        "deviceName" = $DeviceName
        "timestamp" = $timestamp
        "humidityPercentage" = $humidity
        "temperatureCelcius" = $temperature
        "co2ppm" =$c02
    }

    Invoke-RestMethod -Method 'Post' -Uri $Url -Body ($body|ConvertTo-Json) -ContentType "application/json"
}

function Send-NewHub{
    param(
        [string] $Url,
        [string] $HubName
    )

    $body = @{
        "DeviceName" = $HubName
    }

    Invoke-RestMethod -Method 'Post' -Uri $Url -Body ($body|ConvertTo-Json) -ContentType "application/json"
}

function Send-NewDevice{
    param(
        [string] $Url,
        [string] $HubName,
        [string] $DeviceName
    )

    $body = @{
        "name" = $DeviceName
        "hubName" = $HubName
    }

    Invoke-RestMethod -Method 'Post' -Uri $Url -Body ($body|ConvertTo-Json) -ContentType "application/json"
}

$airQualityReadingUrl = "$HostName/api/AirData"
$createHubUrl = "$HostName/api/DataHub"
$createDeviceUrl = "$HostName/api/AirData/device"

Send-NewHub -Url $createHubUrl -HubName $HubName

for($readings = 0; $readings -lt $NumberOfReadings; $readings++){
    for ($devNum = 1; $devNum -le $NumberOfAirDevices; $devNum++) {
        $deviceName = $HubName + $devNum.ToString()

        if ($readings -eq 1){       
            Send-NewDevice -Url $createDeviceUrl -HubName $HubName -DeviceName $deviceName
        }

        Send-AirQualityReadings -Url $airQualityReadingUrl -DeviceName $deviceName
    }

    Start-Sleep -Seconds $ReadingIntervalSeconds
}
