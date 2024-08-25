set -x

kubectl delete -f bloggingsystem-service.yaml
kubectl delete -f bloggingsystem-deployment.yaml
kubectl delete pods -l app=bloggingsystem

set +x