# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

---

## [Unreleased]
### Added
- (New features or functionality being worked on.)

### Changed
- (Changes to existing functionality.)

### Fixed
- (Bug fixes in progress.)

### Removed
- (Code or features being removed.)

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