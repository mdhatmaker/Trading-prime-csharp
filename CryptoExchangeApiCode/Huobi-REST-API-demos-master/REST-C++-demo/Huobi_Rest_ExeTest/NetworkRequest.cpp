#pragma once
//  
//  NetworkRequest.cpp  
//  
//  Created by duzixi.com on 15/8/25.  
//  

#include "NetworkRequest.h"  

using namespace std;
using namespace boost;
using boost::asio::ip::tcp;

// POST请求  
CString PostRequest(CString CShost, CString path, CString form)
{
	char *host = CShost.GetBuffer(CShost.GetLength());
	long length = form.GetLength();
	//form.Replace("\t", "");
	// 声明Asio基础: io_service（任务调度机）  
	boost::asio::io_service io_service;

	// 获取服务器终端列表  
	tcp::resolver resolver(io_service);
	tcp::resolver::query query(host, "http");
	tcp::resolver::iterator iter = resolver.resolve(query);

	// 尝试连接每一个终端，直到成功建立socket连接  
	tcp::socket socket(io_service);
 	try
 	{
		boost::asio::connect(socket, iter);
//		cout << "boost::asio::connect(socket, iter);" << endl;
 	}
 	catch (boost::system::system_error ec)
 	{
		cout << ec.what()<<endl;
		//throw boost::system::system_error(ec);
		//cout << "connect_error:" << ec <<endl;
 		return "connect_error";
 	}

	// 构建网络请求头  
	// 指定 "Connection: close" 在获取应答后断开连接，确保获文件全部数据。  
	boost::asio::streambuf request;
	ostream request_stream(&request);
	request_stream << "POST " << path << " HTTP/1.1\r\n";
	request_stream << "Host: " << host << "\r\n";
	request_stream << "User-Agent: Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/39.0.2171.71 Safari/537.36\r\n";
	//request_stream << "Accept: */*\r\n";
	request_stream << "Content-Type:application/json\r\n";
	request_stream << "Content-Length: " << length << "\r\n";
	request_stream << "Connection: close\r\n\r\n"; // 注意这里是两个空行  
	request_stream << form; //POST 发送的数据本身不包含多于空行  

							// 发送请求  
	boost::asio::write(socket, request);

	//cout << "boost::asio::write(socket, request);" << endl;
	// 读取应答状态. 应答缓冲流 streambuf 会自动增长至完整的行  
	// 该增长可以在构造缓冲流时通过设置最大值限制  
	boost::asio::streambuf response;
	try
	{
		boost::asio::read_until(socket, response, "\r\n");
	}
	catch (...)
	{
			//cout << ec.what() << endl;
			//throw boost::system::system_error(ec);
			//cout << "connect_error:" << ec <<endl;
			return "connect_error";
	}
	//cout << "boost::asio::read_until(socket, response, \"\\r\\n\");" << endl;
	// 检查应答是否OK.  
	istream response_stream(&response);// 应答流  
	string http_version;
	response_stream >> http_version;
	unsigned int status_code;
	response_stream >> status_code;
	string status_message;
	getline(response_stream, status_message);
	if (!response_stream || http_version.substr(0, 5) != "HTTP/")
	{
		printf(" error 无效响应\n");
	}
	if (status_code != 200)
	{
		printf(" error 响应返回 status code %d\n", status_code);
	}
	//cout << "getline(response_stream, status_message);" << endl;

	// 读取应答头部，遇到空行后停止  
	boost::asio::read_until(socket, response, "\r\n\r\n");

	//cout << "boost::asio::read_until" << endl;
	// 显示应答头部  
	string header;
	int len = 0;
	while (getline(response_stream, header) && header != "\r")
	{
		if (header.find("Content-Length: ") == 0) {
			stringstream stream;
			stream << header.substr(16);
			stream >> len;
		}
	}

	long size = response.size();

	if (size > 0) {
		// .... do nothing  
	}

	// 循环读取数据流，直到文件结束  
	boost::system::error_code error;
	while (boost::asio::read(socket, response, boost::asio::transfer_at_least(1), error))
	{
		// 获取应答长度  
		size = response.size();
		if (len != 0) {
			//cout << size << "  Byte  " << (size * 100) / len << "%\n";
		}

	}

	//cout << "boost::asio::read" << endl;
	if (error != boost::asio::error::eof)
	{
		throw boost::system::system_error(error);
	}

	//cout << size << " Byte 内容已下载完毕." << endl;

	// 将streambuf类型转换为CString类型返回  
	istream is(&response);
	is.unsetf(ios_base::skipws);
	string sz;
	sz.append(istream_iterator<char>(is), istream_iterator<char>());
	//cout << "发送结束;" << endl;

	// 返回转换后的字符串  
	return sz.c_str();
}



CString GetRequest(CString sendmsg)
{
	HINTERNET internetopen;
	HINTERNET internetopenurl;
	BOOL internetreadfile;
	DWORD byteread = 0;
	char buffer[1];
	//char ch;
	memset(buffer, 0, 1);
	internetopen = InternetOpen(_T("Testing"), INTERNET_OPEN_TYPE_PRECONFIG, NULL, NULL, 0);
	if (internetopen == NULL) {
		//cout << "InternetOpen初始化失败!" << endl;
		return "Can't InternetOpen!";
	}
	internetopenurl = InternetOpenUrl(internetopen, sendmsg, NULL, 0, INTERNET_FLAG_RELOAD, 0);
	if (internetopenurl == NULL) {
		//cout << "InternetOpenUrl打开Url失败!" << endl;
		InternetCloseHandle(internetopen);
		return "Can't InternetOpenUrl!";
	}
	CString buffs = "";
	while (1) {
		internetreadfile = InternetReadFile(internetopenurl, buffer, sizeof(char), &byteread);
		if (byteread == 0) {
			InternetCloseHandle(internetopenurl);
			break;
		}
		buffs += buffer[0];
	}
	if (internetreadfile == FALSE)
	{
		InternetCloseHandle(internetopenurl);
		return "Can't InternetReadFile!";
	}
	return buffs;
}
