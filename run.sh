#!/bin/bash
case $1 in
start)
    docker swarm init --advertise-addr ${HERMES_SWARM:-"127.0.0.1"}
    docker service create \
        --name registry \
        --publish 5000:5000 \
        registry:2
    docker network create \
        --driver overlay \
        hermes-network
    docker volume create db_data
    docker volume create queue_data
    ;;
stop)
    docker swarm leave --force