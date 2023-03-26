using System;
using System.Data.OleDb;
using System.Data;
using System.Windows;
using System.Data.Entity;


namespace module_17
{
    
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        LocalDBEntities1 context; //объявление контекста
        #region AccessDb start param
        OleDbConnection connectionAccess;
        OleDbDataAdapter dataAdapterAccess;
        DataTable dataTableAccess;
        DataRowView rowViewAccess;
        #endregion AccessDb start param
        public MainWindow()
        {
            InitializeComponent();
            //начальное состояние кнопок управления данными товаров
            add_product_button.IsEnabled = false;
            delete_product_button.IsEnabled = false;
            context = new LocalDBEntities1(); //создание контекста
        }

        private void load_clients_button_Click(object sender, RoutedEventArgs e) //загрузка клиентов
        {
            context.Clients.Load(); //загрузка таблицы клиентов
            gridView.ItemsSource = context.Clients.Local.ToBindingList<Clients>(); //привязка данных базы к таблице
        }

        private void open_AddWindow_button_Click(object sender, RoutedEventArgs e) //открытие окна добавления новых клиентов
        {
            Clients cl = new Clients(); //создание буферного клиента
            AddWindow add = new AddWindow(cl); //создание нового окна AddWindow  
            add.ShowDialog(); //открытие окна AddWindow
            if (add.DialogResult.Value) //проверка действий в окне AddWindow
            {
                context.Clients.Add(cl); //добавление нового клиента из данных ранее открытого окна
            }
            context.SaveChanges(); //сохранение данных контекста
        }
        
        private void delete_client_button_Click(object sender, RoutedEventArgs e) //удаление выбранного клиента
        {
            if (gridView.SelectedItem != null)
            {
                var selectedClient = gridView.SelectedItem as Clients; //опредение выбранного элемента таблицы
                context.Clients.Remove(selectedClient); //удаление клиента из базы
                context.SaveChanges(); //сохранение данных контекста
            }
            else
            {
                MessageBox.Show("Выберите клиента для удаления");
            }
        }
        private void gridView_CurrentCellChanged(object sender, EventArgs e) //действие при редактировании клиента
        {
            context.SaveChanges(); //сохранение данных контекста
        }
        #region AccessDB
        string loginDB;
        string passwordDB;

        private void update_accessDB_Click(object sender, RoutedEventArgs e)
        {
            if (login_tb.Text == "Admin" && pass_tb.Text == "123")
            {
                add_product_button.IsEnabled = true;
                delete_product_button.IsEnabled = true;
                loginDB = login_tb.Text.ToString();
                passwordDB = pass_tb.Text.ToString();
                AccessDB_Connection();
            }
            else
            {
                MessageBox.Show("Неверные данные");
            }
        }

        public void AccessDB_Connection()
        {

            try
            {
                //строка подключения к базе
                var connectionStringAccess = new OleDbConnectionStringBuilder()
                {
                    Provider = "Microsoft.ACE.OLEDB.12.0",
                    DataSource = @"AccessDB.accdb"
                };

                //ввод логина
                connectionStringAccess.Add("User ID", loginDB);

                //ввод пароля
                connectionStringAccess.Add("Jet OLEDB:Database Password", passwordDB);

                //создание подключения к базе
                connectionAccess = new OleDbConnection(connectionStringAccess.ConnectionString);

                //запись новой таблицы в память
                dataTableAccess = new DataTable();

                //создание нового адаптера с командами для подключения к базе Access
                dataAdapterAccess = new OleDbDataAdapter();

                //SELECT---------------------
                var sqlAccess = @"select * from Orders";
                dataAdapterAccess.SelectCommand = new OleDbCommand(sqlAccess, connectionAccess);

                //INSERT---------------------
                sqlAccess = @"INSERT INTO Orders(Email_access,Product_code,Product_name)
                            VALUES (@Email_access,@Product_code,@Product_name);";

                dataAdapterAccess.InsertCommand = new OleDbCommand(sqlAccess, connectionAccess);
                dataAdapterAccess.InsertCommand.Parameters.Add("@Email_access", OleDbType.WChar, 50, "Email_access");
                dataAdapterAccess.InsertCommand.Parameters.Add("@Product_code", OleDbType.WChar, 50, "Product_code");
                dataAdapterAccess.InsertCommand.Parameters.Add("@Product_name", OleDbType.WChar, 50, "Product_name");

                //DELETE---------------------
                sqlAccess = "delete from Orders where Id_access = @Id_access";
                dataAdapterAccess.DeleteCommand = new OleDbCommand(sqlAccess, connectionAccess);
                dataAdapterAccess.DeleteCommand.Parameters.Add("Id_access", OleDbType.Integer, 4, "Id_access");

                //заполнение методом Fill таблицы dataTableAccess из базы
                dataAdapterAccess.Fill(dataTableAccess);

                //заполнение таблицы на форме
                gridViewAccess.DataContext = dataTableAccess.DefaultView;
            }

            catch (Exception ex)
            {
                MessageBox.Show($"AccessDB error: {ex.Message}");
            }          
        }
        

        private void add_product_button_Click(object sender, RoutedEventArgs e)
        {
            DataRow r = dataTableAccess.NewRow();
            AddWindowAccess add = new AddWindowAccess(r);
            add.ShowDialog();

            if (add.DialogResult.Value)
            {
                dataTableAccess.Rows.Add(r);
                dataAdapterAccess.Update(dataTableAccess);
            }
            dataAdapterAccess.Update(dataTableAccess);
            dataTableAccess.Clear();
            dataAdapterAccess.Fill(dataTableAccess);
        }

        private void delete_product_button_Click(object sender, RoutedEventArgs e)
        {
            rowViewAccess = (DataRowView)gridViewAccess.SelectedItem;
            rowViewAccess.Row.Delete();
            dataAdapterAccess.Update(dataTableAccess);
            dataTableAccess.Clear();
            dataAdapterAccess.Fill(dataTableAccess);
        }

        private void help_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Логин Admin, пароль 123");
        }
        #endregion  AccessDB
    }
}
