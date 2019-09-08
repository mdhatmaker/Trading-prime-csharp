

#ifndef __WEBSOCKET_H__
#define __WEBSOCKET_H__


#include <websocketpp/config/asio_client.hpp>
#include <websocketpp/config/asio.hpp>

#include <websocketpp/client.hpp>

#include <iostream>


typedef void (*websocketpp_callbak_open)();
typedef void (*websocketpp_callbak_close)();
typedef void (*websocketpp_callbak_message)(const char *message);


//typedef websocketpp::client<websocketpp::config::asio_client> client;
typedef websocketpp::client<websocketpp::config::asio_tls> client;


using websocketpp::lib::placeholders::_1;
using websocketpp::lib::placeholders::_2;
using websocketpp::lib::bind;

// pull out the type of messages sent by our config
typedef websocketpp::config::asio_tls_client::message_type::ptr message_ptr;
typedef websocketpp::lib::shared_ptr<boost::asio::ssl::context> context_ptr;
typedef client::connection_ptr connection_ptr;

#define MAX_RETRY_COUNT		10000

enum CONNECTION_STATE
{
	CONNECTION_STATE_UNKONWN,
	CONNECTION_STATE_CONNECTING,
	CONNECTION_STATE_DISCONNECT,
};

class WebSocket
{
private:
    client m_endpoint;
	websocketpp::connection_hdl m_hdl;
	std::string m_uri;
	CONNECTION_STATE m_con_state;
public:

	websocketpp_callbak_open callbak_open;
	websocketpp_callbak_close  callbak_close;
	websocketpp_callbak_message callbak_message;

	bool m_manual_close;//是否为主动关闭连接，如果不是用户主动关闭，当接到断开联接回调时则自动执行重新连接机制。
    typedef WebSocket type;

    WebSocket() :  
	m_manual_close(false),
	m_con_state(CONNECTION_STATE_UNKONWN),
	callbak_open(0),
	callbak_close(0),
	callbak_message(0)
	{
		
        m_endpoint.set_access_channels(websocketpp::log::alevel::all);
        m_endpoint.set_error_channels(websocketpp::log::elevel::all);

        // Initialize ASIO
        m_endpoint.init_asio();

        //Register our handlers
        //m_endpoint.set_socket_init_handler(bind(&type::on_socket_init,this,::_1));
        m_endpoint.set_tls_init_handler(bind(&type::on_tls_init,this,::_1));
        m_endpoint.set_message_handler(bind(&type::on_message,this,::_1,::_2));
        m_endpoint.set_open_handler(bind(&type::on_open,this,::_1));
        m_endpoint.set_close_handler(bind(&type::on_close,this,::_1));
        m_endpoint.set_fail_handler(bind(&type::on_fail,this,::_1));
		
    }

    ~WebSocket()
	{
	}

    void start()
	{
        websocketpp::lib::error_code ec;
        client::connection_ptr con = m_endpoint.get_connection(m_uri, ec);

        if (ec) {
            m_endpoint.get_alog().write(websocketpp::log::alevel::app,ec.message());
        }

		m_endpoint.set_open_handshake_timeout(3000);
		m_endpoint.set_close_handshake_timeout(3000);


        //con->set_proxy("http://humupdates.uchicago.edu:8443");

        m_endpoint.connect(con);
	
        // Start the ASIO io_service run loop
        m_endpoint.run();
		if(callbak_close != 0)callbak_close();
    }

    void on_socket_init(websocketpp::connection_hdl) 
	{

    }

    context_ptr on_tls_init(websocketpp::connection_hdl)
	{
        context_ptr ctx = websocketpp::lib::make_shared<boost::asio::ssl::context>(boost::asio::ssl::context::tlsv1);

        try {
            ctx->set_options(boost::asio::ssl::context::default_workarounds |
                             boost::asio::ssl::context::no_sslv2 |
                             boost::asio::ssl::context::no_sslv3 |
                             boost::asio::ssl::context::single_dh_use);
        } catch (std::exception& e) {
            std::cout << e.what() << std::endl;
        }
        return ctx;
    }

    void on_fail(websocketpp::connection_hdl hdl)
	{
		
        client::connection_ptr con = m_endpoint.get_con_from_hdl(hdl);
        
        std::cout << "Fail handler" << std::endl;
        std::cout << con->get_state() << std::endl;
        std::cout << con->get_local_close_code() << std::endl;
        std::cout << con->get_local_close_reason() << std::endl;
        std::cout << con->get_remote_close_code() << std::endl;
        std::cout << con->get_remote_close_reason() << std::endl;
        std::cout << con->get_ec() << " - " << con->get_ec().message() << std::endl;
		
    }

    void on_close_handshake_timeout(websocketpp::connection_hdl hdl)
	{
	}
	
    void on_open(websocketpp::connection_hdl hdl) 
	{
		m_hdl = hdl;
		m_manual_close = false;
		if(callbak_open != 0)callbak_open();
    }
    void on_message(websocketpp::connection_hdl hdl, message_ptr msg) 
	{
		//std::cout << "Message: " << msg->get_payload().c_str() << std::endl;
		if(callbak_message != 0)callbak_message(msg->get_payload().c_str());
    }
    void on_close(websocketpp::connection_hdl hdl) 
	{

    }

    void doclose() 
	{
		m_manual_close = true;
		m_endpoint.close(m_hdl,websocketpp::close::status::going_away,"");
    }

	void run(std::string &uri)
	{
		try {
			m_uri = uri;
			start();
		} catch (const std::exception & e) {
			std::cout << e.what() << std::endl;
		} catch (websocketpp::lib::error_code e) {
			std::cout << e.message() << std::endl;
		} catch (...) {
			std::cout << "other exception" << std::endl;
		}
	}
	void emit(std::string channel)
	{
		string cmd = "{'event':'addChannel','channel':'";
		cmd += channel;
		cmd += "'}";
		m_endpoint.send(m_hdl, cmd, websocketpp::frame::opcode::text);

		//m_endpoint.send(hdl, "{'event':'addChannel','channel':'ok_btcusd_ticker'}", websocketpp::frame::opcode::text);
       // m_endpoint.send(hdl, "{'event':'addChannel','channel':'ok_btcusd_depth'}", websocketpp::frame::opcode::text);
	}

	void emit(std::string channel,string &parameter)
	{
		string cmd = "{'event':'addChannel','channel':'";
		cmd += channel;
		cmd += "','parameters':{";
		cmd += parameter;
		cmd += "}}";
		m_endpoint.send(m_hdl, cmd, websocketpp::frame::opcode::text);

		//m_endpoint.send(hdl, "{'event':'addChannel','channel':'ok_btcusd_ticker'}", websocketpp::frame::opcode::text);
       // m_endpoint.send(hdl, "{'event':'addChannel','channel':'ok_btcusd_depth'}", websocketpp::frame::opcode::text);
	}

	void remove(std::string channel)
	{
		string cmd = "{'event':'removeChannel','channel':'";
		cmd += channel;
		cmd += "'}";
		m_endpoint.send(m_hdl, cmd, websocketpp::frame::opcode::text);

		//m_endpoint.send(hdl, "{'event':'addChannel','channel':'ok_btcusd_ticker'}", websocketpp::frame::opcode::text);
       // m_endpoint.send(hdl, "{'event':'addChannel','channel':'ok_btcusd_depth'}", websocketpp::frame::opcode::text);
	}

};


#endif /* __WEBSOCKET_H__ */
