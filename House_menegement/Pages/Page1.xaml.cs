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
    /// Логика взаимодействия для Page1.xaml
    /// </summary>
    public partial class Page1 : Page
    {
        public Page1()
        {
            InitializeComponent();

            dtgPayment.ItemsSource = House__managementEntities.GetContext().Payment.ToList();
            Cmbdate_of_payment.ItemsSource = House__managementEntities.GetContext().Payment.Select(x => x.date_of_payment).Distinct().ToList();
            Cmbpayment_amount.ItemsSource = House__managementEntities.GetContext().Payment.Select(x => x.payment_amount).ToList();
        }

        private void Cmbdate_of_payment_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string Date_of_payment = Cmbdate_of_payment.SelectedValue.ToString();
            dtgPayment.ItemsSource = House__managementEntities.GetContext().Payment.Where(x => x.date_of_payment == Date_of_payment).ToList();
        }

        private void TxTSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            string search = TxTSearch.Text;
            dtgPayment.ItemsSource = House__managementEntities.GetContext().Payment.Where(x => x.month_and_year_of_payment.Contains(search)).ToList();
        }

        private void BtnResetFiltr_Click(object sender, RoutedEventArgs e)
        {
            dtgPayment.ItemsSource = House__managementEntities.GetContext().Payment.ToList();
        }

        private void Cmbpayment_amount_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string Payment_amount = Cmbpayment_amount.SelectedValue.ToString();
            dtgPayment.ItemsSource = House__managementEntities.GetContext().Payment.Where(x => x.payment_amount == Payment_amount).ToList();
        }

        private void BtnDel_Click(object sender, RoutedEventArgs e)
        {
            var Remove = dtgPayment.SelectedItems.Cast<Payment>().ToList();

            if (MessageBox.Show($"Вы точно хотите удалить следующие {Remove.Count()} элементов?", "Внимание",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    House__managementEntities.GetContext().Payment.RemoveRange(Remove);
                    House__managementEntities.GetContext().SaveChanges();
                    MessageBox.Show("Данные удалены!");

                    dtgPayment.ItemsSource = House__managementEntities.GetContext().Payment.ToList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            ClassFrame.frmObj.Navigate(new Pages.PageAdd(null));
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                House__managementEntities.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                dtgPayment.ItemsSource = House__managementEntities.GetContext().Payment.ToList();
            }
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            
            
            ClassFrame.frmObj.Navigate(new Pages.PageAdd((sender as Button).DataContext as Payment));
        }

        private void BtnList_Click(object sender, RoutedEventArgs e)
        {
            ClassFrame.frmObj.Navigate(new Pages.HousePage());
        }
    }
}
