//#define testnet

using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using FutureLibrary.Model;
using System.Security.Cryptography;
using System.IO;

namespace FutureLibrary.Rest
{
    public class BinanceFRest
    {
        public BinanceFRest()
        {

        }

        public BinanceFRest(string key, string secrect)
        {
            this.BA_KEY = key;
            this.BA_SECRECT = secrect;
        }

        private string BA_KEY;
        private string BA_SECRECT;
#if (!testnet)
        const string BASE_URL = "https://dapi.binance.com";
#else
        const string BASE_URL = "https://testnet.binancefuture.com";
#endif

        #region
        private string httpget(string req)
        {
            var wc = new WebClient();
            string request = req;
            string result = wc.DownloadString(req);
            wc.Dispose();
            return result;
        }

        private static string AddOptionalParas(string resource, string key, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return resource;
            }
            else
            {
                if (resource.Contains("?"))
                {
                    resource = resource + "&" + key + "=" + value;
                }
                else
                {
                    resource = resource + "?" + key + "=" + value;
                }
                return resource;
            }
        }

        private static string BodyAddOptionalParas(string resource, string key, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return resource;
            }
            else
            {
                resource = resource + "&" + key + "=" + value;
                return resource;
            }
        }
        private string HmacSha256(string message, string secret)
        {
            var encoding = new ASCIIEncoding();
            var msgBytes = encoding.GetBytes(message);
            var secretBytes = encoding.GetBytes(secret);
            var hmac = new HMACSHA256(secretBytes);

            var hash = hmac.ComputeHash(msgBytes);

            return BitConverter.ToString(hash).Replace("-", "");
        }
        #endregion
        //public and market api
        #region
        public string GetServerTime()
        {
            string req = BASE_URL + "/dapi/v1/time";
            var res = httpget(req);
            return res;
        }


        public string Ping()
        {
            string req = BASE_URL + "/dapi/v1/ping";
            var res = httpget(req);
            return res;
        }

        public string GetPairsInfo()
        {
            string req = BASE_URL + "/dapi/v1/exchangeInfo";
            var res = httpget(req);
            return res;
        }

        public string GetDepth(string symbol, string limit = "")
        {
            string req = BASE_URL + "/dapi/v1/depth?symbol=" + symbol;
            req = AddOptionalParas(req, "limit", limit);
            var res = httpget(req);
            return res;
        }

        public string GetRecentMarketTrades(string symbol, string limit = "")
        {
            string req = BASE_URL + "/dapi/v1/trades?symbol=" + symbol;
            req = AddOptionalParas(req, "limit", limit);
            var res = httpget(req);
            return res;
        }

        public string GetRawMarketHistoricalTrades(string symbol, string limit = "", string fromId = "")
        {
            string req = BASE_URL + "/dapi/v1/historicalTrades?symbol=" + symbol;
            req = AddOptionalParas(req, "limit", limit);
            req = AddOptionalParas(req, "fromId", fromId);
            var wc = new WebClient();
            wc.Headers.Add("X-MBX-APIKEY", BA_KEY);
            string result = wc.DownloadString(req);
            return result;
        }

        public string GetAggeratedMarketHistoricalTrades(string symbol, string limit = "", string fromId = "")
        {
            string req = BASE_URL + "/dapi/v1/aggTrades?symbol=" + symbol;
            req = AddOptionalParas(req, "limit", limit);
            req = AddOptionalParas(req, "fromId", fromId);
            var res = httpget(req);
            return res;
        }

        //interval:1m,3m,5m,15m,30m,1h,2h,4h,6h,8h,12h,1d,3d,1w,1M
        public string GetKLine(string symbol, string interval, string startTime = "", string endTime = "")
        {
            string req = BASE_URL + "/dapi/v1/klines?symbol=" + symbol + "&interval=" + interval;
            req = AddOptionalParas(req, "startTime", startTime);
            req = AddOptionalParas(req, "endTime", endTime);
            var res = httpget(req);
            return res;
        }

        public string GetMarkPriceAndPremium(string symbol="",string pair="")
        {
            string req = BASE_URL + "/dapi/v1/premiumIndex";
            req = AddOptionalParas(req, "symbol", symbol);
            req = AddOptionalParas(req, "pair", pair);
            var res = httpget(req);
            return res;
        }


        public string GetSummary(string symbol = "")
        {
            string req = BASE_URL + "/dapi/v1/ticker/24hr";
            req = AddOptionalParas(req, "symbol", symbol);
            var res = httpget(req);
            return res;
        }

        public string GetLastPrice(string symbol = "")
        {
            string req = BASE_URL + "/dapi/v1/ticker/price";
            req = AddOptionalParas(req, "symbol", symbol);
            var res = httpget(req);
            return res;
        }

        public string GetTopBook(string symbol = "")
        {
            string req = BASE_URL + "/dapi/v1/ticker/bookTicker";
            req = AddOptionalParas(req, "symbol", symbol);
            var res = httpget(req);
            return res;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pair"></param>
        /// <param name="interval">interval:1m,3m,5m,15m,30m,1h,2h,4h,6h,8h,12h,1d,3d,1w,1M</param>
        /// <param name="contractType">contractType:CURRENT_QUARTER 当季合约,NEXT_QUARTER 次季合约</param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public string GetContinuousKline(string pair, string interval, string contractType, string startTime = "", string endTime = "", string limit="")
        {
            string req = BASE_URL + "/dapi/v1/continuousKlines?pair=" + pair + "&contractType=" + contractType + "&interval=" +interval;
            req = AddOptionalParas(req, "startTime", startTime);
            req = AddOptionalParas(req, "endTime", endTime);
            req = AddOptionalParas(req, "limit", limit);
            var res = httpget(req);
            return res;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pair"></param>
        /// <param name="interval">interval:1m,3m,5m,15m,30m,1h,2h,4h,6h,8h,12h,1d,3d,1w,1M</param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public string GetIndexPriceKline(string pair, string interval, string startTime = "", string endTime = "", string limit = "")
        {
            string req = BASE_URL + "/dapi/v1/indexPriceKlines?pair=" + pair + "&interval=" + interval;
            req = AddOptionalParas(req, "startTime", startTime);
            req = AddOptionalParas(req, "endTime", endTime);
            req = AddOptionalParas(req, "limit", limit);
            var res = httpget(req);
            return res;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="interval">interval:1m,3m,5m,15m,30m,1h,2h,4h,6h,8h,12h,1d,3d,1w,1M</param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public string GetMarkPriceKline(string symbol, string interval, string startTime = "", string endTime = "", string limit = "")
        {
            string req = BASE_URL + "/dapi/v1/markPriceKlines?symbol=" + symbol + "&interval=" + interval;
            req = AddOptionalParas(req, "startTime", startTime);
            req = AddOptionalParas(req, "endTime", endTime);
            req = AddOptionalParas(req, "limit", limit);
            var res = httpget(req);
            return res;
        }

        public string GetAllForceOrders(string symbol="", string pair="", string startTime = "", string endTime = "", string limit = "")
        {
            string req = BASE_URL + "/dapi/v1/allForceOrders";
            req = AddOptionalParas(req, "symbol", symbol);
            req = AddOptionalParas(req, "pair", pair);
            req = AddOptionalParas(req, "startTime", startTime);
            req = AddOptionalParas(req, "endTime", endTime);
            req = AddOptionalParas(req, "limit", limit);
            var res = httpget(req);
            return res;
        }

        public string GetOpenInterest(string symbol)
        {
            string req = BASE_URL + "/dapi/v1/openInterest?symbol="+symbol;
            var res = httpget(req);
            return res;
        }
        #endregion
        //account
        #region
        public string GetTransferHistory(string asset, string startTime, string endTime = "", string current = "", string size = "", string recvWindow = "")
        {
            string req = "https://api.binance.com/sapi/v1/futures/transfer";
            long dt = Convert.ToInt64(TimeConverter.kc_dtToUnixms(DateTime.Now.ToUniversalTime()));
            string body = "asset=" + asset + "&timestamp=" + dt;
            body = BodyAddOptionalParas(body, "startTime", startTime);
            body = BodyAddOptionalParas(body, "endTime", endTime);
            body = BodyAddOptionalParas(body, "current", current);
            body = BodyAddOptionalParas(body, "size", size);
            body = BodyAddOptionalParas(body, "recvWindow", recvWindow);
            var sig = HmacSha256(body, this.BA_SECRECT);
            body += "&signature=" + sig;
            var wc = new WebClient();
            string request = req + "?" + body;
            wc.Headers.Add("X-MBX-APIKEY", this.BA_KEY);
            string result = wc.DownloadString(request);
            wc.Dispose();
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="asset"></param>
        /// <param name="amount"></param>
        /// <param name="type">type:1: 现货账户向合约账户划转 2: 合约账户向现货账户划转</param>
        /// <param name="recvWindow"></param>
        /// <returns></returns>
        public string Transfer(string asset, string amount, string type, string recvWindow = "")
        {
            string req = "https://api.binance.com/sapi/v1/futures/transfer";
            long dt = Convert.ToInt64(TimeConverter.kc_dtToUnixms(DateTime.Now.ToUniversalTime()));
            string body = $"asset={asset}&timestamp={dt.ToString()}&amount={amount}&type={type}";
            body = BodyAddOptionalParas(body, "recvWindow", recvWindow);
            var sig = HmacSha256(body, this.BA_SECRECT);
            body += "&signature=" + sig;
            string res;
            using (WebClient wc = new WebClient())
            {
                wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                wc.Headers.Add("X-MBX-APIKEY", this.BA_KEY);
                res = wc.UploadString(req, body);
            }
            return res;
        }

        public string GetAccountInfo()
        {
            string req = BASE_URL + "/dapi/v1/account";
            long dt = Convert.ToInt64(TimeConverter.kc_dtToUnixms(DateTime.Now.ToUniversalTime()));
            string body = "timestamp=" + dt;
            var sig = HmacSha256(body, this.BA_SECRECT);
            body += "&signature=" + sig;
            var wc = new WebClient();
            string request = req + "?" + body;
            wc.Headers.Add("X-MBX-APIKEY", this.BA_KEY);
            string result = wc.DownloadString(request);
            wc.Dispose();
            return result;
        }

        public string GetPendingOrders(string symbol,string orderId="")
        {
            string req = BASE_URL + "/dapi/v1/openOrders";
            long dt = Convert.ToInt64(TimeConverter.kc_dtToUnixms(DateTime.Now.ToUniversalTime()));
            string body = "symbol=" + symbol + "&timestamp=" + dt;
            if(String.IsNullOrEmpty(orderId))
            {
                body = "symbol=" + symbol + "&orderId" + orderId + "&timestamp=" + dt;
            }
            var sig = HmacSha256(body, this.BA_SECRECT);
            body += "&signature=" + sig;
            var wc = new WebClient();
            string request = req + "?" + body;
            wc.Headers.Add("X-MBX-APIKEY", this.BA_KEY);
            string result = wc.DownloadString(request);
            wc.Dispose();
            return result;
        }

        public string GetAccountBalance()
        {
            string req = BASE_URL + "/dapi/v1/balance";
            long dt = Convert.ToInt64(TimeConverter.kc_dtToUnixms(DateTime.Now.ToUniversalTime()));
            string body = "timestamp=" + dt;
            var sig = HmacSha256(body, this.BA_SECRECT);
            body += "&signature=" + sig;
            var wc = new WebClient();
            string request = req + "?" + body;
            wc.Headers.Add("X-MBX-APIKEY", this.BA_KEY);
            string result = wc.DownloadString(request);
            wc.Dispose();
            return result;
        }

        public string GetPositionMode()
        {
            string req = BASE_URL + "/dapi/v1/positionSide/dual";
            long dt = Convert.ToInt64(TimeConverter.kc_dtToUnixms(DateTime.Now.ToUniversalTime()));
            string body = "timestamp=" + dt;
            var sig = HmacSha256(body, this.BA_SECRECT);
            body += "&signature=" + sig;
            var wc = new WebClient();
            string request = req + "?" + body;
            wc.Headers.Add("X-MBX-APIKEY", this.BA_KEY);
            string result = wc.DownloadString(request);
            wc.Dispose();
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dualSidePosition">"true": 双向持仓模式；"false": 单向持仓模式</param>
        /// <returns></returns>
        public string UpdatePositionMode(string dualSidePosition, string recvWindow="")
        {
            string req = BASE_URL + "/dapi/v1/positionSide/dual";
            long dt = Convert.ToInt64(TimeConverter.kc_dtToUnixms(DateTime.Now.ToUniversalTime()));
            string body = $"dualSidePosition={dualSidePosition}&timestamp={dt.ToString()}";
            body = BodyAddOptionalParas(body, "recvWindow", recvWindow);
            var sig = HmacSha256(body, this.BA_SECRECT);
            body += "&signature=" + sig;
            string res;
            using (WebClient wc = new WebClient())
            {
                wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                wc.Headers.Add("X-MBX-APIKEY", this.BA_KEY);
                res = wc.UploadString(req, body);
            }
            return res;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="marginType">保证金模式 ISOLATED(逐仓), CROSSED(全仓)</param>
        /// <returns></returns>
        public string UpdateMarginType(string symbol, string marginType)
        {
            string req = BASE_URL + "/dapi/v1/marginType";
            long dt = Convert.ToInt64(TimeConverter.kc_dtToUnixms(DateTime.Now.ToUniversalTime()));
            string body = $"marginType={marginType}&symbol={symbol}&timestamp={dt.ToString()}";

            //string body = "symbol=BTCUSDT&side=BUY&type=LIMIT&timeInForce=GTC&quantity=1&price=8500.0&recvWindow=5000&timestamp=" + dt.ToString();

            var sig = HmacSha256(body, this.BA_SECRECT);
            body += "&signature=" + sig;
            string res;
            using (WebClient wc = new WebClient())
            {
                wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                wc.Headers.Add("X-MBX-APIKEY", this.BA_KEY);
                //req = req + "?" + body;
                res = wc.UploadString(req, body);
            }
            return res;
        }

        //side:BUY or SELL
        //type:LIMIT
        public string TestOrder(string symbol, string side, string type, string quantity, string price = "", string timeInForce = "")
        {
            string req = BASE_URL + "/dapi/v1/order/test";
            long dt = Convert.ToInt64(TimeConverter.kc_dtToUnixms(DateTime.Now.ToUniversalTime()));
            string body = $"symbol={symbol}&side={side}&type={type}&quantity={quantity}&timestamp={dt.ToString()}";
            body = BodyAddOptionalParas(body, "price", price);
            body = BodyAddOptionalParas(body, "timeInForce", timeInForce);

            //string body = "symbol=BTCUSDT&side=BUY&type=LIMIT&timeInForce=GTC&quantity=1&price=8500.0&recvWindow=5000&timestamp=" + dt.ToString();

            var sig = HmacSha256(body, this.BA_SECRECT);
            body += "&signature=" + sig;
            string res;
            using (WebClient wc = new WebClient())
            {
                wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                wc.Headers.Add("X-MBX-APIKEY", this.BA_KEY);
                //req = req + "?" + body;
                res = wc.UploadString(req, body);
            }
            return res;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="side">买卖方向 SELL, BUY</param>
        /// <param name="type">订单类型 LIMIT, MARKET, STOP, TAKE_PROFIT, STOP_MARKET, TAKE_PROFIT_MARKET, TRAILING_STOP_MARKET</param>
        /// <param name="quantity"></param>
        /// <param name="price"></param>
        /// <param name="timeInForce"></param>
        /// <returns></returns>
        public string PlaceOrder(string symbol, string side, string type, string quantity, string price = "", string timeInForce = "")
        {
            string req = BASE_URL + "/dapi/v1/order";
            long dt = Convert.ToInt64(TimeConverter.kc_dtToUnixms(DateTime.Now.ToUniversalTime()));
            string body = $"symbol={symbol}&side={side}&type={type}&quantity={quantity}&timestamp={dt.ToString()}";
            body = BodyAddOptionalParas(body, "price", price);
            body = BodyAddOptionalParas(body, "timeInForce", timeInForce);

            //string body = "symbol=BTCUSDT&side=BUY&type=LIMIT&timeInForce=GTC&quantity=1&price=8500.0&recvWindow=5000&timestamp=" + dt.ToString();

            var sig = HmacSha256(body, this.BA_SECRECT);
            body += "&signature=" + sig;
            string res;
            using (WebClient wc = new WebClient())
            {
                wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                wc.Headers.Add("X-MBX-APIKEY", this.BA_KEY);
                //req = req + "?" + body;
                res = wc.UploadString(req, body);
            }
            return res;
        }
        /// <summary>
        /// orderId 与 origClientOrderId 必须至少发送一个
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="orderId"></param>
        /// <param name="origClientOrderId"></param>
        /// <param name="recWindow"></param>
        /// <returns></returns>
        public string CancelOrder(string symbol, string orderId = "", string origClientOrderId = "", string recWindow = "")
        {
            string req = BASE_URL + "/dapi/v1/order";
            long dt = Convert.ToInt64(TimeConverter.kc_dtToUnixms(DateTime.Now.ToUniversalTime()));
            string body = $"symbol={symbol}";
            body = BodyAddOptionalParas(body, "orderId", orderId);
            body = BodyAddOptionalParas(body, "origClientOrderId", origClientOrderId);
            body = BodyAddOptionalParas(body, "recWindow", recWindow);
            body += $"&timestamp={dt.ToString()}";
            var sig = HmacSha256(body, this.BA_SECRECT);
            body += "&signature=" + sig;

            byte[] byteArray = Encoding.UTF8.GetBytes(body);

            WebRequest webreq = WebRequest.Create(req);

            webreq.ContentType = "application/x-www-form-urlencoded";
            webreq.Headers.Add("X-MBX-APIKEY", this.BA_KEY);
            webreq.Method = "DELETE";
            webreq.Timeout = 3000;
            webreq.ContentLength = byteArray.Length;
            Stream reqStream = webreq.GetRequestStream();
            reqStream.Write(byteArray, 0, byteArray.Length);

            string resFromServer;
            WebResponse webres = webreq.GetResponse();
            using (Stream dataStream = webres.GetResponseStream())
            {
                StreamReader reader = new StreamReader(dataStream);
                resFromServer = reader.ReadToEnd();
                Console.WriteLine(resFromServer);
            }
            webres.Close();

            return resFromServer;
        }

        public string CancelAll(string symbol)
        {
            string req = BASE_URL + "/dapi/v1/allOpenOrders";
            long dt = Convert.ToInt64(TimeConverter.kc_dtToUnixms(DateTime.Now.ToUniversalTime()));
            string body = $"symbol={symbol}&timestamp={dt.ToString()}";

            var sig = HmacSha256(body, this.BA_SECRECT);
            body += "&signature=" + sig;
            string res;
            using (WebClient wc = new WebClient())
            {
                wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                wc.Headers.Add("X-MBX-APIKEY", this.BA_KEY);
                //req = req + "?" + body;
                res = wc.UploadString(req, "DELETE", body);
            }
            return res;
        }
        #endregion

    }
    //public enum contractType
    //{
    //    CURRENT_QUARTER,
    //    NEXT_QUARTER
    //}
}
