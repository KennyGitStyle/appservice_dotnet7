using Microsoft.Data.SqlClient;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;

namespace WpfResponsive
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string connString = "Data Source=(localdb)\\MSSQLLocalDB;" +
            "Initial Catalog=Northwind;" +
            "Integrated Security=true;" +
            "Encrypt=false;" +
            "MultipleActiveResultSets=true;";

        private const string sql = "WAITFOR DELAY '00:00:05';" +
  "SELECT EmployeeId, FirstName, LastName FROM Employees";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void GetEmployeesSyncButton_Click(object sender, RoutedEventArgs e)
        {
            Stopwatch timer = Stopwatch.StartNew();
            using(SqlConnection conn = new(connString))
            {
                try
                {
                    conn.Open();
                    SqlCommand command = new(sql, conn);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        string employee = string.Format("{0}: {1} {2}",
                            reader.GetInt32(0), 
                            reader.GetString(1), 
                            reader.GetString(2));
                        EmployeesListBox.Items.Add(employee);
                    }
                    reader.Close();
                    conn.Close();
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);
                }
            }
            EmployeesListBox.Items.Add($"Sync: {timer.ElapsedMilliseconds:N0}ms");
        }

        private async void GetEmployeesAsyncButton_Click(object sender, RoutedEventArgs e)
        {
            Stopwatch timer = Stopwatch.StartNew();
            using (SqlConnection conn = new(connString))
            {
                try
                {
                    await conn.OpenAsync();
                    SqlCommand command = new(sql, conn);
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        string employee = string.Format("{0}: {1} {2}",
                            await reader.GetFieldValueAsync<int>(0),
                            await reader.GetFieldValueAsync<string>(1),
                            await reader.GetFieldValueAsync<string>(2));
                        EmployeesListBox.Items.Add(employee);
                    }
                    await reader.CloseAsync();
                    await conn.CloseAsync();
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);
                }
            }
            EmployeesListBox.Items.Add($"Async: {timer.ElapsedMilliseconds:N0}ms");
        }
    }
}
