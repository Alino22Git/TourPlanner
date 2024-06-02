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
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TourPlannerBusinessLayer.Exceptions;
using TourPlannerBusinessLayer.Service;
using TourPlannerLogging;
using Microsoft.Web.WebView2.Wpf;
using Microsoft.Web.WebView2.Core;
using iText.IO.Image;

namespace TourPlannerBusinessLayer.Managers
{
    public class ReportManager
    {
        private static readonly ILoggerWrapper logger = LoggerFactory.GetLogger();

        public ReportManager()
        {
        }

        public void GenerateReport(Tour tour, string destinationPath, TourService tourService)
        {
            try
            {
                PdfWriter writer = new PdfWriter(destinationPath);
                PdfDocument pdf = new PdfDocument(writer);
                Document document = new Document(pdf);

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

                document.Add(new Paragraph("\n"));

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
            catch (Exception ex)
            {
                string errorMsg = $"Error generating report: {ex.Message}";
                logger.Error(errorMsg);
                throw new ReportManagerException(errorMsg, ex);
            }
        }

        public async Task GenerateReportWithMapScreenshot(Tour tour, string destinationPath, TourService tourService, WebView2 webView)
        {
            try
            {
                string screenshotFilePath = "C:\\Users\\micha\\Desktop\\map_screenshot.png";

                await CaptureScreenshotAsync(webView, screenshotFilePath);

                PdfWriter writer = new PdfWriter(destinationPath);
                PdfDocument pdf = new PdfDocument(writer);
                Document document = new Document(pdf);

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

                document.Add(new Paragraph("\n"));

                Image mapImage = new Image(ImageDataFactory.Create(screenshotFilePath));
                document.Add(mapImage);

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
            catch (Exception ex)
            {
                string errorMsg = $"Error generating report with map screenshot: {ex.Message}";
                logger.Error(errorMsg);
                throw new ReportManagerException(errorMsg, ex);
            }
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
                string errorMsg = $"Error capturing screenshot: {ex.Message}";
                logger.Error(errorMsg);
                throw new ReportManagerException(errorMsg, ex);
            }
        }

        public async void GenerateSummaryReport(string destinationPath, TourService tourService)
        {
            try
            {
                PdfWriter writer = new PdfWriter(destinationPath);
                PdfDocument pdf = new PdfDocument(writer);
                Document document = new Document(pdf);

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

                    document.Add(new Paragraph($"Tour: {tour.Name}")
                        .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA))
                        .SetFontSize(18)
                        .SetBold()
                        .SetFontColor(ColorConstants.BLUE)
                        .SetMarginTop(10)
                        .SetMarginBottom(10));

                    Table table = new Table(UnitValue.CreatePercentArray(new float[] { 1, 2 }))
                        .UseAllAvailableWidth();

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
            catch (Exception ex)
            {
                string errorMsg = $"Error generating summary report: {ex.Message}";
                logger.Error(errorMsg);
                throw new ReportManagerException(errorMsg, ex);
            }
        }

        private Cell GetHeaderCell(string headerText)
        {
            return new Cell().Add(new Paragraph(headerText)).SetBold().SetBackgroundColor(ColorConstants.GRAY);
        }
    }
}
