      #!/bin/bash

      ############    Open Firewall ports  ###############

      # For Tentacle
      echo "-A RH-Firewall-1-INPUT -m state --state NEW -m tcp -p tcp --dport 10933 -j ACCEPT" >> /etc/sysconfig/iptables

      # For ssh
      echo "-A RH-Firewall-1-INPUT -m state --state NEW -m tcp -p tcp --dport 22 -j ACCEPT" >> /etc/sysconfig/iptables

      # For palavyr server
      echo "-A RH-Firewall-1-INPUT -m state --state NEW -m tcp -p tcp --dport 5000 -j ACCEPT" >> /etc/sysconfig/iptables

      service iptables restart

      ############    Install docker and start it   ###############

      thumbprint="#{OCTOPUS_THUMBPRINT}"  # The thumbprint of your Octopus Server
      apiKey="#{OCTOPUS_API_KEY}"         # An Octopus Server api key with permission to add machines
      environment="#{Environment}"        # The environment to register the Tentacle in

      serverUrl="https://palavyr.octopus.app"  # The url of your Octopus server
      spaceName="Palavyr" # The name of the space to register the Tentacle in
      name=$HOSTNAME     # The name of the Tentacle at is will appear in the Octopus portal
      role="#{SERVER_DEPLOYMENT_TARGET_ROLE}"   # The role to assign to the Tentacle
      machinePolicy="Clean up Autoscale Targets on Scale Down"
      configFilePath="/etc/octopus/default/tentacle-default.config"
      applicationPath="/home/Octopus/Applications/"

      publicIp=$(curl http://169.254.169.254/latest/meta-data/public-ipv4)

      curl -L https://octopus.com/downloads/latest/Linux_x64TarGz/OctopusTentacle --output tentacle-linux_x64.tar.gz

      mkdir /opt/octopus
      tar xvzf tentacle-linux_x64.tar.gz -C /opt/octopus

      /opt/octopus/tentacle/Tentacle create-instance --config "$configFilePath"
      /opt/octopus/tentacle/Tentacle new-certificate --if-blank
      /opt/octopus/tentacle/Tentacle configure --port 10933 --noListen False --reset-trust --app "$applicationPath"
      /opt/octopus/tentacle/Tentacle configure --trust $thumbprint
      echo "Registering the Tentacle $name with server $serverUrl in environment $environment with role $role"
      /opt/octopus/tentacle/Tentacle register-with --server "$serverUrl" --apiKey "$apiKey" --space "$spaceName" --name "$name" --env "$environment" --role "$role" --policy "$machinePolicy" --publicHostName $publicIp

      /opt/octopus/tentacle/Tentacle service --install --start

      ############    Install docker and start it   ###############
      set -ex
      sudo yum update -y
      sudo amazon-linux-extras install docker -y
      sudo service docker start
      sudo usermod -a -G docker ec2-user
      sudo curl -L https://github.com/docker/compose/releases/latest/download/docker-compose-$(uname -s)-$(uname -m) -o /usr/local/bin/docker-compose
      sudo chmod +x /usr/local/bin/docker-compose

      docker-compose version

      ############# Get the aws cli ###############
      curl "https://awscli.amazonaws.com/awscli-exe-linux-x86_64.zip" -o "awscliv2.zip"
      unzip awscliv2.zip
      sudo ./aws/install

      ############# Create Aws Profile ###############
      ECRREGISTRY="#{ECR_REGISTRY}"

      mkdir ~/.aws -p

      echo "
[Palavyr_ecr]
aws_access_key_id = #{AWS_ECR_ACCESS_KEY}
aws_secret_access_key = #{AWS_ECR_SECRET_KEY}
" >> ~/.aws/credentials

      echo "
[default]
region = #{AWS_REGION}
output = text
" >> ~/.aws/config

      ############# Log in docker to the ECR registery ###############

      /usr/local/bin/aws ecr get-login-password --region "#{AWS_REGION}" | docker login --username AWS --password-stdin $env:ECR_REGISTRY
