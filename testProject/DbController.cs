using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Data.OleDb;
using System.Windows.Forms;
using System.Resources;
using System.CodeDom;
using System.Collections;
using System.Data;

namespace testProject
{
    class DbController
    {
        public static string connectString;

        public static OleDbConnection myConnection;

        public static DataTable table = new DataTable();

        public static DataSet dataSet = new DataSet();

        public static OleDbDataAdapter adapter = new OleDbDataAdapter();

        public static OleDbCommand command = new OleDbCommand("", myConnection);

        public static DataGridView dataGridView = new DataGridView();

        public static string createBirthDayFolderNDataBase(string folderName, string fileName)
        {
            try
            {
                DirectoryInfo dirInfo = new DirectoryInfo(folderName);
                if (!dirInfo.Exists)
                    dirInfo.Create();
                folderName += '\\' + fileName;

                ADOX.Catalog BD = new ADOX.Catalog();
                BD.Create($"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={folderName}.mdb;Jet OLEDB:Engine Type=5");

                myConnection = new OleDbConnection($"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={folderName}.mdb");

                myConnection.Open();
                OleDbCommand com = new OleDbCommand();

                //Создание Таблицы Турнира
                com = new OleDbCommand("CREATE TABLE BirthDay(ID COUNTER, Фамилия CHAR(50),Имя CHAR(50), Дата_Рождения DATETIME)", myConnection);
                com.ExecuteNonQuery();
                myConnection.Close();
                return ("Connected");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                return ("Disconnected");
            }
        }
        public static string connectToDataBase(string fiellPath)
        {
            try
            {
                myConnection = new OleDbConnection($"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={fiellPath}");
                if (myConnection.State == ConnectionState.Open)
                {
                    MessageBox.Show("База уже подключена");
                    return ("Connected"); ;
                }
                myConnection.Open();
                myConnection.Close();
                OleDbCommand com = new OleDbCommand();

                return ("Connected");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                return ("Disconnected");
            }
        }
        public static void readAll(DataGridView inputDataGrid)
        {
            OleDbCommand com = new OleDbCommand();
            adapter = new OleDbDataAdapter();
            table = new DataTable();
            dataSet = new DataSet();
            dataGridView = inputDataGrid;

            myConnection.Open();
            com.CommandText = "SELECT * FROM BirthDay";

            com.Connection = myConnection;
            OleDbDataReader reader = com.ExecuteReader();
            table.Load(reader);
            dataSet.Tables.Add(table);
            
            dataGridView.DataSource = table;
            myConnection.Close();
        }
        public static void add(PersonBirthDay inputPerson)
        {
            try
            {
                if (myConnection.State != ConnectionState.Open)
                {
                    myConnection.Open();
                }

                command = new OleDbCommand("INSERT INTO BirthDay(Фамилия, Имя, Дата_Рождения)" + "VALUES (@surName, @name, @birthDay)", myConnection);
                command.Parameters.AddWithValue("surName", inputPerson.Surname);
                command.Parameters.AddWithValue("name", inputPerson.Name);
                command.Parameters.AddWithValue("birthDay", inputPerson.BirthDay);

                command.ExecuteNonQuery();
                myConnection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                if (myConnection.State == ConnectionState.Open)
                {
                    myConnection.Close();
                }
            }
        }

        public static void delete(int index)
        {
            try
            {
                if (myConnection.State != ConnectionState.Open)
                {
                    myConnection.Open();
                }
                MessageBox.Show(index.ToString());
                command = new OleDbCommand("DELETE FROM BirthDay WHERE ID = @id", myConnection);
                command.Parameters.AddWithValue("id", index);

                command.ExecuteNonQuery();
                myConnection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                if (myConnection.State == ConnectionState.Open)
                {
                    myConnection.Close();
                }
            }
        }
    }
}

