# Invoice Management

A lightweight invoice management application built with Blazor and .NET 9.

The application allows users to create, view, edit, and delete invoices together with their line items. It also provides real-time line totals and invoice total calculations.

> Note: .NET 9 is used for this implementation with prior approval for the assessment.

---

## Features

- View all invoices
- View invoice details and line items
- Create new invoices
- Edit existing invoices
- Delete invoices with confirmation
- Add and remove invoice line items dynamically
- Real-time line total calculations
- Real-time invoice total calculation
- Invoice status management
- Input validation using FluentValidation
- Automatic SQLite database creation and migration
- Global exception handling
- Loading, empty, validation, and error states

---

## Technology Stack

- .NET 9
- Blazor Web App
- Interactive Server rendering
- Entity Framework Core
- SQLite
- FluentValidation
- Bootstrap

---

## Architecture

The solution uses a lightweight Clean Architecture approach and is divided into four projects:

```text
InvoiceManagement
│
├── InvoiceManagement.Domain
├── InvoiceManagement.Application
├── InvoiceManagement.Infrastructure
└── InvoiceManagement.Web
```

### Domain

Contains the core domain objects and enums.

Main types:

- `Invoice`
- `InvoiceLineItem`
- `InvoiceStatus`

The domain layer has no dependency on the other application layers.

### Application

Contains application logic and abstractions.

Responsibilities include:

- DTOs
- Invoice application service
- Service interfaces
- Database abstraction
- FluentValidation validators

The UI communicates with the application through `IInvoiceService` rather than accessing Entity Framework Core directly.

### Infrastructure

Contains persistence-related implementations.

Responsibilities include:

- `AppDbContext`
- Entity Framework Core configurations
- SQLite integration
- Database migrations
- Implementation of `IApplicationDbContext`

This keeps persistence concerns outside the UI and domain layers.

### Web

Contains the Blazor user interface and web-specific concerns.

Responsibilities include:

- Invoice list
- Invoice details
- Create invoice
- Edit invoice
- Navigation
- User interaction
- Global web exception handling
- UI error handling

---

## Dependency Injection and Separation of Concerns

Dependency Injection is used throughout the application to keep components loosely coupled.

For example, Blazor components depend on:

```csharp
IInvoiceService
```

instead of directly creating or depending on `InvoiceService`.

Similarly, `InvoiceService` depends on:

```csharp
IApplicationDbContext
```

instead of directly depending on the concrete `AppDbContext`.

The concrete implementations are registered in the dependency injection container during application startup.

This separation allows the UI, application logic, domain model, and persistence implementation to remain focused on their individual responsibilities.

---

## Data Model

### Invoice

The `Invoice` entity contains six properties:

- Id
- InvoiceNumber
- InvoiceDate
- CustomerName
- Status
- LineItems

### Invoice Line Item

The `InvoiceLineItem` entity contains six properties:

- Id
- InvoiceId
- Description
- Quantity
- UnitPrice
- Invoice

Calculated values such as line totals and invoice totals are intentionally kept outside the persisted entities.

---

## Calculated Properties

A line total is calculated from:

```text
Quantity × Unit Price
```

The invoice total is calculated as the sum of all line totals.

These values are calculated dynamically rather than stored in the database, preventing duplicated derived data.

The UI updates totals when quantity or unit price values are changed.

---

## State and Data Binding

Blazor two-way data binding is used in the Create and Edit pages.

For example:

```razor
@bind-Value="model.InvoiceNumber"
```

and:

```razor
@bind-Value="item.Quantity"
```

The page model is represented by `SaveInvoiceDto`.

Line items are maintained as a collection within the page model. Adding or removing a line item updates the component state and causes Blazor to re-render the relevant UI.

Calculated totals are derived from the current component state, so the displayed totals remain synchronized with the entered values.

The application uses Interactive Server rendering for interactive operations such as:

- Adding line items
- Removing line items
- Updating totals
- Saving invoices
- Delete confirmation

---

## Validation

FluentValidation is used in the Application layer.

Invoice validation includes:

- Invoice number is required
- Invoice number has a maximum length
- Customer name is required
- Customer name has a maximum length
- Invoice status must be valid
- At least one line item is required

Line item validation includes:

- Description is required
- Quantity must be greater than zero
- Unit price cannot be negative

The database also contains constraints for important data integrity rules.

The UI prevents removal of the final line item for a better user experience, while application-level validation still enforces the rule independently.

Invoice numbers are also checked for uniqueness.

---

## UX Decisions

The UI was designed to keep invoice management simple and intuitive.

Some of the UX decisions include:

- A dedicated invoice list with direct View, Edit, and Delete actions
- Status badges to make invoice states easy to identify
- Dynamic line item management
- Immediate line and invoice total calculations
- Delete confirmation before destructive operations
- Prevention of accidental duplicate save/delete actions
- Loading indicators during asynchronous operations
- Empty state when no invoices exist
- Validation messages displayed directly to the user
- Friendly error state for unexpected UI errors
- Prevention of deleting the final line item

These decisions reduce unnecessary navigation and provide immediate feedback for common user actions.

---

## Database

SQLite is used as the database because it is file-based and portable.

The connection string is configured as:

```text
Data Source=invoices.db
```

Entity Framework Core migrations are automatically applied during application startup using:

```csharp
await context.Database.MigrateAsync();
```

Therefore, a developer does not need to manually create the database or run `Update-Database` before starting the application.

If the database file does not exist, it is created automatically and all pending migrations are applied.

No sample invoice data is automatically recreated after deletion.

---

## Running the Application

### Prerequisites

Install:

- .NET 9 SDK

### Steps

1. Clone the repository.

2. Open the solution in Visual Studio or navigate to the solution directory using a terminal.

3. Restore the dependencies:

```bash
dotnet restore
```

4. Build the solution:

```bash
dotnet build
```

5. Run the Web project:

```bash
dotnet run --project InvoiceManagement.Web
```

6. Open the URL displayed in the terminal.

The SQLite database will be created automatically when the application starts.

---

## How to Create an Invoice

1. Open the **Invoices** page.
2. Click **New Invoice**.
3. Enter the invoice number.
4. Enter the customer name.
5. Select the invoice date and status.
6. Enter the first line item.
7. Use **Add Item** to add additional line items if required.
8. Enter quantity and unit price values.
9. Review the automatically calculated total.
10. Click **Save Invoice**.

---

## How to Edit an Invoice

1. Open the **Invoices** page.
2. Locate the required invoice.
3. Click **Edit**.
4. Update the invoice information as required.
5. Modify existing line items.
6. Add new line items using **Add Item**.
7. Remove line items using the remove button.
8. Review the updated calculated total.
9. Click **Save Changes**.

At least one line item must remain on an invoice.

---

## Viewing an Invoice

From the invoice list, click **View**.

The details page displays:

- Invoice number
- Customer
- Invoice date
- Status
- Line items
- Quantity
- Unit price
- Line totals
- Invoice total

The invoice can also be edited or deleted directly from this page.

---

## Deleting an Invoice

An invoice can be deleted from either the invoice list or the invoice details page.

A confirmation message is displayed before deletion to prevent accidental removal.

Deleting an invoice also deletes its associated line items through the configured relationship.

---

## Error Handling

Expected validation errors are handled close to the user interaction and displayed as validation messages.

Unexpected HTTP-level errors are handled through a global exception handler and logged without exposing internal exception details to the user.

Blazor UI errors are protected by an error boundary that displays a user-friendly error state.

This keeps expected validation failures separate from unexpected application failures.

---

## Design Considerations

The solution intentionally avoids unnecessary architectural complexity.

For a module of this size, introducing additional patterns such as CQRS, MediatR, Repository, or Unit of Work would add abstractions without providing a meaningful benefit.

Instead, the application uses:

```text
Blazor Component
       ↓
IInvoiceService
       ↓
InvoiceService
       ↓
IApplicationDbContext
       ↓
AppDbContext
       ↓
SQLite
```

This provides clear separation of concerns and dependency inversion while keeping the implementation easy to understand and maintain.