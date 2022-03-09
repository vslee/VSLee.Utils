using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VSLee.Utils.ExchangeBase
{ // C# byte: 0 to 255, SQL Server tinyint	0 to 255
	public enum Side : byte
	{
		Buy = 10,
		Sell = 20,
		/// <summary>
		/// IB: Sell Short as part of a combo leg
		/// </summary>
		SellShort = 30,
		/// <summary>
		/// IB: Short Sale Exempt action.
		/// SSHORTX allows some orders to be marked as exempt from the new SEC Rule 201
		/// </summary>
		SShortX = 40,
		/// <summary>
		/// TDA: separate type here
		/// </summary>
		BuyToCover = 50,
		/// <summary>
		/// TDA: 
		/// </summary>
		Exchange = 60,
		/// <summary>
		/// TDA: 
		/// </summary>
		Excercise_Option = 70,
		/// <summary>
		/// IEX Hist: always
		/// IB (Undefined)
		/// </summary>
		NotAvailable = 80,
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

		Market = 10, Limit = 20,

		/// <summary>
		/// IB: A market order that is submitted to execute as close to the closing price as possible.
		/// Non US Futures, Non US Options, Stocks
		/// </summary>
		MarketOnClose = 30,

		/// <summary>
		/// IB: An LOC (Limit-on-Close) order that executes at the closing price if the closing price is at or better than the submitted limit price, according to the rules of the specific exchange. Otherwise the order will be cancelled. 
		/// Non US Futures , Stocks
		/// </summary>
		LimitOnClose = 40,

		// TODO: reconcile OrderTypes from IB, TDA, Kraken, etc...
	}

	public enum TDAOrderType : byte
	{
		// Market
		Market = 0,
		Stop_Market_GTC = 2,
		Stop_Market_Day = 4,
		MOO_BegOfDayMarket = 6,
        Market_ConvertedFrom_MOO = 8,
		MOC_EndOfDayMarket = 10,
        Market_ConvertedFrom_MOC = 12,
        EndOfBTSessionMarket = 14,

		// Limit
		Limit_GTC = 20,
		Stop_Limit_GTC = 22,
		Stop_Limit_Day = 24,
		Limit_Day_Ext = 26,
		Limit_GTC_Ext = 28,
		Limit_Day = 30,

		// TS
		TS_Percent = 40,
		TS_Dollar = 42,

		// TD AMTD Acct deposits
		Debit = 50,
		Credit = 52,
	}

	public enum Exchange_Enum : byte
	{
		// crypto exchanges
		ApolloX = 5,
		Binance = 7,
		BinanceDEX = 8,
		BinanceJersey = 9,
		BinanceTR = 10,
		BinanceUS = 11,
		BitBlinx = 14,
		Bitfinex = 17,
		BitFlyer = 20,
		Bithumb = 23,
		BitMax = 26,
		BitMEX = 29,
		Bitstamp = 31,
		Bittrex = 34,
		BL3P = 37,
		BlockchainCom = 40,
		BTCEX = 43,
		BtcTurk = 46,
		Bybit = 49,
		CEXIO = 51,
		Chiliz = 53,
		/// <summary> coinbase pro AKA GDAX </summary>
		Coinbase = 56,
		Coincheck = 59,
		CoinEx = 61,
		Coinlist = 64,
		Coinone = 67,
		Coinzo = 70,
		Compound = 73,
		CryptoCom = 75,
		CurveFinance = 79,
		Digifinex = 81,
		Dydx = 83,
		Exante = 85,
		FTX = 87,
		FTXUS = 88,
		GateIO = 90,
		Gemini = 92,
		itBit = 94,
		HitBTC = 96,
		Huobi = 98,
		Kraken = 100,
		KuCoin = 102,
		LBank = 104,
		LiquidQuoine = 106,
		LiveCoin = 108,
		MEXC = 110,
		NDAX = 112,
		OKCoin = 114,
		OKEx = 118,
		Paribu = 120,
		Poloniex = 122,
		Tokenlon = 124,
		ThePit = 126,
		Uniswap = 128,
		Upbit = 130,
		ZBcom = 132,

		BacktestGradient = 135,
		BacktestStandard = 145,

		// stock exchanges / brokers / data sources
		Alpaca = 150,
		AlphaVantage = 154,
		Dukascopy = 158,
		IEX = 162,
		InteractiveBrokers = 166,
		NasdaqWeb = 170,
		NasdaqWebFile = 174,
		QuantQuote = 178,
		Robinhood = 182,
		TDAmeritrade = 186,
		WRDS = 190,
		YahooWeb = 194,
		YahooWebFile = 198,

		// placeholders for discriminators
		No_Exchange = 200,
		Discriminator_Base = 202,
		Discriminator_StandardBase = 204,
		Discriminator_Standard = 206,
		Discriminator_StandardDay = 208,
		Discriminator_StandardEquity = 210,
		Discriminator_StandardCrypto = 212,
		Discriminator_GradientBase = 214,
		Discriminator_Gradient = 216,
		Discriminator_GradientDay = 218,
		Discriminator_GradientEquityBase = 220,
		Discriminator_GradientEquity = 222,
		Discriminator_GradientCryptoBase = 224,
		Discriminator_GradientCrypto = 226,
		Discriminator_ESLedger = 228,
		Discriminator_BinanceBase = 230,
	}

	public enum ExpandedDiscriminator : byte
	{
		NoExpandedValue = 3,

		BacktestGradient = 135,
		BacktestStandard = 145,

		No_Exchange = 200,
		Discriminator_Base = 202,
		Discriminator_StandardBase = 204,
		Discriminator_Standard = 206,
		Discriminator_StandardDay = 208,
		Discriminator_StandardEquity = 210,
		Discriminator_StandardCrypto = 212,
		Discriminator_GradientBase = 214,
		Discriminator_Gradient = 216,
		Discriminator_GradientDay = 218,
		Discriminator_GradientEquityBase = 220,
		Discriminator_GradientEquity = 222,
		Discriminator_GradientCryptoBase = 224,
		Discriminator_GradientCrypto = 226,
		Discriminator_ESLedger = 228,
		Discriminator_BinanceBase = 230,
	}

	public enum Realm : byte
	{
		Equity = 10,
		Crypto = 20,
	}

	public enum TimeInForce : byte
	{
		/// <summary>
		/// Binance: Good 'til canceled.
		/// Coinbase: Good till canceled orders remain open on the book until
		///			  canceled. This is the default behavior if no policy is specified.
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
		/// Coinbase: Good till time orders remain open on the book until
		///			  canceled or the allotted cancel_after is depleted on the
		///			  matching engine. GTT orders are guaranteed to cancel before
		///			  any other order is processed after the cancel_after timestamp
		///			  which is returned by the API. A day is considered 24 hours.
		/// </summary>
		GTT = 20,

		/// <summary>
		/// Binance: Immediate or cancel.
		/// Coinbse: Immediate or cancel orders instantly cancel the remaining
		///			 size of the limit order instead of opening it on the book.
		/// </summary>
		IOC = 30,

		/// <summary>
		/// Binance: Fill or kill.
		/// Coinbase: Fill or kill orders are rejected if the entire size
		///			  cannot be matched.
		/// </summary>
		FOK = 40,

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
		DAY = 50,

		/// <summary>
		/// IB: Day until Canceled: similar to a day order, but instead of being canceled and removed from the trading 
		///		screen at the end of the day, the order is deactivated. This means that the order is canceled AT THE 
		///		EXCHANGE but remains on your trading screen to be re-transmitted whenever you click the Transmit button.
		/// </summary>
		DTC = 60,
	}

	public enum OrderStatus : byte
	{
		// internal program statuses (no official status on GDAX)
		/// <summary> Order was just created but has not been inserted into DB. May be combined and deleted prior to insert. </summary>
		TemporaryPriorToDBInsert = 5,
		/// <summary> in DB but waiting for entry order to execute prior to submitting normal orders </summary>
		WaitingForEntryOrder = 10,
		/// <summary> prior to submission </summary>
		CreatedReadyToSubmit = 15,
		/// <summary> prior to submission, but now out of the OrdersReadyToSubmit RBPQ </summary>
		PreSubmitted = 20,
		/// <summary>
		/// prior to GDAX acknowledgement
		/// IB (PendingSubmit, not Submitted - that maps to Open) not sent by TWS and should be
		/// explicitly set by the API developer when an order is submitted: order was
		/// sent from TWS, but confirmation has not been received that it has been
		/// received by the destination. Most commonly because exchange is closed.
		/// </summary>
		Submitted = 25,
		/// <summary>
		/// GDAX/Binance: submitted, but not accepted
		/// </summary>
		Rejected = 30,
		/// <summary> internally filled </summary>
		SyntheticFill = 35,
		/// <summary>
		/// Cancelled internally and never submitted
		/// </summary>
		CancelledInternalPriorToSubmit = 40,
		/// <summary>
		/// Only clientID no serverID and not found on server despite search.
		/// - happens when order is submitted and then disconnects before server confirmation
		/// </summary>
		OrphanPermanent = 45,
		/// <summary>
		/// + need to implement setting OrderStatus during Gradient Backtesting
		/// </summary>
		GradientBacktest_Placeholder = 50,

		// Exchange statuses
		/// <summary>
		/// GDAX: received, prior to being "Received"?
		/// IB (PreSubmitted): indicates that a simulated order type has been accepted by the IB system and
		/// that this order has yet to be elected. The order is held in the IB system
		/// (and the status remains DARK BLUE) until the election criteria are met.
		/// At that time the order is transmitted to the order destination as specified
		/// (and the order status color will change).
		/// </summary>
		PendingReceived = 60,
		/// <summary>
		/// a.k.a. PendingOpen or Received
		/// GDAX (received): same as "received" realtime state
		/// </summary>
		Active = 65,
		/// <summary>
		/// GDAX: now on the order book
		/// Binance (New)
		/// IB (Submitted): indicates that your order has been accepted at the order destination and is working.
		/// </summary>
		Open = 70,
		/// <summary>
		/// GDAX: not in GDAX
		/// Binance: order is still Open
		/// IB: same
		/// </summary>
		FilledPartiallyAndStillOpen = 75,
		/// <summary>
		/// Binance: still open but soon to be closed
		/// IB: (not sent by TWS and should be explicitly set by the API 
		/// developer when an order is submitted)  request has been sent 
		/// to cancel an order but confirmation has not been received of its cancellation.
		/// </summary>
		CancelPending = 80,
		/// <summary>
		/// GDAX/Binance: either cancelled or filled, and also either settled or not 
		/// </summary>
		DoneAmbiguous = 85,
		// settled (settled is a actually a separate boolean field in GDAX)

		/// <summary>
		/// GDAX: does not exist
		/// Binance:
		/// IB: 
		/// </summary>
		Expired = 90,

		/// <summary>
		/// does have official GDAX status, but unable to parse
		/// IB (Error, None):  No Order Status. not sent by TWS and should be explicitly set by the API developer when an error has occured
		/// </summary>
		InvalidStatus = 95, // unable to parse GDAX status

		/// <summary>
		/// GDAX: does not exist
		/// Binance:
		/// IB: indicates that the balance of your order has been confirmed canceled by the IB system.
		/// This could occur unexpectedly when IB or the destination has rejected your order.
		/// </summary>
		Canceled = 100,
		/// <summary>
		/// GDAX: does not exist
		/// Binance:
		/// IB: The order has been completely filled.
		/// </summary>
		Filled = 105,
		/// <summary>
		/// was pending cancel but got filled in the meantime, so cancel failed
		/// </summary>
		FilledFullyFailedCancel = 110,
		/// <summary>
		/// was pending cancel but got filled (partially) in the meantime, so cancel (partially) failed
		/// </summary>
		FilledPartiallyThenCancelled = 115,
		/// <summary>
		/// Filled while program was offline so did not catch fill
		/// - this is caught when market conditions mean the order ought to have been filled
		/// </summary>
		FilledOfflineUncomfirmed = 120,
		/// <summary>
		/// Previous FilledOfflineUnconfirmed that has now been confirmed using trade history
		/// </summary>
		FilledOfflineConfirmed = 125,
		/// <summary>
		/// GDAX: does not exist
		/// Binance: does not exist
		/// IB: indicates an order is not working, possible reasons include:
		/// - it is invalid or triggered an error. A corresponding error code is expected to the error() function.
		/// - the order is to short shares but the order is being held while shares are being located.
		/// - an order is placed manually in TWS while the exchange is closed.
		/// - an order is blocked by TWS due to a precautionary setting and appears there in an untransmitted state
		/// </summary>
		Inactive = 130,
		/// <summary>
		/// GDAX: does not exist
		/// Binance: does not exist
		/// IB (ApiPending): indicates order has not yet been sent to IB server, for instance 
		/// if there is a delay in receiving the security definition. Uncommonly received.
		/// </summary>
		PendingInApi = 135,
		/// <summary>
		/// GDAX: does not exist
		/// Binance: does not exist
		/// IB (ApiCancelled): after an order has been submitted and before it has been acknowledged, 
		/// an API client can request its cancellation, producing this state.
		/// </summary>
		CancelledByApi = 140,
	}

	public enum DoneReason : Byte
	{
		Filled = 10,
		Cancelled = 20,
		CancelledOrphan = 30,
		CancelledManually = 40,
		/// <summary> provided only by IB </summary>
		CancelledByTrader = 50,
		CancelledByBroker = 60,
		CancelledByBrokerMarginInitial = 70,
		CancelledByBrokerRegT = 80,
		CancelledByBrokerLiquidation = 90,
		/// <summary> ultimately considered Rejected, so that's what the OrderStatus is </summary>
		CancelledByTraderThenRejected = 100,
		/// <summary>
		/// in Binance/IB (not in Coinbase)
		/// Reason is stored in a separate enum: OrderRejectedReason
		/// </summary>
		Rejected = 110,
		/// <summary>
		/// in Binance/IB (not in Coinbase)
		/// </summary>
		Expired = 120,
		/// <summary>
		/// this DoneReason is set before the cancellation is even sent to the broker/exchange
		/// </summary>
		NoLongerReadyCancelledByTraderDB = 130,
		CombinedWithNewOrders = 140,
		CombinedWithNewOrdersAndPriceOverridden = 150,
		InternalMatched = 160,
	}
	public enum NEPLStatus : byte
	{
		Not_Existent = 10,
		Testing = 20,
		Tested_NoEPCs = 30,
		Tested_EPCs = 40,
	}

	public enum OHLC_Flag : byte
	{
		// Extended Hours
		Pre_Market = 5, Post_Market = 10,

		// Open
		Open_OnTime = 20, Open_Late = 25, Open_Very_Late = 25,

		// Intraday
		Intraday = 40, Probable_Bad = 45,
		/// <summary> daily is for 1 Day resolution OHLCs </summary>
		Daily = 50,

		// Close
		Close_Very_Early = 70, Close_Early = 75, Close_OnTime = 80, Close_Late = 85,
		Close_Very_Early_Holiday = 90, Close_Early_Holiday = 95, Close_OnTime_Holiday = 100, Close_Late_Holiday = 105,
	}

	public enum CPRStatus : byte
	{
		No_search_attempted = 10,
		Found_DefaultSearch = 20,
		Not_Found_Results_TooLow = 30,
		Not_Found_Results_TooHigh = 40,
		Not_Found_Depth_TooLow = 50,
		Not_Found_No_Data = 60,
	}

	public enum CPR_DRS_Status : byte
	{
		No_search_attempted = 10,
		Searching = 20,
		Full_Found = 30,
		Partial_Found = 40,
		None_Found = 50,
	}

	public enum CPElement : byte
	{
		Bool1 = 10,
		Bool2 = 20,
		Int1 = 30,
		Int2 = 40,
		Int3 = 50,
		Double1 = 60,
		Double2 = 70,
		Double3 = 80,
	}

	public enum CPR_DRS_CPT_Status : byte
	{
		No_search_attempted = 10,
		Full_Parted = 20,
		Partial_Parted = 30,
		Unable_To_Part = 40,
	}

	public enum BFS_Test_Status : byte
	{
		No_search_attempted = 10,
		SEs_Created = 20,
		BEs_Tested = 30,
		SEs_Statted = 40,
		EPLs_ECed = 50,
	}

	public enum OrderTypePattern : byte
	{   // order rules for OCA
		//· expire must be the same for both legs 
		//· ordtype cannot be Market for either leg 
		//· symbol + action + ordertype cannot be the exact same for both legs
		MEntry_Day__MStop_GTC = 10,
		LEntry_Day__MStop_GTC = 20,
		MEntry_Day__LStop_GTC = 30,
		LEntry_Day__LStop_GTC = 40,
		MEntry_Day__MOCExit = 50,
		MOCEntry__M_DayExit = 60,
		// + probably need to add a MOCEntry__MOCExit,
	}

	public static partial class EBEnumExtensionMethods
	{
		public static bool IsExt(this TDAOrderType orderType)
		{
			return Exts.Contains(orderType);
		}

		public static bool IsMarket(this TDAOrderType orderType)
		{
			return Markets.Contains(orderType);
		}

		public static bool IsMO(this TDAOrderType orderType)
		{
			return MOs.Contains(orderType);
		}

		public static TDAOrderType[] Exts
		{
			get { return new TDAOrderType[] {TDAOrderType.Limit_Day_Ext, TDAOrderType.Limit_GTC_Ext}; }
		}

		public static TDAOrderType[] Markets
		{
			get
			{
				return new TDAOrderType[]
				{
					TDAOrderType.Market, TDAOrderType.MOO_BegOfDayMarket, TDAOrderType.MOC_EndOfDayMarket,
                    TDAOrderType.Market_ConvertedFrom_MOO, TDAOrderType.Market_ConvertedFrom_MOC,
					TDAOrderType.Stop_Market_Day, TDAOrderType.Stop_Market_GTC
				};
			}
		}

        public static TDAOrderType[] MOs
            => new TDAOrderType[] { TDAOrderType.MOO_BegOfDayMarket, TDAOrderType.MOC_EndOfDayMarket,
                    TDAOrderType.Market_ConvertedFrom_MOO, TDAOrderType.Market_ConvertedFrom_MOC, };

		/// <summary>
		/// Entry is either MOC or EOD
		/// </summary>
		/// <param name="orderTypePattern"></param>
		/// <returns></returns>
		public static bool InvolvesEODEntry(this OrderTypePattern orderTypePattern)
		{
			return EODEntrys.Contains(orderTypePattern);
		}

		/// <summary>
		/// Exit is either MOC or EOD
		/// </summary>
		/// <param name="orderTypePattern"></param>
		/// <returns></returns>
		public static bool InvolvesEODExit(this OrderTypePattern orderTypePattern)
		{
			return EODExits.Contains(orderTypePattern);
		}

		public static OrderTypePattern[] EODEntrys
		{
			get { return new OrderTypePattern[] {OrderTypePattern.MEntry_Day__MOCExit, OrderTypePattern.MOCEntry__M_DayExit}; }
		}
		public static OrderTypePattern[] EODExits
		{
			get { return new OrderTypePattern[] { OrderTypePattern.MEntry_Day__MOCExit, OrderTypePattern.MOCEntry__M_DayExit }; }
		}

		public static TDAOrderType EntryType(this OrderTypePattern orderTypePattern)
		{
			if (orderTypePattern == OrderTypePattern.MEntry_Day__MStop_GTC || orderTypePattern == OrderTypePattern.MEntry_Day__LStop_GTC || orderTypePattern == OrderTypePattern.MEntry_Day__MOCExit)
				return TDAOrderType.Market;
			else if (orderTypePattern == OrderTypePattern.LEntry_Day__MStop_GTC || orderTypePattern == OrderTypePattern.LEntry_Day__LStop_GTC)
				return TDAOrderType.Limit_GTC;
			else // if (orderTypePattern == OrderTypePattern.MOC__MEntry_Day)
				return TDAOrderType.MOC_EndOfDayMarket;
		}

		public static TDAOrderType StopType(this OrderTypePattern orderTypePattern)
		{
			// + change MO and OM OTPs to have ExitType output instead of StopType
			if (orderTypePattern == OrderTypePattern.MEntry_Day__MStop_GTC || orderTypePattern == OrderTypePattern.LEntry_Day__MStop_GTC)
				return TDAOrderType.Stop_Market_GTC;
			else if (orderTypePattern == OrderTypePattern.MEntry_Day__LStop_GTC || orderTypePattern == OrderTypePattern.LEntry_Day__LStop_GTC)
				return TDAOrderType.Stop_Limit_GTC;
			else //	orderTypePattern == OrderTypePattern.MEntry_Day__MOCExit || orderTypePattern == OrderTypePattern.MOCEntry__M_DayExit)
				throw new Exception("Should not be using StopType() on this type of OTP, likly using OTOCA incorrectly");
		}

		public static TDAOrderType ExitType(this OrderTypePattern orderTypePattern)
		{
			if (orderTypePattern == OrderTypePattern.MEntry_Day__MOCExit)
				return TDAOrderType.MOC_EndOfDayMarket;
			else if (orderTypePattern == OrderTypePattern.MOCEntry__M_DayExit)
				return TDAOrderType.Market;
			else
				return TDAOrderType.Limit_GTC; // seems like the rest are all just LimitGTC
		}

		public static String ShortName(this OrderTypePattern orderTypePattern)
		{
			switch (orderTypePattern)
			{
				case OrderTypePattern.MEntry_Day__MStop_GTC:
					return "MM";
				case OrderTypePattern.LEntry_Day__MStop_GTC:
					return "LM";
				case OrderTypePattern.MEntry_Day__LStop_GTC:
					return "ML";
				case OrderTypePattern.LEntry_Day__LStop_GTC:
					return "LL";
				case OrderTypePattern.MEntry_Day__MOCExit:
					return "MO";
				case OrderTypePattern.MOCEntry__M_DayExit:
					return "OM";
				default:
					return "";
			}
		}

		public static Exchange_Enum ParseExchangeEnum(this string exchangeString)
		{
			if (Enum.TryParse<Exchange_Enum>(exchangeString, true, out var exchange))
				return exchange;
			else switch (exchangeString.ToLowerInvariant())
				{
					//case "binance": return Exchange_Enum.Binance;
					//case "binancedex": return Exchange_Enum.BinanceDEX;
					//case "binancejersey": return Exchange_Enum.BinanceJersey;
					//case "binanceus": return Exchange_Enum.BinanceUS;
					//case "bitblinx": return Exchange_Enum.BitBlinx;
					//case "bitfinex": return Exchange_Enum.Bitfinex;
					//case "bitflyer": return Exchange_Enum.BitFlyer;
					//case "bithumb": return Exchange_Enum.Bithumb;
					//case "bitmex": return Exchange_Enum.BitMEX;
					//case "bitstamp": return Exchange_Enum.Bitstamp;
					//case "bittrex": return Exchange_Enum.Bittrex;
					//case "bl3p": return Exchange_Enum.BL3P;
					//case "cexio": return Exchange_Enum.CEXIO;
					case "coinbase": // will automatically fall through to gdax
					case "coinbasepro": // will automatically fall through to gdax
					case "gdax": return Exchange_Enum.Coinbase;
					//case "coinex": return Exchange_Enum.CoinEx;
					//case "digifinex": return Exchange_Enum.Digifinex;
					//case "gemini": return Exchange_Enum.Gemini;
					//case "itbit": return Exchange_Enum.itBit;
					//case "hitbtc": return Exchange_Enum.HitBTC;
					//case "huobi": return Exchange_Enum.Huobi;
					//case "kraken": return Exchange_Enum.Kraken;
					//case "kucoin": return Exchange_Enum.KuCoin;
					//case "lbank": return Exchange_Enum.LBank;
					//case "ndax": return Exchange_Enum.NDAX;
					//case "okcoin": return Exchange_Enum.OKCoin;
					//case "okex": return Exchange_Enum.OKEx;
					//case "poloniex": return Exchange_Enum.Poloniex;
					//case "thepit": return Exchange_Enum.ThePit;
					//case "zbcom": return Exchange_Enum.ZBcom;
					//case "discriminator_gradientequity": return Exchange_Enum.Discriminator_GradientEquity;
					default: throw new NotImplementedException("unable to parse exchangeString: " + exchangeString);
				}
		}
		public static Realm GetRealm(this Exchange_Enum exchange)
		{
			if (CryptoDiscriminators.Contains(exchange)
				|| ESLedgerExchanges.Contains(exchange)
				|| CELedgerExchanges.Contains(exchange)) return Realm.Crypto;
			if (EquityExchanges.Contains(exchange)) return Realm.Equity;
			throw new NotImplementedException($"Unexpected exchange in GetRealm() + {exchange}");
		}

		public static Exchange_Enum[] CryptoDiscriminators => new Exchange_Enum[]
		{
			Exchange_Enum.Discriminator_GradientCrypto, Exchange_Enum.Discriminator_GradientCryptoBase, Exchange_Enum.Discriminator_StandardCrypto,
		};
		public static bool IsESLedgerExchange(this Exchange_Enum exchange)
		{
			return ESLedgerExchanges.Contains(exchange);
		}
		public static Exchange_Enum[] ESLedgerExchanges => new Exchange_Enum[]
		{
			Exchange_Enum.ApolloX, Exchange_Enum.Binance, Exchange_Enum.BinanceDEX,
			Exchange_Enum.BinanceJersey, Exchange_Enum.BinanceUS, Exchange_Enum.BitBlinx,
			Exchange_Enum.Bitfinex, Exchange_Enum.BitFlyer, Exchange_Enum.Bithumb,
			Exchange_Enum.BitMEX, Exchange_Enum.Bitstamp, Exchange_Enum.Bittrex,
			Exchange_Enum.BL3P, Exchange_Enum.Bybit, Exchange_Enum.CEXIO,
			Exchange_Enum.Coinbase, Exchange_Enum.CryptoCom, Exchange_Enum.Digifinex,
			Exchange_Enum.Dydx, Exchange_Enum.FTX, Exchange_Enum.FTXUS,
			Exchange_Enum.GateIO, Exchange_Enum.Gemini, Exchange_Enum.itBit,
			Exchange_Enum.HitBTC, Exchange_Enum.Huobi, Exchange_Enum.Kraken,
			Exchange_Enum.KuCoin, Exchange_Enum.LBank, Exchange_Enum.NDAX,
			Exchange_Enum.OKCoin, Exchange_Enum.OKEx, Exchange_Enum.Poloniex,
			Exchange_Enum.ThePit, Exchange_Enum.ZBcom,
		};
		public static bool IsCELedgerExchange(this Exchange_Enum exchange)
		{
			return CELedgerExchanges.Contains(exchange);
		}
		public static Exchange_Enum[] CELedgerExchanges => new Exchange_Enum[]
		{
			Exchange_Enum.BitMax, Exchange_Enum.Bittrex, Exchange_Enum.BtcTurk, Exchange_Enum.Chiliz, Exchange_Enum.CoinEx, Exchange_Enum.Coinzo, Exchange_Enum.LiveCoin, Exchange_Enum.Paribu,
		};
		public static Exchange_Enum[] EquityExchanges => new Exchange_Enum[]
		{
			Exchange_Enum.AlphaVantage, Exchange_Enum.Dukascopy, Exchange_Enum.InteractiveBrokers, 
			Exchange_Enum.IEX, Exchange_Enum.TDAmeritrade, Exchange_Enum.WRDS,
			Exchange_Enum.Discriminator_GradientEquity, Exchange_Enum.Discriminator_GradientEquityBase, Exchange_Enum.Discriminator_StandardEquity,
		};

		public static bool IsPreSubmitted(this OrderStatus orderStatus)
		{
			return PreSubmitted.Contains(orderStatus);
		}
		public static OrderStatus[] PreSubmitted => new OrderStatus[]
		{
			OrderStatus.TemporaryPriorToDBInsert, OrderStatus.WaitingForEntryOrder,
			OrderStatus.CreatedReadyToSubmit, OrderStatus.PreSubmitted,
		};
		public static bool IsSubmittedButNotDone(this OrderStatus orderStatus)
			=> orderStatus.IsSubmittedButPreConfirmedByServer() || orderStatus.IsReceivedButNotDone();
		public static bool IsSubmittedButPreConfirmedByServer(this OrderStatus orderStatus)
		{
			return SubmittedButPreConfirmedByServer.Contains(orderStatus);
		}
		public static OrderStatus[] SubmittedButPreConfirmedByServer => new OrderStatus[]
		{
			OrderStatus.Submitted, OrderStatus.PendingReceived,	OrderStatus.PendingInApi,
		};
		public static bool IsReceivedButNotDone(this OrderStatus orderStatus)
			=> ReceivedButNotDone.Contains(orderStatus);
		public static OrderStatus[] ReceivedButNotDone => new OrderStatus[]
		{
			OrderStatus.Active, OrderStatus.Open, OrderStatus.FilledPartiallyAndStillOpen,  OrderStatus.CancelPending,
			OrderStatus.Inactive, // not currently active but can probably be re-activated
		};
		public static bool IsDone(this OrderStatus orderStatus)
			=> orderStatus.IsFilled() || orderStatus.IsCancelled()
			|| orderStatus == OrderStatus.OrphanPermanent || orderStatus == OrderStatus.DoneAmbiguous;
		public static bool IsFilled(this OrderStatus orderStatus)
			=> Filled.Contains(orderStatus);
		public static OrderStatus[] Filled => new OrderStatus[]
		{
			OrderStatus.Filled,
			OrderStatus.SyntheticFill,
			OrderStatus.FilledFullyFailedCancel,
			OrderStatus.FilledOfflineUncomfirmed,
			OrderStatus.FilledOfflineConfirmed,
			// don't include FilledPartiallyAndStillOpen bc that's not done yet
			OrderStatus.FilledPartiallyThenCancelled,
		};
		public static bool IsCancelled(this OrderStatus orderStatus)
			=> Cancelled.Contains(orderStatus);
		public static OrderStatus[] Cancelled => new OrderStatus[]
		{
			OrderStatus.SyntheticFill,
			OrderStatus.Rejected, OrderStatus.Expired, 
			//, OrderStatus.InvalidStatus, OrphanPermanent should not be part of this bc it could be active
			OrderStatus.Canceled, OrderStatus.CancelledByApi, OrderStatus.CancelledInternalPriorToSubmit,
			// OrderStatus.Inactive don't include inactive, bc these can probably be re-activated
			OrderStatus.FilledPartiallyThenCancelled,
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
			OrderStatus.FilledOfflineUncomfirmed,
			OrderStatus.DoneAmbiguous,
			OrderStatus.TemporaryPriorToDBInsert,
		};
	}
}