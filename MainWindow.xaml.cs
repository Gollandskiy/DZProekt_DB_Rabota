
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
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

namespace DZProekt
{
    public partial class MainWindow : Window
    {
        private SqlConnection connection;
        public ObservableCollection<String> columns { get; set; } = new();
        public ObservableCollection<UserGroups> GroupUser { get; set; } = new();

        public MainWindow()
        {
            InitializeComponent();
            connection = null!;
            this.DataContext = this;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                connection = new(App.ConnectString);
                connection.Open();
                LoadGroups();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Close();
            }
        }

        private void LoadGroups()
        {
            using SqlCommand command = new();
            command.Connection = connection;
            command.CommandText = "SELECT * FROM GroupUser";
            try
            {
                using SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    GroupUser.Add(new UserGroups
                    {
                        Id = reader.GetGuid(0),
                        Name = reader.GetString(1),
                        Pass = reader.GetString(2),
                        Picture = reader.GetString(3)
                    });
                    // columns.Add(
                    // $"Id: {reader.GetGuid(0).ToString()[..4]},\n Name: {reader.GetString(1)},\n Desc: {reader.GetString(2)},\n Picture: {reader.GetString(3)}");
                }
                //reader.GetGuid(0);
                //reader.GetGuid("Id");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Query Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            connection?.Dispose();
        }

        private void CreateGroup_Click(object sender, RoutedEventArgs e)
        {
            using SqlCommand command = new();
            command.Connection = connection;
            command.CommandText =
                 @"CREATE TABLE GroupUser (
                    Id            UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
                    Name        NVARCHAR(50)     NOT NULL,
                    Pass           NTEXT            NOT NULL,
                    Picture     NVARCHAR(50)     NULL
                                                        )";
            try
            {
                command.ExecuteNonQuery();
                MessageBox.Show("Table Created!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Create Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void InsertGroup_Click(object sender, RoutedEventArgs e)
        {
            using SqlCommand command = new();
            command.Connection = connection;
            command.CommandText =
                @"INSERT INTO GroupUser
                    ( Id, Name, Pass, Picture )
                                                     VALUES
                    ( NEWID(), N'Golland',     N'623459aaa', N'Ship.png' ),
                    ( NEWID(), N'Denoro',   N'123321asdf', N'Animeshnik.jpg' ),
                    ( NEWID(), N'Pampers228',  N'YaBest22234', N'Minecraft.jpg' ),
                    ( NEWID(), N'NePridymalNik123', N'NePomnyBlyaaaa', N'Black.jpg' ),
                    ( NEWID(), N'Xxx_Nagibator_xxX', N'YaNagibayVSEX!', N'KrytoiVOchkax.jpg' )";
            try
            {
                command.ExecuteNonQuery();
                MessageBox.Show("Table Created!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Insert Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void GroupCount_Click(object sender, RoutedEventArgs e)
        {
            using SqlCommand command = new();
            command.Connection = connection;
            command.CommandText = "SELECT COUNT(*) FROM GroupUser";
            try
            {
                int cnt = Convert.ToInt32(command.ExecuteScalar());
                MessageBox.Show($"Table has {cnt} rows");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Query Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListViewItem item)
            {
                if (item.Content is UserGroups group)
                {
                    MessageBox.Show(group.Name);
                }
                //var group = item.Content as ProductGroups;
                //if (group is not null)
                //{
                //
                //}
            }
        }
    }
}
