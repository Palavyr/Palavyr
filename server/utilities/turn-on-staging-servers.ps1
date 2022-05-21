Write-Host "Starting the EC2 Instance!"

$instanceId = "#{Palavyr.EC2.InstanceID}"
$octopusAPIKey = "#{Palavyr.Internal.APIKEY}"
$region = "#{AWS:Region}"

try {
    aws ec2 start-instances --region $region --instance-ids $instanceId
    Start-Sleep -Seconds 10
}
catch {
    Write-Host "Command To start instance failed. Perhaps its already started"
    exit 1
}

$status = $false;
$count = 0
while (!$status) {
    $power = Get-EC2InstanceStatus -InstanceId  $instanceId
    Write-Host "Current instance Status: $($power.InstanceState.Name)"
    $status = $power.InstanceState.Name -eq "running"

    $count += 1
    if ($count -gt 30) {
        Write-Host "Failed to start the EC2 Instance!"
        exit 1
    }

    if (!$status) {
        Write-Host "Ec2 Instance $instanceId is not running yet. Waiting for 10 seconds..."
        Start-Sleep -Seconds 10
    }
}



$updatedIP = aws ec2 describe-instances --instance-ids $instanceId --query 'Reservations[*].Instances[*].PublicIpAddress' --output text
$updatedURI = "https://$($updatedIP):10933/"


$octopusURL = "https://palavyr.octopus.app"
$header = @{ "X-Octopus-ApiKey" = $octopusAPIKey }


$spaceName = "Palavyr"
$space = (Invoke-RestMethod -Method Get -Uri "$octopusURL/api/spaces/all" -Headers $header) | Where-Object { $_.Name -eq $spaceName }


# migrator target
$machineAID = "Machines-103"
$machineA = Invoke-RestMethod -Method Get -Uri "$octopusURL/api/$($space.Id)/machines/$($machineAID)" -Headers $header
$machineA.Endpoint.Uri = $updatedURI
$machineA.Uri = $updatedURI
Invoke-RestMethod -Method Put -Uri "$octopusURL/api/$($space.Id)/machines/$($machineAID)" -Body ($machineA | ConvertTo-Json -Depth 10) -Headers $header
Invoke-RestMethod -Method Get -Uri "$octopusURL/api/$($space.Id)/machines/$($machineAID)/connection" -Headers $header

# server target
$machineBID = "Machines-21"
$machineB = Invoke-RestMethod -Method Get -Uri "$octopusURL/api/$($space.Id)/machines/$($machineBID)" -Headers $header
$machineB.Endpoint.Uri = $updatedURI
$machineB.Uri = $updatedURI
Invoke-RestMethod -Method Put -Uri "$octopusURL/api/$($space.Id)/machines/$($machineBID)" -Body ($machineB | ConvertTo-Json -Depth 10) -Headers $header
Invoke-RestMethod -Method Get -Uri "$octopusURL/api/$($space.Id)/machines/$($machineBID)/connection" -Headers $header


