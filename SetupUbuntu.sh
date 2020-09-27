###### CONFIGURE AN UBUNTU EC2 INSTANCE FOR DEPLOYMENT AS WEB API   #####

mkdir palavyrServer
mkdir palavyrPDF

## Create the credentials directory
cd $HOME
mkdir .aws
cd .aws
## CREATE credentials and include
  # [default]
  # aws_access_key_id = ##
  # aws_secret_access_key = ##
  # region = ap-southeast-2


## Install dotnet core
wget https://packages.microsoft.com/config/ubuntu/20.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb
sudo apt-get update; \
  sudo apt-get install -y apt-transport-https && \
  sudo apt-get update && \
  sudo apt-get install -y dotnet-sdk-3.1


# install the linux build of tentacle
sudo apt-key adv --fetch-keys https://apt.octopus.com/public.key
sudo add-apt-repository "deb https://apt.octopus.com/ stretch main"
sudo apt-get update

sudo apt-get install tentacle   # install tentacle
sudo /opt/octopus/tentacle/configure-tentacle.sh  # follow the 
sudo /opt/octopus/tentacle/Tentacle service --install --start --instance "palavyrUbuntuTentacle"

## If you pass the worong configuration (thumbprint) to the tentacle, like I did onthe first go...
sudo /opt/octopus/tentacle/Tentacle configure --trust=68950398753876384508374503874503 (this is not a real thumprint...)
sudo /opt/octopus/tentacle/Tentacle service --restart


## install some leet dependencies
sudo apt-get install nodejs                 # node js ftw
sudo ln -s /usr/bin/nodejs /usr/bin/node    # may not work, ignore
sudo apt-get install npm                    # npm
sudo npm install pm2 -g                     # process manager


## install postgress and assign permissions and roles
##https://www.digitalocean.com/community/tutorials/how-to-install-postgresql-on-ubuntu-20-04-quickstart
sudo apt-get update && sudo apt-get -y upgrade
sudo apt install postgresql postgresql-contrib

## Setting the configs correctl
#https://www.shubhamdipt.com/blog/postgresql-on-ec2-ubuntu-in-aws/  

sudo -u postgres psql
postgres=#\password​

## start the db
Success. You can now start the database server using:
    pg_ctlcluster 12 main start

## Create a new server/database?

psql -U postgres -W 
username=postgres
CREATE SERVER myserver FOREIGN DATA WRAPPER postgres_fdw OPTIONS (host 'localhost', dbname 'palavyr_staging', port '5432');


