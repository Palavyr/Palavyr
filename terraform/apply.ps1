Write-Host "Applying your current configuration..."

(Get-Content -path "./backend.tf" -Raw) -replace '#{TERRAFORM_WORKSPACE}','palavyr-local-dev'
(Get-Content -path "./backend.tf" -Raw) -replace '#{TERRAFORM_API_KEY}',"$env:TERRAFORM_API_KEY"

terraform apply --var-file="./envs/dev.tfvars"

(Get-Content -path "./backend.tf" -Raw) -replace 'palavyr-local-dev','#{TERRAFORM_WORKSPACE}'
(Get-Content -path "./backend.tf" -Raw) -replace "$env:TERRAFORM_API_KEY",'#{TERRAFORM_API_KEY}'
