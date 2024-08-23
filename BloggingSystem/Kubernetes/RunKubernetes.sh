set -x

kubectl apply -f certs/bloggingsystem-certs.yaml

cd ceph
source Run.sh
cd ..

cd elasticsearch
source Run.sh
cd ..

cd kibana
source Run.sh
cd ..

cd mongodb
source Run.sh
cd ..

cd redis
source Run.sh
cd ..

cd bloggingsystem
source Run.sh
cd ..

set +x

#$SHELL