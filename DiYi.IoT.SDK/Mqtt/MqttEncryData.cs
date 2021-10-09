using System;
using System.Collections.Generic;
using System.Text;
using DiYi.IoT.SDK.Util;
using NewLife.Caching;

namespace DiYi.IoT.SDK.Mqtt
{
    public class MqttEncryData
    {
        private readonly FullRedis _redis;
        public MqttEncryData(FullRedis redis)
        {
            _redis = redis;
        }
        public (string ts, string mid, string sign) GetMqttData(string smartBoxSn, int deviceType)
        {
            var timeStamp = DateTime.UtcNow.ToLong().ToString();
            var strMid = smartBoxSn + timeStamp + new Random().Next(1000, 9999);
            var redisKey = $"SmartBoxToken:{smartBoxSn}";
            var authToken = _redis.Get<string>(redisKey);  //柜子token
            var signContent = deviceType + strMid + smartBoxSn + authToken;
            var sign = EncryptUtil.MD5Encrypt16(signContent).ToLower();

            return (timeStamp, strMid, sign);
        }

        public String EncryptData(String data)
        {
            var dataLength = Encoding.Default.GetByteCount(data);
            return "DyIot#" + dataLength.ToString().PadLeft(4, '0') + data;
        }
    }
}
