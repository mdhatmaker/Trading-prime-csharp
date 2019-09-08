
//#pragma comment(lib, "..\\..\\..\\..\\libcurl-vc10-x86-release-dll-ipv6-sspi-spnego-winssl\\libcurl_imp.lib")
#include "curl.h"
#include <list>
using namespace std;
#include "uri.h"
#include "string.h"

CUrl::CUrl()
{
	isheadread = 0;
	retcode = 0;
	cookies = 0;
	code = CURLE_OK;
	curl = 0;
	dataverify = false;
	file_size = 0;
	request_size = 0;
	isfirstwirte = true;
}


CUrl::~CUrl()
{
	cookies = NULL;
}

CUrl &CUrl::GetCUrl()
{
	static CUrl s_CUrl;
	return s_CUrl;
};


int CUrl::global_init()
{
	CURLcode code = curl_global_init(CURL_GLOBAL_ALL);
	if (code != CURLE_OK) 
	{
		return 2;
	}
	return 1;
}

void CUrl::global_cleanup()
{
	curl_global_cleanup();
}

void CUrl::set_share_handle(CURL* curl_handle)
{
	static CURLSH* share_handle = NULL;
	if (!share_handle)
	{
		share_handle = curl_share_init();
		curl_share_setopt(share_handle, CURLSHOPT_SHARE, CURL_LOCK_DATA_DNS);
	}
	curl_easy_setopt(curl_handle, CURLOPT_SHARE, share_handle);
	curl_easy_setopt(curl_handle, CURLOPT_DNS_CACHE_TIMEOUT, 60 * 5);
}

/*
 * 将请求返回的结果打印输出到屏幕.
 */
long CUrl::call_wirte_func(void *buffer, int size, int nmemb, void *uri)
{

	long count = size * nmemb;
	
	Uri *puri = (Uri *)uri;
	CUrl *pcurl = (CUrl *)puri->pcurl;


	if (pcurl != 0 && pcurl->isfirstwirte == true)
	{
		pcurl->get_response_code();
		pcurl->get_response_length();
		pcurl->get_response_contenttype();

		puri->request_size =	pcurl->request_size;
		puri->request_cursize =	0;
	}

	if (pcurl->request_size > 0)
	{
		if (puri->curbuf == NULL)
		{
			puri->curbuf = (char *)malloc(pcurl->request_size+1);
		}
		memcpy(puri->curbuf + puri->request_cursize,(const char *)buffer, count);
		puri->request_cursize += count;
		//puri->curbuf_allocsize = pcurl->request_size;
		puri->curbuf_size += count;
		puri->curbuf[puri->request_size] = 0;
	}
	else
	{
		if (puri->curbuf == NULL)
		{
			puri->curbuf = (char *)malloc(count+1);
			memcpy(puri->curbuf, buffer, count);
			puri->request_cursize = count;
			puri->request_size = count;
			//puri->curbuf_allocsize = count;
			puri->curbuf_size = count;
		} 
		else
		{
			puri->curbuf = (char *)realloc(puri->curbuf, puri->request_size + count+1);
			memcpy(puri->curbuf + puri->request_size , buffer, count);
			puri->request_cursize += count;
			puri->request_size += count;
			//puri->curbuf_allocsize += count;
			puri->curbuf_size += count;
		}
		puri->curbuf[puri->request_size] = 0;
	}

	pcurl->isfirstwirte = false;
	return count;
}

/*
 * 一旦接收到http 头部数据后将调用该函数。
 * 返回http header回调函数  
 */
   
size_t CUrl::header_callback(const char  *ptr, size_t size, size_t nmemb, void *uri)     
{
	long count = size * nmemb;

	Uri *puri = (Uri *)uri;

	CUrl *pcurl = (CUrl *)puri->pcurl;
	if (pcurl != 0)
	{
		string str_header(ptr, count);
		string headername;
		string headervalue;
	}

    return count;   
}    


/*
 * 执行 API.
 */
void * CUrl::requset(void *uri)
{

	Uri *puri = (Uri *)uri;

	char errbuf[CURL_ERROR_SIZE];
	string urlstr;
	string params;
	//Uri *uri = new ResponseData;
	puri->pcurl = this;

	set_share_handle(curl);

	//curl = curl_multi_init();
	curl = curl_easy_init();

	if (curl == NULL) {
		puri->errcode = CURLE_FAILED_INIT;
		return puri;
	}
	//curl_easy_setopt(curl, CURLOPT_VERBOSE, 1);
	curl_easy_setopt(curl, CURLOPT_ERRORBUFFER, errbuf);
	urlstr = puri->GetUrl();
	string cookie_file = "";

	// 设置url
	if (puri->method == METHOD_POST)
	{
		curl_easy_setopt(curl, CURLOPT_URL, urlstr.c_str());
	}

	if (puri->method == METHOD_GET)
	{
		params = puri->GetParamSet();
		if (!params.empty())
		{
			params = "?" + params;
			urlstr = urlstr + params;
		}
		curl_easy_setopt(curl, CURLOPT_URL, urlstr.c_str());
	}
	
	if (puri->protocol == HTTP_PROTOCOL_HTTPS)
	{
		////////////////////////
		////////////////////////// 跳过服务器SSL验证，不使用CA证书
		////////////////////////curl_easy_setopt(curl, CURLOPT_SSL_VERIFYPEER, 0);
		////////////////////////// 验证服务器端发送的证书，默认是 2(高), 1(中), 0(禁用)
		////////////////////////curl_easy_setopt(curl, CURLOPT_SSL_VERIFYHOST, 2L);
		////////////////////////		
		

		wstring wstrfilname = str::GetModuleDirectory();
		wstrfilname += L"root.pem";
		string strfilname = str::UnicodeToAnsi(wstrfilname);
	
		if (!strfilname.empty())
		{
			curl_easy_setopt(curl, CURLOPT_SSL_VERIFYPEER, true);   	// 只信任CA颁布的证书  
			curl_easy_setopt(curl, CURLOPT_CAINFO,strfilname.c_str());				// CA根证书（用来验证的网站证书是否是CA颁布）  
			curl_easy_setopt(curl, CURLOPT_SSL_VERIFYHOST, 2);			// 检查证书中是否设置域名，并且是否与提供的主机名匹配 
		} 
		else
		{
			curl_easy_setopt(curl, CURLOPT_SSL_VERIFYPEER, false);	 	// 信任任何证书  
			curl_easy_setopt(curl, CURLOPT_SSL_VERIFYHOST, 1); 			// 检查证书中是否设置域名  
		}
	}


	curl_easy_setopt(curl, CURLOPT_NOSIGNAL, 1);  //http://blog.chinaunix.net/uid-23145525-id-4343403.html 解决方式是禁用掉alarm这种超时， curl_easy_setopt(curl, CURLOPT_NOSIGNAL, 1L)。

	curl_easy_setopt(curl, CURLOPT_FORBID_REUSE, 1); //默认情况下libcurl完成一个任务以后，出于重用连接的考虑不会马上关闭		如果没有新的TCP请求来重用这个连接，那么只能等到CLOSE_WAIT超时，这个时间默认在7200秒甚至更高，太多的CLOSE_WAIT连接会导致性能问题,解决方法：		curl_easy_setopt(curl, CURLOPT_FORBID_REUSE, 1); 	最好再修改一下TCP参数调低CLOSE_WAIT和TIME_WAIT的超时时间

	// 设置301、302跳转跟随location  
    curl_easy_setopt(curl, CURLOPT_FOLLOWLOCATION, 1);
	//30秒超时
	curl_easy_setopt(curl, CURLOPT_TIMEOUT, 20);   //只需要设置一个秒的数量就可以
	//curl_easy_setopt(curl, CURLOPT_NOSIGNAL, 1);    //注意，毫秒超时一定要设置这个 
	//curl_easy_setopt(curl, CURLOPT_TIMEOUT_MS, 200);  //超时毫秒，cURL 7.16.2中被加入。从PHP 5.2.3起可使用 


	// 设置获取返回内容的回调函数
	curl_easy_setopt(curl, CURLOPT_WRITEFUNCTION, &CUrl::call_wirte_func);
	// 设置获取返回内容的缓冲区
	curl_easy_setopt(curl, CURLOPT_WRITEDATA, puri);

	//抓取头信息，回调函数   
	curl_easy_setopt(curl, CURLOPT_HEADERFUNCTION, &CUrl::header_callback );   
	curl_easy_setopt(curl, CURLOPT_HEADERDATA, puri);   


	curl_easy_setopt(curl, CURLOPT_USERAGENT, "Mozilla/5.0 (xaioguxian; rv:1.0.0) xaioguxian"); 


	struct curl_slist *headers = NULL; /* init to NULL is important */


	if (puri->method == METHOD_POST)
	{
		params = puri->GetParamSet();
		
		// 设置post参数
		curl_easy_setopt(curl, CURLOPT_POST, 1);
		curl_easy_setopt(curl, CURLOPT_POSTFIELDS, params.c_str() != NULL ? params.c_str() : "");
	}
	if (puri->method == METHOD_GET) {
		curl_easy_setopt(curl, CURLOPT_HTTPGET, true);
	}

	puri->errcode = curl_easy_perform(curl);/* transfer http */
	
	/*
	if (headers != 0)
	{
		curl_slist_free_all(headers); // free the header list
	}
	*/

    if (CURLE_OK == puri->errcode) 
	{
		// 已获得返回的数据
		puri->GetUTF8Result();
	}
	
	curl_easy_cleanup(curl);
	curl = 0;
	return puri;
}


void CUrl::get_response_code()
{
	code = curl_easy_getinfo(curl, CURLINFO_RESPONSE_CODE , &retcode);
}

void CUrl::get_response_length()
{
	double length;
	code = curl_easy_getinfo(curl, CURLINFO_CONTENT_LENGTH_DOWNLOAD , &length);
	request_size = length;
}

void CUrl::get_response_contenttype()
{
	char *pbuff = NULL;
	code = curl_easy_getinfo(curl,CURLINFO_CONTENT_TYPE,&pbuff); //获取内容
	if (code == CURLE_OK && pbuff)								 //做好检查，大家都好
	{
		contenttype_str = pbuff;
	}
}
