using System;
using System.Windows.Forms;
using System.Collections.Generic;

using System.IO;
using System.Data;
using System.Drawing;

namespace testProject
{
    public partial class Form1 : Form
    {
        private string dbFileName;
        private string folderName;

        private List<PersonBirthDay> PersonList = new List<PersonBirthDay>();

        public Form1()
        {
            InitializeComponent();
        }

        private void createButton_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result = dbFolderBrowserDialog.ShowDialog();

                if (result == DialogResult.OK)
                {
                    pathTextBox.Text = dbFolderBrowserDialog.SelectedPath + "\\" + dbNameTextBox.Text;
                }

                folderName = pathTextBox.Text;
                lbStatusText.Text = DbController.createBirthDayFolderNDataBase(folderName, dbNameTextBox.Text);
                lbStatusText.ForeColor = connectingLabelColorEqual(lbStatusText.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error" + ex.Message);
            }
        }

        private void connectButton_Click(object sender, EventArgs e)
        {
            if (!File.Exists(dbFileName))
                MessageBox.Show("Please, create DB and blank table (Push \"Create\" button)");
            try
            {
                lbStatusText.Text = DbController.connectToDataBase(dbFileName);
                lbStatusText.ForeColor = connectingLabelColorEqual(lbStatusText.Text);
            }
            catch (Exception ex)
            {
                lbStatusText.Text = "Disconnected";
                lbStatusText.ForeColor = connectingLabelColorEqual(lbStatusText.Text);
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void readAllButton_Click(object sender, EventArgs e)
        {
            DbController.readAll(dgvViewer);
            PersonList = getPersonBirthDayList();

            //test
            string str = "";
            foreach (var item in PersonList)
            {
                str += item.ToString() + "\n";
            }
            MessageBox.Show(str);
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            PersonBirthDay tmpPerson = new PersonBirthDay(nameTextBox.Text, surnameTextBox.Text, birthDayDateTimePicker.Value);
            DbController.add(tmpPerson);
            DbController.readAll(dgvViewer);
        }

        private void browseButton_Click(object sender, EventArgs e)
        {
            DialogResult result = bdFileDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                folderName = bdFileDialog.FileName;
            }
            pathTextBox.Text = bdFileDialog.FileName;
        }

        public Color connectingLabelColorEqual(string inputString)
        {
            if (inputString == "Connected") return Color.Green;
            return Color.Red;
        }

        private void pathTextBox_TextChanged(object sender, EventArgs e)
        {
            dbFileName = pathTextBox.Text;
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            DbController.delete(Convert.ToInt32(dgvViewer.CurrentRow.Cells[0].Value));
            DbController.readAll(dgvViewer);
        }

        private List<PersonBirthDay> getPersonBirthDayList()
        {
            PersonBirthDay tmpPerson = new PersonBirthDay();
            List<PersonBirthDay> retList = new List<PersonBirthDay>(0);
            //foreach (DataGridViewRow item in dgvViewer.Rows)
            for (int i = 0; i < dgvViewer.Rows.Count - 1; i++)
            {
                //try
                //{
                    tmpPerson = new PersonBirthDay();
                    tmpPerson.Surname = dgvViewer.Rows[i].Cells[1].Value.ToString().Replace(" ", "");
                    tmpPerson.Name = dgvViewer.Rows[i].Cells[2].Value.ToString().Replace(" ", "");
                    tmpPerson.BirthDay = Convert.ToDateTime(dgvViewer.Rows[i].Cells[3].Value.ToString());
                    MessageBox.Show(tmpPerson.ToString());
                    retList.Add(tmpPerson);
                //}
                //catch (Exception ex) { }
            }
            return retList;
        }

        private void updateFromDGVButton_Click(object sender, EventArgs e)
        {

        }

        private void showNearestButton_Click(object sender, EventArgs e)
        {
            List<PersonBirthDay> tmpPersonList = new List<PersonBirthDay>();
            List<PersonBirthDay> retList = new List<PersonBirthDay>();

            int countOfNearest = Convert.ToInt32(countOfNearestTextBox.Text);
            tmpPersonList = PersonList.GetRange(0, PersonList.Count);

            string retstr = $"Ближайщие {countOfNearest} Дней рождений:\n";

            for (int i = 0; i < countOfNearest; i++)
            {
                var def = (DateTime.Now - tmpPersonList[0].BirthDay).TotalDays;
                int index = 0;
                for (int j = 0; j < tmpPersonList.Count; j++)
                {
                    if ((DateTime.Now - tmpPersonList[j].BirthDay).TotalDays < def)
                    {
                        def = (DateTime.Now - tmpPersonList[j].BirthDay).TotalDays;
                        index = j;
                    }
                }
                retstr += tmpPersonList[index].ToString() + "\n";
                retList.Add(tmpPersonList[index]);
                tmpPersonList.RemoveAt(index);
            }
            MessageBox.Show(retstr);
        }
    }
}