
# resource "aws_security_group" "all_open_temp" {
#   name        = "secg-aot-${var.autoscale_group_name}"
#   description = "All ports open temporarily for testing"
#   vpc_id      = var.vpc_id

#   ingress {
#     from_port   = 0
#     to_port     = 0
#     protocol    = "tcp"
#     cidr_blocks = ["0.0.0.0/0"]
#   }

#   egress {
#     from_port   = 0
#     to_port     = 0
#     protocol    = "-1"
#     cidr_blocks = ["0.0.0.0/0"]
#   }
#   lifecycle {
#     create_before_destroy = true
#   }
# }

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
    create_before_destroy = false
  }
}

resource "aws_security_group" "ssh" {
  name        = "secg-ssh-${var.autoscale_group_name}"
  description = "Open a port for ssh"
  vpc_id      = var.vpc_id

  ingress {
    from_port   = 22
    to_port     = 22
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
    create_before_destroy = false
  }
}

resource "aws_security_group" "listener_forward" {
  name        = "secg-lf-http-${var.autoscale_group_name}"
  description = "Allow incoming traffic from the load balancer to the nginx port for the server"
  vpc_id      = var.vpc_id

  ingress {
    from_port   = 5000
    to_port     = 5000
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
    create_before_destroy = false
  }
}

resource "random_id" "this" {
  byte_length = 6
}

# create launch configuration for ASG :
resource "aws_launch_configuration" "this" {
  image_id      = data.aws_ami.my_ami.id
  instance_type = var.instance_type

  user_data = data.template_cloudinit_config.deployment_data.rendered
  security_groups = [
    # aws_security_group.listener.id,
    aws_security_group.listener_forward.id,
    aws_security_group.tent.id,
    aws_security_group.ssh.id
    # aws_security_group.all_open_temp.id
  ]

  associate_public_ip_address = true

  key_name = "palavyr-autoscale"

  lifecycle {
    create_before_destroy = false
  }
}

# create ASG with Launch Configuration :
resource "aws_autoscaling_group" "asg" {
  name                      = var.autoscale_group_name
  launch_configuration      = aws_launch_configuration.this.name
  min_size                  = 1
  max_size                  = 1
  vpc_zone_identifier       = [var.public_subnets[0], var.public_subnets[1]]
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
