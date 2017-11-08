using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CallCenterLocal.Control
{
    public class HttpManager
    {
        const int bytebuff = 10240;
        const int ReadWriteTimeOut = 2 * 1000;//超时等待时间
        const int TimeOutWait = 5 * 1000;//超时等待时间
        const int MaxTryTime = 12;
        static Dictionary<string, int> TryNumDic = new Dictionary<string, int>();

        /// <summary>
        /// 下载文件（同步）  支持断点续传
        /// </summary>
        /// <param name="url">文件url</param>
        /// <param name="savepath">本地保存路径</param>
        /// <param name="progress">下载进度（百分比）</param>
        /// <param name="size">下载文件大小</param>
        public static void DowLoadFile(string url, string savepath, UZipProgressDelegate uZipProgress,long size = 0 )
        {
            //打开上次下载的文件
            long lStartPos = 0;
            FileStream fs;
            if (File.Exists(savepath))
            {
                //fs = File.OpenWrite(savepath);
                //lStartPos = fs.Length;
                //fs.Seek(lStartPos, SeekOrigin.Current);//移动文件流中的当前指针
                File.Delete(savepath);
            }

            string direName = Path.GetDirectoryName(savepath);
            if (!Directory.Exists(direName))//如果不存在保存文件夹路径，新建文件夹
            {
                Directory.CreateDirectory(direName);
            }
            fs = new FileStream(savepath, FileMode.Create);
            lStartPos = 0;

            HttpWebRequest request = null;
            try
            {
                if (size == 0)
                {
                    size = GetFileContentLength(url);
                    if (size < 0)
                        return;
                }
                if (size != 0 && size == lStartPos)
                {
                    //下载完成
                    fs.Close();
                    return;
                }

                request = (HttpWebRequest)WebRequest.Create(url);
                request.ReadWriteTimeout = ReadWriteTimeOut;
                request.Timeout = TimeOutWait;
                request.Headers.Add(HttpControl.azHeader, HttpControl.GetFileAutohrization);
                if (lStartPos > 0)
                    request.AddRange((int)lStartPos);//设置Range值，断点续传

                //向服务器请求，获得服务器回应数据流
                WebResponse respone = request.GetResponse();
                long totalSize = respone.ContentLength + lStartPos;
                long curSize = lStartPos;

                Stream ns = respone.GetResponseStream();

                byte[] nbytes = new byte[bytebuff];
                int nReadSize = 0;
                while (nReadSize < respone.ContentLength)
                {
                    nReadSize = ns.Read(nbytes, 0, bytebuff);
                    fs.Write(nbytes, 0, nReadSize);

                    curSize += nReadSize;
                    //下载进度计算
                    uZipProgress?.Invoke(curSize,totalSize);
                }
                fs.Flush();
                ns.Close();
                fs.Close();
                if (curSize != totalSize)//文件长度不等于下载长度，下载出错
                {
                    throw new Exception();
                }
                if (request != null)
                {
                    request.Abort();
                }
                TryNumDic.Remove(url);
            }
            catch(Exception ex)
            {
                if (request != null)
                {
                    request.Abort();
                }

                fs.Close();
                if (TryNumDic.ContainsKey(url))
                {
                    if (TryNumDic[url] > MaxTryTime)
                    {
                        TryNumDic.Remove(url);
                        throw new Exception();
                    }
                    else
                    {
                        TryNumDic[url]++;
                    }
                }
                else
                {
                    TryNumDic.Add(url, 1);
                }
                //DowLoadFile(url, savepath, uZipProgress,size);
            }
        }

        public static string HttpDownloadFile(string url, string path, UZipProgressDelegate uZipProgress)
        {
            try
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
                String filePath = path.Remove(path.LastIndexOf("\\") + 1);
                if (!Directory.Exists(filePath))
                    Directory.CreateDirectory(filePath);
                long total = GetFileContentLength(url);

                // 设置参数
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;

                //发送请求并获取相应回应数据
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                request.Headers.Add(HttpControl.azHeader, HttpControl.GetFileAutohrization);
                //直到request.GetResponse()程序才开始向目标网页发送Post请求
                Stream responseStream = response.GetResponseStream();
                Console.WriteLine("开始下载流!");

                //创建本地文件写入流
                Stream stream = new FileStream(path, FileMode.Create);
                Console.WriteLine("开始文件流!");
                byte[] bArr = new byte[bytebuff];
                int size = responseStream.Read(bArr, 0, (int)bArr.Length);
                long currentSize = 0;
                DateTime messageTime = DateTime.Now;
                while (size > 0)
                {
                    stream.Write(bArr, 0, size);
                    size = responseStream.Read(bArr, 0, (int)bArr.Length);
                    currentSize += size;
                    var timeSpan = (DateTime.Now - messageTime).TotalMilliseconds;
                    if(timeSpan > 500)
                    {
                        uZipProgress(currentSize, total);
                        messageTime = DateTime.Now;
                    }
                }
                stream.Close();
                Console.WriteLine("关闭文件流!");
                responseStream.Close();
                Console.WriteLine("关闭下载流!");
                uZipProgress(currentSize, total);
            }
            catch (Exception ex)
            {
                String message = ex.Message;
                Console.WriteLine(message);
                throw (ex);
            }
            return path;
        }

        /// <summary>
        /// 获取下载文件长度
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static long GetFileContentLength(string url)
        {
            HttpWebRequest request = null;
            try
            {
                request = (HttpWebRequest)HttpWebRequest.Create(url);
                request.Headers.Add(HttpControl.azHeader, HttpControl.GetFileAutohrization);
                request.Timeout = TimeOutWait;
                request.ReadWriteTimeout = ReadWriteTimeOut;
                //向服务器请求，获得服务器回应数据流
                WebResponse respone = request.GetResponse();
                request.Abort();
                return respone.ContentLength;
            }
            catch (Exception e)
            {
                if (request != null)
                    request.Abort();
                return -1;
            }
        }
    }
}
