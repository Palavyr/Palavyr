#!bin/bash

# Make the dev bucket
aws --endpoint-url=http://localhost:4566 s3 mb s3://palavyr-user-data-development

# create verified email address for the default user
aws ses verify-email-identity --email-address admin@palavyr.com --endpoint-url=http://localhost:4566

