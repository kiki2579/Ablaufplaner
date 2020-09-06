using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JesusHouseAblaufplaner
{
    public partial class Form3 : Form
    {
        public Dictionary<string, Keys> hotkeys = new Dictionary<string, Keys>();
        Form1 fm1;
        int tab = 0;

        public Form3(Dictionary<string, Keys> fm1hotkeys, int fm1tab)
        {
            InitializeComponent();

            hotkeys = fm1hotkeys;
            tab = fm1tab;
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            label1.Text = "Help Menu\n" +
                "Dies ist ein Programm um Abläufe zu planen. Dafür kannst du Tabellen erstellen, speichern, öffnen oder drucken.\n\n" +
                "Tabellen:\n" +
                $"Du kannst neue Zeilen erstellen, indem Du '{hotkeys["hinzufügenToolStripMenuItem"]}' drückst.\n" +
                $"Außerdem kannst du Zeilen entfernen, indem du '{hotkeys["entfernenToolStripMenuItem"]}' drückst.\n\n" +
                $"Dateimanagement:\n" +
                $"Du kannst deine Tabellen mit '{hotkeys["speichernToolStripMenuItem"]}' speichern und mit\n" +
                $"'{hotkeys["öffnenToolStripMenuItem"]}' wieder öffnen.\n" +
                "Zudem kannst du deine Tabelle mit 'null' drucken.\n\n" +
                $"In den Einstellungen kannst du alle Hotkeys bearbeiten und beliebig verändern, oder hier in diesen Hilfetext schauen.\n\n" +
                $"Starten der Präsentation";

            if (tab == 0) tabControl1.SelectedTab = tabPage1;
            else if (tab == 1) tabControl1.SelectedTab = tabPage2;
            else if (tab == 2) tabControl1.SelectedTab = tabPage3;
        }
    }
}
