set -x

kubectl delete -f kibana-deployment.yaml
kubectl delete -f kibana-service.yaml
kubectl delete -f kibana-configmap.yaml
kubectl delete pods -l app=kibana

set +