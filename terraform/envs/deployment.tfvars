environment               = "#{Environment}"
app_domain_name           = "#{DomainPrefixWithDot}app.palavyr.com"
widget_domain_name        = "#{DomainPrefixWithDot}widget.palavyr.com"
server_domain_name        = "#{DomainPrefixWithDot}server.palavyr.com"
protect_from_deletion     = "#{ProtectResourcesFromDeletion}"
hosted_zone_domain_name   = "palavyr.com"
aws_region                = "#{AWS_REGION}"
aws_account_id            = "#{AWS_ACCOUNT_ID}"
ecr_access_key            = "#{AWS_ECR_ACCESS_KEY}"
ecr_secret_key            = "#{AWS_ECR_SECRET_KEY}"
database_instance_type    = "db.t3.micro"
scale_group_instance_type = "t2.micro"
database_password         = "#{DATABASE_PASSWORD}"
role                      = "palavyr-autoscale"
octopus_api_key           = "#{OCTOPUS_API_KEY}"
terraform_api_key         = "#{TERRAFORM_API_KEY}"
terraform_workspace       = "#{TERRAFORM_WORKSPACE}"