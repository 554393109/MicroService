/************************************************************************
 * 文件标识：  CD20AC10-9746-4EDC-B4D8-A7655E4A1D42
 * 项目名称：  Utility.HttpClientUtils  
 * 项目描述：  
 * 类 名 称：  ClassicClient
 * 版 本 号：  v1.0.0.0 
 * 说    明：  
 * 作    者：  尹自强
 * 创建时间：  2018/07/01 16:24:57
 * 更新时间：  2018/07/01 16:24:57
************************************************************************
 * Copyright @ 尹自强 2018. All rights reserved.
************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Utility.HttpClientUtils
{
    /// <summary>
    /// 使用HttpWebRequest和HttpWebResponse兼容低版本.Net
    /// </summary>
    public class ClassicClient
        : Utility.HttpClientUtils.IClient
    {
        //private static ManualResetEvent allDone = new ManualResetEvent(false);

        #region 变量

        private int _timeout = 15 * 1000;
        private Encoding _charset;
        private string _format,
            _cert_filepath,
            _cert_password;

        /// <summary>
        /// 请求超时
        /// 单位：毫秒
        /// 默认15000
        /// </summary>
        public virtual int Timeout
        {
            get { return this._timeout <= 0 ? 15 * 1000 : this._timeout; }
            set { this._timeout = value; }
        }

        /// <summary>
        /// 内容编码
        /// 默认UTF-8
        /// </summary>
        public virtual Encoding Charset
        {
            get { return this._charset == null ? Encoding.UTF8 : this._charset; }
            set { this._charset = value; }
        }

        /// <summary>
        /// 参数格式
        /// 默认hash
        /// </summary>
        public virtual string Format
        {
            get { return this._format == null ? "hash" : this._format; }
            set { this._format = value; }
        }

        /// <summary>
        /// 证书路径
        /// </summary>
        public virtual string CertPath
        {
            get { return this._cert_filepath; }
            set { this._cert_filepath = value; }
        }

        /// <summary>
        /// 证书密码
        /// </summary>
        public virtual string CertPassword
        {
            get { return this._cert_password; }
            set { this._cert_password = value; }
        }

        #endregion 变量

        #region 构造函数

        public ClassicClient() { }

        public ClassicClient(int timeout)
        {
            this.Timeout = timeout;
        }

        public ClassicClient(string cert_filepath, string cert_password)
        {
            if (string.IsNullOrWhiteSpace(cert_filepath))
                throw new Exception(string.Format("证书路径不存在"));

            if (!File.Exists(cert_filepath))
                throw new Exception(string.Format("证书路径【{0}】，文件不存在", cert_filepath));

            this.CertPath = cert_filepath;
            this.CertPassword = cert_password;
        }

        public ClassicClient(int timeout, string cert_filepath, string cert_password)
            : this(cert_filepath, cert_password)
        {
            this.Timeout = timeout;
        }

        public ClassicClient(int timeout, string format, string cert_filepath, string cert_password)
            : this(timeout, cert_filepath, cert_password)
        {
            this.Format = format;
        }

        public ClassicClient(int timeout, string format, Encoding charset, string cert_filepath, string cert_password)
            : this(timeout, format, cert_filepath, cert_password)
        {
            this.Charset = charset;
        }

        #endregion 构造函数

        private HttpWebRequest request = null;
        private HttpWebResponse response = null;
        private Stream reqStream = null;

        public string Post(string url, object content)
        {
            System.GC.Collect();                            //垃圾回收，回收没有正常关闭的http连接
            string result = string.Empty;                   //返回结果

            try
            {
                string str_content = this.GetContent(content);

                this.Verify(url, str_content);
                this.SetRequest(url);

                #region 往目标服务器发送数据

                byte[] postData = this.Charset.GetBytes(str_content);
                request.ContentLength = postData.Length;
                reqStream = request.GetRequestStream();
                reqStream.Write(postData, 0, postData.Length);
                reqStream.Close();

                #endregion 往目标服务器发送数据

                #region 获取目标服务端响应数据

                response = (HttpWebResponse)request.GetResponse();

                #endregion 获取目标服务端响应数据

                #region 读取目标服务器响应数据

                using (System.IO.Stream stream = response.GetResponseStream())
                {
                    try
                    {
                        using (StreamReader reader = new StreamReader(stream, encoding: this.Charset))
                        {
                            try
                            {
                                result = reader.ReadToEnd().Trim();
                            }
                            catch (Exception ex)
                            {
                                //LogHelper.Error(ex);
                                throw ex;
                            }
                            finally
                            {
                                if (reader != null)
                                    reader.Close();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        //LogHelper.Error(ex);
                        throw ex;
                    }
                    finally
                    {
                        if (stream != null)
                            stream.Close();
                    }
                }

                #endregion 读取目标服务器响应数据
            }
            catch (System.Threading.ThreadAbortException ex)
            {
                //LogHelper.Error(ex);
                System.Threading.Thread.ResetAbort();
                throw ex;
            }
            catch (Exception ex)
            {
                //LogHelper.Error(ex);
                throw ex;
            }
            finally
            {
                #region Dispose

                //关闭连接和流
                if (response != null)
                {
                    try
                    {
                        response.Close();
                    }
                    catch (Exception ex)
                    {
                        //LogHelper.Error(ex);
                    }
                }
                if (request != null)
                {
                    try
                    {
                        request.Abort();
                    }
                    catch (Exception ex)
                    {
                        //LogHelper.Error(ex);
                    }
                }

                #endregion Dispose
            }

            return result;
        }

        public string Post(string url, object content, IDictionary<string, FileItem> fileParams)
        {
            throw new NotImplementedException();
        }

        public void PostAsync(string url, object content)
        {
            System.GC.Collect();                            //垃圾回收，回收没有正常关闭的http连接

            try
            {
                string str_content = this.GetContent(content);

                this.Verify(url, str_content);
                this.SetRequest(url);

                Dictionary<string, dynamic> dic_state = new Dictionary<string, dynamic>();
                dic_state.Add("request", request);
                dic_state.Add("content", str_content);

                //request.BeginGetRequestStream(new AsyncCallback(GetRequestStreamCallback), request);
                request.BeginGetRequestStream(new AsyncCallback(GetRequestStreamCallback), dic_state);
            }
            catch (System.Threading.ThreadAbortException ex)
            {
                //LogHelper.Error(ex);
                System.Threading.Thread.ResetAbort();
                throw ex;
            }
            catch (Exception ex)
            {
                //LogHelper.Error(ex);
                throw ex;
            }

            //allDone.WaitOne();
        }

        private void GetRequestStreamCallback(IAsyncResult asynchronousResult)
        {
            //HttpWebRequest request = (HttpWebRequest)asynchronousResult.AsyncState;
            Dictionary<string, dynamic> dic_state = asynchronousResult.AsyncState as Dictionary<string, dynamic>;
            request = dic_state["request"];

            // End the operation
            Stream postStream = request.EndGetRequestStream(asynchronousResult);

            string postData = dic_state["content"];

            // Convert the string into a byte array.
            byte[] byteArray = this.Charset.GetBytes(postData);

            // Write to the request stream.
            postStream.Write(byteArray, 0, postData.Length);
            postStream.Close();

            // Start the asynchronous operation to get the response
            request.BeginGetResponse(new AsyncCallback(GetResponseCallback), request);

        }

        private void GetResponseCallback(IAsyncResult asynchronousResult)
        {
            try
            {
                request = (HttpWebRequest)asynchronousResult.AsyncState;

                // End the operation
                response = (HttpWebResponse)request.EndGetResponse(asynchronousResult);

                using (Stream stream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(stream: stream, encoding: this.Charset))
                    {
                        try
                        {
                            string result = reader.ReadToEnd();
                            //LogHelper.Debug(string.Format("GetResponseCallback {0:yyyy-MM-dd HH:mm:ss.ffff}", DateTime.Now));
                        }
                        catch (Exception ex)
                        {
                            //LogHelper.Error(ex);
                            throw ex;
                        }
                        finally
                        {
                            if (reader != null)
                                reader.Close();
                        }

                        // Close the stream object
                        if (stream != null)
                            stream.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                //LogHelper.Error(ex);
                throw ex;
            }
            finally
            {
                // Release the HttpWebResponse
                if (response != null)
                {
                    try
                    {
                        response.Close();
                    }
                    catch (Exception ex)
                    {
                        //LogHelper.Error(ex);
                    }
                }

                if (request != null)
                {
                    try
                    {
                        request.Abort();
                    }
                    catch (Exception ex)
                    {
                        //LogHelper.Error(ex);
                    }
                }
            }

            //allDone.Set();
        }



        /// <summary>
        /// 构造postData
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        private string GetContent(dynamic content)
        {
            string _content = string.Empty;

            if (content is string)
                return Convert.ToString(content);

            if (content is IDictionary)
            {
                switch (this.Format)
                {
                    case "xml":
                    _content = WebCommon.BuildXml(content);
                    break;

                    case "hash":
                    default:
                    _content = WebCommon.BuildHash(content);
                    break;
                }
            }

            return _content;
        }

        private void Verify(string url, object content)
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new Exception("请求地址为空");

            if (content == null)
                throw new Exception("请求内容为空");

            if (!(content is string))
                throw new Exception("请求内容类型错误");
        }

        /// <summary>
        /// 设置HttpWebRequest
        /// </summary>
        /// <param name="url"></param>
        private void SetRequest(string url)
        {
            Uri uri = new Uri(url);
            //设置最大连接数
            ServicePointManager.DefaultConnectionLimit = 1024;

            var servicePoint = System.Net.ServicePointManager.FindServicePoint(uri);
            if (servicePoint != null)
                servicePoint.ConnectionLeaseTimeout = (1000 * 60 * 5);          //★★修复目标方因蓝绿部署导致的请求失败 默认5分钟回收★★

            //设置https验证方式
            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                request = (HttpWebRequest)WebRequest.CreateDefault(uri);
            }
            else
            {
                request = (HttpWebRequest)WebRequest.Create(uri);
            }

            request.ServicePoint.Expect100Continue = false;
            request.KeepAlive = false;
            request.Timeout = this.Timeout;
            request.ContentType = "application/x-www-form-urlencoded;charset=" + this.Charset;
            request.Method = "POST";
            request.UserAgent = "Cysoft.PayProxy";
            request.Headers["DNT"] = "1";


            //设置代理服务器
            //WebProxy proxy = new WebProxy();                            //定义一个网关对象
            //proxy.Address = new Uri("PROXY_URL");                       //网关服务器端口:端口
            //request.Proxy = proxy;
            request.Proxy = null;

            //是否使用证书
            if (!string.IsNullOrWhiteSpace(this.CertPath))
            {
                X509Certificate cer = new X509Certificate(this.CertPath, this.CertPassword);
                request.ClientCertificates.Add(cer);
            }
        }

        private bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true;
        }

    }
}
