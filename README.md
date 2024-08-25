# Blogging System

The Blogging System is a modern web application for managing blog posts, built using various cutting-edge technologies. This project leverages ASP.NET Core MVC for the web application framework, MongoDB for document storage, Elasticsearch and Kibana for search and analytics, Redis for caching, Ceph S3 for scalable object storage, and Docker Compose for container orchestration.

## Features

- User Authentication & Authorization: Secure login and registration for users.
- Create/Edit/Delete Blog Posts: Full CRUD (Create, Read, Update, Delete) functionality for blog posts.
- View Blog Posts: Display all blog posts, with options to search by author.
- Commenting System: Users can leave comments on blog posts.
- Search Functionality: Fast and efficient search capabilities for blog posts using Elasticsearch.
- Analytics & Monitoring: Real-time monitoring and visualization of application data with Kibana.
- Caching: Improved performance through Redis caching.
- Media Storage: Scalable object storage for media files using Ceph S3.

## Screenshots

### Login page
![LoginPage](Screenshots/LoginPage.png)

### Register page
![RegisterPage](Screenshots/RegisterPage.png)

### Posts main page
![PostsPage](Screenshots/PostsPage.png)

### Create post page
![CreatePostPage](Screenshots/CreatePostPage.png)

## Technologies Used

- ASP.NET Core MVC: Provides the framework for building the web application, handling HTTP requests, and rendering views.
- MongoDB: A NoSQL database used to store blog posts, comments, and users in a flexible, document-oriented format.
- Elasticsearch: Used for search by post author and indexing of blog posts, enabling fast and efficient search capabilities.
- Kibana: Provides visualization and analysis tools for monitoring and exploring data indexed in Elasticsearch.
- Redis: A fast, in-memory data store used for caching user sessions to improve performance.
- Ceph S3: An object storage service compatible with the S3 API, used for storing and retrieving media files, such as images and attachments.
- Docker Compose: Allows you to define and manage multi-container Docker applications, making it easier to set up and run the application stack.
- Kubernetes: Manages containerized applications across a cluster of servers, providing automated deployment, scaling, and management of application containers. It ensures high availability and scalability of the application components.

## [Docker-Compose](BloggingSystem/Docker-Compose/README.md) Installation

## [Kubernetes](BloggingSystem/Kubernetes/README.md) Installation

## Contributing

Contributions are welcome! Please submit issues or pull requests with any improvements, bug fixes, or new features.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.