set -x

kubectl apply -f ceph-mon-deployment.yaml
kubectl apply -f ceph-mgr-deployment.yaml

source ./ceph/configure.sh

kubectl rollout restart deployment ceph-mon
kubectl rollout restart deployment ceph-mgr

kubectl apply -f ceph-osd-deployment.yaml
kubectl apply -f ceph-rgw-deployment.yaml

# Exporting keys from dashboard.sh
source ./ceph/dashboard.sh

source ./elasticsearch/Run.sh

# Wait for Elasticsearch pod to be ready
until kubectl exec $(kubectl get pod -l app=elasticsearch -o jsonpath='{.items[0].metadata.name}') -- curl -s --cacert /usr/share/elasticsearch/config/bloggingsystem.crt https://localhost:9200 | grep -q "missing authentication credentials"; do
  echo "Waiting for Elasticsearch..."
  sleep 10
done

source ./elasticsearch/configure.sh

source ./kibana/Run.sh
#docker-compose up -d --build

set +x

$SHELL