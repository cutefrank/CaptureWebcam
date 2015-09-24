using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//加入參考
using Emgu.CV;
using Emgu.CV.Structure;
using System.Drawing.Imaging;
using System.IO;


namespace WindowsFormsApplication1
{
    public partial class CaptureWebcam : Form
    {
        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            timer1.Interval = 10000; //每十秒存一次圖檔
            timer1.Tick += timer1_Tick;
        }

        void timer1_Tick(object sender, EventArgs e)
        {
            //存圖檔
            FileInfo fp = new FileInfo(@"C:\CaptureFiles\" + DateTime.Now.ToString("MMddyyHmmss") + @".jpg");
            FileStream fs = fp.Create();
            pictureBox1.Image.Save(fs, System.Drawing.Imaging.ImageFormat.Jpeg);
            fs.Close();
        }

        private Capture cap = null;// Webcam物件

        public CaptureWebcam()
        {
            InitializeComponent();
            cap = new Capture(0); // 連結到攝影機0，如果你有兩台攝影機，第二台就是1
            Application.Idle += new EventHandler(Application_Idle); // 在Idle的event下，把畫面設定到pictureBox上(當然你也可以用timer事件)
        }

        void Application_Idle(object sender, EventArgs e)
        {
            Image<Bgr, Byte>frame = cap.QueryFrame(); // 去query該畫面
            pictureBox1.Image = frame.ToBitmap(); // 把畫面轉換成bitmap型態，在餵給pictureBox元件
        }

        
        //手動保存圖片
        private void saveBtn_Click(object sender, System.EventArgs e)
        {
            bool isSave = true;
            SaveFileDialog saveImageDialog = new SaveFileDialog();
            saveImageDialog.Title = "圖片保存";
            saveImageDialog.Filter = @"jpeg|*.jpg|bmp|*.bmp|gif|*.gif";

            if (saveImageDialog.ShowDialog() == DialogResult.OK)
            {
                string fileName = saveImageDialog.FileName.ToString();

                if (fileName != "" && fileName != null)
                {
                    string fileExtName = fileName.Substring(fileName.LastIndexOf(".") + 1).ToString();

                    System.Drawing.Imaging.ImageFormat imgformat = null;

                    if (fileExtName != "")
                    {
                        switch (fileExtName)
                        {
                            case "jpg":
                                imgformat = System.Drawing.Imaging.ImageFormat.Jpeg;
                                break;
                            case "bmp":
                                imgformat = System.Drawing.Imaging.ImageFormat.Bmp;
                                break;
                            case "gif":
                                imgformat = System.Drawing.Imaging.ImageFormat.Gif;
                                break;
                            default:
                                MessageBox.Show("只能存取為: jpg,bmp,gif 格式");
                                isSave = false;
                                break;
                        }

                    }

                    //預設保存為JPG格式
                    if (imgformat == null)
                    {
                        imgformat = System.Drawing.Imaging.ImageFormat.Jpeg;
                    }

                    if (isSave)
                    {
                        try
                        {
                            this.pictureBox1.Image.Save(fileName, imgformat);
                            //MessageBox.Show("圖片已經成功保存!");
                        }
                        catch
                        {
                            MessageBox.Show("保存失敗,你還沒有截取過圖片或已經清空圖片!");
                        }
                    }

                }

            }
        }


    }
}
