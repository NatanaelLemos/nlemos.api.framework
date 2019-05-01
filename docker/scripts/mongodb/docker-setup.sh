#!/bin/bash
app="mongo-default"

if docker ps | awk -v app="$app" 'NR > 1 && $NF == app{ret=1; exit} END{exit !ret}'; then
    docker rm -f $app
fi

if docker container ls -a | awk -v app="$app" 'NR > 1 && $NF == app{ret=1; exit} END{exit !ret}'; then
  docker container rm $app -f
fi

docker build -t $app .

docker run -dP \
-e MONGODB_ADMIN_USER=[[ADMIN_USER]] \
-e MONGODB_ADMIN_PASS=[[ADMIN_PASS]] \
-e MONGODB_APPLICATION_DATABASE=admin \
-e MONGODB_APPLICATION_USER=[[APPLICATION_USER]] \
-e MONGODB_APPLICATION_PASS=[[APPLICATION_USER_PASS]] \
-v $app:/data/db \
-p 27017:27017 \
--name $app $app
