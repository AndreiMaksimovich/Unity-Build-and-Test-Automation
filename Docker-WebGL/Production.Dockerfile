FROM nginx:latest

COPY ./Docker-WebGL/src/index.html /usr/share/nginx/html

COPY ./Builds/WebGL/Desktop /usr/share/nginx/html/Desktop
COPY ./Builds/WebGL/Mobile /usr/share/nginx/html/Mobile

RUN chown -R nginx:nginx /usr/share/nginx/html

COPY ./Docker-WebGL/nginx/default.conf.template /etc/nginx/templates/default.conf.template