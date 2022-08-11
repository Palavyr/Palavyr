# This takes care of attaching these users to the group. When you
# create your user below, make sure you link it to this resource by
# adding `module.iam_iam-[your_user_name].name` to the `users` array.
resource "aws_iam_policy_attachment" "machine_user_policy_attachment" {
  name = "machine-upa-${lower(var.environment)}-${lower(random_id.rand.hex)}"
  users = [
    module.i_am_ecr.iam_user_name,
    module.i_am_palavyr.iam_user_name
    # add more machine users here
  ]
  groups     = [aws_iam_group.machine_users_group.name]
  policy_arn = aws_iam_policy.machine_full_access_user_policy.arn
}


####    Add new machine users below    #####
# --------------------------------------------------------------------------------------------------------------------------
module "i_am_ecr" {
  source  = "terraform-aws-modules/iam/aws//modules/iam-user"
  version = "5.2.0"

  name                  = "ecr-registry-${lower(var.environment)}-${lower(random_id.rand.hex)}"
  create_iam_access_key = true
  tags                  = local.tags
}
output "i_am_ecr_access_key" {
  value     = module.i_am_ecr.iam_access_key_id
  sensitive = true
}

output "i_am_ecr_secret_key" {
  value     = module.i_am_ecr.iam_access_key_encrypted_secret
  sensitive = true
}
#  --------------------------------------------------------
module "i_am_palavyr" {
  source  = "terraform-aws-modules/iam/aws//modules/iam-user"
  version = "5.2.0"

  name                  = "palavyr-${lower(var.environment)}-${lower(random_id.rand.hex)}"
  create_iam_access_key = true

  tags = local.tags
}

output "palavyr_access_key" {
  value     = module.i_am_palavyr.iam_access_key_id
  sensitive = true

}
output "palavyr_secret_key" {
  value     = module.i_am_palavyr.iam_access_key_secret
  sensitive = true
}
