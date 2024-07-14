set -x

docker-compose up -d ceph-mon ceph-mgr

source ./Docker/ceph/configure.sh

docker-compose restart ceph-mon ceph-mgr

docker-compose up -d ceph-osd ceph-rgw

# Exporting keys from dashboard.sh
source ./Docker/ceph/dashboard.sh

docker-compose up -d

set +x

#$SHELL