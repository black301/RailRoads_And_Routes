# Transport System Prototype





## Overview

Transport System Prototype is a comprehensive web application designed to manage and streamline transportation operations. This system provides an intuitive interface for managing vehicles, stations, routes, and trip schedules, making it ideal for bus companies, train operators, and other transportation service providers.

## Features

### Trip Management

- **Create Trips**: Schedule new trips with detailed information
- **Route Visualization**: Interactive visual representation of routes
- **Seat Management**: Track total and available seats for each trip
- **Pricing**: Set and manage ticket prices


### Vehicle Management

- **Vehicle Types**: Support for different vehicle types (buses, trains, etc.)
- **Capacity Tracking**: Monitor vehicle seating capacity
- **Vehicle Assignment**: Assign specific vehicles to trips


### Station Management

- **Station Directory**: Maintain a database of all stations
- **Route Planning**: Create routes between stations
- **Location Tracking**: Track departure and destination points


### User Interface

- **Modern Dashboard**: Clean, responsive interface for administrators
- **Form Validation**: Comprehensive client-side validation
- **Interactive Elements**: Dynamic route visualization and form updates


## Technologies Used

### Backend

- **ASP.NET MVC**: Core framework for the application
- **C#**: Primary backend programming language
- **Entity Framework**: ORM for database operations
- **SQL Server**: Database for storing application data


### Frontend

- **HTML5/CSS3**: Modern markup and styling
- **JavaScript/jQuery**: Client-side interactivity
- **Bootstrap**: Responsive design framework
- **Font Awesome**: Icon library for visual elements


## Installation

### Prerequisites

- Visual Studio 2019 or newer
- .NET Framework 4.7.2 or .NET Core 3.1+
- SQL Server (Express or higher)


### Setup Steps

1. Clone the repository

```plaintext
git clone https://github.com/your-organization/transport-system-prototype.git
```


2. Open the solution file in Visual Studio

```plaintext
Transport_system_prototype.sln
```


3. Restore NuGet packages

```plaintext
Right-click on the solution in Solution Explorer and select "Restore NuGet Packages"
```


4. Update the connection string in `Web.config` or `appsettings.json` to point to your SQL Server instance
5. Run database migrations

```plaintext
Update-Database
```


6. Build and run the application

```plaintext
Press F5 or click the "Start" button in Visual Studio
```




## Usage

### Creating a New Trip

1. Navigate to the Trips section from the main dashboard
2. Click "Create New Trip" button
3. Fill in the required information:

1. Select a vehicle from the dropdown
2. Choose departure and destination stations
3. Set the trip price
4. Specify the number of seats and available seats
5. Select the trip date



4. The route visualization will update dynamically as you select stations
5. Submit the form to create the trip


### Managing Vehicles

1. Navigate to the Vehicles section
2. View all registered vehicles with their details
3. Add new vehicles with specifications
4. Edit or remove existing vehicles


### Station Management

1. Access the Stations section
2. View all stations in the system
3. Add new stations with location details
4. Edit station information as needed


## Project Structure

```plaintext
Transport_system_prototype/
├── Controllers/
│   ├── HomeController.cs
│   ├── TripController.cs
│   ├── VehicleController.cs
│   └── StationController.cs
├── Models/
│   ├── Trip.cs
│   ├── Vehicle.cs
│   └── Station.cs
├── Views/
│   ├── Home/
│   ├── Trip/
│   │   ├── Index.cshtml
│   │   ├── Create.cshtml
│   │   └── Details.cshtml
│   ├── Vehicle/
│   └── Station/
├── Scripts/
│   └── custom.js
├── Content/
│   └── styles.css
└── App_Data/
```

## Best Practices

- **Form Validation**: Always validate user input on both client and server sides
- **Route Planning**: Ensure departure and destination stations are different
- **Seat Management**: Available seats should never exceed the total number of seats
- **Date Selection**: Trip dates should always be in the future

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Acknowledgements

- Font Awesome for the icon library
- Bootstrap team for the responsive framework
- jQuery contributors for the JavaScript library


---

© 2025 Transport System Prototype. All rights reserved.
