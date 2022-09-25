#!bin/bash
echo "-------------------------------------Script-01"
wrkdir=$(pwd)
cd /root
mkdir .aws
cd .aws
touch credentials
echo "
[default]
aws_access_key_id = testUser
aws_secret_access_key = testAccessKey
region = us-east-1
" > credentials
aws configure set aws_access_key_id default_access_key --profile=localstack
aws configure set aws_secret_access_key default_secret_key --profile=localstack
aws configure set region us-east-1 --profile=localstack
echo "########### Listing profile ###########"
aws configure list --profile=localstack
cd $wrkdir