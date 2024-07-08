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
    internal class SortFilterSearch : Contact
    {
        public SortFilterSearch() : base("DefaultFirstName", "DefaultSecondName", "DefaultNumber", "DefaultEmail", "DefaultNote", DateTime.Today)
        {

        }



        public override void Sort(string type, DataTable contacts)
        {
            if (type == "alphabetical")
            {
                //Sort the contacts DataTable alphabetically based on the "First name" column
                contacts.DefaultView.Sort = "First name ASC";
            }
            else if (type == "latest")
            {
                //Sort the contacts DataTable by the row index in descending order
                contacts.DefaultView.Sort = "RowID DESC";
            }
            else if (type == "earliest")
            {
                //Sort the contacts DataTable by the row index in ascending order
                contacts.DefaultView.Sort = "RowID ASC";
            }
        }

        public override void Filter(string type, int number, DataTable contacts)
        {
            if (type == "number")
            {
                if (number == 1)
                {
                    contacts.DefaultView.RowFilter = "Number LIKE '010%'";
                }
                else if (number == 2)
                {
                    contacts.DefaultView.RowFilter = "Number LIKE '011%'";
                }
                else if (number == 3)
                {
                    contacts.DefaultView.RowFilter = "Number LIKE '012%'";
                }
            }
            else if (type == "email")
            {
                if (number == 1)
                {
                    contacts.DefaultView.RowFilter = "Email LIKE '%@gmail%'";
                }
                else if (number == 2)
                {
                    contacts.DefaultView.RowFilter = "Email LIKE '%@yahoo%'";
                }
            }
        }

        public override void Search(string searchTerm, DataTable contacts)
        {
            string filterExpression = $"[First name] LIKE '{searchTerm}%'" +
                                      $" OR [Second name] LIKE '{searchTerm}%'" +
                                      $" OR [Number] LIKE '%{searchTerm}%'" +
                                      $" OR [Email] LIKE '{searchTerm}%'";
            contacts.DefaultView.RowFilter = filterExpression;
        }
    }
}