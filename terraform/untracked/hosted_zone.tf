resource "aws_route53_record" "palavyr_record_NS" {
  name    = "palavyr.com"
  records = ["ns-1445.awsdns-52.org.", "ns-2035.awsdns-62.co.uk.", "ns-708.awsdns-24.net.", "ns-435.awsdns-54.com."]
  ttl     = "172800"
  type    = "NS"
  zone_id = aws_route53_zone.hosted_zone.zone_id
}

resource "aws_route53_record" "palavyr_record_SOA" {
  name    = "palavyr.com"
  records = ["ns-435.awsdns-54.com. awsdns-hostmaster.amazon.com. 1 7200 900 1209600 86400"]
  ttl     = "900"
  type    = "SOA"
  zone_id = aws_route53_zone.hosted_zone.zone_id
}

resource "aws_route53_zone" "hosted_zone" {
  comment       = "DNS for palavyr.com"
  force_destroy = "false"
  name          = "palavyr.com"

  tags = {
    Project = "Palavyr"
  }

  tags_all = {
    Project = "Palavyr"
  }
}
