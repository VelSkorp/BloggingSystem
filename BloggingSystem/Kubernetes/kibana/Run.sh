set -x

kubectl apply -f kibana-configmap.yaml
kubectl apply -f kibana-deployment.yaml
kubectl apply -f kibana-service.yaml

set +x