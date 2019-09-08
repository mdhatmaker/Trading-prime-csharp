## 环境要求
(php>=5.3.3)
(workerman框架 demo中带的仅供测试)

## 运行  
下载该demo

### 1.订阅  
```php
<?php
require_once __DIR__ . '/subscribe.php';
subscribe(function($data) {
	var_dump($data);
});
```
在终端运行 `php youFile.php`


### 2. 请求
```php
<?php
require_once __DIR__ . '/request.php';
request(function($data) {
	var_dump($data);
});
```
在终端运行 `php youFile.php`

## 注意
生产环境下推荐利用systemctl等工具进行部署，

## 反馈
#### [china.hehanlin@gmail.com](https://github.com/hehanlin)

