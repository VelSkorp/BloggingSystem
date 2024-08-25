set -x

kubectl delete -f redis-deployment.yaml
kubectl delete -f redis-service.yaml
kubectl delete -f redis-pvc.yaml
kubectl delete pods -l app=redis

set +