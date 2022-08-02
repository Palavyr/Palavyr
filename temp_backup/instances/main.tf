terraform {
  required_version = ">= 0.13.0"
}

resource "aws_instance" "tfer--i-002D-080f76ecdd5beceb2_New-0020-Production-0020-Palavyr" {
  ami                         = "ami-0b17e49efb8d755c3"
  associate_public_ip_address = "true"
  availability_zone           = "us-east-1a"

  capacity_reservation_specification {
    capacity_reservation_preference = "open"
  }

  cpu_core_count       = "1"
  cpu_threads_per_core = "1"

  credit_specification {
    cpu_credits = "standard"
  }

  disable_api_stop        = "false"
  disable_api_termination = "false"
  ebs_optimized           = "false"

  enclave_options {
    enabled = "false"
  }

  get_password_data                    = "false"
  hibernation                          = "false"
  instance_initiated_shutdown_behavior = "stop"
  instance_type                        = "t2.small"
  ipv6_address_count                   = "0"
  key_name                             = "PalavyrPem2021"

  maintenance_options {
    auto_recovery = "default"
  }

  metadata_options {
    http_endpoint               = "enabled"
    http_put_response_hop_limit = "1"
    http_tokens                 = "optional"
    instance_metadata_tags      = "disabled"
  }

  monitoring = "false"

  private_dns_name_options {
    enable_resource_name_dns_a_record    = "false"
    enable_resource_name_dns_aaaa_record = "false"
    hostname_type                        = "ip-name"
  }

  private_ip = "10.0.97.58"

  root_block_device {
    delete_on_termination = "true"
    encrypted             = "false"

    tags = {
      Palavyr = "EC2"
    }

    volume_size = "30"
    volume_type = "gp2"
  }

  source_dest_check = "true"
  subnet_id         = var.subnet_id_1_private

  tags = {
    Name    = "New Production Palavyr"
    Palavyr = "EC2"
  }

  tags_all = {
    Name    = "New Production Palavyr"
    Palavyr = "EC2"
  }

  tenancy                = "default"
  vpc_security_group_ids = var.security_group_ids
}

resource "aws_lb" "tfer--production-002D-palavyr-002D-loadbalancer" {
  desync_mitigation_mode     = "defensive"
  drop_invalid_header_fields = "false"
  enable_deletion_protection = "false"
  enable_http2               = "true"
  enable_waf_fail_open       = "false"
  idle_timeout               = "60"
  internal                   = "false"
  ip_address_type            = "ipv4"
  load_balancer_type         = "application"
  name                       = "production-palavyr-loadbalancer"
  security_groups            = var.security_group_ids #

  subnet_mapping {
    subnet_id = var.subnet_id_1_private
  }

  subnet_mapping {
    subnet_id = var.subnet_id_2_private
  }

  subnets = ["${var.subnet_id_2_private}", "${var.subnet_id_1_private}"]

  tags = {
    Palavyr = "ProductionLoadBalancer"
  }

  tags_all = {
    Palavyr = "ProductionLoadBalancer"
  }
}

resource "aws_lb_listener" "tfer--arn-003A-aws-003A-elasticloadbalancing-003A-us-002D-east-002D-1-003A-306885252482-003A-listener-002F-app-002F-production-002D-palavyr-002D-loadbalancer-002F-efce1837c433d91c-002F-a4a1d0728da3595f" {
  certificate_arn = "arn:aws:acm:us-east-1:306885252482:certificate/8ec5fa03-cd67-465d-be94-2ae3b064c2c0"

  default_action {
    target_group_arn = "arn:aws:elasticloadbalancing:us-east-1:306885252482:targetgroup/production-palavyr-targets/0883c2b756bf8cd3"
    type             = "forward"
  }

  load_balancer_arn = aws_lb.tfer--production-002D-palavyr-002D-loadbalancer.id
  port              = "443"
  protocol          = "HTTPS"
  ssl_policy        = "ELBSecurityPolicy-2016-08"
}

resource "aws_lb_listener" "tfer--arn-003A-aws-003A-elasticloadbalancing-003A-us-002D-east-002D-1-003A-306885252482-003A-listener-002F-app-002F-production-002D-palavyr-002D-loadbalancer-002F-efce1837c433d91c-002F-e056056495b34808" {
  default_action {
    target_group_arn = "arn:aws:elasticloadbalancing:us-east-1:306885252482:targetgroup/production-palavyr-targets/0883c2b756bf8cd3"
    type             = "forward"
  }

  load_balancer_arn = aws_lb.tfer--production-002D-palavyr-002D-loadbalancer.id
  port              = "80"
  protocol          = "HTTP"
}


resource "aws_lb_target_group" "tfer--production-002D-palavyr-002D-targets" {
  deregistration_delay = "300"

  health_check {
    enabled             = "true"
    healthy_threshold   = "5"
    interval            = "30"
    matcher             = "200"
    path                = "/healthcheck"
    port                = "traffic-port"
    protocol            = "HTTP"
    timeout             = "5"
    unhealthy_threshold = "2"
  }

  load_balancing_algorithm_type = "round_robin"
  name                          = "production-palavyr-targets"
  port                          = "80"
  protocol                      = "HTTP"
  protocol_version              = "HTTP1"
  slow_start                    = "0"

  stickiness {
    cookie_duration = "86400"
    enabled         = "false"
    type            = "lb_cookie"
  }

  tags = {
    Palavyr = "rds production"
  }

  tags_all = {
    Palavyr = "rds production"
  }

  target_type = "instance"
  vpc_id      = var.vpc_id
}

resource "aws_lb_target_group_attachment" "tfer--arn-003A-aws-003A-elasticloadbalancing-003A-us-002D-east-002D-1-003A-306885252482-003A-targetgroup-002F-production-002D-palavyr-002D-targets-002F-0883c2b756bf8cd3-002D-20220730012450999700000001" {
  target_group_arn = "arn:aws:elasticloadbalancing:us-east-1:306885252482:targetgroup/production-palavyr-targets/0883c2b756bf8cd3"
  target_id        = "i-080f76ecdd5beceb2"
}

resource "aws_network_interface" "tfer--eni-002D-0102422eb42393bf3" {
  description             = "ELB app/production-palavyr-loadbalancer/efce1837c433d91c"
  ipv4_prefix_count       = "0"
  ipv6_address_count      = "0"
  ipv6_prefix_count       = "0"
  private_ip              = "10.0.76.233"
  private_ip_list         = ["10.0.76.233"]
  private_ip_list_enabled = "true"
  security_groups         = var.security_group_ids
  source_dest_check       = "true"
  subnet_id               = var.subnet_id_1_private
}

resource "aws_network_interface" "tfer--eni-002D-02dd72ddd3694a443" {
  attachment {
    device_index = "0"
    instance     = "i-080f76ecdd5beceb2"
  }

  description             = "Primary network interface"
  ipv4_prefix_count       = "0"
  ipv6_address_count      = "0"
  ipv6_prefix_count       = "0"
  private_ip              = "10.0.97.58"
  private_ip_list         = ["10.0.97.58"]
  private_ip_list_enabled = "true"
  security_groups         = var.security_group_ids
  source_dest_check       = "true"
  subnet_id               = var.subnet_id_1_private

  tags = {
    Palavyr = "EC2"
  }

  tags_all = {
    Palavyr = "EC2"
  }
}

resource "aws_network_interface" "tfer--eni-002D-07e318bdc98fe7f92" {
  description             = "ELB app/production-palavyr-loadbalancer/efce1837c433d91c"
  ipv4_prefix_count       = "0"
  ipv6_address_count      = "0"
  ipv6_prefix_count       = "0"
  private_ip              = "10.1.90.140"
  private_ip_list         = ["10.1.90.140"]
  private_ip_list_enabled = "true"
  security_groups         = var.security_group_ids
  source_dest_check       = "true"
  subnet_id               = "subnet-0704cbef441993b25"
}

resource "aws_iam_role_policy_attachment" "tfer--AWSServiceRoleForElasticLoadBalancing_AWSElasticLoadBalancingServiceRolePolicy" {
  policy_arn = "arn:aws:iam::aws:policy/aws-service-role/AWSElasticLoadBalancingServiceRolePolicy"
  role       = "AWSServiceRoleForElasticLoadBalancing"
}


resource "aws_iam_role" "tfer--AWSServiceRoleForElasticLoadBalancing" {
  assume_role_policy = <<POLICY
{
  "Statement": [
    {
      "Action": "sts:AssumeRole",
      "Effect": "Allow",
      "Principal": {
        "Service": "elasticloadbalancing.amazonaws.com"
      }
    }
  ],
  "Version": "2012-10-17"
}
POLICY

  description          = "Allows ELB to call AWS services on your behalf."
  managed_policy_arns  = ["arn:aws:iam::aws:policy/aws-service-role/AWSElasticLoadBalancingServiceRolePolicy"]
  max_session_duration = "3600"
  name                 = "AWSServiceRoleForElasticLoadBalancing"
  path                 = "/aws-service-role/elasticloadbalancing.amazonaws.com/"
}
