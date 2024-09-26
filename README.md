# Stock Forecasting Solution

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT) ![Environment: Windows](https://img.shields.io/badge/Environment-Windows-blue)  ![IDE: VisualStudio](https://img.shields.io/badge/IDE-VisualStudio-purple) 

This solution consists of multiple projects aimed at forecasting stock prices using machine learning models. The solution is built using C# and targets .NET 8.0.

## Projects

### 1. StockForecasting

This project is a Windows Forms application that provides a graphical user interface for stock forecasting.

#### Key Files:
- `Program.cs`: The main entry point for the application. It subscribes to log messages and initializes the application.
- `StockForecasting.csproj`: The project file that includes dependencies and project settings.

#### Dependencies:
- `Syncfusion.Chart.Windows` (Version 26.2.14)

### 2. StockForecastProject

This project is a console application that handles the data processing and machine learning model training for stock forecasting.

#### Key Files:
- `Program.cs`: The main entry point for the console application. It handles data loading, preprocessing, and model training.
- `StockForecastProject.csproj`: The project file that includes dependencies and project settings.

### 3. ClassLibrary

This project contains shared classes and methods used by both the `StockForecasting` and `StockForecastProject` projects.

#### Key Files:
- `ClassLibrary.csproj`: The project file that includes dependencies and project settings.

#### Dependencies:
- `Dapper` (Version 2.1.35)
- `Microsoft.Data.SqlClient` (Version 5.2.2)
- `Microsoft.ML.TimeSeries` (Version 3.0.1)

## Getting Started

### Prerequisites

- .NET 8.0 SDK
- Visual Studio 2022 or later

### Building the Solution

1. Clone the repository.<br>`git clone https://github.com/Giuseppe1343/StockForecastProject.git
cd <repository-directory>`
2. Open the solution in Visual Studio: <br>`start StockForecasting.sln`
3. Restore the NuGet packages: <br>`dotnet restore`
4. Build the solution: <br>`dotnet build`
## Usage


### Windows Forms Application

The Windows Forms application provides a graphical interface for visualizing stock forecasts. It subscribes to log messages and displays them in message boxes based on their log level (Info, Error, Warning).

### Console Application

The console application handles the data processing and machine learning model training. It subscribes to log messages and outputs them to the console. The application continuously processes stock data, trains models, and evaluates their performance.

## Acknowledgements

- [Syncfusion](https://www.syncfusion.com/) for providing the charting library.
- [Dapper](https://github.com/DapperLib/Dapper) for the micro ORM.
- [Microsoft.ML.TimeSeries](https://www.nuget.org/packages/Microsoft.ML.TimeSeries/) for the time series forecasting library.

