# Driving License Management Department (DVLD)

## Project Overview
Driving License Management Department (DVLD) is a comprehensive Windows forms application designed to streamline and automate the processes related to driver's license services. The system manages license issuance, renewal, testing, and administrative tasks for various vehicle license categories, ensuring efficient and transparent license management.

---

## Key Features
- **License Management**:
  - Support for multiple license classes (motorcycles, cars, commercial vehicles, etc.).
  - Complete license application workflow.
- **Testing and Exams**:
  - Medical and theoretical exam management.
  - Practical driving test scheduling.
- **License Services**:
  - License renewal and replacement services.
- **User Management**:
  - Detailed user and person management.
- **Administrative Controls**:
  - System management tools for administrators.

---

## Technical Specifications
- **Programming Language**: C#
- **Database**: SQL Server
- **Frontend**: Windows Forms
- **Data Access**: ADO.NET
- **Architecture**: 3-Tier Architecture
  
---

## Recent Improvements
This section showcases my growth as a developer and documents key improvements made to the project, along with the dates they were implemented.

- **Single Responsibility Principle (SRP)**:
  - Refactored the **People feature** into smaller, single-responsibility classes:
    - `PersonSqlStatements`, `PersonParameterBuilder`, and `PersonDataMapper` for the Data Access Layer [2025-01-25].
    - `Person`, `PersonService`, `PersonRepoService`, `PersonValidator`, and `PersonMapper` for the Business Layer [2025-01-23].
  - Improved maintainability and adherence to SOLID principles.
- **Data Transfer**: Introduced **Data Transfer Objects (DTOs)** for cleaner data handling [2025-01-18].
- **Resource Management**: Implemented proper resource disposal patterns to reduce memory leaks [2025-01-15].

For a detailed history of improvements, see the [CHANGELOG.md](./CHANGELOG.md) file.

---

## License Classes Supported
1. Small Motorcycle License
2. Heavy Motorcycle License
3. Standard Car License
4. Commercial License (Taxi/Limousine)
5. Agricultural Vehicle License
6. Small and Medium Bus License
7. Heavy Vehicle and Truck License

---

## Installation Requirements
- **Development Environment**: Visual Studio
- **Framework**: .NET Framework
- **Database**: SQL Server
- **Operating System**: Windows

## License
This project is licensed under the [MIT License](./LICENSE).
