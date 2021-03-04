#!/bin/bash
for arg in $@
do
    case $arg in
        --start)
            docker service create \
                --name hermes-queue \
                --network hermes-network \
                --publish 15672:15672 \
                --mount type=volume,src=queue_data,dst=/var/lib/rabbitmq \
                --env RABBITMQ_DEFAULT_USER=${HERMES_QUEUE_USER:-"root"} \
                --env RABBITMQ_DEFAULT_PASS=${HERMES_QUEUE_PASSWORD:-"aa2b0aad-8ae8-4b89-9f59-1505ca4ed9de"} \
                rabbitmq:3.8.11-management
            ;;
        --update)
            docker service update hermes-queue --force
            ;;
        --stop)
            docker service rm hermes-queue
            ;;
        *)
            echo "Invalid command"
            ;;
    esac
done