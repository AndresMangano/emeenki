#!/bin/bash
for arg in $@
do
    case $arg in
        push)
            docker build \
                --tag ${HERMES_REGISTRY:-localhost:5000}/hermes-api:latest \
                .
            docker push ${HERMES_REGISTRY:-localhost:5000}/hermes-api:latest
            ;;
        start)
            docker service create \
                --name hermes-api \
                --network hermes-network \
                --publish 8080:8080 \
                --env SQLConnection=${HERMES_DB_CONNECTION:-"Server=hermes-db;Database=hermes;Uid=root;Pwd=f90ef86b-b326-4188-a08d-af18fc547aa0;"} \
                --env Authentication__Secret=${HERMES_API_AUTH_SECRET:-"a549bb4d-2353-4a2a-ac3a-077773aa3336"} \
                --env UIServer=${HERMES_WEB_URL:-"http://127.0.0.1"} \
                --env Queue__UserName=${HERMES_QUEUE_USER:-"root"} \
                --env Queue__Password=${HERMES_QUEUE_PASSWORD:-"aa2b0aad-8ae8-4b89-9f59-1505ca4ed9de"} \
                ${HERMES_REGISTRY:-localhost:5000}/hermes-api:latest
            ;;
        update)
            docker service update \
                --image ${HERMES_REGISTRY:-localhost:5000}/hermes-api:latest \
                hermes-api
            ;;
        stop)
            docker service rm hermes-api
            ;;
        *)
            echo "Invalid command"
            ;;
    esac
done