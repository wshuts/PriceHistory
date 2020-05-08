using System;
using System.Collections;

namespace PriceHistory
{
	/// <summary>
	/// Summary description for Fund.
	/// </summary>
	public class Fund
	{
		private string _fundName;
		private string _fundNumber;
		private string _startDate;
		private string _endDate;
		private IDictionary _priceInfo=new SortedList();
        private IList _vector = new ArrayList();
        private double _mean;
        private double _standardDeviation;
        private IDictionary _correlationCoefficients = new SortedList();

		public Fund()
		{
		}
		
		public Fund(string fundName,string fundNumber)
		{
			_fundName=fundName;
			_fundNumber=fundNumber;
		}
		
		public string FundNumber
		{
			get{return _fundNumber;}
		}
		
		public string FundName
		{
			get{return _fundName;}
		}
		
		public string StartDate
		{
			get
			{
				_startDate=(string)((SortedList)_priceInfo).GetKey(0);
				return _startDate;
			}
		}
		
		public string EndDate
		{
			get
			{
				_endDate=(string)((SortedList)_priceInfo).GetKey(_priceInfo.Count-1);
				return _endDate;
			}
		}
		
		public IDictionary PriceInfo
		{
			get{return _priceInfo;}
		}

        public IDictionary CorrelationCoefficients
		{
			get{return _correlationCoefficients;}
		}

		public IList Vector
		{
			get
			{
                //TODO: initialize _vector properly i.e. only once after _priceInfo is initialized
				_vector.Clear();
				foreach(string price in _priceInfo.Values)
				{
					_vector.Add(Double.Parse(price));
				}
				return _vector;
			}
		}

		public void CalculateStatistics()
		{
			CalculateMean();
			CalculateStandardDeviation();
		}

		private void CalculateMean()
		{
			_mean=Statistics.Mean(Vector);
		}
		
		private void CalculateStandardDeviation()
		{
			_standardDeviation=Statistics.StandardDeviation(Vector);
		}
	}
}
