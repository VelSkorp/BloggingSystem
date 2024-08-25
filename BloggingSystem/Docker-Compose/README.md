# Blogging System

## Docker-Compose Installation

If you prefer to run the application in a Docker containers, follow these steps:

1. Clone the repository:

```bash
git clone https://github.com/VelSkorp/BloggingSystem.git
```

2. Navigate to the project directory:

```bash
cd BloggingSystem
```

3. Run `RunDockerCompose.sh` file to launch the application in docker containers using docker-compose.
3. Run `StopDockerCompose.sh` to delete and clean up the application data.

## Usage

Once the application is running, you can interact with the Blogging System through the web interface.

### Access the Application

- Open your browser and navigate to `http://localhost:8080` or `https://localhost:8081` using SSL connection.
- Register a new account or log in with existing credentials.

### Creating a New Blog Post

- Navigate to the "Create New Post" page from the main menu.
- Fill in the title, content, and upload images.
- Save the post to publish it.

### Commenting on Posts

- View a blog post and leave a comment at the bottom of the post.
- Comments are visible to all users.

### Access the Ceph S3 storage

- Open your browser and navigate to `https://localhost:8443` using SSL connection.
- Use the username `admin` and passwrod `admin` to access the Ceph S3 dashboard.

### Access the Kibana

- Open your browser and navigate to `https://localhost:5601` using SSL connection.
- Use the username `elastic` and passwrod `development` to access the kibana.

### Access the Elasticsearch API

- Use `https://localhost:9200` Url on host machine or `https://elasticsearch:9200` Url inside docker containers in a default docker-compose network.
- Use the username `elastic` and passwrod `development` to access the `elastic` superuser.
- Use the username `kibana_system` and passwrod `development` to access the `kibana_system` user used to connect kibana to elasticsearch.
- It is also possible to access other elasticsearch built-in users by changing the user password in the `/Docker/elasticsearch/configure.sh` file using the `docker-compose exec` command.