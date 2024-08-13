set -x

docker-compose up -d ceph-mon ceph-mgr

source ./Docker/ceph/configure.sh

docker-compose restart ceph-mon ceph-mgr

docker-compose up -d ceph-osd ceph-rgw

# Exporting keys from dashboard.sh
source ./Docker/ceph/dashboard.sh

docker-compose up -d --build elasticsearch

# Wait for Elasticsearch to be available
until docker-compose exec elasticsearch sh -c "until curl -s --cacert /usr/share/elasticsearch/config/bloggingsystem.crt https://localhost:9200 | grep -q \"missing authentication credentials\"; do sleep 30; done;" > /dev/null 2>&1; do
  echo "Waiting for Elasticsearch..."
  sleep 10
done

source ./Docker/elasticsearch/configure.sh

docker-compose up -d --build

set +x

#$SHELL