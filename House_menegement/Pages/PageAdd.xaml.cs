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

namespace House_menegement.Pages
{
    /// <summary>
    /// Логика взаимодействия для PageAdd.xaml
    /// </summary>
    public partial class PageAdd : Page
    {
        private Payment _currentPayment = new Payment();
        public PageAdd(Payment selectedPayment)
        {
            InitializeComponent();

      


            if (selectedPayment != null)

                _currentPayment = selectedPayment;

            Cmbid_apartment.ItemsSource = House__managementEntities.GetContext().Apartment.ToList();
            Cmbid_apartment.SelectedValuePath = "id_apartment";
            Cmbid_apartment.DisplayMemberPath = "square";

            Cmbview_Code.ItemsSource = House__managementEntities.GetContext().payment_type.Distinct().ToList();
            Cmbview_Code.SelectedValuePath = "view_Code";
            Cmbview_Code.DisplayMemberPath = "name";
            DataContext = _currentPayment;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(_currentPayment.month_and_year_of_payment))
                errors.AppendLine("Укажите Месяц и год оплаты буквами");
            if (string.IsNullOrWhiteSpace(_currentPayment.payment_amount))
                errors.AppendLine("Укажите Месяц и год оплаты");
            if (string.IsNullOrWhiteSpace(_currentPayment.date_of_payment))
                errors.AppendLine("Укажите дату оплаты числами");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }
            if (_currentPayment.payment_code == 0)
                House__managementEntities.GetContext().Payment.Add(_currentPayment);
            
            try
            {
                House__managementEntities.GetContext().SaveChanges();
                MessageBox.Show("Информация сохранена!");
                Classes.ClassFrame.frmObj.Navigate(new Pages.Page1());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());

            }
        }
    }
}
