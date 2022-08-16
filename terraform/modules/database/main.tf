resource "aws_security_group" "db" {
  name        = "secg-db-https-${var.database_name}"
  description = "Allow incoming traffic from the load balancer to the nginx port for the server"
  vpc_id      = var.vpc_id

  ingress {
    from_port   = 5432
    to_port     = 5432
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"]
  }

  egress {
    from_port   = 0
    to_port     = 0
    protocol    = "-1"
    cidr_blocks = ["0.0.0.0/0"]
  }
  lifecycle {
    create_before_destroy = true
  }
}


resource "aws_db_subnet_group" "database" {
  description = "RDS subnet group for ${var.database_name}"
  name        = var.database_subnet_group_name
  subnet_ids  = var.public_subnets
}

resource "aws_db_parameter_group" "database" {
  name   = var.rds_param_group_name
  family = "postgres13"

  parameter {
    name  = "log_connections"
    value = "1"
  }
}

resource "aws_db_instance" "database" {
  identifier                            = var.database_name
  instance_class                        = var.instance_class
  username                              = var.database_username
  password                              = var.db_password
  vpc_security_group_ids                = [aws_security_group.db.id]
  deletion_protection                   = var.protect_from_deletion
  availability_zone                     = "${var.aws_region}a"
  allocated_storage                     = 10
  engine                                = "postgres"
  engine_version                        = "13.4"
  db_subnet_group_name                  = aws_db_subnet_group.database.name
  parameter_group_name                  = aws_db_parameter_group.database.name
  publicly_accessible                   = false
  skip_final_snapshot                   = true
  auto_minor_version_upgrade            = "true"
  iops                                  = 0
  backup_window                         = "07:44-08:14"
  maintenance_window                    = "sun:04:40-sun:05:10"
  max_allocated_storage                 = 1000
  performance_insights_enabled          = true
  performance_insights_retention_period = 7
  port                                  = 5420
  storage_encrypted                     = true

  tags = var.tags
}
