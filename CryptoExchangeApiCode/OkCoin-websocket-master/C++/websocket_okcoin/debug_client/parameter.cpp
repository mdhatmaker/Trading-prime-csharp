#include "string.h"
#include "parameter.h"
#include <sstream>

Parameter::Parameter()
{
}

Parameter::~Parameter()
{
}

void Parameter::Clear()
{
	m_items.clear();
}

void Parameter::Add(const char *name,const char *value)
{
	//m_items[name] = str::URLEncode(value);
	m_items[name] = value;
}
void Parameter::AddNoEncode(const char *name,const char *value)
{
	m_items[name] = value;
}



std::string Parameter::ToString()
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

std::string Parameter::ToJsonString()
{
	std::string paramStr;

	map<string, string>::const_iterator it = m_items.begin();
	if (it != m_items.end()) 
	{

		for (;;) {
			paramStr += "'";
			paramStr += (*it).first;
			paramStr += "'";
			paramStr += ":";
			paramStr += "'";
			paramStr += (*it).second;
			paramStr += "'";
			it++;
			if (it != m_items.end()) {
				paramStr += ",";
			} else {
				break;
			}
		}
		
	}
	return paramStr;
}
string Parameter::Get(const string name) const
{
	map<string, string >::const_iterator it;
	for (it = m_items.begin(); it != m_items.end(); it++) {
		if ((*it).first == name) {
			return (*it).second;
		}
	}
	return "";
}

list<string> Parameter::GetArray(const string name) const
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



void Parameter::AddParam(const string &strName, const string &strValue)
{
	if(!strValue.empty())
	{
		Add(strName.c_str(), strValue.c_str());
	}
};

void Parameter::AddParam(const char *szName, const char *szValue)
{
	Add(szName, szValue);
};


string Parameter::GetSign(string &secret_key)
{
	string params = ToString();
	params +="&secret_key=";
	params +=secret_key;
	string sign = str::GetMD5(params);
	transform(sign.begin(), sign.end(), sign.begin(), ::toupper);  
	return sign;
}

/*
string Parameter::GetSign()
{
	string params = ToString();
	//params +="&secret_key=";
	//params +=secret_key;
	string sign = str::GetMD5(params);
	transform(sign.begin(), sign.end(), sign.begin(), ::toupper);  
	return sign;
}
*/





