output "connection_string" {
  value = "Server=${aws_db_instance.database.endpoint}<SplitMe>Port=${aws_db_instance.database.port}<SplitMe>Database=AppDatabase<SplitMe>User Id=${var.database_username}<SplitMe>Password=${var.db_password}"
}
