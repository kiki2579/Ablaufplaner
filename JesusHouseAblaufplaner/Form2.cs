using Newtonsoft.Json;
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

namespace JesusHouseAblaufplaner
{
    public partial class Form2 : Form
    {

        public int lines = 0;
        public int prespoint = 0;
        public String[][] table;

        public bool fullscreen = false;

        public DateTime date = DateTime.Now;
        public int tablehours, tableminutes;
        public double timeBefore;
        public double overtime_indikator;
        public bool hoch = false;
        public TimeSpan timeSpan;
        DateTime startdate;

        public dynamic language;


        FormState formState = new FormState(); //https://www.codeproject.com/Articles/16618/How-To-Make-a-Windows-Form-App-Truly-Full-Screen-a

        private void timer1_Tick(object sender, EventArgs e)
        {
            date = DateTime.Now;
            timeSpan = startdate - date;
            Console.WriteLine($"TestZeit: {timeBefore}");
            Console.WriteLine($"TestZeit2: {timeSpan.TotalSeconds}");
            if (timeBefore < timeSpan.TotalSeconds) hoch = true;
            else hoch = false;
            if (!hoch)
                label8.Text = timeSpan.ToString(@"hh\:mm\:ss");
            else label8.Text = "+" + timeSpan.ToString(@"hh\:mm\:ss");
            //Console.WriteLine(timeSpan.ToString(@"hh\:mm\:ss"));
            if (!hoch && timeSpan.TotalSeconds > 60)
                label8.ForeColor = Color.Green;
            else if (!hoch && timeSpan.TotalSeconds < 60)
                label8.ForeColor = Color.DarkOrange;
            if (hoch || timeSpan.TotalSeconds <= overtime_indikator)
                label8.ForeColor = Color.Red;
            if (timeSpan.TotalSeconds < 1) hoch = true;
            timeBefore = timeSpan.TotalSeconds;
        }

        public string time;
        

        public Form2(int fm1_lines, string[][] fm1table, int fm1_table_row)
        {
            InitializeComponent();
            table = new String[fm1_table_row][];
            for (int i = 0; i < table.GetLength(0); i++)
            {
                table[i] = new String[4];
            }
            lines = fm1_lines;
            table = fm1table;
        }


        

        private void Form2_Load(object sender, EventArgs e)
        {
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

            panel4.BackColor = Color.FromArgb(30, Color.Black);

            Console.WriteLine("Test§: " + lines);
            //TableLayoutRowStyleCollection styles = tableLayoutPanel1.RowStyles;
            //foreach (RowStyle style in styles)
            //{
            //    // Set the row height to 20 pixels.
            //    style.SizeType = SizeType.Absolute;
            //    style.Height = 50;
            //}

            TableLayoutRowStyleCollection styles = tableLayoutPanel3.RowStyles;
            foreach (RowStyle style in styles)
            {
                // Set the row height to 20 pixels.
                style.SizeType = SizeType.Absolute;
                style.Height = 70;
            }

            for (int i = 0; i < lines-1; i ++)
            {                
                
                  //tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
                  //tableLayoutPanel1.RowCount++;

                    Console.WriteLine(table[i][2] + " : " + lines);
                    Label label = new Label();
                    label.Name = "next" + Convert.ToString(i);
                    label.Dock = DockStyle.Fill;
                    label.TextAlign = ContentAlignment.MiddleCenter;
                    label.Text = $"Was: {table[prespoint + 1 + i][1]}\nWer: {table[prespoint + 1 + i][2]}\nUm: {table[prespoint + 1 + i][0]}\n({table[prespoint + 1 + i][3]})\n";
                    label.Font = new Font("Arial", 22);
                    Console.WriteLine(label.Text);
                    TableLayoutControlCollection controls = tableLayoutPanel3.Controls;
                    controls.Add(label);
                    Console.WriteLine(table[i][2] + " : " + lines);
                    /*Label label1 = new Label();
                    label1.Name = "placeholder" + Convert.ToString(i);
                    label1.Dock = DockStyle.Fill;
                    controls.Add(label1);*/
                

            }
            label3.Text = table[prespoint][1];
            Int32.TryParse(string.Concat(table[prespoint+1][0][0], table[prespoint+1][0][1]), out tablehours);//string1 + string2 - > int
            Int32.TryParse(string.Concat(table[prespoint+1][0][3], table[prespoint+1][0][4]), out tableminutes);// prespoin +1 weil wir uns auf die verbleibende zeit beziehen. also auf die startzeit des nächsten
            date = DateTime.Now;
            Console.WriteLine($"Test_Clock: {tablehours}");
            Console.WriteLine($"Test2_Clock: {date.Day.ToString()}");
            if (tablehours == 0) // wenn 0:00 dann Nächter Tag
            {
                date = date.AddDays(1);
            }
            Console.WriteLine($"Test3_Clock: {date.Day.ToString()}");
            startdate = new DateTime(Convert.ToInt32(date.ToString("yyyy")), Convert.ToInt32(date.ToString("MM")), Convert.ToInt32(date.ToString("dd")), tablehours, tableminutes, 0);            
            date = DateTime.Now;
            timeSpan = startdate - date;
            overtime_indikator = timeSpan.TotalSeconds * 0.1;
            Console.WriteLine(timeSpan.ToString(@"hh\:mm\:ss"));
            timer1.Enabled = true;
            label4.Text = table[prespoint][2];
            label5.Text = table[prespoint][3];
            tableLayoutPanel3.Enabled = true;
            //panel1.Enabled = true; 
            
        }

        public void Reload()
        {   

            timer1.Enabled = false;
            for (int i = 0; i < tableLayoutPanel3.RowCount; i++)
            {
                tableLayoutPanel3.Controls.RemoveByKey("next" + Convert.ToString(i));
                tableLayoutPanel3.Controls.RemoveByKey("placeholder" + Convert.ToString(i));
            }

            for (int i = 1; i < lines; i++)
            {
                //tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
                //tableLayoutPanel1.RowCount++;

                Console.WriteLine(table[prespoint + i][2] + " : " + lines);
                Label label = new Label();
                label.Name = "next" + Convert.ToString(i - 1);
                label.Dock = DockStyle.Fill;
                label.TextAlign = ContentAlignment.MiddleCenter;
                if (table[prespoint + i][1] != null) label.Text = $"Was: {table[prespoint + i][1]}\nWer: {table[prespoint + i][2]}\nUm: {table[prespoint + i][0]}\n({table[prespoint + i][3]})\n{label.Name}";
                label.Font = new Font("Arial", 22);
                Console.WriteLine(label.Text);
                TableLayoutControlCollection controls = tableLayoutPanel3.Controls;
                controls.Add(label);
                Console.WriteLine(table[prespoint + i][2] + " : " + lines);
                /*Label label1 = new Label();
                label1.Name = "placeholder" + Convert.ToString(i - 1);
                label1.Dock = DockStyle.Fill;
                controls.Add(label1);*/
            }

            label3.Text = table[prespoint][1];
            try
            {
                Int32.TryParse(string.Concat(table[prespoint + 1][0][0], table[prespoint+1][0][1]), out tablehours);//string1 + string2 - > int
                Int32.TryParse(string.Concat(table[prespoint + 1][0][3], table[prespoint+1][0][4]), out tableminutes);
            }
            catch (NullReferenceException e)
            {
                //Wenn nichts wéiteres geplant ist!;
                label8.Text = language.Deutsch.Form2.endOfTable;
                label8.Font = new Font("Microsoft Sans Serif", this.Size.Width / 30, FontStyle.Bold);
                label4.Text = table[prespoint][2];
                label8.ForeColor = Color.White;
                timer1.Enabled = false;
                return;
            }
            label5.Text = table[prespoint][3];
            date = DateTime.Now;
            if (tablehours == 0)
            {
                date = date.AddDays(1);
            }
            startdate = new DateTime(Convert.ToInt32(date.ToString("yyyy")), Convert.ToInt32(date.ToString("MM")), Convert.ToInt32(date.ToString("dd")), tablehours, tableminutes, 0);
            date = DateTime.Now;
            if (hoch) startdate.Add(timeSpan);
            else startdate.Subtract(timeSpan);
            timeSpan = startdate - date;
            overtime_indikator = timeSpan.TotalSeconds * 0.1;
            timer1.Enabled = true;
            label4.Text = table[prespoint][2];
            label5.Text = table[prespoint][3];
            if(fullscreen == true)
            {
                label8.Font = new Font("Microsoft Sans Serif", this.Size.Width / 15);
            }
        }
        

        private void Form2_KeyDown(object sender, KeyEventArgs e)
        {



            if (e.KeyCode == Keys.F11)
            {
                if (fullscreen) return;
                this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
                formState.Maximize(this);
                panel4.BackColor = Color.FromArgb(5, Color.White);
                label7.BackColor = Color.FromArgb(5, Color.White);
                panel4.Location = new Point(this.Size.Width / 2 - panel4.Size.Width/2, 57);
                panel4.Visible = true;
                timer2.Enabled = true;
                fullscreen = true;
                tableLayoutPanel1.Height = this.Size.Height;
                tableLayoutPanel1.Width = this.Size.Width;
                TableLayoutRowStyleCollection styles = tableLayoutPanel3.RowStyles;
                foreach (RowStyle style in styles)
                {
                    // Set the row height to 20 pixels.
                    style.SizeType = SizeType.Absolute;
                    style.Height = 140;
        
                }
           
            }
            else if (e.KeyCode == Keys.Escape)
            {
                if (!fullscreen) return;
                formState.Restore(this);
                this.WindowState = System.Windows.Forms.FormWindowState.Normal;
                fullscreen = false;
                TableLayoutRowStyleCollection styles = tableLayoutPanel3.RowStyles;
                foreach (RowStyle style in styles)
                {
                    // Set the row height to 20 pixels.
                    style.SizeType = SizeType.Absolute;
                    style.Height = 70;

                }
            }
            else if (e.KeyCode == Keys.Left)
            {
                if (prespoint != 0)
                {
                    prespoint--;
                }
                Reload();
                for (int i = 0; i <= lines; i++)
                {
                    //Control label = tableLayoutPanel1.Controls.Find("next" + i.ToString(), false).First() as Label;
                }
            }
            else if (e.KeyCode == Keys.Right)
            {
                if (table[prespoint+1][1] != null)
                {
                    prespoint++;
                }
                Reload();
                for (int i = 0; i <= lines; i++)
                {
                    //Control label = tableLayoutPanel1.Controls.Find("next" + i.ToString(), false).First() as Label;
                }
            }


        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            panel4.BackColor = Color.FromArgb(0, Color.White);
            panel4.Visible = false;
            timer2.Enabled = false;
        }


        private void Form2_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState.Equals(FormWindowState.Normal))
            {
                //label1.TextAlign = ContentAlignment.BottomRight;
                this.FormBorderStyle = FormBorderStyle.Sizable;
                //label1.TextAlign = ContentAlignment.TopCenter;
                tableLayoutPanel1.Height = 595;
                tableLayoutPanel1.Width = 365;
            }
            tableLayoutPanel1.Height = this.Size.Height-30;
            tableLayoutPanel1.Width = this.Size.Width-10;
            label1.Font = new Font("Microsoft Sans Serif", this.Size.Width / 40, FontStyle.Underline);
            label2.Font = new Font("Microsoft Sans Serif", this.Size.Width / 30, FontStyle.Underline);
            label3.Font = new Font("Microsoft Sans Serif", this.Size.Width / 30);
            label4.Font = new Font("Microsoft Sans Serif", this.Size.Width / 30, FontStyle.Bold);
            label5.Font = new Font("Microsoft Sans Serif", this.Size.Width / 30);
            label7.Font = new Font("Microsoft Sans Serif", this.Size.Width / 35, FontStyle.Underline);
            label8.Font = new Font("Microsoft Sans Serif", this.Size.Width / 15);
          
      
            /*for (int i = 0; i <= lines; i++)
            {
                Control label = tableLayoutPanel1.Controls.Find("next" + Convert.ToString(i), false).First() as Label;
                label.Font = new Font("Arial", this.Size.Width / 50);
            }*/
        }
    }
}
