set -x

# Enable the dashboard module
kubectl exec $CEPH_MON_NAME ceph mgr module enable dashboard

# Set login credentials
kubectl exec $CEPH_MON_NAME ceph dashboard set-login-credentials admin admin

# Configure the dashboard to use the self signed certificate
kubectl exec $CEPH_MON_NAME ceph config set mgr mgr/dashboard/ssl true
kubectl exec $CEPH_MON_NAME ceph dashboard create-self-signed-cert
kubectl exec $CEPH_MON_NAME ceph config set mgr mgr/dashboard/server_addr ceph-mgr
kubectl exec $CEPH_MON_NAME ceph config set mgr mgr/dashboard/server_port 8443

# Create dashboard user
kubectl exec $CEPH_MON_NAME radosgw-admin user create --uid=dashusr --display-name="DashBoard User" --system

# Retrieve access and secret keys
_accesskey=$(kubectl exec $CEPH_MON_NAME radosgw-admin user info --uid dashusr | grep -i access_key | cut -d ":" -f 2 | tr -d '", ')
_secretkey=$(kubectl exec $CEPH_MON_NAME radosgw-admin user info --uid dashusr | grep -i secret_key | cut -d ":" -f 2 | tr -d '", ')

# Set RGW API credentials in the dashboard
kubectl exec $CEPH_MON_NAME ceph dashboard set-rgw-api-access-key $_accesskey
kubectl exec $CEPH_MON_NAME ceph dashboard set-rgw-api-secret-key $_secretkey

# Export keys as environment variables to be used in docker-compose.yml
export ACCESS_KEY="$_accesskey"
export SECRET_KEY="$_secretkey"

# Set pool size and enable rgw application
kubectl exec $CEPH_MON_NAME ceph osd pool set default.rgw.buckets.data size 1
kubectl exec $CEPH_MON_NAME ceph osd pool application enable default.rgw.buckets.data rgw

set +x