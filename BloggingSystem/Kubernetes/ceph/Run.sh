set -x

kubectl apply -f ceph-pvc.yaml
kubectl apply -f ceph-configmap.yaml
kubectl apply -f ceph-mon-deployment.yaml

until kubectl get pods -l app=ceph-mon -o jsonpath="{.items[0].status.containerStatuses[0].ready}" | grep -q true; do
  echo "Waiting for ceph-mon to be ready..."
  sleep 5
done

kubectl exec ceph-mon-0 -- ceph osd pool create default.rgw.buckets.data 128 128

kubectl apply -f ceph-mgr-deployment.yaml
kubectl apply -f ceph-osd-deployment.yaml
kubectl apply -f ceph-rgw-deployment.yaml

until kubectl get pods -l app=ceph-mgr -o jsonpath="{.items[0].status.containerStatuses[0].ready}" | grep -q true &&
      kubectl get pods -l app=ceph-osd -o jsonpath="{.items[0].status.containerStatuses[0].ready}" | grep -q true &&
      kubectl get pods -l app=ceph-rgw -o jsonpath="{.items[0].status.containerStatuses[0].ready}" | grep -q true; do
  echo "Waiting for ceph-mgr, ceph-osd and ceph-rgw to be ready..."
  sleep 5
done

# Exporting keys from dashboard.sh
source dashboard.sh

set +x