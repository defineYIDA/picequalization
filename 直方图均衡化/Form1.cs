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
using 直方图均衡化.Controls;

namespace 直方图均衡化
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            //设置combox的初始项
            comboBox1.SelectedIndex = 0;
        }
        /// <summary>
        /// 存储原始图像
        /// </summary>
        Bitmap bitmap;
        /// <summary>
        /// 存储处理后图像
        /// </summary>
        Bitmap newbitmap;
        /// <summary>
        /// 打开文件按钮，点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string path = openFileDialog1.FileName;
                bitmap = (Bitmap)Image.FromFile(path);
                pictureBox1.Image = bitmap.Clone() as Image;
            }
        }
        /// <summary>
        /// 保存图片按钮，点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <summary>
        /// 双击浏览原始图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
            Showpicture showpicture = new Showpicture();
            showpicture.picture.Image = bitmap.Clone() as Image;
            showpicture.Height = bitmap.Height;
            showpicture.Width = bitmap.Width;
            showpicture.Show();
        }
        /// <summary>
        /// 双击浏览处理后图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox2_DoubleClick(object sender, EventArgs e)
        {

            Showpicture showpicture = new Showpicture();
            showpicture.picture.Image = newbitmap.Clone() as Image;
            showpicture.Height = newbitmap.Height;
            showpicture.Width = newbitmap.Width;
            showpicture.Show();

        }
        /// <summary>
        /// 执行操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button10_Click(object sender, EventArgs e)
        {
            if(radioButton1.Checked == true)
            {
                newbitmap = Operation.Equalization(bitmap);
                if(newbitmap != null)pictureBox2.Image = newbitmap.Clone() as Image;//显示至pictureBox2
                else { MessageBox.Show("原始图片为空！", "提示："); }
            }
            if(radioButton3.Checked == true)
            {
                newbitmap = Operation.Mosaic(bitmap);
                if (newbitmap != null) pictureBox2.Image = newbitmap.Clone() as Image;//显示至pictureBox2
                else { MessageBox.Show("原始图片为空！", "提示："); }
            }
        }
        /// <summary>
        /// 版本和git地址标签
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkLabel1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/1158558425/picequalization.git");
        }
        /// <summary>
        /// 清理视图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            if(comboBox1.SelectedItem.ToString() == "全部")
            {
                bitmap = null;
                newbitmap = null;
                Operation.newbitmap = null;
                pictureBox1.Image = null;
                pictureBox2.Image = null;
                MessageBox.Show("已全部清空！", "提示：");
            }
            else if (comboBox1.SelectedItem.ToString() == "原始图像")
            {
                bitmap = null;
                pictureBox1.Image = null;
                MessageBox.Show("原始图已清空！", "提示：");
            }
            else
            {
                newbitmap = null;
                pictureBox2.Image = null;
                Operation.newbitmap = null;
                MessageBox.Show("处理图已清空！", "提示：");
            }


        }
    }
}
