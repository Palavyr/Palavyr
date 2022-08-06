# Create Security Group for ALB Ingress :
resource "aws_security_group" "sec_group" {
  name   = "alg_http_ingress"
  vpc_id = module.vpc.vpc_id

  # Allow ingress to http port 80 from anywhere
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
}
