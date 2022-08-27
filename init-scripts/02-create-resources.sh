#!bin/bash
echo "-------------------------------------Script-02"
aws --endpoint-url=http://localhost:4566 s3 mb s3://palavyr-user-data-development
aws ses verify-email-identity --email-address admin@palavyr.com --endpoint-url=http://localhost:4566