namespace Network
{
    public class NetErrorConstant
    {
        public const int UNKNOW_PROTOCOL = 2;           //协议错误
        public const int SEND_EXCEPTION = 1000;       //发送异常
        public const int ILLEGAL_PACKAGE = 1001;      //接受到错误数据包
        public const int ZERO_BYTE = 1002;            //收发0字节
        public const int PACKAGE_TIMEOUT = 1003;      //收包超时
        public const int PROXY_TIMEOUT = 1004;        //proxy超时
        public const int FAIL_TO_CONNECT = 1005;      //3次连接不上
        public const int PROXY_ERROR = 1006;          //proxy重启
        public const int ON_DESTROY = 1007;           //结束的时候，关闭网络连接
        public const int ON_KICKOUT = 25;           //被踢了
    }
}