# data source to fetch hosted zone info from domain name:
data "aws_route53_zone" "hosted_zone" {
  name         = var.hosted_zone_domain_name
  private_zone = false
}
