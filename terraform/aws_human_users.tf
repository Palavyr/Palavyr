# This takes care of attaching these users to the group. When you
# create your user below, make sure you link it to this resource by
# adding `module.iam_iam-[your_user_name].name` to the `users` array.
resource "aws_iam_policy_attachment" "human_user_policy_attachment" {
  name = "human_user_policy_attachment_${lower(var.environment)}"
  users = [
    module.i_am_paulgradie.iam_user_name
    # add more users here
  ]
  groups     = [aws_iam_group.human_users_group.name]
  policy_arn = aws_iam_policy.human_full_access_user_policy.arn
}

####    Add new users below    #####


module "i_am_paulgradie" {
  source  = "terraform-aws-modules/iam/aws//modules/iam-user"
  version = "5.2.0"
  # insert the 1 required variable here

  name                  = "paul-gradie-${lower(var.environment)}"
  create_iam_access_key = true
  tags                  = local.tags
}
output "paulgradie_access_key" {
  value     = module.i_am_paulgradie.iam_access_key_id
  sensitive = true
}

output "paulgradie_secret_key" {
  value     = module.i_am_paulgradie.iam_access_key_encrypted_secret
  sensitive = true
}

module "i_am_adambeck" {
  source  = "terraform-aws-modules/iam/aws//modules/iam-user"
  version = "5.2.0"
  # insert the 1 required variable here

  name                  = "adam-beck-${lower(var.environment)}"
  create_iam_access_key = true
  tags                  = local.tags

}

output "adambeck_access_key" {
  value     = module.i_am_adambeck.iam_access_key_id
  sensitive = true
}

output "adambeck_secret_key" {
  value     = module.i_am_adambeck.iam_access_key_encrypted_secret
  sensitive = true
}


######### Here is a template

# module "iam_iam-[your_user_name]" {
#   source  = "terraform-aws-modules/iam/aws//modules/iam-user"
#   version = "5.2.0"
#   # insert the 1 required variable here

#   name                  = "[your-username]-${lower(var.environment)}"
#   create_iam_access_key = true
#   tags                  = local.tags
# }

#  Don't forget to add the user to the user group!
