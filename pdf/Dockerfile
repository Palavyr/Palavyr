FROM public.ecr.aws/lambda/nodejs:12

EXPOSE 8080

ENV PORT=5603
ENV VERSION=1

WORKDIR /usr/src/app

COPY . .
RUN npm run build

CMD ["lambda.handler"]