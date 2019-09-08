#ifndef __NET_CURL_H__
#define __NET_CURL_H__

#include <list>
#include <map>

using namespace std;


#include <Windows.h>
#include "curl/libcurl-vc10-x86-release-dll-ipv6-sspi-spnego-winssl/include/curl/curl.h"


class CUrl
{
public:
	CUrl();
	~CUrl();
	static CUrl &GetCUrl();

	static int global_init();
	static void global_cleanup();
	
	void set_share_handle(CURL* curl_handle);

	int initapi();


	/*
	 * 将请求返回的结果打印输出到屏幕.
	 */

	static long call_wirte_func(void *buffer, int size, int nmemb, void *uri);

	static size_t header_callback(const char  *ptr, size_t size, size_t nmemb, void *uri);


	/*
	 * 执行 API.
	 */
	void * requset(void *_uri);

	void get_response_code();
	void get_response_length();
	void get_response_contenttype();

	bool isheadread;//协议头是否已经读取过

	CURLcode code;
	long retcode;
	CURL *curl;
	struct curl_slist *cookies;
	list<string> cookielist;

	bool dataverify;

	string m_sessionid;
	string file_name;

	long file_size ;	
	long request_size;
	string contenttype_str;

	bool isfirstwirte;

};

#endif /* __NET_CURL_H__ */