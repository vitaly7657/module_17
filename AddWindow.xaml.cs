using System.Windows;

namespace module_17
{
    /// <summary>
    /// Логика взаимодействия для AddWindow.xaml
    /// </summary>
    public partial class AddWindow : Window
    {
        public AddWindow()
        {
            InitializeComponent();
        }

        public AddWindow(Clients cl):this()
        {
            cancel_add_clien_button.Click += delegate //действия кнопки отмены
            {
                this.DialogResult = false; //результат действий в данном окне
            };
            add_client_button.Click += delegate //декйствия кнопки добавления клиента
            {
                //определение свойств нового клиента
                cl.Client_surname = txt_Client_surname.Text;
                cl.Client_name = txt_Client_name.Text;
                cl.Client_patronymic = txt_Client_patronymic.Text;
                cl.Phone_number = txt_Phone_number.Text;
                cl.Email = txt_Email.Text;
                this.DialogResult = !false; //результат действий в данном окне
            };
        }
    }
}
