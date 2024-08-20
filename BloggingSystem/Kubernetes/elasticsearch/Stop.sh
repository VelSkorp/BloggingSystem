set -x

kubectl delete -f elasticsearch-deployment.yaml
kubectl delete -f elasticsearch-service.yaml
kubectl delete -f elasticsearch-configmap.yaml
kubectl delete -f elasticsearch-pvc.yaml
kubectl delete -f elasticsearch-pv.yaml
kubectl delete -f elasticsearch-secret.yaml
kubectl delete pods -l app=elasticsearch

set +x