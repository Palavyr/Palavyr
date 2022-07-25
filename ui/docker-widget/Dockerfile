FROM nginx:stable-alpine

WORKDIR /usr/share/nginx/html
RUN rm -rf ./*

WORKDIR /app
COPY ./ui/dist-widget /usr/share/nginx/html

COPY ./ui/nginx.conf /etc/nginx/conf.d/default.conf
EXPOSE 80
ENTRYPOINT ["nginx", "-g", "daemon off;"]
