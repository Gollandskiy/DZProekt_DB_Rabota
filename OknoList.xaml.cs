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
using System.Windows.Shapes;

namespace DZProekt
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class OknoList : Window
    {

        public UserGroups? UserGroup2 { get; set; }   // посилання на редаговану групу



        public OknoList(UserGroups productGroup3)
        {
            InitializeComponent();
            this.UserGroup2 = productGroup3;
            this.DataContext = this.UserGroup2;
        }



        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }



        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {

            DialogResult = true;
        }



        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            UserGroup2 = null!;
            Close();
        }
    }
}
