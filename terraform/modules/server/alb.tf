
resource "aws_lb" "alb" {
  name            = var.application_load_balancer_name
  subnets         = var.public_subnets
  security_groups = [var.security_group_id]
  internal        = false
  idle_timeout    = 60
  tags            = var.tags
}

resource "aws_lb_target_group" "alb_tg" {
  name     = "tg-https-${var.application_load_balancer_name}"
  port     = 443
  protocol = "HTTPS"
  vpc_id   = var.vpc_id
  tags     = var.tags

  health_check {
    healthy_threshold   = 3
    unhealthy_threshold = 10
    timeout             = 5
    interval            = 10
    path                = "/"
    port                = 443
  }
}

resource "aws_lb_listener" "alb_listener" {
  load_balancer_arn = aws_lb.alb.arn
  port              = 443
  protocol          = "HTTPS"

  default_action {
    target_group_arn = aws_lb_target_group.alb_tg.arn
    type             = "forward"
  }
}
