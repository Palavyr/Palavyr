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

# data source to fetch hosted zone info from domain name:
data "aws_route53_zone" "hosted_zone" {
  name = var.hosted_zone_domain_name
}


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
      #!/bin/bash

      serverUrl="https://palavyr.octopus.app"  # The url of your Octopus server
      thumbprint="#{OCTOPUS_THUMBPRINT}"       # The thumbprint of your Octopus Server
      apiKey="#{OCTOPUS_API_KEY}"           # An Octopus Server api key with permission to add machines
      spaceName="Palavyr" # The name of the space to register the Tentacle in
      name=$HOSTNAME      # The name of the Tentacle at is will appear in the Octopus portal
      environment="#{Environment}"  # The environment to register the Tentacle in
      role="palavyr-autoscale"   # The role to assign to the Tentacle
      machinePolicy="Clean up Autoscale Targets on Scale Down"
      configFilePath="/etc/octopus/default/tentacle-default.config"
      applicationPath="/home/Octopus/Applications/"

      curl -L https://octopus.com/downloads/latest/Linux_x64TarGz/OctopusTentacle --output tentacle-linux_x64.tar.gz

      mkdir /opt/octopus
      tar xvzf tentacle-linux_x64.tar.gz -C /opt/octopus

      /opt/octopus/tentacle/Tentacle create-instance --config "$configFilePath"
      /opt/octopus/tentacle/Tentacle new-certificate --if-blank
      /opt/octopus/tentacle/Tentacle configure --port 10933 --noListen False --reset-trust --app "$applicationPath"
      /opt/octopus/tentacle/Tentacle configure --trust $thumbprint
      echo "Registering the Tentacle $name with server $serverUrl in environment $environment with role $role"
      /opt/octopus/tentacle/Tentacle register-with --server "$serverUrl" --apiKey "$apiKey" --space "$spaceName" --name "$name" --env "$environment" --role "$role" --policy "$machinePolicy"

      /opt/octopus/tentacle/Tentacle service --install --start

      ###########################

      # Install docker and start it
      set -ex
      sudo yum update -y
      sudo amazon-linux-extras install docker -y
      sudo service docker start
      sudo usermod -a -G docker ec2-user

      ############################




      EOT
  }
}

