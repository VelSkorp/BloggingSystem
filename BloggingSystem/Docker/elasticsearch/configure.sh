set -x

# Set password for kibana_system user
echo "Setting kibana_system password";
docker-compose exec elasticsearch sh -c "curl -u elastic:development -X POST \"https://localhost:9200/_security/user/kibana_system/_password\" -H \"Content-Type: application/json\" -d '{ \"password\" : \"development\" }' --cacert /usr/share/elasticsearch/config/bloggingsystem.crt"

set +x