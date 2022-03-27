
using System;
using System.Collections.Generic;
using Microsoft.Win32;
using System.Windows;
using System.Linq;
using System.IO;

namespace CDMACodeAssigner
{
    public partial class MainWindow : Window
    {
        private List<Sequence> sequencesList = new List<Sequence>();
        private List<User> usersList = new List<User>();

        private List<string> namesTakenList = new List<string>();
        private List<string> namesList = new List<string>();

        private int maxSF = 0;
        public MainWindow()
        {
            InitializeComponent();
            EnableButtons();

            for (int i = 0; i < 10; i++)
            {
                SFComboBox.Items.Add(Math.Pow(2, i));
            }

            SFComboBox.SelectedIndex = 0;

            for (char letter = 'A'; letter <= 'Z'; letter++)
            {
                namesList.Add(letter + "");
            }
        }

        // Generate the OVSF tree based on maximum SF value from the current users list
        private void GenerateSequences()
        {
            List<int> sequenceOrtho = new List<int>();
            List<int> sequence = new List<int> { 1 };

            double numberOfSequences;
            int length = 0;

            numberOfSequences = Math.Log2(maxSF);

            sequencesList.Add(new Sequence { length = 1, values = sequence });

            for (int i = 0; i < numberOfSequences; i++)
            {
                int listLength = sequencesList.Count;

                for (int j = length; j < listLength; j++)
                {
                    //Normal sequence
                    sequence = sequencesList[j].values.Concat(sequencesList[j].values).ToList();
                    sequencesList.Add(new Sequence { length = (length + 1) * 2, values = sequence });

                    // Orthogonal sequence
                    sequenceOrtho = sequencesList[j].values;
                    sequenceOrtho = sequenceOrtho.Concat(sequencesList[j].values.Select(x => -x)).ToList();
                    sequencesList.Add(new Sequence { length = (length + 1) * 2, values = sequenceOrtho });

                    sequenceOrtho = new List<int>();
                }
                length = listLength;
            }
        }

        // Main algorithm - assigning sequences from OVSF tree to users
        private void AssignSequences()
        {
            List<List<int>> sequencesAssigned = new List<List<int>>();
            int lastIndex;

            foreach (var user in usersList)
            {
                lastIndex = 0;

                foreach (var sequence in sequencesList)
                {
                    // If the sequence can be assigned to a user
                    if (user.sf == sequence.length && !sequencesAssigned.Contains(sequence.values) && sequence.isBlocked == false)
                    {
                        user.sequence = sequence.values;
                        sequence.isBlocked = true;

                        BlockSequences(sequence.length);

                        sequencesAssigned.Add(sequence.values);
                        break;
                    }
                    lastIndex++;
                }
                // If the sequence is already blocked
                for (int i = lastIndex; i < sequencesList.Count; i++)
                {
                    if (sequencesList[i].isBlocked == true)
                    {
                        BlockSequences(sequencesList[i].length);
                    }
                }
            }
        }

        // Block appropriate sequences choosen in AssignSequences() method
        void BlockSequences(int sequenceLength)
        {
            int indexBack = sequenceLength / 2;
            int indexFront = sequenceLength;

            int end = sequenceLength + (sequenceLength - 1);
            int start = sequenceLength - 1;

            for (int i = start; i < end; i++)
            {
                if (sequencesList[i].isBlocked == true)
                {
                    sequencesList[i - indexBack].isBlocked = true;

                    // Break if index is out of range - last OVSF tree column
                    try
                    {
                        sequencesList[i + indexFront].isBlocked = true;
                        sequencesList[i + indexFront + 1].isBlocked = true;
                    }
                    catch
                    {
                        break;
                    }
                }

                if (i % 2 != 0)
                {
                    indexBack += 1;
                }
                indexFront += 1;
            }

            // The first sequence (1) is always blocked
            sequencesList[0].isBlocked = true;
        }

        private void AddToOVSF()
        {
            // Add users with sequences assigned to them to listviews representing the OVSF tree
            foreach (var user in usersList)
            {
                if (user.sf == 1)
                {
                    if (user.sequence.Count > 0)
                    {
                        SF1ListView.Items.Add("User " + user.name + " Sequence: " + String.Join("", user.sequence));
                    }
                    else
                    {
                        UsersUnservedListView.Items.Add("User " + user.name);
                    }
                }
                else if (user.sf == 2)
                {
                    if (user.sequence.Count > 0)
                    {
                        SF2ListView.Items.Add("User " + user.name + " Sequence: " + String.Join("", user.sequence));
                    }
                    else
                    {
                        UsersUnservedListView.Items.Add("User " + user.name);
                    }
                }
                else if (user.sf == 4)
                {
                    if (user.sequence.Count > 0)
                    {
                        SF4ListView.Items.Add("User " + user.name + " Sequence: " + String.Join("", user.sequence));
                    }
                    else
                    {
                        UsersUnservedListView.Items.Add("User " + user.name);
                    }
                }
                else if (user.sf == 8)
                {
                    if (user.sequence.Count > 0)
                    {
                        SF8ListView.Items.Add("User " + user.name + " Sequence: " + String.Join("", user.sequence));
                    }
                    else
                    {
                        UsersUnservedListView.Items.Add("User " + user.name);
                    }
                }
                else if (user.sf == 16)
                {
                    if (user.sequence.Count > 0)
                    {
                        SF16ListView.Items.Add("User " + user.name + " Sequence: " + String.Join("", user.sequence));
                    }
                    else
                    {
                        UsersUnservedListView.Items.Add("User " + user.name);
                    }
                }
                else if (user.sf == 32)
                {
                    if (user.sequence.Count > 0)
                    {
                        SF32ListView.Items.Add("User " + user.name + " Sequence: " + String.Join("", user.sequence));
                    }
                    else
                    {
                        UsersUnservedListView.Items.Add("User " + user.name);
                    }
                }
                else if (user.sf == 64)
                {
                    if (user.sequence.Count > 0)
                    {
                        SF64ListView.Items.Add("User " + user.name + " Sequence: " + String.Join("", user.sequence));
                    }
                    else
                    {
                        UsersUnservedListView.Items.Add("User " + user.name);
                    }
                }
                else if (user.sf == 128)
                {
                    if (user.sequence.Count > 0)
                    {
                        SF128ListView.Items.Add("User " + user.name + " Sequence: " + String.Join("", user.sequence));
                    }
                    else
                    {
                        UsersUnservedListView.Items.Add("User " + user.name);
                    }
                }
                else if (user.sf == 256)
                {
                    if (user.sequence.Count > 0)
                    {
                        SF256ListView.Items.Add("User " + user.name + " Sequence: " + String.Join("", user.sequence));
                    }
                    else
                    {
                        UsersUnservedListView.Items.Add("User " + user.name);
                    }
                }
                else if (user.sf == 512)
                {
                    if (user.sequence.Count > 0)
                    {
                        SF512ListView.Items.Add("User " + user.name + " Sequence: " + String.Join("", user.sequence));
                    }
                    else
                    {
                        UsersUnservedListView.Items.Add("User " + user.name);
                    }
                }
            }
        }

        // Enable/disable buttons depending on number of users in listbox and listviews
        private void EnableButtons()
        {
            if (UsersListBox.Items.Count == 0)
            {
                AssignCodesBtn.IsEnabled = false;
                RemoveUserBtn.IsEnabled = false;
            }
            else
            {
                AssignCodesBtn.IsEnabled = true;
                RemoveUserBtn.IsEnabled = true;
            }

            if (UsersListBox.Items.Count >= 26)
            {
                AddUserBtn.IsEnabled = false;
            }
            else
            {
                AddUserBtn.IsEnabled = true;
            }

            if (UsersListBox.Items.Count == 0 
                && SF1ListView.Items.Count == 0
                && SF2ListView.Items.Count == 0
                && SF4ListView.Items.Count == 0
                && SF8ListView.Items.Count == 0
                && SF16ListView.Items.Count == 0
                && SF32ListView.Items.Count == 0
                && SF64ListView.Items.Count == 0
                && SF128ListView.Items.Count == 0
                && SF256ListView.Items.Count == 0
                && SF512ListView.Items.Count == 0)
            {
                ClearCodesBtn.IsEnabled = false;
            }
            else
            {
                ClearCodesBtn.IsEnabled = true;
            }
        }

        // Clear all listviews representing OVSF tree with assigned users
        private void ClearSFLists()
        {
            SF1ListView.Items.Clear();
            SF2ListView.Items.Clear();
            SF4ListView.Items.Clear();
            SF8ListView.Items.Clear();
            SF16ListView.Items.Clear();
            SF32ListView.Items.Clear();
            SF64ListView.Items.Clear();
            SF128ListView.Items.Clear();
            SF256ListView.Items.Clear();
            SF512ListView.Items.Clear();
        }

        // Add user (maximum of 26 A-Z users)
        private void AddUserBtn_Click(object sender, RoutedEventArgs e)
        {
            string name = "";

            foreach (string letter in namesList)
            {
                if (!namesTakenList.Contains(letter))
                {
                    name = letter;
                    namesTakenList.Add(letter);
                    break;
                }
            }

            usersList.Add(new User { name = name, sf = Convert.ToInt32(SFComboBox.SelectedItem) });

            // Sort users by their SF value (ascending)
            usersList = usersList.OrderBy(o => o.sf).ToList();

            UsersListBox.ItemsSource = usersList;
            UsersListBox.SelectedIndex = -1;

            // Determine maximum SF value from the current users list
            maxSF = usersList.Max(user => user.sf);

            EnableButtons();
            
        }

        // Remove selected user
        private void RemoveUserBtn_Click(object sender, RoutedEventArgs e)
        {
            if (UsersListBox.SelectedIndex >= 0)
            {
                usersList.RemoveAt(UsersListBox.SelectedIndex);
                namesTakenList.RemoveAt(UsersListBox.SelectedIndex);
            }

            EnableButtons();

            UsersListBox.Items.Refresh();
            UsersListBox.SelectedIndex = 0;

            if (usersList.Count > 0)
            {
                maxSF = usersList.Max(user => user.sf);
            }
        }

        // Clear everything on screen
        private void ClearCodesBtn_Click(object sender, RoutedEventArgs e)
        {
            SelectedSequenceTextBlock.Text = "";

            UsersUnservedListView.Items.Clear();
            namesTakenList.Clear();
            sequencesList.Clear();
            usersList.Clear();

            UsersListBox.SelectedIndex = -1;
            UsersListBox.Items.Refresh();

            ClearSFLists();
            EnableButtons();
        }

        // Assign codes if the listbox with users has any users in it
        private void AssignCodesBtn_Click(object sender, RoutedEventArgs e)
        {
            UsersUnservedListView.Items.Clear();
            SelectedSequenceTextBlock.Text = "";
            ClearSFLists();

            GenerateSequences();
            AssignSequences();
            AddToOVSF();

            // Clear screen
            namesTakenList.Clear();
            sequencesList.Clear();
            usersList.Clear();

            UsersListBox.SelectedIndex = -1;
            UsersListBox.Items.Refresh();

            EnableButtons();    
        }

        // Save currently assigned codes to .txt file 
        private void SaveFileBtn_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.FileName = "OVSFTree";
            dialog.DefaultExt = ".txt";
            dialog.Filter = "Text documents (.txt)|*.txt";

            Nullable<bool> result = dialog.ShowDialog();

            if (result == true)
            {
                string name = dialog.FileName;
                File.WriteAllText(name, "");

                int count = SF1ListView.Items.Count
                    + SF2ListView.Items.Count
                    + SF4ListView.Items.Count
                    + SF8ListView.Items.Count
                    + SF16ListView.Items.Count
                    + SF32ListView.Items.Count
                    + SF64ListView.Items.Count
                    + SF128ListView.Items.Count
                    + SF256ListView.Items.Count
                    + SF512ListView.Items.Count;

                File.AppendAllText(name, "Number of users in tree: " + count.ToString() + "\n\n");
                File.AppendAllText(name, "SF1: " + "\n");

                foreach (var item in SF1ListView.Items)
                {
                    File.AppendAllText(name, item.ToString() + "\n");
                }

                File.AppendAllText(name, "\n" + "SF2: " + "\n");

                foreach (var item in SF2ListView.Items)
                {
                    File.AppendAllText(name, item.ToString() + "\n");
                }

                File.AppendAllText(name, "\n" + "SF4: " + "\n");

                foreach (var item in SF4ListView.Items)
                {
                    File.AppendAllText(name, item.ToString() + "\n");
                }
                File.AppendAllText(name, "\n" + "SF8: " + "\n");

                foreach (var item in SF8ListView.Items)
                {
                    File.AppendAllText(name, item.ToString() + "\n");
                }

                File.AppendAllText(name, "\n" + "SF16: " + "\n");

                foreach (var item in SF16ListView.Items)
                {
                    File.AppendAllText(name, item.ToString() + "\n");
                }
                File.AppendAllText(name, "\n" + "SF32: " + "\n");

                foreach (var item in SF32ListView.Items)
                {
                    File.AppendAllText(name, item.ToString() + "\n");
                }

                File.AppendAllText(name, "\n" + "SF64: " + "\n");

                foreach (var item in SF64ListView.Items)
                {
                    File.AppendAllText(name, item.ToString() + "\n");
                }

                File.AppendAllText(name, "\n" + "SF128: " + "\n");

                foreach (var item in SF128ListView.Items)
                {
                    File.AppendAllText(name, item.ToString() + "\n");
                }

                File.AppendAllText(name, "\n" + "SF256: " + "\n");

                foreach (var item in SF256ListView.Items)
                {
                    File.AppendAllText(name, item.ToString() + "\n");
                }

                File.AppendAllText(name, "\n" + "SF512: " + "\n");

                foreach (var item in SF512ListView.Items)
                {
                    File.AppendAllText(name, item.ToString() + "\n");
                }
            }
        }

        // Mouse events - double-click listview element to show the sequence assigned to selected user
        private void SF1ListView_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (SF1ListView.SelectedIndex >= 0)
            {
                SelectedSequenceTextBlock.Text = SF1ListView.SelectedItem.ToString();
            }
        }

        private void SF2ListView_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (SF2ListView.SelectedIndex >= 0)
            {
                SelectedSequenceTextBlock.Text = SF2ListView.SelectedItem.ToString();
            }
        }

        private void SF4ListView_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (SF4ListView.SelectedIndex >= 0)
            {
                SelectedSequenceTextBlock.Text = SF4ListView.SelectedItem.ToString();
            }
        }

        private void SF8ListView_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (SF8ListView.SelectedIndex >= 0)
            {
                SelectedSequenceTextBlock.Text = SF8ListView.SelectedItem.ToString();
            }
        }

        private void SF16ListView_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (SF16ListView.SelectedIndex >= 0)
            {
                SelectedSequenceTextBlock.Text = SF16ListView.SelectedItem.ToString();
            }
        }

        private void SF32ListView_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (SF32ListView.SelectedIndex >= 0)
            {
                SelectedSequenceTextBlock.Text = SF32ListView.SelectedItem.ToString();
            }
        }

        private void SF64ListView_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (SF64ListView.SelectedIndex >= 0)
            {
                SelectedSequenceTextBlock.Text = SF64ListView.SelectedItem.ToString();
            }
        }

        private void SF128ListView_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (SF128ListView.SelectedIndex >= 0)
            {
                SelectedSequenceTextBlock.Text = SF128ListView.SelectedItem.ToString();
            }
        }

        private void SF256ListView_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (SF256ListView.SelectedIndex >= 0)
            {
                SelectedSequenceTextBlock.Text = SF256ListView.SelectedItem.ToString();
            }
        }

        private void SF512ListView_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (SF512ListView.SelectedIndex >= 0)
            {
                SelectedSequenceTextBlock.Text = SF512ListView.SelectedItem.ToString();
            }
        }
    }
}