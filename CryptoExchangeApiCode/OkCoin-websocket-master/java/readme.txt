WebSocket使用说明：

1.继承WebSocketBase创建客户端类；

  public class Client extends WebSocketBase{
	public Client(String url,WebSocketService service){
		super(url,service);
	}
  }

2.实现WebSocketService接口创建订阅消息处理类：
  public class WebSocketServiceImpl implements WebSocketService{
	
	@Override
	public void onReceive(String msg){
	//TODO something;
	}
  }

3.创建客户端实例并启动(传递url参数和消息处理类实例初始化)：

  //国内站url
  String url = "wss://real.okcoin.cn:10440/websocket/okcoinapi";
  
  //国际站url
  Strint url = "wss://real.okcoin.com:10440/websocket/okcoinapi";
  
  //订阅消息处理类
  WebSocketService service = new WebSocketServiceImpl();
  
  //WebSocket客户端
  WebSoketClient client = new WebSoketClient(url,service);
  client.start();
  
4.调用相关方法获取相应数据：

  1)订阅比特币市场行情
    client.addChannel("ok_sub_spotusd_btc_ticker");

  2)取消订阅比特币市场行情
    client.removeChannel("ok_sub_spotusd_btc_ticker");

  3)期货下单交易
    client.futureTrade(apiKey, secretKey, "btc_usd", "this_week", 2.3, 2, 1, 0, 10);

  4)实时交易数据   apiKey
    client.futureRealtrades(apiKey, secretKey);

  5)取消期货交易
    client.cancelFutureOrder(apiKey, secretKey, "btc_usd", 123456L, "this_week");

  6)现货下单交易
    client.spotTrade(apiKey, secretKey, "btc_usd", 3.2, 2.3, "buy");

  7)现货交易数据
    client.realTrades(apiKey, secretKey);

  8)现货取消订单
    client.cancelOrder(apiKey, secretKey, "btc_usd", 123L);

  9)获取用户信息
    client.getUserInfo(apiKey,secretKey);
