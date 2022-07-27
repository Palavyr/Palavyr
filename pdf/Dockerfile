FROM public.ecr.aws/lambda/nodejs:12

USER root

EXPOSE 8080

ENV PORT=5603
ENV VERSION=1

COPY . ${LAMBDA_TASK_ROOT}

RUN npm run build

ENTRYPOINT ["lambda.handler"]