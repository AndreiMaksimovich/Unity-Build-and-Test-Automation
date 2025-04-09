FROM nginx:latest

COPY ./Docker-WebGL/src/index.html /usr/share/nginx/html/index.html

RUN chown -R nginx:nginx /usr/share/nginx/html

COPY ./Docker-WebGL/nginx/default.conf.template /etc/nginx/templates/default.conf.template