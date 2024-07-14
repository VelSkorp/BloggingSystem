set -x

docker-compose down
docker volume rm $(docker volume ls)

set +x