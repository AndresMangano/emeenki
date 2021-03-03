#!/bin/bash
for arg in $@
do
    case $arg in
        --push)
            docker build \
                --tag ${HERMES_REGISTRY:-"localhost:5000"}/hermes-worker:latest \
                .
            docker push ${HERMES_REGISTRY:-"localhost:5000"}/hermes-worker:latest
            ;;
        --start)
            docker service create \
                --name hermes-worker \
                --network hermes-network \
                --env ConnectionString=${HERMES_DB_CONNECTION:-"Server=hermes-db;Database=hermes;Uid=root;Pwd=f90ef86b-b326-4188-a08d-af18fc547aa0;"} \
                --env Queue__UserName=${HERMES_QUEUE_USER:-"root"} \
                --env Queue__Password=${HERMES_QUEUE_PASSWORD:-"aa2b0aad-8ae8-4b89-9f59-1505ca4ed9de"} \
                ${HERMES_REGISTRY:-"localhost:5000"}/hermes-worker:latest
            ;;
        --update)
            docker service update \
                --image ${HERMES_REGISTRY:-"localhost:5000"}/hermes-worker:latest \
                hermes-worker
            ;;
        --stop)
            docker service rm hermes-worker
            ;;
        *)
            echo "Invalid command"
            ;;
    esac
done