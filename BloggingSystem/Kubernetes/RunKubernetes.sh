set -x

kubectl apply -f certs/bloggingsystem-certs.yaml

kubectl apply -f ceph/ceph-pvc.yaml
kubectl apply -f ceph/ceph-configmap.yaml
kubectl apply -f ceph/ceph-mon-deployment.yaml

sleep 60

kubectl exec ceph-mon-0 -- ceph osd pool create default.rgw.buckets.data 128 128

kubectl apply -f ceph/ceph-mgr-deployment.yaml
kubectl apply -f ceph/ceph-osd-deployment.yaml
kubectl apply -f ceph/ceph-rgw-deployment.yaml

sleep 60

source ceph/dashboard.sh

# kubectl rollout restart deployment ceph-mon
# kubectl rollout restart deployment ceph-mgr
# 
# kubectl apply -f ceph-osd-deployment.yaml
# kubectl apply -f ceph-rgw-deployment.yaml
# 
# # Exporting keys from dashboard.sh
# source ./ceph/dashboard.sh
# 
# source ./elasticsearch/Run.sh
# 
# # Wait for Elasticsearch pod to be ready
# until kubectl exec $(kubectl get pod -l app=elasticsearch -o jsonpath='{.items[0].metadata.name}') -- curl -s --cacert /usr/share/elasticsearch/config/bloggingsystem.crt https://localhost:9200 | grep -q "missing authentication credentials"; do
#   echo "Waiting for Elasticsearch..."
#   sleep 10
# done
# 
# source ./elasticsearch/configure.sh
# 
# source ./kibana/Run.sh
# #docker-compose up -d --build

set +x

$SHELL