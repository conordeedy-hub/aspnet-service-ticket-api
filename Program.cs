using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// In-memory store modeled after the Appliance Warehouse Service Department workflow
var tickets = new List<ServiceTicket>
{
    new(1001, "Refrigerator cooling compressor diagnostic", "Liam Henson", "High", "Open"),
    new(1002, "Washer drain pump replacement - awaiting part", "Mark Robles", "Medium", "In Progress"),
    new(1003, "Smart oven Wi-Fi module calibration", "Lee Wong", "Low", "Resolved")
};

app.MapGet("/api/health", () => Results.Ok(new
{
    service = "Service Dispatch Ticket API",
    status = "Healthy",
    activeTickets = tickets.Count(t => t.Status != "Resolved")
}));

app.MapGet("/api/tickets", (string? status, string? technician) =>
{
    IEnumerable<ServiceTicket> query = tickets;

    if (!string.IsNullOrWhiteSpace(status))
        query = query.Where(t => t.Status.Equals(status, StringComparison.OrdinalIgnoreCase));

    if (!string.IsNullOrWhiteSpace(technician))
        query = query.Where(t => t.AssignedTechnician.Contains(technician, StringComparison.OrdinalIgnoreCase));

    return Results.Ok(query);
});

app.MapPost("/api/tickets", (CreateTicketRequest request) =>
{
    if (string.IsNullOrWhiteSpace(request.IssueDescription))
        return Results.BadRequest(new { error = "IssueDescription is required." });

    var nextId = tickets.Any() ? tickets.Max(t => t.TicketId) + 1 : 1001;
    var ticket = new ServiceTicket(
        nextId,
        request.IssueDescription,
        request.AssignedTechnician ?? "Unassigned",
        request.Priority ?? "Medium",
        "Open"
    );

    tickets.Add(ticket);
    return Results.Created($"/api/tickets/{ticket.TicketId}", ticket);
});

app.Run();

record ServiceTicket(int TicketId, string IssueDescription, string AssignedTechnician, string Priority, string Status);
record CreateTicketRequest(string IssueDescription, string? AssignedTechnician, string? Priority);
