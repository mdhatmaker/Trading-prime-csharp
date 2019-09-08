C# WebSocket
免责声明：
此WebSocket基于第三方WebSocketSharp，OKCoin技术对其进行简单封装，如使用过程中由于第三方插件不稳定导致的问题，OKCoin不负任何责任。希望用户在使用前认真测试。

使用说明：
1、WebSocketBase 是对WebSocketSharp的封装，其中重要一点实现断网重连的功能。
2、在断网重连情况下，由于OKCoin WebSocket Server 根据Session 记录用户订阅信息。所以在断开重连时需要用户重新订阅所需信息！！！
   我们提供了判断是否发生重连事件wb.isReconnect()，用户可以根据此函数返回值进行重新订阅。
3、用户需要实现WebSocketService中的onReceive 方法，WebSocket 返回数据由此函数接收。
4、具体样例参考Example。





