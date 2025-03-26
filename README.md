# Intermediate C# Developer Take-Home Test - Price Submission API

## Overview

This project implements a price submission API that allows users to submit prices for items to a retailer and check the submission status. The system interacts with a provided `MockRetailerService` to simulate the retailer's acceptance of prices. The solution utilizes Entity Framework (EF) with SQLite for data persistence and a background process (e.g., Hangfire) to handle asynchronous price submissions.

## Requirements

-   Create two API endpoints:
    -   `Submit Price`: Accepts item prices and returns a submission ID.
    -   `Get Price Submit Status`: Accepts a submission ID and returns the submission status.
-   Use Entity Framework (EF) with SQLite or InMemory database.
-   Implement the `Submit Price` endpoint to:
    -   Respond immediately with a submission ID.
    -   Check the `MockRetailerService.CanSubmitPrice()` method up to three times.
    -   Transition the price submission status from `PendingSubmission` to `Submitted` when `CanSubmitPrice()` returns `true`.
    -   Asynchronously submit the price to `MockRetailerService.SubmitPrice()` once it's accepted.
-   Implement the `Get Price Submit Status` endpoint to:
    -   Consume the submission ID.
    -   Respond with the submission status (`PendingSubmission`, `Submitted`, `Failed`).
    -   Provide a reason for failure if the status is `Failed`.
-   Use the provided `IDataReceiverService` and `MockRetailerService` code without modification.

## Technical Details

### Solution Architecture

-   **API Endpoints:**
    -   `POST /api/prices`: Accepts a JSON payload containing the `Gtin` and `Price` of an item.
    -   `GET /api/prices/{id}`: Retrieves the submission status for a given submission ID.
-   **Data Storage:**
    -   Entity Framework Core with SQLite is used to persist price submission data.
    -   A `PriceSubmission` entity is defined to store the submission ID, GTIN, price, status, and failure reason (if applicable).
-   **Background Processing:**
    -   Hangfire is used to handle asynchronous price submissions and retries.
    -   A background job is created when a price is submitted, which checks the retailer's acceptance status and submits the price accordingly.

### How to Run

1.  **Clone the repository.**
2.  **Navigate to the project directory:** `cd GS1Takehome`
3.  **Restore NuGet packages:** `dotnet restore`
4.  **Create database migration:** `dotnet ef migrations add [MigrationName]`
5.  **Apply database migrations:** `dotnet ef database update`
6.  **Run the application:** `dotnet run`
7.  **Use a tool like Postman or curl to interact with the API endpoints.**

### API Endpoints

-   `POST /api/prices`
    -   Request Body:
        ```json
        {
          "gtin": "05021731354670",
          "price": 10.99
        }
        ```
    -   Response:
        ```json
        {
          "id": "generated_guid"
        }
        ```
-   `GET /api/prices/{id}`
    -   Response:
        ```json
        {
            "status": 0,
            "reason": "string"
        }
        ```
