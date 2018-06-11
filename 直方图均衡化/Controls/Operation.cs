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
        /// <summary>
        /// 暗角处理
        /// </summary>
        public static Bitmap Darkangle(Bitmap bitmap)
        {
            if (bitmap != null)
            {
                newbitmap = bitmap.Clone() as Bitmap;
                int width = newbitmap.Width;
                int height = newbitmap.Height;
                float cx = width / 2;
                float cy = height / 2;
                float maxDist = cx * cx + cy * cy;
                float currDist = 0, factor;
                Color pixel;

                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        currDist = ((float)i - cx) * ((float)i - cx) + ((float)j - cy) * ((float)j - cy);
                        factor = currDist / maxDist;

                        pixel = newbitmap.GetPixel(i, j);
                        int red = (int)(pixel.R * (1 - factor));
                        int green = (int)(pixel.G * (1 - factor));
                        int blue = (int)(pixel.B * (1 - factor));
                        newbitmap.SetPixel(i, j, Color.FromArgb(red, green, blue));
                    }
                }
            }
            return newbitmap;
        }
        /// <summary>
        /// 亮度降低
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static Bitmap Light_Reduction(Bitmap bitmap)
        {
            if (bitmap != null)
            {
                newbitmap = bitmap.Clone() as Bitmap;
                Color pixel;
                int red, green, blue;
                for (int x = 0; x < newbitmap.Width; x++)
                {
                    for (int y = 0; y < newbitmap.Height; y++)
                    {
                        pixel = newbitmap.GetPixel(x, y);
                        red = (int)(pixel.R * 0.6);
                        green = (int)(pixel.G * 0.6);
                        blue = (int)(pixel.B * 0.6);
                        newbitmap.SetPixel(x, y, Color.FromArgb(red, green, blue));
                    }
                }
            }
            return newbitmap;
        }
        /// <summary>
        /// 浮雕
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static Bitmap Cameo(Bitmap bitmap)
        {
            if (bitmap != null)
            {
                newbitmap = bitmap.Clone() as Bitmap;
                Color pixel;
                int red, green, blue;
                for (int x = 0; x < newbitmap.Width; x++)
                {
                    for (int y = 0; y < newbitmap.Height; y++)
                    {
                        pixel = newbitmap.GetPixel(x, y);
                        red = (int)(255 - pixel.R);
                        green = (int)(255 - pixel.G);
                        blue = (int)(255 - pixel.B);
                        newbitmap.SetPixel(x, y, Color.FromArgb(red, green, blue));
                    }
                }
            }
            return newbitmap;
        }
        /// <summary>
        /// 扩散
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static Bitmap Spread(Bitmap bitmap)
        {
            if (bitmap != null)
            {
                newbitmap = bitmap.Clone() as Bitmap;
                Color pixel;
                int red, green, blue;
                int flag = 0;
                for (int x = 0; x < newbitmap.Width; x++)
                {
                    for (int y = 0; y < newbitmap.Height; y++)
                    {
                        Random ran = new Random();
                        int RankKey = ran.Next(-5, 5);
                        if (x + RankKey >= newbitmap.Width || y + RankKey >= newbitmap.Height || x + RankKey < 0 || y + RankKey < 0)
                        {
                            flag = 1;
                            continue;
                        }

                        pixel = newbitmap.GetPixel(x + RankKey, y + RankKey);
                        red = (int)(pixel.R);
                        green = (int)(pixel.G);
                        blue = (int)(pixel.B);
                        newbitmap.SetPixel(x, y, Color.FromArgb(red, green, blue));
                    }
                }
            }
            return newbitmap;
        }
        /// <summary>
        /// 去色
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static Bitmap Decoloration(Bitmap bitmap)
        {
            if (bitmap != null)
            {
                newbitmap = bitmap.Clone() as Bitmap;
                Color pixel;
                int gray;
                for (int x = 0; x < newbitmap.Width; x++)
                {
                    for (int y = 0; y < newbitmap.Height; y++)
                    {
                        pixel = newbitmap.GetPixel(x, y);
                        gray = (int)(0.3 * pixel.R + 0.59 * pixel.G + 0.11 * pixel.B);
                        newbitmap.SetPixel(x, y, Color.FromArgb(gray, gray, gray));
                    }
                }
            }
            return newbitmap;
        }
        /// <summary>
        /// 翻转
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static Bitmap RotateFlip(Bitmap bitmap)
        {
            RotateFlipType rotateFlipType = RotateFlipType.Rotate180FlipY;
            newbitmap = bitmap.Clone() as Bitmap;
            newbitmap.RotateFlip(rotateFlipType);
            return newbitmap;
        }

    }
}
