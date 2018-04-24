using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VSLee.Utils.ExchangeBase
{
	public enum Side : byte { Buy, Sell }
	public static partial class EBEnumExtensionMethods
	{
		public static Side OpposingSide(this Side side)
		{
			if (side == Side.Buy)
				return Side.Sell;
			else
				return Side.Buy;
		}
	}

	public enum OrderType : byte
	{
		/// Kraken
		/// market
		/// limit(price = limit price)
		/// stop-loss(price = stop loss price)
		/// take-profit(price = take profit price)
		/// stop-loss-profit(price = stop loss price, price2 = take profit price)
		/// stop-loss-profit-limit(price = stop loss price, price2 = take profit price)
		/// stop-loss-limit(price = stop loss trigger price, price2 = triggered limit price)
		/// take-profit-limit(price = take profit trigger price, price2 = triggered limit price)
		/// trailing-stop(price = trailing stop offset)
		/// trailing-stop-limit(price = trailing stop offset, price2 = triggered limit offset)
		/// stop-loss-and-limit(price = stop loss price, price2 = limit price)
		/// settle-position

		Market, Limit
	}

	public enum Exchange_Enum : byte
	{
		Binance = 5,
		GDAX = 10,
		Kraken = 15,
		Poloniex = 20,

		Backtest = 30, // + just one backtest for now, but may need to create separate ones in the future for diff data sources
	}

	public enum TimeInForce : byte
	{
		/// <summary>
		/// Good 'til canceled.
		/// </summary>
		GTC = 10,

		/// <summary>
		/// Immediate or cancel.
		/// </summary>
		IOC = 20,

		/// <summary>
		/// Fill or kill.
		/// </summary>
		FOK = 30
	}

	public enum OrderStatus : byte
	{
		// internal program statuses (no official status on GDAX)
		Created, // prior to submission
		Submitted, // prior to GDAX acknowledgement
		/// <summary>
		/// GDAX/Binance: submitted, but not accepted
		/// </summary>
		Rejected,
		SyntheticFill, // internally filled

		// Exchange statuses
		PendingReceived, // received by GDAX, prior to being "Received"?
		/// <summary>
		/// GDAX: same as "received" realtime state
		/// </summary>
		Active,
		Open, // now on the order book on GDAX (status of New on Binance)
		/// <summary>
		/// only in Binance, order is still Open
		/// </summary>
		PartiallyFilled,
		/// <summary>
		/// Binance: still open but soon to be closed
		/// </summary>
		PendingCancel,
		/// <summary>
		/// GDAX/Binance: either cancelled or filled, and also either settled or not 
		/// </summary>
		Done,
	    // settled (settled is a actually a separate boolean field in GDAX)

		/// <summary>
		/// Binance only
		/// </summary>
		Expired,

		/// <summary>
		/// does have official GDAX stautus, but unable to parse
		/// </summary>
		InvalidStatus, // unable to parse GDAX status
	}

	public static partial class EBEnumExtensionMethods
	{
		public static bool IsInPlay(this OrderStatus orderStatus)
		{
			return InPlay.Contains(orderStatus);
		}
		public static OrderStatus[] InPlay => new OrderStatus[]
		{
			OrderStatus.Created, OrderStatus.Submitted, OrderStatus.PendingReceived,
			OrderStatus.Active, OrderStatus.Open, OrderStatus.PartiallyFilled,  OrderStatus.PendingCancel,
		};
		public static bool IsSubmittedButNotDone(this OrderStatus orderStatus)
		{
			return SubmittedButNotDone.Contains(orderStatus);
		}
		public static OrderStatus[] SubmittedButNotDone => new OrderStatus[]
		{
			OrderStatus.Submitted, OrderStatus.PendingReceived,
			OrderStatus.Active, OrderStatus.Open, OrderStatus.PartiallyFilled,  OrderStatus.PendingCancel,
		};
		public static bool IsDone(this OrderStatus orderStatus)
		{
			return Done.Contains(orderStatus);
		}
		public static OrderStatus[] Done => new OrderStatus[]
		{
			OrderStatus.Done, OrderStatus.SyntheticFill,
			OrderStatus.Rejected, OrderStatus.Expired, //, OrderStatus.InvalidStatus should not be part of this bc it could be active
		};
	}}
