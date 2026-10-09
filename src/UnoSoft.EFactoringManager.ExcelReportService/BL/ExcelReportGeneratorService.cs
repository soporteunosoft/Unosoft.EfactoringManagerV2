using ClosedXML.Excel;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using UnoSoft.ExcelReportService.BL.Entities;

namespace UnoSoft.EFactoringManager.ExcelReportService.BL
{
    public class ExcelReportGeneratorService
    {


        public ExcelReportGeneratorService() { }


        public byte[] GenerateReport(Report report, List<dynamic> data)
        {
            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add(report.Name);
            worksheet.Row(report.Body.InitialRow).Height = 50; // Altura en puntos

            // Ajustar el ancho de la columna automáticamente


            // Escribir encabezados
            int colIndex = report.Body.InitialColumn;
            foreach (var field in report.Body.Fields)
            {
                var cell = worksheet.Cell(report.Body.InitialRow, colIndex);


                cell.Value = field.Caption;

                cell.Style.Font.FontName = report.Font?.Name ?? "Arial";
                cell.Style.Font.FontSize = 11; // float.Parse(report.Font.Size);
                cell.Style.Font.Bold = true; // field.Font != null ? (bool)field.Font?.Bold : false;
                cell.Style.Alignment.SetWrapText(true);
                cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                cell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                //if (!string.IsNullOrEmpty(field.Format))
                //    cell.Style.NumberFormat.Format = field.Format;
                // Ajustar ancho de columna
                worksheet.Column(colIndex).Width = field.Width;
                //worksheet.Column(colIndex).AdjustToContents();
                /*switch (field.Alignment)
                {
                    case ReportAlignment.Center:
                        break;
                    case ReportAlignment.Right:
                        cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                        break;
                    default:
                        cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                        break;

                }*/

                // Bordes

                cell.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                cell.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                cell.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                cell.Style.Border.BottomBorder = XLBorderStyleValues.Thin;


                cell.Style.Fill.BackgroundColor = XLColor.FromHtml(field.HeaderColor ?? report.Body.HeaderColor ?? "#D0D0D0");

                colIndex++;
            }

            // Simulación de datos (puedes reemplazar esto con los datos reales)


            // Llenar datos
            int rowIndex = report.Body.InitialRow + 1;
            foreach (var record in data)
            {
                colIndex = report.Body.InitialColumn;
                foreach (var field in report.Body.Fields)
                {
                    //var recordDict = (IDictionary<string, object>)record;
                    var recordDict = record as IDictionary<string, object>;
                    if (field.FieldName != null && recordDict.ContainsKey(field.FieldName))
                    {
                        var cellValue = recordDict[field.FieldName];
                        worksheet.Cell(rowIndex, colIndex).Value = ConvertCellValue(cellValue);
                        worksheet.Cell(rowIndex, colIndex).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(rowIndex, colIndex).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(rowIndex, colIndex).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(rowIndex, colIndex).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(rowIndex, colIndex).Style.Font.FontName = report.Font?.Name ?? "Arial";
                        worksheet.Cell(rowIndex, colIndex).Style.Font.FontSize = 10; // float.Parse(report.Font.Size);
                        switch (field.Type)
                        {
                            default:

                                break;
                            case FieldType.MONEDA:
                                worksheet.Cell(rowIndex, colIndex).Style.NumberFormat.Format = "$#,##0";
                                break;
                            case FieldType.PORCENTAJE:
                                worksheet.Cell(rowIndex, colIndex).Style.NumberFormat.Format = "0%";
                                break;
                        }

                    }
                    colIndex++;
                }
                rowIndex++;
            }
          // if (data.Count > 0)
            //{
                CreateSectionHeader(worksheet, 
                                    report.Header, 
                                   (data != null && data.Count > 0)
                                    ? data[0]
                                    : new ExpandoObject());
                CreateSection(worksheet, rowIndex, data.Count, report.Body.InitialColumn, report.Body.FinalColumn, report.Footer, null);
           //}

            // Guardar archivo
            using (var stream = new MemoryStream())
            {
                workbook.SaveAs(stream); // Guardar en el stream
                return stream.ToArray(); // Convertir a arreglo de bytes
            }
        }

        private void CreateSection(IXLWorksheet worksheet, int initialRow, int detailCount, int initialColum, int finalColum, ReportSection reportSection, JObject data)
        {
            if (reportSection == null)
                return;
            var range = $"{GetColumnLetter(initialColum)}{initialRow}:{GetColumnLetter(finalColum)}{initialRow}";

            // Añadir bordes a varias celdas en la fila
            worksheet.Range(range).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            worksheet.Range(range).Style.Border.TopBorder = XLBorderStyleValues.Thin;
            worksheet.Range(range).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            worksheet.Range(range).Style.Border.RightBorder = XLBorderStyleValues.Thin;


            foreach (var field in reportSection.Fields)
            {
                var cell = worksheet.Cell(initialRow, field.Column);
                cell.Style.Font.Bold = field.Font != null ? field.Font.Bold : false;
                switch (field.Type)
                {
                    case FieldType.TEXT:
                        cell.Value = field.Value;
                        break;
                    case FieldType.FORMULA:
                        cell.FormulaA1 = !string.IsNullOrEmpty(field.Value) ?
                                            field.Value.Replace("[INITIAL]", (initialRow - detailCount).ToString())
                                                       .Replace("[FINAL]", (initialRow - 1).ToString()) : "";
                        cell.Style.NumberFormat.Format = "$#,##0";
                        break;
                }
                if (!string.IsNullOrEmpty(field.Format))
                    cell.Style.NumberFormat.Format = field.Format;

                switch (field.Alignment)
                {
                    case ReportAlignment.Center:
                        cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        break;
                    case ReportAlignment.Right:
                        cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                        break;
                    default:
                        cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                        break;

                }


                cell.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                cell.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                cell.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                cell.Style.Border.BottomBorder = XLBorderStyleValues.Thin;


                cell.Style.Fill.BackgroundColor = XLColor.FromHtml(field.CellColor ?? reportSection.BackgroundColor ?? "#FFFFFF");
            }


        }

        private void CreateSectionHeader(IXLWorksheet worksheet, ReportSection reportSection, dynamic data)
        {

            var dataDict = data as IDictionary<string, object>;

            foreach (var field in reportSection.Fields)
            {
                var cell = worksheet.Cell(field.Row, field.Column);
                cell.Style.Font.Bold = field.Font != null && field.Font.Bold;

                cell.Value = field.Value;
                if (!string.IsNullOrEmpty(field.FieldName) && dataDict.ContainsKey(field.FieldName))
                {
                    cell.Value = ConvertCellValue(dataDict[field.FieldName]);
                }


                if (!string.IsNullOrEmpty(field.Format))
                    cell.Style.NumberFormat.Format = field.Format;

                switch (field.Alignment)
                {
                    case ReportAlignment.Center:
                        cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        break;
                    case ReportAlignment.Right:
                        cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                        break;
                    default:
                        cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                        break;

                }


                cell.Style.Border.RightBorder = field?.Borders?.Right == true ? XLBorderStyleValues.Thin : XLBorderStyleValues.None;
                cell.Style.Border.LeftBorder = field?.Borders?.Left == true ? XLBorderStyleValues.Thin : XLBorderStyleValues.None;
                cell.Style.Border.TopBorder = field?.Borders?.Top == true ? XLBorderStyleValues.Thin : XLBorderStyleValues.None;
                cell.Style.Border.BottomBorder = field?.Borders?.Bottom == true ? XLBorderStyleValues.Thin : XLBorderStyleValues.None;



                cell.Style.Fill.BackgroundColor = XLColor.FromHtml(field.CellColor ?? reportSection.BackgroundColor ?? "#FFFFFF");
            }


        }

        private dynamic ConvertCellValue(object cellValue)
        {
            if (cellValue == null)
            {
                return string.Empty; // Devolver cadena vacía si es null
            }
            else if (cellValue is DateTime)
            {
                return (DateTime)cellValue; // Si es DateTime, devolverlo tal cual
            }
            else if (cellValue is int || cellValue is double || cellValue is decimal)
            {
                return Convert.ToDouble(cellValue); // Si es numérico, convertirlo a double
            }
            else if (cellValue is bool)
            {
                return (bool)cellValue; // Si es bool, devolverlo tal cual
            }
            else
            {
                return cellValue.ToString(); // Convertir cualquier otro tipo a string
            }
        }

        private static string GetColumnLetter(int columnNumber)
        {
            string columnLetter = string.Empty;
            while (columnNumber > 0)
            {
                int remainder = (columnNumber - 1) % 26;
                columnLetter = (char)(remainder + 'A') + columnLetter;
                columnNumber = (columnNumber - 1) / 26;
            }
            return columnLetter;
        }
    }
}