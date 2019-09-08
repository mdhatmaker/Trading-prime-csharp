#ifndef __PARAMETER_H__
#define __PARAMETER_H__

#include <string>
#include <map>
#include <vector>
#include <list>
#include <algorithm>


using namespace std;


class Parameter
{
public:
	Parameter();
	~Parameter();
	map<string, string> m_items;

public:
	string Get(const string name) const;
	list<string> GetArray(const string name) const;
	string ToString();
	string ToJsonString();


	void Clear();
	void Add(const char *name,const char *value);

	void AddNoEncode(const char *name,const char *value);

	void Set(const char *name,const char *value);

	void ClearParam(){ Clear(); };
	void AddParam(const string &strName, const string &strValue);
	void AddParam(const char *szName,const char *szValue);

	string GetSign(string &secret_key);
	//string GetSign();
};



#endif /* __PARAMETER_H__ */


