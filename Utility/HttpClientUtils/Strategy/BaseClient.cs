/************************************************************************
 * 文件标识：  131E6AC4-7D98-477B-B58D-33C45A570288
 * 项目名称：  Utility.HttpClientUtils  
 * 项目描述：  
 * 类 名 称：  BaseClient
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
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Utility.HttpClientUtils
{
    /// <summary>
    /// 抽象HttpClient
    /// </summary>
    public abstract class BaseClient
        : Utility.HttpClientUtils.IClientStrategy
    {
        private Encoding _charset;
        private string _format;

        /// <summary>
        /// 内容编码
        /// 默认UTF-8
        /// </summary>
        public Encoding Charset
        {
            get { return this._charset ?? Encoding.UTF8; }
            set { this._charset = value; }
        }

        /// <summary>
        /// 参数格式
        /// 默认hash
        /// </summary>
        public string Format
        {
            get { return this._format ?? "hash"; }
            set { this._format = value; }
        }

        /// <summary>
        /// HttpClient实例
        /// </summary>
        public HttpClient Client { private get; set; }



        /// <summary>
        /// 同步POST
        /// </summary>
        /// <param name="url"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public virtual string Post(string url, object content)
        {
            try
            {
                string result = null;
                string str_content = this.GetContent(content);

                this.Verify(url, str_content);
                this.SetServicePoint(url);

                using (HttpContent httpContent = new StringContent(str_content, this.Charset, "application/x-www-form-urlencoded"))
                {
                    HttpResponseMessage response = this.Client.PostAsync(url, httpContent).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        result = response.Content.ReadAsStringAsync().Result;
                    }
                    else
                    {
                        //LogHelper.Error(string.Format("HttpClientUtils BaseClient Post StatusCode：【{0}】，ReasonPhrase：【{1}】", response.StatusCode, response.ReasonPhrase));
                    }

                    httpContent.Dispose();
                }
                return result;
            }
            catch (HttpRequestException ex)
            {
                //LogHelper.Error(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                //LogHelper.Error(ex.GetType().FullName);
                //LogHelper.Error(ex);
                throw ex;
            }
        }

        /// <summary>
        /// 同步POST文件
        /// </summary>
        /// <param name="url"></param>
        /// <param name="content"></param>
        /// <param name="fileParams">文件列表</param>
        /// <returns></returns>
        public virtual string Post(string url, object content, IDictionary<string, FileItem> fileParams)
        {
            if (fileParams == null || fileParams.Count == 0)
                return Post(url, content);

            try
            {
                string result = null;
                string str_content = this.GetContent(content);

                this.Verify(url, str_content);
                this.SetServicePoint(url);

                using (MultipartFormDataContent httpContent = new MultipartFormDataContent())
                {
                    #region 添加文件

                    IEnumerator<KeyValuePair<string, FileItem>> fileEnum = fileParams.GetEnumerator();
                    while (fileEnum.MoveNext())
                    {
                        string key = fileEnum.Current.Key;
                        FileItem fileItem = fileEnum.Current.Value;

                        //StreamContent streamConent = new StreamContent(fileItem.GetStream());
                        //ByteArrayContent imageContent = new ByteArrayContent(streamConent.ReadAsByteArrayAsync().Result);
                        //imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
                        //httpContent.Add(imageContent, key, fileItem.GetFileName());

                        httpContent.Add(new StreamContent(fileItem.GetStream()), key, fileItem.GetFileName());
                    }

                    #endregion 添加文件

                    #region 添加参数

                    if (content is Hashtable)
                    {
                        Hashtable param = content as Hashtable;
                        foreach (string key in param.Keys)
                        {
                            httpContent.Add(new StringContent(param[key].ToString()), key);
                        }
                    }

                    #endregion 添加参数

                    HttpResponseMessage response = this.Client.PostAsync(url, httpContent).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        result = response.Content.ReadAsStringAsync().Result;
                    }
                    else
                    {
                        //LogHelper.Error(string.Format("HttpClientUtils BaseClient PostFile StatusCode：【{0}】，ReasonPhrase：【{1}】", response.StatusCode, response.ReasonPhrase));
                    }

                    httpContent.Dispose();
                }

                return result;
            }
            catch (System.Threading.ThreadAbortException ex)
            {
                //LogHelper.Error(ex);
                System.Threading.Thread.ResetAbort();
                throw ex;
            }
            catch (HttpRequestException ex)
            {
                //LogHelper.Error(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                //LogHelper.Error(ex.GetType().FullName);
                //LogHelper.Error(ex);
                throw ex;
            }
        }

        /// <summary>
        /// 异步POST
        /// 只发不收
        /// </summary>
        /// <param name="url"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public async virtual void PostAsync(string url, object content)
        {
            try
            {
                string str_content = this.GetContent(content);

                this.Verify(url, str_content);
                this.SetServicePoint(url);

                HttpContent httpContent = new StringContent(str_content, this.Charset, "application/x-www-form-urlencoded");

                await Task.Run(() => {
                    bool Success = this.Client.PostAsync(url, httpContent)
                    .ContinueWith(requestTask => {
                        Task<HttpResponseMessage> response = requestTask;
                        if (response.Status == TaskStatus.RanToCompletion)
                        {
                            HttpResponseMessage result = response.Result;
                            result.EnsureSuccessStatusCode();
                            result.Content.ReadAsStringAsync().ContinueWith(readTask => {
                                //LogHelper.Debug(string.Format("HttpClientUtils BaseClient PostAsync：{0}", readTask.Result));
                            });
                        }
                    }).Wait(millisecondsTimeout: AppConfig.GetValue_Cache("HttpClient_TaskWait").ToInt32());
                });
            }
            catch (System.Threading.ThreadAbortException ex)
            {
                //LogHelper.Error(ex);
                System.Threading.Thread.ResetAbort();
                throw ex;
            }
            catch (HttpRequestException ex)
            {
                //LogHelper.Error(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                //LogHelper.Error(ex.GetType().FullName);
                //LogHelper.Error(ex);
                throw ex;
            }
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
                    _content = WebCommon.BuildHash(content, false);
                    break;
                }
            }

            return _content;
        }

        private void Verify(string url, object content)
        {
            if (Client == null)
                throw new Exception("连接对象为空");

            if (string.IsNullOrWhiteSpace(url))
                throw new Exception("请求地址为空");

            if (content == null)
                throw new Exception("请求内容为空");

            //if (content.GetType() != typeof(string))
            if (!(content is string))
                throw new Exception("请求内容类型错误");
        }

        private void SetServicePoint(string url)
        {
            System.Net.ServicePointManager.Expect100Continue = false;
            var servicePoint = System.Net.ServicePointManager.FindServicePoint(new Uri(url));
            if (servicePoint != null)
            {
                // servicePoint.ConnectionLimit == 2147483647
                servicePoint.ConnectionLeaseTimeout = (1000 * 60 * 5);          //★★修复目标方因蓝绿部署导致的请求失败 默认5分钟回收★★
            }

            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                //System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                //System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
                System.Net.ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
            }
        }

        private bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true;
        }
    }
}
