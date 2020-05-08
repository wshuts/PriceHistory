using System;

namespace PriceHistory
{
	/// <summary>
	/// Summary description for FundInfo.
	/// TODO: Delete class if ultimately not needed
	/// </summary>
	public class FundInfo
	{
		private string _fundName;
		private string _fundNumber;
		
		public FundInfo(string fundName, string fundNumber)
		{
			_fundName=fundName;
			_fundNumber=fundNumber;
		}
		
		public string FundName
		{
			get{return _fundName;}
		}
		
		public string FundNumber
		{
			get{return _fundNumber;}
		}
	}
}
