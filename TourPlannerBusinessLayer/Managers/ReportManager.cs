using iText.IO.Font.Constants;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using TourPlannerBusinessLayer.Service;
using Microsoft.Web.WebView2.Wpf;
using Microsoft.Web.WebView2.Core;
using iText.IO.Image;

namespace TourPlannerBusinessLayer.Managers
{
    public class ReportManager
    {
        public void GenerateReport(Tour tour, string destinationPath, TourService tourService)
        {
            PdfWriter writer = new PdfWriter(destinationPath);
            PdfDocument pdf = new PdfDocument(writer);
            Document document = new Document(pdf);

            // Tour Information
            Paragraph tourHeader = new Paragraph($"Tour: {tour.Name}")
                    .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA))
                    .SetFontSize(18)
                    .SetBold();
            document.Add(tourHeader);

            document.Add(new Paragraph($"From: {tour.From}"));
            document.Add(new Paragraph($"To: {tour.To}"));
            document.Add(new Paragraph($"Distance: {tour.Distance} km"));
            document.Add(new Paragraph($"Time: {tour.Time} h"));
            document.Add(new Paragraph($"Description: {tour.Description}"));
            document.Add(new Paragraph($"Transport Type: {tour.TransportType}"));
            document.Add(new Paragraph($"Popularity: {tour.Popularity}"));
            document.Add(new Paragraph($"Child Friendliness: {tour.ChildFriendliness}"));

            // Adding a line break
            document.Add(new Paragraph("\n"));

            // Tour Logs Information
            Paragraph tourLogsHeader = new Paragraph("Tour Logs")
                    .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA))
                    .SetFontSize(16)
                    .SetBold();
            document.Add(tourLogsHeader);

            Table table = new Table(UnitValue.CreatePercentArray(new float[] { 1, 2, 2, 1, 1, 1 })).UseAllAvailableWidth();
            table.AddHeaderCell(GetHeaderCell("Date"));
            table.AddHeaderCell(GetHeaderCell("Comment"));
            table.AddHeaderCell(GetHeaderCell("Difficulty"));
            table.AddHeaderCell(GetHeaderCell("Total Distance"));
            table.AddHeaderCell(GetHeaderCell("Total Time"));
            table.AddHeaderCell(GetHeaderCell("Rating"));

            foreach (var log in tour.TourLogs)
            {
                table.AddCell(log.Date?.ToString("dd.MM.yyyy") ?? string.Empty);
                table.AddCell(log.Comment ?? string.Empty);
                table.AddCell(log.Difficulty ?? string.Empty);
                table.AddCell(log.TotalDistance.ToString());
                table.AddCell(log.TotalTime.ToString());
                table.AddCell(log.Rating ?? string.Empty);
            }

            document.Add(table);
            document.Close();
        }


        public async Task GenerateReportWithMapScreenshot(Tour tour, string destinationPath, TourService tourService, WebView2 webView)
        {
            // Define the screenshot file path
            string screenshotFilePath = "C:\\Users\\micha\\Desktop\\map_screenshot.png";
            
            // Capture the screenshot
            await CaptureScreenshotAsync(webView, screenshotFilePath);

            PdfWriter writer = new PdfWriter(destinationPath);
            PdfDocument pdf = new PdfDocument(writer);
            Document document = new Document(pdf);

            // Tour Information
            Paragraph tourHeader = new Paragraph($"Tour: {tour.Name}")
                    .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA))
                    .SetFontSize(18)
                    .SetBold();
            document.Add(tourHeader);

            document.Add(new Paragraph($"From: {tour.From}"));
            document.Add(new Paragraph($"To: {tour.To}"));
            document.Add(new Paragraph($"Distance: {tour.Distance} km"));
            document.Add(new Paragraph($"Time: {tour.Time} h"));
            document.Add(new Paragraph($"Description: {tour.Description}"));
            document.Add(new Paragraph($"Transport Type: {tour.TransportType}"));
            document.Add(new Paragraph($"Popularity: {tour.Popularity}"));
            document.Add(new Paragraph($"Child Friendliness: {tour.ChildFriendliness}"));

            // Adding a line break
            document.Add(new Paragraph("\n"));

            // Add the map screenshot
            Image mapImage = new Image(ImageDataFactory.Create(screenshotFilePath));
            document.Add(mapImage);

            // Tour Logs Information
            Paragraph tourLogsHeader = new Paragraph("Tour Logs")
                    .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA))
                    .SetFontSize(16)
                    .SetBold();
            document.Add(tourLogsHeader);

            Table table = new Table(UnitValue.CreatePercentArray(new float[] { 1, 2, 2, 1, 1, 1 })).UseAllAvailableWidth();
            table.AddHeaderCell(GetHeaderCell("Date"));
            table.AddHeaderCell(GetHeaderCell("Comment"));
            table.AddHeaderCell(GetHeaderCell("Difficulty"));
            table.AddHeaderCell(GetHeaderCell("Total Distance"));
            table.AddHeaderCell(GetHeaderCell("Total Time"));
            table.AddHeaderCell(GetHeaderCell("Rating"));

            foreach (var log in tour.TourLogs)
            {
                table.AddCell(log.Date?.ToString("dd.MM.yyyy") ?? string.Empty);
                table.AddCell(log.Comment ?? string.Empty);
                table.AddCell(log.Difficulty ?? string.Empty);
                table.AddCell(log.TotalDistance.ToString());
                table.AddCell(log.TotalTime.ToString());
                table.AddCell(log.Rating ?? string.Empty);
            }

            document.Add(table);
            document.Close();
        }

        public async Task CaptureScreenshotAsync(WebView2 webView, string screenshotFilePath)
        {
            try
            {
                using (var stream = new FileStream(screenshotFilePath, FileMode.Create, FileAccess.Write))
                {
                    await webView.CoreWebView2.CapturePreviewAsync(CoreWebView2CapturePreviewImageFormat.Png, stream);
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
            }
        }


        public async void GenerateSummaryReport(string destinationPath, TourService tourService)
        {
            PdfWriter writer = new PdfWriter(destinationPath);
            PdfDocument pdf = new PdfDocument(writer);
            Document document = new Document(pdf);

            // Set document properties
            document.Add(new Paragraph("Tour Summary Report")
                .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA))
                .SetFontSize(24)
                .SetBold()
                .SetTextAlignment(TextAlignment.CENTER)
                .SetMarginBottom(20));

            List<Tour> tours = await tourService.GetToursAsync();

            foreach (var tour in tours)
            {
                double totalDistance = 0;
                double totalTime = 0;

                foreach (var log in tour.TourLogs)
                {
                    totalDistance += log.TotalDistance;
                    totalTime += log.TotalTime;
                }

                double averageDistance = tour.TourLogs.Count > 0 ? totalDistance / tour.TourLogs.Count : 0;
                double averageTime = tour.TourLogs.Count > 0 ? totalTime / tour.TourLogs.Count : 0;

                // Tour header
                document.Add(new Paragraph($"Tour: {tour.Name}")
                    .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA))
                    .SetFontSize(18)
                    .SetBold()
                    .SetFontColor(ColorConstants.BLUE)
                    .SetMarginTop(10)
                    .SetMarginBottom(10));

                // Create a table for tour details
                Table table = new Table(UnitValue.CreatePercentArray(new float[] { 1, 2 }))
                    .UseAllAvailableWidth();

                // Add table header
                table.AddHeaderCell(new Cell().Add(new Paragraph("Metric")
                    .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA))
                    .SetFontSize(12)
                    .SetBold()
                    .SetFontColor(ColorConstants.WHITE))
                    .SetBackgroundColor(ColorConstants.GRAY));

                table.AddHeaderCell(new Cell().Add(new Paragraph("Value")
                    .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA))
                    .SetFontSize(12)
                    .SetBold()
                    .SetFontColor(ColorConstants.WHITE))
                    .SetBackgroundColor(ColorConstants.GRAY));

                // Add data to the table
                table.AddCell(new Cell().Add(new Paragraph("Average Distance")
                    .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA))
                    .SetFontSize(12)));

                table.AddCell(new Cell().Add(new Paragraph($"{averageDistance:F2} km")
                    .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA))
                    .SetFontSize(12)));

                table.AddCell(new Cell().Add(new Paragraph("Average Time")
                    .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA))
                    .SetFontSize(12)));

                table.AddCell(new Cell().Add(new Paragraph($"{averageTime:F2} h")
                    .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA))
                    .SetFontSize(12)));

                document.Add(table);
            }

            document.Close();
        }


        private Cell GetHeaderCell(string headerText)
        {
            return new Cell().Add(new Paragraph(headerText)).SetBold().SetBackgroundColor(ColorConstants.GRAY);
        }
    }
}
