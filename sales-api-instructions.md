# Developer Evaluation – Implementation Instructions

## Objective

Implement a **Sales API (complete CRUD)** following DDD principles, using the existing project template as the foundation.

Before starting implementation, you MUST:
1. Analyze the existing template structure
2. Respect its architectural patterns (DDD, layering, naming conventions)
3. Extend it without breaking existing standards

---

## Step 0 – Understand the Existing Template

Before coding anything:

- Identify layers (expected):
  - API / Presentation
  - Application
  - Domain
  - Infrastructure
- Identify patterns already used:
  - Mediator (MediatR)
  - AutoMapper
  - Result pattern (if exists)
  - Validation (FluentValidation or similar)

### Important
DO NOT create a new architecture.
ADAPT the solution to the current template.

---

## Core Requirement

Build a **Sales API** with full CRUD.

The API must manage:

- Sale number
- Sale date
- Customer (External Identity pattern)
- Branch (External Identity pattern)
- Products (External Identity pattern)
- Quantities
- Unit prices
- Discounts
- Total amount per item
- Total sale amount
- Status (Cancelled / Not Cancelled)

---

## Domain Design (DDD)

### Aggregate Root: Sale

A Sale must:
- Be the aggregate root
- Control its items
- Enforce business rules

### Entities

#### Sale
- Id
- SaleNumber
- Date
- CustomerId + CustomerName (denormalized)
- BranchId + BranchName (denormalized)
- Status (Active / Cancelled)
- Items (collection)
- TotalAmount

#### SaleItem
- ProductId + ProductName (denormalized)
- Quantity
- UnitPrice
- Discount
- TotalAmount
- IsCancelled

---

## Business Rules (MANDATORY)

1. Quantity rules per item:
   - < 4 items → NO discount
   - >= 4 items → 10% discount
   - 10 to 20 items → 20% discount
   - > 20 items → THROW error (not allowed)

2. Discount must be calculated automatically:
   - Do NOT accept discount from input

3. Total per item:
   total = quantity * unitPrice - discount

4. Sale total:
   sum of all item totals

5. Cancellation:
   - Sale can be cancelled
   - Item can be cancelled independently

---

## Application Layer

Use Mediator pattern:

### Commands
- CreateSaleCommand
- UpdateSaleCommand
- CancelSaleCommand
- CancelItemCommand

### Queries
- GetSaleById
- GetSales (with pagination/filter/sort)

---

## API Layer

### Endpoints

- POST /sales
- GET /sales
- GET /sales/{id}
- PUT /sales/{id}
- DELETE /sales/{id}

Additional:
- POST /sales/{id}/cancel
- POST /sales/{id}/items/{itemId}/cancel

---

## API Features

Pagination:
- _page
- _size

Sorting:
- _order

Filtering:
- field=value
- _min / _max
- * for partial

---

## Error Handling

{
  "type": "string",
  "error": "string",
  "detail": "string"
}

---

## Events (Differential)

- SaleCreated
- SaleModified
- SaleCancelled
- ItemCancelled

Use logs or internal dispatcher.

---

## Persistence

- EF Core
- PostgreSQL

---

## Testing

- xUnit
- NSubstitute
- Faker

---

## Logging

Log events and errors.

---

## Git & Organization

- Semantic commits
- Separate concerns

---

## README Requirements

- How to run
- How to test
- Example requests

---

## Final Instruction

Prioritize:
1. Business rules
2. Clean architecture
3. Readability
4. Testability
