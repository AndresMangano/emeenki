#!/bin/bash
for arg in $@
do
    case $arg in
        push)
            docker build \
                --tag ${HERMES_REGISTRY:-"localhost:5000"}/hermes-web:latest \
                --build-arg SERVER_CONFIG=${HERMES_WEB_CONFIG:-"local.conf"} \
                --build-arg API_URL=${HERMES_API_URL:-"http://127.0.0.1"} \
                .
            docker push ${HERMES_REGISTRY:-"localhost:5000"}/hermes-web:latest
            ;;
        start)
            docker service create \
                --name hermes-web \
                --network hermes-network \
                --publish 80:80 \
                --publish 443:443 \
                --mount type=bind,src=/etc/letsencrypt,dst=/etc/letsencrypt \
                --mount type=bind,src=/var/www/data/.well-known/acme-challenge,dst=/usr/share/nginx/html/.well-known/acme-challenge \
                ${HERMES_REGISTRY:-"localhost:5000"}/hermes-web:latest
            ;;
        update)
            docker service update \
                --image ${HERMES_REGISTRY:-"localhost:5000"}/hermes-web:latest \
                hermes-web
            ;;
        stop)
            docker service rm hermes-web
            ;;
        *)
            echo "Invalid command"
            ;;
    esac
done