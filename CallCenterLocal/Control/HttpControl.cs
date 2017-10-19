using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using CallCenterLocal.Data;


namespace CallCenterLocal.Control
{
    public class HttpControl
    {
        public const string GetWorkflowCmd = "http://123.59.82.44:8080/main-flow/list.do";
        public const string GetNeedCallPhoneCmd = Form1.server + "/api/view/task/get/call/";
        public const string TestWorkflowCmd = Form1.server + "/api/view/task/tocall/";
        public const string GetFtpInfoCmd = "http://106.75.65.223" + "/api/file/geturl/";

        public static string ObjectToJson(Object obj)
        {
            JavaScriptSerializer jsonSerialize = new JavaScriptSerializer();
            return jsonSerialize.Serialize(obj);
        }
        // 反序列化
        public static Object JsonToObject<T>(string jsonStr)
        {
            JavaScriptSerializer jsonSerialize = new JavaScriptSerializer();
            return jsonSerialize.Deserialize<T>(jsonStr);
        }

        public static List<T> JSONStringToList<T>(string JsonStr)

        {

            JavaScriptSerializer Serializer = new JavaScriptSerializer();
            List<T> objs = Serializer.Deserialize<List<T>>(JsonStr);

            return objs;

        }
        public static object PostMoths(string url, string param,Token token)
        {
            try
            {
                //string tempToken = "a08074ead1a8f3b4ffa42895b28d02938f3aacbf";
                string strURL = url;
                System.Net.HttpWebRequest request;
                request = (System.Net.HttpWebRequest)WebRequest.Create(strURL);
                request.Method = "POST";
                request.ContentType = "application/json;charset=UTF-8";
                if(url != GetWorkflowCmd)
                    request.Headers.Add("Authorization", "Token " + token.token);
                string paraUrlCoded = param;
                byte[] payload;
                payload = System.Text.Encoding.UTF8.GetBytes(paraUrlCoded);
                request.ContentLength = payload.Length;
                Stream writer = request.GetRequestStream();
                writer.Write(payload, 0, payload.Length);
                writer.Close();
                System.Net.HttpWebResponse response;
                response = (System.Net.HttpWebResponse)request.GetResponse();
                System.IO.Stream s;
                s = response.GetResponseStream();
                string StrDate = "";
                string strValue = "";
                StreamReader Reader = new StreamReader(s, Encoding.UTF8);
                while ((StrDate = Reader.ReadLine()) != null)
                {
                    strValue += StrDate + "\r\n";
                }
                switch(url)
                {
                    case GetWorkflowCmd:
                        {
                            var ret =  JsonToObject<ResultWorkflows>(strValue);
                            return ret;
                        }
                        break;
                    case TestWorkflowCmd:
                        {
                            var ret = JsonToObject<TestWorkflowResult>(strValue);
                            return ret;
                        }
                        break;
                    case GetFtpInfoCmd:
                        {
                            var ret = JsonToObject<ResultFtpInfo>(strValue);
                            return ret;
                        }
                        break;
                    default:
                        return strValue;
                }
            }
            catch(Exception e)
            {
                return null;
            }
        }

        public static string GetHttpResponseList<T>(string url, int Timeout,string token)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            try
            {
                request.Method = "GET";
                request.ContentType = "application/json;charset=UTF-8 ";
                request.Headers.Add("Authorization", "Token " + token);
                //request.Headers["Authorization"] = "Token " + token;
                //request.PreAuthenticate = true;
                //request.UserAgent = null;
                request.Timeout = Timeout;

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream myResponseStream = response.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
                string retString = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                myResponseStream.Close();
                
                return retString;
            }
            catch(Exception e)
            {
                throw (e);
            }

        }

        public static HttpWebResponse CreatePostHttpResponse(string url, IDictionary<string, string> parameters, int timeout, string userAgent, CookieCollection cookies)
        {
            HttpWebRequest request = null;
            //如果是发送HTTPS请求  
            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                request = WebRequest.Create(url) as HttpWebRequest;
            }
            else
            {
                request = WebRequest.Create(url) as HttpWebRequest;
            }
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";

            //设置代理UserAgent和超时
            //request.UserAgent = userAgent;
            //request.Timeout = timeout; 

            if (cookies != null)
            {
                request.CookieContainer = new CookieContainer();
                request.CookieContainer.Add(cookies);
            }
            //发送POST数据  
            if (!(parameters == null || parameters.Count == 0))
            {
                StringBuilder buffer = new StringBuilder();
                int i = 0;
                foreach (string key in parameters.Keys)
                {
                    if (i > 0)
                    {
                        buffer.AppendFormat("&{0}={1}", key, parameters[key]);
                    }
                    else
                    {
                        buffer.AppendFormat("{0}={1}", key, parameters[key]);
                        i++;
                    }
                }
                byte[] data = Encoding.ASCII.GetBytes(buffer.ToString());
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
            }
            string[] values = request.Headers.GetValues("Content-Type");
            return request.GetResponse() as HttpWebResponse;
        }

        /// <summary>
        /// 获取请求的数据
        /// </summary>
        public static string GetResponseString(HttpWebResponse webresponse)
        {
            using (Stream s = webresponse.GetResponseStream())
            {
                StreamReader reader = new StreamReader(s, Encoding.UTF8);
                return reader.ReadToEnd();

            }
        }

    }

}
