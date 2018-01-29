using System;
using System.Collections.Generic;
using System.Text;

namespace VSLee.Utils.ExchangeBase
{
	public enum Side { Buy, Sell }

	public enum OrderType
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
	public static partial class EnumExtensionMethods
	{
		public static Side OpposingSide(this Side side)
		{
			if (side == Side.Buy)
				return Side.Sell;
			else
				return Side.Buy;
		}
	}

	public enum Exchange_Enum : byte
	{
		GDAX = 10,
		Binance = 11
	}
}
