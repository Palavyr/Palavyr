output "pdf_server_url" {
  description = "URL for the PDF server"
  value       = module.pdf_server.base_url
}

output "database_connection_string" {
  description = "value of the database_connection_string output"
  value       = module.database.connection_string
}

output "configuration_app_website_bucket"{
  description = "S3 bucket for the dashboard"
  value       = module.configuration_app_website.aws_s3_bucket.name
}
output "widget_app_website"{
  description = "S3 bucket for the dashboard"
  value       = module.configuration_app_website.aws_s3_bucket.name
}

output "user_data_bucket" {
  description = "value of the user_data_bucket output"
  value       = module.palavyr_user_data_bucket.bucket
}
