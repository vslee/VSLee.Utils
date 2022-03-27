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
				{ Exchange_Enum.ApolloX,        $"{standard},firstTradeId,lastTradeId" },
				{ Exchange_Enum.Binance,        $"{standard},firstTradeId,lastTradeId" },
				{ Exchange_Enum.BinanceDEX,     $"{standard},buyerOrderId,sellerOrderId,buyerAddress,sellerAddress,tickerType" },
				{ Exchange_Enum.BinanceJersey,  $"{standard},firstTradeId,lastTradeId" },
				{ Exchange_Enum.BinanceUS,      $"{standard},firstTradeId,lastTradeId" },
				{ Exchange_Enum.Bitfinex,       standard },
				{ Exchange_Enum.BitFlyer,       $"{standard},buyChildOrderAcceptanceId,sellChildOrderAcceptanceId" },
				{ Exchange_Enum.Bithumb,           "side,size,price,time" }, // - tradeId
				{ Exchange_Enum.BitMax,       $"{standard},seqNum" },
				{ Exchange_Enum.BitMEX,         standard },
				{ Exchange_Enum.Bitstamp,       $"{standard},buyOrderId,sellOrderId" },
				{ Exchange_Enum.Bittrex,        standard },
				{ Exchange_Enum.BL3P,           "side,size,price,time" }, // - tradeId
				{ Exchange_Enum.BtcTurk,         standard },
				{ Exchange_Enum.Bybit,       $"{standard},crossSequence" },
				{ Exchange_Enum.Coinbase,       $"{standard},makerOrderId,takerOrderId" },
				{ Exchange_Enum.CoinEx,         standard },
				{ Exchange_Enum.CryptoCom,         standard },
				{ Exchange_Enum.Digifinex,      standard },
				{ Exchange_Enum.Dydx,           "side,size,price,time" }, // - tradeId
				{ Exchange_Enum.FTX,       $"{standard},isLiquidationOrder" },
				{ Exchange_Enum.FTXUS,       $"{standard},isLiquidationOrder" },
				{ Exchange_Enum.GateIO,         standard },
				{ Exchange_Enum.Gemini,         standard },
				{ Exchange_Enum.HitBTC,          standard },
				{ Exchange_Enum.Huobi,          standard },
				{ Exchange_Enum.itBit,          "size,price,time,tradeId" }, // - side
				{ Exchange_Enum.Kraken,         "side,size,price,time,orderType" }, // - tradeID; + orderType
				{ Exchange_Enum.KuCoin,         $"{standard},makerOrderId,takerOrderId" },
				{ Exchange_Enum.LBank,           "side,size,price,time" }, // - tradeId
				{ Exchange_Enum.LiveCoin,         $"{standard},orderBuyId,orderSellId" },
				{ Exchange_Enum.NDAX,           $"{standard},order1Id,order2Id,direction,isBlockTrade,clientOrderId" },
				{ Exchange_Enum.OKCoin,         standard },
				{ Exchange_Enum.OKEx,           standard },
				{ Exchange_Enum.Paribu,           "side,size,price,time" }, // - tradeId
				{ Exchange_Enum.Poloniex,       standard },
				{ Exchange_Enum.ZBcom,          standard },
			};

		const string standard = "side,size,price,time,tradeId";
	}
}