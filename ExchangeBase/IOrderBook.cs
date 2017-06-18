using System;
using System.Collections.Generic;
using System.Text;

namespace VSLee.Utils.ExchangeBase
{
	public interface IOrderBook<T>
	{
		T Spread { get; }

		T Midpoint { get; }

		T BestBuy { get; }

		T BestSell { get; }
	}
}
