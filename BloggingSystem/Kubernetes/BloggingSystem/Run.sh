set -x

kubectl apply -f bloggingsystem-deployment.yaml
kubectl apply -f bloggingsystem-service.yaml

set +x