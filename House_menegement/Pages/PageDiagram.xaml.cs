using House_menegement.Classes;
using System;
using System.Collections.Generic;
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
using System.Windows.Forms.DataVisualization.Charting;


namespace House_menegement.Pages
{
    /// <summary>
    /// Логика взаимодействия для PageDiagram.xaml
    /// </summary>
    public partial class PageDiagram : Page
    {
        //private House__managementEntities _context = new House__managementEntities();
        public PageDiagram()
        {
            InitializeComponent();
            ChartPayments.ChartAreas.Add(new ChartArea("Main"));

            var currentSeries = new Series("Payments")
            {
                IsValueShownAsLabel = true
            };
            ChartPayments.Series.Add(currentSeries);

            ComboUser.ItemsSource = House__managementEntities.GetContext().Payment.ToList();
            ComboChartTypes.ItemsSource = Enum.GetValues(typeof(SeriesChartType));
        }

        private void UpdateChart(object sender, SelectionChangedEventArgs e)
        {
            if (ComboUser.SelectedItem is Payment currentUser &&
                ComboChartTypes.SelectedItem is SeriesChartType currentType)
            {
                Series currentSeries = ChartPayments.Series.FirstOrDefault();
                currentSeries.ChartType = currentType;
                currentSeries.Points.Clear();
                var categoriesList = House__managementEntities.GetContext().Payment.ToList();
                foreach (var category in categoriesList)
                {
                    currentSeries.Points.AddXY(category.Management_Company, category.buy_amount);
                    //currentSeries.Points.AddXY(category.name,
                    //    House__managementEntities.GetContext().Payment.ToList().Where(p => p.payment_type == currentUser
                    //    && p. == category).Sum(p => p.buy_amount * p.buy_amount));
                }
            }
        }

        private void Btnescape_Click(object sender, RoutedEventArgs e)
        {
            ClassFrame.frmObj.Navigate(new Pages.HousePage());
        }
    }
}