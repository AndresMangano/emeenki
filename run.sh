#!/bin/bash
for arg in $@
do
    case $1 in
        swarm)
            docker swarm init --advertise-addr ${HERMES_SWARM:-"127.0.0.1"}
            docker service create \
                --name registry \
                --publish 5000:5000 \
                registry:2
            ;;
        install)
            docker network create \
                --driver overlay \
                hermes-network
            docker volume create db_data
            docker volume create queue_data
            ;;
        start)
            sh ./hermes-db/run.sh start
            sh ./hermes-queue/run.sh start
            sh ./hermes-worker/run.sh push start
            sh ./hermes-api/run.sh push start
            sh ./hermes-web/run.sh push start
            ;;
        stop)
            docker service rm hermes-web
            docker service rm hermes-api
            docker service rm hermes-worker
            docker service rm hermes-queue
            docker service rm hermes-db
            ;;
        *)
            echo "Invalid command"
            ;;
    esac
done