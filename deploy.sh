#!/bin/bash
cd hermes-api
docker build -t localhost:5000/hermes-api:latest .
docker push localhost:5000/hermes-api:latest
cd ../hermes-web
docker build -t localhost:5000/hermes-web:latest .
docker push localhost:5000/hermes-web:latest
cd ../hermes-worker
docker build -t localhost:5000/hermes-worker:latest .
docker push localhost:5000/hermes-worker:latest
cd ..
docker stack deploy -c docker-compose.yaml -c docker-compose.develop.yaml hermes