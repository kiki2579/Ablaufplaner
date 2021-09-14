using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JesusHouseAblaufplaner
{

    /*
     "English": {

     },
        "Deutsch": {
    "Error": {
      "Test":  "test"
    },
    "Information": {

    }
     */
    class lang
    {
        public DE Deutsch { get; set; }
        public string lang_chosen { get; set; }

        public class English
        {
            public save Save { get; set; }
            public form__2 Form2 { get; set; }
            public timedelay delay { get; set; }

            public class save
            {
                public empty Empty { get; set; }
                public notsaved Notsaved { get; set; }
                public class empty
                {
                    public string msg { get; set; }
                    public string caption { get; set; }
                }
                public class notsaved
                {
                    public string msg { get; set; }
                    public string caption { get; set; }
                }
            }
            public class form__2
            {
                public string endOfTable { get; set; }
            }
            public class timedelay
            {
                public string msg { get; set; }
                public string caption { get; set; }
            }
        }

        public class DE
        {
            public save Speichern { get; set; }
            public form__2 Form2 { get; set; }
            public timedelay delay { get; set; }
            public class save
            {
                public empty Empty { get; set; }
                public notsaved NotSaved { get; set; }
                public class empty
                {
                    public string msg { get; set; }
                    public string caption { get; set; }
                }
                public class notsaved
                {
                    public string msg { get; set; }
                    public string caption { get; set; }
                }                
            }
            public class form__2
            {     
                public string endOfTable { get; set; }                
            }
            public class timedelay
            {
                public string msg { get; set; }
                public string caption { get; set; }
            }
        }
    }
}
