# fetch ubuntu ami id:
# data "aws_ami" "my_ami" {
#   filter {
#     name   = "name"
#     values = ["ubuntu/images/hvm-ssd/ubuntu-focal-20.04-amd64-server-*"]
#   }

#   filter {
#     name   = "virtualization-type"
#     values = ["hvm"]
#   }
#   most_recent = true
#   owners      = ["099720109477"]
# }

data "aws_ami" "my_ami" {
  most_recent = true
  owners      = ["amazon"]

  filter {
    name   = "name"
    values = ["amzn2-ami-hvm-*-x86_64-ebs"]
  }
}


# generate user data script :
data "template_cloudinit_config" "deployment_data" {
  gzip          = false
  base64_encode = true

  part {
    content_type = "text/x-shellscript"
    content      = <<-EOT
    #! /bin/bash

    # Install docker and start it
    set -ex
    sudo yum update -y
    sudo amazon-linux-extras install docker -y
    sudo service docker start
    sudo usermod -a -G docker ec2-user

    # Install tentacle
    sudo apt-key adv --fetch-keys https://apt.octopus.com/public.key # Add Octopus public key to apt
    sudo add-apt-repository "deb https://apt.octopus.com/ stretch main" # Add Octopus repository to apt
    sudo apt-get update # Make sure everything else is up-to-date
    sudo apt-get install tentacle # Install Tentacle for Linux

    # Register the tentacle with Octopus

    serverUrl="https://palavyr.octopus.app" # Url to our Octopus server
    serverCommsPort=10933 # Port to use for the Polling Tentacle
    apiKey="${var.octopus_api_key}" # API key that has permission to add machines
    name=$HOSTNAME # Name of the Linux machine
    environment="${var.environment}"
    role="palavyr-autoscale"
    configFilePath="/etc/octopus/default/tentacle-default.config" # Location on disk to store the configuration
    applicationPath="/home/Octopus/Applications/" # Location where deployed applications will be installed to
    policy="Clean up Autoscale Targets on Scale Down"
    space="Palavyr"

    # Create a new Tentacle instance
    /opt/octopus/tentacle/Tentacle create-instance --config "$configFilePath"

    # Create a new self-signed certificate for secure communication with Octopus server
    /opt/octopus/tentacle/Tentacle new-certificate --if-blank

    # Configure the Tentacle specifying it is not a listening Tentacle and setting where deployed applications go
    /opt/octopus/tentacle/Tentacle configure --noListen False --reset-trust --app "$applicationPath"

    echo "Registering the Tentacle $name with server $serverUrl in environment $environment with role $role"

    /opt/octopus/tentacle/Tentacle register-with --server "$serverUrl" --apiKey "$apiKey" --name "$name" --env "$environment" --role "$role" --comms-style "TentacleActive" --server-comms-port $serverCommsPort --policy $policy --space $space

    sudo /opt/octopus/tentacle/Tentacle service --install --start
    EOT
  }
}

# data source to fetch hosted zone info from domain name:
data "aws_route53_zone" "hosted_zone" {
  name = var.hosted_zone_domain_name
}
