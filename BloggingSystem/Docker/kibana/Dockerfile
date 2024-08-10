FROM docker.elastic.co/kibana/kibana:8.15.0

# Copy certificates and configuration file into the container
COPY Docker/certs /usr/share/kibana/config
COPY Docker/kibana/kibana.yml /usr/share/kibana/config

# Expose the necessary ports
EXPOSE 5601 5601