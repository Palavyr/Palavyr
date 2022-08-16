output "connection_string" {
  value = "Server=${aws_db_instance.database.endpoint}<SplitMe>Port=${aws_db_instance.database.port}<SplitMe>Database=AppDatabase<SplitMe>User Id=${var.database_username}<SplitMe>Password=${var.db_password}"
}

output "database_subnets" {
  value = var.public_subnets
}

output "database_security_group_id" {
  value = aws_security_group.db.id
}
