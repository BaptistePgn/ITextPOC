using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using System.Data;

using var document = new Document(new PdfDocument(new PdfWriter("invoice.pdf")));

// Add the header
Paragraph headerLeft = new Paragraph("Name \n " +
       "Siren Number \n " +
       "Head office address \n " +
       "Type of company \n " +
       "Amount of share capital \n" +
       "VAT identification number")
    .SetTextAlignment(TextAlignment.LEFT);

// Add first left top paragraph
document.Add(headerLeft);








