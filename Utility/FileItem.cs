/************************************************************************
 * 文件标识：  62A87553-312E-426E-9C5C-D898727569A9
 * 项目名称：  Utility  
 * 项目描述：  
 * 类 名 称：  FileItem
 * 版 本 号：  v1.0.0.0 
 * 说    明：  
 * 作    者：  尹自强
 * 创建时间：  2018/07/01 16:24:57
 * 更新时间：  2018/07/01 16:24:57
************************************************************************
 * Copyright @ 尹自强 2018. All rights reserved.
************************************************************************/

using System;
using System.IO;
using System.Text;

namespace Utility
{
    public class FileItem
    {
        private string fileName;
        private string fileFullName;
        private string mimeType;
        private byte[] content;
        private FileInfo fileInfo;
        public bool Exists { get { return this.fileInfo.Exists; } }

        /// <summary>
        /// 基于本地文件的构造器。
        /// </summary>
        /// <param name="fileInfo">本地文件</param>
        public FileItem(FileInfo fileInfo)
        {
            if (fileInfo == null || !fileInfo.Exists)
            {
                throw new ArgumentException("fileInfo is null or not exists!");
            }
            this.fileInfo = fileInfo;
        }

        /// <summary>
        /// 基于本地文件全路径的构造器。
        /// </summary>
        /// <param name="filePath">本地文件全路径</param>
        public FileItem(string filePath)
            : this(new FileInfo(filePath))
        { }

        /// <summary>
        /// 基于文件名和字节流的构造器。
        /// </summary>
        /// <param name="fileName">文件名称（服务端持久化字节流到磁盘时的文件名）</param>
        /// <param name="content">文件字节流</param>
        public FileItem(string fileName, byte[] content)
        {
            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentNullException("fileName");
            if (content == null || content.Length == 0)
                throw new ArgumentNullException("content");

            this.fileName = fileName;
            this.content = content;
        }

        /// <summary>
        /// 基于文件名、字节流和媒体类型的构造器。
        /// </summary>
        /// <param name="fileName">文件名（服务端持久化字节流到磁盘时的文件名）</param>
        /// <param name="content">文件字节流</param>
        /// <param name="mimeType">媒体类型</param>
        public FileItem(string fileName, byte[] content, String mimeType)
            : this(fileName, content)
        {
            if (string.IsNullOrEmpty(mimeType))
                throw new ArgumentNullException("mimeType");
            this.mimeType = mimeType;
        }

        public string GetFileName()
        {
            if (this.fileName == null && this.fileInfo != null && this.fileInfo.Exists)
            {
                this.fileName = this.fileInfo.Name;
            }
            return this.fileName;
        }

        public string GetFileFullName()
        {
            if (this.fileFullName == null && this.fileInfo != null && this.fileInfo.Exists)
            {
                this.fileFullName = this.fileInfo.FullName;
            }
            return this.fileFullName;
        }

        public long GetFileSize()
        {
            if (this.fileInfo != null && this.fileInfo.Exists)
                return fileInfo.Length;
            else
                throw new Exception("GetFileSize fail,error:文件不存在");
        }

        public string GetMimeType()
        {
            if (this.mimeType == null)
            {
                this.mimeType = this.GetMimeType(GetContent());
            }
            return this.mimeType;
        }

        public byte[] GetContent()
        {
            if (this.content == null && this.fileInfo != null && this.fileInfo.Exists)
            {
                using (Stream fileStream = this.fileInfo.OpenRead())
                {
                    this.content = new byte[fileStream.Length];
                    fileStream.Read(content, 0, content.Length);
                }
            }

            return this.content;
        }

        public FileStream GetStream()
        {
            FileStream fileStream = null;
            if (this.content == null && this.fileInfo != null && this.fileInfo.Exists)
                fileStream = this.fileInfo.OpenRead();

            return fileStream;
        }

        public string GetCheckSum(string type = "md5")
        {
            string CheckSum = string.Empty;

            if (type.Equals("md5", StringComparison.OrdinalIgnoreCase))
            {
                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();

                try
                {
                    byte[] bytes = md5.ComputeHash(this.GetContent());

                    StringBuilder result = new StringBuilder();
                    for (int i = 0; i < bytes.Length; i++)
                    {
                        string hex = bytes[i].ToString("X");
                        if (hex.Length == 1)
                        {
                            result.Append("0");
                        }
                        result.Append(hex);
                    }

                    CheckSum = result.ToString();
                }
                catch (Exception ex)
                {
                    throw new Exception("GetCheckSum fail,error:" + ex.Message);
                }
            }

            return CheckSum;
        }

        public FileInfo GetFileInfo()
        {
            if (this.fileInfo != null && this.fileInfo.Exists)
                return this.fileInfo;
            else
                return null;
        }

        /// <summary>
        /// 获取文件的真实媒体类型。目前只支持JPG, GIF, PNG, BMP四种图片文件。
        /// </summary>
        /// <param name="fileData">文件字节流</param>
        /// <returns>媒体类型</returns>
        public string GetMimeType(byte[] fileData)
        {
            string suffix = GetFileSuffix(fileData);
            string mimeType;

            switch (suffix)
            {
                case "JPG":
                mimeType = "image/jpeg";
                break;
                case "GIF":
                mimeType = "image/gif";
                break;
                case "PNG":
                mimeType = "image/png";
                break;
                case "BMP":
                mimeType = "image/bmp";
                break;
                default:
                mimeType = "application/octet-stream";
                break;
            }

            return mimeType;
        }

        /// <summary>
        /// 获取文件的真实后缀名。目前只支持JPG, GIF, PNG, BMP四种图片文件。
        /// </summary>
        /// <param name="fileData">文件字节流</param>
        /// <returns>JPG, GIF, PNG or null</returns>
        public string GetFileSuffix(byte[] fileData)
        {
            if (fileData == null || fileData.Length < 10)
            {
                return null;
            }

            if (fileData[0] == 'G' && fileData[1] == 'I' && fileData[2] == 'F')
            {
                return "GIF";
            }
            else if (fileData[1] == 'P' && fileData[2] == 'N' && fileData[3] == 'G')
            {
                return "PNG";
            }
            else if (fileData[6] == 'J' && fileData[7] == 'F' && fileData[8] == 'I' && fileData[9] == 'F')
            {
                return "JPG";
            }
            else if (fileData[0] == 'B' && fileData[1] == 'M')
            {
                return "BMP";
            }
            else
            {
                return null;
            }
        }

    }
}
