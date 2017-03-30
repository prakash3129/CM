// BrowserSpeak
//
// by Mark Gladding
// Copyright 2009 Tumbywood Software
// http://www.text2go.com
//
// You are free to reuse this code in any commercial or non-commercial work.
//
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace HttpServer
{
    public class ImageLocator : ResourceLocator
    {
        public ImageLocator(global::System.Resources.ResourceManager resourceManager) :
            base("jpg")
        {
            mResourceManager = resourceManager;
            Extensions.Add("jpeg");
            Extensions.Add("png");
            Extensions.Add("gif");
            Extensions.Add("icon");
        }

        public override void OutputResource(string name, string extension, HttpListenerContext context)
        {
            Image image = null;
            if(name.EndsWith("_disabled"))
            {
                name = name.Substring(0, name.Length - 9);
                if(!mDisabledImages.TryGetValue(name, out image))
                {
                    Image srcImage = mResourceManager.GetObject(name) as Image;
                    if(srcImage != null)
                    {
                        Image disabledImage = srcImage.Clone() as Image;
                        using(Graphics disabledImageGraphics = Graphics.FromImage(disabledImage))
                        {
                            disabledImageGraphics.Clear(Color.Transparent);
                            ControlPaint.DrawImageDisabled(disabledImageGraphics, srcImage, 0, 0, Color.White);
                            mDisabledImages[name] = disabledImage;  // Cache for later
                            image = disabledImage;
                        }
                    }
                }
            }
            else
            {
                image = mResourceManager.GetObject(name) as Image;
            }
            if (image == null)
            {
                context.Response.StatusCode = 404;
                return;
            }

            MemoryStream stream = new MemoryStream();
            image.Save(stream, GetImageFormat(extension));
            byte[] buffer = stream.GetBuffer();
            context.Response.ContentType = string.Format("img/{0}", extension);
            context.Response.ContentLength64 = buffer.Length;
            context.Response.OutputStream.Write(buffer, 0, buffer.Length);
        }

        private ImageFormat GetImageFormat(string extension)
        {
            switch (extension.ToLower())
            {
                case "jpg":
                case "jpeg":
                    return ImageFormat.Jpeg;
                case "png":
                    return ImageFormat.Png;
                case "gif":
                    return ImageFormat.Gif;
                default:
                    return ImageFormat.Bmp;
            }
        }

        private Dictionary<string, Image> mDisabledImages = new Dictionary<string, Image>();
        private global::System.Resources.ResourceManager mResourceManager;
    }
}
