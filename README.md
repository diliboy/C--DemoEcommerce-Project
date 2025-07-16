# 🛒 Demo eCommerce Microservices Backend in .NET 8

This is a **backend-only eCommerce project** built using **C# and ASP.NET Core (.NET 8)** following the **Clean Architecture** and **Microservices** principles. The goal of this project is to learn and demonstrate a real-world scalable architecture in .NET using best practices.

---
#### This project was created as part of my journey to learn C# and .NET, with a help of tutorial by exploring microservices with clean architecture and build a strong foundation for enterprise-level backend development.
---

## 🏗️ Project Structure

This repository includes the following microservices:

| Service | Description |
|--------|-------------|
| **Authentication API** | Handles user registration, login, and JWT-based authentication |
| **Product API**        | Manages product catalog with role-based authorization (Admin only for add/edit/delete) |
| **Order API**          | Allows users to place orders, view details, and manage orders |
| **API Gateway (Ocelot)** | Centralized entry point for routing requests to respective microservices |
| **Shared Library**     | Contains reusable components such as logging, database context, and common response models |

All services are independent but communicate via **HTTP REST APIs**, and all requests are routed through the **API Gateway**.

---

## 🚀 Technologies Used

- **.NET 8** (ASP.NET Core Web API)
- **Clean Architecture** (Presentation, Application, Domain, Infrastructure layers)
- **Entity Framework Core**
- **JWT Authentication**
- **Ocelot API Gateway**
- **Postman** for API testing
- **BCrypt** for password hashing
- **Serilog** for structured logging

---

## 🔐 Authentication and Authorization

- **JWT tokens** are issued on login and used for secured API access.
- **Role-based authorization**:
  - Only users with **Admin role** can create, update, or delete products.
  - All authenticated users can place orders and view their own order details.

---

## ✅ Features

### 🔑 Authentication Service
- Register new users
- Login and receive JWT tokens
- Secure API access with roles

### 📦 Product Service
- Get all products
- Get product by ID
- Add, update, and delete products (Admins only)

### 🧾 Order Service
- Create new orders
- Fetch order details by ID
- Get orders by client
- Update or delete orders

### 🌐 API Gateway
- All requests pass through Ocelot gateway (`https://localhost:5003`)
- Handles routing to respective microservices
- Supports rate limiting, retry policies, and centralized control

---

## 📂 Clean Architecture Overview

Each microservice follows Clean Architecture:

- Domain # Business entities and interfaces
- Application # DTOs, interfaces, services, policies
- Infrastructure # EF Core, logging, data access, config
- Presentation # ASP.NET Core Web API (controllers, startup)

---

## 📬 API Testing

Use **Postman** to test the endpoints. JWT tokens must be included in the `Authorization` header for protected endpoints.

Example:
```
Authorization: Bearer <your-jwt-token>
```

---

## 🛠️ Getting Started

1. Clone the repo:
   ```
   git clone https://github.com/<your-username>/<repo-name>.git
   cd <repo-name>
   ```
2. Ensure you have .NET 8 SDK and SQL Server installed

3. Update your appsettings.json files with appropriate connection strings

4. Run EF migrations (per microservice):

  ```
  add-migration Initial -o Data/Migrations
  update-database
  ```
5. Run each project (API Gateway, Product, Order, Auth) independently via Visual Studio or CLI

6. Use Postman to test through.
  ```
  https://localhost:5003
  ```
---

🔭 Future Improvements
- Add unit and integration tests

- Implement service discovery and load balancing

- Add frontend using React/Blazor

- Add RabbitMQ or Kafka for async event-driven communication
