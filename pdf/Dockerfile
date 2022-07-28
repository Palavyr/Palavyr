FROM public.ecr.aws/lambda/nodejs:12

USER root

EXPOSE 8080

ENV PORT=5603
ENV VERSION=1

COPY . ${LAMBDA_TASK_ROOT}

RUN npm run build

ENTRYPOINT ["lambda.handler"]



# ################

FROM node:14-alpine

ENV PHANTOMJS_VERSION=2.1.1
ENV PHANTOMJS_PATH=/usr/local/bin/phantomjs
RUN apk update && apk add --no-cache fontconfig curl curl-dev && \
    cd /tmp && curl -Ls https://github.com/dustinblackman/phantomized/releases/download/${PHANTOMJS_VERSION}/dockerized-phantomjs.tar.gz | tar xz && \
    cp -R lib lib64 / && \
    cp -R usr/lib/x86_64-linux-gnu /usr/lib && \
    cp -R usr/share /usr/share && \
    cp -R etc/fonts /etc && \
    curl -k -Ls https://bitbucket.org/ariya/phantomjs/downloads/phantomjs-${PHANTOMJS_VERSION}-linux-x86_64.tar.bz2 | tar -jxf - && \
    cp phantomjs-2.1.1-linux-x86_64/bin/phantomjs /usr/local/bin/phantomjs

WORKDIR ${LAMBDA_TASK_ROOT}/temp

COPY . .

RUN npm run build
RUN mv dist ..

WORKDIR ${LAMBDA_TASK_ROOT}
RUN rm -rf temp

EXPOSE 5603

ENV PORT=5603
ENV VERSION=1

ENTRYPOINT ["lambda.handler"]
