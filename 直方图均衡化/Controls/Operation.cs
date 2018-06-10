using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace 直方图均衡化.Controls
{
    public class Operation
    {
        /// <summary>
        /// 用来存储处理后图片
        /// </summary>
        public static Bitmap newbitmap;
        /// <summary>
        /// 均衡化处理
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static Bitmap Equalization(Bitmap bitmap)
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
                    graydense[i] = graydense[i] + graydense[i - 1];
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
                // pictureBox2.Image = newbitmap.Clone() as Image;//显示至pictureBox2
            }
            return newbitmap;
        }
        /// <summary>
        /// 马赛克处理
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static Bitmap Mosaic(Bitmap bitmap)
        {
            if (bitmap != null)
            {
                newbitmap = bitmap.Clone() as Bitmap;
                int RIDIO = 30;//马赛克的尺度，默认为周围两个像素
                for (int h = 0; h < newbitmap.Height; h += RIDIO)
                {
                    for (int w = 0; w < newbitmap.Width; w += RIDIO)
                    {
                        int avgRed = 0, avgGreen = 0, avgBlue = 0;
                        int count = 0;
                        //取周围的像素
                        for (int x = w; (x < w + RIDIO && x < newbitmap.Width); x++)
                        {
                            for (int y = h; (y < h + RIDIO && y < newbitmap.Height); y++)
                            {
                                Color pixel = newbitmap.GetPixel(x, y);
                                avgRed += pixel.R;
                                avgGreen += pixel.G;
                                avgBlue += pixel.B;
                                count++;
                            }
                        }

                        //取平均值
                        avgRed = avgRed / count;
                        avgBlue = avgBlue / count;
                        avgGreen = avgGreen / count;

                        //设置颜色
                        for (int x = w; (x < w + RIDIO && x < newbitmap.Width); x++)
                        {
                            for (int y = h; (y < h + RIDIO && y < newbitmap.Height); y++)
                            {
                                Color newColor = Color.FromArgb(avgRed, avgGreen, avgBlue);
                                newbitmap.SetPixel(x, y, newColor);
                            }
                        }
                    }
                }
            }
            return newbitmap;
        }

    }
}
