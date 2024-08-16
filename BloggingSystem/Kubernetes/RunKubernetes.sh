set -x

#docker-compose up -d ceph-mon ceph-mgr

#source ./Docker/ceph/configure.sh

#docker-compose restart ceph-mon ceph-mgr

#docker-compose up -d ceph-osd ceph-rgw

# Exporting keys from dashboard.sh
#source ./Docker/ceph/dashboard.sh

source ./elasticsearch/Run.sh

# Wait for Elasticsearch pod to be ready
until kubectl exec $(kubectl get pod -l app=elasticsearch -o jsonpath='{.items[0].metadata.name}') -- curl -s --cacert /usr/share/elasticsearch/config/bloggingsystem.crt https://localhost:9200 | grep -q "missing authentication credentials"; do
  echo "Waiting for Elasticsearch..."
  sleep 10
done

source ./elasticsearch/configure.sh
source ./kibana/Run.sh

#docker-compose up -d --build

set +x

$SHELL