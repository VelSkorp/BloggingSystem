set -x

# Set password for kibana_system user
docker-compose exec elasticsearch sh -c "curl -u elastic:${ELASTIC_PASSWORD} -X POST \"https://localhost:9200/_security/user/kibana_system/_password\" -H \"Content-Type: application/json\" -d '{ \"password\" : \"${KIBANA_PASSWORD}\" }' --cacert /usr/share/elasticsearch/config/bloggingsystem.crt"

set +x