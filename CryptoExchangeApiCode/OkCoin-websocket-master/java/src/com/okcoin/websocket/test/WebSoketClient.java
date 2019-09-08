package com.okcoin.websocket.test;


import com.okcoin.websocket.WebSocketBase;
import com.okcoin.websocket.WebSocketService;
/**
 * 通过继承WebSocketBase创建WebSocket客户端
 * @author okcoin
 *
 */
public class WebSoketClient extends WebSocketBase{
	public WebSoketClient(String url,WebSocketService service){
		super(url,service);
	}
}
