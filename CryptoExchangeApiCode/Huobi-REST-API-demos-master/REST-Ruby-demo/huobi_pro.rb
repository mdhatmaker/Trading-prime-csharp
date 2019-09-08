require 'httparty'
require 'json'
require 'open-uri'
require 'rack'
require 'digest/md5'
require 'base64'
class HuobiPro

  def initialize(access_key,secret_key,account_id,signature_version="2")
      @access_key = access_key
      @secret_key = secret_key
      @signature_version = signature_version
      @account_id = account_id
      @uri = URI.parse "https://api.huobi.pro/"
      @header = {
        'Content-Type'=> 'application/json',
        'Accept' => 'application/json',
        'Accept-Language' => 'zh-CN',
        'User-Agent'=> 'Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/39.0.2171.71 Safari/537.36'
      }
  end

  ## 获取交易对
  def symbols
    path = "/v1/common/symbols"
    request_method = "GET"
    params ={}
    util(path,params,request_method)
  end

  ## 获取市场深度
  def depth(symbol,type="step0")
    path = "/market/depth"
    request_method = "GET"
    params ={"symbol" => symbol,"type"=>type}
    util(path,params,request_method)
  end

  ## K线数据
  def history_kline(symbol,period,size=150)
    path = "/market/history/kline"
    request_method = "GET"
    params ={"symbol" => symbol,"period"=>period,"size" => size}
    util(path,params,request_method)
  end

  ## 获取聚合行情(Ticker)
  def merged(symbol)
    path = "/market/detail/merged"
    request_method = "GET"
    params ={"symbol" => symbol}
    util(path,params,request_method)
  end

  ## 获取 Market Depth 数据
  def market_trade(symbol)
    path = "/market/depth"
    request_method = "GET"
    params ={"symbol" => symbol}
    util(path,params,request_method)
  end

  ## 获取 Trade Detail 数据
  def trade_detail(symbol)
    path = "/market/trade"
    request_method = "GET"
    params ={"symbol" => symbol}
    util(path,params,request_method)
  end

  ## 批量获取最近的交易记录
  def history_trade(symbol,size=1)
    path = "/market/history/trade"
    request_method = "GET"
    params ={"symbol" => symbol,"size" => size}
    util(path,params,request_method)
  end

  ## 获取 Market Detail 24小时成交量数据
  def market_detail(symbol)
    path = "/market/detail"
    request_method = "GET"
    params ={"symbol" => symbol}
    util(path,params,request_method)
  end

  ## 查询系统支持的所有币种
  def currencys
    path = "/v1/common/currencys"
    request_method = "GET"
    params ={}
    util(path,params,request_method)
  end

  ## 查询当前用户的所有账户(即account-id)
  def accounts
    path = "/v1/account/accounts"
    request_method = "GET"
    params ={}
    json = util(path,params,request_method)
  end

  ## 获取账户资产状况
  def balances
    path = "/v1/account/accounts/#{@account_id}/balance"
    request_method = "GET"
    balances = {"account_id"=>@account_id}
    util(path,{},request_method)
  end

  ## 创建并执行一个新订单
  ## 如果使用借贷资产交易
  ## 请在下单接口/v1/order/orders/place
  ## 请求参数source中填写'margin-api'
  def new_order(symbol,side,price,count)
    params ={
      "account-id" => @account_id,
      "amount" => count,
      "price" => price,
      "source" => "api",
      "symbol" => symbol,
      "type" => "#{side}-limit"
    }
    path = "/v1/order/orders/place"
    request_method = "POST"
    util(path,params,request_method)
  end

  ## 申请提现虚拟币
  def withdraw_virtual_create(address,amount,currency)
    path = "/v1/dw/withdraw/api/create"
    params ={
      "address" =>address,
      "amount" => amount,
      "currency" => currency
    }
    request_method = "POST"
    util(path,params,request_method)
  end

  ## 申请取消提现虚拟币
  def withdraw_virtual_cancel(withdraw_id)
    path = "/v1/dw/withdraw-virtual/#{withdraw_id}/cancel"
    params ={"withdraw_id" => withdraw_id}
    request_method = "POST"
    util(path,params,request_method)
  end

  ## 查询某个订单详情
  def order_status(order_id,market)
    path = "/v1/order/orders/#{order_id}"
    request_method = "GET"
    params ={"order-id" => order_id}
    util(path,params,request_method)
  end

  ## 申请撤销一个订单请求
  def submitcancel(order_id)
    path = "/v1/order/orders/#{order_id}/submitcancel"
    request_method = "POST"
    params ={"order-id" => order_id}
    util(path,params,request_method)
  end
  ## 批量撤销订单
  def batchcancel(order_ids)
    path = "/v1/order/orders/batchcancel"
    request_method = "POST"
    params ={"order-ids" => order_ids}
    util(path,params,request_method)
  end

  ## 查询某个订单的成交明细
  def matchresults(order_id)
    path = "/v1/order/orders/#{order_id}/matchresults"
    request_method = "GET"
    params ={"order-id" => order_id}
    util(path,params,request_method)
  end

  ## 查询当前委托、历史委托
  def open_orders(symbol,side)
    params ={
      "symbol" => symbol,
      "types" => "#{side}-limit",
      "states" => "pre-submitted,submitted,partial-filled,partial-canceled"
    }
    path = "/v1/order/orders"
    request_method = "GET"
    util(path,params,request_method)
  end

  ## 查询当前成交、历史成交
  def history_matchresults(symbol)
    path = "/v1/order/matchresults"
    params ={"symbol"=>symbol}
    request_method = "GET"
    util(path,params,request_method)
  end

  ## 现货账户划入至借贷账户
  def transfer_in_margin(symbol,currency,amount)
    path = "/v1/dw/transfer-in/margin"
    params ={"symbol"=>symbol,"currency"=>currency,"amount"=>amount}
    request_method = "POST"
    util(path,params,request_method)
  end

  ## 借贷账户划出至现货账户
  def transfer_out_margin(symbol,currency,amount)
    path = "/v1/dw/transfer-out/margin"
    params ={"symbol"=>symbol,"currency"=>currency,"amount"=>amount}
    request_method = "POST"
    util(path,params,request_method)
  end

  ## 借贷订单
  def loan_orders(symbol,currency)
    path = "/v1/margin/loan-orders"
    params ={"symbol"=>symbol,"currency"=>currency}
    request_method = "POST"
    util(path,params,request_method)
  end

  ## 归还借贷
  def repay(order_id,amount)
    path = "/v1/margin/orders/{order-id}/repay"
    params ={"order-id"=>order_id,"amount"=>amount}
    request_method = "GET"
    util(path,params,request_method)
  end
  ## 借贷账户详情
  def margin_accounts_balance(symbol)
    path = "/v1/margin/accounts/balance?symbol={symbol}"
    params ={}
    request_method = "GET"
    util(path,params,request_method)
  end
  ## 申请借贷
  def margin_orders(symbol,currency,amount)
    path = "/v1/margin/orders"
    params ={"symbol"=>symbol,"currency"=>currency,"amount"=>amount}
    request_method = "POST"
    util(path,params,request_method)
  end

  private

  def util(path,params,request_method)
    h =  {
      "AccessKeyId"=>@access_key,
      "SignatureMethod"=>"HmacSHA256",
      "SignatureVersion"=>@signature_version,
      "Timestamp"=> Time.now.getutc.strftime("%Y-%m-%dT%H:%M:%S")
    }
    h = h.merge(params) if request_method == "GET"
    data = "#{request_method}\napi.huobi.pro\n#{path}\n#{Rack::Utils.build_query(hash_sort(h))}"
    h["Signature"] = sign(data)
    url = "https://api.huobi.pro#{path}?#{Rack::Utils.build_query(h)}"
    http = Net::HTTP.new(@uri.host, @uri.port)
    http.use_ssl = true
    begin
      JSON.parse http.send_request(request_method, url, JSON.dump(params),@header).body
    rescue Exception => e
      {"message" => 'error' ,"request_error" => e.message}
    end
  end

  def sign(data)
    Base64.encode64(OpenSSL::HMAC.digest('sha256',@secret_key,data)).gsub("\n","")
  end

  def hash_sort(ha)
    Hash[ha.sort_by{|key, val|key}]
  end

end

access_key = '********************************'
secret_key = '********************************'
account_id = '******'
huobi_pro = HuobiPro.new(access_key,secret_key,account_id)
# p huobi_pro.balances
# p huobi_pro.symbols
# p huobi_pro.depth('ethbtc')
# p huobi_pro.history_kline('ethbtc',"1min")
# p huobi_pro.merged('ethbtc')
# p huobi_pro.trade_detail('ethbtc')
# p huobi_pro.history_trade('ethbtc')