FROM docker.elastic.co/elasticsearch/elasticsearch:8.15.0

# Copy certificates and configuration file into the container
COPY Docker/certs /usr/share/elasticsearch/config
COPY Docker/elasticsearch/elasticsearch.yml /usr/share/elasticsearch/config

# Create the Elasticsearch keystore and add secure settings
RUN /usr/share/elasticsearch/bin/elasticsearch-keystore create
RUN echo "development" | /usr/share/elasticsearch/bin/elasticsearch-keystore add --stdin xpack.security.transport.ssl.truststore.secure_password
RUN echo "development" | /usr/share/elasticsearch/bin/elasticsearch-keystore add --stdin xpack.security.transport.ssl.keystore.secure_password

# Expose the necessary ports
EXPOSE 9200 9200