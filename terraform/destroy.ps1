Write-Host "Applying your current configuration..."
(Get-Content -path "./backend.tf" -Raw) -replace '#{TERRAFORM_WORKSPACE}', 'palavyr-dev' | Out-File -path "./backend.tf" -Encoding UTF8
(Get-Content -path "./backend.tf" -Raw) -replace '#{TERRAFORM_API_KEY}', "$env:TERRAFORM_API_KEY" | Out-File -path "./backend.tf" -Encoding UTF8

try {
    terraform init
    terraform destroy --var-file="./envs/dev.tfvars"
}
finally {
    (Get-Content -path "./backend.tf" -Raw) -replace 'palavyr-dev', '#{TERRAFORM_WORKSPACE}' | Out-File -path "./backend.tf" -Encoding UTF8
    (Get-Content -path "./backend.tf" -Raw) -replace "$env:TERRAFORM_API_KEY", '#{TERRAFORM_API_KEY}' | Out-File -path "./backend.tf" -Encoding UTF8

}