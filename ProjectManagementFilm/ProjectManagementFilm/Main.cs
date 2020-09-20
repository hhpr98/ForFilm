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

namespace ProjectManagementFilm
{
    public partial class Main : Form
    {
        private Panel pn = new Panel();
        private int selIndex = 0;
        private List<PhimClass> dataFilm = null; // data origin (data nguyên bản)
        private List<PhimClass> dataToShow = null; // data qua quá trình lọc, sắp xếp, tìm kiếm

        public Main()
        {
            InitializeComponent();
            LoadComponent();
            dataFilm = getPhimFromFolder();
            dataToShow = dataFilm.Where(p => p.id != -1).ToList();
            dataToShow.Sort((a, b) => (b.date.CompareTo(a.date)));
            LoadData(dataToShow);
        }

        #region load
        // load panel, thiết đặt kích thước, scroll theo chiều dọc
        private void LoadComponent()
        {
            this.Size = new Size(850, 600);
            this.BackColor = Color.Black;

            pn.Location = new Point(40, 50);
            pn.Size = new Size(750, 500);
            pn.BackColor = Color.White;
            this.Controls.Add(pn);
            pn.AutoScroll = false;
            pn.HorizontalScroll.Enabled = false;
            pn.HorizontalScroll.Visible = false;
            pn.HorizontalScroll.Maximum = 0;
            pn.AutoScroll = true;

            //ScrollBar vScrollBar1 = new VScrollBar();
            //vScrollBar1.Dock = DockStyle.Right;
            //vScrollBar1.Scroll += (sender, e) => { pn.VerticalScroll.Value = vScrollBar1.Value; };
            //pn.Controls.Add(vScrollBar1);

            // comboBox sort
            cbSort.DataSource = new List<string> { "Theo ngày xem", "Theo bảng chữ cái", "Theo độ ưu tiên", "Theo năm" };
            cbSort.SelectedIndexChanged += CbSort_SelectedIndexChanged;
            cbFilter.DataSource = new List<string> { "Tất cả", "2020", "2019", "2018", "2017", "2016", "2015", "2014", "2013", "2012", "2011", "2010", "2009", "2008", "2007", "2006", "2005", "2004", "2003", "2002", "2001", "2000", "<2000" };
            cbFilter.SelectedIndexChanged += CbFilter_SelectedIndexChanged;
        }

        // load dữ liệu phim lên panel từ  list có sẵn
        private void LoadData(List<PhimClass> dataFilm)
        {
            this.pn.Controls.Clear(); // clear 

            int ylocal = 10;
            for (int i = 0; i < dataFilm.Count(); i++)
            {
                PictureBox pb = new PictureBox();
                pb.Size = new Size(20, 20);
                pb.Location = new Point(10, ylocal);
                Bitmap avt = new Bitmap(Application.StartupPath + "\\Resources\\icons8_Play.png");
                pb.Image = avt;
                pb.SizeMode = PictureBoxSizeMode.StretchImage;
                this.pn.Controls.Add(pb);

                var lb = new Label();
                lb.Text = dataFilm[i].nameFilm;
                lb.Size = new Size(600, 30);
                lb.Font = new Font("Times New Roman", 12F, FontStyle.Bold);
                lb.ForeColor = Color.Blue;
                lb.Location = new Point(30, ylocal);
                this.pn.Controls.Add(lb);

                Label lbStar = new Label();
                lbStar.Text = dataFilm[i].favorite.ToString();
                lbStar.Size = new Size(20, 30);
                lbStar.Font = new Font("Times New Roman", 12F, FontStyle.Bold);
                lbStar.ForeColor = Color.Red;
                lbStar.Location = new Point(645, ylocal + 1);
                this.pn.Controls.Add(lbStar);

                PictureBox pbstar = new PictureBox();
                pbstar.Size = new Size(20, 20);
                pbstar.Location = new Point(670, ylocal);
                Bitmap pbStarImg = new Bitmap(Application.StartupPath + "\\Resources\\icons8_Star_Filled_48px.png");
                pbstar.Image = pbStarImg;
                pbstar.SizeMode = PictureBoxSizeMode.StretchImage;
                this.pn.Controls.Add(pbstar);

                PictureBox pbclick = new PictureBox();
                pbclick.Size = new Size(20, 20);
                pbclick.Location = new Point(710, ylocal);
                Bitmap pbimg = new Bitmap(Application.StartupPath + "\\Resources\\icons8_Opened_Folder.png");
                pbclick.Image = pbimg;
                pbclick.SizeMode = PictureBoxSizeMode.StretchImage;
                pbclick.Click += Pbclick_Click;
                pbclick.Name = "p" + dataFilm[i].id.ToString(); // p0, p2, p3,.....,p11, p12,....., p1000,.....
                pbclick.Cursor = Cursors.Hand;
                this.pn.Controls.Add(pbclick);

                ylocal += 40;
            }
        }

        // lấy danh sách phim từ file, đọc dữ liệu và chuyển thành dạng class
        private List<PhimClass> getPhimFromFolder()
        {
            List<PhimClass> lf = new List<PhimClass>();
            var path = @"C:/git/ForFilm/data";
            var info = new DirectoryInfo(path);
            FileInfo[] list = info.GetFiles();
            int incr = 0;
            foreach (var index in list)
            {
                var str = index.Name;
                var item = new PhimClass();
                item.id = incr;
                incr++;
                item.path = str;
                item.nameFilm = str.Substring(0, str.Length - 18);// 18 là độ dài " x dd-mm-yyyy.html"
                var d = str.Substring(str.Length - 15, 10); // dd-mm-yyyy (15 là dd-mm-yyyy.html)
                int dd = int.Parse(d.Substring(0, 2)); // dd
                int mm = int.Parse(d.Substring(3, 2)); // mm
                int yyyy = int.Parse(d.Substring(6, 4)); // yyyy
                var dateWatch = new DateTime(yyyy, mm, dd); // dd/mm/yyyy
                item.date = dateWatch;
                var fav = str.Substring(str.Length - 17, 1);
                item.favorite = int.Parse(fav);
                var idx = str.LastIndexOf(")");
                if (idx == -1)
                    item.year = "0";
                else
                {
                    var stringYear = str.Substring(idx - 4, 4);
                    item.year = stringYear;
                }
                lf.Add(item);
            }
            return lf;
        }
        #endregion

        #region event
        // combox changed event
        private void CbSort_SelectedIndexChanged(object sender, EventArgs e)
        {
            selIndex = cbSort.SelectedIndex;

            switch (selIndex)
            {
                case 0:
                    dataToShow = dataFilm.Where(p => p.id != -1).ToList();
                    dataToShow.Sort((a, b) => (b.date.CompareTo(a.date)));
                    LoadData(dataToShow);
                    break;
                case 1:
                    dataToShow = dataFilm.Where(p => p.id != -1).ToList();
                    dataToShow.Sort((a, b) => (a.nameFilm.CompareTo(b.nameFilm)));
                    LoadData(dataToShow);
                    break;
                case 2:
                    dataToShow = dataFilm.Where(f => f.favorite > 0).ToList();
                    dataToShow.Sort((a, b) => (b.favorite.CompareTo(a.favorite)));
                    LoadData(dataToShow);
                    break;
                case 3:
                    dataToShow = dataFilm.Where(p => p.id != -1).ToList();
                    dataToShow.Sort((a, b) => (b.year.CompareTo(a.year)));
                    LoadData(dataToShow);
                    break;
            }
        }

        // xem chi tiết 1 phim
        private void Pbclick_Click(object sender, EventArgs e)
        {
            var pb = sender as PictureBox;
            var id = int.Parse(pb.Name.Substring(1, pb.Name.Length - 1));
            var find = dataFilm.Where(f => f.id == id).ToList();
            var name = find[0].path;
            var path = @"C:\git\ForFilm\data\" + name;
            System.Diagnostics.Process.Start(path);
        }

        // search click
        private void btnSearch_Click(object sender, EventArgs e)
        {
            var txt = txtSearch.Text;
            if (txt == "")
            {
                dataToShow = dataFilm.Where(p => p.id != -1).ToList();
                dataToShow.Sort((a, b) => (a.nameFilm.CompareTo(b.nameFilm)));
                LoadData(dataToShow);
            }
            else
            {
                dataToShow = dataFilm.Where(p => p.nameFilm.ToLower().Contains(txt.ToLower())).ToList();
                dataToShow.Sort((a, b) => (a.date.CompareTo(b.date)));
                LoadData(dataToShow);
            }
        }

        // clear click
        private void btnClear_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            dataToShow = dataFilm.Where(p => p.id != -1).ToList();
            dataToShow.Sort((a, b) => (a.date.CompareTo(b.date)));
            LoadData(dataToShow);
        }

        private void CbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            var cbFil = sender as ComboBox;
            //MessageBox.Show(cbFil.Text);

            if (cbFil.Text == "Tất cả")
            {
                dataToShow = dataFilm.Where(p => p.id != -1).ToList();
                dataToShow.Sort((a, b) => (a.date.CompareTo(b.date)));
                LoadData(dataToShow);
            }
            else if (cbFil.Text == "<2000")
            {
                dataToShow = dataFilm.Where(p => p.year[0] != '2').ToList();
                dataToShow.Sort((a, b) => (a.date.CompareTo(b.date)));
                LoadData(dataToShow);
            }
            else
            {
                dataToShow = dataFilm.Where(p => p.year == cbFil.Text).ToList();
                dataToShow.Sort((a, b) => (a.date.CompareTo(b.date)));
                LoadData(dataToShow);
            }
        }
        #endregion
    }
}
