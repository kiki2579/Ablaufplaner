using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace JesusHouseAblaufplaner
{

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }



        public int textboxname = 0;
        public bool saved = true;
        public int table_lines = 0;

        public string file_name;

        public Dictionary<string, Keys> hotkeys = new Dictionary<string, Keys>();
        //public Dictionary<string, string, string, string> lang = new Dictionary<string, string, string, string>(); //<sprache,typ,typ,text>
        public dynamic language;




        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                hotkeys.Add(hilfeToolStripMenuItem.Name, hilfeToolStripMenuItem.ShortcutKeys);
                hotkeys.Add(speichernToolStripMenuItem.Name, speichernToolStripMenuItem.ShortcutKeys);
                hotkeys.Add(öffnenToolStripMenuItem.Name, öffnenToolStripMenuItem.ShortcutKeys);
                hotkeys.Add(hinzufügenToolStripMenuItem.Name, hinzufügenToolStripMenuItem.ShortcutKeys);
                hotkeys.Add(entfernenToolStripMenuItem.Name, entfernenToolStripMenuItem.ShortcutKeys);
                hotkeys.Add(startenToolStripMenuItem.Name, startenToolStripMenuItem.ShortcutKeys);
            }
            catch (ArgumentException)
            {
                Console.WriteLine("An element with that key already exists!");
            }

            Console.WriteLine(Environment.CurrentDirectory);

            string json;

            using (StreamReader rd = new StreamReader(@"..\..\res\lang\lang.json"))
            {
                json = rd.ReadToEnd();
            }
            
            try
            {
                language = JsonConvert.DeserializeObject<lang>(json);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            

        }
        
        private void hinzufügenToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            saved = false;
            if (table_lines >= table.RowCount) return;
            for (int i = 0; i < 4; i++)
            {
                TextBox textBox = new TextBox();// { Name = Convert.ToString(textboxname + i) };
                textBox.Name = "textBox" + Convert.ToString(textboxname + i);
                Console.WriteLine(textBox.Name);
                textBox.Dock = DockStyle.Fill;
                textBox.KeyDown += TextBoxesKeyPressed;
                textBox.KeyPress += TextBoxesKeyPressEvent;
                //textBox.Text = textBox.Name;
                TableLayoutControlCollection controls = table.Controls;
                try
                {
                    controls.Add(textBox);
                }
                catch (System.ArgumentException)
                {
                    
                }
            }
            textboxname += 4;
            table_lines += 1;
        }

        private bool nonNumberEntered = false;
        private Keys keys;

        private void TextBoxesKeyPressed(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            saved = false;

            nonNumberEntered = false;

            keys = e.KeyCode;

            if (e.KeyCode < Keys.D0 || e.KeyCode > Keys.D9)
            {
                if (e.KeyCode < Keys.NumPad0 || e.KeyCode > Keys.NumPad9)
                {
                    if (e.KeyCode != Keys.Back)
                        nonNumberEntered = true;
                    else nonNumberEntered = false;
                }
            }

            if (Control.ModifierKeys == Keys.Shift) nonNumberEntered = true;
            if (Control.ModifierKeys == Keys.Capital ) nonNumberEntered = true;

        }

        private void TextBoxesKeyPressEvent(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            for (int i = 0; i <= textboxname; i += 4) //Nur Textboxen aus Spalte 1
            {
                if (textBox.Name.Equals("textBox" + Convert.ToString(i)))
                {
                    if (nonNumberEntered)
                    {
                        string msg = "Nur Zahlen";
                        string caption = "Graf Zahl diggah";

                        MessageBox.Show(msg, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        e.Handled = true;
                    }
                    if (textBox.Text.Length > 3)
                    {
                        if (keys != Keys.Back)
                            e.Handled = true;
                    }
                    if (textBox.Text.Length == 0)
                    {
                        Console.WriteLine("0");
                        if (keys < Keys.D0 || keys > Keys.D2)
                        {
                            if (keys < Keys.NumPad0 || keys > Keys.NumPad2)
                            {
                                if (keys != Keys.Back)
                                {
                                    e.Handled = true;
                                }
                            }
                        }
                    }
                    if (textBox.Text.Length == 1)
                    {
                        Console.WriteLine("1");
                        if (textBox.Text[0] == '2')
                        {
                            if (keys < Keys.D0 || keys > Keys.D3)
                            {
                                if (keys < Keys.NumPad0 || keys > Keys.NumPad3)
                                {
                                    if (keys != Keys.Back)
                                    {
                                        e.Handled = true;
                                    }
                                }
                            }
                        }
                    }
                    if (textBox.Text.Length == 2)
                    {
                        Console.WriteLine("2");
                        if (keys < Keys.D0 || keys > Keys.D5)
                        {
                            if (keys < Keys.NumPad0 || keys > Keys.NumPad5)
                            {
                                if (keys != Keys.Back)
                                {
                                    e.Handled = true;
                                }
                            }
                        }
                    }
                    if (textBox.Text.Length > 3)
                    {
                        if (textBox.SelectionLength.Equals(textBox.Text.Length))
                        {
                            Console.WriteLine("Test");
                            e.Handled = false;
                        }
                        if (!nonNumberEntered)
                        {
                            textBox.SelectAll();
                        }else if (keys != Keys.Back)
                        {
                            Console.WriteLine("too long");
                            e.Handled = true;
                        }
                    }
                }
            }
        }

        private void entfernenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (textboxname > 0)
            {
                textboxname -= 4;
                table_lines -= 1;
                saved = false;
            }
            if (textboxname <= 0)
            {
                textboxname = 0;
                table_lines = 0;
                saved = true;
            }
            
            for (int i = 0; i < 4; i++)
            {
                TextBox textBox = new TextBox();        
                textBox.Name = "textBox" + Convert.ToString(textboxname + i);
                textBox.Dock = DockStyle.Fill;
                //textBox.Text = "textBox" + Convert.ToString(textboxname + i);
                textBox.KeyDown -= TextBoxesKeyPressed;
                textBox.KeyPress -= TextBoxesKeyPressEvent;

                table.Controls.RemoveByKey("textBox" + Convert.ToString(textboxname + i));
            }
        }

        private void speichernToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (textboxname == 0)
            {
                //string msg = "The table is empty! Please Fill the table.\nFor Help open the Help Menu or press 'F1'.";
                //string caption = "Empty table!";
                
                MessageBox.Show(language.Deutsch.Speichern.Empty.msg, language.Deutsch.Speichern.Empty.caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            saveFile();
        }

        private void öffnenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!saved)
            {
                //string msg = "Your table isn't saved. Do you want to save it?";
                //string caption = "Unsaved Changes";
                DialogResult result;

                result = MessageBox.Show(language.Deutsch.Speichern.NotSaved.msg, language.Deutsch.Speichern.NotSaved.caption, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Stop);
                if (result == DialogResult.Cancel) return;
                else if (result == DialogResult.Yes) saveFile();
                else if (result == DialogResult.No) openFile();

            }
            else openFile();
        }

        public void openFile()
        {
            openFileDialog1.Filter = "Kirche files (*.kabp)|*.kabp|Text files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                file_name = openFileDialog1.FileName;
                try
                {
                    using (StreamReader rd = new StreamReader(openFileDialog1.FileName))
                    {
                        do
                        {
                            if (textboxname > 0)
                            {
                                textboxname -= 4;
                                table_lines -= 1;
                                saved = false;
                            }
                            if (textboxname <= 0)
                            {
                                textboxname = 0;
                                table_lines = 0;
                                saved = true;
                            }

                            for (int i = 0; i < 4; i++)
                            {
                                TextBox textBox = new TextBox();
                                textBox.Name = "textBox" + Convert.ToString(textboxname + i);
                                textBox.Dock = DockStyle.Fill;
                                //textBox.Text = "textBox" + Convert.ToString(textboxname + i);
                                textBox.KeyDown -= TextBoxesKeyPressed;
                                textBox.KeyPress -= TextBoxesKeyPressEvent;
                                table.Controls.RemoveByKey("textBox" + Convert.ToString(textboxname + i));
                            } 
                        } while (textboxname > 0);                        

                        do
                        {
                            String line = rd.ReadLine();
                            String[] erg = new String[4];
                            int point = 0;
                            bool nextcol = false;
                            Console.WriteLine(line);
                            for (int i = 0; i < line.Length; i++)
                            {
                                if (line[i] != '|') erg[point] += line[i];
                                else
                                {
                                    if (nextcol)
                                    {
                                        point += 1;
                                        nextcol = false;
                                    }
                                    else nextcol = true;
                                    
                                }
                            }
                            for (int i = 0; i < 4; i++)
                            {
                                TextBox textBox = new TextBox();// { Name = Convert.ToString(textboxname + i) };
                                textBox.Name = "textBox" + Convert.ToString(textboxname + i);
                                Console.WriteLine(textBox.Name);
                                textBox.Dock = DockStyle.Fill;
                                textBox.Text = erg[i];
                                textBox.KeyDown += TextBoxesKeyPressed;
                                textBox.KeyPress += TextBoxesKeyPressEvent;
                                TableLayoutControlCollection controls = table.Controls;
                                controls.Add(textBox);
                            }
                            textboxname += 4;
                            table_lines += 1;                            
                        } while (!rd.EndOfStream);                    
                    }
                }
                catch (IOException e)
                {
                    Console.WriteLine("The file could not be read!");
                    Console.WriteLine(e.Message);
                }
            }
        }


        public void saveFile()
        {

            saveFileDialog1.Filter = "Kirche files (*.kabp)|*.kabp|Text files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                file_name = saveFileDialog1.FileName;

                try
                {
                    using (StreamWriter wr = new StreamWriter(saveFileDialog1.FileName))
                    {
                        int l = textboxname / 4;
                        int line = 0;
                        Console.WriteLine(l);
                        //string[,] input = new string[l, 4];
                        string input;
                        for (int i = 0; i < l; i++)
                        {
                            input = "";
                            for (int j = 0; j < 4; j++)
                            {
                                string textboxstring = "textBox" + Convert.ToString(line + j);
                                Console.WriteLine("Test: textBox" + Convert.ToString(line + j));
                                Control textBox = table.Controls.Find(textboxstring, false).First() as TextBox;
                                Console.WriteLine(textBox.Text);
                                if (j < 3) input += $"{textBox.Text}||";//textBox.Text + "||"
                                else input += textBox.Text;
                            }
                            line += 4;
                            wr.WriteLine(input);  
                        }
                        saved = true;
                    }
                }
                catch (IOException e)
                {
                    Console.WriteLine("The table could not be saved!");
                    Console.WriteLine(e.Message);
                }
            }
        }

        private void startenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            String[][] content = new String[table.RowCount][];
            for (int i = 0; i < content.GetLength(0); i++)
            {
                content[i] = new String[4];
            }

            if (!saved)
            {
                //string msg = "Your table isn't saved. You have to save before you present it or open another table!";
                //string caption = "Unsaved Changes";
                DialogResult result;

                result = MessageBox.Show(language.Deutsch.Speichern.NotSaved.msg, language.Deutsch.Speichern.NotSaved.caption, MessageBoxButtons.OKCancel, MessageBoxIcon.Stop);
                if (result == DialogResult.Cancel) return;
                else if (result == DialogResult.OK) saveFile();
            }
            if (table_lines != 0 && saved)
            {
                try
                {
                    using (StreamReader rd = new StreamReader(file_name))
                    {
                        int j = 0;
                        do
                        {
                            String line = rd.ReadLine();
                            String[] erg = new String[4];
                            int point = 0;
                            bool nextcol = false;
                            Console.WriteLine(line);
                            for (int i = 0; i < line.Length; i++)
                            {
                                if (line[i] != '|') erg[point] += line[i];
                                else
                                {
                                    if (nextcol)
                                    {
                                        point += 1;
                                        nextcol = false;
                                    }
                                    else nextcol = true;

                                }
                            }
                            content[j] = erg;
                            j++;
                        } while (!rd.EndOfStream);
                    }
                    Console.WriteLine("Test2: " + content);
                    for (int i = 0; i < content.Length; i++)
                    {
                        Console.WriteLine("Test: " + content[i][0]);
                        Char[] savestate = new Char[5];
                        for (int j = 0; j < content[i][0].Length; j++)
                        {
                            Console.WriteLine(content[i][0][j]);
                            savestate[j] = content[i][0][j];
                        }
                        content[i][0] = savestate[0].ToString() + savestate[1].ToString() + ':' + savestate[2].ToString() + savestate[3].ToString();
                
                    }

                }
                catch (IOException ee)
                {
                    Console.WriteLine("The file could not be read!");
                    Console.WriteLine(ee.Message);
                    return;
                }catch (NullReferenceException e2)
                {

                }

            }
            else return;

            Console.WriteLine(table_lines);
            Form2 Form2 = new Form2(table_lines, content, table.RowCount);
            Form2.Visible = true;

            Form2.Activate();
            Form2.Enabled = true;
        }

        private void Form1_FormClosing(object sender, CancelEventArgs e)
        {
            if (!saved)
            {
                //string msg = "Your table isn't saved. Do you want to saved it?";
                //string caption = "Unsaved Changes";
                DialogResult result;

                result = MessageBox.Show(language.Deutsch.Speichern.NotSaved.msg, language.Deutsch.Speichern.NotSaved.caption, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Stop);
                if (result == DialogResult.Cancel)
                {
                    e.Cancel = true;
                    return;
                }
                else if (result == DialogResult.Yes)
                {
                    e.Cancel = true;
                    saveFile();
                }

    
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void druckenToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void hilfeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3(hotkeys, 2);
            form3.Visible = true;
            form3.Activate();
            form3.Enabled = true;
        }

        private void hotkeysToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3(hotkeys, 0);
            form3.Visible = true;
            form3.Activate();
            form3.Enabled = true;
        }

        private void spracheToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3(hotkeys, 1);
            form3.Visible = true;
            form3.Activate();
            form3.Enabled = true;
        }
        
    }
}
