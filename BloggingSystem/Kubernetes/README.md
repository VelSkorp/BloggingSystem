# Blogging System

## Kubernetes Installation

If you prefer to run the application in a Kubernetes containers, follow these steps:

1. Clone the repository:

```bash
git clone https://github.com/VelSkorp/BloggingSystem.git
```

2. Navigate to the project directory:

```bash
cd BloggingSystem/Kubernetes
```

3. Run `RunKubernetes.sh` file to launch the application in docker containers using docker-compose.
3. Run `StopKubernetes.sh` to delete and clean up the application data.

## Usage

Once the application is running, you can interact with the Blogging System through the web interface.

### Access the Application

- Open your browser and navigate to `http://localhost:30080` or `https://localhost:30081` for SSL connection on the host machine.
- Use the address http://bloggingsystem-service:8080 or https://bloggingsystem-service:8081 for SSL connection inside the k8s network.
- Register a new account or log in with existing credentials.

### Creating a New Blog Post

- Navigate to the "Create New Post" page from the main menu.
- Fill in the title, content, and upload images.
- Save the post to publish it.

### Commenting on Posts

- View a blog post and leave a comment at the bottom of the post.
- Comments are visible to all users.

### Access the Ceph S3 storage dashboard

- Open your browser and navigate to `https://localhost:30084` using SSL connection.
- To access the Cpeh S3 dashboard inside the k8s network, use the address https://ceph-mgr:8443.
- Use the username `admin` and passwrod `admin` to access the Ceph S3 dashboard.

### Access the Ceph S3 storage API

- Use the address http://localhost:30480 or https://localhost:30481 for SSL connection on the host machine.
- Use the address http://ceph-rgw:7480 or https://ceph-rgw:7481 for SSL connection inside the k8s network.

### Access the Kibana

- Open your browser and navigate to `https://localhost:30056` using SSL connection.
- To access the Kibana dashboard inside the k8s network, use the address https://kibana-service:5601.
- Use the username `elastic` and passwrod `development` to access the kibana.

### Access the Elasticsearch API

- Use `https://localhost:30092` Url on host machine or `https://elasticsearch:9200` Url inside k8s network.
- Use the username `elastic` and passwrod `development` to access the `elastic` superuser.
- Use the username `kibana_system` and passwrod `development` to access the `kibana_system` user used to connect kibana to elasticsearch.
- It is also possible to access other elasticsearch built-in users by changing the user password in the `/elasticsearch/Run.sh` file using the `kubectl exec` command.