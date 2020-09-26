using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VSLee.Utils.ExchangeBase
{ // C# byte: 0 to 255, SQL Server tinyint	0 to 255
	public enum Side : byte
	{
		Buy = 2,
		Sell = 4,
		/// <summary>
		/// IB: Sell Short as part of a combo leg
		/// </summary>
		SellShort = 6,
		/// <summary>
		/// IB: Short Sale Exempt action.
		/// SSHORTX allows some orders to be marked as exempt from the new SEC Rule 201
		/// </summary>
		SShortX = 8,
		/// <summary>
		/// IEX Hist: always
		/// IB (Undefined)
		/// </summary>
		NotAvailable = 10
	}
	public static partial class EBEnumExtensionMethods
	{
		public static Side OpposingSide(this Side side)
		{
			if (side == Side.Buy)
				return Side.Sell;
			else if (side == Side.Sell)
				return Side.Buy;
			else throw new Exception($"Unexpected side in OpposingSide(): {side}");
		}
	}

	public enum OrderType : byte
	{
		// Kraken
		// market
		// limit(price = limit price)
		// stop-loss(price = stop loss price)
		// take-profit(price = take profit price)
		// stop-loss-profit(price = stop loss price, price2 = take profit price)
		// stop-loss-profit-limit(price = stop loss price, price2 = take profit price)
		// stop-loss-limit(price = stop loss trigger price, price2 = triggered limit price)
		// take-profit-limit(price = take profit trigger price, price2 = triggered limit price)
		// trailing-stop(price = trailing stop offset)
		// trailing-stop-limit(price = trailing stop offset, price2 = triggered limit offset)
		// stop-loss-and-limit(price = stop loss price, price2 = limit price)
		// settle-position

		Market = 2, Limit = 4,

		/// <summary>
		/// IB: A market order that is submitted to execute as close to the closing price as possible.
		/// Non US Futures, Non US Options, Stocks
		/// </summary>
		MarketOnClose = 6,

		/// <summary>
		/// IB: An LOC (Limit-on-Close) order that executes at the closing price if the closing price is at or better than the submitted limit price, according to the rules of the specific exchange. Otherwise the order will be cancelled. 
		/// Non US Futures , Stocks
		/// </summary>
		LimitOnClose = 8,

		// TODO: reconcile OrderTypes from IB, TDA, Kraken, etc...
	}

	public enum Exchange_Enum : byte
	{
		// crypto exchanges
		Binance = 5,
		BinanceDEX = 6,
		BinanceJersey = 7,
		BinanceUS = 8,
		BitBlinx = 11,
		Bitfinex = 14,
		BitFlyer = 17,
		Bithumb = 20,
		BitMEX = 23,
		Bitstamp = 26,
		Bittrex = 29,
		BL3P = 32,
		CEXIO = 34,
		/// <summary> coinbase pro AKA GDAX </summary>
		Coinbase = 36,
		CoinEx = 38,
		Digifinex = 40,
		Gemini = 42,
		itBit = 44,
		HitBTC = 46,
		Huobi = 48,
		Kraken = 50,
		KuCoin = 52,
		LBank = 54,
		NDAX = 56,
		OKCoin = 59,
		OKEx = 61,
		Poloniex = 64,
		ThePit = 66,
		ZBcom = 68,

		Backtest = 80, // + just one backtest for now, but may need to create separate ones in the future for diff data sources

		// stock exchanges / brokers / data sources
		Alpaca = 110,
		AlphaVantage = 115,
		Dukascopy = 120,
		IEX = 125,
		InteractiveBrokers = 130,
		Robinhood = 135,
		TDAmeritrade = 140,
		WRDS = 145,

		// placeholders for discriminators
		Discriminator_Base = 150,
		Discriminator_StandardBase = 154,
		Discriminator_Standard = 156,
		Discriminator_StandardEquity = 158,
		Discriminator_StandardCrypto = 160,
		Discriminator_GradientBase = 162,
		Discriminator_Gradient = 164,
		Discriminator_GradientEquityBase = 166,
		Discriminator_GradientEquity = 168,
		Discriminator_GradientCryptoBase = 170,
		Discriminator_GradientCrypto = 172,
		Discriminator_ESLedger = 174,
		Discriminator_BinanceBase = 176,
	}

	public enum Realm : byte
	{
		Equity = 2,
		Crypto = 4,
	}

	public enum TimeInForce : byte
	{
		/// <summary>
		/// Binance: Good 'til canceled.
		/// IB: Good until canceled. The order will continue to work within the system and in the marketplace until it 
		///		executes or is canceled. GTC orders will be automatically be cancelled under the following conditions: If a 
		///		corporate action on a security results in a stock split (forward or reverse), exchange for shares, or 
		///		distribution of shares. If you do not log into your IB account for 90 days.
		///		At the end of the calendar quarter following the current quarter. For example, an order placed during the 
		///		third quarter of 2011 will be canceled at the end of the first quarter of 2012. If the last day is a 
		///		non-trading day, the cancellation will occur at the close of the final trading day of that quarter. For 
		///		example, if the last day of the quarter is Sunday, the orders will be cancelled on the preceding Friday.
		///		Orders that are modified will be assigned a new “Auto Expire” date consistent with the end of the calendar 
		///		quarter following the current quarter.
		///		Orders submitted to IB that remain in force for more than one day will not be reduced for dividends.To 
		///		allow adjustment to your order price on ex-dividend date, consider using a Good-Til-Date/Time(GTD) or 
		///		Good-after-Time/Date(GAT) order type, or a combination of the two.
		/// </summary>
		GTC = 10,

		/// <summary>
		/// Binance: Immediate or cancel.
		/// </summary>
		IOC = 20,

		/// <summary>
		/// Binance: Fill or kill.
		/// </summary>
		FOK = 30,

		/// <summary>
		/// IB:  Using "Day" as the time in force for a Regular Trading Hours ("RTH") Only order specifies that the order 
		///		will work throughout the trading day during regular trading hours until it is filled, is canceled by the user, 
		///		or expires at the end of the trading day. (By default, the time in force for orders is set to "Day." Orders 
		///		for some products are entered with a default restriction of Regular Trading Hours (RTH) Only to prevent orders 
		///		from being executed during the pre- and post-market sessions).
		///		A day order that is submitted after regular trading hours* with the order attribute of RTH will be held in the 
		///		system and begin to work at the start of the next trading day. The order will be active during the next trading 
		///		day until it is filled, is canceled by the user, or expires at the end of the trading day. IMPORTANT NOTE: You 
		///		can add order attributes to any order or change default settings to specify that orders be activated, triggered, 
		///		or filled outside of regular trading hours.
		///		Note:   You can add order attributes to any order or change default settings to specify that orders be activated, 
		///		triggered, or filled outside of regular trading hours.
		///		* For TWS: Day orders submitted with “RTH ONLY” will be canceled between 4:00-4:05 pm. Day orders submitted after 
		///		4:05 pm with “RTH ONLY” will be queued for the next day.
		/// </summary>
		DAY = 40,

		/// <summary>
		/// IB: Day until Canceled: similar to a day order, but instead of being canceled and removed from the trading 
		///		screen at the end of the day, the order is deactivated. This means that the order is canceled AT THE 
		///		EXCHANGE but remains on your trading screen to be re-transmitted whenever you click the Transmit button.
		/// </summary>
		DTC = 50,
	}

	public enum OrderStatus : byte
	{
		// internal program statuses (no official status on GDAX)
		/// <summary> prior to submission </summary>
		CreatedNotReadyToSubmit = 5,
		/// <summary> prior to submission </summary>
		CreatedReadyToSubmit = 10,
		/// <summary> prior to submission, but now out of the OrdersReadyToSubmit RBPQ </summary>
		PreSubmitted = 15,
		/// <summary>
		/// prior to GDAX acknowledgement
		/// IB (PendingSubmit, not Submitted - that maps to Open) not sent by TWS and should be
		/// explicitly set by the API developer when an order is submitted: order was
		/// sent from TWS, but confirmation has not been received that it has been
		/// received by the destination. Most commonly because exchange is closed.
		/// </summary>
		Submitted = 20,
		/// <summary>
		/// GDAX/Binance: submitted, but not accepted
		/// </summary>
		Rejected = 25,
		SyntheticFill = 30, // internally filled
		
		// Exchange statuses
		/// <summary>
		/// GDAX: received, prior to being "Received"?
		/// IB (PreSubmitted): indicates that a simulated order type has been accepted by the IB system and
		/// that this order has yet to be elected. The order is held in the IB system
		/// (and the status remains DARK BLUE) until the election criteria are met.
		/// At that time the order is transmitted to the order destination as specified
		/// (and the order status color will change).
		/// </summary>
		PendingReceived = 35,
		/// <summary>
		/// GDAX (received): same as "received" realtime state
		/// </summary>
		Active = 40,
		/// <summary>
		/// GDAX: now on the order book
		/// Binance (New)
		/// IB (Submitted): indicates that your order has been accepted at the order destination and is working.
		/// </summary>
		Open = 45,
		/// <summary>
		/// GDAX: not in GDAX
		/// Binance: order is still Open
		/// IB: same
		/// </summary>
		PartiallyFilled = 50,
		/// <summary>
		/// Binance: still open but soon to be closed
		/// IB: (not sent by TWS and should be explicitly set by the API 
		/// developer when an order is submitted)  request has been sent 
		/// to cancel an order but confirmation has not been received of its cancellation.
		/// </summary>
		PendingCancel = 55,
		/// <summary>
		/// GDAX/Binance: either cancelled or filled, and also either settled or not 
		/// </summary>
		DoneAmbiguous = 60,
		// settled (settled is a actually a separate boolean field in GDAX)

		/// <summary>
		/// GDAX: does not exist
		/// Binance:
		/// IB: 
		/// </summary>
		Expired = 65,

		/// <summary>
		/// does have official GDAX stautus, but unable to parse
		/// IB (Error, None):  No Order Status. not sent by TWS and should be explicitly set by the API developer when an error has occured
		/// </summary>
		InvalidStatus = 70, // unable to parse GDAX status

		/// <summary>
		/// GDAX: does not exist
		/// Binance:
		/// IB: indicates that the balance of your order has been confirmed canceled by the IB system.
		/// This could occur unexpectedly when IB or the destination has rejected your order.
		/// </summary>
		Canceled = 75,
		/// <summary>
		/// GDAX: does not exist
		/// Binance:
		/// IB: The order has been completely filled.
		/// </summary>
		Filled = 80,
		/// <summary>
		/// GDAX: does not exist
		/// Binance: does not exist
		/// IB: indicates an order is not working, possible reasons include:
		/// - it is invalid or triggered an error. A corresponding error code is expected to the error() function.
		/// - the order is to short shares but the order is being held while shares are being located.
		/// - an order is placed manually in TWS while the exchange is closed.
		/// - an order is blocked by TWS due to a precautionary setting and appears there in an untransmitted state
		/// </summary>
		Inactive = 85,
		/// <summary>
		/// GDAX: does not exist
		/// Binance: does not exist
		/// IB: indicates order has not yet been sent to IB server, for instance 
		/// if there is a delay in receiving the security definition. Uncommonly received.
		/// </summary>
		ApiPending = 90,
		/// <summary>
		/// GDAX: does not exist
		/// Binance: does not exist
		/// IB: after an order has been submitted and before it has been acknowledged, 
		/// an API client can request its cancellation, producing this state.
		/// </summary>
		ApiCancelled = 95,
		/// <summary>
		/// Cancelled internally and never submitted
		/// </summary>
		InternalCancelled = 100,
		/// <summary>
		/// Only clientID no serverID and not found on server despite search.
		/// - happens when order is submitted and then disconnects before server confirmation
		/// </summary>
		OrphanPermanent = 105,
	}

	public static partial class EBEnumExtensionMethods
	{
		public static Exchange_Enum ParseExchangeEnum(this string exchangeString)
		{
			switch (exchangeString.ToLowerInvariant())
			{
				case "binance": return Exchange_Enum.Binance;
				case "binancedex": return Exchange_Enum.BinanceDEX;
				case "binancejersey": return Exchange_Enum.BinanceJersey;
				case "binanceus": return Exchange_Enum.BinanceUS;
				case "bitblinx": return Exchange_Enum.BitBlinx;
				case "bitfinex": return Exchange_Enum.Bitfinex;
				case "bitflyer": return Exchange_Enum.BitFlyer;
				case "bithumb": return Exchange_Enum.Bithumb;
				case "bitmex": return Exchange_Enum.BitMEX;
				case "bitstamp": return Exchange_Enum.Bitstamp;
				case "bittrex": return Exchange_Enum.Bittrex;
				case "bl3p": return Exchange_Enum.BL3P;
				case "cexio": return Exchange_Enum.CEXIO;
				case "coinbase": // will automatically fall through to gdax
				case "gdax": return Exchange_Enum.Coinbase;
				case "coinex": return Exchange_Enum.CoinEx;
				case "digifinex": return Exchange_Enum.Digifinex;
				case "gemini": return Exchange_Enum.Gemini;
				case "itbit": return Exchange_Enum.itBit;
				case "hitbtc": return Exchange_Enum.HitBTC;
				case "huobi": return Exchange_Enum.Huobi;
				case "kraken": return Exchange_Enum.Kraken;
				case "kucoin": return Exchange_Enum.KuCoin;
				case "lbank": return Exchange_Enum.LBank;
				case "ndax": return Exchange_Enum.NDAX;
				case "okcoin": return Exchange_Enum.OKCoin;
				case "okex": return Exchange_Enum.OKEx;
				case "poloniex": return Exchange_Enum.Poloniex;
				case "thepit": return Exchange_Enum.ThePit;
				case "zbcom": return Exchange_Enum.ZBcom;
				case "discriminator_gradientequity": return Exchange_Enum.Discriminator_GradientEquity;
				default: throw new NotImplementedException("unable to parse exchangeString: " + exchangeString);
			}
		}
		public static Realm GetRealm(this Exchange_Enum exchange)
		{
			if (CryptoExchanges.Contains(exchange)) return Realm.Crypto;
			if (EquityExchanges.Contains(exchange)) return Realm.Equity;
			throw new NotImplementedException($"Unexpected exchange in GetRealm() + {exchange}");
		}
		public static Exchange_Enum[] CryptoExchanges => new Exchange_Enum[]
		{
			Exchange_Enum.Binance, Exchange_Enum.BinanceDEX, Exchange_Enum.BinanceJersey, Exchange_Enum.BinanceUS,
			Exchange_Enum.BitBlinx, Exchange_Enum.Bitfinex, Exchange_Enum.BitFlyer,
			Exchange_Enum.BitMEX, Exchange_Enum.Bitstamp, Exchange_Enum.Bittrex,
			Exchange_Enum.BL3P, Exchange_Enum.CEXIO, Exchange_Enum.Coinbase,
			Exchange_Enum.CoinEx, Exchange_Enum.Digifinex, Exchange_Enum.Gemini,
			Exchange_Enum.itBit, Exchange_Enum.Huobi, Exchange_Enum.Kraken,
			Exchange_Enum.KuCoin, Exchange_Enum.NDAX, Exchange_Enum.OKCoin,
			Exchange_Enum.OKEx, Exchange_Enum.Poloniex, Exchange_Enum.ThePit,
			Exchange_Enum.ZBcom,
			Exchange_Enum.Discriminator_GradientCrypto, Exchange_Enum.Discriminator_GradientCryptoBase, Exchange_Enum.Discriminator_StandardCrypto,
		};
		public static bool IsESLedgerExchange(this Exchange_Enum exchange)
		{
			return ESLedgerExchanges.Contains(exchange);
		}
		public static Exchange_Enum[] ESLedgerExchanges => new Exchange_Enum[]
		{
			Exchange_Enum.Binance, Exchange_Enum.BinanceDEX, Exchange_Enum.BinanceJersey, Exchange_Enum.BinanceUS,
			Exchange_Enum.BitBlinx, Exchange_Enum.Bitfinex, Exchange_Enum.BitFlyer,
			Exchange_Enum.BitMEX, Exchange_Enum.Bitstamp, Exchange_Enum.Bittrex,
			Exchange_Enum.BL3P, Exchange_Enum.CEXIO, Exchange_Enum.Coinbase,
			Exchange_Enum.Digifinex, Exchange_Enum.Gemini, // not CoinEx
			Exchange_Enum.itBit, Exchange_Enum.Huobi, Exchange_Enum.Kraken,
			Exchange_Enum.KuCoin, Exchange_Enum.NDAX, Exchange_Enum.OKCoin,
			Exchange_Enum.OKEx, Exchange_Enum.Poloniex, Exchange_Enum.ThePit,
			Exchange_Enum.ZBcom,
		};
		public static bool IsCELedgerExchange(this Exchange_Enum exchange)
		{
			return CELedgerExchanges.Contains(exchange);
		}
		public static Exchange_Enum[] CELedgerExchanges => new Exchange_Enum[]
		{
			Exchange_Enum.Bittrex, Exchange_Enum.CoinEx,
		};
		public static Exchange_Enum[] EquityExchanges => new Exchange_Enum[]
		{
			Exchange_Enum.AlphaVantage, Exchange_Enum.Dukascopy, Exchange_Enum.InteractiveBrokers, 
			Exchange_Enum.IEX, Exchange_Enum.TDAmeritrade, Exchange_Enum.WRDS,
			Exchange_Enum.Discriminator_GradientEquity, Exchange_Enum.Discriminator_GradientEquityBase, Exchange_Enum.Discriminator_StandardEquity,
		};

		public static bool IsPreConfirmedByServer(this OrderStatus orderStatus)
		{
			return PreConfirmedByServer.Contains(orderStatus);
		}
		public static OrderStatus[] PreConfirmedByServer => new OrderStatus[]
		{
			OrderStatus.CreatedNotReadyToSubmit, OrderStatus.CreatedReadyToSubmit, OrderStatus.PreSubmitted, OrderStatus.Submitted, OrderStatus.PendingReceived,
			OrderStatus.Active,
		};
		public static bool IsInPlay(this OrderStatus orderStatus)
		{
			return InPlay.Contains(orderStatus);
		}
		public static OrderStatus[] InPlay => new OrderStatus[]
		{
			OrderStatus.CreatedNotReadyToSubmit, OrderStatus.CreatedReadyToSubmit, OrderStatus.PreSubmitted, OrderStatus.Submitted, OrderStatus.PendingReceived,
			OrderStatus.Active, OrderStatus.Open, OrderStatus.PartiallyFilled,  OrderStatus.PendingCancel,
		};
		public static bool IsSubmittedButNotDone(this OrderStatus orderStatus)
			=> SubmittedButNotDone.Contains(orderStatus);
		public static OrderStatus[] SubmittedButNotDone => new OrderStatus[]
		{
			OrderStatus.Submitted, OrderStatus.PendingReceived, OrderStatus.ApiPending,
			OrderStatus.Active, OrderStatus.Open, OrderStatus.PartiallyFilled,  OrderStatus.PendingCancel,
		};
		public static bool IsDone(this OrderStatus orderStatus)
		{
			return Cancelled.Contains(orderStatus)
				|| orderStatus == OrderStatus.Filled
				|| orderStatus == OrderStatus.SyntheticFill;
			// don't include PartialFilled bc that's not done yet
		}
		public static bool IsCancelled(this OrderStatus orderStatus)
			=> Cancelled.Contains(orderStatus);
		public static OrderStatus[] Cancelled => new OrderStatus[]
		{
			OrderStatus.DoneAmbiguous, OrderStatus.SyntheticFill,
			OrderStatus.Rejected, OrderStatus.Expired, 
			//, OrderStatus.InvalidStatus, OrphanPermanent should not be part of this bc it could be active
			OrderStatus.Canceled, OrderStatus.ApiCancelled, OrderStatus.InternalCancelled,
			OrderStatus.Filled, OrderStatus.Inactive
		};
		/// <summary>
		/// Should warn/be aware of, since the program shouldn't be hitting these normally
		/// </summary>
		/// <param name="orderStatus"></param>
		/// <returns></returns>
		public static bool IsWarn(this OrderStatus orderStatus)
		{
			return Warn.Contains(orderStatus);
		}
		public static OrderStatus[] Warn => new OrderStatus[]
		{ // don't include OrphanPermanent bc that is logged elsewhere
			OrderStatus.Rejected, OrderStatus.Expired,
			OrderStatus.InvalidStatus, OrderStatus.Inactive,
			OrderStatus.Canceled, // don't include ApiCancelled bc that normally happens during program operation
		};
	}
}