using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FutureLibrary.Rest;

namespace Net45ConsoleApp
{
    public class Startup
    {
        public Startup()
        {
            string key = "";
            string secret_key = "";
            var BAFrest = new BinanceFuture(key, secret_key);
            var _bacst = BAFrest.GetServerTime();
            var _bacping = BAFrest.Ping();
            var _bacpairsinfo = BAFrest.GetPairsInfo();
            var _bacdepth = BAFrest.GetDepth("BTCUSD_200925");
            var _bacrecenttrades = BAFrest.GetRecentMarketTrades("BTCUSD_200925");
            //need key
            var _bacrawmkthistrades = BAFrest.GetRawMarketHistoricalTrades("BTCUSD_200925");
            //need key
            var _bacaggmkthistrades = BAFrest.GetAggeratedMarketHistoricalTrades("BTCUSD_200925");
            var _backline = BAFrest.GetKLine("BTCUSD_200925", "30m");
            var _bacmarkandfundrate = BAFrest.GetMarkPriceAndPremium("BTCUSD_200925");
            //following 3 funcs symbol is optional
            var _bac24hrssummary = BAFrest.GetSummary("BTCUSD_200925");
            var _baclast = BAFrest.GetLastPrice("BTCUSD_200925");
            var _bactopbook = BAFrest.GetTopBook("BTCUSD_200925");
            //interval: 1m,3m,5m,15m,30m,1h,2h,4h,6h,8h,12h,1d,3d,1w,1M
            //    contractType: CURRENT_QUARTER 当季合约, NEXT_QUARTER 次季合约
            var _continuouskline = BAFrest.GetContinuousKline("BTCUSD", "30m", "CURRENT_QUARTER");
            var _indexkline = BAFrest.GetIndexPriceKline("BTCUSD", "30m");
            var _markkline = BAFrest.GetMarkPriceKline("BTCUSD_200925", "30m");
            var _forcedorder = BAFrest.GetAllForceOrders();
            var _openinterest = BAFrest.GetOpenInterest("BTCUSD_200925");

            //account
            var _balance = BAFrest.GetAccountBalance();
            var _accountinfo = BAFrest.GetAccountInfo();
            var _openorders = BAFrest.GetPendingOrders("BTCUSD_200925");

            var _positionmode = BAFrest.GetPositionMode();
            var _updatepositionresult = BAFrest.UpdatePositionMode("false");
            var _margintype = BAFrest.UpdateMarginType("BTCUSD_200925", "CROSSED");
            var _testorder = BAFrest.TestOrder("BTCUSD_200925", "BUY", "LIMIT", "1", "9500", "GTC");
            //需要充币实盘测试
            var _placeorder = BAFrest.PlaceOrder("BTCUSD_200925", "BUY", "LIMIT", "1", "9500", "GTC");
            //需要挂单实盘测试orderId 与 origClientOrderId 必须至少发送一个
            var _cancelOrder = BAFrest.CancelOrder("BTCUSD_200925", "");

            var _cancelallorder = BAFrest.CancelAll("BTCUSD_200925");

        }
    }
}
