resource "aws_lb" "alb" {
  name            = var.application_load_balancer_name
  subnets         = var.public_subnets
  security_groups = [aws_security_group.this.id]
  internal        = false
  idle_timeout    = 60

  tags = var.tags
}

resource "aws_lb_target_group" "alb_tg" {
  name     = aws_lb.alb.name
  port     = 5000
  protocol = "HTTP"
  vpc_id   = var.vpc_id
  tags     = var.tags

  health_check {
    healthy_threshold   = 3
    unhealthy_threshold = 10
    timeout             = 5
    interval            = 10
    path                = "/healthcheck"
    port                = 5000
  }
}

resource "aws_lb_listener" "http" {
  load_balancer_arn = aws_lb.alb.arn
  port              = 80
  protocol          = "HTTP"

  default_action {
    type = "redirect"

    redirect {
      port        = 443
      protocol    = "HTTPS"
      status_code = "HTTP_301"
    }
  }
}

resource "aws_lb_listener" "https" {
  load_balancer_arn = aws_lb.alb.arn
  port              = 443
  protocol          = "HTTPS"

  certificate_arn = aws_acm_certificate.cert.arn

  default_action {
    target_group_arn = aws_lb_target_group.alb_tg.arn
    type             = "forward"
  }
}

resource "aws_lb_listener_certificate" "lb_cert" {
  listener_arn    = aws_lb_listener.https.arn
  certificate_arn = aws_acm_certificate.cert.arn
}
