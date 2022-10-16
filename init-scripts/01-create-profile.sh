#!bin/bash
echo "-------------------------------------Script-01"
wrkdir=$(pwd)
cd /root || exit
mkdir .aws
cd .aws || exit
touch credentials
echo "
[default]
aws_access_key_id = testUser
aws_secret_access_key = testAccessKey
region = us-east-1
" > credentials
aws configure set aws_access_key_id default_access_key --profile=localstack --endpoint-url=http://localstack:4566
aws configure set aws_secret_access_key default_secret_key --profile=localstack  --endpoint-url=http://localstack:4566
aws configure set region us-east-1 --profile=localstack  --endpoint-url=http://localstack:4566
echo "########### Listing profile ###########"
aws configure list --profile=localstack  --endpoint-url=http://localstack:4566
cd $wrkdir