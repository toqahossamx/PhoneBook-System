using System;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FontAwesome.Sharp;
using MetroSet_UI;

//Please change the filepath in lines 629 and 641 to the one you have on your computer
//In the Data folder is the Contacts.txt file that has only a sample of contacts, feel free to add any contacts to the file but make sure they follow the same formatting
//There's a backup for the contacts in the Data folder named Contacts(Backup) please use it, if you lose the contacts data

namespace Alpha
{

    public partial class PhoneBook : Form
    {
        //Button Highlight
        private IconButton CurrentButton;
        private Panel leftBorderButton;


        private SortFilterSearch sfs = new SortFilterSearch(); //Object to use the methods sort, filter, and delete
        DataTable contacts = new DataTable(); //Data table object for contacts
        DataTable deletedContacts = new DataTable(); //Data table object for deleted contacts
        DataTable FavoritesContacts = new DataTable(); //Data table object for favorite contacts
        private DateTime currentDate = DateTime.Today; //Variable carrying the date of today to show the contacts that have their birthday today

        bool editing = false; //To keep track if the user is editing or not


        public PhoneBook()
        {
            InitializeComponent();

            //Border
            leftBorderButton = new Panel();
            leftBorderButton.Size = new Size(7, 60);
            PanelMenu.Controls.Add(leftBorderButton);

            //Form
            this.Text = String.Empty;
            this.ControlBox = false;
            this.DoubleBuffered = true; //To enhance performance
            this.MaximizedBounds = Screen.FromHandle(this.Handle).WorkingArea;

        }
        private void PhoneBook_Load(object sender, EventArgs e)
        {
            //Add columns to contacts data table
            contacts.Columns.Add("First name");
            contacts.Columns.Add("Second name");
            contacts.Columns.Add("Number");
            contacts.Columns.Add("Email");
            contacts.Columns.Add("Note");
            contacts.Columns.Add("Birthday", typeof(DateTime));
            contacts.Columns.Add("RowID", typeof(int));
            //Column row id is for sorting by latest and by earliest
            contacts.Columns["RowID"].AutoIncrement = true;
            contacts.Columns["RowID"].AutoIncrementSeed = 1;

            //Add columns to deleted contacts data table
            deletedContacts.Columns.Add("First name");
            deletedContacts.Columns.Add("Second name");
            deletedContacts.Columns.Add("Number");
            deletedContacts.Columns.Add("Email");
            deletedContacts.Columns.Add("Note");
            deletedContacts.Columns.Add("Birthday", typeof(DateTime));

            //Add columns to favorite contacts data table
            FavoritesContacts.Columns.Add("First name");
            FavoritesContacts.Columns.Add("Second name");
            FavoritesContacts.Columns.Add("Number");
            FavoritesContacts.Columns.Add("Email");
            FavoritesContacts.Columns.Add("Note");
            FavoritesContacts.Columns.Add("Birthday", typeof(DateTime));


            //Set data source for the tables in the GUI
            ContactsGridView.DataSource = contacts;
            DeletedContactsGridView.DataSource = deletedContacts;
            FavoritesGridView.DataSource = FavoritesContacts;


            ContactsGridView.Columns["RowID"].Visible = false; //Hiding the row  id column because it's not essential for the user

        }
        //------------------------------------------------------------------------------
        //Separate Methods
        private void ActiveButton(object senderBtn, Color color)
        {
            if (senderBtn != null)
            {
                DisableButton();
                CurrentButton = (IconButton)senderBtn;
                CurrentButton.BackColor = Color.FromArgb(38, 37, 82);
                CurrentButton.ForeColor = color;
                CurrentButton.TextAlign = ContentAlignment.MiddleCenter;
                CurrentButton.IconColor = color;
                CurrentButton.TextImageRelation = TextImageRelation.TextBeforeImage;
                CurrentButton.ImageAlign = ContentAlignment.MiddleRight;
                //Border Button
                leftBorderButton.BackColor = color;
                leftBorderButton.Location = new Point(0, CurrentButton.Location.Y);
                leftBorderButton.Visible = true;
                leftBorderButton.BringToFront();
            }
        }
        private void DisableButton()
        {
            if (CurrentButton != null)
            {
                CurrentButton.BackColor = Color.FromArgb(31, 31, 70);
                CurrentButton.ForeColor = Color.WhiteSmoke;
                CurrentButton.TextAlign = ContentAlignment.MiddleLeft;
                CurrentButton.IconColor = Color.WhiteSmoke;
                CurrentButton.TextImageRelation = TextImageRelation.ImageBeforeText;
                CurrentButton.ImageAlign = ContentAlignment.MiddleLeft;
            }
        }


        //Drag Form
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

        private void Panel_TitleBar_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }


        //The following code are some methods that was used above in the GUI
        private void ClearTextBoxes() //To initialize the text boxes
        {
            FirstNameTextBox.Text = "";
            SecondNameTextBox.Text = "";
            NumberTextBox.Text = "";
            EmailTextBox.Text = "";
            NoteTextBox.Text = "";
            BirthdayTextBox.Text = "";
        }


        private void DisplayContactData(Contact contact) //To display the data in the object contact into the text boxes
        {
            FirstNameTextBox.Text = contact.FirstName;
            SecondNameTextBox.Text = contact.SecondName;
            NumberTextBox.Text = contact.Number;
            EmailTextBox.Text = contact.Email;
            NoteTextBox.Text = contact.Note;
            BirthdayTextBox.Text = contact.Birthday.ToString("MM-dd-yyyy");
        }


        private void UpdateContactFromDataRow(Contact contact, DataRow row) //Add the data in the object to the row defined
        {
            row["First name"] = contact.FirstName;
            row["Second name"] = contact.SecondName;
            row["Number"] = contact.Number;
            row["Email"] = contact.Email;
            row["Note"] = contact.Note;
            row["Birthday"] = contact.Birthday;
        }
        // The end of the methods that was used above in the GUI
        //------------------------------------------------------------------------------
        private void AddContactButton_Click(object sender, EventArgs e)
        {
            ActiveButton(sender, Color.LightBlue);

            AddContactTabPage.Visible = true;
            //Open page to add contacts
            TabControl.SelectedTab = AddContactTabPage;
            //Initializing text-boxes
            ClearTextBoxes();


        }

        private void DisplayButton_Click(object sender, EventArgs e)
        {
            ActiveButton(sender, Color.LightBlue);

            //Open page to display contacts
            TabControl.SelectedTab = DisplayTabPage;

            //Remove the filter effect if it was used
            contacts.DefaultView.RowFilter = string.Empty;

            //Rebind the DataGridView to the original DataTable
            ContactsGridView.DataSource = contacts;
        }

        private void FavoritesButton_Click(object sender, EventArgs e)
        {
            ActiveButton(sender, Color.LightBlue);
            TabControl.SelectedTab = FavoritesTabPage;
        }

        private void RecycleBinButton_Click(object sender, EventArgs e)
        {
            ActiveButton(sender, Color.LightBlue);
            TabControl.SelectedTab = RecycleTabPage;
        }

        private void Home_Button_Click(object sender, EventArgs e)
        {
            Reset();
        }

        private void Reset()
        {
            DisableButton();
            leftBorderButton.Visible = false;
        }

        //------------------------------------------------------------------------------
        private void ExitButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void MaxButton_Click(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Normal)
                WindowState = FormWindowState.Maximized;
            else
                WindowState = FormWindowState.Normal;
        }
        private void MinButton_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        //-------------------------------------------------
        private void ButtonSave_Click(object sender, EventArgs e)
        {
            //Ensure the first name is not empty
            if (string.IsNullOrWhiteSpace(FirstNameTextBox.Text))
            {
                MessageBox.Show("First name cannot be empty.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //Validate first name (only letters and spaces)
            bool isValidFirstName = FirstNameTextBox.Text.All(c => char.IsLetter(c) || char.IsWhiteSpace(c));
            if (!isValidFirstName)
            {
                MessageBox.Show("First name must contain only letters and spaces.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //Ensure the second name is not empty
            if (string.IsNullOrWhiteSpace(SecondNameTextBox.Text))
            {
                MessageBox.Show("Second name cannot be empty.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //Validate second name (only letters and spaces)
            bool isValidSecondName = SecondNameTextBox.Text.All(c => char.IsLetter(c) || char.IsWhiteSpace(c));
            if (!isValidSecondName)
            {
                MessageBox.Show("Second name must contain only letters and spaces.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //Ensure phone number is not empty
            if (string.IsNullOrWhiteSpace(NumberTextBox.Text))
            {
                MessageBox.Show("Phone number cannot be empty.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //Validate phone number (only digits)
            bool isValidPhoneNumber = NumberTextBox.Text.All(char.IsDigit);
            if (!isValidPhoneNumber)
            {
                MessageBox.Show("Phone number must contain only digits.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //Validate birthday
            DateTime parsedBirthday;
            
            string birthdayText = BirthdayTextBox.Text.Trim();
            if (!string.IsNullOrWhiteSpace(birthdayText))
            {
                bool isValidDate = DateTime.TryParseExact(
                    birthdayText,
                    "MM-dd-yyyy",
                    null,
                    System.Globalization.DateTimeStyles.None,
                    out parsedBirthday
                                                         );

                if (!isValidDate)
                {
                    MessageBox.Show("Invalid birthday date format. Please enter a valid date in 'MM-dd-yyyy' format.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                
            }
            else
            {
                parsedBirthday = DateTime.MinValue; //Default value for cases where birthday is not provided
            }

            //If all validations pass, proceed with saving
            //Saving the data written in the text boxes into a new object from class contact
            Contact contact = new Contact(
                FirstNameTextBox.Text,
                SecondNameTextBox.Text,
                NumberTextBox.Text,
                EmailTextBox.Text,
                NoteTextBox.Text,
                parsedBirthday
                                        );

            if (editing) //If the user was editing an existing contact
            {
                //Update existing contact
                int rowIndex = ContactsGridView.CurrentCell.RowIndex; //Defining which contact was selected to be updated
                DataRow rowToUpdate = contacts.Rows[rowIndex]; //Saving the selected row to a new variable of type DataRow
                UpdateContactFromDataRow(contact, rowToUpdate); //Replace the old data in the selected row with the new data
            }
            else //If the user was adding a new contact
            {
                //Add new contact
                contacts.Rows.Add(contact.ToDataRow(contacts));
            }

            //Clear TextBoxes after saving
            ClearTextBoxes();
            editing = false; //Returning to the default state
        }


        private void Birthday_Reminder_Click(object sender, EventArgs e)
        {
            DataTable myContacts = contacts;
            Contact.CheckForUpcomingBirthdays(myContacts);

        }

        private void ButtonSort_Click(object sender, EventArgs e)
        {
            panelSort.Visible = !panelSort.Visible;
        }

        private void FilterButton_Click(object sender, EventArgs e)
        {
            panelfilter.Visible = !panelfilter.Visible;
        }

        private void ByNumberButton_Click(object sender, EventArgs e)
        {
            panelByNum.Visible = !panelByNum.Visible;
        }

        private void ByEmailButton_Click(object sender, EventArgs e)
        {
            panelByEmail.Visible = !panelByEmail.Visible;
        }

        private void ButtonEdit_Click(object sender, EventArgs e)
        {
            //Check if there are any rows in the contacts DataTable
            if (contacts.Rows.Count > 0)
            {
                //Get the index of the selected row in the contacts DataGridView
                int selectedIndex = ContactsGridView.CurrentCell.RowIndex;

                if (ContactsGridView.SelectedRows.Count > 0) // Check if there's a selected row
                {  
                    //Open page to add contacts
                    TabControl.SelectedTab = AddContactTabPage;
                    //Get the phone number from the selected row in the DataGridView
                    string phoneNumber = ContactsGridView.SelectedRows[0].Cells["Number"].Value.ToString();
                    //Find the DataRow in the contacts DataTable with the matching phone number
                    DataRow[] selectedDataRows = contacts.Select($"Number = '{phoneNumber}'"); //Key value
                    //Save the data in the row in a new variable
                    DataRow selectedRow = selectedDataRows[0];
                    //Create a Contact object from the selected DataRow
                    Contact selectedContact = Contact.FromDataRow(selectedRow);
                    //Display the selected contact's data in the text boxes for editing
                    DisplayContactData(selectedContact);
                    //Set editing flag to true
                    editing = true;
                }
                else
                {
                    //Handle the case when the selected index is invalid
                    MessageBox.Show("Please select a valid contact to edit.");
                }

            }
            else
            {
                //Handle the case when there are no rows in the contacts DataTable
                MessageBox.Show("There are no contacts to edit.");
            }
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            //Check if there are any rows in the contacts DataTable
            if (contacts.Rows.Count > 0)
            {
                //Get the index of the selected row in the contacts DataGridView
                int selectedIndex = ContactsGridView.CurrentCell.RowIndex;

                if (ContactsGridView.SelectedRows.Count > 0) // Check if there's a selected row
                { 
                    //Get the phone number from the selected row in the DataGridView
                    string phoneNumber = ContactsGridView.SelectedRows[0].Cells["Number"].Value.ToString();
                    //Find the DataRow in the contacts DataTable with the matching phone number
                    DataRow[] selectedDataRows = contacts.Select($"Number = '{phoneNumber}'");
                    //Save the data in the row in a new variable
                    DataRow selectedRow = selectedDataRows[0];
                    //Put the data into new object
                    Contact deletedContact = Contact.FromDataRow(selectedRow);
                    //Add the row to the recycle bin
                    deletedContacts.Rows.Add(deletedContact.ToDataRow(deletedContacts));
                    //Check if the deleted contact was in the Favorites list
                    DataRow[] favoriteRows = FavoritesContacts.Select($"[Number] = '{phoneNumber}'");
                    if (favoriteRows.Length > 0)
                    {
                        //Remove the contact from the Favorites list
                        FavoritesContacts.Rows.Remove(favoriteRows[0]);
                    }
                    //Delete the entire row from the contacts DataTable
                    contacts.Rows.Remove(selectedRow);
                }
                else
                {
                    //Handle the case when the selected index is invalid
                    MessageBox.Show("Please select a valid contact to delete.");
                }
            }
            else
            {
                //Handle the case when there are no rows in the contacts DataTable
                MessageBox.Show("There are no contacts to delete.");
            }
        }

        private void perDeleteButton_Click(object sender, EventArgs e)
        {
            //Check if there are any rows in the deleted contacts DataTable
            if (deletedContacts.Rows.Count > 0)
            {
                //Get the index of the selected row in the contacts DataGridView
                int selectedIndex = DeletedContactsGridView.CurrentCell.RowIndex;

                if (DeletedContactsGridView.SelectedRows.Count > 0) // Check if there's a selected row
                {
                    //Delete the entire row from the recycle bin
                    deletedContacts.Rows[DeletedContactsGridView.CurrentCell.RowIndex].Delete();
                }
                else
                {
                    //Handle the case when the selected index is invalid
                    MessageBox.Show("Please select a valid contact to permanently delete.");
                }
            }
            else
            {
                //Handle the case when there are no rows in the contacts DataTable
                MessageBox.Show("There are no contacts to permanently delete.");
            }
        }

        private void RestoreButton_Click(object sender, EventArgs e)
        {
            //Check if there are any rows in the deleted contacts DataTable
            if (deletedContacts.Rows.Count > 0)
            {
                //Get the index of the selected row in the contacts DataGridView
                int selectedIndex = DeletedContactsGridView.CurrentCell.RowIndex;

                if (DeletedContactsGridView.SelectedRows.Count > 0) // Check if there's a selected row
                {
                    //Save the data in the row in a new variable
                    DataRow selectedRow = deletedContacts.Rows[DeletedContactsGridView.CurrentCell.RowIndex];
                    //Put the data into new object
                    Contact restoredContact = Contact.FromDataRow(selectedRow);
                    //Retrieve from recycle bin
                    contacts.Rows.Add(restoredContact.ToDataRow(contacts));
                    //Delete the entire row from the recycle bin
                    deletedContacts.Rows[DeletedContactsGridView.CurrentCell.RowIndex].Delete();
                }
                else
                {
                    //Handle the case when there are no rows in the contacts DataTable
                    MessageBox.Show("Please select a valid contact to restore.");
                }
            }
            else
            {
                //Handle the case when there are no rows in the deleted contacts DataTable
                MessageBox.Show("There are no contacts to restore.");
            }
        }

        private void ButtonAddFav_Click(object sender, EventArgs e)
        {
            //Check if there are any rows in the contacts DataTable
            if (contacts.Rows.Count > 0)
            {
                //Get the index of the selected row in the contacts DataGridView
                int selectedIndex = ContactsGridView.CurrentCell.RowIndex;

                if (ContactsGridView.SelectedRows.Count > 0) // Check if there's a selected row
                {
                    //Get the phone number from the selected row in the DataGridView
                    string phoneNumber = ContactsGridView.SelectedRows[0].Cells["Number"].Value.ToString();
                    //Find the DataRow in the contacts DataTable with the matching phone number
                    DataRow[] selectedDataRows = contacts.Select($"Number = '{phoneNumber}'");
                    //Save the data in the row in a new variable
                    DataRow selectedRow = selectedDataRows[0];
                    //Put the data into new object
                    Contact favoritedContact = Contact.FromDataRow(selectedRow);
                    //Add to favorite list
                    FavoritesContacts.Rows.Add(favoritedContact.ToDataRow(FavoritesContacts));
                }

                else
                {
                    //Handle the case when the selected index is invalid
                    MessageBox.Show("Please select a valid contact to add to favorites.");
                }
            }
            else
            {
                //Handle the case when there are no rows in the contacts DataTable
                MessageBox.Show("There are no contacts to add to favorites.");
            }
        }

        private void ButtonRemoveFav_Click(object sender, EventArgs e)
        {
            //Check if there are any rows in the favorites contacts DataTable
            if (FavoritesContacts.Rows.Count > 0)
            {
                //Get the index of the selected row in the contacts DataGridView
                int selectedIndex = FavoritesGridView.CurrentCell.RowIndex;

                if (FavoritesGridView.SelectedRows.Count > 0) //Check if there's a selected row
                {
                    //Remove the entire row from the favorite list
                    FavoritesContacts.Rows[FavoritesGridView.CurrentCell.RowIndex].Delete();
                }
                else
                {
                    //Handle the case when there are no rows in the contacts DataTable
                    MessageBox.Show("Please select a valid contact to remove from favorites.");
                }
            }
            else
            {
                //Handle the case when there are no rows in the favorites contacts DataTable
                MessageBox.Show("There are no contacts to remove from favorites.");
            }
        }

        private void ButtonAlpha_Click(object sender, EventArgs e)
        {
            //Call the Sort method of SortFilterSearch class
            sfs.Sort("alphabetical", contacts);
        }

        private void ButtonLatest_Click(object sender, EventArgs e)
        {
            sfs.Sort("latest", contacts);
        }

        private void ButtonByEarliest_Click(object sender, EventArgs e)
        {
            sfs.Sort("earliest", contacts);
        }

        private void Button010_Click(object sender, EventArgs e)
        {
            //Call the Filter method of SortFilterSearch class
            sfs.Filter("number", 1, contacts);
        }

        private void Button011_Click(object sender, EventArgs e)
        {
            sfs.Filter("number", 2, contacts);
        }

        private void Button012_Click(object sender, EventArgs e)
        {
            sfs.Filter("number", 3, contacts);
        }

        private void Buttongmail_Click(object sender, EventArgs e)
        {
            sfs.Filter("email", 1, contacts);
        }

        private void ButtonYahoo_Click(object sender, EventArgs e)
        {
            sfs.Filter("email", 2, contacts);
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            //Call the Search method of SortFilterSearch class
            string searchTerm = SearchBox.Text.Trim();
            sfs.Search(searchTerm, contacts);
        }
        bool performImport1 = true;
        private void ImportButton_Click(object sender, EventArgs e)
        {
            if (performImport1)
            {
                performImport1 = false; //To perform the import only once
                Files.ImportContacts(@"C:\Users\total\Main Drive\AASTMT\Term 4\Object-Oriented Programming\Project\PhoneBook_System\Data\Contacts.txt", contacts);
            }
        }

        private void ButtonBirth_Click(object sender, EventArgs e)
        {
            DataTable myContacts = contacts;
            Contact.CheckForUpcomingBirthdays(myContacts);
        }

        private void ExportButton_Click(object sender, EventArgs e)
        {
            string filePath = @"C:\Users\total\Main Drive\AASTMT\Term 4\Object-Oriented Programming\Project\PhoneBook_System\Data\Contacts.txt";
            Files.SaveContactsToFile(filePath, ContactsGridView);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void AddContactTabPage_Click(object sender, EventArgs e)
        {

        }
    }
}