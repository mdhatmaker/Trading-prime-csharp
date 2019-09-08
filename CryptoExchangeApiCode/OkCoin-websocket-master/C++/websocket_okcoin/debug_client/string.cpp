#include <Windows.h>
#include "string.h"
#include <sstream>

wstring str::GetModuleDirectory()
{
	wstring dir;
	wchar_t szBuf[MAX_PATH]={0};
	GetModuleFileNameW(NULL, szBuf, MAX_PATH);

	wstring filename(szBuf);
	int pos=filename.find_last_of(L'\\',filename.length());
	dir=filename.substr(0,pos+1);
	
	return dir;
}


string str::UnicodeToAnsi( const wstring &wstrSource)
{
	return UnicodeToAnsi( wstrSource.c_str());
};
string str::UnicodeToAnsi( const wchar_t* pwzsSource)
{
	if (pwzsSource == NULL)
	{
		return "";
	}
	std::string strTemp="";
	int nLen = WideCharToMultiByte(CP_ACP, 0, pwzsSource, -1, NULL, 0, NULL, NULL);
	if (nLen<= 0) return strTemp;
	char* pszDst = (char * )malloc(nLen+2);
	if (NULL == pszDst) return strTemp;
	memset(pszDst, 0, nLen+2);
	WideCharToMultiByte(CP_ACP, 0, pwzsSource, -1, pszDst, nLen, NULL, NULL);
	pszDst[nLen -1] = 0;
	strTemp=pszDst;
	free(pszDst);
	return strTemp;

};


string str::GetMD5(const char * pzsSource)
{
	return md5::MD5String(pzsSource);		
};

string str::GetMD5(const string &strSource)
{
	return md5::MD5String(strSource.c_str());		
};

BYTE str::toHex(const BYTE &x)
{
	return x > 9 ? x -10 + 'A': x + '0';
};

BYTE str::fromHex(const BYTE &x)
{
	return isdigit(x) ? x-'0' : x-'A'+10;
};

string str::URLEncode(const string &sIn)
{
    string sOut;
    for( size_t ix = 0; ix < sIn.size(); ix++ )
    {      
        BYTE buf[4];
        memset( buf, 0, 4 );
        if( isalnum( (BYTE)sIn[ix] ) )
        {      
            buf[0] = sIn[ix];
        }
        //else if ( isspace( (BYTE)sIn[ix] ) ) //貌似把空格编码成%20或者+都可以
        //{
        //    buf[0] = '+';
        //}
        else
        {
            buf[0] = '%';
            buf[1] = toHex( (BYTE)sIn[ix] >> 4 );
            buf[2] = toHex( (BYTE)sIn[ix] % 16);
        }
        sOut += (char *)buf;
    }
    return sOut;
};

string str::URLDecode(const string &sIn)
{
    string sOut;
    for( int ix = 0; ix < sIn.size(); ix++ )
    {
        BYTE ch = 0;
        if(sIn[ix]=='%')
        {
            ch = (fromHex(sIn[ix+1])<<4);
            ch |= fromHex(sIn[ix+2]);
            ix += 2;
        }
        else if(sIn[ix] == '+')
        {
            ch = ' ';
        }
        else
        {
            ch = sIn[ix];
        }
        sOut += (char)ch;
    }
    return sOut;
};

std::string  str::UTF8ToGB(const char* str)
{

	std::string result;
	WCHAR *strSrc;
	char *szRes;
	if (0 == str)
	{
		return result;
	}
	if (0 == str[0])
	{
		return result;
	}
	//获得临时变量的大小
	int i = MultiByteToWideChar(CP_UTF8, 0, str, -1, NULL, 0);
	strSrc = new WCHAR[i+1];
	MultiByteToWideChar(CP_UTF8, 0, str, -1, strSrc, i);

	//获得临时变量的大小
	i = WideCharToMultiByte(CP_ACP, 0, strSrc, -1, NULL, 0, NULL, NULL);
	szRes = new char[i+1];
	WideCharToMultiByte(CP_ACP, 0, strSrc, -1, szRes, i, NULL, NULL);

	result = szRes;
	delete []strSrc;
	delete []szRes;

	return result;
}


