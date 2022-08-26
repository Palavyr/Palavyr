FROM nginx:stable-alpine

ENV CURRENTENV ""
ENV API_URL ""
ENV WEB_URL ""
ENV WIDGET_URL ""
ENV STRIPE_KEY ""
ENV VERSION ""
ENV LANDING_WIDGET_APIKEY ""
ENV GOOGLE_ANALYTICS_KEY ""
ENV GOOGLE_YOUTUBE_KEY ""
ENV GOOGLE_ANALYTICS_WIDGET_KEY ""
ENV GOOGLE_FONTS_APIKEY ""


WORKDIR /usr/share/nginx/html
RUN rm -rf ./*

WORKDIR /app
COPY ./ui/dist-frontend /usr/share/nginx/html

COPY ./ui/nginx.conf /etc/nginx/conf.d/default.conf
EXPOSE 8090
ENTRYPOINT ["nginx", "-g", "daemon off;"]
