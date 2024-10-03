# .NET SQL ORM Benchmarks

This project benchmarks various SQL Object-Relational Mappers (ORMs) in .NET to evaluate their performance and efficiency.

## Table of Contents
- [Introduction](#introduction)
- [Getting Started](#getting-started)
- [Benchmarked ORMs](#benchmarked-orms)
- [Running the Benchmarks](#running-the-benchmarks)
- [Results](#results)
- [Contributing](#contributing)
- [License](#license)

## Introduction
The goal of this project is to provide a comprehensive comparison of different .NET SQL ORMs. By running standardized benchmarks, we aim to help developers choose the most suitable ORM for their needs.

## Getting Started
To get started with running the benchmarks, clone the repository and follow the instructions below.

### Prerequisites
- .NET SDK

### Installation
1. Clone the repository:
    ```sh
    git clone https://github.com/alalbers77/dotnet-sql-orm-benchmarks.git
    cd dotnet-sql-orm-benchmarks
    ```
2. Restore dependencies:
    ```sh
    dotnet restore
    ```

## Benchmarked ORMs
The following ORMs are included in the benchmarks:
- Entity Framework Core
- Dapper

## Running the Benchmarks
To run the benchmarks, execute the following command:
```sh
dotnet run -c Release
```

## Results
Results will be displayed in the console after the benchmarks complete. Detailed results can be found in the `BenchmarkDotNet.Artifacts` directory.

## Contributing
Contributions are welcome! Please open an issue or submit a pull request.

## License
This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.
