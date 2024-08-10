FROM docker.elastic.co/kibana/kibana:7.17.23

# Copy certificates and configuration file into the container
COPY Docker/certs /usr/share/elasticsearch/config
COPY Docker/kibana/kibana.yml /usr/share/kibana/config

# Expose the necessary ports
EXPOSE 5601 5601