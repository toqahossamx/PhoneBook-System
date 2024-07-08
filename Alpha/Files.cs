using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Alpha
{

    public class Files
    {
        public static void ImportContacts(string filePath, DataTable contacts)
        {
            //Get lines from the text file
            string[] lines = File.ReadAllLines(filePath);

            foreach (string line in lines)
            {
                string[] values = line.Split('/');

                //Trim each value to remove any whitespace and add to a new row in the DataTable
                DataRow newRow = contacts.NewRow();
                for (int i = 0; i < values.Length; i++)
                {
                    newRow[i] = values[i].Trim();
                }
                contacts.Rows.Add(newRow);
            }
        }


        public static void SaveContactsToFile(string filePath, DataGridView dataGridView)
        {
            using (TextWriter writer = new StreamWriter(filePath)) //using to close buffers
            {
                for (int i = 0; i < dataGridView.Rows.Count - 1; i++) //Exclude the last row which is empty
                {
                    var row = dataGridView.Rows[i];
                    var stringBuilder = new StringBuilder(); //Allows for handling mutable data

                    for (int j = 0; j < row.Cells.Count - 1; j++) //Exclude auto-incremented RowID
                    {
                        if (j > 0)
                        {
                            stringBuilder.Append("/");  //Append separator
                        }

                        var cellValue = row.Cells[j].Value;

                        if (cellValue == null)
                        {
                            stringBuilder.Append(string.Empty); 
                        }
                        else if (j == 5 && DateTime.TryParse(cellValue.ToString(), out DateTime date)) //out to help manage errors
                        {
                            stringBuilder.Append(date.ToString("MM-dd-yyyy"));  //Format date as MM-dd-yyyy
                        }
                        else
                        {
                            stringBuilder.Append(cellValue.ToString());  //Convert values to string
                        }
                    }

                    writer.WriteLine(stringBuilder.ToString());  //Write the formatted row to the file
                }
            }

            MessageBox.Show("Data Saved");
        }
    }
}