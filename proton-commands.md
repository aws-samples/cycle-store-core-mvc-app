## copy my AWS account number to a variable name account_id
account_id=$(aws sts get-caller-identity --output text --query Account)

## create an Amazon S3 bucket to store our environment and service template
aws s3api create-bucket --bucket "proton-cli-templates-${account_id}" --region us-east-1

## move to the directory where I have cloned template from git-hub
cd aws-proton-sample-templates/loadbalanced-fargate-svc/

## IAM role to provision infrastructure
aws iam create-role --role-name ProtonServiceRole --assume-role-policy-document file://./policies/proton-service-assume-policy.json

aws iam attach-role-policy --role-name ProtonServiceRole --policy-arn arn:aws:iam::aws:policy/AdministratorAccess

## Associate ProtonServiceRole role

aws proton-preview \
  --endpoint-url https://proton.us-east-1.amazonaws.com \
  --region us-east-1 \
  update-account-roles \
  --account-role-details "pipelineServiceRoleArn=arn:aws:iam::${account_id}:role/ProtonServiceRole"
  
## create environment template entry

aws proton-preview \
--endpoint-url https://proton.us-east-1.amazonaws.com \
--region us-east-1 \
create-environment-template \
--template-name "cycle-store-dev-env-template" \
--display-name "cycle-store-Dev-VPC" \
--description "Cycle-store Dev VPC with Public Access and ECS Cluster"

## tag this templete with major version

aws proton-preview \
--endpoint-url https://proton.us-east-1.amazonaws.com \
--region us-east-1 \
create-environment-template-major-version \
--template-name "cycle-store-dev-env-template" \
--description "Version 1"

## compress this and upload it to our dedicated S3 bucket
tar -zcvf env-template.tar.gz environment/ && aws s3 cp env-template.tar.gz s3://proton-cli-templates-${account_id}/env-template.tar.gz && rm env-template.tar.gz

## to let Proton know that we have a new version of our environment template available
aws proton-preview \
--endpoint-url https://proton.us-east-1.amazonaws.com \
--region us-east-1 \
create-environment-template-minor-version \
--template-name "cycle-store-dev-env-template" \
--description "Cycle store Dev Environment Version 1" \
--major-version-id "1" \
--source-s3-bucket proton-cli-templates-${account_id} \
--source-s3-key env-template.tar.gz

aws proton-preview \
--endpoint-url https://proton.us-east-1.amazonaws.com \
--region us-east-1 \
wait environment-template-registration-complete \
--template-name "cycle-store-dev-env-template" \
--major-version-id "1" \
--minor-version-id "0"

## Publish
aws proton-preview \
--endpoint-url https://proton.us-east-1.amazonaws.com \
--region us-east-1 \
update-environment-template-minor-version \
--template-name "cycle-store-dev-env-template" \
--major-version-id "1" \
--minor-version-id "0" \
--status "PUBLISHED"

## Service Template provision:

aws proton-preview \
--endpoint-url https://proton.us-east-1.amazonaws.com \
--region us-east-1 \
create-service-template \
--template-name "lb-cycle-store-service-template" \
--display-name "LoadbalancedCycleStoreService" \
--description "Fargate Service with an Application Load Balancer"

aws proton-preview \
--endpoint-url https://proton.us-east-1.amazonaws.com \
--region us-east-1 \
create-service-template-major-version \
--template-name "lb-cycle-store-service-template" \
--description "Version 1" \
--compatible-environment-template-major-version-arns arn:aws:proton:us-east-1:${account_id}:environment-template/cycle-store-dev-env-template:1

tar -zcvf svc-template.tar.gz service/ && aws s3 cp svc-template.tar.gz s3://proton-cli-templates-${account_id}/svc-template.tar.gz && rm svc-template.tar.gz

aws proton-preview \
--endpoint-url https://proton.us-east-1.amazonaws.com \
--region us-east-1 \
create-service-template-minor-version \
--template-name "lb-cycle-store-service-template" \
--description "Version 1" \
--major-version-id "1" \
--source-s3-bucket proton-cli-templates-${account_id} \
--source-s3-key svc-template.tar.gz

aws proton-preview \
--endpoint-url https://proton.us-east-1.amazonaws.com \
--region us-east-1 \
wait service-template-registration-complete \
--template-name "lb-cycle-store-service-template" \
--major-version-id "1" \
--minor-version-id "0"


aws proton-preview \
--endpoint-url https://proton.us-east-1.amazonaws.com \
--region us-east-1 \
update-service-template-minor-version \
--template-name "lb-cycle-store-service-template" \
--major-version-id "1" \
--minor-version-id "0" \
--status "PUBLISHED"



