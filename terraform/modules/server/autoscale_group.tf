# Create a security group for EC2 instances to allow ingress on port 80 :
resource "aws_security_group" "ec2_ingress" {
  name        = "ec2_http_ingress"
  description = "Used for autoscale group"
  vpc_id      = var.vpc_id

  # HTTP access from anywhere
  ingress {
    from_port   = 80
    to_port     = 80
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

# create launch configuration for ASG :
resource "aws_launch_configuration" "asg_launch_conf" {
  name_prefix     = "tf-cloufront-alb-"
  image_id        = data.aws_ami.ubuntu_ami.id
  instance_type   = var.instance_type
  user_data       = data.template_cloudinit_config.deployment_data.rendered
  security_groups = [aws_security_group.ec2_ingress.id]

  lifecycle {
    create_before_destroy = true
  }
}

# create ASG with Launch Configuration :
resource "aws_autoscaling_group" "asg" {
  name                 = var.autoscale_group_name
  launch_configuration = aws_launch_configuration.asg_launch_conf.name
  min_size             = 3
  max_size             = 10
  vpc_zone_identifier  = var.private_subnets
  target_group_arns    = [aws_lb_target_group.alb_tg.arn]

  lifecycle {
    create_before_destroy = true
  }

  depends_on = [
    var.vpc_id,
    aws_lb_target_group.alb_tg,
    aws_launch_configuration.asg_launch_conf
  ]
}