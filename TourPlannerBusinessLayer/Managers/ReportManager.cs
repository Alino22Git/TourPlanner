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

namespace TourPlannerBusinessLayer.Managers
{
    public class ReportManager
    {
        public void GenerateReport(Tour tour, string destinationPath)
        {
            PdfWriter writer = new PdfWriter(destinationPath);
            PdfDocument pdf = new PdfDocument(writer);
            Document document = new Document(pdf);

            // Tour Information
            Paragraph tourHeader = new Paragraph($"Tour: {tour.Name}")
                    .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA))
                    .SetFontSize(18)
                    .SetBold()
                    .SetFontColor(ColorConstants.BLUE);
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
                    .SetBold()
                    .SetFontColor(ColorConstants.GREEN);
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

        private Cell GetHeaderCell(string headerText)
        {
            return new Cell().Add(new Paragraph(headerText)).SetBold().SetBackgroundColor(ColorConstants.GRAY);
        }
    }
}
