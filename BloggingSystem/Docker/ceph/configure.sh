set -x

# For one copy, size 1
docker-compose exec ceph-mon sh -c "echo 'osd pool default min size = 1' | tee -a /etc/ceph/ceph.conf"
docker-compose exec ceph-mon sh -c "echo 'osd pool default size = 1' | tee -a /etc/ceph/ceph.conf"
docker-compose exec ceph-mon sh -c "echo 'max open files = 655350' | tee -a  /etc/ceph/ceph.conf"
docker-compose exec ceph-mon sh -c "echo 'cephx cluster require signatures = false' | tee -a /etc/ceph/ceph.conf"
docker-compose exec ceph-mon sh -c "echo 'cephx service require signatures = false' | tee -a /etc/ceph/ceph.conf"
docker-compose exec ceph-mon sh -c "echo 'osd max object namespace len = 64' | tee -a  /etc/ceph/ceph.conf"

# Configure rgw for using https
docker-compose exec ceph-mon sh -c "echo 'rgw_frontends = beast ssl_port=7481 ssl_certificate=/certs/bloggingsystem.crt ssl_private_key=/certs/bloggingsystem.key' | tee -a  /etc/ceph/ceph.conf"

# Must be for osd run
docker-compose exec ceph-mon sh -c "echo 'osd max object name len = 256' | tee -a /etc/ceph/ceph.conf"

# Must be for non health warning
docker-compose exec ceph-mon ceph osd pool create default.rgw.buckets.data 128 128
set +x