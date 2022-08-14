echo "Applying the current configuration..."

sed -i 's/#{TERRAFORM_WORKSPACE}/palavyr-local-dev/' backend.tf
sed -i 's/#{TERRAFORM_API_KEY}/'+$env:TERRAFORM_API_KEY+'/' backend.tf

terraform init
terraform destroy --var-file="./envs/dev.tfvars"

sed -i 's/palavyr-local-dev/#{TERRAFORM_WORKSPACE}/' backend.tf
sed -i 's/'+$env:TERRAFORM_API_KEY+'/#{TERRAFORM_API_KEY}/' backend.tf

echo "Finished."
