set -x

# Enable the dashboard module
docker-compose exec ceph-mon ceph mgr module enable dashboard

# Set login credentials
docker-compose exec ceph-mon ceph dashboard set-login-credentials admin admin

# Configure the dashboard to use the self signed certificate
docker-compose exec ceph-mon ceph config set mgr mgr/dashboard/ssl true
docker-compose exec ceph-mon ceph dashboard create-self-signed-cert
docker-compose exec ceph-mon ceph config set mgr mgr/dashboard/server_addr ceph-mgr
docker-compose exec ceph-mon ceph config set mgr mgr/dashboard/server_port 8443

# Create dashboard user
docker-compose exec ceph-mon radosgw-admin user create --uid=dashusr --display-name="DashBoard User" --system

# Retrieve access and secret keys
_accesskey=$(docker-compose exec ceph-mon radosgw-admin user info --uid dashusr | grep -i access_key | cut -d ":" -f 2 | tr -d '", ')
_secretkey=$(docker-compose exec ceph-mon radosgw-admin user info --uid dashusr | grep -i secret_key | cut -d ":" -f 2 | tr -d '", ')

# Set RGW API credentials in the dashboard
docker-compose exec ceph-mon ceph dashboard set-rgw-api-access-key $_accesskey
docker-compose exec ceph-mon ceph dashboard set-rgw-api-secret-key $_secretkey

# Export keys as environment variables to be used in docker-compose.yml
export ACCESS_KEY="$_accesskey"
export SECRET_KEY="$_secretkey"

# Set pool size and enable rgw application
docker exec ceph-mon ceph osd pool set default.rgw.buckets.data size 1
docker exec ceph-mon ceph osd pool application enable default.rgw.buckets.data rgw

set +x