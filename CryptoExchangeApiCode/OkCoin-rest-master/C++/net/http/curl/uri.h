#ifndef __URL_H__
#define __URL_H__

#include <string>
#include <map>
#include <vector>
#include <list>
#include <algorithm>
#include "curl.h"

using namespace std;

typedef void (*Url_CallBak)(void *_uri);

enum CURL_REQUEST_SAVE_TO
{
	CURL_REQUEST_SAVE_TO_MEMROY,
	CURL_REQUEST_SAVE_TO_FILE
};

enum HTTP_PROTOCOL
{
	HTTP_PROTOCOL_HTTP, HTTP_PROTOCOL_HTTPS 
};

const enum HTTP_METHOD
{
	METHOD_GET, METHOD_POST 
};

class CUriParameter
{
public:
	CUriParameter();
	~CUriParameter();
	map<string, string> m_items;

public:
	string Get(const string name) const;
	list<string> GetArray(const string name) const;
	string ToString();

	void Clear();
	void Add(const char *name,const char *value);

	void AddNoEncode(const char *name,const char *value);

	void Set(const char *name,const char *value);
};


class Uri
{
public:
	Uri();
	~Uri();
	CURL_REQUEST_SAVE_TO saveto;
	bool isinit;
	int type;
	char *protoclstr;
	char *domain;
	string api;
	string url;
	HTTP_PROTOCOL protocol;
	HTTP_METHOD method;

	CUriParameter m_param;
	CUriParameter m_header;

	string result;

	CURLcode errcode;

	void * pcurl;

	long request_size;			//请求应该返回数据的大小
	long request_cursize;		//请求实际已经接收数据的大小

	char *curbuf;				//当前可用的数据buf比如去掉协议头的buf，
	long curbuf_size;			//当前buf中实际数据的大小

	/////////////////////////////////////
	void clear();
	void Reset();
	void FreeBuf();

	void ClearParam(){ m_param.Clear(); };
	void AddParam(const string &strName, const string &strValue);
	void AddParam(const char *szName,const char *szValue);

	string GetSign(string &secret_key);
	string GetParamSet();

	void ClearHeader(){ m_header.Clear(); };
	void AddHeader(const char *szName,const char *szValue);
	
	string AddParamToUrl(const string &strUrl);
	string GetUrl();
	void Requset();

	void GetUTF8Result();
};


#endif /* __URL_H__ */


