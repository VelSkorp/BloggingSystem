set -x

_elasticPassword=$(kubectl get secret elasticsearch-credentials -o jsonpath='{.data.elastic_password}' | base64 --decode)
_kibanaPassword=$(kubectl get secret elasticsearch-credentials -o jsonpath='{.data.kibana_password}' | base64 --decode)

# Set Kibana system password
kubectl exec $(kubectl get pod -l app=elasticsearch -o jsonpath='{.items[0].metadata.name}') -- sh -c "curl -u elastic:${_elasticPassword} -X POST \"https://localhost:9200/_security/user/kibana_system/_password\" -H \"Content-Type: application/json\" -d '{ \"password\" : \"${_kibanaPassword}\" }' --cacert /usr/share/elasticsearch/config/bloggingsystem.crt"

set +x