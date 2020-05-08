using System;

namespace PriceHistory
{
	/// <summary>
	/// Summary description for CorrelationCoefficient.
	/// </summary>
	public class CorrelationCoefficient
	{
		private string _fundName;
		private double _correlationCoefficient;

		public CorrelationCoefficient(string fundName,double correlationCoefficient)
		{
			_fundName=fundName;
			_correlationCoefficient=correlationCoefficient;
		}
		
		public string FundName
		{
			get{return _fundName;}
		}
		
		public double CorrelationCoeff
		{
			get{return _correlationCoefficient;}
		}
	}
}
