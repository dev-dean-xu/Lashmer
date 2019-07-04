//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Linq;
//using System.Text.RegularExpressions;
//using System.Threading.Tasks;
//using DocumentFormat.OpenXml.Packaging;
//using DocumentFormat.OpenXml.Spreadsheet;

//namespace LashmerAdmin.Services
//{
//    public class ExcelReader
//    {
//        private static string GetCellValue(SpreadsheetDocument document, CellType cell)
//        {
//            string cellValue = string.Empty;
//            var stringTablePart = document.WorkbookPart.SharedStringTablePart;
//            var value = "";
//            if (cell.CellValue != null)
//            {
//                value = cell.CellValue.InnerXml;
//            }

//            if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
//            {
//                double valDouble;
//                string cellText = stringTablePart.SharedStringTable.ChildElements[int.Parse(value)].InnerText;
//                var chkDobule = double.TryParse(cellText, out valDouble);

//                //BP-2282: Prod: D30 Babywear cannot upload packs due to dept size profiles
//                cellValue = (chkDobule && valDouble != 0) ? valDouble.ToString() : cellText;
//            }
//            else
//            {
//                double valDouble;
//                var chkDobule = double.TryParse(value, out valDouble);
//                cellValue = chkDobule ? valDouble.ToString() : value;
//            }
//            return cellValue.Trim();
//        }

//        public static DataTable ParseExcelFile(string filepath, string strSheetName = "")
//        {
//            var result = new DataTable();

//            using (var spreadSheetDocument = SpreadsheetDocument.Open(filepath, false))
//            {
//                var sheets = spreadSheetDocument.WorkbookPart.Workbook.GetFirstChild<Sheets>().Elements<Sheet>();

//                var relationshipId = string.IsNullOrEmpty(strSheetName) ? sheets.First().Id.Value : (from sh in sheets where sh.Name == strSheetName select sh.Id.Value).FirstOrDefault();

//                var worksheetPart = (WorksheetPart)spreadSheetDocument.WorkbookPart.GetPartById(relationshipId);
//                var workSheet = worksheetPart.Worksheet;
//                var sheetData = workSheet.GetFirstChild<SheetData>();
//                var rows = sheetData.Descendants<Row>().ToList();

//                foreach (var openXmlElement in rows.ElementAt(0))
//                {
//                    var cell = (Cell)openXmlElement;
//                    result.Columns.Add(GetCellValue(spreadSheetDocument, cell));
//                }

//                var fixrows = new FixExcelRows();

//                foreach (var row in rows)
//                {
//                    var tempRow = result.NewRow();
//                    var breakFlag = false;
//                    var getElement = 0;
//                    for (var i = 0; i < result.Columns.Count; i++)
//                    {
//                        var colValue = "";
//                        try
//                        {
//                            var cell = row.Descendants<Cell>().ElementAt(getElement);
//                            var columnName = fixrows.GetColumnName(cell.CellReference);

//                            var currentColumnIndex = fixrows.ConvertColumnNameToNumber(columnName);

//                            if (currentColumnIndex == i)
//                            {
//                                colValue = GetCellValue(spreadSheetDocument, row.Descendants<Cell>().ElementAt(getElement));
//                                getElement++;
//                            }
//                            else
//                            {
//                                colValue = "";
//                            }
//                        }
//                        catch (Exception)
//                        {
//                            // ignored
//                        }

//                        if ((i == 0) && string.IsNullOrEmpty(colValue))
//                        {
//                            breakFlag = true;
//                            break;
//                        }
//                        tempRow[i] = colValue;
//                    }
//                    if (breakFlag) break;

//                    result.Rows.Add(tempRow);
//                }

//            }
//            //remove headers
//            result.Rows.RemoveAt(0);

//            return result;
//        }

//        private class FixExcelRows
//        {
//            Row _excelRow = new Row();

//            private IEnumerator<Cell> GetEnumerator()
//            {
//                var currentCount = 0;

//                foreach (var cell in _excelRow.Descendants<Cell>())
//                {
//                    var columnName = GetColumnName(cell.CellReference);

//                    var currentColumnIndex = ConvertColumnNameToNumber(columnName);

//                    for (; currentCount < currentColumnIndex; currentCount++)
//                    {
//                        yield return new Cell();
//                    }

//                    yield return cell;
//                    currentCount++;
//                }
//            }

//            public string GetColumnName(string cellReference)
//            {
//                var regex = new Regex("[A-Za-z]+");
//                var match = regex.Match(cellReference);

//                return match.Value;
//            }

//            public int ConvertColumnNameToNumber(string columnName)
//            {
//                if (!Regex.IsMatch(columnName, "^[A-Z]+$"))
//                    throw new ArgumentException();

//                var colLetters = columnName.ToCharArray();
//                Array.Reverse(colLetters);

//                var convertedValue = 0;
//                for (var i = 0; i < colLetters.Length; i++)
//                {
//                    var letter = colLetters[i];
//                    var current = i == 0 ? letter - 65 : letter - 64;
//                    convertedValue += current * (int)Math.Pow(26, i);
//                }

//                return convertedValue;
//            }
//        }
//    }
//}
