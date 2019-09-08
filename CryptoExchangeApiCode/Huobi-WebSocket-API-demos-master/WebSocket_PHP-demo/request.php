<?php
 
use Workerman\Worker;
use Workerman\Connection\AsyncTcpConnection;
require_once __DIR__ . '/workerman/Autoloader.php';


/*
*请求数据函数
$sub_str type: string e.g market.btcusdt.kline.1min 具体请查看api
$callback type: function 回调函数，当获得数据时会调用
*/
function request($callback, $req_str="market.btcusdt.kline.1min") {
    $GLOBALS['req_str'] = $req_str;
    $GLOBALS['callback'] = $callback;
    $worker = new Worker();
    $worker->onWorkerStart = function($worker) {
        // ssl需要访问443端口
        $con = new AsyncTcpConnection('ws://api.huobi.pro:443/ws');

        // 设置以ssl加密方式访问，使之成为wss
        $con->transport = 'ssl';

        $con->onConnect = function($con) {
            $data = json_encode([
                'req' => $GLOBALS['req_str'],
                'id' => 'id' . time()
            ]);
            $con->send($data);
        };

        $con->onMessage = function($con, $data) {
            $data = gzdecode($data);
            $data = json_decode($data, true);
            if(isset($data['ping'])) {
                $con->send(json_encode([
                    "pong" => $data['ping']
                ]));
            }else{
                call_user_func_array($GLOBALS['callback'], array($data));           
            }
        };

        $con->connect();
    };

    Worker::runAll();
}
