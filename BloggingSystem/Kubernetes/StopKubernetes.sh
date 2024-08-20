set -x

kubectl delete -f ceph/ceph-mgr-deployment.yaml
kubectl delete -f ceph/ceph-mon-deployment.yaml
kubectl delete -f ceph/ceph-pvc.yaml


set +x