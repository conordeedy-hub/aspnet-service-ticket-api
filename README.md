# Service Dispatch Ticket API (C# / ASP.NET Core)

A RESTful **ASP.NET Core Minimal API** built in **C#** to manage field-service appointment tickets, filter workloads by technician or ticket status, and log new repair requests. Designed as the backend implementation companion to my **Systems Analysis & Design (CIS2321)** service-department case study.

## API Endpoints
- `GET /api/health` — Returns API status and count of active (unresolved) tickets.
- `GET /api/tickets?status=Open&technician=Liam` — Retrieves service tickets with optional filtering by status or assigned technician.
- `POST /api/tickets` — Validates input and creates a new service ticket with an auto-incremented ID.

## How to Run Locally
```bash
git clone https://github.com
cd aspnet-service-ticket-api
dotnet run
```
