using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Net.Mail;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using QuestPDF.Helpers;
using System.IO;
using Transport__system_prototype.Migrations;

namespace Transport__system_prototype.Controllers
{
    public class EmailTestController : Controller
    {
        public IActionResult SendTestEmail()
        {
            try
            {
                // Set the QuestPDF license type
                QuestPDF.Settings.License = LicenseType.Community;

                // Enable debugging to get more detailed error information
                QuestPDF.Settings.EnableDebugging = true;

                // Email settings
                var fromAddress = new MailAddress("biocodeocteam4010@gmail.com", "Transport System");
                var toAddress = new MailAddress("ali.essam.abdelhaleem.20@gmail.com"); // change to your target test email
                const string fromPassword = "hmrj gfwn kdhx cljl"; // use app password if possible

                const string subject = "Booking Confirmation - Your Ticket";
                const string body = "Thank you for booking with us! Please find your ticket attached.";

                // Generate the Ticket PDF
                var ticketBytes = GenerateTestTicket();

                // Prepare email
                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                };

                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body
                })
                {
                    // Attach the PDF ticket
                    message.Attachments.Add(new Attachment(new MemoryStream(ticketBytes), "Ticket.pdf", "application/pdf"));

                    smtp.Send(message);
                }

                return Content("Email sent successfully!");
            }
            catch (Exception ex)
            {
                return Content($"Failed to send email. Error: {ex.Message}");
            }
        }

        private byte[] GenerateTestTicket()
        {
            // Sample data - in a real app, this would come from your database
            var ticketData = new
            {
                PassengerName = "John Doe",
                From = "Cairo",
                To = "Alexandria",
                Date = "May 2, 2025",
                Time = "10:00 AM",
                Seats = 2,
                Price = "$40.00",
                BookingId = "BK-12345",
                QrData = "BK-12345-CAIRO-ALEX-20250502-1000"
            };

            var pdf = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(1, Unit.Centimetre);
                    page.DefaultTextStyle(x => x.FontSize(10));

                    // Header with background
                    page.Header().Height(50).Background(Colors.Teal.Medium).Padding(10).Row(row =>
                    {
                        row.RelativeItem(3).Column(col =>
                        {
                            col.Item().Text("TransportSystem").FontSize(16).Bold().FontColor(Colors.White);
                            col.Item().Text("Your Journey, Our Priority").FontSize(9).FontColor(Colors.White);
                        });

                        row.RelativeItem(2).AlignRight().AlignMiddle().Text(text =>
                        {
                            text.Span("Booking ID: ").FontColor(Colors.White);
                            text.Span(ticketData.BookingId).Bold().FontColor(Colors.White);
                        });
                    });

                    // Content
                    page.Content().Column(column =>
                    {
                        // Title
                        column.Item().PaddingTop(10).AlignCenter().Text("TRAVEL TICKET")
                            .FontSize(16).Bold().FontColor(Colors.Teal.Medium);

                        // Main content
                        column.Item().PaddingTop(10).Background(Colors.Grey.Lighten5).Border(1).BorderColor(Colors.Grey.Lighten3).Padding(10).Column(contentCol =>
                        {
                            // Passenger Information
                            contentCol.Item().Text("Passenger Information").FontSize(12).Bold().FontColor(Colors.Teal.Medium);
                            contentCol.Item().PaddingTop(5).Row(nameRow =>
                            {
                                nameRow.RelativeItem().Text("Name:").FontColor(Colors.Grey.Medium);
                                nameRow.RelativeItem(3).Text(ticketData.PassengerName).Bold();
                            });

                            contentCol.Item().PaddingTop(10);

                            // Trip Details
                            contentCol.Item().Text("Trip Details").FontSize(12).Bold().FontColor(Colors.Teal.Medium);
                            contentCol.Item().PaddingTop(5).Row(tripRow =>
                            {
                                tripRow.RelativeItem().Text("From:").FontColor(Colors.Grey.Medium);
                                tripRow.RelativeItem().Text(ticketData.From).Bold();
                                tripRow.RelativeItem().Text("To:").FontColor(Colors.Grey.Medium);
                                tripRow.RelativeItem().Text(ticketData.To).Bold();
                            });

                            contentCol.Item().PaddingTop(10);

                            // Date & Time
                            contentCol.Item().Text("Date & Time").FontSize(12).Bold().FontColor(Colors.Teal.Medium);
                            contentCol.Item().PaddingTop(5).Row(dateRow =>
                            {
                                dateRow.RelativeItem().Text("Date:").FontColor(Colors.Grey.Medium);
                                dateRow.RelativeItem().Text(ticketData.Date).Bold();
                                dateRow.RelativeItem().Text("Time:").FontColor(Colors.Grey.Medium);
                                dateRow.RelativeItem().Text(ticketData.Time).Bold();
                            });

                            contentCol.Item().PaddingTop(10);

                            // Payment Details
                            contentCol.Item().Text("Payment Details").FontSize(12).Bold().FontColor(Colors.Teal.Medium);
                            contentCol.Item().PaddingTop(5).Row(paymentRow =>
                            {
                                paymentRow.RelativeItem().Text("Seats:").FontColor(Colors.Grey.Medium);
                                paymentRow.RelativeItem().Text(ticketData.Seats.ToString()).Bold();
                                paymentRow.RelativeItem().Text("Price:").FontColor(Colors.Grey.Medium);
                                paymentRow.RelativeItem().Text(ticketData.Price).Bold();
                            });
                        });

                        // Divider
                        column.Item().PaddingVertical(10).LineHorizontal(1);

                        // Boarding pass section
                        column.Item().Background(Colors.Grey.Lighten5).Border(1).BorderColor(Colors.Grey.Lighten3).Padding(10).Column(boardingCol =>
                        {
                            boardingCol.Item().Text("BOARDING PASS").FontSize(12).Bold();

                            boardingCol.Item().PaddingTop(5).Row(row =>
                            {
                                row.RelativeItem().Column(col =>
                                {
                                    col.Item().Row(fromRow =>
                                    {
                                        fromRow.RelativeItem().Text("From:").FontColor(Colors.Grey.Medium);
                                        fromRow.RelativeItem(3).Text(ticketData.From).Bold();
                                    });
                                    col.Item().Row(toRow =>
                                    {
                                        toRow.RelativeItem().Text("To:").FontColor(Colors.Grey.Medium);
                                        toRow.RelativeItem(3).Text(ticketData.To).Bold();
                                    });
                                });

                                row.RelativeItem().Column(col =>
                                {
                                    col.Item().Row(dateRow =>
                                    {
                                        dateRow.RelativeItem().Text("Date:").FontColor(Colors.Grey.Medium);
                                        dateRow.RelativeItem(3).Text(ticketData.Date).Bold();
                                    });
                                    col.Item().Row(timeRow =>
                                    {
                                        timeRow.RelativeItem().Text("Time:").FontColor(Colors.Grey.Medium);
                                        timeRow.RelativeItem(3).Text(ticketData.Time).Bold();
                                    });
                                    col.Item().Row(seatsRow =>
                                    {
                                        seatsRow.RelativeItem().Text("Seats:").FontColor(Colors.Grey.Medium);
                                        seatsRow.RelativeItem(3).Text(ticketData.Seats.ToString()).Bold();
                                    });
                                });

                                row.RelativeItem().AlignRight().AlignMiddle().Text(ticketData.BookingId).FontSize(12).Bold();
                            });
                        });

                        // Instructions
                        column.Item().PaddingTop(10).AlignCenter().Text("Please present this ticket before boarding").FontColor(Colors.Grey.Medium);

                        // Terms and conditions
                        column.Item().PaddingTop(10).AlignCenter().Column(termsCol =>
                        {
                            termsCol.Item().Text("Thank you for choosing TransportSystem for your journey!").FontSize(9);
                            termsCol.Item().Text("For any assistance, please contact our support at support@transportsystem.com").FontSize(9);
                            termsCol.Item().Text("Terms and conditions apply. Please arrive 15 minutes before departure.").FontSize(8).FontColor(Colors.Grey.Medium);
                        });
                    });

                    // Footer
                    page.Footer().AlignCenter().Text(text =>
                    {
                        text.Span("© 2025 TransportSystem - ").FontSize(8).FontColor(Colors.Grey.Medium);
                        text.Span("Page ").FontSize(8).FontColor(Colors.Grey.Medium);
                        text.CurrentPageNumber().FontSize(8).FontColor(Colors.Grey.Medium);
                        text.Span(" of ").FontSize(8).FontColor(Colors.Grey.Medium);
                        text.TotalPages().FontSize(8).FontColor(Colors.Grey.Medium);
                    });
                });
            });

            return pdf.GeneratePdf();
        }
    }
}