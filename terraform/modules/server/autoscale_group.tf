# Create a security group for EC2 instances to allow ingress on port 80 :
resource "aws_security_group" "this" {
  name        = "secgrp-${var.autoscale_group_name}"
  description = "Used for autoscale group"
  vpc_id      = var.vpc_id

  # HTTP access from anywhere
  ingress {
    from_port   = 80
    to_port     = 4001
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

resource "random_id" "this" {
  byte_length = 6
}

# create launch configuration for ASG :
resource "aws_launch_configuration" "this" {
  image_id      = data.aws_ami.my_ami.id
  instance_type = var.instance_type

  user_data       = data.template_cloudinit_config.deployment_data.rendered
  security_groups = [aws_security_group.this.id]

  lifecycle {
    create_before_destroy = true
  }
}

# create ASG with Launch Configuration :
resource "aws_autoscaling_group" "asg" {
  name                      = var.autoscale_group_name
  launch_configuration      = aws_launch_configuration.this.name
  min_size                  = 1
  max_size                  = 2
  vpc_zone_identifier       = var.private_subnets
  target_group_arns         = [aws_lb_target_group.alb_tg.arn]
  health_check_grace_period = 300
  health_check_type         = "EC2"
  force_delete              = true

  lifecycle {
    create_before_destroy = true
  }

  # depends_on = [
  #   var.vpc_id,
  #   var.private_subnets,
  #   aws_lb_target_group.alb_tg,
  #   aws_launch_configuration.this
  # ]
}
