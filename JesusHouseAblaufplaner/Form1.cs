﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
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
            else
            {
                statusStrip1.Visible = true;
                toolStripProgressBar1.Minimum = 0;
                toolStripProgressBar1.Maximum = table_lines;
                toolStripProgressBar1.Value = 0;
                toolStripProgressBar1.Step = 1;
                openFile();
                statusStrip1.Visible = false;
            }
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
                            toolStripProgressBar1.PerformStep();
                        } while (textboxname > 0);

                        using (StreamReader sr = new StreamReader(file_name))
                        {
                            int c = 0;
                            while (sr.ReadLine() != null) { c++; }
                            toolStripProgressBar1.Maximum = c;
                            toolStripProgressBar1.Value = 1;
                        }
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
                            toolStripProgressBar1.PerformStep();
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
            DateTime date;
            TimeSpan timeSpan;
            DateTime startdate;
            int tablehours, tableminutes;
            double timeBefore;

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
                    for (int i = 0; i <= content.Length; i++)
                    {
                        try
                        {
                            Console.WriteLine("Test: " + content[i][0].Length);
                            Char[] savestate = new Char[5];
                            for (int j = 0; j < content[i][0].Length; j++)
                            {
                                Console.WriteLine(content[i][0][j]);
                                savestate[j] = content[i][0][j];
                            }
                            content[i][0] = savestate[0].ToString() + savestate[1].ToString() + ':' + savestate[2].ToString() + savestate[3].ToString();
                        }
                        catch
                        {
                            break;
                        }
                        
                
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


            Int32.TryParse(string.Concat(content[0][0][0], content[0][0][1]), out tablehours);//string1 + string2 - > int
            Int32.TryParse(string.Concat(content[0][0][3], content[0][0][4]), out tableminutes);// prespoin +1 weil wir uns auf die verbleibende zeit beziehen. also auf die startzeit des nächsten

            date = DateTime.Now;
            Console.WriteLine($"Form1_Test_Clock: {tableminutes}");
            Console.WriteLine($"Form1_Test2_Clock: {date.Day.ToString()}");
            if (tablehours == 0) // wenn 0:00 dann Nächter Tag
            {
                date = date.AddDays(1);
            }
            Console.WriteLine($"Form1_Test3_Clock: {date.Day.ToString()}");
            startdate = new DateTime(Convert.ToInt32(date.ToString("yyyy")), Convert.ToInt32(date.ToString("MM")), Convert.ToInt32(date.ToString("dd")), tablehours, tableminutes, 0);
            date = DateTime.Now;
            timeSpan = startdate - date;
            Console.WriteLine(timeSpan.ToString(@"hh\:mm\:ss"));
            timeBefore = timeSpan.TotalMilliseconds;
            wait(5);
            date = DateTime.Now;
            timeSpan = startdate - date;
            Console.WriteLine($"TestZeit: {timeBefore}");
            Console.WriteLine($"TestZeit2: {timeSpan.TotalMilliseconds}");
            Console.WriteLine(timeSpan.ToString(@"hh\:mm\:ss"));
            if (timeBefore > timeSpan.TotalMilliseconds)
            {
                DialogResult result;
                result = MessageBox.Show(language.Deutsch.delay.msg, language.Deutsch.delay.caption, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (result == DialogResult.Cancel)
                {
                    return;
                }else if (result == DialogResult.Yes){

                    for (int i = 0; i <= content.Length; i++)
                    {
                        try
                        {
                            Int32.TryParse(string.Concat(content[i][0][0], content[i][0][1]), out tablehours);//string1 + string2 - > int
                            Int32.TryParse(string.Concat(content[i][0][3], content[i][0][4]), out tableminutes);// prespoin +1 weil wir uns auf die verbleibende zeit beziehen. also auf die startzeit des nächsten
                                                        
                            Console.WriteLine($"Form12_Test_Clock: {tablehours}");
                            Console.WriteLine($"Form12_Test2_Clock: {date.Hour.ToString()}");
                            if (tablehours == 0) // wenn 0:00 dann Nächter Tag
                            {
                                date = date.AddDays(1);
                            }
                            Console.WriteLine($"Form12_Test3_Clock: {date.Day.ToString()}");
                            startdate = new DateTime(Convert.ToInt32(date.ToString("yyyy")), Convert.ToInt32(date.ToString("MM")), Convert.ToInt32(date.ToString("dd")), tablehours, tableminutes, 0);                       
                            Console.WriteLine("StartTest: " + startdate.ToString(@"HH\:mm\:ss"));
                            startdate -= timeSpan;
                            Console.WriteLine(timeSpan.ToString(@"hh\:mm\:ss"));
                            Console.WriteLine(startdate.ToString(@"HH\:mm\:ss"));
                            content[i][0] = startdate.ToString(@"HH\:mm");
                            Console.WriteLine(content[i][0]);
                        }
                        catch
                        {
                            break;
                        }

                    }
                }
            }

            Console.WriteLine(table_lines);
            Form2 Form2 = new Form2(table_lines, content, table.RowCount);
            Form2.Visible = true;

            Form2.Activate();
            Form2.Enabled = true;
        }

        public void wait(int milliseconds) //Anstatt wie bei System.Threading.Thread.Sleep() wird das UI nicht angehalten
        {
            var timerForm = new System.Windows.Forms.Timer();
            if (milliseconds == 0 || milliseconds < 0) return;

            // Console.WriteLine("start wait timer");
            timerForm.Interval = milliseconds;
            timerForm.Enabled = true;
            timerForm.Start();

            timerForm.Tick += (s, e) =>
            {
                timerForm.Enabled = false;
                timerForm.Stop();
                // Console.WriteLine("stop wait timer");
            };

            while (timerForm.Enabled)
            {
                Application.DoEvents();
            }
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

        #region Drucken

        int count = 0;
        private void scrollDown()
        {
            Point current = table.AutoScrollPosition;
            Point scrolled = new Point(current.X, -current.Y + 384);
            table.AutoScrollPosition = scrolled;
        }
        private void druckenToolStripMenuItem_Click(object sender, EventArgs e)
        {

            //Scroll to the top
            //count = 0;
            printDialog1.AllowPrintToFile = true;            
            printDialog1.Document = printDocument1;

            if (printDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    printDocument1.Print();
                }
                catch (Win32Exception)
                {

                }
            }
        }

        public static Bitmap resizeImage(Bitmap imgToResize, Size size)
        {
            return (Bitmap)(new Bitmap(imgToResize, size));
        }

        private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {
            //for (int i = 0; i < 2; i++)
            //{
            //    using (Bitmap printImage = new Bitmap(table.Width, table.Height))
            //    {
            //        table.DrawToBitmap(printImage, new Rectangle(0, 0, printImage.Width, printImage.Height));
            //        if (i == 0) e.Graphics.DrawImage(printImage, 0, panel1.Width);
            //        else e.Graphics.DrawImage(printImage, 0, printImage.Width * i);                    
            //    }
            //}

            if (count == 0)
            {
                using (Bitmap printImage = new Bitmap(table.Width - 20, table.Height - 12))// um die letze sichtbare reihe genau abzuschneiden
                {
                    table.DrawToBitmap(printImage, new Rectangle(0, 0, printImage.Width, printImage.Height));
                    e.Graphics.DrawImage(printImage, 0, panel1.Height);
                }
                using (Bitmap printImage = new Bitmap(panel1.Width - 20, panel1.Height))
                {
                    panel1.DrawToBitmap(printImage, new Rectangle(0, 0, printImage.Width, printImage.Height));
                    e.Graphics.DrawImage(printImage, 0, 0);
                }
            }
            for (int i = 1; i < 3; i++)
            {
                scrollDown();
                using (Bitmap printImage = new Bitmap(table.Width - 20, table.Height - 12))
                {
                    table.DrawToBitmap(printImage, new Rectangle(0, 0, printImage.Width, printImage.Height));
                    e.Graphics.DrawImage(printImage, 0, (table.Height + 24)*i);
                } 
            }
            count++;
            Console.WriteLine(table_lines / 21 + " | " + count);
            e.HasMorePages = count < table_lines/21;
        }
        #endregion

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
