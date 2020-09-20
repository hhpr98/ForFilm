using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementFilm
{
    class PhimClass
    {
        public int id { get; set; }
        public string nameFilm { get; set; } // ex : Đường chuyên - TangDynastour (2018)
        public string path { get; set; } // ex : Đường chuyên - TangDynastour (2018) 20-07-2019.html
        public DateTime date { get; set; } // 20/07/2019
        public int favorite { get; set; }
        public string year { get; set; }
    }
}
