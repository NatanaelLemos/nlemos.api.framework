#!/bin/bash
app="services-default-rabbitmq"
appUser="[[RABBITMQ-USER]]"
appPwd="[[RABBITMQ-PASS]]"

echo ------------------------------------------------------------
echo -------------------------Rabbit MQ--------------------------
echo ------------------------------------------------------------

if docker ps | awk -v app="$app" 'NR > 1 && $NF == app{ret=1; exit} END{exit !ret}'; then
    docker rm -f $app
fi

if docker container ls -a | awk -v app="$app" 'NR > 1 && $NF == app{ret=1; exit} END{exit !ret}'; then
  docker container rm $app -f
fi

docker run -d --hostname $app -p 5672:5672 -e RABBITMQ_DEFAULT_USER=$appUser -e RABBITMQ_DEFAULT_PASS=$appPwd --name $app rabbitmq:3
