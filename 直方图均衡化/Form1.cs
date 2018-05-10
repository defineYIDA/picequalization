using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace 直方图均衡化
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        Bitmap bitmap;
        Bitmap newbitmap;
        Stopwatch sw = new Stopwatch();
        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string path = openFileDialog1.FileName;
                bitmap = (Bitmap)Image.FromFile(path);
                pictureBox1.Image = bitmap.Clone() as Image;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            bool isSave = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string fileName = saveFileDialog1.FileName.ToString();

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
                                MessageBox.Show("只能存取为: jpg,bmp,gif 格式");
                                isSave = false;
                                break;
                        }

                    }

                    //默认保存为JPG格式   
                    if (imgformat == null)
                    {
                        imgformat = System.Drawing.Imaging.ImageFormat.Jpeg;
                    }

                    if (isSave)
                    {
                        try
                        {
                            this.pictureBox2.Image.Save(fileName, imgformat);
                            //MessageBox.Show("图片已经成功保存!");   
                        }
                        catch
                        {
                            MessageBox.Show("保存失败,你还没有截取过图片或已经清空图片!");
                        }
                    }
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (bitmap != null)
            {
                newbitmap = bitmap.Clone() as Bitmap;//clone一个副本
                int width = newbitmap.Width;
                int height = newbitmap.Height;
                int size = width * height;
                //总像数个数
                int[] gray = new int[256];
                //定义一个int数组，用来存放各像元值的个数
                double[] graydense = new double[256];
                //定义一个float数组，存放每个灰度像素个数占比
                for (int i = 0; i < width; ++i)
                    for (int j = 0; j < height; ++j)
                    {
                        Color pixel = newbitmap.GetPixel(i, j);
                        //计算各像元值的个数
                        gray[Convert.ToInt16(pixel.R)] += 1;
                        //由于是灰度只读取R值
                    }
                for (int i = 0; i < 256; i++)
                {
                    graydense[i] = (gray[i] * 1.0) / size;
                    //每个灰度像素个数占比
                }

                for (int i = 1; i < 256; i++)
                {
                    graydense[i] = graydense[i]+ graydense[i - 1];
                    //累计百分比
                }
            
                for (int i = 0; i < width; ++i)
                    for (int j = 0; j < height; ++j)
                    {
                        Color pixel = newbitmap.GetPixel(i, j);
                        int oldpixel = Convert.ToInt16(pixel.R);//原始灰度
                        int newpixel = 0;
                        if (oldpixel == 0)
                            newpixel = 0;
                            //如果原始灰度值为0则变换后也为0
                        else
                            newpixel = Convert.ToInt16(graydense[Convert.ToInt16(pixel.R)] * 255);
                            //如果原始灰度不为0，则执行变换公式为   <新像元灰度 = 原始灰度 * 累计百分比>
                        pixel = Color.FromArgb(newpixel, newpixel, newpixel);
                        newbitmap.SetPixel(i, j, pixel);//读入newbitmap
                    }
                pictureBox2.Image = newbitmap.Clone() as Image;//显示至pictureBox2
                
            }
        }

        
    }
}
