set -x

kubectl delete -f mongodb-deployment.yaml
kubectl delete -f mongodb-service.yaml
kubectl delete -f mongodb-configmap.yaml
kubectl delete -f mongodb-pvc.yaml
kubectl delete -f mongodb-pv.yaml
kubectl delete pods -l app=mongodb

set +