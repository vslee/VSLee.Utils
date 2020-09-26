using System.Collections.Generic;

namespace VSLee.Utils.ExchangeBase
{
	public static class PLP
	{
		/// <summary>
		/// key: exchange, value: header reference
		/// </summary>
		public static readonly SortedList<Exchange_Enum, string> HeaderReference
			= new SortedList<Exchange_Enum, string>() {
				{ Exchange_Enum.Binance,        "side,size,price,time,tradeId,firstTradeId,lastTradeId" },
				{ Exchange_Enum.BinanceDEX,     "side,size,price,time,tradeId,buyerOrderId,sellerOrderId,buyerAddress,sellerAddress,tickerType" },
				{ Exchange_Enum.BinanceJersey,  "side,size,price,time,tradeId,firstTradeId,lastTradeId" },
				{ Exchange_Enum.BinanceUS,      "side,size,price,time,tradeId,firstTradeId,lastTradeId" },
				{ Exchange_Enum.Bitfinex,       "side,size,price,time,tradeId" },
				{ Exchange_Enum.BitFlyer,       "side,size,price,time,tradeId,buyChildOrderAcceptanceId,sellChildOrderAcceptanceId" },
				{ Exchange_Enum.BitMEX,         "side,size,price,time,tradeId" },
				{ Exchange_Enum.Bitstamp,       "side,size,price,time,tradeId,buyOrderId,sellOrderId" },
				{ Exchange_Enum.Bittrex,        "side,size,price,time,tradeId" },
				{ Exchange_Enum.BL3P,           "side,size,price,time" }, // - tradeId
				{ Exchange_Enum.Coinbase,       "side,size,price,time,tradeId,makerOrderId,takerOrderId" },
				{ Exchange_Enum.CoinEx,         "side,size,price,time,tradeId" },
				{ Exchange_Enum.Digifinex,      "side,size,price,time,tradeId" },
				{ Exchange_Enum.Gemini,         "side,size,price,time,tradeId" },
				{ Exchange_Enum.Huobi,          "side,size,price,time,tradeId" },
				{ Exchange_Enum.itBit,          "size,price,time,tradeId" }, // - side
				{ Exchange_Enum.Kraken,         "side,size,price,time,orderType" }, // - tradeID; + orderType
				{ Exchange_Enum.KuCoin,         "side,size,price,time,tradeId,makerOrderId,takerOrderId" },
				{ Exchange_Enum.NDAX,           "side,size,price,time,tradeId,order1Id,order2Id,direction,isBlockTrade,clientOrderId" },
				{ Exchange_Enum.OKCoin,         "side,size,price,time,tradeId" },
				{ Exchange_Enum.OKEx,           "side,size,price,time,tradeId" },
				{ Exchange_Enum.Poloniex,       "side,size,price,time,tradeId" },
				{ Exchange_Enum.ZBcom,          "side,size,price,time,tradeId" },
			};
	}
}