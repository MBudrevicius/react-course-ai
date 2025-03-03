# React-Course-AI Backend

This is the backend service for the **React-Course-AI** project. It is built using **.NET 9** and serves as the API layer for the frontend application.

## Prerequisites
Ensure you have the following installed before running the backend:
- [.NET 9 SDK](https://dotnet.microsoft.com/)

## Setup & Running the Project
To set up and run the backend, follow these steps. All commands should be run inside the `react-course-ai/back-end/` folder.

### 1. Configure Secrets
Copy the necessary secrets into `react-course-ai/back-end/appsettings.json`. You can retrieve them from Discord.

### 2. Install Dependencies
Run the following command to install the required NuGet packages:
```sh
 dotnet restore
```

### 3. Start the Server
Run the application using:
```sh
 dotnet run
```

### 4. Access the API
Once the server is running, the backend API will be available at the localhost port specified in your console output. For example:
```
http://localhost:5255
```

### 5. (Optional) Use Swagger UI
To explore and test API endpoints, open Swagger UI in your browser:
```
http://localhost:{your-port}/swagger
```
Replace `{your-port}` with the actual port displayed in your console.

---

For any questions or support, reach out on Discord.

