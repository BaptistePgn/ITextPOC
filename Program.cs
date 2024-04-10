using iText.Kernel.Colors;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using System.Data;

#region Initialize a PdfDocument
var writer = new PdfWriter("invoice.pdf");
var pdf = new PdfDocument(writer);

// Define the page size
PageSize pageSize = PageSize.A4;

// Create a Document
using var document = new Document(pdf, pageSize);

// Define the half width of the page
float halfWidth = ((pageSize.GetWidth() - (document.GetLeftMargin() + document.GetRightMargin())) / 2) - 5;
#endregion

#region Header
// Create a Table to contain the two divs
Table table = new Table(new float[] { halfWidth, halfWidth })
    .SetWidth(UnitValue.CreatePercentValue(100));

// Add the left div
Div leftDiv = new Div()
    .SetTextAlignment(TextAlignment.LEFT)
    .Add(new Paragraph("Name \n " +
        "Siren Number \n " +
        "Head office address \n " +
        "Type of company \n " +
        "Amount of share capital \n" +
        "VAT identification number"));
table.AddCell(new Cell().SetBorder(Border.NO_BORDER).Add(leftDiv));

// Add the right div
Div rightDiv = new Div()
    .SetTextAlignment(TextAlignment.RIGHT)
    .Add(new Paragraph("Invoice number \n" +
        "Invoice date \n" +
        "Client name \n" +
        "Head office address"));
table.AddCell(new Cell().SetBorder(Border.NO_BORDER).Add(rightDiv));

// Add the table to the document
document.Add(table);

// Add a line to separate the header from the body
document.Add(new LineSeparator(new SolidLine()).SetMarginTop(10).SetMarginBottom(10));
#endregion

#region Body
// Add the title
document.Add(new Paragraph("Date of service or sale")
       .SetTextAlignment(TextAlignment.CENTER)
       .SetFontSize(20));

//Create a DataTable to contain the data
DataTable dataTable = new DataTable();
dataTable.Columns.Add("Description", typeof(string));
dataTable.Columns.Add("Unit price", typeof(decimal));
dataTable.Columns.Add("VAT rate", typeof(decimal));
dataTable.Columns.Add("Quantity", typeof(int));
dataTable.Columns.Add("Total price excl.", typeof(decimal));
dataTable.Columns.Add("Total price incl. VAT", typeof(decimal));

// Add the table content
for (int i = 0; i < 10; i++)
{
    var row = dataTable.NewRow();
    row["Description"] = $"Product {i + 1}";
    row["Unit price"] = 10.0m;
    row["VAT rate"] = 0.21m;
    row["Quantity"] = i + 1;
    row["Total price excl."] = 10.0m * (i + 1);
    row["Total price incl. VAT"] = 10.0m * (i + 1) * 1.21m;
    dataTable.Rows.Add(row);
}

// Create a Table to contain the data
Table dataTablePdf = new Table(new float[] { 5, 1, 1, 1, 1, 1 })
    .SetMarginTop(10)
    .SetWidth(UnitValue.CreatePercentValue(100));

// Add the headers
foreach (DataColumn column in dataTable.Columns)
{
    dataTablePdf.AddHeaderCell(column.ColumnName);
}

// Add the data
foreach (DataRow row in dataTable.Rows)
{
    foreach (DataColumn column in dataTable.Columns)
    {
        dataTablePdf.AddCell(row[column].ToString());
    }
}

// Add the table to the document
document.Add(dataTablePdf);

// Add the total
decimal totalExcl = dataTable.AsEnumerable().Sum(x => x.Field<decimal>("Total price excl."));
decimal totalIncl = dataTable.AsEnumerable().Sum(x => x.Field<decimal>("Total price incl. VAT"));

document.Add(new Paragraph($"Total excl. VAT: {totalExcl}").SetTextAlignment(TextAlignment.RIGHT));
document.Add(new Paragraph($"Total incl. VAT: {totalIncl}").SetTextAlignment(TextAlignment.RIGHT));
#endregion

#region Footer 
// Create a footer
Paragraph footer = new();
footer.Add("Date\n");
footer.Add(new Text("\n"));
footer.Add("Mention of penalties for late payment\n");
footer.Add("Other..");
footer.SetTextAlignment(TextAlignment.RIGHT);
footer.SetFixedPosition(-document.GetLeftMargin(), document.GetBottomMargin(), pageSize.GetWidth());
// Add the footer to the document
document.Add(footer);

// Close the document
document.Close();
#endregion







