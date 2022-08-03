# Deploy infra with terraform


palavyr.auto.tvars
environment = "#{Environment}"
resource_group_name = "#{ResourceGroupName}"


## Example

// Use the shared database
data "azurerm_sql_server" "sql" {
  name = "octopus-microsites-${lower(var.environment)}"
  resource_group_name = "Microsites-${var.environment}"
}

resource "azurerm_mssql_database" "db" {
  name                = "crows-nest"
  server_id           = data.azurerm_sql_server.sql.id
  sku_name            =  var.environment == "Production" ? "GP_S_Gen5_2" : "GP_S_Gen5_2"
  min_capacity    = 0.5
  auto_pause_delay_in_minutes = 60
  storage_account_type = "Local"
}

// Use the shared app service plan
data "azurerm_app_service_plan" "microsites" {
  name                = "octopus-microsites-${lower(var.environment)}"
  resource_group_name = "Microsites-${var.environment}"
}
