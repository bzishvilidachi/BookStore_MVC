# ASP.NET Core (.NET 8) Project

## Overview
This project is a comprehensive ASP.NET Core (.NET 8) web application built using **MVC** and **Razor Pages**. The project integrates various important features such as **Identity Framework**, **Entity Framework**, **Stripe Payment Integration**, and more.

## Table of Contents
- [Project Structure](#project-structure)
- [Technologies Used](#technologies-used)
- [Features](#features)
- [Setup Instructions](#setup-instructions)
- [Database Setup](#database-setup)
- [Authentication and Authorization](#authentication-and-authorization)
- [Stripe Payment Integration](#stripe-payment-integration)
- [Deploying to Azure](#deploying-to-azure)
- [Contributing](#contributing)
- [License](#license)

## Project Structure

- **Controllers**: Handles the HTTP requests and responses.
- **Models**: Contains the application’s data models.
- **Views**: Contains the Razor Views for rendering the UI.
- **wwwroot**: Static files like CSS, JavaScript, and images.
- **Data**: Contains classes for interacting with the database and applying migrations.
- **Services**: Contains logic for handling business operations like payment integration and email notifications.
- **TagHelpers**: Custom tag helpers for dynamic rendering in Razor Views.

## Technologies Used

- **ASP.NET Core 8**
- **Entity Framework Core**
- **Identity Framework** (for authentication and role management)
- **Stripe** (for payment integration)
- **Bootstrap 5**
- **Microsoft Azure** (for deployment)

## Features

- **ASP.NET Core MVC & Razor Pages**: MVC structure with dynamic views.
- **User Authentication**: Integrated with ASP.NET Core Identity to manage user registration, login, and role-based access.
- **Role Management**: Assign roles to users for controlling access to specific parts of the application.
- **Entity Framework**: Database operations with code-first migrations and automatic seeding.
- **Stripe Payment Integration**: Users can make payments through Stripe.
- **Custom Tag Helpers**: Use of custom tag helpers for easier dynamic content rendering.
- **Partial Views & View Components**: Organized views for better code management and reuse.
- **Email Notifications**: Sends email notifications to users (can be customized).
- **TempData**: Store temporary data between requests.
- **Session Management**: Session handling in ASP.NET Core for storing user data temporarily.
  
## Setup Instructions

1. Clone this repository.
    ```bash
    git clone <repository_url>
    ```

2. Navigate to the project directory:
    ```bash
    cd <project_name>
    ```

3. Restore the project dependencies:
    ```bash
    dotnet restore
    ```

4. Apply migrations to the database:
    ```bash
    dotnet ef database update
    ```

5. Run the project:
    ```bash
    dotnet run
    ```

6. Open the browser and navigate to `https://localhost:5001` (or the default port set).

## Database Setup

- The database is set up using **Entity Framework Core** with code-first migrations.
- Migrations are automatically applied on startup, but you can run them manually using:
    ```bash
    dotnet ef migrations add <migration_name>
    dotnet ef database update
    ```

## Authentication and Authorization

- The app uses **ASP.NET Core Identity** for user management.
- Users can register, log in, and access certain pages based on their roles.
- The system supports role-based authorization to restrict access to specific pages or actions.

  

## Admin Access

To access the main functionalities of the project, you need to log in as an admin. The admin account is seeded automatically during the migration process. Use the following default credentials to log in:

- **Username:** `admin@bookstore.com`
- **Password:** `Admin123*`

If you prefer creating your own admin account, follow these steps:
1. Run the application and create a new user.
2. Update the user in the database to have an admin role (you can use the SQL scripts provided in the `Database` folder).
   

## Stripe Payment Integration

1. Set up a Stripe account and get your API keys.
2. Add the keys to the `appsettings.json` file:
    ```json
    "Stripe": {
      "PublishableKey": "<your_publishable_key>",
      "SecretKey": "<your_secret_key>"
    }
    ```
3. Implement payment functionality by integrating Stripe into the checkout flow.


## Contributing

Feel free to fork this repository, submit issues, or create pull requests. Please follow the standard GitHub workflows.






