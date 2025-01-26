# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

---
## [2.4.0] - 2025-01-26

### Added
- **Solution File**: Created a new solution file (`DVLD_Backend.sln`) to group BAL, DAL, and DTOs projects.
  - This ensures all backend projects are loaded together in Visual Studio.
  - Removes the need to manually re-add projects each time.

### Changed
- **Refactor**: Removed the Windows Forms UI (presentation layer) from the repository.
  - The Windows Forms project has been moved to a separate folder outside of Git.
  - The focus is now on refactoring the backend (BAL, DAL, and DTOs) for a future web-based frontend.
  - This change removes the distraction of maintaining the Windows Forms UI and allows full focus on backend improvements.
- **Project Dependencies**: Updated project dependencies to ensure proper build order.

### Future Plans
- Transition to a modern web application using ASP.NET Core.
- Improve backend architecture and add unit tests.
  
---

## [1.4.0] - 2025-01-26
### Added
**`UserSqlStatements` Class**:  
  - Centralized all SQL queries related to user operations into a single, dedicated class.  
  - Ensures consistent query usage across the application and simplifies maintenance and updates to SQL logic.  

**`UserParameterBuilder` Class**:  
  - Encapsulates the logic for building and configuring `SqlCommand` parameters.  
  - Reduces redundancy and improves code clarity by isolating parameter creation logic in one place.  

**`UserDataMapper` Class**:  
  - Handles the mapping between database query results and `UsersDTO` objects.  
  - Promotes separation of concerns by decoupling data retrieval from data transformation logic.  

### Changed  
- Refactored the data access layer to improve maintainability, readability, and adherence to the **Single Responsibility Principle (SRP)** and **Don't Repeat Yourself (DRY)** principles.  
- Introduced three dedicated helper classes (`UserSqlStatements`, `UserParameterBuilder`, and `UserDataMapper`) to streamline repository operations, reduce code duplication, and enhance modularity.  
- Updated existing repository implementations to leverage the new helper classes, ensuring a more structured and maintainable codebase.  

---

## [1.3.0] - 2025-01-25
### Added
- **PersonSqlStatements**: A new class to centralize SQL queries.
- **PersonParameterBuilder**: A new class to handle `SqlCommand` parameter logic.
- **PersonDataMapper**: A new class to manage mapping between database results and `PersonDTO`.

### Changed
- **Refactored `PersonRepository`**:
  - Created new helper classes, each with a single responsibility.
  - Updated `PersonRepository` to use the new helper classes.

### Fixed
- **Correct Logic for Checking Person Image Path**: The code incorrectly checked if the image path was null or empty. This has been fixed by adding a '!' (not operator) to the conditional statement.

### Removed
- **Redundant Code**: Removed redundant methods and logic from the old `PersonRepository` class.

---

## [1.2.0] - 2025-01-23
### Added
- **PersonValidator**: A new class to handle validation logic for person data.
- **PersonMapper**: A new class to map data between `Person` and `PersonDTO`.
- **PersonRepoService**: A new class to handle data access logic.
  
### Changed
- **Refactored `clsPerson`**: Split the `clsPerson` class into multiple classes, each with a single responsibility:
  - `Person`: Encapsulates person data.
  - `PersonService`: Manages business logic and workflows.
  - `PersonRepoService`: Handles data access logic.
- **Updated Presentation Layer**: Adjusted forms to use the new `PersonService` and related classes.
- **Adhered to C# Naming Conventions**: Updated class, method, and variable names to follow C# naming conventions.

### Fixed
- **Naming Inconsistencies**: Fixed naming issues to ensure consistency across the codebase.

### Removed
- **Redundant Code**: Removed redundant methods and logic from the old `clsPerson` class.

---

## [1.1.0] - 2025-01-18
### Added
- Introduced **Data Transfer Objects (DTOs)** to encapsulate data transferred between the **Data Access Layer (DAL)** and the **Business Layer (BL)**.
  - Created DTO classes for all entities (e.g., `PersonDTO`, `LicenseDTO`, `TestDTO`, etc.).
  - Updated the DAL to return DTO objects instead of using `ref` parameters.
  - Updated the Business Layer to map DTO objects to business entities (e.g., `clsPerson`, `clsLicense`, `clsTest`, etc.).
- Added a **new project** (`DVLD.DTOs`) to house all DTO classes, ensuring clean separation of concerns and reusability across layers.

### Changed
- Improved **resource management** in the Data Access Layer by implementing the `using` statement for `SqlConnection`, `SqlCommand`, and `SqlDataReader`.
  - Ensures proper disposal of database resources and reduces the risk of memory leaks.
- Refactored the **Business Layer** to use DTOs for data transfer:
  - Replaced multiple `ref` parameters with DTO objects.
  - Simplified methods like `Find`, `Add`, and `Update` by mapping DTOs to business entities.
- Updated the **DAL methods** to handle `null` or empty values for optional fields (e.g., `ThirdName`, `Email`, `ImagePath`) and map them to `DBNull.Value` when inserting or updating records.

### Fixed
- Resolved potential **circular dependency** issues by moving logic for related entities (e.g., `clsCountry`) to the Business Layer and avoiding direct references to business entities in the DAL.
- Fixed inconsistent handling of optional fields in the DAL (e.g., `ThirdName`, `Email`, `ImagePath`).

### Removed
- Removed redundant `ref` parameters from DAL methods, replacing them with DTOs for cleaner and more maintainable code.

---

## [1.0.0] - 2025-01-04
### Added
- Basic license management functionality
- Support for multiple license classes:
  - Small Motorcycle License
  - Heavy Motorcycle License
  - Standard Car License
  - Commercial License
  - Agricultural Vehicle License
  - Bus License
  - Heavy Vehicle License
- Medical and theoretical exam management
- Practical driving test scheduling
- License renewal and replacement services
- User and person management
- Administrative controls

### Changed
- N/A

### Fixed
- N/A

### Removed
- N/A
 
---

### Links
- **[Delete/Remove-Windows-Forms-UI](https://github.com/Hocine-Bec/DVLD-Project/compare/v1.4.0...v2.4.0)**: Removed the **Windows Forms UI (presentation layer)** to focus on backend development..
- **[Refactor/SRP-Data-Access-Layer](https://github.com/Hocine-Bec/DVLD-Project/compare/v1.3.0...v1.4.0)**: Improved **Data Access Layer (DAL)** class to adhere to the **Don't Repeat Yourself Principle (DRY)**.
- **[Refactor/SRP-People-Business-Layer](https://github.com/Hocine-Bec/DVLD-Project/compare/v1.1.0...v1.2.0)**: Improved `clsPerson` class to adhere to the **Single Responsibility Principle (SRP)**.
- **[Refactor/Add-DTOs](https://github.com/Hocine-Bec/DVLD-Project/compare/v1.0.0...v1.1.0)**: Introduced **Data Transfer Objects (DTOs)** for cleaner data handling.
- **[Initial Release](https://github.com/Hocine-Bec/DVLD-Project/tree/v1.0.0)**: Initial implementation of the project.
