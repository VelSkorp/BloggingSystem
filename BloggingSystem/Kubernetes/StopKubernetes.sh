set -x

cd bloggingsystem
source Stop.sh
cd ..

cd mongodb
source Stop.sh
cd ..

cd redis
source Stop.sh
cd ..

cd kibana
source Stop.sh
cd ..

cd elasticsearch
source Stop.sh
cd ..

cd ceph
source Stop.sh
cd ..

kubectl delete -f certs/bloggingsystem-certs.yaml
kubectl delete secret ceph-credentials

set +x