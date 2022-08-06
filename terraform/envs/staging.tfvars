protect_from_deletion     = false
hosted_zone_domain_name   = "palavyr.com"
app_domain_name           = "staging.app.palavyr.com"
widget_domain_name        = "staging.widget.palavyr.com"
server_domain_name        = "staging.server.palavyr.com"
aws_region                = "${AWS_REGION}"
environment               = "${Environment}"
aws_account_id            = "${AWS_ACCOUNT_ID}"
ecr_access_key            = "${AWS_ECR_ACCESS_KEY}"
ecr_secret_key            = "${AWS_ECR_SECRET_KEY}"
scale_group_instance_type = "${SCALE_GROUP_INSTANCE_TYPE}"
database_instance_type    = "${DATABASE_INSTANCE_TYPE}"