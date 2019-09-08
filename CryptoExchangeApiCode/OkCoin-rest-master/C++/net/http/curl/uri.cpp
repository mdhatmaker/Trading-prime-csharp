#include "string.h"
#include "uri.h"
#include <sstream>

CUriParameter::CUriParameter()
{
}

CUriParameter::~CUriParameter()
{
}

void CUriParameter::Clear()
{
	m_items.clear();
}

void CUriParameter::Add(const char *name,const char *value)
{
	//m_items[name] = str::URLEncode(value);
	m_items[name] = value;
}
void CUriParameter::AddNoEncode(const char *name,const char *value)
{
	m_items[name] = value;
}
void CUriParameter::Set(const char *name,const char *value)
{
	map<string, string >::iterator it;
	for (it = m_items.begin(); it != m_items.end(); it++) {
		if ((*it).first == name) {
			(*it).second = str::URLEncode(value);
			return;
		}
	}
	Add(name, value);
}

std::string CUriParameter::ToString()
{
	std::string paramStr;

	map<string, string>::const_iterator it = m_items.begin();
	if (it != m_items.end()) {
		for (;;) {
			paramStr += (*it).first;
			paramStr += "=";
			paramStr += (*it).second;

			it++;
			if (it != m_items.end()) {
				paramStr += "&";
			} else {
				break;
			}
		}
	}
	return paramStr;
}

string CUriParameter::Get(const string name) const
{
	map<string, string >::const_iterator it;
	for (it = m_items.begin(); it != m_items.end(); it++) {
		if ((*it).first == name) {
			return (*it).second;
		}
	}
	return "";
}

list<string> CUriParameter::GetArray(const string name) const
{
	list<string> values;
	
	map<string, string >::const_iterator it;
	for (it = m_items.begin(); it != m_items.end(); it++)
	{
		if ((*it).first == name) {
			values.push_back((*it).second);
		}
	}

	return values;
}

Uri::Uri():
	errcode(CURLE_OK),
	pcurl(0),
	request_size(0),
	request_cursize(0),

	curbuf(0),				//当前buf
	//curbuf_allocsize(0),	//当前buf分配空间大小
	curbuf_size(0),			//当前buf中实际数据的大小

	isinit(false)
{
	saveto = CURL_REQUEST_SAVE_TO_MEMROY;
	type = 0;
	protoclstr = "";
	domain = "";
	protocol = HTTP_PROTOCOL_HTTP;
	method = METHOD_GET;
}
Uri::~Uri()
{
	FreeBuf();
}
void Uri::clear()
{
	Reset();
	FreeBuf();
};

void Uri::Reset()
{
	ClearParam();
	ClearParam();

	result.clear();
	errcode = CURLE_OK;

	request_size = 0;
	request_cursize = 0;

	if (curbuf != 0)
	{
		free(curbuf);
		curbuf = 0;	
		curbuf_size = 0;
	}
};

void Uri::FreeBuf()
{
	if (curbuf != 0)
	{
		free(curbuf);
		curbuf = 0;
		curbuf_size = 0;
	}

	if (pcurl != 0)
	{
		delete pcurl;
		pcurl = 0;
	}
}


void Uri::AddParam(const string &strName, const string &strValue)
{
	if(!strValue.empty())
	{
		m_param.Add(strName.c_str(), strValue.c_str());
	}
};

void Uri::AddParam(const char *szName, const char *szValue)
{
	m_param.Add(szName, szValue);
};


string Uri::GetSign(string &secret_key)
{
	string params = GetParamSet();
	params +="&secret_key=";
	params +=secret_key;
	string sign = str::GetMD5(params);
	transform(sign.begin(), sign.end(), sign.begin(), ::toupper);  
	return sign;
}


string Uri::GetParamSet()
{
	return m_param.ToString();
}

void Uri::AddHeader(const char *szName, const char *szValue)
{
	m_header.AddNoEncode(szName, szValue);
};


string Uri::AddParamToUrl(const string &strUrl)
{
	string url = strUrl;
	string paramStr = m_param.ToString();

	if (paramStr.length() > 0) {
		if(strUrl.find("?") > -1) {
			url += "&" + paramStr;
		} else {
			url += "?" + paramStr;
		}
	}

	return url;
}



string Uri::GetUrl()
{	
	string urlstr;
	urlstr = protoclstr;
	urlstr += domain;
	urlstr += api;
	url = urlstr;
	return urlstr;
}

void Uri::Requset()
{
	if(isinit == true)
	{
		CUrl curl;
		curl.requset(this);
		pcurl = 0;
	}
	return ;
};


void Uri::GetUTF8Result()
{
	if (curbuf != 0)
	{
		result = str::UTF8ToGB(curbuf);
		if (curbuf)
		{
			free(curbuf);
			curbuf = NULL;
		}
	}
}