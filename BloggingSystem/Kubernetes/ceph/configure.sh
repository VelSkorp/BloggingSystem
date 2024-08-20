set -x

_cephMonName=$(kubectl get pod -l app=ceph-mon -o jsonpath='{.items[0].metadata.name}')

# For one copy, size 1
kubectl exec $_cephMonName -- sh -c "echo 'osd pool default min size = 1' | tee -a /etc/ceph/ceph.conf"
kubectl exec $_cephMonName -- sh -c "echo 'osd pool default size = 1' | tee -a /etc/ceph/ceph.conf"
kubectl exec $_cephMonName -- sh -c "echo 'max open files = 655350' | tee -a  /etc/ceph/ceph.conf"
kubectl exec $_cephMonName -- sh -c "echo 'cephx cluster require signatures = false' | tee -a /etc/ceph/ceph.conf"
kubectl exec $_cephMonName -- sh -c "echo 'cephx service require signatures = false' | tee -a /etc/ceph/ceph.conf"
kubectl exec $_cephMonName -- sh -c "echo 'osd max object namespace len = 64' | tee -a  /etc/ceph/ceph.conf"

# Configure rgw for using https
kubectl exec $_cephMonName -- sh -c "echo 'rgw_frontends = beast port=7480 ssl_port=7481 ssl_certificate=/certs/bloggingsystem.crt ssl_private_key=/certs/bloggingsystem.key' | tee -a  /etc/ceph/ceph.conf"
kubectl exec $_cephMonName -- sh -c "echo 'rgw_verify_ssl = false' | tee -a  /etc/ceph/ceph.conf"

# Must be for osd run
kubectl exec $_cephMonName -- sh -c "echo 'osd max object name len = 256' | tee -a /etc/ceph/ceph.conf"

# Must be for non health warning
kubectl exec $_cephMonName ceph osd pool create default.rgw.buckets.data 128 128

export CEPH_MON_NAME="$_cephMonName"

set +x