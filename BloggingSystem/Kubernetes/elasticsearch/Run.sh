set -x

kubectl apply -f elasticsearch-configmap.yaml
kubectl apply -f elasticsearch-secret.yaml
kubectl apply -f elasticsearch-pv.yaml
kubectl apply -f elasticsearch-pvc.yaml
kubectl apply -f elasticsearch-deployment.yaml
kubectl apply -f elasticsearch-service.yaml

# Wait for Elasticsearch pod to be ready
#until kubectl exec $(kubectl get pod -l app=elasticsearch -o jsonpath='{.items[0].metadata.name}') -- curl -s --cacert /usr/share/elasticsearch/config/bloggingsystem.crt https://localhost:9200 | grep -q "missing authentication credentials"; do
until curl -k -s --head --request GET https://elasticsearch:9200 | grep "200 OK" > /dev/null; do 
  echo "Waiting for Elasticsearch..."
  sleep 10
done

_elasticPassword=$(kubectl get secret elasticsearch-credentials -o jsonpath='{.data.elastic_password}' | base64 --decode)
_kibanaPassword=$(kubectl get secret elasticsearch-credentials -o jsonpath='{.data.kibana_password}' | base64 --decode)

# Set Kibana system password
kubectl exec $(kubectl get pod -l app=elasticsearch -o jsonpath='{.items[0].metadata.name}') -- sh -c "curl -u elastic:${_elasticPassword} -X POST \"https://localhost:9200/_security/user/kibana_system/_password\" -H \"Content-Type: application/json\" -d '{ \"password\" : \"${_kibanaPassword}\" }' --cacert /usr/share/elasticsearch/config/bloggingsystem.crt"

set +x