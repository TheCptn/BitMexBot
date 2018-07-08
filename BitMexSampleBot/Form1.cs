using BitMEX;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Web.Script.Serialization;
using System.Net;
using System.Linq;
using System.Text;
using WebSocketSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BitMexSampleBot
{
    public partial class Form1 : Form
    {
        //Variables by Afzal

        #region "Variables by Afzal"
        bool boolUpdateCandles;
        bool boolUpdatingCandles;
        int retryAttempts = 0;
        //string log = "";
        StreamWriter log = new StreamWriter(Application.StartupPath + "\\" + DateTime.Now.ToString("yyyyMMddHHmm") + ".txt");
        JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();

        PlacedOrders pOrder;
        List<PlacedOrders> pOrders = new List<PlacedOrders>();

        WebSocket ws;
        Dictionary<string, decimal> Prices = new Dictionary<string, decimal>();

        DataTable Orders = new DataTable();
        //Till Here

        #endregion

        #region "Previous Variables"

        // IMPORTANT - Enter your API Key information below

        //TEST NET - NEW
        //private static string TestbitmexKey = "YOURHEREKEYHERE";
        //private static string TestbitmexSecret = "YOURSECRETHERE";
        private static string TestbitmexDomain = "https://testnet.bitmex.com";

        //REAL NET
        //private static string bitmexKey = "YOURHEREKEYHERE";
        //private static string bitmexSecret = "YOURSECRETHERE";
        private static string bitmexDomain = "https://www.bitmex.com";


        BitMEXApi bitmex;
        List<OrderBook> CurrentBook = new List<OrderBook>();
        List<Instrument> ActiveInstruments = new List<Instrument>();
        Instrument ActiveInstrument = new Instrument();
        List<Candle> Candles = new List<Candle>();

        bool Running = false;
        string Mode = "Wait";
        List<Position> OpenPositions = new List<Position>();
        List<Order> OpenOrders = new List<Order>();

        // For BBand Indicator Info, 20, close 2
        int BBLength = 20;
        double BBMultiplier = 2;

        // For EMA Indicator Periods, also used in MACD
        int EMA1Period = 1;  // Slow MACD EMA Default = 26
        int EMA2Period = 13;  // Fast MACD EMA Dafault = 12
        int EMA3Period = 9;

        // For MACD
        int MACDEMAPeriod = 9;  // MACD smoothing period

        // For checking API validity before attempting orders/account specific moves
        bool APIValid = false;
        double WalletBalance = 0;

        // For ATR
        int ATR1Period = 7;
        int ATR2Period = 20;

        // For Over Time
        int OTContractsPer = 0;
        int OTIntervalSeconds = 0;
        int OTIntervalCount = 0;
        int OTTimerCount = 0;
        string OTSide = "Buy";

        // For RSI
        int RSIPeriod = 14;

        // For Stochastic (STOCH)
        int STOCHLookbackPeriod = 14;
        int STOCHDPeriod = 3;

        // For MFI
        int MFIPeriod = 14;

        // For WMA
        int WMAPeriod1 = 5; // WMA Period 1 must be /2 of period 2 to use in the HMA
        int WMAPeriod2 = 10; // WMA Period 2 should also be the same as the HMA Period you select if you are using HMA

        // For HMA
        int HMAPeriod = 10;

        // For ALMA
        int ALMAPeriod = 9;
        int ALMASigma = 6;
        double? ALMAOffset = 0.85;

        // NEW - For VWAP
        int VWAPPeriod = 20; // Sets a lookback limit so we don't go too far back

        double CurrentPrice = 0;

        #endregion



        #region "Methods by Afzal"


        private void InitializeWebSocket()
        {
            ws = new WebSocket("wss://www.bitmex.com/realtime");
            ws.OnMessage += (sender, e) =>
            {
                try
                {
                    JObject Message = JObject.Parse(e.Data);
                    if (Message.ContainsKey("table"))
                    {
                        if ((string)Message["action"] == "partial")
                        {
                            MessageBox.Show("Hello");
                        }
                        if ((string)Message["table"] == "tradeBin1m")
                        {
                            if (Message.ContainsKey("data"))
                            {
                                Candles = JsonConvert.DeserializeObject<List<Candle>>(Message["data"].ToString()).OrderByDescending(a => a.TimeStamp).ToList();
                                JArray TD = (JArray)Message["data"];
                                if (TD.Any())
                                {
                                    //decimal Price = (decimal)TD.Children().LastOrDefault()["price"];
                                    //string Symbol = (string)TD.Children().LastOrDefault()["symbol"];
                                    //Prices[Symbol] = Price;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    //MessageBox.Show(ex.Message);
                }
            };

            ws.Connect();

            // Assemble our price dictionary
            foreach (Instrument i in ActiveInstruments)
            {
                ws.Send("{\"op\": \"subscribe\", \"args\": [\"tradeBin1m:" + i.Symbol + "\"]}");
            }

        }



        private void InitializeAuthWebSocket()
        {
            ws = new WebSocket("wss://www.bitmex.com/realtime");
            ws.OnMessage += (sender, e) =>
            {
                try
                {
                    JObject Message = JObject.Parse(e.Data);
                    if (Message.ContainsKey("table"))
                    {
                        if ((string)Message["action"] == "partial")
                        {
                            MessageBox.Show("Hello");
                        }
                        if ((string)Message["table"] == "tradeBin1m")
                        {
                            if (Message.ContainsKey("data"))
                            {
                                Candles = JsonConvert.DeserializeObject<List<Candle>>(Message["data"].ToString()).OrderByDescending(a => a.TimeStamp).ToList();
                                JArray TD = (JArray)Message["data"];
                                if (TD.Any())
                                {
                                    //decimal Price = (decimal)TD.Children().LastOrDefault()["price"];
                                    //string Symbol = (string)TD.Children().LastOrDefault()["symbol"];
                                    //Prices[Symbol] = Price;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    //MessageBox.Show(ex.Message);
                }
            };

            ws.Connect();
            string nonce = bitmex.GetNonce().ToString();
            string message = "GET/realtime" + nonce;
            byte[] sig = bitmex.hmacsha256WS(Encoding.UTF8.GetBytes(message));
            string signature = BitMEXApi.ByteArrayToString(sig);
            string req = "{\"op\": \"authKeyExpires\", \"args\": [\"" + bitmex.apiKey + "\"," + nonce + ",\"" + signature + "\"]}";
            ws.Send(req);

            // Assemble our price dictionary

            ws.Send("{\"op\": \"subscribe\", \"args\": [\"order" + "\"]}");


        }
        private void LogMessage(string msg)
        {
            //log = "[ " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture) + " ] " + msg + Environment.NewLine + log;
            log.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture) + " ] " + msg);
        }

        public bool CheckForInternetConnection()
        {
            try
            {
                using (var client = new WebClient())
                using (client.OpenRead("http://clients3.google.com/generate_204"))
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        private void RefreshOrderStatus()
        {
            while (Running)
            {
                if (CheckForInternetConnection())
                {
                    var AllOrders = bitmex.GetAllOrders(ActiveInstrument.Symbol);
                    if (AllOrders != null)
                    {
                        int i; int j;
                        for (i = 0; i < Orders.Rows.Count; i++)
                        {
                            if (Orders.Rows[i]["OrderID"].ToString().Trim() != "0")
                            {
                                for (j = 0; j < AllOrders.Count; j++)
                                {
                                    if (Orders.Rows[i]["OrderID"].ToString().Trim().ToLower() == AllOrders[j].OrderId.ToLower())
                                    {
                                        break;
                                    }

                                }
                                if (j < AllOrders.Count) //Found
                                {
                                    Orders.Rows[i]["OrderStatus"] = AllOrders[j].OrdStatus;
                                }
                            }
                        }
                    }


                }
                Thread.Sleep(15000);
            }
        }

        private void AutoTrade()
        {
            string resp;
            while (Running)
            {

                if (CheckForInternetConnection() && Candles.Count >= 2)
                {
                    OpenPositions = bitmex.GetOpenPositions(ActiveInstrument.Symbol);
                    OpenOrders = bitmex.GetOpenOrders(ActiveInstrument.Symbol);



                    if (OpenOrders != null && OpenPositions != null)
                    {
                        #region "Close positions"
                        try
                        {
                            if (chkAutoMarketTakeProfits.Checked && OpenPositions.Any() && Mode != "Sell" && Mode != "Buy") // See if we are taking profits on open positions, and have positions open and we aren't in our buy or sell periods
                            {
                                lblAutoUnrealizedROEPercent.Text = Math.Round((Convert.ToDouble(OpenPositions[0].UnrealisedRoePcnt * 100)), 2).ToString();
                                // Did we meet our profit threshold yet?
                                if (Convert.ToDouble(OpenPositions[0].UnrealisedRoePcnt * 100) >= Convert.ToDouble(nudAutoMarketTakeProfitPercent.Value))
                                {
                                    // Make a market order to close out the position, also cancel all orders so nothing else fills if we had unfilled limit orders still open.
                                    string Side = "Sell";
                                    int Quantity = 0;
                                    if (OpenPositions[0].CurrentQty > 0)
                                    {
                                        Side = "Sell";
                                        Quantity = Convert.ToInt32(OpenPositions[0].CurrentQty);
                                    }
                                    else if (OpenPositions[0].CurrentQty < 0)
                                    {
                                        Side = "Buy";
                                        Quantity = Convert.ToInt32(OpenPositions[0].CurrentQty) * -1;
                                    }
                                    #region "Place buy order"
                                    resp = bitmex.MarketOrder(ActiveInstrument.Symbol, Side, Quantity);
                                    if (resp.ToLower().Contains("error"))
                                    {
                                        ErrorObject err = jsonSerializer.Deserialize<ErrorObject>(resp);
                                        LogMessage("Error " + err.error.name + ":" + err.error.message);
                                        AddToOrderList("Market", Side, Quantity.ToString(), "0", "0", "Failed");
                                    }
                                    else
                                    {
                                        LogMessage("Successfully placed " + Side + " order , quantity " + Quantity);
                                        OrderResponse or = jsonSerializer.Deserialize<OrderResponse>(resp);
                                        AddToOrderList("Market", Side, Quantity.ToString(), "0", or.orderID, or.ordStatus);
                                    }
                                    #endregion

                                    // Get our positions and orders again to be able to process rest of logic with new information.
                                    OpenPositions = bitmex.GetOpenPositions(ActiveInstrument.Symbol);
                                    OpenOrders = bitmex.GetOpenOrders(ActiveInstrument.Symbol);
                                }
                            }
                        }
                        catch (Exception ex)
                        {

                            LogMessage("Exception in position close : " + ex.Message);
                        }

                        #endregion


                        try
                        {
                            #region "rdoBuy"
                            if (rdoBuy.Checked)
                            {
                                switch (Mode)
                                {
                                    case "Buy":
                                        // See if we already have a position open
                                        if (OpenPositions.Any())
                                        {
                                            // We have an open position, is it at our desired quantity?
                                            if (OpenPositions[0].CurrentQty < nudAutoQuantity.Value)
                                            {
                                                // If we have an open order, edit it
                                                if (OpenOrders.Any(a => a.Side == "Sell"))
                                                {
                                                    // We still have an open sell order, cancel that order, make a new buy order
                                                    #region "Cancel all orders"
                                                    resp = bitmex.CancelAllOpenOrders(ActiveInstrument.Symbol);
                                                    if (resp.ToLower().Contains("error"))
                                                    {
                                                        ErrorObject err = jsonSerializer.Deserialize<ErrorObject>(resp);
                                                        LogMessage("Error " + err.error.name + ":" + err.error.message);
                                                    }
                                                    else
                                                    {
                                                        LogMessage("Successfully cancelled all open orders");
                                                    }
                                                    //string result = bitmex.CancelAllOpenOrders(ActiveInstrument.Symbol);
                                                    #endregion


                                                    #region "Place buy order"
                                                    resp = AutoMakeOrder("Buy", Convert.ToInt32(OpenPositions[0].CurrentQty));
                                                    if (resp.ToLower().Contains("error"))
                                                    {
                                                        ErrorObject err = jsonSerializer.Deserialize<ErrorObject>(resp);
                                                        LogMessage("Error " + err.error.name + ":" + err.error.message);
                                                        AddToOrderList(ddlAutoOrderType.SelectedItem.ToString(), "Buy", Convert.ToInt32(OpenPositions[0].CurrentQty).ToString(), "0", "0", "Failed");
                                                    }
                                                    else
                                                    {
                                                        LogMessage("Successfully placed buy order");
                                                        OrderResponse or = jsonSerializer.Deserialize<OrderResponse>(resp);
                                                        AddToOrderList(ddlAutoOrderType.SelectedItem.ToString(), "Buy", Convert.ToInt32(OpenPositions[0].CurrentQty).ToString(), "0", or.orderID, or.ordStatus);

                                                    }
                                                    //AutoMakeOrder("Buy", Convert.ToInt32(OpenPositions[0].CurrentQty));
                                                    #endregion
                                                }
                                                else if (OpenOrders.Any(a => a.Side == "Buy") && OpenOrders.Any(a => a.OrdStatus != "Canceled"))
                                                {
                                                    // Edit our only open order, code should not allow for more than 1 at a time for now
                                                    #region "Modify order"
                                                    resp = bitmex.EditOrderPrice(OpenOrders[0].OrderId, CalculateMakerOrderPrice("Buy"));
                                                    if (resp.ToLower().Contains("error"))
                                                    {
                                                        ErrorObject err = jsonSerializer.Deserialize<ErrorObject>(resp);
                                                        LogMessage("Error " + err.error.name + ":" + err.error.message);
                                                    }
                                                    else
                                                    {
                                                        LogMessage("Successfully modified buy order price");
                                                    }
                                                    //string result = bitmex.EditOrderPrice(OpenOrders[0].OrderId, CalculateMakerOrderPrice("Buy"));
                                                    #endregion
                                                }

                                            } // No else, it is filled to where we want.
                                        }
                                        else
                                        {
                                            if (OpenOrders.Any())
                                            {
                                                // If we have an open order, edit it
                                                if (OpenOrders.Any(a => a.Side == "Sell") && OpenOrders.Any(a => a.OrdStatus != "Canceled"))
                                                {
                                                    // We still have an open sell order, cancel that order, make a new buy order
                                                    #region "Cancel Orders"
                                                    resp = bitmex.CancelAllOpenOrders(ActiveInstrument.Symbol);
                                                    if (resp.ToLower().Contains("error"))
                                                    {
                                                        ErrorObject err = jsonSerializer.Deserialize<ErrorObject>(resp);
                                                        LogMessage("Error " + err.error.name + ":" + err.error.message);
                                                    }
                                                    else
                                                    {
                                                        LogMessage("Successfully cancelled all open orders");
                                                    }
                                                    //string result = bitmex.CancelAllOpenOrders(ActiveInstrument.Symbol);

                                                    #endregion

                                                    #region "Place buy order"
                                                    resp = AutoMakeOrder("Buy", Convert.ToInt32(OpenPositions[0].CurrentQty));
                                                    if (resp.ToLower().Contains("error"))
                                                    {
                                                        ErrorObject err = jsonSerializer.Deserialize<ErrorObject>(resp);
                                                        LogMessage("Error " + err.error.name + ":" + err.error.message);
                                                        AddToOrderList(ddlAutoOrderType.SelectedItem.ToString(), "Buy", Convert.ToInt32(OpenPositions[0].CurrentQty).ToString(), "0", "0", "Failed");
                                                    }
                                                    else
                                                    {
                                                        LogMessage("Successfully placed buy order");
                                                        OrderResponse or = jsonSerializer.Deserialize<OrderResponse>(resp);
                                                        AddToOrderList(ddlAutoOrderType.SelectedItem.ToString(), "Buy", Convert.ToInt32(OpenPositions[0].CurrentQty).ToString(), "0", or.orderID, or.ordStatus);

                                                    }
                                                    //AutoMakeOrder("Buy", Convert.ToInt32(OpenPositions[0].CurrentQty));

                                                    #endregion
                                                }
                                                else if (OpenOrders.Any(a => a.Side == "Buy") && OpenOrders.Any(a => a.OrdStatus != "Canceled"))
                                                {
                                                    // Edit our only open order, code should not allow for more than 1 at a time for now
                                                    #region "Modify orders"
                                                    resp = bitmex.EditOrderPrice(OpenOrders[0].OrderId, CalculateMakerOrderPrice("Buy"));
                                                    if (resp.ToLower().Contains("error"))
                                                    {
                                                        ErrorObject err = jsonSerializer.Deserialize<ErrorObject>(resp);
                                                        LogMessage("Error " + err.error.name + ":" + err.error.message);
                                                    }
                                                    else
                                                    {
                                                        LogMessage("Successfully modified buy order price");
                                                    }
                                                    //string result = bitmex.EditOrderPrice(OpenOrders[0].OrderId, CalculateMakerOrderPrice("Buy"));

                                                    #endregion
                                                }
                                            }
                                            else
                                            {
                                                #region "Place buy order"

                                                resp = AutoMakeOrder("Buy", Convert.ToInt32(nudAutoQuantity.Value));
                                                if (resp.ToLower().Contains("error"))
                                                {
                                                    ErrorObject err = jsonSerializer.Deserialize<ErrorObject>(resp);
                                                    LogMessage("Error " + err.error.name + ":" + err.error.message);
                                                    AddToOrderList(ddlAutoOrderType.SelectedItem.ToString(), "Buy", Convert.ToInt32(nudAutoQuantity.Value).ToString(), "0", "0", "Failed");
                                                }
                                                else
                                                {
                                                    LogMessage("Successfully placed buy order");
                                                    OrderResponse or = jsonSerializer.Deserialize<OrderResponse>(resp);
                                                    AddToOrderList(ddlAutoOrderType.SelectedItem.ToString(), "Buy", Convert.ToInt32(nudAutoQuantity.Value).ToString(), "0", or.orderID, or.ordStatus);

                                                }

                                                //AutoMakeOrder("Buy", Convert.ToInt32(nudAutoQuantity.Value));

                                                #endregion
                                            }
                                        }
                                        break;
                                    case "CloseAndWait":
                                        // See if we have open positions, if so, close them
                                        if (OpenPositions.Any())
                                        {
                                            // Now, do we have open orders?  If so, we want to make sure they are at the right price
                                            if (OpenOrders.Any())
                                            {
                                                if (OpenOrders.Any(a => a.Side == "Buy") && OpenOrders.Any(a => a.OrdStatus != "Canceled"))
                                                {
                                                    // We still have an open buy order, cancel that order, make a new sell order
                                                    #region "Cancel all orders"
                                                    resp = bitmex.CancelAllOpenOrders(ActiveInstrument.Symbol);
                                                    if (resp.ToLower().Contains("error"))
                                                    {
                                                        ErrorObject err = jsonSerializer.Deserialize<ErrorObject>(resp);
                                                        LogMessage("Error " + err.error.name + ":" + err.error.message);
                                                    }
                                                    else
                                                    {
                                                        LogMessage("Successfully cancelled all open orders");
                                                    }
                                                    //string result = bitmex.CancelAllOpenOrders(ActiveInstrument.Symbol);
                                                    #endregion


                                                    #region "Place sell order"
                                                    resp = AutoMakeOrder("Sell", Convert.ToInt32(OpenPositions[0].CurrentQty));
                                                    if (resp.ToLower().Contains("error"))
                                                    {
                                                        ErrorObject err = jsonSerializer.Deserialize<ErrorObject>(resp);
                                                        LogMessage("Error " + err.error.name + ":" + err.error.message);
                                                        AddToOrderList(ddlAutoOrderType.SelectedItem.ToString(), "Sell", Convert.ToInt32(OpenPositions[0].CurrentQty).ToString(), "0", "0", "Failed");
                                                    }
                                                    else
                                                    {
                                                        LogMessage("Successfully placed sell order");
                                                        OrderResponse or = jsonSerializer.Deserialize<OrderResponse>(resp);
                                                        AddToOrderList(ddlAutoOrderType.SelectedItem.ToString(), "Sell", Convert.ToInt32(OpenPositions[0].CurrentQty).ToString(), "0", or.orderID, or.ordStatus);

                                                    }
                                                    #endregion
                                                    //string result = bitmex.CancelAllOpenOrders(ActiveInstrument.Symbol);
                                                    //AutoMakeOrder("Sell", Convert.ToInt32(OpenPositions[0].CurrentQty));
                                                }
                                                else if (OpenOrders.Any(a => a.Side == "Sell") && OpenOrders.Any(a => a.OrdStatus != "Canceled"))
                                                {
                                                    // Edit our only open order, code should not allow for more than 1 at a time for now
                                                    #region "modify sell order"
                                                    resp = bitmex.EditOrderPrice(OpenOrders[0].OrderId, CalculateMakerOrderPrice("Sell"));
                                                    if (resp.ToLower().Contains("error"))
                                                    {
                                                        ErrorObject err = jsonSerializer.Deserialize<ErrorObject>(resp);
                                                        LogMessage("Error " + err.error.name + ":" + err.error.message);
                                                    }
                                                    else
                                                    {
                                                        LogMessage("Successfully modified sell order");
                                                    }
                                                    //string result = bitmex.EditOrderPrice(OpenOrders[0].OrderId, CalculateMakerOrderPrice("Sell"));

                                                    #endregion
                                                }

                                            }
                                            else
                                            {
                                                // No open orders, need to make an order to sell
                                                #region "Place sell order"
                                                resp = AutoMakeOrder("Sell", Convert.ToInt32(OpenPositions[0].CurrentQty));
                                                if (resp.ToLower().Contains("error"))
                                                {
                                                    ErrorObject err = jsonSerializer.Deserialize<ErrorObject>(resp);
                                                    LogMessage("Error " + err.error.name + ":" + err.error.message);
                                                    AddToOrderList(ddlAutoOrderType.SelectedItem.ToString(), "Sell", Convert.ToInt32(OpenPositions[0].CurrentQty).ToString(), "0", "0", "Failed");
                                                }
                                                else
                                                {
                                                    LogMessage("Successfully placed sell order");
                                                    OrderResponse or = jsonSerializer.Deserialize<OrderResponse>(resp);
                                                    AddToOrderList(ddlAutoOrderType.SelectedItem.ToString(), "Sell", Convert.ToInt32(OpenPositions[0].CurrentQty).ToString(), "0", or.orderID, or.ordStatus);

                                                }
                                                //AutoMakeOrder("Sell", Convert.ToInt32(OpenPositions[0].CurrentQty));
                                                #endregion
                                            }
                                        }
                                        else if (OpenOrders.Any())
                                        {
                                            // We don't have an open position, but we do have an open order, close that order, we don't want to open any position here.
                                            #region "Cancel all orders"
                                            resp = bitmex.CancelAllOpenOrders(ActiveInstrument.Symbol);
                                            if (resp.ToLower().Contains("error"))
                                            {
                                                ErrorObject err = jsonSerializer.Deserialize<ErrorObject>(resp);
                                                LogMessage("Error " + err.error.name + ":" + err.error.message);
                                            }
                                            else
                                            {
                                                LogMessage("Successfully cancelled all open orders");
                                            }
                                            //string result = bitmex.CancelAllOpenOrders(ActiveInstrument.Symbol);
                                            #endregion
                                            //string result = bitmex.CancelAllOpenOrders(ActiveInstrument.Symbol);
                                        }
                                        break;
                                    case "Wait":
                                        // We are in wait mode, no new buying or selling - close open orders
                                        if (OpenOrders.Any())
                                        {
                                            #region "Cancel all orders"
                                            resp = bitmex.CancelAllOpenOrders(ActiveInstrument.Symbol);
                                            if (resp.ToLower().Contains("error"))
                                            {
                                                ErrorObject err = jsonSerializer.Deserialize<ErrorObject>(resp);
                                                LogMessage("Error " + err.error.name + ":" + err.error.message);
                                            }
                                            else
                                            {
                                                LogMessage("Successfully cancelled all open orders");
                                            }
                                            //string result = bitmex.CancelAllOpenOrders(ActiveInstrument.Symbol);
                                            #endregion
                                            //string result = bitmex.CancelAllOpenOrders(ActiveInstrument.Symbol);
                                        }
                                        break;
                                }
                            }
                            #endregion

                            #region "rdoSell"
                            else if (rdoSell.Checked)
                            {
                                switch (Mode)
                                {
                                    case "Sell":
                                        // See if we already have a position open
                                        if (OpenPositions.Any())
                                        {
                                            // We have an open position, is it at our desired quantity?
                                            if (OpenPositions[0].CurrentQty < nudAutoQuantity.Value)
                                            {
                                                // If we have an open order, edit it
                                                if (OpenOrders.Any(a => a.Side == "Buy"))
                                                {
                                                    // We still have an open Buy order, cancel that order, make a new Sell order
                                                    #region "Cancel all orders"
                                                    resp = bitmex.CancelAllOpenOrders(ActiveInstrument.Symbol);
                                                    if (resp.ToLower().Contains("error"))
                                                    {
                                                        ErrorObject err = jsonSerializer.Deserialize<ErrorObject>(resp);
                                                        LogMessage("Error " + err.error.name + ":" + err.error.message);
                                                    }
                                                    else
                                                    {
                                                        LogMessage("Successfully cancelled all orders");
                                                    }
                                                    #endregion

                                                    #region "Place sell order"
                                                    resp = AutoMakeOrder("Sell", Convert.ToInt32(OpenPositions[0].CurrentQty));
                                                    if (resp.ToLower().Contains("error"))
                                                    {
                                                        ErrorObject err = jsonSerializer.Deserialize<ErrorObject>(resp);
                                                        LogMessage("Error " + err.error.name + ":" + err.error.message);
                                                        AddToOrderList(ddlAutoOrderType.SelectedItem.ToString(), "Sell", Convert.ToInt32(OpenPositions[0].CurrentQty).ToString(), "0", "0", "Failed");
                                                    }
                                                    else
                                                    {
                                                        LogMessage("Successfully placed sell order");
                                                        OrderResponse or = jsonSerializer.Deserialize<OrderResponse>(resp);
                                                        AddToOrderList(ddlAutoOrderType.SelectedItem.ToString(), "Sell", Convert.ToInt32(OpenPositions[0].CurrentQty).ToString(), "0", or.orderID, or.ordStatus);

                                                    }
                                                    #endregion
                                                }
                                                else if (OpenOrders.Any(a => a.Side == "Sell") && OpenOrders.Any(a => a.OrdStatus != "Canceled"))
                                                {
                                                    // Edit our only open order, code should not allow for more than 1 at a time for now
                                                    #region "Modify order"
                                                    resp = bitmex.EditOrderPrice(OpenOrders[0].OrderId, CalculateMakerOrderPrice("Sell"));
                                                    if (resp.ToLower().Contains("error"))
                                                    {
                                                        ErrorObject err = jsonSerializer.Deserialize<ErrorObject>(resp);
                                                        LogMessage("Error " + err.error.name + ":" + err.error.message);
                                                    }
                                                    else
                                                    {
                                                        LogMessage("Successfully modified order");
                                                    }
                                                    #endregion
                                                }

                                            } // No else, it is filled to where we want.
                                        }
                                        else
                                        {
                                            if (OpenOrders.Any())
                                            {
                                                // If we have an open order, edit it
                                                if (OpenOrders.Any(a => a.Side == "Buy") && OpenOrders.Any(a => a.OrdStatus != "Canceled"))
                                                {
                                                    // We still have an open buy order, cancel that order, make a new sell order
                                                    #region "Cancel all orders"
                                                    resp = bitmex.CancelAllOpenOrders(ActiveInstrument.Symbol);
                                                    if (resp.ToLower().Contains("error"))
                                                    {
                                                        ErrorObject err = jsonSerializer.Deserialize<ErrorObject>(resp);
                                                        LogMessage("Error " + err.error.name + ":" + err.error.message);
                                                    }
                                                    else
                                                    {
                                                        LogMessage("Successfully cancelled all orders");
                                                    }
                                                    #endregion

                                                    #region "Place sell order"
                                                    resp = AutoMakeOrder("Sell", Convert.ToInt32(OpenPositions[0].CurrentQty));
                                                    if (resp.ToLower().Contains("error"))
                                                    {
                                                        ErrorObject err = jsonSerializer.Deserialize<ErrorObject>(resp);
                                                        LogMessage("Error " + err.error.name + ":" + err.error.message);
                                                        AddToOrderList(ddlAutoOrderType.SelectedItem.ToString(), "Sell", Convert.ToInt32(OpenPositions[0].CurrentQty).ToString(), "0", "0", "Failed");
                                                    }
                                                    else
                                                    {
                                                        LogMessage("Successfully placed sell order");
                                                        OrderResponse or = jsonSerializer.Deserialize<OrderResponse>(resp);
                                                        AddToOrderList(ddlAutoOrderType.SelectedItem.ToString(), "Sell", Convert.ToInt32(OpenPositions[0].CurrentQty).ToString(), "0", or.orderID, or.ordStatus);

                                                    }
                                                    #endregion
                                                }
                                                else if (OpenOrders.Any(a => a.Side == "Sell") && OpenOrders.Any(a => a.OrdStatus != "Canceled"))
                                                {
                                                    // Edit our only open order, code should not allow for more than 1 at a time for now
                                                    #region "Modify order"
                                                    resp = bitmex.EditOrderPrice(OpenOrders[0].OrderId, CalculateMakerOrderPrice("Sell"));
                                                    if (resp.ToLower().Contains("error"))
                                                    {
                                                        ErrorObject err = jsonSerializer.Deserialize<ErrorObject>(resp);
                                                        LogMessage("Error " + err.error.name + ":" + err.error.message);
                                                    }
                                                    else
                                                    {
                                                        LogMessage("Successfully modified order");
                                                    }
                                                    #endregion
                                                }
                                            }
                                            else
                                            {
                                                #region "Place sell order"
                                                resp = AutoMakeOrder("Sell", Convert.ToInt32(nudAutoQuantity.Value));
                                                if (resp.ToLower().Contains("error"))
                                                {
                                                    ErrorObject err = jsonSerializer.Deserialize<ErrorObject>(resp);
                                                    LogMessage("Error " + err.error.name + ":" + err.error.message);
                                                    AddToOrderList(ddlAutoOrderType.SelectedItem.ToString(), "Sell", Convert.ToInt32(nudAutoQuantity.Value).ToString(), "0", "0", "Failed");
                                                }
                                                else
                                                {
                                                    LogMessage("Successfully placed sell order");
                                                    OrderResponse or = jsonSerializer.Deserialize<OrderResponse>(resp);
                                                    AddToOrderList(ddlAutoOrderType.SelectedItem.ToString(), "Sell", Convert.ToInt32(nudAutoQuantity.Value).ToString(), "0", or.orderID, or.ordStatus);

                                                }
                                                #endregion
                                            }
                                        }
                                        break;
                                    case "CloseAndWait":
                                        // See if we have open positions, if so, close them
                                        if (OpenPositions.Any())
                                        {
                                            // Now, do we have open orders?  If so, we want to make sure they are at the right price
                                            if (OpenOrders.Any())
                                            {
                                                if (OpenOrders.Any(a => a.Side == "Sell") && OpenOrders.Any(a => a.OrdStatus != "Canceled"))
                                                {
                                                    // We still have an open sell order, cancel that order, make a new buy order
                                                    #region "Cancel all orders"
                                                    resp = bitmex.CancelAllOpenOrders(ActiveInstrument.Symbol);
                                                    if (resp.ToLower().Contains("error"))
                                                    {
                                                        ErrorObject err = jsonSerializer.Deserialize<ErrorObject>(resp);
                                                        LogMessage("Error " + err.error.name + ":" + err.error.message);
                                                    }
                                                    else
                                                    {
                                                        LogMessage("Successfully cancelled all orders");
                                                    }
                                                    #endregion

                                                    #region "Place buy order"
                                                    resp = AutoMakeOrder("Buy", Convert.ToInt32(OpenPositions[0].CurrentQty));
                                                    if (resp.ToLower().Contains("error"))
                                                    {
                                                        ErrorObject err = jsonSerializer.Deserialize<ErrorObject>(resp);
                                                        LogMessage("Error " + err.error.name + ":" + err.error.message);
                                                        AddToOrderList(ddlAutoOrderType.SelectedItem.ToString(), "Buy", Convert.ToInt32(OpenPositions[0].CurrentQty).ToString(), "0", "0", "Failed");
                                                    }
                                                    else
                                                    {
                                                        LogMessage("Successfully placed buy order");
                                                        OrderResponse or = jsonSerializer.Deserialize<OrderResponse>(resp);
                                                        AddToOrderList(ddlAutoOrderType.SelectedItem.ToString(), "Buy", Convert.ToInt32(OpenPositions[0].CurrentQty).ToString(), "0", or.orderID, or.ordStatus);

                                                    }
                                                    #endregion
                                                }
                                                else if (OpenOrders.Any(a => a.Side == "Buy") && OpenOrders.Any(a => a.OrdStatus != "Canceled"))
                                                {
                                                    // Edit our only open order, code should not allow for more than 1 at a time for now
                                                    #region "Modify Order"
                                                    resp = bitmex.EditOrderPrice(OpenOrders[0].OrderId, CalculateMakerOrderPrice("Buy"));
                                                    if (resp.ToLower().Contains("error"))
                                                    {
                                                        ErrorObject err = jsonSerializer.Deserialize<ErrorObject>(resp);
                                                        LogMessage("Error " + err.error.name + ":" + err.error.message);
                                                    }
                                                    else
                                                    {
                                                        LogMessage("Successfully modified buy order");
                                                        //AddToOrderList("", "Buy", Convert.ToInt32(OpenPositions[0].CurrentQty).ToString(), "0");

                                                    }
                                                    #endregion
                                                }

                                            }
                                            else
                                            {
                                                // No open orders, need to make an order to sell
                                                #region "Place buy Order"
                                                resp = AutoMakeOrder("Buy", Convert.ToInt32(OpenPositions[0].CurrentQty));
                                                if (resp.ToLower().Contains("error"))
                                                {
                                                    ErrorObject err = jsonSerializer.Deserialize<ErrorObject>(resp);
                                                    LogMessage("Error " + err.error.name + ":" + err.error.message);
                                                    AddToOrderList(ddlAutoOrderType.SelectedItem.ToString(), "Buy", Convert.ToInt32(OpenPositions[0].CurrentQty).ToString(), "0", "0", "Failed");
                                                }
                                                else
                                                {
                                                    LogMessage("Successfully placed buy order");
                                                    OrderResponse or = jsonSerializer.Deserialize<OrderResponse>(resp);
                                                    AddToOrderList(ddlAutoOrderType.SelectedItem.ToString(), "Buy", Convert.ToInt32(OpenPositions[0].CurrentQty).ToString(), "0", or.orderID, or.ordStatus);

                                                }
                                                #endregion
                                            }
                                        }
                                        else if (OpenOrders.Any())
                                        {
                                            // We don't have an open position, but we do have an open order, close that order, we don't want to open any position here.
                                            #region "Cancel all orders"
                                            resp = bitmex.CancelAllOpenOrders(ActiveInstrument.Symbol);
                                            if (resp.ToLower().Contains("error"))
                                            {
                                                ErrorObject err = jsonSerializer.Deserialize<ErrorObject>(resp);
                                                LogMessage("Error " + err.error.name + ":" + err.error.message);
                                            }
                                            else
                                            {
                                                LogMessage("Successfully cancelled all orders");
                                            }
                                            #endregion
                                        }
                                        break;
                                    case "Wait":
                                        // We are in wait mode, no new buying or selling - close open orders
                                        if (OpenOrders.Any())
                                        {
                                            #region "Cancel all orders"
                                            resp = bitmex.CancelAllOpenOrders(ActiveInstrument.Symbol);
                                            if (resp.ToLower().Contains("error"))
                                            {
                                                ErrorObject err = jsonSerializer.Deserialize<ErrorObject>(resp);
                                                LogMessage("Error " + err.error.name + ":" + err.error.message);
                                            }
                                            else
                                            {
                                                LogMessage("Successfully cancelled all orders");
                                            }
                                            #endregion
                                        }
                                        break;
                                }
                            }
                            #endregion

                            #region "rdoSwitch"
                            else if (rdoSwitch.Checked)
                            {
                                switch (Mode)
                                {
                                    case "Buy":
                                        if (OpenPositions.Any())
                                        {
                                            int PositionDifference = Convert.ToInt32(nudAutoQuantity.Value - OpenPositions[0].CurrentQty);

                                            if (OpenOrders.Any())
                                            {
                                                // If we have an open order, edit it
                                                if (OpenOrders.Any(a => a.Side == "Sell") && OpenOrders.Any(a => a.OrdStatus != "Canceled"))
                                                {
                                                    // We still have an open Sell order, cancel that order, make a new Buy order
                                                    #region "Cancel All Orders"
                                                    resp = bitmex.CancelAllOpenOrders(ActiveInstrument.Symbol);
                                                    if (resp.ToLower().Contains("error"))
                                                    {
                                                        ErrorObject err = jsonSerializer.Deserialize<ErrorObject>(resp);
                                                        LogMessage("Error " + err.error.name + ":" + err.error.message);
                                                    }
                                                    else
                                                    {
                                                        LogMessage("Successfully cancelled all orders");
                                                    }
                                                    #endregion


                                                    #region "Place Buy order"
                                                    resp = AutoMakeOrder("Buy", PositionDifference);
                                                    if (resp.ToLower().Contains("error"))
                                                    {
                                                        ErrorObject err = jsonSerializer.Deserialize<ErrorObject>(resp);
                                                        LogMessage("Error " + err.error.name + ":" + err.error.message);
                                                        AddToOrderList(ddlAutoOrderType.SelectedItem.ToString(), "Buy", PositionDifference.ToString(), "0", "0", "Failed");
                                                    }
                                                    else
                                                    {
                                                        LogMessage("Successfully placed buy order");
                                                        OrderResponse or = jsonSerializer.Deserialize<OrderResponse>(resp);
                                                        AddToOrderList(ddlAutoOrderType.SelectedItem.ToString(), "Buy", PositionDifference.ToString(), "0", or.orderID, or.ordStatus);

                                                    }
                                                    #endregion
                                                }
                                                else if (OpenOrders.Any(a => a.Side == "Buy") && OpenOrders.Any(a => a.OrdStatus != "Canceled"))
                                                {
                                                    // Edit our only open order, code should not allow for more than 1 at a time for now
                                                    #region "Modify order"
                                                    resp = bitmex.EditOrderPrice(OpenOrders[0].OrderId, CalculateMakerOrderPrice("Buy"));
                                                    if (resp.ToLower().Contains("error"))
                                                    {
                                                        ErrorObject err = jsonSerializer.Deserialize<ErrorObject>(resp);
                                                        LogMessage("Error " + err.error.name + ":" + err.error.message);
                                                    }
                                                    else
                                                    {
                                                        LogMessage("Successfully Modified order");
                                                    }
                                                    #endregion
                                                }
                                            }
                                            else
                                            {
                                                // No open orders, make one for the difference
                                                if (PositionDifference != 0)
                                                {
                                                    #region "Place buy order"
                                                    resp = AutoMakeOrder("Buy", Convert.ToInt32(PositionDifference));
                                                    if (resp.ToLower().Contains("error"))
                                                    {
                                                        ErrorObject err = jsonSerializer.Deserialize<ErrorObject>(resp);
                                                        LogMessage("Error " + err.error.name + ":" + err.error.message);
                                                        AddToOrderList(ddlAutoOrderType.SelectedItem.ToString(), "Buy", PositionDifference.ToString(), "0", "0", "Failed");
                                                    }
                                                    else
                                                    {
                                                        LogMessage("Successfully placed buy order");
                                                        OrderResponse or = jsonSerializer.Deserialize<OrderResponse>(resp);
                                                        AddToOrderList(ddlAutoOrderType.SelectedItem.ToString(), "Buy", PositionDifference.ToString(), "0", or.orderID, or.ordStatus);

                                                    }
                                                    #endregion
                                                }

                                            }

                                        }
                                        else
                                        {
                                            if (OpenOrders.Any())
                                            {
                                                // If we have an open order, edit it
                                                if (OpenOrders.Any(a => a.Side == "Sell") && OpenOrders.Any(a => a.OrdStatus != "Canceled"))
                                                {
                                                    // We still have an open Sell order, cancel that order, make a new Buy order
                                                    #region "Cancel All orders"
                                                    resp = bitmex.CancelAllOpenOrders(ActiveInstrument.Symbol);
                                                    if (resp.ToLower().Contains("error"))
                                                    {
                                                        ErrorObject err = jsonSerializer.Deserialize<ErrorObject>(resp);
                                                        LogMessage("Error " + err.error.name + ":" + err.error.message);
                                                    }
                                                    else
                                                    {
                                                        LogMessage("Successfully cancelled all orders");
                                                    }
                                                    #endregion

                                                    #region  "Place buy order"


                                                    resp = AutoMakeOrder("Buy", Convert.ToInt32(nudAutoQuantity.Value));
                                                    if (resp.ToLower().Contains("error"))
                                                    {
                                                        ErrorObject err = jsonSerializer.Deserialize<ErrorObject>(resp);
                                                        LogMessage("Error " + err.error.name + ":" + err.error.message);
                                                        AddToOrderList(ddlAutoOrderType.SelectedItem.ToString(), "Buy", Convert.ToInt32(nudAutoQuantity.Value).ToString(), "0", "0", "Failed");
                                                    }
                                                    else
                                                    {
                                                        LogMessage("Successfully placed buy order");
                                                        OrderResponse or = jsonSerializer.Deserialize<OrderResponse>(resp);
                                                        AddToOrderList(ddlAutoOrderType.SelectedItem.ToString(), "Buy", Convert.ToInt32(nudAutoQuantity.Value).ToString(), "0", or.orderID, or.ordStatus);

                                                    }
                                                    #endregion
                                                }
                                                else if (OpenOrders.Any(a => a.Side == "Buy") && OpenOrders.Any(a => a.OrdStatus != "Canceled"))
                                                {
                                                    // Edit our only open order, code should not allow for more than 1 at a time for now
                                                    #region "Modify order"
                                                    resp = bitmex.EditOrderPrice(OpenOrders[0].OrderId, CalculateMakerOrderPrice("Buy"));
                                                    if (resp.ToLower().Contains("error"))
                                                    {
                                                        ErrorObject err = jsonSerializer.Deserialize<ErrorObject>(resp);
                                                        LogMessage("Error " + err.error.name + ":" + err.error.message);
                                                    }
                                                    else
                                                    {
                                                        LogMessage("Successfullymodified order");
                                                    }
                                                    #endregion
                                                }
                                            }
                                            else
                                            {
                                                #region "Place buy order"
                                                resp = AutoMakeOrder("Buy", Convert.ToInt32(nudAutoQuantity.Value));
                                                if (resp.ToLower().Contains("error"))
                                                {
                                                    ErrorObject err = jsonSerializer.Deserialize<ErrorObject>(resp);
                                                    LogMessage("Error " + err.error.name + ":" + err.error.message);
                                                    AddToOrderList(ddlAutoOrderType.SelectedItem.ToString(), "Buy", Convert.ToInt32(nudAutoQuantity.Value).ToString(), "0", "0", "Failed");
                                                }
                                                else
                                                {
                                                    LogMessage("Successfully placed buy order");
                                                    OrderResponse or = jsonSerializer.Deserialize<OrderResponse>(resp);
                                                    AddToOrderList(ddlAutoOrderType.SelectedItem.ToString(), "Buy", Convert.ToInt32(nudAutoQuantity.Value).ToString(), "0", or.orderID, or.ordStatus);

                                                }
                                                #endregion
                                            }
                                        }
                                        break;
                                    case "Sell":
                                        if (OpenPositions.Any())
                                        {
                                            int PositionDifference = Convert.ToInt32(nudAutoQuantity.Value + OpenPositions[0].CurrentQty);

                                            if (OpenOrders.Any())
                                            {
                                                // If we have an open order, edit it
                                                if (OpenOrders.Any(a => a.Side == "Buy") && OpenOrders.Any(a => a.OrdStatus != "Canceled"))
                                                {
                                                    // We still have an open Sell order, cancel that order, make a new Buy order

                                                    #region "Cancel all orders"
                                                    resp = bitmex.CancelAllOpenOrders(ActiveInstrument.Symbol);
                                                    if (resp.ToLower().Contains("error"))
                                                    {
                                                        ErrorObject err = jsonSerializer.Deserialize<ErrorObject>(resp);
                                                        LogMessage("Error " + err.error.name + ":" + err.error.message);
                                                    }
                                                    else
                                                    {
                                                        LogMessage("Successfully cancelled all orders");
                                                    }
                                                    #endregion

                                                    #region  "Place sell order"
                                                    resp = AutoMakeOrder("Sell", PositionDifference);
                                                    if (resp.ToLower().Contains("error"))
                                                    {
                                                        ErrorObject err = jsonSerializer.Deserialize<ErrorObject>(resp);
                                                        LogMessage("Error " + err.error.name + ":" + err.error.message);
                                                        AddToOrderList(ddlAutoOrderType.SelectedItem.ToString(), "Sell", PositionDifference.ToString(), "0", "0", "Failed");
                                                    }
                                                    else
                                                    {
                                                        LogMessage("Successfully placed sell order");
                                                        OrderResponse or = jsonSerializer.Deserialize<OrderResponse>(resp);
                                                        AddToOrderList(ddlAutoOrderType.SelectedItem.ToString(), "Sell", PositionDifference.ToString(), "0", or.orderID, or.ordStatus);

                                                    }
                                                    #endregion
                                                }
                                                else if (OpenOrders.Any(a => a.Side == "Sell") && OpenOrders.Any(a => a.OrdStatus != "Canceled"))
                                                {
                                                    // Edit our only open order, code should not allow for more than 1 at a time for now
                                                    #region "Modify order"
                                                    resp = bitmex.EditOrderPrice(OpenOrders[0].OrderId, CalculateMakerOrderPrice("Sell"));
                                                    if (resp.ToLower().Contains("error"))
                                                    {
                                                        ErrorObject err = jsonSerializer.Deserialize<ErrorObject>(resp);
                                                        LogMessage("Error " + err.error.name + ":" + err.error.message);
                                                    }
                                                    else
                                                    {
                                                        LogMessage("Successfully modified order");
                                                    }
                                                    #endregion
                                                }
                                            }
                                            else
                                            {
                                                // No open orders, make one for the difference
                                                if (PositionDifference != 0)
                                                {
                                                    #region "Place sell order"
                                                    resp = AutoMakeOrder("Sell", Convert.ToInt32(PositionDifference));
                                                    if (resp.ToLower().Contains("error"))
                                                    {
                                                        ErrorObject err = jsonSerializer.Deserialize<ErrorObject>(resp);
                                                        LogMessage("Error " + err.error.name + ":" + err.error.message);
                                                        AddToOrderList(ddlAutoOrderType.SelectedItem.ToString(), "Sell", PositionDifference.ToString(), "0", "0", "Failed");
                                                    }
                                                    else
                                                    {
                                                        LogMessage("Successfully placed sell order");
                                                        OrderResponse or = jsonSerializer.Deserialize<OrderResponse>(resp);
                                                        AddToOrderList(ddlAutoOrderType.SelectedItem.ToString(), "Sell", PositionDifference.ToString(), "0", or.orderID, or.ordStatus);

                                                    }
                                                    #endregion
                                                }

                                            }

                                        }
                                        else
                                        {
                                            if (OpenOrders.Any())
                                            {
                                                // If we have an open order, edit it
                                                if (OpenOrders.Any(a => a.Side == "Buy") && OpenOrders.Any(a => a.OrdStatus != "Canceled"))
                                                {
                                                    // We still have an open Sell order, cancel that order, make a new Buy order

                                                    #region "Cancel all orders"
                                                    resp = bitmex.CancelAllOpenOrders(ActiveInstrument.Symbol);
                                                    if (resp.ToLower().Contains("error"))
                                                    {
                                                        ErrorObject err = jsonSerializer.Deserialize<ErrorObject>(resp);
                                                        LogMessage("Error " + err.error.name + ":" + err.error.message);
                                                    }
                                                    else
                                                    {
                                                        LogMessage("Successfully cancelled all orders");
                                                    }
                                                    #endregion

                                                    #region "Place sell order"
                                                    resp = AutoMakeOrder("Sell", Convert.ToInt32(nudAutoQuantity.Value));
                                                    if (resp.ToLower().Contains("error"))
                                                    {
                                                        ErrorObject err = jsonSerializer.Deserialize<ErrorObject>(resp);
                                                        LogMessage("Error " + err.error.name + ":" + err.error.message);
                                                        AddToOrderList(ddlAutoOrderType.SelectedItem.ToString(), "Sell", Convert.ToInt32(nudAutoQuantity.Value).ToString(), "0", "0", "Failed");
                                                    }
                                                    else
                                                    {
                                                        LogMessage("Successfully placed sell order");
                                                        OrderResponse or = jsonSerializer.Deserialize<OrderResponse>(resp);
                                                        AddToOrderList(ddlAutoOrderType.SelectedItem.ToString(), "Sell", Convert.ToInt32(nudAutoQuantity.Value).ToString(), "0", or.orderID, or.ordStatus);

                                                    }
                                                    #endregion
                                                }
                                                else if (OpenOrders.Any(a => a.Side == "Sell") && OpenOrders.Any(a => a.OrdStatus != "Canceled"))
                                                {
                                                    // Edit our only open order, code should not allow for more than 1 at a time for now

                                                    #region "Modify order"
                                                    resp = bitmex.EditOrderPrice(OpenOrders[0].OrderId, CalculateMakerOrderPrice("Sell"));
                                                    if (resp.ToLower().Contains("error"))
                                                    {
                                                        ErrorObject err = jsonSerializer.Deserialize<ErrorObject>(resp);
                                                        LogMessage("Error " + err.error.name + ":" + err.error.message);
                                                    }
                                                    else
                                                    {
                                                        LogMessage("Successfully modified order");
                                                    }
                                                    #endregion
                                                }
                                            }
                                            else
                                            {

                                                #region "Place sell order"
                                                resp = AutoMakeOrder("Sell", Convert.ToInt32(nudAutoQuantity.Value));

                                                if (resp.ToLower().Contains("error"))
                                                {
                                                    ErrorObject err = jsonSerializer.Deserialize<ErrorObject>(resp);
                                                    LogMessage("Error " + err.error.name + ":" + err.error.message);
                                                    AddToOrderList(ddlAutoOrderType.SelectedItem.ToString(), "Sell", Convert.ToInt32(nudAutoQuantity.Value).ToString(), "0", "0", "Failed");
                                                }
                                                else
                                                {
                                                    LogMessage("Successfully placed sell order");
                                                    OrderResponse or = jsonSerializer.Deserialize<OrderResponse>(resp);
                                                    AddToOrderList(ddlAutoOrderType.SelectedItem.ToString(), "Sell", Convert.ToInt32(nudAutoQuantity.Value).ToString(), "0", or.orderID, or.ordStatus);

                                                }

                                                #endregion
                                            }
                                        }
                                        break;
                                    case "CloseLongsAndWait":
                                        if (OpenPositions.Any())
                                        {
                                            // Now, do we have open orders?  If so, we want to make sure they are at the right price
                                            if (OpenOrders.Any())
                                            {
                                                if (OpenOrders.Any(a => a.Side == "Buy") && OpenOrders.Any(a => a.OrdStatus != "Canceled"))
                                                {
                                                    // We still have an open buy order, cancel that order, make a new sell order
                                                    #region "Cancel all orders"
                                                    resp = bitmex.CancelAllOpenOrders(ActiveInstrument.Symbol);
                                                    if (resp.ToLower().Contains("error"))
                                                    {
                                                        ErrorObject err = jsonSerializer.Deserialize<ErrorObject>(resp);
                                                        LogMessage("Error " + err.error.name + ":" + err.error.message);
                                                    }
                                                    else
                                                    {
                                                        LogMessage("Successfully cancelled all orders");
                                                    }
                                                    #endregion

                                                    #region "Place sell order"
                                                    resp = AutoMakeOrder("Sell", Convert.ToInt32(OpenPositions[0].CurrentQty));
                                                    if (resp.ToLower().Contains("error"))
                                                    {
                                                        ErrorObject err = jsonSerializer.Deserialize<ErrorObject>(resp);
                                                        LogMessage("Error " + err.error.name + ":" + err.error.message);
                                                        AddToOrderList(ddlAutoOrderType.SelectedItem.ToString(), "Sell", Convert.ToInt32(OpenPositions[0].CurrentQty).ToString(), "0", "0", "Failed");
                                                    }
                                                    else
                                                    {
                                                        LogMessage("Successfully placed sell order");
                                                        OrderResponse or = jsonSerializer.Deserialize<OrderResponse>(resp);
                                                        AddToOrderList(ddlAutoOrderType.SelectedItem.ToString(), "Sell", Convert.ToInt32(OpenPositions[0].CurrentQty).ToString(), "0", or.orderID, or.ordStatus);

                                                    }
                                                    #endregion
                                                }
                                                else if (OpenOrders.Any(a => a.Side == "Sell") && OpenOrders.Any(a => a.OrdStatus != "Canceled"))
                                                {
                                                    // Edit our only open order, code should not allow for more than 1 at a time for now
                                                    #region "Modify order"
                                                    resp = bitmex.EditOrderPrice(OpenOrders[0].OrderId, CalculateMakerOrderPrice("Sell"));
                                                    if (resp.ToLower().Contains("error"))
                                                    {
                                                        ErrorObject err = jsonSerializer.Deserialize<ErrorObject>(resp);
                                                        LogMessage("Error " + err.error.name + ":" + err.error.message);
                                                    }
                                                    else
                                                    {
                                                        LogMessage("Successfully modified order");
                                                    }
                                                    #endregion
                                                }

                                            }
                                            else if (OpenPositions[0].CurrentQty > 0)
                                            {
                                                // No open orders, need to make an order to sell

                                                #region "Place sell order"
                                                resp = AutoMakeOrder("Sell", Convert.ToInt32(OpenPositions[0].CurrentQty));

                                                if (resp.ToLower().Contains("error"))
                                                {
                                                    ErrorObject err = jsonSerializer.Deserialize<ErrorObject>(resp);
                                                    LogMessage("Error " + err.error.name + ":" + err.error.message);
                                                    AddToOrderList(ddlAutoOrderType.SelectedItem.ToString(), "Sell", Convert.ToInt32(OpenPositions[0].CurrentQty).ToString(), "0", "0", "Failed");
                                                }
                                                else
                                                {
                                                    LogMessage("Successfully placed sell order");
                                                    OrderResponse or = jsonSerializer.Deserialize<OrderResponse>(resp);
                                                    AddToOrderList(ddlAutoOrderType.SelectedItem.ToString(), "Sell", Convert.ToInt32(OpenPositions[0].CurrentQty).ToString(), "0", or.orderID, or.ordStatus);

                                                }

                                                #endregion
                                            }
                                        }
                                        else if (OpenOrders.Any())
                                        {
                                            // We don't have an open position, but we do have an open order, close that order, we don't want to open any position here.

                                            #region "Cancel all orders"
                                            resp = bitmex.CancelAllOpenOrders(ActiveInstrument.Symbol);

                                            if (resp.ToLower().Contains("error"))
                                            {
                                                ErrorObject err = jsonSerializer.Deserialize<ErrorObject>(resp);
                                                LogMessage("Error " + err.error.name + ":" + err.error.message);
                                            }
                                            else
                                            {
                                                LogMessage("Successfully cancelled all orders");
                                            }
                                            #endregion
                                        }
                                        break;
                                    case "CloseShortsAndWait":
                                        // Close any open orders, close any open shorts, we've missed our chance to long.
                                        if (OpenPositions.Any())
                                        {
                                            // Now, do we have open orders?  If so, we want to make sure they are at the right price
                                            if (OpenOrders.Any())
                                            {
                                                if (OpenOrders.Any(a => a.Side == "Sell") && OpenOrders.Any(a => a.OrdStatus != "Canceled"))
                                                {
                                                    // We still have an open sell order, cancel that order, make a new buy order
                                                    #region  "Cancell all orders"


                                                    resp = bitmex.CancelAllOpenOrders(ActiveInstrument.Symbol);
                                                    if (resp.ToLower().Contains("error"))
                                                    {
                                                        ErrorObject err = jsonSerializer.Deserialize<ErrorObject>(resp);
                                                        LogMessage("Error " + err.error.name + ":" + err.error.message);
                                                    }
                                                    else
                                                    {
                                                        LogMessage("Successfully cancelled all orders");
                                                    }
                                                    #endregion

                                                    #region "Place buy order"
                                                    resp = AutoMakeOrder("Buy", Convert.ToInt32(OpenPositions[0].CurrentQty));
                                                    if (resp.ToLower().Contains("error"))
                                                    {
                                                        ErrorObject err = jsonSerializer.Deserialize<ErrorObject>(resp);
                                                        LogMessage("Error " + err.error.name + ":" + err.error.message);
                                                        AddToOrderList(ddlAutoOrderType.SelectedItem.ToString(), "Buy", Convert.ToInt32(OpenPositions[0].CurrentQty).ToString(), "0", "0", "Failed");
                                                    }
                                                    else
                                                    {
                                                        LogMessage("Successfully placed buy order");
                                                        OrderResponse or = jsonSerializer.Deserialize<OrderResponse>(resp);
                                                        AddToOrderList(ddlAutoOrderType.SelectedItem.ToString(), "Buy", Convert.ToInt32(OpenPositions[0].CurrentQty).ToString(), "0", or.orderID, or.ordStatus);

                                                    }
                                                    #endregion
                                                }
                                                else if (OpenOrders.Any(a => a.Side == "Buy") && OpenOrders.Any(a => a.OrdStatus != "Canceled"))
                                                {
                                                    // Edit our only open order, code should not allow for more than 1 at a time for now
                                                    #region "Modify order"
                                                    resp = bitmex.EditOrderPrice(OpenOrders[0].OrderId, CalculateMakerOrderPrice("Buy"));
                                                    if (resp.ToLower().Contains("error"))
                                                    {
                                                        ErrorObject err = jsonSerializer.Deserialize<ErrorObject>(resp);
                                                        LogMessage("Error " + err.error.name + ":" + err.error.message);
                                                    }
                                                    else
                                                    {
                                                        LogMessage("Successfully modified order to price ");
                                                    }
                                                    #endregion
                                                }

                                            }
                                            else if (OpenPositions[0].CurrentQty < 0)
                                            {
                                                // No open orders, need to make an order to sell
                                                #region "Place buy order"
                                                resp = AutoMakeOrder("Buy", Convert.ToInt32(OpenPositions[0].CurrentQty));
                                                if (resp.ToLower().Contains("error"))
                                                {
                                                    ErrorObject err = jsonSerializer.Deserialize<ErrorObject>(resp);
                                                    LogMessage("Error " + err.error.name + ":" + err.error.message);
                                                    AddToOrderList(ddlAutoOrderType.SelectedItem.ToString(), "Buy", Convert.ToInt32(OpenPositions[0].CurrentQty).ToString(), "0", "0", "Failed");
                                                }
                                                else
                                                {
                                                    LogMessage("Successfully placed buy order");
                                                    OrderResponse or = jsonSerializer.Deserialize<OrderResponse>(resp);
                                                    AddToOrderList(ddlAutoOrderType.SelectedItem.ToString(), "Buy", Convert.ToInt32(OpenPositions[0].CurrentQty).ToString(), "0", or.orderID, or.ordStatus);

                                                }
                                                #endregion
                                            }
                                        }
                                        else if (OpenOrders.Any())
                                        {
                                            // We don't have an open position, but we do have an open order, close that order, we don't want to open any position here.
                                            #region "Cancel all orders"
                                            resp = bitmex.CancelAllOpenOrders(ActiveInstrument.Symbol);
                                            if (resp.ToLower().Contains("error"))
                                            {
                                                ErrorObject err = jsonSerializer.Deserialize<ErrorObject>(resp);
                                                LogMessage("Error " + err.error.name + ":" + err.error.message);
                                            }
                                            else
                                            {
                                                LogMessage("Successfully cancelled all orders");
                                            }
                                            #endregion
                                        }
                                        break;
                                }
                            }
                            #endregion
                        }
                        catch (Exception ex)
                        {

                            LogMessage("Exception : " + ex.Message);
                        }


                    }

                }
                else
                {
                    LogMessage("Internet connectivity is not available");
                }

                Thread.Sleep(10000);
            }
        }
        #endregion
        public Form1()
        {
            InitializeComponent();
            InitializeDropdownsAndSettings();
            InitializeAPI();
            InitializeCandleArea();
            InitializeOverTime();

        }
        private void InitializeDropdownsAndSettings()
        {
            ddlNetwork.SelectedIndex = 0;
            ddlOrderType.SelectedIndex = 0;
            ddlCandleTimes.SelectedIndex = 0;
            ddlAutoOrderType.SelectedIndex = 0;

            LoadAPISettings();
        }

        private void LoadAPISettings()
        {
            switch (ddlNetwork.SelectedItem.ToString())
            {
                case "TestNet":
                    txtAPIKey.Text = Properties.Settings.Default.TestAPIKey;
                    txtAPISecret.Text = Properties.Settings.Default.TestAPISecret;
                    break;
                case "RealNet":
                    txtAPIKey.Text = Properties.Settings.Default.APIKey;
                    txtAPISecret.Text = Properties.Settings.Default.APISecret;
                    break;
            }
        }

        private void InitializeOverTime() // NEW - Just updates the summary
        {
            UpdateOverTimeSummary();
        }

        private void InitializeCandleArea()
        {
            //tmrCandleUpdater.Start();
        }

        private void InitializeAPI()
        {
            switch (ddlNetwork.SelectedItem.ToString())
            {
                case "TestNet":
                    bitmex = new BitMEXApi(txtAPIKey.Text, txtAPISecret.Text, TestbitmexDomain);
                    break;
                case "RealNet":
                    bitmex = new BitMEXApi(txtAPIKey.Text, txtAPISecret.Text, bitmexDomain);
                    break;
            }

            // We must do this in case symbols are different on test and real net
            GetAPIValidity(); // Validate API keys by checking and displaying account balance.
            InitializeSymbolInformation();

        }

        private void InitializeSymbolInformation()
        {
            ActiveInstruments = bitmex.GetActiveInstruments();
            if(ActiveInstruments!=null)
            {
                ActiveInstruments=ActiveInstruments.OrderByDescending(a => a.Volume24H).ToList();
                ddlSymbol.DataSource = ActiveInstruments;
                ddlSymbol.DisplayMember = "Symbol";
                ddlSymbol.SelectedIndex = 0;
                ActiveInstrument = ActiveInstruments[0];
            }
        }

        private double CalculateMakerOrderPrice(string Side)
        {
            CurrentBook = bitmex.GetOrderBook(ActiveInstrument.Symbol, 1);

            double SellPrice = CurrentBook.Where(a => a.Side == "Sell").FirstOrDefault().Price;
            double BuyPrice = CurrentBook.Where(a => a.Side == "Buy").FirstOrDefault().Price;

            double OrderPrice = 0;

            switch (Side)
            {
                case "Buy":
                    OrderPrice = BuyPrice;

                    if (BuyPrice + ActiveInstrument.TickSize >= SellPrice)
                    {
                        OrderPrice = BuyPrice;
                    }
                    else if (BuyPrice + ActiveInstrument.TickSize < SellPrice)
                    {
                        OrderPrice = BuyPrice + ActiveInstrument.TickSize;
                    }
                    break;
                case "Sell":
                    OrderPrice = SellPrice;

                    if (SellPrice - ActiveInstrument.TickSize <= BuyPrice)
                    {
                        OrderPrice = SellPrice;
                    }
                    else if (SellPrice - ActiveInstrument.TickSize > BuyPrice)
                    {
                        OrderPrice = SellPrice - ActiveInstrument.TickSize;
                    }
                    break;
            }
            return OrderPrice;
        }

        private void MakeOrder(string Side, int Qty, double Price = 0)
        {
            if (chkCancelWhileOrdering.Checked)
            {
                string resp = bitmex.CancelAllOpenOrders(ActiveInstrument.Symbol);
            }
            switch (ddlOrderType.SelectedItem.ToString())
            {
                case "Limit Post Only":
                    if (Price == 0)
                    {
                        Price = CalculateMakerOrderPrice(Side);
                    }
                    var MakerBuy = bitmex.PostOrderPostOnly(ActiveInstrument.Symbol, Side, Price, Qty);
                    break;
                case "Market":
                    string resp = bitmex.MarketOrder(ActiveInstrument.Symbol, Side, Qty);
                    break;
            }
        }

        private string AutoMakeOrder(string Side, int Qty, double Price = 0)
        {
            switch (ddlAutoOrderType.SelectedItem.ToString())
            {
                case "Limit Post Only":
                    if (Price == 0)
                    {
                        Price = CalculateMakerOrderPrice(Side);
                    }

                    var MakerBuy = bitmex.PostOrderPostOnly(ActiveInstrument.Symbol, Side, Price, Qty);
                    return MakerBuy;
                //break;
                case "Market":
                    return bitmex.MarketOrder(ActiveInstrument.Symbol, Side, Qty);
                    //break;
            }
            return "";
        }

        private void btnBuy_Click(object sender, EventArgs e)
        {
            MakeOrder("Buy", Convert.ToInt32(nudQty.Value));

            //AddToOrderList("Market", "Sell", "123456", "0");
            //dgvOrders.DataSource = Orders;
            //dgvOrders.Refresh();
        }


        private void btnSell_Click(object sender, EventArgs e)
        {
            MakeOrder("Sell", Convert.ToInt32(nudQty.Value));
        }

        private void btnCancelOpenOrders_Click(object sender, EventArgs e)
        {
            bitmex.CancelAllOpenOrders(ActiveInstrument.Symbol);
        }

        private void ddlNetwork_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadAPISettings();
            InitializeAPI();
        }

        private void ddlSymbol_SelectedIndexChanged(object sender, EventArgs e)
        {
            ActiveInstrument = bitmex.GetInstrument(((Instrument)ddlSymbol.SelectedItem).Symbol)[0];
        }

        private void UpdateCandles()
        {
            while (boolUpdateCandles)
            {
                if (CheckForInternetConnection())
                {
                    boolUpdatingCandles = true;
                    // Get candles
                    Candles = bitmex.GetCandleHistory(ActiveInstrument.Symbol, 500, ddlCandleTimes.SelectedItem.ToString());

                    if(Candles!=null)
                    {
                        Candles = Candles.OrderBy(a => a.TimeStamp).ToList();

                        // Set Indicator Info

                        foreach (Candle c in Candles)
                        {
                            c.PCC = Candles.Where(a => a.TimeStamp < c.TimeStamp).Count();

                            int MA1Period = Convert.ToInt32(nudMA1.Value);
                            int MA2Period = Convert.ToInt32(nudMA2.Value);

                            if (c.PCC >= MA1Period)
                            {
                                // Get the moving average over the last X periods using closing -- INCLUDES CURRENT CANDLE <=
                                c.MA1 = Candles.Where(a => a.TimeStamp <= c.TimeStamp).OrderByDescending(a => a.TimeStamp).Take(MA1Period).Average(a => a.Close);
                            } // With not enough candles, we don't set to 0, we leave it null.

                            if (c.PCC >= MA2Period)
                            {
                                // Get the moving average over the last X periods using closing -- INCLUDES CURRENT CANDLE <=
                                c.MA2 = Candles.Where(a => a.TimeStamp <= c.TimeStamp).OrderByDescending(a => a.TimeStamp).Take(MA2Period).Average(a => a.Close);
                            } // With not enough candles, we don't set to 0, we leave it null.

                            if (c.PCC >= BBLength) // Bollinger Bands
                            {
                                // BBand calculation available on trading view wiki: https://www.tradingview.com/wiki/Bollinger_Bands_(BB)
                                // You might need to also google how to calculate standard deviation as well: https://stackoverflow.com/questions/14635735/how-to-efficiently-calculate-a-moving-standard-deviation

                                // BBMiddle is just 20 period moving average
                                c.BBMiddle = Candles.Where(a => a.TimeStamp <= c.TimeStamp).OrderByDescending(a => a.TimeStamp).Take(BBLength).Average(a => a.Close);

                                // Calculating the std deviation is important, and the hard part.
                                double total_squared = 0;
                                double total_for_average = Convert.ToDouble(Candles.Where(a => a.TimeStamp <= c.TimeStamp).OrderByDescending(a => a.TimeStamp).Take(BBLength).Sum(a => a.Close));
                                foreach (Candle cb in Candles.Where(a => a.TimeStamp <= c.TimeStamp).OrderByDescending(a => a.TimeStamp).Take(BBLength).ToList())
                                {
                                    total_squared += Math.Pow(Convert.ToDouble(cb.Close), 2);
                                }
                                double stdev = Math.Sqrt((total_squared - Math.Pow(total_for_average, 2) / BBLength) / BBLength);
                                c.BBUpper = c.BBMiddle + (stdev * BBMultiplier);
                                c.BBLower = c.BBMiddle - (stdev * BBMultiplier);
                            }


                            // EMA
                            if (c.PCC >= EMA1Period)
                            {
                                double p1 = EMA1Period + 1;
                                double EMAMultiplier = Convert.ToDouble(2 / p1);

                                if (c.PCC == EMA1Period)
                                {
                                    // This is our seed EMA, using SMA of EMA1 Period for EMA 1
                                    c.EMA1 = Candles.Where(a => a.TimeStamp <= c.TimeStamp).OrderByDescending(a => a.TimeStamp).Take(EMA1Period).Average(a => a.Close);
                                }
                                else
                                {
                                    double? LastEMA = Candles.Where(a => a.TimeStamp < c.TimeStamp).OrderByDescending(a => a.TimeStamp).Take(1).FirstOrDefault().EMA1;
                                    c.EMA1 = ((c.Close - LastEMA) * EMAMultiplier) + LastEMA;
                                }
                            }

                            if (c.PCC >= EMA2Period)
                            {
                                double p1 = EMA2Period + 1;
                                double EMAMultiplier = Convert.ToDouble(2 / p1);

                                if (c.PCC == EMA2Period)
                                {
                                    // This is our seed EMA, using SMA
                                    c.EMA2 = Candles.Where(a => a.TimeStamp <= c.TimeStamp).OrderByDescending(a => a.TimeStamp).Take(EMA2Period).Average(a => a.Close);
                                }
                                else
                                {
                                    double? LastEMA = Candles.Where(a => a.TimeStamp < c.TimeStamp).OrderByDescending(a => a.TimeStamp).Take(1).FirstOrDefault().EMA2;
                                    c.EMA2 = ((c.Close - LastEMA) * EMAMultiplier) + LastEMA;
                                }
                            }

                            if (c.PCC >= EMA3Period)
                            {
                                double p1 = EMA3Period + 1;
                                double EMAMultiplier = Convert.ToDouble(2 / p1);

                                if (c.PCC == EMA3Period)
                                {
                                    // This is our seed EMA, using SMA
                                    c.EMA3 = Candles.Where(a => a.TimeStamp <= c.TimeStamp).OrderByDescending(a => a.TimeStamp).Take(EMA3Period).Average(a => a.Close);
                                }
                                else
                                {
                                    double? LastEMA = Candles.Where(a => a.TimeStamp < c.TimeStamp).OrderByDescending(a => a.TimeStamp).Take(1).FirstOrDefault().EMA3;
                                    c.EMA3 = ((c.Close - LastEMA) * EMAMultiplier) + LastEMA;
                                }
                            }

                            // MACD
                            // We can only do this if we have the longest EMA period, EMA1
                            if (c.PCC >= EMA1Period)
                            {

                                double p1 = MACDEMAPeriod + 1;
                                double MACDEMAMultiplier = Convert.ToDouble(2 / p1);

                                c.MACDLine = (c.EMA2 - c.EMA1); // default is 12EMA - 26EMA
                                if (c.PCC == EMA1Period + MACDEMAPeriod - 1)
                                {
                                    // Set this to SMA of MACDLine to seed it
                                    c.MACDSignalLine = Candles.Where(a => a.TimeStamp <= c.TimeStamp).OrderByDescending(a => a.TimeStamp).Take(MACDEMAPeriod).Average(a => (a.MACDLine));
                                }
                                else if (c.PCC > EMA1Period + MACDEMAPeriod - 1)
                                {
                                    // We can calculate this EMA based off past candle EMAs
                                    double? LastMACDSignalLine = Candles.Where(a => a.TimeStamp < c.TimeStamp).OrderByDescending(a => a.TimeStamp).Take(1).FirstOrDefault().MACDSignalLine;
                                    c.MACDSignalLine = ((c.MACDLine - LastMACDSignalLine) * MACDEMAMultiplier) + LastMACDSignalLine;
                                }
                                c.MACDHistorgram = c.MACDLine - c.MACDSignalLine;
                            }

                            // ATR, setting TR
                            if (c.PCC == 0)
                            {
                                c.SetTR(c.High);
                            }
                            else if (c.PCC > 0)
                            {
                                c.SetTR(Candles.Where(a => a.TimeStamp < c.TimeStamp).OrderByDescending(a => a.TimeStamp).Take(1).FirstOrDefault().Close);
                            }

                            // Setting ATRs
                            if (c.PCC == ATR1Period - 1)
                            {
                                c.ATR1 = Candles.Where(a => a.TimeStamp <= c.TimeStamp).OrderByDescending(a => a.TimeStamp).Take(ATR1Period).Average(a => a.TR);
                            }
                            else if (c.PCC > ATR1Period - 1)
                            {
                                double p1 = ATR1Period + 1;
                                double ATR1Multiplier = Convert.ToDouble(2 / p1);
                                double? LastATR1 = Candles.Where(a => a.TimeStamp < c.TimeStamp).OrderByDescending(a => a.TimeStamp).Take(1).FirstOrDefault().ATR1;
                                c.ATR1 = ((c.TR - LastATR1) * ATR1Multiplier) + LastATR1;
                            }

                            if (c.PCC == ATR2Period - 1)
                            {
                                c.ATR2 = Candles.Where(a => a.TimeStamp <= c.TimeStamp).OrderByDescending(a => a.TimeStamp).Take(ATR2Period).Average(a => a.TR);
                            }
                            else if (c.PCC > ATR2Period - 1)
                            {
                                double p1 = ATR2Period + 1;
                                double ATR2Multiplier = Convert.ToDouble(2 / p1);
                                double? LastATR2 = Candles.Where(a => a.TimeStamp < c.TimeStamp).OrderByDescending(a => a.TimeStamp).Take(1).FirstOrDefault().ATR2;
                                c.ATR2 = ((c.TR - LastATR2) * ATR2Multiplier) + LastATR2;
                            }

                            // For RSI
                            if (c.PCC == RSIPeriod - 1)
                            {
                                // AVG Gain is average of just gains, for all periods, (14), not just periods with gains.  Same goes for losses but with losses.
                                c.AVGGain = Candles.Where(a => a.TimeStamp <= c.TimeStamp).OrderByDescending(a => a.TimeStamp).Where(a => a.GainOrLoss > 0).Take(RSIPeriod).Sum(a => a.GainOrLoss) / RSIPeriod;
                                c.AVGLoss = (Candles.Where(a => a.TimeStamp <= c.TimeStamp).OrderByDescending(a => a.TimeStamp).Where(a => a.GainOrLoss < 0).Take(RSIPeriod).Sum(a => a.GainOrLoss) / RSIPeriod) * -1;

                                c.RS = c.AVGGain / c.AVGLoss; // Only like this on first one (seeding it)
                                c.RSI = 100 - (100 / (1 + c.RS));
                            }
                            else if (c.PCC > RSIPeriod - 1)
                            {
                                // AVG Gain is average of just gains, for all periods, (14), not just periods with gains.  Same goes for losses but with losses.
                                c.AVGGain = Candles.Where(a => a.TimeStamp <= c.TimeStamp).OrderByDescending(a => a.TimeStamp).Where(a => a.GainOrLoss > 0).Take(RSIPeriod).Sum(a => a.GainOrLoss) / RSIPeriod;
                                c.AVGLoss = (Candles.Where(a => a.TimeStamp <= c.TimeStamp).OrderByDescending(a => a.TimeStamp).Where(a => a.GainOrLoss < 0).Take(RSIPeriod).Sum(a => a.GainOrLoss) / RSIPeriod) * -1;

                                double? LastAVGGain = Candles.Where(a => a.TimeStamp < c.TimeStamp).OrderByDescending(a => a.TimeStamp).Take(1).FirstOrDefault().AVGGain;
                                double? LastAVGLoss = Candles.Where(a => a.TimeStamp < c.TimeStamp).OrderByDescending(a => a.TimeStamp).Take(1).FirstOrDefault().AVGLoss;
                                double? Gain = 0;
                                double? Loss = 0;

                                if (c.GainOrLoss > 0)
                                {
                                    Gain = c.GainOrLoss;
                                }
                                else if (c.GainOrLoss < 0)
                                {
                                    Loss = c.GainOrLoss;
                                }

                                c.RS = (((LastAVGGain * (RSIPeriod - 1)) + Gain) / RSIPeriod) / (((LastAVGLoss * (RSIPeriod - 1)) + Loss) / RSIPeriod);
                                c.RSI = 100 - (100 / (1 + c.RS));
                            }

                            // For STOCH
                            if (c.PCC >= STOCHLookbackPeriod - 1)
                            {
                                double? HighInLookback = Candles.Where(a => a.TimeStamp <= c.TimeStamp).OrderByDescending(a => a.TimeStamp).Take(STOCHLookbackPeriod).Max(a => a.High);
                                double? LowInLookback = Candles.Where(a => a.TimeStamp <= c.TimeStamp).OrderByDescending(a => a.TimeStamp).Take(STOCHLookbackPeriod).Min(a => a.Low);

                                c.STOCHK = ((c.Close - LowInLookback) / (HighInLookback - LowInLookback)) * 100;
                            }
                            if (c.PCC >= STOCHLookbackPeriod - 1 + STOCHDPeriod) // difference of -1 and 2 is 3, to allow for the 3 period SMA required for STOCH
                            {
                                c.STOCHD = Candles.Where(a => a.TimeStamp <= c.TimeStamp).OrderByDescending(a => a.TimeStamp).Take(STOCHDPeriod).Average(a => a.STOCHK);
                            }

                            // For MFI
                            if (c.PCC > 0)
                            {
                                // This line uses a function in the candle class to set the Money Flow Change by passing the previous typical price
                                c.SetMoneyFlowChange(Candles.Where(a => a.TimeStamp < c.TimeStamp).OrderByDescending(a => a.TimeStamp).Take(1).FirstOrDefault().TypicalPrice);
                            }
                            else
                            {
                                c.MoneyFlowChange = 0;
                            }

                            if (c.PCC >= MFIPeriod - 1) // We have enough candles we can actually start calculating the MFI
                            {
                                // Have to start with MoneyFlowRatio
                                // Positive flow gets the sum of all the raw money flow on days where money flow change was positive, negative flow is opposite.
                                double? PositiveFlow = Candles.Where(a => a.TimeStamp <= c.TimeStamp).OrderByDescending(a => a.TimeStamp).Take(MFIPeriod).Where(a => a.MoneyFlowChange > 0).Sum(a => a.RawMoneyFlow);
                                double? NegativeFlow = Candles.Where(a => a.TimeStamp <= c.TimeStamp).OrderByDescending(a => a.TimeStamp).Take(MFIPeriod).Where(a => a.MoneyFlowChange < 0).Sum(a => a.RawMoneyFlow);

                                c.MoneyFlowRatio = PositiveFlow / NegativeFlow;

                                c.MFI = 100 - (100 / (1 + c.MoneyFlowRatio));
                            }

                            if (c.PCC == 1) // For PVT
                            {
                                // We can set the first PVT
                                double? LastClose = Candles.Where(a => a.TimeStamp < c.TimeStamp).OrderByDescending(a => a.TimeStamp).Take(1).FirstOrDefault().Close;
                                c.PVT = ((c.Close - LastClose) / LastClose) * c.Volume;
                            }
                            else if (c.PCC > 1)
                            {
                                // We can set all other PVTs
                                double? LastClose = Candles.Where(a => a.TimeStamp < c.TimeStamp).OrderByDescending(a => a.TimeStamp).Take(1).FirstOrDefault().Close;
                                double? LastPVT = Candles.Where(a => a.TimeStamp < c.TimeStamp).OrderByDescending(a => a.TimeStamp).Take(1).FirstOrDefault().PVT;
                                c.PVT = (((c.Close - LastClose) / LastClose) * c.Volume) + LastPVT;
                            }

                            // For WMA
                            if (c.PCC >= WMAPeriod1)
                            {
                                double? Total = 0;
                                double? WMATot = 0;

                                for (int i = 0; i < WMAPeriod1; i++)
                                {
                                    // Add the closing cost of the nth item back times n + 1 (i = n)..... 
                                    Total += (Candles.Where(a => a.TimeStamp <= c.TimeStamp).OrderByDescending(a => a.TimeStamp).Take(WMAPeriod1).Skip(i).FirstOrDefault().Close * (WMAPeriod1 - i));
                                    WMATot += WMAPeriod1 - i;
                                }

                                c.WMA1 = (Total / WMATot);
                            }

                            // For WMA
                            if (c.PCC >= WMAPeriod2)
                            {
                                double? Total = 0;
                                double? WMATot = 0;

                                for (int i = 0; i < WMAPeriod2; i++)
                                {
                                    // Add the closing cost of the nth item back times n + 1 (i = n)..... we do n + 1 because it starts at 0, minimum weight is 1
                                    Total += (Candles.Where(a => a.TimeStamp <= c.TimeStamp).OrderByDescending(a => a.TimeStamp).Skip(i).FirstOrDefault().Close * (WMAPeriod2 - i));
                                    WMATot += WMAPeriod2 - i;
                                }

                                c.WMA2 = (Total / WMATot);
                            }

                            // For HMA
                            if (c.PCC >= HMAPeriod)
                            {
                                int k = (int)Math.Round(Math.Sqrt(HMAPeriod), 0); // rounds the square root of 10 to nearest integer

                                double? Total = 0;
                                double? WMATot = 0;

                                for (int i = 0; i < k; i++)
                                {
                                    Candle LoopCandle = Candles.Where(a => a.TimeStamp <= c.TimeStamp).OrderByDescending(a => a.TimeStamp).Skip(i).FirstOrDefault();
                                    Total += (((2 * LoopCandle.WMA1) - (LoopCandle.WMA2)) * (WMAPeriod2 - i));
                                    WMATot += WMAPeriod2 - i;
                                }

                                c.HMA = (Total / WMATot);
                            }

                            // For ALMA
                            if (c.PCC >= ALMAPeriod)
                            {
                                double m = Convert.ToDouble(Math.Floor(Convert.ToDecimal((ALMAOffset * (ALMAPeriod - 1)))));
                                double s = Convert.ToDouble(ALMAPeriod / ALMASigma);

                                double? Total = 0;
                                double? WMATot = 0;

                                for (int i = 0; i < ALMAPeriod; i++)
                                {
                                    Candle LoopCandle = Candles.Where(a => a.TimeStamp <= c.TimeStamp).OrderByDescending(a => a.TimeStamp).Skip(ALMAPeriod - 1 - i).FirstOrDefault();
                                    double? Weight = Math.Exp(-(Math.Pow((i - m), 2) / 2 * Math.Pow(s, 2)));
                                    double? WeightValue = Weight * LoopCandle.Close;
                                    Total += WeightValue;
                                    WMATot += Weight;
                                }

                                c.ALMA = (Total / WMATot);
                            }


                            // NEW - For VWAP
                            double? CumulativeTypicalPriceVolume = Candles.Where(a => a.TimeStamp <= c.TimeStamp).OrderByDescending(a => a.TimeStamp).Take(VWAPPeriod).Sum(a => a.TypicalPriceVolume);
                            double? CumulativeVolume = Candles.Where(a => a.TimeStamp <= c.TimeStamp).OrderByDescending(a => a.TimeStamp).Take(VWAPPeriod).Sum(a => a.Volume);

                            c.VWAP = CumulativeTypicalPriceVolume / CumulativeVolume;



                            Candles = Candles.OrderByDescending(a => a.TimeStamp).ToList();
                            boolUpdatingCandles = false;

                        }

                    }

                }
                else
                {
                    LogMessage("Internet connectivity is not available while updating candles");
                }
                Thread.Sleep(int.Parse(nudCandleRefreshTime.Value.ToString()) * 1000);
            }

            //Moving this to timer
            //// Show Candles
            //dgvCandles.DataSource = Candles;

            //// This is where we are going to determine the "mode" of the bot based on MAs, trades happen on another timer
            //if(Running)//We could set this up to also ignore setting bot mode if we've already reviewed current candles
            //                //  However, if you wanted to use info from the most current candle, that wouldn't work well
            //{
            //    SetBotMode();  // We really only need to set bot mode if the bot is running
            //    btnAutomatedTrading.Text = "Stop - " + Mode;// so we can see what the mode of the bot is while running
            //}
        }

        private void SetBotMode()
        {
            // This is where we are going to determine what mode the bot is in
            if (rdoBuy.Checked)
            {
                //if ((Candles[1].MA1 > Candles[1].MA2) && (Candles[2].MA1 <= Candles[2].MA2)) // Most recently closed candle crossed over up
                //{
                //    // Did the last full candle have MA1 cross above MA2?  We'll need to buy now.
                //    Mode = "Buy";
                //}
                //else if ((Candles[1].MA1 < Candles[1].MA2) && (Candles[2].MA1 >= Candles[2].MA2))
                //{
                //    // Did the last full candle have MA1 cross below MA2?  We'll need to close any open position.
                //    Mode = "CloseAndWait";
                //}
                //else if ((Candles[1].MA1 > Candles[1].MA2) && (Candles[2].MA1 > Candles[2].MA2))
                //{
                //    // If no crossover, is MA1 still above MA2? We'll need to leave our position open.
                //    Mode = "Wait";
                //}
                //else if ((Candles[1].MA1 < Candles[1].MA2) && (Candles[2].MA1 < Candles[2].MA2))
                //{
                //    // If no crossover, is MA1 still below MA2? We'll need to make sure we don't have a position open.
                //    Mode = "CloseAndWait";
                //}

                // MACD Example
                if ((Candles[1].MACDLine > Candles[1].MACDSignalLine) && (Candles[2].MACDLine <= Candles[2].MACDSignalLine)) // Most recently closed candle crossed over up
                {
                    // Did the last full candle have MACDLine cross above MACDSignalLine?  We'll need to buy now.
                    Mode = "Buy";
                    LogMessage("the last full candle have MACDLine cross above MACDSignalLine?  We'll need to buy now.");
                }
                else if ((Candles[1].MACDLine < Candles[1].MACDSignalLine) && (Candles[2].MACDLine >= Candles[2].MACDSignalLine))
                {
                    // Did the last full candle have MACDLine cross below MACDSignalLine?  We'll need to close any open position.
                    Mode = "CloseAndWait";
                    LogMessage("the last full candle have MACDLine cross below MACDSignalLine?  We'll need to close any open position.");
                }
                else if ((Candles[1].MACDLine > Candles[1].MACDSignalLine) && (Candles[2].MACDLine > Candles[2].MACDSignalLine))
                {
                    // If no crossover, is MACDLine still above MACDSignalLine? We'll need to leave our position open.
                    Mode = "Wait";
                    LogMessage("no crossover,  MACDLine still above MACDSignalLine, We'll need to leave our position open.");
                }
                else if ((Candles[1].MACDLine < Candles[1].MACDSignalLine) && (Candles[2].MACDLine < Candles[2].MACDSignalLine))
                {
                    // If no crossover, is MACDLine still below MACDSignalLine? We'll need to make sure we don't have a position open.
                    Mode = "CloseAndWait";
                    LogMessage("no crossover,  MACDLine still below MACDSignalLine, We'll need to make sure we don't have a position open.");
                }

            }
            else if (rdoSell.Checked)
            {
                if ((Candles[1].MA1 > Candles[1].MA2) && (Candles[2].MA1 <= Candles[2].MA2)) // Most recently closed candle crossed over up
                {
                    // Did the last full candle have MA1 cross above MA2?  We'll need to close any open position.
                    Mode = "CloseAndWait";
                    LogMessage("the last full candle have -MA1 cross above MA2,  We'll need to close any open position.");
                }
                else if ((Candles[1].MA1 < Candles[1].MA2) && (Candles[2].MA1 >= Candles[2].MA2))
                {
                    // Did the last full candle have MA1 cross below MA2?  We'll need to sell now.
                    Mode = "Sell";
                    LogMessage("the last full candle have -MA1 cross below MA2,  We'll need to sell now.");
                }
                else if ((Candles[1].MA1 > Candles[1].MA2) && (Candles[2].MA1 > Candles[2].MA2))
                {
                    // If no crossover, is MA1 still above MA2? We'll need to make sure we don't have a position open.
                    Mode = "CloseAndWait";
                    LogMessage("no crossover,  MA1 still above -MA2, We'll need to make sure we don't have a position open.");
                }
                else if ((Candles[1].MA1 < Candles[1].MA2) && (Candles[2].MA1 < Candles[2].MA2))
                {
                    // If no crossover, is MA1 still below MA2? We'll need to leave our position open.
                    Mode = "Wait";
                    LogMessage("no crossover,  MA1 still below -MA2, We'll need to leave our position open.");
                }
            }
            else if (rdoSwitch.Checked)
            {
                //NEW                        
                if ((Convert.ToDouble(Candles[0].Close) > Candles[1].EMA2))// && (Candles[2].EMA1 <= Candles[2].EMA2)) // Most recently closed candle crossed over up
                {
                    // Did the last full candle have MA1 cross above MA2?  Triggers a buy in switch setting.
                    Mode = "Buy";
                    LogMessage("the last close has crossed above EMA of " + EMA2Period + ",  Triggers a buy in switch setting." + Convert.ToString(CurrentPrice));
                }
                else if ((Convert.ToDouble(Candles[0].Close) < Candles[1].EMA2))// && (Candles[2].EMA1 >= Candles[2].EMA2))
                {
                    // Did the last full candle have MA1 cross below MA2?  Triggers a sell in switch setting
                    Mode = "Sell";
                    LogMessage("the last close has crossed below EMA of " + EMA2Period + ",  Triggers a sell in switch setting" + Convert.ToString(CurrentPrice));
                }
                else if ((Convert.ToDouble(Candles[0].Close) > Candles[1].EMA2) && (Candles[2].EMA1 > Candles[2].EMA2))
                {
                    // If no crossover, is MA1 still above MA2? Keep long position open, close any shorts if they are still open.
                    //Mode = "CloseShortsAndWait";
                    //LogMessage("no crossover, close still above EMA of " + EMA2Period + ", Keep long position open, close any shorts if they are still open." + Convert.ToString(CurrentPrice));
                }
                else if ((Convert.ToDouble(Candles[0].Close) < Candles[1].EMA2) && (Candles[2].EMA1 < Candles[2].EMA2))
                {
                    // If no crossover, is MA1 still below MA2? Keep short position open, close any longs if they are still open.
                    //Mode = "CloseLongsAndWait";
                    //LogMessage("no crossover, close still below EMA of " + EMA2Period + ", Keep short position open, close any longs if they are still open." + Convert.ToString(CurrentPrice));
                }
            }
        }

        private void tmrCandleUpdater_Tick(object sender, EventArgs e)
        {

            // Show Candles
            if (!boolUpdatingCandles)
            {
                dgvCandles.DataSource = Candles;

                // This is where we are going to determine the "mode" of the bot based on MAs, trades happen on another timer
                if (Running)//We could set this up to also ignore setting bot mode if we've already reviewed current candles
                            //  However, if you wanted to use info from the most current candle, that wouldn't work well
                {
                    SetBotMode();  // We really only need to set bot mode if the bot is running
                    btnAutomatedTrading.Text = "Stop - " + Mode;// so we can see what the mode of the bot is while running
                }
            }
            dgvOrders.DataSource = Orders;
            //if (chkUpdateCandles.Checked)
            //{
            //    boolUpdateCandles = true;
            //    UpdateCandles();
            //}
            //else
            //{

            //}

        }

        private void chkUpdateCandles_CheckedChanged(object sender, EventArgs e)
        {
            if (chkUpdateCandles.Checked)
            {

                boolUpdateCandles = true;

                Thread threadCandleUpdate = new Thread(() => UpdateCandles());
                threadCandleUpdate.IsBackground = true;
                threadCandleUpdate.Start();
                tmrCandleUpdater.Interval = int.Parse(nudCandleRefreshTime.Value.ToString()) * 1000;
                tmrCandleUpdater.Start();
            }
            else
            {
                boolUpdateCandles = false;
                tmrCandleUpdater.Stop();
            }
        }

        private void ddlCandleTimes_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateCandles();
        }

        private void btnAutomatedTrading_Click(object sender, EventArgs e)
        {
            if (btnAutomatedTrading.Text == "Start")
            {


                //tmrAutoTradeExecution.Start();
                btnAutomatedTrading.Text = "Stop - " + Mode;
                btnAutomatedTrading.BackColor = Color.Red;
                Running = true;
                rdoBuy.Enabled = false;
                rdoSell.Enabled = false;
                rdoSwitch.Enabled = false;
                //retryAttempts = int.Parse(nudRetryAttempts.Value.ToString());

                Thread autoTrading = new Thread(() => AutoTrade());
                autoTrading.IsBackground = true;
                autoTrading.Start();

                Thread checkOrderStatus = new Thread(() => RefreshOrderStatus());
                checkOrderStatus.IsBackground = true;
                checkOrderStatus.Start();

            }
            else
            {
                //tmrAutoTradeExecution.Stop();
                btnAutomatedTrading.Text = "Start";
                btnAutomatedTrading.BackColor = Color.LightGreen;
                Running = false;
                rdoBuy.Enabled = true;
                rdoSell.Enabled = true;
                rdoSwitch.Enabled = true;
            }

        }

        private void AddToOrderList(string OrderType, string OrderSide, string qty, string price, string OrderID, string OrderStatus = "Pending")
        {
            //pOrder = new PlacedOrders();
            //pOrder.OrderID = (pOrders.Count+1).ToString();
            //pOrder.PlacingTime = DateTime.Now.ToString();
            //pOrder.OrderType = OrderType;
            //pOrder.OrderSide = OrderSide;
            //pOrder.OrderQuantity = qty;
            //pOrder.OrderPrice = price;
            //pOrder.BotStatus = Mode;
            //pOrders.Add(pOrder);

            DataRow dr = Orders.NewRow();
            dr[0] = OrderID.ToString();
            dr[1] = DateTime.Now.ToString();
            dr[2] = OrderType;
            dr[3] = OrderSide;
            dr[4] = qty.ToString(); ;
            dr[5] = price.ToString(); ;
            dr[6] = OrderStatus;
            dr[7] = Mode;
            Orders.Rows.Add(dr);
        }
        private void tmrAutoTradeExecution_Tick(object sender, EventArgs e)
        {


            //string resp;
            //int RetryCount = int.Parse(nudRetryAttempts.Value.ToString());
            //if (CheckForInternetConnection())
            //{
            //    OpenPositions = bitmex.GetOpenPositions(ActiveInstrument.Symbol);
            //    OpenOrders = bitmex.GetOpenOrders(ActiveInstrument.Symbol);



            //    if (OpenOrders != null && OpenPositions != null)
            //    {
            //        #region "Close positions"
            //        try
            //        {
            //            if (chkAutoMarketTakeProfits.Checked && OpenPositions.Any() && Mode != "Sell" && Mode != "Buy") // See if we are taking profits on open positions, and have positions open and we aren't in our buy or sell periods
            //            {
            //                lblAutoUnrealizedROEPercent.Text = Math.Round((Convert.ToDouble(OpenPositions[0].UnrealisedRoePcnt * 100)), 2).ToString();
            //                // Did we meet our profit threshold yet?
            //                if (Convert.ToDouble(OpenPositions[0].UnrealisedRoePcnt * 100) >= Convert.ToDouble(nudAutoMarketTakeProfitPercent.Value))
            //                {
            //                    // Make a market order to close out the position, also cancel all orders so nothing else fills if we had unfilled limit orders still open.
            //                    string Side = "Sell";
            //                    int Quantity = 0;
            //                    if (OpenPositions[0].CurrentQty > 0)
            //                    {
            //                        Side = "Sell";
            //                        Quantity = Convert.ToInt32(OpenPositions[0].CurrentQty);
            //                    }
            //                    else if (OpenPositions[0].CurrentQty < 0)
            //                    {
            //                        Side = "Buy";
            //                        Quantity = Convert.ToInt32(OpenPositions[0].CurrentQty) * -1;
            //                    }
            //                    #region "Place buy order"
            //                    resp = bitmex.MarketOrder(ActiveInstrument.Symbol, Side, Quantity);
            //                    if (resp.ToLower().Contains("error"))
            //                    {
            //                        ErrorObject err = jsonSerializer.Deserialize<ErrorObject>(resp);
            //                        LogMessage("Error " + err.error.name + ":" + err.error.message);
            //                    }
            //                    else
            //                    {
            //                        LogMessage("Successfully placed " + Side + " order , quantity " + Quantity);
            //                        AddToOrderList("Market", Side, Quantity.ToString(), "0", or.orderID, or.ordStatus);
            //                    }
            //                    #endregion

            //                    // Get our positions and orders again to be able to process rest of logic with new information.
            //                    OpenPositions = bitmex.GetOpenPositions(ActiveInstrument.Symbol);
            //                    OpenOrders = bitmex.GetOpenOrders(ActiveInstrument.Symbol);
            //                }
            //            }
            //        }
            //        catch (Exception ex)
            //        {

            //            LogMessage("Exception in position close : " + ex.Message);
            //        }

            //        #endregion


            //        try
            //        {
            //            #region "rdoBuy"
            //            if (rdoBuy.Checked)
            //            {
            //                switch (Mode)
            //                {
            //                    case "Buy":
            //                        // See if we already have a position open
            //                        if (OpenPositions.Any())
            //                        {
            //                            // We have an open position, is it at our desired quantity?
            //                            if (OpenPositions[0].CurrentQty < nudAutoQuantity.Value)
            //                            {
            //                                // If we have an open order, edit it
            //                                if (OpenOrders.Any(a => a.Side == "Sell"))
            //                                {
            //                                    // We still have an open sell order, cancel that order, make a new buy order
            //                                    #region "Cancel all orders"
            //                                    resp = bitmex.CancelAllOpenOrders(ActiveInstrument.Symbol);
            //                                    if (resp.ToLower().Contains("error"))
            //                                    {
            //                                        ErrorObject err = jsonSerializer.Deserialize<ErrorObject>(resp);
            //                                        LogMessage("Error " + err.error.name + ":" + err.error.message);
            //                                    }
            //                                    else
            //                                    {
            //                                        LogMessage("Successfully cancelled all open orders");
            //                                    }
            //                                    //string result = bitmex.CancelAllOpenOrders(ActiveInstrument.Symbol);
            //                                    #endregion


            //                                    #region "Place buy order"
            //                                    resp = AutoMakeOrder("Buy", Convert.ToInt32(OpenPositions[0].CurrentQty));
            //                                    if (resp.ToLower().Contains("error"))
            //                                    {
            //                                        ErrorObject err = jsonSerializer.Deserialize<ErrorObject>(resp);
            //                                        LogMessage("Error " + err.error.name + ":" + err.error.message);
            //                                    }
            //                                    else
            //                                    {
            //                                        LogMessage("Successfully placed buy order");
            //                                        AddToOrderList("", "Buy", Convert.ToInt32(OpenPositions[0].CurrentQty).ToString(), "0");

            //                                    }
            //                                    //AutoMakeOrder("Buy", Convert.ToInt32(OpenPositions[0].CurrentQty));
            //                                    #endregion
            //                                }
            //                                else if (OpenOrders.Any(a => a.Side == "Buy"))
            //                                {
            //                                    // Edit our only open order, code should not allow for more than 1 at a time for now
            //                                    #region "Modify order"
            //                                    resp = bitmex.EditOrderPrice(OpenOrders[0].OrderId, CalculateMakerOrderPrice("Buy"));
            //                                    if (resp.ToLower().Contains("error"))
            //                                    {
            //                                        ErrorObject err = jsonSerializer.Deserialize<ErrorObject>(resp);
            //                                        LogMessage("Error " + err.error.name + ":" + err.error.message);
            //                                    }
            //                                    else
            //                                    {
            //                                        LogMessage("Successfully modified buy order price");
            //                                    }
            //                                    //string result = bitmex.EditOrderPrice(OpenOrders[0].OrderId, CalculateMakerOrderPrice("Buy"));
            //                                    #endregion
            //                                }

            //                            } // No else, it is filled to where we want.
            //                        }
            //                        else
            //                        {
            //                            if (OpenOrders.Any())
            //                            {
            //                                // If we have an open order, edit it
            //                                if (OpenOrders.Any(a => a.Side == "Sell"))
            //                                {
            //                                    // We still have an open sell order, cancel that order, make a new buy order
            //                                    #region "Cancel Orders"
            //                                    resp = bitmex.CancelAllOpenOrders(ActiveInstrument.Symbol);
            //                                    if (resp.ToLower().Contains("error"))
            //                                    {
            //                                        ErrorObject err = jsonSerializer.Deserialize<ErrorObject>(resp);
            //                                        LogMessage("Error " + err.error.name + ":" + err.error.message);
            //                                    }
            //                                    else
            //                                    {
            //                                        LogMessage("Successfully cancelled all open orders");
            //                                    }
            //                                    //string result = bitmex.CancelAllOpenOrders(ActiveInstrument.Symbol);

            //                                    #endregion

            //                                    #region "Place buy order"
            //                                    resp = AutoMakeOrder("Buy", Convert.ToInt32(OpenPositions[0].CurrentQty));
            //                                    if (resp.ToLower().Contains("error"))
            //                                    {
            //                                        ErrorObject err = jsonSerializer.Deserialize<ErrorObject>(resp);
            //                                        LogMessage("Error " + err.error.name + ":" + err.error.message);
            //                                    }
            //                                    else
            //                                    {
            //                                        LogMessage("Successfully placed buy order");
            //                                        AddToOrderList("", "Buy", Convert.ToInt32(OpenPositions[0].CurrentQty).ToString(), "0");

            //                                    }
            //                                    //AutoMakeOrder("Buy", Convert.ToInt32(OpenPositions[0].CurrentQty));

            //                                    #endregion
            //                                }
            //                                else if (OpenOrders.Any(a => a.Side == "Buy"))
            //                                {
            //                                    // Edit our only open order, code should not allow for more than 1 at a time for now
            //                                    #region "Modify orders"
            //                                    resp = bitmex.EditOrderPrice(OpenOrders[0].OrderId, CalculateMakerOrderPrice("Buy"));
            //                                    if (resp.ToLower().Contains("error"))
            //                                    {
            //                                        ErrorObject err = jsonSerializer.Deserialize<ErrorObject>(resp);
            //                                        LogMessage("Error " + err.error.name + ":" + err.error.message);
            //                                    }
            //                                    else
            //                                    {
            //                                        LogMessage("Successfully modified buy order price");
            //                                    }
            //                                    //string result = bitmex.EditOrderPrice(OpenOrders[0].OrderId, CalculateMakerOrderPrice("Buy"));

            //                                    #endregion
            //                                }
            //                            }
            //                            else
            //                            {
            //                                #region "Place buy order"

            //                                resp = AutoMakeOrder("Buy", Convert.ToInt32(nudAutoQuantity.Value));
            //                                if (resp.ToLower().Contains("error"))
            //                                {
            //                                    ErrorObject err = jsonSerializer.Deserialize<ErrorObject>(resp);
            //                                    LogMessage("Error " + err.error.name + ":" + err.error.message);
            //                                }
            //                                else
            //                                {
            //                                    LogMessage("Successfully placed buy order");
            //                                    AddToOrderList("", "Buy", Convert.ToInt32(nudAutoQuantity.Value).ToString(), "0");

            //                                }

            //                                //AutoMakeOrder("Buy", Convert.ToInt32(nudAutoQuantity.Value));

            //                                #endregion
            //                            }
            //                        }
            //                        break;
            //                    case "CloseAndWait":
            //                        // See if we have open positions, if so, close them
            //                        if (OpenPositions.Any())
            //                        {
            //                            // Now, do we have open orders?  If so, we want to make sure they are at the right price
            //                            if (OpenOrders.Any())
            //                            {
            //                                if (OpenOrders.Any(a => a.Side == "Buy"))
            //                                {
            //                                    // We still have an open buy order, cancel that order, make a new sell order
            //                                    #region "Cancel all orders"
            //                                    resp = bitmex.CancelAllOpenOrders(ActiveInstrument.Symbol);
            //                                    if (resp.ToLower().Contains("error"))
            //                                    {
            //                                        ErrorObject err = jsonSerializer.Deserialize<ErrorObject>(resp);
            //                                        LogMessage("Error " + err.error.name + ":" + err.error.message);
            //                                    }
            //                                    else
            //                                    {
            //                                        LogMessage("Successfully cancelled all open orders");
            //                                    }
            //                                    //string result = bitmex.CancelAllOpenOrders(ActiveInstrument.Symbol);
            //                                    #endregion


            //                                    #region "Place sell order"
            //                                    resp = AutoMakeOrder("Sell", Convert.ToInt32(OpenPositions[0].CurrentQty));
            //                                    if (resp.ToLower().Contains("error"))
            //                                    {
            //                                        ErrorObject err = jsonSerializer.Deserialize<ErrorObject>(resp);
            //                                        LogMessage("Error " + err.error.name + ":" + err.error.message);
            //                                    }
            //                                    else
            //                                    {
            //                                        LogMessage("Successfully placed sell order");
            //                                        AddToOrderList("", "Sell", Convert.ToInt32(OpenPositions[0].CurrentQty).ToString(), "0");

            //                                    }
            //                                    #endregion
            //                                    //string result = bitmex.CancelAllOpenOrders(ActiveInstrument.Symbol);
            //                                    //AutoMakeOrder("Sell", Convert.ToInt32(OpenPositions[0].CurrentQty));
            //                                }
            //                                else if (OpenOrders.Any(a => a.Side == "Sell"))
            //                                {
            //                                    // Edit our only open order, code should not allow for more than 1 at a time for now
            //                                    #region "modify sell order"
            //                                    resp = bitmex.EditOrderPrice(OpenOrders[0].OrderId, CalculateMakerOrderPrice("Sell"));
            //                                    if (resp.ToLower().Contains("error"))
            //                                    {
            //                                        ErrorObject err = jsonSerializer.Deserialize<ErrorObject>(resp);
            //                                        LogMessage("Error " + err.error.name + ":" + err.error.message);
            //                                    }
            //                                    else
            //                                    {
            //                                        LogMessage("Successfully modified sell order");
            //                                    }
            //                                    //string result = bitmex.EditOrderPrice(OpenOrders[0].OrderId, CalculateMakerOrderPrice("Sell"));

            //                                    #endregion
            //                                }

            //                            }
            //                            else
            //                            {
            //                                // No open orders, need to make an order to sell
            //                                #region "Place sell order"
            //                                resp = AutoMakeOrder("Sell", Convert.ToInt32(OpenPositions[0].CurrentQty));
            //                                if (resp.ToLower().Contains("error"))
            //                                {
            //                                    ErrorObject err = jsonSerializer.Deserialize<ErrorObject>(resp);
            //                                    LogMessage("Error " + err.error.name + ":" + err.error.message);
            //                                }
            //                                else
            //                                {
            //                                    LogMessage("Successfully placed sell order");
            //                                    AddToOrderList("", "Sell", Convert.ToInt32(OpenPositions[0].CurrentQty).ToString(), "0");

            //                                }
            //                                //AutoMakeOrder("Sell", Convert.ToInt32(OpenPositions[0].CurrentQty));
            //                                #endregion
            //                            }
            //                        }
            //                        else if (OpenOrders.Any())
            //                        {
            //                            // We don't have an open position, but we do have an open order, close that order, we don't want to open any position here.
            //                            #region "Cancel all orders"
            //                            resp = bitmex.CancelAllOpenOrders(ActiveInstrument.Symbol);
            //                            if (resp.ToLower().Contains("error"))
            //                            {
            //                                ErrorObject err = jsonSerializer.Deserialize<ErrorObject>(resp);
            //                                LogMessage("Error " + err.error.name + ":" + err.error.message);
            //                            }
            //                            else
            //                            {
            //                                LogMessage("Successfully cancelled all open orders");
            //                            }
            //                            //string result = bitmex.CancelAllOpenOrders(ActiveInstrument.Symbol);
            //                            #endregion
            //                            //string result = bitmex.CancelAllOpenOrders(ActiveInstrument.Symbol);
            //                        }
            //                        break;
            //                    case "Wait":
            //                        // We are in wait mode, no new buying or selling - close open orders
            //                        if (OpenOrders.Any())
            //                        {
            //                            #region "Cancel all orders"
            //                            resp = bitmex.CancelAllOpenOrders(ActiveInstrument.Symbol);
            //                            if (resp.ToLower().Contains("error"))
            //                            {
            //                                ErrorObject err = jsonSerializer.Deserialize<ErrorObject>(resp);
            //                                LogMessage("Error " + err.error.name + ":" + err.error.message);
            //                            }
            //                            else
            //                            {
            //                                LogMessage("Successfully cancelled all open orders");
            //                            }
            //                            //string result = bitmex.CancelAllOpenOrders(ActiveInstrument.Symbol);
            //                            #endregion
            //                            //string result = bitmex.CancelAllOpenOrders(ActiveInstrument.Symbol);
            //                        }
            //                        break;
            //                }
            //            }
            //            #endregion

            //            #region "rdoSell"
            //            else if (rdoSell.Checked)
            //            {
            //                switch (Mode)
            //                {
            //                    case "Sell":
            //                        // See if we already have a position open
            //                        if (OpenPositions.Any())
            //                        {
            //                            // We have an open position, is it at our desired quantity?
            //                            if (OpenPositions[0].CurrentQty < nudAutoQuantity.Value)
            //                            {
            //                                // If we have an open order, edit it
            //                                if (OpenOrders.Any(a => a.Side == "Buy"))
            //                                {
            //                                    // We still have an open Buy order, cancel that order, make a new Sell order
            //                                    #region "Cancel all orders"
            //                                    resp = bitmex.CancelAllOpenOrders(ActiveInstrument.Symbol);
            //                                    if (resp.ToLower().Contains("error"))
            //                                    {
            //                                        ErrorObject err = jsonSerializer.Deserialize<ErrorObject>(resp);
            //                                        LogMessage("Error " + err.error.name + ":" + err.error.message);
            //                                    }
            //                                    else
            //                                    {
            //                                        LogMessage("Successfully cancelled all orders");
            //                                    }
            //                                    #endregion

            //                                    #region "Place sell order"
            //                                    resp = AutoMakeOrder("Sell", Convert.ToInt32(OpenPositions[0].CurrentQty));
            //                                    if (resp.ToLower().Contains("error"))
            //                                    {
            //                                        ErrorObject err = jsonSerializer.Deserialize<ErrorObject>(resp);
            //                                        LogMessage("Error " + err.error.name + ":" + err.error.message);
            //                                    }
            //                                    else
            //                                    {
            //                                        LogMessage("Successfully placed sell order");
            //                                        AddToOrderList("", "Sell", Convert.ToInt32(OpenPositions[0].CurrentQty).ToString(), "0");

            //                                    }
            //                                    #endregion
            //                                }
            //                                else if (OpenOrders.Any(a => a.Side == "Sell"))
            //                                {
            //                                    // Edit our only open order, code should not allow for more than 1 at a time for now
            //                                    #region "Modify order"
            //                                    resp = bitmex.EditOrderPrice(OpenOrders[0].OrderId, CalculateMakerOrderPrice("Sell"));
            //                                    if (resp.ToLower().Contains("error"))
            //                                    {
            //                                        ErrorObject err = jsonSerializer.Deserialize<ErrorObject>(resp);
            //                                        LogMessage("Error " + err.error.name + ":" + err.error.message);
            //                                    }
            //                                    else
            //                                    {
            //                                        LogMessage("Successfully modified order");
            //                                    }
            //                                    #endregion
            //                                }

            //                            } // No else, it is filled to where we want.
            //                        }
            //                        else
            //                        {
            //                            if (OpenOrders.Any())
            //                            {
            //                                // If we have an open order, edit it
            //                                if (OpenOrders.Any(a => a.Side == "Buy"))
            //                                {
            //                                    // We still have an open buy order, cancel that order, make a new sell order
            //                                    #region "Cancel all orders"
            //                                    resp = bitmex.CancelAllOpenOrders(ActiveInstrument.Symbol);
            //                                    if (resp.ToLower().Contains("error"))
            //                                    {
            //                                        ErrorObject err = jsonSerializer.Deserialize<ErrorObject>(resp);
            //                                        LogMessage("Error " + err.error.name + ":" + err.error.message);
            //                                    }
            //                                    else
            //                                    {
            //                                        LogMessage("Successfully cancelled all orders");
            //                                    }
            //                                    #endregion

            //                                    #region "Place sell order"
            //                                    resp = AutoMakeOrder("Sell", Convert.ToInt32(OpenPositions[0].CurrentQty));
            //                                    if (resp.ToLower().Contains("error"))
            //                                    {
            //                                        ErrorObject err = jsonSerializer.Deserialize<ErrorObject>(resp);
            //                                        LogMessage("Error " + err.error.name + ":" + err.error.message);
            //                                    }
            //                                    else
            //                                    {
            //                                        LogMessage("Successfully placed sell order");
            //                                        AddToOrderList("", "Sell", Convert.ToInt32(OpenPositions[0].CurrentQty).ToString(), "0");

            //                                    }
            //                                    #endregion
            //                                }
            //                                else if (OpenOrders.Any(a => a.Side == "Sell"))
            //                                {
            //                                    // Edit our only open order, code should not allow for more than 1 at a time for now
            //                                    #region "Modify order"
            //                                    resp = bitmex.EditOrderPrice(OpenOrders[0].OrderId, CalculateMakerOrderPrice("Sell"));
            //                                    if (resp.ToLower().Contains("error"))
            //                                    {
            //                                        ErrorObject err = jsonSerializer.Deserialize<ErrorObject>(resp);
            //                                        LogMessage("Error " + err.error.name + ":" + err.error.message);
            //                                    }
            //                                    else
            //                                    {
            //                                        LogMessage("Successfully modified order");
            //                                    }
            //                                    #endregion
            //                                }
            //                            }
            //                            else
            //                            {
            //                                #region "Place sell order"
            //                                resp = AutoMakeOrder("Sell", Convert.ToInt32(nudAutoQuantity.Value));
            //                                if (resp.ToLower().Contains("error"))
            //                                {
            //                                    ErrorObject err = jsonSerializer.Deserialize<ErrorObject>(resp);
            //                                    LogMessage("Error " + err.error.name + ":" + err.error.message);
            //                                }
            //                                else
            //                                {
            //                                    LogMessage("Successfully placed sell order");
            //                                    AddToOrderList("", "Sell", Convert.ToInt32(nudAutoQuantity.Value).ToString(), "0");

            //                                }
            //                                #endregion
            //                            }
            //                        }
            //                        break;
            //                    case "CloseAndWait":
            //                        // See if we have open positions, if so, close them
            //                        if (OpenPositions.Any())
            //                        {
            //                            // Now, do we have open orders?  If so, we want to make sure they are at the right price
            //                            if (OpenOrders.Any())
            //                            {
            //                                if (OpenOrders.Any(a => a.Side == "Sell"))
            //                                {
            //                                    // We still have an open sell order, cancel that order, make a new buy order
            //                                    #region "Cancel all orders"
            //                                    resp = bitmex.CancelAllOpenOrders(ActiveInstrument.Symbol);
            //                                    if (resp.ToLower().Contains("error"))
            //                                    {
            //                                        ErrorObject err = jsonSerializer.Deserialize<ErrorObject>(resp);
            //                                        LogMessage("Error " + err.error.name + ":" + err.error.message);
            //                                    }
            //                                    else
            //                                    {
            //                                        LogMessage("Successfully cancelled all orders");
            //                                    }
            //                                    #endregion

            //                                    #region "Place buy order"
            //                                    resp = AutoMakeOrder("Buy", Convert.ToInt32(OpenPositions[0].CurrentQty));
            //                                    if (resp.ToLower().Contains("error"))
            //                                    {
            //                                        ErrorObject err = jsonSerializer.Deserialize<ErrorObject>(resp);
            //                                        LogMessage("Error " + err.error.name + ":" + err.error.message);
            //                                    }
            //                                    else
            //                                    {
            //                                        LogMessage("Successfully placed buy order");
            //                                        AddToOrderList("", "Buy", Convert.ToInt32(OpenPositions[0].CurrentQty).ToString(), "0");

            //                                    }
            //                                    #endregion
            //                                }
            //                                else if (OpenOrders.Any(a => a.Side == "Buy"))
            //                                {
            //                                    // Edit our only open order, code should not allow for more than 1 at a time for now
            //                                    #region "Modify Order"
            //                                    resp = bitmex.EditOrderPrice(OpenOrders[0].OrderId, CalculateMakerOrderPrice("Buy"));
            //                                    if (resp.ToLower().Contains("error"))
            //                                    {
            //                                        ErrorObject err = jsonSerializer.Deserialize<ErrorObject>(resp);
            //                                        LogMessage("Error " + err.error.name + ":" + err.error.message);
            //                                    }
            //                                    else
            //                                    {
            //                                        LogMessage("Successfully modified buy order");
            //                                        //AddToOrderList("", "Buy", Convert.ToInt32(OpenPositions[0].CurrentQty).ToString(), "0");

            //                                    }
            //                                    #endregion
            //                                }

            //                            }
            //                            else
            //                            {
            //                                // No open orders, need to make an order to sell
            //                                #region "Place buy Order"
            //                                resp = AutoMakeOrder("Buy", Convert.ToInt32(OpenPositions[0].CurrentQty));
            //                                if (resp.ToLower().Contains("error"))
            //                                {
            //                                    ErrorObject err = jsonSerializer.Deserialize<ErrorObject>(resp);
            //                                    LogMessage("Error " + err.error.name + ":" + err.error.message);
            //                                }
            //                                else
            //                                {
            //                                    LogMessage("Successfully placed buy order");
            //                                    AddToOrderList("", "Buy", Convert.ToInt32(OpenPositions[0].CurrentQty).ToString(), "0");

            //                                }
            //                                #endregion
            //                            }
            //                        }
            //                        else if (OpenOrders.Any())
            //                        {
            //                            // We don't have an open position, but we do have an open order, close that order, we don't want to open any position here.
            //                            #region "Cancel all orders"
            //                            resp = bitmex.CancelAllOpenOrders(ActiveInstrument.Symbol);
            //                            if (resp.ToLower().Contains("error"))
            //                            {
            //                                ErrorObject err = jsonSerializer.Deserialize<ErrorObject>(resp);
            //                                LogMessage("Error " + err.error.name + ":" + err.error.message);
            //                            }
            //                            else
            //                            {
            //                                LogMessage("Successfully cancelled all orders");
            //                            }
            //                            #endregion
            //                        }
            //                        break;
            //                    case "Wait":
            //                        // We are in wait mode, no new buying or selling - close open orders
            //                        if (OpenOrders.Any())
            //                        {
            //                            #region "Cancel all orders"
            //                            resp = bitmex.CancelAllOpenOrders(ActiveInstrument.Symbol);
            //                            if (resp.ToLower().Contains("error"))
            //                            {
            //                                ErrorObject err = jsonSerializer.Deserialize<ErrorObject>(resp);
            //                                LogMessage("Error " + err.error.name + ":" + err.error.message);
            //                            }
            //                            else
            //                            {
            //                                LogMessage("Successfully cancelled all orders");
            //                            }
            //                            #endregion
            //                        }
            //                        break;
            //                }
            //            }
            //            #endregion

            //            #region "rdoSwitch"
            //            else if (rdoSwitch.Checked)
            //            {
            //                switch (Mode)
            //                {
            //                    case "Buy":
            //                        if (OpenPositions.Any())
            //                        {
            //                            int PositionDifference = Convert.ToInt32(nudAutoQuantity.Value - OpenPositions[0].CurrentQty);

            //                            if (OpenOrders.Any())
            //                            {
            //                                // If we have an open order, edit it
            //                                if (OpenOrders.Any(a => a.Side == "Sell"))
            //                                {
            //                                    // We still have an open Sell order, cancel that order, make a new Buy order
            //                                    #region "Cancel All Orders"
            //                                    resp = bitmex.CancelAllOpenOrders(ActiveInstrument.Symbol);
            //                                    if (resp.ToLower().Contains("error"))
            //                                    {
            //                                        ErrorObject err = jsonSerializer.Deserialize<ErrorObject>(resp);
            //                                        LogMessage("Error " + err.error.name + ":" + err.error.message);
            //                                    }
            //                                    else
            //                                    {
            //                                        LogMessage("Successfully cancelled all orders");
            //                                    }
            //                                    #endregion


            //                                    #region "Place Buy order"
            //                                    resp = AutoMakeOrder("Buy", PositionDifference);
            //                                    if (resp.ToLower().Contains("error"))
            //                                    {
            //                                        ErrorObject err = jsonSerializer.Deserialize<ErrorObject>(resp);
            //                                        LogMessage("Error " + err.error.name + ":" + err.error.message);
            //                                    }
            //                                    else
            //                                    {
            //                                        LogMessage("Successfully placed buy order");
            //                                        AddToOrderList("", "Buy", PositionDifference.ToString(), "0");

            //                                    }
            //                                    #endregion
            //                                }
            //                                else if (OpenOrders.Any(a => a.Side == "Buy"))
            //                                {
            //                                    // Edit our only open order, code should not allow for more than 1 at a time for now
            //                                    #region "Modify order"
            //                                    resp = bitmex.EditOrderPrice(OpenOrders[0].OrderId, CalculateMakerOrderPrice("Buy"));
            //                                    if (resp.ToLower().Contains("error"))
            //                                    {
            //                                        ErrorObject err = jsonSerializer.Deserialize<ErrorObject>(resp);
            //                                        LogMessage("Error " + err.error.name + ":" + err.error.message);
            //                                    }
            //                                    else
            //                                    {
            //                                        LogMessage("Successfully Modified order");
            //                                    }
            //                                    #endregion
            //                                }
            //                            }
            //                            else
            //                            {
            //                                // No open orders, make one for the difference
            //                                if (PositionDifference != 0)
            //                                {
            //                                    #region "Place buy order"
            //                                    resp = AutoMakeOrder("Buy", Convert.ToInt32(PositionDifference));
            //                                    if (resp.ToLower().Contains("error"))
            //                                    {
            //                                        ErrorObject err = jsonSerializer.Deserialize<ErrorObject>(resp);
            //                                        LogMessage("Error " + err.error.name + ":" + err.error.message);
            //                                    }
            //                                    else
            //                                    {
            //                                        LogMessage("Successfully placed buy order");
            //                                        AddToOrderList("", "Buy", PositionDifference.ToString(), "0");

            //                                    }
            //                                    #endregion
            //                                }

            //                            }

            //                        }
            //                        else
            //                        {
            //                            if (OpenOrders.Any())
            //                            {
            //                                // If we have an open order, edit it
            //                                if (OpenOrders.Any(a => a.Side == "Sell"))
            //                                {
            //                                    // We still have an open Sell order, cancel that order, make a new Buy order
            //                                    #region "Cancel All orders"
            //                                    resp = bitmex.CancelAllOpenOrders(ActiveInstrument.Symbol);
            //                                    if (resp.ToLower().Contains("error"))
            //                                    {
            //                                        ErrorObject err = jsonSerializer.Deserialize<ErrorObject>(resp);
            //                                        LogMessage("Error " + err.error.name + ":" + err.error.message);
            //                                    }
            //                                    else
            //                                    {
            //                                        LogMessage("Successfully cancelled all orders");
            //                                    }
            //                                    #endregion

            //                                    #region  "Place buy order"


            //                                    resp = AutoMakeOrder("Buy", Convert.ToInt32(nudAutoQuantity.Value));
            //                                    if (resp.ToLower().Contains("error"))
            //                                    {
            //                                        ErrorObject err = jsonSerializer.Deserialize<ErrorObject>(resp);
            //                                        LogMessage("Error " + err.error.name + ":" + err.error.message);
            //                                    }
            //                                    else
            //                                    {
            //                                        LogMessage("Successfully placed buy order");
            //                                        AddToOrderList("", "Buy", Convert.ToInt32(nudAutoQuantity.Value).ToString(), "0");

            //                                    }
            //                                    #endregion
            //                                }
            //                                else if (OpenOrders.Any(a => a.Side == "Buy"))
            //                                {
            //                                    // Edit our only open order, code should not allow for more than 1 at a time for now
            //                                    #region "Modify order"
            //                                    resp = bitmex.EditOrderPrice(OpenOrders[0].OrderId, CalculateMakerOrderPrice("Buy"));
            //                                    if (resp.ToLower().Contains("error"))
            //                                    {
            //                                        ErrorObject err = jsonSerializer.Deserialize<ErrorObject>(resp);
            //                                        LogMessage("Error " + err.error.name + ":" + err.error.message);
            //                                    }
            //                                    else
            //                                    {
            //                                        LogMessage("Successfullymodified order");
            //                                    }
            //                                    #endregion
            //                                }
            //                            }
            //                            else
            //                            {
            //                                #region "Place buy order"
            //                                resp = AutoMakeOrder("Buy", Convert.ToInt32(nudAutoQuantity.Value));
            //                                if (resp.ToLower().Contains("error"))
            //                                {
            //                                    ErrorObject err = jsonSerializer.Deserialize<ErrorObject>(resp);
            //                                    LogMessage("Error " + err.error.name + ":" + err.error.message);
            //                                }
            //                                else
            //                                {
            //                                    LogMessage("Successfully placed buy order");
            //                                    AddToOrderList("", "Buy", Convert.ToInt32(nudAutoQuantity.Value).ToString(), "0");

            //                                }
            //                                #endregion
            //                            }
            //                        }
            //                        break;
            //                    case "Sell":
            //                        if (OpenPositions.Any())
            //                        {
            //                            int PositionDifference = Convert.ToInt32(nudAutoQuantity.Value + OpenPositions[0].CurrentQty);

            //                            if (OpenOrders.Any())
            //                            {
            //                                // If we have an open order, edit it
            //                                if (OpenOrders.Any(a => a.Side == "Buy"))
            //                                {
            //                                    // We still have an open Sell order, cancel that order, make a new Buy order

            //                                    #region "Cancel all orders"
            //                                    resp = bitmex.CancelAllOpenOrders(ActiveInstrument.Symbol);
            //                                    if (resp.ToLower().Contains("error"))
            //                                    {
            //                                        ErrorObject err = jsonSerializer.Deserialize<ErrorObject>(resp);
            //                                        LogMessage("Error " + err.error.name + ":" + err.error.message);
            //                                    }
            //                                    else
            //                                    {
            //                                        LogMessage("Successfully cancelled all orders");
            //                                    }
            //                                    #endregion

            //                                    #region  "Place sell order"
            //                                    resp = AutoMakeOrder("Sell", PositionDifference);
            //                                    if (resp.ToLower().Contains("error"))
            //                                    {
            //                                        ErrorObject err = jsonSerializer.Deserialize<ErrorObject>(resp);
            //                                        LogMessage("Error " + err.error.name + ":" + err.error.message);
            //                                    }
            //                                    else
            //                                    {
            //                                        LogMessage("Successfully placed sell order");
            //                                        AddToOrderList("", "Sell", PositionDifference.ToString(), "0");

            //                                    }
            //                                    #endregion
            //                                }
            //                                else if (OpenOrders.Any(a => a.Side == "Sell"))
            //                                {
            //                                    // Edit our only open order, code should not allow for more than 1 at a time for now
            //                                    #region "Modify order"
            //                                    resp = bitmex.EditOrderPrice(OpenOrders[0].OrderId, CalculateMakerOrderPrice("Sell"));
            //                                    if (resp.ToLower().Contains("error"))
            //                                    {
            //                                        ErrorObject err = jsonSerializer.Deserialize<ErrorObject>(resp);
            //                                        LogMessage("Error " + err.error.name + ":" + err.error.message);
            //                                    }
            //                                    else
            //                                    {
            //                                        LogMessage("Successfully modified order");
            //                                    }
            //                                    #endregion
            //                                }
            //                            }
            //                            else
            //                            {
            //                                // No open orders, make one for the difference
            //                                if (PositionDifference != 0)
            //                                {
            //                                    #region "Place sell order"
            //                                    resp = AutoMakeOrder("Sell", Convert.ToInt32(PositionDifference));
            //                                    if (resp.ToLower().Contains("error"))
            //                                    {
            //                                        ErrorObject err = jsonSerializer.Deserialize<ErrorObject>(resp);
            //                                        LogMessage("Error " + err.error.name + ":" + err.error.message);
            //                                    }
            //                                    else
            //                                    {
            //                                        LogMessage("Successfully placed sell order");
            //                                        AddToOrderList("", "Sell", PositionDifference.ToString(), "0");

            //                                    }
            //                                    #endregion
            //                                }

            //                            }

            //                        }
            //                        else
            //                        {
            //                            if (OpenOrders.Any())
            //                            {
            //                                // If we have an open order, edit it
            //                                if (OpenOrders.Any(a => a.Side == "Buy"))
            //                                {
            //                                    // We still have an open Sell order, cancel that order, make a new Buy order

            //                                    #region "Cancel all orders"
            //                                    resp = bitmex.CancelAllOpenOrders(ActiveInstrument.Symbol);
            //                                    if (resp.ToLower().Contains("error"))
            //                                    {
            //                                        ErrorObject err = jsonSerializer.Deserialize<ErrorObject>(resp);
            //                                        LogMessage("Error " + err.error.name + ":" + err.error.message);
            //                                    }
            //                                    else
            //                                    {
            //                                        LogMessage("Successfully cancelled all orders");
            //                                    }
            //                                    #endregion

            //                                    #region "Place sell order"
            //                                    resp = AutoMakeOrder("Sell", Convert.ToInt32(nudAutoQuantity.Value));
            //                                    if (resp.ToLower().Contains("error"))
            //                                    {
            //                                        ErrorObject err = jsonSerializer.Deserialize<ErrorObject>(resp);
            //                                        LogMessage("Error " + err.error.name + ":" + err.error.message);
            //                                    }
            //                                    else
            //                                    {
            //                                        LogMessage("Successfully placed sell order");
            //                                        AddToOrderList("", "Sell", Convert.ToInt32(nudAutoQuantity.Value).ToString(), "0");

            //                                    }
            //                                    #endregion
            //                                }
            //                                else if (OpenOrders.Any(a => a.Side == "Sell"))
            //                                {
            //                                    // Edit our only open order, code should not allow for more than 1 at a time for now

            //                                    #region "Modify order"
            //                                    resp = bitmex.EditOrderPrice(OpenOrders[0].OrderId, CalculateMakerOrderPrice("Sell"));
            //                                    if (resp.ToLower().Contains("error"))
            //                                    {
            //                                        ErrorObject err = jsonSerializer.Deserialize<ErrorObject>(resp);
            //                                        LogMessage("Error " + err.error.name + ":" + err.error.message);
            //                                    }
            //                                    else
            //                                    {
            //                                        LogMessage("Successfully modified order");
            //                                    }
            //                                    #endregion
            //                                }
            //                            }
            //                            else
            //                            {

            //                                #region "Place sell order"
            //                                resp = AutoMakeOrder("Sell", Convert.ToInt32(nudAutoQuantity.Value));

            //                                if (resp.ToLower().Contains("error"))
            //                                {
            //                                    ErrorObject err = jsonSerializer.Deserialize<ErrorObject>(resp);
            //                                    LogMessage("Error " + err.error.name + ":" + err.error.message);
            //                                }
            //                                else
            //                                {
            //                                    LogMessage("Successfully placed sell order");
            //                                    AddToOrderList("", "Sell", Convert.ToInt32(nudAutoQuantity.Value).ToString(), "0");

            //                                }

            //                                #endregion
            //                            }
            //                        }
            //                        break;
            //                    case "CloseLongsAndWait":
            //                        if (OpenPositions.Any())
            //                        {
            //                            // Now, do we have open orders?  If so, we want to make sure they are at the right price
            //                            if (OpenOrders.Any())
            //                            {
            //                                if (OpenOrders.Any(a => a.Side == "Buy"))
            //                                {
            //                                    // We still have an open buy order, cancel that order, make a new sell order
            //                                    #region "Cancel all orders"
            //                                    resp = bitmex.CancelAllOpenOrders(ActiveInstrument.Symbol);
            //                                    if (resp.ToLower().Contains("error"))
            //                                    {
            //                                        ErrorObject err = jsonSerializer.Deserialize<ErrorObject>(resp);
            //                                        LogMessage("Error " + err.error.name + ":" + err.error.message);
            //                                    }
            //                                    else
            //                                    {
            //                                        LogMessage("Successfully cancelled all orders");
            //                                    }
            //                                    #endregion

            //                                    #region "Place sell order"
            //                                    resp = AutoMakeOrder("Sell", Convert.ToInt32(OpenPositions[0].CurrentQty));
            //                                    if (resp.ToLower().Contains("error"))
            //                                    {
            //                                        ErrorObject err = jsonSerializer.Deserialize<ErrorObject>(resp);
            //                                        LogMessage("Error " + err.error.name + ":" + err.error.message);
            //                                    }
            //                                    else
            //                                    {
            //                                        LogMessage("Successfully placed sell order");
            //                                        AddToOrderList("", "Sell", Convert.ToInt32(OpenPositions[0].CurrentQty).ToString(), "0");

            //                                    }
            //                                    #endregion
            //                                }
            //                                else if (OpenOrders.Any(a => a.Side == "Sell"))
            //                                {
            //                                    // Edit our only open order, code should not allow for more than 1 at a time for now
            //                                    #region "Modify order"
            //                                    resp = bitmex.EditOrderPrice(OpenOrders[0].OrderId, CalculateMakerOrderPrice("Sell"));
            //                                    if (resp.ToLower().Contains("error"))
            //                                    {
            //                                        ErrorObject err = jsonSerializer.Deserialize<ErrorObject>(resp);
            //                                        LogMessage("Error " + err.error.name + ":" + err.error.message);
            //                                    }
            //                                    else
            //                                    {
            //                                        LogMessage("Successfully modified order");
            //                                    }
            //                                    #endregion
            //                                }

            //                            }
            //                            else if (OpenPositions[0].CurrentQty > 0)
            //                            {
            //                                // No open orders, need to make an order to sell

            //                                #region "Place sell order"
            //                                resp = AutoMakeOrder("Sell", Convert.ToInt32(OpenPositions[0].CurrentQty));

            //                                if (resp.ToLower().Contains("error"))
            //                                {
            //                                    ErrorObject err = jsonSerializer.Deserialize<ErrorObject>(resp);
            //                                    LogMessage("Error " + err.error.name + ":" + err.error.message);
            //                                }
            //                                else
            //                                {
            //                                    LogMessage("Successfully placed sell order");
            //                                    AddToOrderList("", "Sell", Convert.ToInt32(OpenPositions[0].CurrentQty).ToString(), "0");

            //                                }

            //                                #endregion
            //                            }
            //                        }
            //                        else if (OpenOrders.Any())
            //                        {
            //                            // We don't have an open position, but we do have an open order, close that order, we don't want to open any position here.

            //                            #region "Cancel all orders"
            //                            resp = bitmex.CancelAllOpenOrders(ActiveInstrument.Symbol);

            //                            if (resp.ToLower().Contains("error"))
            //                            {
            //                                ErrorObject err = jsonSerializer.Deserialize<ErrorObject>(resp);
            //                                LogMessage("Error " + err.error.name + ":" + err.error.message);
            //                            }
            //                            else
            //                            {
            //                                LogMessage("Successfully cancelled all orders");
            //                            }
            //                            #endregion
            //                        }
            //                        break;
            //                    case "CloseShortsAndWait":
            //                        // Close any open orders, close any open shorts, we've missed our chance to long.
            //                        if (OpenPositions.Any())
            //                        {
            //                            // Now, do we have open orders?  If so, we want to make sure they are at the right price
            //                            if (OpenOrders.Any())
            //                            {
            //                                if (OpenOrders.Any(a => a.Side == "Sell"))
            //                                {
            //                                    // We still have an open sell order, cancel that order, make a new buy order
            //                                    #region  "Cancell all orders"


            //                                    resp = bitmex.CancelAllOpenOrders(ActiveInstrument.Symbol);
            //                                    if (resp.ToLower().Contains("error"))
            //                                    {
            //                                        ErrorObject err = jsonSerializer.Deserialize<ErrorObject>(resp);
            //                                        LogMessage("Error " + err.error.name + ":" + err.error.message);
            //                                    }
            //                                    else
            //                                    {
            //                                        LogMessage("Successfully cancelled all orders");
            //                                    }
            //                                    #endregion

            //                                    #region "Place buy order"
            //                                    resp = AutoMakeOrder("Buy", Convert.ToInt32(OpenPositions[0].CurrentQty));
            //                                    if (resp.ToLower().Contains("error"))
            //                                    {
            //                                        ErrorObject err = jsonSerializer.Deserialize<ErrorObject>(resp);
            //                                        LogMessage("Error " + err.error.name + ":" + err.error.message);
            //                                    }
            //                                    else
            //                                    {
            //                                        LogMessage("Successfully placed buy order");
            //                                        AddToOrderList("", "Buy", Convert.ToInt32(OpenPositions[0].CurrentQty).ToString(), "0");

            //                                    }
            //                                    #endregion
            //                                }
            //                                else if (OpenOrders.Any(a => a.Side == "Buy"))
            //                                {
            //                                    // Edit our only open order, code should not allow for more than 1 at a time for now
            //                                    #region "Modify order"
            //                                    resp = bitmex.EditOrderPrice(OpenOrders[0].OrderId, CalculateMakerOrderPrice("Buy"));
            //                                    if (resp.ToLower().Contains("error"))
            //                                    {
            //                                        ErrorObject err = jsonSerializer.Deserialize<ErrorObject>(resp);
            //                                        LogMessage("Error " + err.error.name + ":" + err.error.message);
            //                                    }
            //                                    else
            //                                    {
            //                                        LogMessage("Successfully modified order");
            //                                    }
            //                                    #endregion
            //                                }

            //                            }
            //                            else if (OpenPositions[0].CurrentQty < 0)
            //                            {
            //                                // No open orders, need to make an order to sell
            //                                #region "Place buy order"
            //                                resp = AutoMakeOrder("Buy", Convert.ToInt32(OpenPositions[0].CurrentQty));
            //                                if (resp.ToLower().Contains("error"))
            //                                {
            //                                    ErrorObject err = jsonSerializer.Deserialize<ErrorObject>(resp);
            //                                    LogMessage("Error " + err.error.name + ":" + err.error.message);
            //                                }
            //                                else
            //                                {
            //                                    LogMessage("Successfully placed buy order");
            //                                    AddToOrderList("", "Buy", Convert.ToInt32(OpenPositions[0].CurrentQty).ToString(), "0");

            //                                }
            //                                #endregion
            //                            }
            //                        }
            //                        else if (OpenOrders.Any())
            //                        {
            //                            // We don't have an open position, but we do have an open order, close that order, we don't want to open any position here.
            //                            #region "Cancel all orders"
            //                            resp = bitmex.CancelAllOpenOrders(ActiveInstrument.Symbol);
            //                            if (resp.ToLower().Contains("error"))
            //                            {
            //                                ErrorObject err = jsonSerializer.Deserialize<ErrorObject>(resp);
            //                                LogMessage("Error " + err.error.name + ":" + err.error.message);
            //                            }
            //                            else
            //                            {
            //                                LogMessage("Successfully cancelled all orders");
            //                            }
            //                            #endregion
            //                        }
            //                        break;
            //                }
            //            }
            //            #endregion
            //        }
            //        catch (Exception ex)
            //        {

            //            LogMessage("Exception : " + ex.Message);
            //        }


            //    }

            //}
            //else
            //{
            //    LogMessage("Internet connectivity is not available");
            //}
            CurrentPrice = Convert.ToDouble(Candles[0].Close);
        }

        // Check account balance/validity
        private void GetAPIValidity()
        {
            try // Code is simple, if we get our account balance without an error the API is valid, if not, it will throw an error and API will be marked not valid.
            {

                WalletBalance = bitmex.GetAccountBalance();
                if (WalletBalance >= 0)
                {
                    APIValid = true;
                    stsAPIValid.Text = "API keys are valid";
                    stsAccountBalance.Text = "Balance: " + WalletBalance.ToString();
                }
                else
                {
                    APIValid = false;
                    stsAPIValid.Text = "API keys are invalid";
                    stsAccountBalance.Text = "Balance: 0";
                }
            }
            catch (Exception ex)
            {
                APIValid = false;
                stsAPIValid.Text = "API keys are invalid";
                stsAccountBalance.Text = "Balance: 0";
            }
        }

        // Update balances
        private void btnAccountBalance_Click(object sender, EventArgs e)
        {
            GetAPIValidity();
        }

        // Set Market Stops
        private void btnManualSetStop_Click(object sender, EventArgs e)
        {
            OpenPositions = bitmex.GetOpenPositions(ActiveInstrument.Symbol);

            if (OpenPositions.Any()) // Only set stops if we have open positions
            {
                // Now determine what kind of stop to set
                if (OpenPositions[0].CurrentQty > 0)
                {
                    // Determine stop price, x percent below current price.
                    double PercentPriceDifference = Convert.ToDouble(Candles[0].Close) * (Convert.ToDouble(nudStopPercent.Value) / 100);
                    double StopPrice = Convert.ToDouble(Candles[0].Close) - PercentPriceDifference;
                    // Round the Stop Price down to the tick size so the price is valid
                    StopPrice = StopPrice - (StopPrice % ActiveInstrument.TickSize);
                    // Set a stop to sell
                    bitmex.MarketStop(ActiveInstrument.Symbol, "Sell", StopPrice, Convert.ToInt32(OpenPositions[0].CurrentQty), true, ddlCandleTimes.SelectedItem.ToString());
                }
                else if (OpenPositions[0].CurrentQty < 0)
                {
                    // Determine stop price, x percent below current price.
                    double PercentPriceDifference = Convert.ToDouble(Candles[0].Close) * (Convert.ToDouble(nudStopPercent.Value) / 100);
                    double StopPrice = Convert.ToDouble(Candles[0].Close) + PercentPriceDifference;
                    // Round the Stop Price down to the tick size so the price is valid
                    StopPrice = StopPrice - (StopPrice % ActiveInstrument.TickSize);
                    // Set a stop to sell
                    bitmex.MarketStop(ActiveInstrument.Symbol, "Buy", StopPrice, (Convert.ToInt32(OpenPositions[0].CurrentQty) * -1), true, ddlCandleTimes.SelectedItem.ToString());
                }
            }
        }

        private void txtAPIKey_TextChanged(object sender, EventArgs e)
        {
            switch (ddlNetwork.SelectedItem.ToString())
            {
                case "TestNet":
                    Properties.Settings.Default.TestAPIKey = txtAPIKey.Text;
                    break;
                case "RealNet":
                    Properties.Settings.Default.APIKey = txtAPIKey.Text;
                    break;
            }
            SaveSettings();
            InitializeAPI();
        }

        private void txtAPISecret_TextChanged(object sender, EventArgs e)
        {
            switch (ddlNetwork.SelectedItem.ToString())
            {
                case "TestNet":
                    Properties.Settings.Default.TestAPISecret = txtAPISecret.Text;
                    break;
                case "RealNet":
                    Properties.Settings.Default.APISecret = txtAPISecret.Text;
                    break;
            }
            SaveSettings();
            InitializeAPI();
        }

        private void SaveSettings()
        {
            Properties.Settings.Default.Save();
        }


        // Over Time ordering
        private void UpdateOverTimeSummary()
        {
            OTContractsPer = Convert.ToInt32(nudOverTimeContracts.Value);
            OTIntervalSeconds = Convert.ToInt32(nudOverTimeInterval.Value);
            OTIntervalCount = Convert.ToInt32(nudOverTimeIntervalCount.Value);

            lblOverTimeSummary.Text = (OTContractsPer * OTIntervalCount).ToString() + " Contracts over " + OTIntervalCount.ToString() + " orders during a total of " + (OTIntervalCount * OTIntervalSeconds).ToString() + " seconds.";

        }

        private void nudOverTimeContracts_ValueChanged(object sender, EventArgs e)
        {
            UpdateOverTimeSummary();
        }

        private void nudOverTimeInterval_ValueChanged(object sender, EventArgs e)
        {
            UpdateOverTimeSummary();
        }

        private void nudOverTimeIntervalCount_ValueChanged(object sender, EventArgs e)
        {
            UpdateOverTimeSummary();
        }

        private void btnBuyOverTimeOrder_Click(object sender, EventArgs e)
        {
            LogMessage("Buying over time");
            UpdateOverTimeSummary(); // Makes sure our variables are current.

            OTSide = "Buy";

            tmrTradeOverTime.Interval = OTIntervalSeconds * 1000; // Must multiply by 1000, because timers operate in milliseconds.
            tmrTradeOverTime.Start(); // Start the timer.
            stsOTProgress.Value = 0;
            stsOTProgress.Visible = true;
        }

        private void btnSellOverTimeOrder_Click(object sender, EventArgs e)
        {
            LogMessage("Selling over time");
            UpdateOverTimeSummary(); // Makes sure our variables are current.

            OTSide = "Sell";

            tmrTradeOverTime.Interval = OTIntervalSeconds * 1000; // Must multiply by 1000, because timers operate in milliseconds.
            tmrTradeOverTime.Start(); // Start the timer.
            stsOTProgress.Value = 0;
            stsOTProgress.Visible = true;
        }

        private void tmrTradeOverTime_Tick(object sender, EventArgs e)
        {
            string resp;
            if (CheckForInternetConnection())
            {
                tmrTradeOverTime.Enabled = false;
                OTTimerCount++;
                //while (true)
                //{
                resp = bitmex.MarketOrder(ActiveInstrument.Symbol, OTSide, OTContractsPer);
                if (resp.Contains("Error"))
                {
                    ErrorObject err = jsonSerializer.Deserialize<ErrorObject>(resp);
                    LogMessage("Error " + err.error.name + ":" + err.error.message);
                    //Thread.Sleep(2000);
                }
                else
                {
                    LogMessage("Successfully placed " + OTSide + " order over time");
                    //break;
                }
                //}


                double Percent = ((double)OTTimerCount / (double)OTIntervalCount) * 100;
                stsOTProgress.Value = Convert.ToInt32(Math.Round(Percent));

                if (OTTimerCount == OTIntervalCount)
                {
                    OTTimerCount = 0;
                    tmrTradeOverTime.Stop();
                    stsOTProgress.Value = 0;
                    stsOTProgress.Visible = false;

                }
                tmrTradeOverTime.Enabled = true;

            }
            else
            {
                LogMessage("Internet connectivity is not available");
            }
        }

        private void btnOverTimeStop_Click(object sender, EventArgs e)
        {
            OTTimerCount = 0;
            stsOTProgress.Value = 0;
            stsOTProgress.Visible = false;
            tmrTradeOverTime.Stop();
        }

        private void btnBulkTest_Click(object sender, EventArgs e)
        {
            //string orders = "[{\"orderQty\": 89, \"price\": 9479, \"side\": \"Sell\", \"symbol\": \"XBTUSD\"}," +
            //    " { \"orderQty\": 143, \"price\": 9527, \"side\": \"Sell\", \"symbol\": \"XBTUSD\"}," +
            //    " { \"orderQty\": 231, \"price\": 9605, \"side\": \"Sell\", \"symbol\": \"XBTUSD\"}," +
            //    " { \"orderQty\": 374, \"price\": 9731, \"side\": \"Sell\", \"symbol\": \"XBTUSD\"}," +
            //    " { \"orderQty\": 605, \"price\": 9935, \"side\": \"Sell\", \"symbol\": \"XBTUSD\"}," +
            //    " { \"orderQty\": 978, \"price\": 10266, \"side\": \"Sell\", \"symbol\": \"XBTUSD\"}," +
            //    " { \"orderQty\": 1583, \"price\": 10800, \"side\": \"Sell\", \"symbol\": \"XBTUSD\"}]";
            //bitmex.BulkOrder(orders);



            List<Order> Orders = new List<Order>();

            // While we are manually testing, we are going to manually code in the orders, but
            //  idealy you would use controls to determine how to make orders
            Order Ord = new Order();
            Ord.OrderQty = 10;
            Ord.Price = 10000;
            Ord.Side = "Sell";
            Ord.Symbol = ActiveInstrument.Symbol;

            Order Ord2 = new Order();
            Ord2.OrderQty = 20;
            Ord2.Price = 10010;
            Ord2.Side = "Sell";
            Ord2.Symbol = ActiveInstrument.Symbol;

            Orders.Add(Ord);
            Orders.Add(Ord2);


            if (Orders.Any())
            {
                string OrderJSON = BuildBulkOrder(Orders);
                bitmex.BulkOrder(OrderJSON);
            }


        }

        private string BuildBulkOrder(List<Order> Orders, bool Amend = false)
        {
            StringBuilder str = new StringBuilder();

            str.Append("[");

            int i = 1;
            foreach (Order o in Orders)
            {
                if (i > 1)
                {
                    str.Append(", ");
                }
                str.Append("{");
                if (Amend == true)
                {
                    str.Append("\"orderID\": \"" + o.OrderId.ToString() + "\", ");
                }
                str.Append("\"orderQty\": " + o.OrderQty.ToString() + ", \"price\": " + o.Price.ToString() + ", \"side\": \"" + o.Side + "\", \"symbol\": \"" + o.Symbol + "\"");
                str.Append("}");
                i++;
            }

            str.Append("]");

            return str.ToString();
        }

        private void btnBulkShift_Click(object sender, EventArgs e)
        {
            List<Order> OpenOrders = bitmex.GetOpenOrders(ActiveInstrument.Symbol);

            if (OpenOrders.Any())
            {
                foreach (Order o in OpenOrders)
                {
                    o.Price += 20;
                }

                string OrderJSON = BuildBulkOrder(OpenOrders, true);
                bitmex.AmendBulkOrder(OrderJSON);
            }


        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
            log.AutoFlush = true;
            bitmex.apiKey = "PBYi59QNS3XH0-eBwBgZOLO1";
            bitmex.apiSecret = "tpr1N_U9Xwo-LicubnEJjQdYVRTmpDaQmYfo2zhqwWKLOdXa";

            //    public string OrderID;
            //public string PlacingTime;
            //public string OrderType;
            //public string OrderSide;
            //public string OrderQuantity;
            //public string OrderPrice;
            //public string BotStatus;

            Orders.Columns.Add("OrderID");
            Orders.Columns.Add("PlacingTime");
            Orders.Columns.Add("OrderType");
            Orders.Columns.Add("OrderSide");
            Orders.Columns.Add("OrderQuantity");
            Orders.Columns.Add("OrderPrice");
            Orders.Columns.Add("OrderStatus");
            Orders.Columns.Add("BotStatus");



            //InitializeAuthWebSocket();
        }

        private void rdoBuy_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void btnShowHideCols_Click(object sender, EventArgs e)
        {
            if (gbShowHideCols.Visible == false)
            {
                gbShowHideCols.BringToFront();
                DataTable dt = new DataTable();
                dt.Columns.Add("Show");
                dt.Columns.Add("Column Header");
                DataRow dr;
                int i;
                for (i = 0; i < dgvCandles.Columns.Count; i++)
                {
                    dr = dt.NewRow();
                    if (dgvCandles.Columns[i].Visible)
                        dr[0] = "Hide";
                    else
                        dr[0] = "Show";
                    dr[1] = dgvCandles.Columns[i].HeaderText;
                    dt.Rows.Add(dr);
                }
                dgShowHideCols.DataSource = dt;
                gbShowHideCols.Visible = true;
            }
            else
            {
                gbShowHideCols.Visible = false;
            }
        }

        private void dgShowHideCols_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                if (dgShowHideCols.Rows[e.RowIndex].Cells[0].Value.ToString().Trim() == "Hide")
                {
                    dgShowHideCols.Rows[e.RowIndex].Cells[0].Value = "Show";
                    dgvCandles.Columns[e.RowIndex].Visible = false;
                    dgShowHideCols.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Orange;
                    dgShowHideCols.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.White;
                }
                else
                {
                    dgShowHideCols.Rows[e.RowIndex].Cells[0].Value = "Hide";
                    dgvCandles.Columns[e.RowIndex].Visible = true;
                    dgShowHideCols.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LimeGreen;
                    dgShowHideCols.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.White;
                }

            }
        }

        private void dgShowHideCols_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {

        }

        private void dgShowHideCols_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            foreach (DataGridViewRow Myrow in dgShowHideCols.Rows)
            {            //Here 2 cell is target value and 1 cell is Volume
                Myrow.DefaultCellStyle.ForeColor = Color.White;
                if (Myrow.Cells[0].Value.ToString().Trim() == "Show")// Or your condition 
                {
                    Myrow.DefaultCellStyle.BackColor = Color.Orange;
                }
                else
                {
                    Myrow.DefaultCellStyle.BackColor = Color.LimeGreen;
                }
            }
        }

        private void nudCandleRefreshTime_ValueChanged(object sender, EventArgs e)
        {
            tmrCandleUpdater.Interval = int.Parse(nudCandleRefreshTime.Value.ToString()) * 1000;
        }
    }

    public class PlacedOrders
    {
        public string OrderID { get; set; }
        public string PlacingTime { get; set; }
        public string OrderType { get; set; }
        public string OrderSide { get; set; }
        public string OrderQuantity { get; set; }
        public string OrderPrice { get; set; }
        public string OrderStatus { get; set; }
        public string BotStatus { get; set; }
    }


    public class OrderResponse
    {
        public string orderID { get; set; }
        public string clOrdID { get; set; }
        public string clOrdLinkID { get; set; }
        public string account { get; set; }
        public string symbol { get; set; }
        public string side { get; set; }
        public string simpleOrderQty { get; set; }
        public string orderQty { get; set; }
        public string price { get; set; }
        public string displayQty { get; set; }
        public string stopPx { get; set; }
        public string pegOffsetValue { get; set; }
        public string pegPriceType { get; set; }
        public string currency { get; set; }
        public string settlCurrency { get; set; }
        public string ordType { get; set; }
        public string timeInForce { get; set; }
        public string execInst { get; set; }
        public string contingencyType { get; set; }
        public string exDestination { get; set; }
        public string ordStatus { get; set; }
        public string triggered { get; set; }
        public string workingIndicator { get; set; }
        public string ordRejReason { get; set; }
        public string simpleLeavesQty { get; set; }
        public string leavesQty { get; set; }
        public string simpleCumQty { get; set; }
        public string cumQty { get; set; }
        public string avgPx { get; set; }
        public string multiLegReportingType { get; set; }
        public string text { get; set; }
        public string transactTime { get; set; }
        public string timestamp { get; set; }
    }
}
