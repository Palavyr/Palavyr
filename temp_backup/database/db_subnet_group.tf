# resource "aws_db_subnet_group" "tfer--default-002D-vpc-002D-7d14a107" {
#   description = "Created from the RDS Management Console"
#   name        = "default-vpc-7d14a107"
#   subnet_ids  = ["subnet-5ef86039", "subnet-b11c7b9f", "subnet-03321a49", "subnet-28107574", "subnet-d62e7cd9", "subnet-e19c30df"]
# }

# resource "aws_db_subnet_group" "tfer--palavyr-002D-db-002D-subnet-002D-group" {
#   description = "Palavyr Subnets for Serverless DBs"
#   name        = "palavyr-db-subnet-group"
#   subnet_ids  = ["${data.terraform_remote_state.subnet.outputs.aws_subnet_tfer--subnet-002D-0ccabdc7222055adf_id}", "${data.terraform_remote_state.subnet.outputs.aws_subnet_tfer--subnet-002D-0704cbef441993b25_id}"]
# }
