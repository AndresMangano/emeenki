#!/bin/bash
case $1 in
start)
    docker service create \
        --name hermes-db \
        --network hermes-network \
        --publish 3306:3306 \
        --mount type=volume,src=db_data,dst=/var/lib/mysql \
        --env MYSQL_DATABASE=hermes \
        --env MYSQL_ROOT_PASSWORD=${HERMES_DB_ROOT_PASSWORD:-"f90ef86b-b326-4188-a08d-af18fc547aa0"} \
        mysql:5.7.29
    ;;
update)
    docker service update hermes-db --force
    ;;
stop)
    docker service rm hermes-db
    ;;
*)
    echo "Invalid command"
    ;;
esac