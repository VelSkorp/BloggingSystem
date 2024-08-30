set -x

kubectl delete -f ceph-rgw-deployment.yaml
kubectl delete -f ceph-osd-deployment.yaml
kubectl delete -f ceph-mgr-deployment.yaml
kubectl delete -f ceph-mon-statefulset.yaml
kubectl delete -f ceph-configmap.yaml
kubectl delete -f ceph-pvc.yaml
kubectl delete pods -l app=ceph-rgw
kubectl delete pods -l app=ceph-osd
kubectl delete pods -l app=ceph-mgr
kubectl delete pods -l app=ceph-mon

set +x