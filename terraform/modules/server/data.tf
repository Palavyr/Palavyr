# fetch ubuntu ami id:
data "aws_ami" "ubuntu_ami" {
  filter {
    name   = "name"
    values = ["ubuntu/images/hvm-ssd/ubuntu-focal-20.04-amd64-server-*"]
  }

  filter {
    name   = "virtualization-type"
    values = ["hvm"]
  }
  most_recent = true
  owners      = ["099720109477"]
}
# generate user data script :
data "template_cloudinit_config" "deployment_data" {
  gzip          = false
  base64_encode = true

  part {
    content_type = "text/x-shellscript"
    content      = <<-EOT
    #! /bin/bash

    # install docker and start the server
    sudo yum update -y
    sudo yum install docker -y
    sudo service docker start
    sudo usermod -a -G docker ec2-user

    apt-key adv --fetch-keys https://apt.octopus.com/public.key
    apt-get update
    apt-get install tentacle

    /opt/octopus/tentacle/configure-tentacle.sh

    /opt/octopus/tentacle/Tentacle service --install --start

    # AWS_ACCESS_KEY_ID=${var.ecr_access_key} AWS_SECRET_ACCESS_KEY=${var.ecr_secret_key} aws ecr get-login-password --region ${var.aws_region} | docker login --username AWS --password-stdin ${var.aws_account_id}.dkr.ecr.${var.aws_region}.amazonaws.com

    # # pull this stage image and
    # docker pull ${var.aws_account_id}.dkr.ecr.${var.aws_region}.amazonaws.com/palavyr/palavyr-server:${lower(var.environment)}-latest
    # docker run -t palavyr/palavyr-server:${lower(var.environment)}-latest .

    EOT
  }
}

# data source to fetch hosted zone info from domain name:
data "aws_route53_zone" "hosted_zone" {
  name = var.hosted_zone_domain_name
}
