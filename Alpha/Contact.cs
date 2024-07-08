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

    public class Contact
    {
        private string _firstName;
        private string _secondName;
        private string _number;
        private string _email;
        private string _note;
        private DateTime _birthday;

        public string FirstName
        {
            get { return _firstName; }
            set { _firstName = value; }
        }

        public string SecondName
        {
            get { return _secondName; }
            set { _secondName = value; }
        }

        public string Number
        {
            get { return _number; }
            set { _number = value; }
        }

        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }

        public string Note
        {
            get { return _note; }
            set { _note = value; }
        }

        public DateTime Birthday
        {
            get { return _birthday; }
            set { _birthday = value; }
        }

        public Contact(string firstName, string secondName, string number, string email, string note, DateTime birthday)
        {
            _firstName = firstName;
            _secondName = secondName;
            _number = number;
            _email = email;
            _note = note;
            _birthday = birthday;
        }

        public static Contact FromDataRow(DataRow row)
        {
            //Parse data from DataRow and return a new Contact object
            string firstName = row["First name"].ToString();
            string secondName = row["Second name"].ToString();
            string number = row["Number"].ToString();
            string email = row["Email"].ToString();
            string note = row["Note"].ToString();
            DateTime birthday = (DateTime)row["Birthday"];

            return new Contact(firstName, secondName, number, email, note, birthday);
        }

        public DataRow ToDataRow(DataTable table)
        {
            //Create a new DataRow 
            DataRow row = table.NewRow();
            row["First name"] = _firstName;
            row["Second name"] = _secondName;
            row["Number"] = _number;
            row["Email"] = _email;
            row["Note"] = _note;
            row["Birthday"] = _birthday;

            return row;
        }

        public virtual void Sort(string type, DataTable contacts) { }

        public virtual void Filter(string type, int number, DataTable contacts) { }

        public virtual void Search(string searchTerm, DataTable contacts) { }


        public static void CheckForUpcomingBirthdays(DataTable contacts)
        {
            DateTime today = DateTime.Today;
            DateTime nextWeek = today.AddDays(7);

            List<string> upcomingBirthdays = new List<string>();

            foreach (DataRow row in contacts.Rows)
            {
                if (row["Birthday"] != DBNull.Value)
                {
                    DateTime birthday = (DateTime)row["Birthday"];

                    //Create a birthday this year with the same day and month
                    DateTime thisYearBirthday = new DateTime(today.Year, birthday.Month, birthday.Day);

                    //If the birthday is between today and next week
                    if (thisYearBirthday >= today && thisYearBirthday <= nextWeek)
                    {
                        upcomingBirthdays.Add($"{row["First name"]} {row["Second name"]} - {thisYearBirthday.ToShortDateString()}");
                    }
                }
            }

            if (upcomingBirthdays.Count > 0)
            {
                string message = "Upcoming Birthdays:\n" + string.Join("\n", upcomingBirthdays);
                MessageBox.Show(message, "Birthday Reminder", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("No upcoming birthdays in the next 7 days.", "Birthday Reminder", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}