
resource "aws_security_group" "tent" {
  name        = "secg-t-${var.autoscale_group_name}"
  description = "Open a port for tentacle to connect to the instance"
  vpc_id      = var.vpc_id

  ingress {
    from_port   = 10933
    to_port     = 10933
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

resource "aws_security_group" "this" {
  name        = "secg-https-${var.autoscale_group_name}"
  description = "Allow incoming traffic from the load balancer to the nginx port for the server"
  vpc_id      = var.vpc_id

  ingress {
    from_port   = 443
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
  security_groups = [aws_security_group.this.id, aws_security_group.tent.id]

  associate_public_ip_address = true

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

  tag {
    key                 = "name"
    value               = "palavyr-autoscale"
    propagate_at_launch = true
  }

  tag {
    key                 = "env"
    value               = var.environment
    propagate_at_launch = true
  }

  lifecycle {
    create_before_destroy = true
  }

  instance_refresh {
    strategy = "Rolling"
    preferences {
      min_healthy_percentage = 0
    }
  }
}
