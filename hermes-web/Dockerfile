FROM node:lts AS build

ARG SERVER_CONFIG
ARG API_URL
ENV REACT_APP_HERMES_API_URL=${API_URL}

WORKDIR /hermes-ui
COPY package.json .
RUN npm install
COPY . .
RUN npm run build
WORKDIR /
COPY ${SERVER_CONFIG} ./server.conf

FROM nginx:latest
WORKDIR /usr/share/nginx/html
COPY --from=build /hermes-ui/build .
COPY --from=build /server.conf /etc/nginx/conf.d/default.conf
RUN mkdir -p ./.well-known/acme-challenge
VOLUME [ "/etc/letsencrypt" ]
VOLUME [ "/usr/share/nginx/html/.well-known/acme-challenge" ]
EXPOSE 80
EXPOSE 443