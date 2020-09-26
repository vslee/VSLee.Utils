using System;
using System.Collections.Generic;
using System.Text;

namespace VSLee.Utils.ExchangeBase
{
	public interface IOrderBook<TPrice>
	{
		TPrice Spread { get; }

		// + ideally would do VWAP

		TPrice MidpointPrice { get; }

		TPrice BestBuyPrice { get; }

		TPrice BestSellPrice { get; }

		// + public TPrice GetBestOrderPrice(Side side) => side == Side.Buy ? BestBuyPrice : BestSellPrice; activate after merge
	}
}
