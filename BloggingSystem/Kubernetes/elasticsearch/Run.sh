set -x

kubectl apply -f elasticsearch-configmap.yaml
kubectl apply -f elasticsearch-secret.yaml
kubectl apply -f elasticsearch-pv.yaml
kubectl apply -f elasticsearch-pvc.yaml
kubectl apply -f elasticsearch-deployment.yaml
kubectl apply -f elasticsearch-service.yaml

set +