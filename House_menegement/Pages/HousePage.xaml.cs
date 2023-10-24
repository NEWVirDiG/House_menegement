using House_menegement.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Excel = Microsoft.Office.Interop.Excel;
using Word = Microsoft.Office.Interop.Word;

namespace House_menegement.Pages
{
    /// <summary>
    /// Логика взаимодействия для HousePage.xaml
    /// </summary>
    public partial class HousePage : Page
    {
        public HousePage()
        {
            InitializeComponent();

            var currentPayment = House__managementEntities.GetContext().Payment.ToList();
            LViewHouse.ItemsSource = currentPayment;
            DataContext = LViewHouse;
            CmbFiltr.Items.Add("Все пользователи");
            foreach (var item in House__managementEntities.GetContext().Payment.
              Select(x => x.Management_Company).Distinct().ToList())
                CmbFiltr.Items.Add(item);


            //allPayment.Insert(0, new Payment
            //{
            //    month_and_year_of_payment = "Все типы"
            //});
            //ComboType.ItemsSource= allPayment;

            //var currentHouse = House__managementEntities.GetContext().Payment.ToList();
            //LViewHouse.ItemsSource = currentHouse;
        }

        private void TBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        

        private void CheckActual_Checked(object sender, RoutedEventArgs e)
        {

        }

       //private void UpdatePayment()
       // {
       //     var currentPayment = House__managementEntities.GetContext();Payment.ToList();

       //     if (ComboType.SelectedIndex > 0) 
       //         currentPayment  = currentPayment.Where(p = >.Types.Contains(ComboType.SelectedItem as string)).ToList();    
       // }

        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            string search = TxTSearch.Text;
            if (TxTSearch.Text != null)
            {
                LViewHouse.ItemsSource = House__managementEntities.GetContext().Payment.
                    Where(x => x.month_and_year_of_payment.Contains(search)
                    || x.payment_amount.Contains(search)
                    || x.date_of_payment.Contains(search)
                    //|| x.payment_type.name.ToString().Contains(search)
                    || x.Management_Company.ToString().Contains(search)).ToList();

                

            }
        }

        private void RbUp_Checked(object sender, RoutedEventArgs e)
        {
            LViewHouse.ItemsSource = House__managementEntities.GetContext().Payment.
                OrderBy(x => x.Management_Company).ToList();
        }

        private void RbDown_Checked(object sender, RoutedEventArgs e)
        {
            LViewHouse.ItemsSource = House__managementEntities.GetContext().Payment.
               OrderByDescending(x => x.Management_Company).ToList();
        }

        private void CmbFiltr_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           if (CmbFiltr.SelectedValue.ToString() == "Вск пользователи")
            {
                LViewHouse.ItemsSource = House__managementEntities.GetContext().Payment.ToList();
            }
           else
            {
                LViewHouse.ItemsSource = House__managementEntities.GetContext().Payment.
                    Where(x =>x.Management_Company == CmbFiltr.SelectedValue.ToString()).ToList();
            }
        }

        private void BtnSaveToExcel_Click(object sender, RoutedEventArgs e)
        {
            //объект Excel
            var app = new Excel.Application();

            //книга 
            Excel.Workbook wb = app.Workbooks.Add();
            //лист
            Excel.Worksheet worksheet = app.Worksheets.Item[1];
            int indexRows = 1;
            //ячейка
            worksheet.Cells[1][indexRows] = "Номер";
            worksheet.Cells[2][indexRows] = "Управляющая комания";
            worksheet.Cells[3][indexRows] = "Дата";
            worksheet.Cells[4][indexRows] = "Площадь";
            worksheet.Cells[5][indexRows] = "Тип оплаты";

            //список пользователей из таблицы после фильтрации и поиска
            var printItems = LViewHouse.Items;
            //цикл по данным из списка для печати
            foreach (Payment item in printItems)
            {
                worksheet.Cells[1][indexRows + 1] = indexRows;
                worksheet.Cells[2][indexRows + 1] = item.Management_Company;
                worksheet.Cells[3][indexRows + 1] = item.month_and_year_of_payment;
                worksheet.Cells[4][indexRows + 1] = item.Apartment.square;
                worksheet.Cells[5][indexRows + 1].Value = item.payment_type.name.ToString();

                indexRows++;
            }
            Excel.Range range = worksheet.Range[worksheet.Cells[2][indexRows + 1],
                    worksheet.Cells[5][indexRows + 1]];
            range.ColumnWidth = 30; //ширина столбцов
            range.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;//выравнивание по левому краю

            //показать Excel
            app.Visible = true;
        }

        private void BtnSaveToExcelTemplate_Click(object sender, RoutedEventArgs e)
        {
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook wb = excelApp.Workbooks.Open($"{Directory.GetCurrentDirectory()}\\Шаблон.xlsx");
            Excel.Worksheet ws = (Excel.Worksheet)wb.Worksheets[1];
            ws.Cells[4, 2] = DateTime.Now.ToString();
            ws.Cells[4, 5] = 7;
            int indexRows = 6;
            //ячейка
            ws.Cells[1][indexRows] = "Номер";
            ws.Cells[2][indexRows] = "Управляющая комания";
            ws.Cells[3][indexRows] = "Дата";
            ws.Cells[4][indexRows] = "Площадь";
            ws.Cells[5][indexRows] = "Тип оплаты";

            //список пользователей из таблицы после фильтрации и поиска
            var printItems = LViewHouse.Items;
            //цикл по данным из списка для печати
            foreach (Payment item in printItems)
            {
                ws.Cells[1][indexRows + 1] = indexRows;
                ws.Cells[2][indexRows + 1] = item.Management_Company;
                ws.Cells[3][indexRows + 1] = item.month_and_year_of_payment;
                ws.Cells[4][indexRows + 1] = item.Apartment.square;
                ws.Cells[5][indexRows + 1].Value = item.payment_type.name.ToString();

                indexRows++;
            }
            ws.Cells[indexRows + 2, 3] = "Подпись";
            ws.Cells[indexRows + 2, 5] = "Калинин В.А.";
            excelApp.Visible = true;
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            ClassFrame.frmObj.Navigate(new PageAdd((sender as System.Windows.Controls.Button).DataContext as Payment));
        }

        private void Btnescape_Click(object sender, RoutedEventArgs e)
        {
            ClassFrame.frmObj.Navigate(new Pages.Page1());
        }

        private void ComboType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

     
        }

        private void Btnreset_Click(object sender, RoutedEventArgs e)
        {
            LViewHouse.ItemsSource = House__managementEntities.GetContext().Payment.ToList();
        }

        private void BtnSaveToWord_Click(object sender, RoutedEventArgs e)
        {
            var allEmployees = House__managementEntities.GetContext().Payment.ToList();

            var application = new Word.Application();

            Word.Document document = application.Documents.Add();

            Word.Paragraph empParagraph = document.Paragraphs.Add();
            Word.Range empRange = empParagraph.Range;
            empRange.Text = "Сотрудники";
            empRange.Font.Bold = 4;
            empRange.Font.Italic = 4;
            empRange.Font.Color = Word.WdColor.wdColorBlue;
            empRange.InsertParagraphAfter();

            Word.Paragraph tableParagraph = document.Paragraphs.Add();
            Word.Range tableRange = tableParagraph.Range;
            Word.Table paymentsTable = document.Tables.Add(tableRange, allEmployees.Count() + 1, 4);
            paymentsTable.Borders.InsideLineStyle = paymentsTable.Borders.OutsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;
            paymentsTable.Range.Cells.VerticalAlignment = Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;

            Word.Range cellRange;

            cellRange = paymentsTable.Cell(1, 1).Range;
            cellRange.Text = "ФИО";
            cellRange = paymentsTable.Cell(1, 2).Range;
            cellRange.Text = "Адрес";
            cellRange = paymentsTable.Cell(1, 3).Range;
            cellRange.Text = "Номер телефона";
            cellRange = paymentsTable.Cell(1, 4).Range;
            cellRange.Text = "Оклад";

            paymentsTable.Rows[1].Range.Bold = 1;
            paymentsTable.Rows[1].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;

            for (int i = 0; i < allEmployees.Count(); i++)
            {
                var currentEmployee = allEmployees[i];

                //cellRange = paymentsTable.Cell(i + 2, 1).Range;
                //Word.InlineShape imageShape = cellRange.InlineShapes.AddPicture(AppDomain.CurrentDomain.BaseDirectory
                //    + "..\\..\\" + currentEmployee.photo);
                //imageShape.Width = imageShape.Height = 40;
                //cellRange.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                cellRange = paymentsTable.Cell(i + 2, 1).Range;
                cellRange.Text = currentEmployee.month_and_year_of_payment;

                cellRange = paymentsTable.Cell(i + 2, 2).Range;
                cellRange.Text = currentEmployee.payment_amount;

                cellRange = paymentsTable.Cell(i + 2, 3).Range;
                cellRange.Text = currentEmployee.IsActual;

                cellRange = paymentsTable.Cell(i + 2, 4).Range;
                cellRange.Text = currentEmployee.Management_Company.ToString();
            }
            Payment maxSalary = House__managementEntities.GetContext().Payment
                .OrderByDescending(p => p.Management_Company).FirstOrDefault();
            if (maxSalary != null)
            {
                Word.Paragraph maxSalaryParagraph = document.Paragraphs.Add();
                Word.Range maxSalaryRange = maxSalaryParagraph.Range;
                maxSalaryRange.Text = $"Самая дорогая управляющая компания - {maxSalary.Management_Company}";
                maxSalaryRange.Font.Color = Word.WdColor.wdColorDarkRed;
                maxSalaryRange.InsertParagraphAfter();
            }

            Payment minSalary = House__managementEntities.GetContext().Payment
                .OrderBy(p => p.Management_Company).FirstOrDefault();
            if (minSalary != null)
            {
                Word.Paragraph minSalaryParagraph = document.Paragraphs.Add();
                Word.Range minSalaryRange = minSalaryParagraph.Range;
                minSalaryRange.Text = $"Самая дешевая управляющая компания - {minSalary.Management_Company}";
                minSalaryRange.Font.Color = Word.WdColor.wdColorDarkGreen;
                minSalaryRange.InsertParagraphAfter();
            }

            application.Visible = true;

            document.SaveAs2(@"D:\ИСП.21.1А Калинин и Девяткин\ПРАКТИКА\12 10 23 новый влад\House_menegement\House_menegement\bin\Debug\Test.docx");
        }

        private void BtnSaveToPDF_Click(object sender, RoutedEventArgs e)
        {
            var allEmployees = House__managementEntities.GetContext().Payment.ToList();

            var application = new Word.Application();

            Word.Document document = application.Documents.Add();

            Word.Paragraph empParagraph = document.Paragraphs.Add();
            Word.Range empRange = empParagraph.Range;
            empRange.Text = "Сотрудники";
            empRange.Font.Bold = 4;
            empRange.Font.Italic = 4;
            empRange.Font.Color = Word.WdColor.wdColorBlue;
            empRange.InsertParagraphAfter();

            Word.Paragraph tableParagraph = document.Paragraphs.Add();
            Word.Range tableRange = tableParagraph.Range;
            Word.Table paymentsTable = document.Tables.Add(tableRange, allEmployees.Count() + 1, 4);
            paymentsTable.Borders.InsideLineStyle = paymentsTable.Borders.OutsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;
            paymentsTable.Range.Cells.VerticalAlignment = Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;

            Word.Range cellRange;

            cellRange = paymentsTable.Cell(1, 1).Range;
            cellRange.Text = "ФИО";
            cellRange = paymentsTable.Cell(1, 2).Range;
            cellRange.Text = "Адрес";
            cellRange = paymentsTable.Cell(1, 3).Range;
            cellRange.Text = "Номер телефона";
            cellRange = paymentsTable.Cell(1, 4).Range;
            cellRange.Text = "Оклад";

            paymentsTable.Rows[1].Range.Bold = 1;
            paymentsTable.Rows[1].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;

            for (int i = 0; i < allEmployees.Count(); i++)
            {
                var currentEmployee = allEmployees[i];

                //cellRange = paymentsTable.Cell(i + 2, 1).Range;
                //Word.InlineShape imageShape = cellRange.InlineShapes.AddPicture(AppDomain.CurrentDomain.BaseDirectory
                //    + "..\\..\\" + currentEmployee.photo);
                //imageShape.Width = imageShape.Height = 40;
                //cellRange.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                cellRange = paymentsTable.Cell(i + 2, 1).Range;
                cellRange.Text = currentEmployee.month_and_year_of_payment;

                cellRange = paymentsTable.Cell(i + 2, 2).Range;
                cellRange.Text = currentEmployee.payment_amount;

                cellRange = paymentsTable.Cell(i + 2, 3).Range;
                cellRange.Text = currentEmployee.IsActual;

                cellRange = paymentsTable.Cell(i + 2, 4).Range;
                cellRange.Text = currentEmployee.Management_Company.ToString();
            }
            Payment maxSalary = House__managementEntities.GetContext().Payment
                .OrderByDescending(p => p.Management_Company).FirstOrDefault();
            if (maxSalary != null)
            {
                Word.Paragraph maxSalaryParagraph = document.Paragraphs.Add();
                Word.Range maxSalaryRange = maxSalaryParagraph.Range;
                maxSalaryRange.Text = $"Самая дорогая управляющая компания - {maxSalary.Management_Company}";
                maxSalaryRange.Font.Color = Word.WdColor.wdColorDarkRed;
                maxSalaryRange.InsertParagraphAfter();
            }

            Payment minSalary = House__managementEntities.GetContext().Payment
                .OrderBy(p => p.Management_Company).FirstOrDefault();
            if (minSalary != null)
            {
                Word.Paragraph minSalaryParagraph = document.Paragraphs.Add();
                Word.Range minSalaryRange = minSalaryParagraph.Range;
                minSalaryRange.Text = $"Самая дешевая управляющая компания - {minSalary.Management_Company}";
                minSalaryRange.Font.Color = Word.WdColor.wdColorDarkGreen;
                minSalaryRange.InsertParagraphAfter();
            }

            application.Visible = true;

            document.SaveAs2(@"D:\ИСП.21.1А Калинин и Девяткин\ПРАКТИКА\12 10 23 новый влад\House_menegement\House_menegement\bin\Debug\Test.pdf", Word.WdExportFormat.wdExportFormatPDF);

        }

        private void BtnDiagram_Click(object sender, RoutedEventArgs e)
        {
            ClassFrame.frmObj.Navigate(new Pages.PageDiagram());
        }
    }
}
