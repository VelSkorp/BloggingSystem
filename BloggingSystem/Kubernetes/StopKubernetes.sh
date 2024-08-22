set -x

kubectl delete -f ceph/ceph-rgw-deployment.yaml
kubectl delete -f ceph/ceph-osd-deployment.yaml
kubectl delete -f ceph/ceph-mgr-deployment.yaml
kubectl delete -f ceph/ceph-mon-deployment.yaml
kubectl delete -f ceph/ceph-pvc.yaml
kubectl delete -f ceph/ceph-configmap.yaml

kubectl delete -f certs/bloggingsystem-certs.yaml


set +x