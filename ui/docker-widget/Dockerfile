FROM nginx:stable-alpine


ENV API_URL ""
ENV GOOGLE_ANALYTICS_KEY ""
ENV CURRENTENV ""
ENV GOOGLE_ANALYTICS_WIDGET_KEY ""
ENV GOOGLE_FONTS_APIKEY ""


WORKDIR /usr/share/nginx/html
RUN rm -rf ./*

WORKDIR /app
COPY ./ui/dist-widget /usr/share/nginx/html

COPY ./ui/nginx.conf /etc/nginx/conf.d/default.conf
EXPOSE 80
ENTRYPOINT ["nginx", "-g", "daemon off;"]
