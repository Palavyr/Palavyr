terraformer plan aws --compact --path-pattern "{output}/{provider}" --resources="acm,alb,api_gateway,cloudfront,cloudtrail,ec2_instance,eni,iam,igw,lambda,nacl,rds,route53,route_table,s3,sg,subnet,vpc"


terraformer import plan .\generated\aws\plan.json