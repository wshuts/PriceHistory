using System;
using System.Collections;
using System.IO;

namespace PriceHistory
{
	/// <summary>
	/// Summary description for Controller.
	/// </summary>
	public class Controller
	{
		private PriceServer _priceServer;
		private IDictionary _fundTable=new SortedList();
		private IList _selectedFunds=new ArrayList();
		private DateTime _startDate;
		private DateTime _endDate;

		public Controller()
		{
            _priceServer = new AdjustedPriceServer();
		}
		
		public IDictionary FundTable
		{
			get{return _fundTable;}
		}
		
		public DateTime StartDate
		{
			set{_startDate=value;}
		}
		
		public DateTime EndDate
		{
			set{_endDate=value;}
		}

		public void InitializeFundTable()
		{
			//Get table of available funds from price server
			_priceServer.GetFundTable();

			//Update _fundTable
			_fundTable.Clear();
			foreach(DictionaryEntry dE in _priceServer.FundTable)
			{
				_fundTable.Add(dE.Key,dE.Value);
			}

            //Remove Money Market Funds from _fundTable i.e. StandardDeviation=0
            RemoveMoneyMarketFunds();
       }

        private void RemoveMoneyMarketFunds()
        {
            ArrayList keysToRemove = new ArrayList();
            foreach (string fundName in _fundTable.Keys)
            {
                if (fundName.IndexOf("Money Mkt") != -1) keysToRemove.Add(fundName);
            }

            foreach (string fundName in keysToRemove)
            {
                _fundTable.Remove(fundName);
            }
        }

		public void UpdateSelectedFunds(IList selectedItems)
		{
			_selectedFunds.Clear();
			foreach(string fundname in selectedItems)
			{
				_selectedFunds.Add(new Fund(fundname,(string)_fundTable[fundname]));
			}
		}

		public void Calculate()
		{
			//Retrieve price info
			RetrievePriceInfo();

            //TODO
			//Validate price info
            //ValidatePriceInfo();
			
			//Calculate correlations
			CalculateCorrelations();

			//Generate output file
			GenerateOutputFile();
		}

		public void RetrievePriceInfo()
		{
			foreach(Fund fund in _selectedFunds)
			{
				string fundNumber=fund.FundNumber;
				_priceServer.RetrievePriceInfo(fundNumber,_startDate,_endDate);
				
				//Update fund.PriceInfo
				fund.PriceInfo.Clear();		
				foreach(DictionaryEntry dE in _priceServer.PriceInfo)
				{
					fund.PriceInfo.Add(dE.Key,dE.Value);
				}
			}
		}

        public void ValidatePriceInfo()
        {
            //Case: PriceInfo SortedList is empty

            //Case: StartDate and EndDate do not match requested dates
            ArrayList fundsToRemove = new ArrayList();
            foreach (Fund fund in _selectedFunds)
            {
                bool temp3;
                bool temp4;
                string temp1 = _startDate.ToString("MM/dd/yyyy");
                string temp2 = _endDate.ToString("MM/dd/yyyy");
                if (temp1 == fund.StartDate) temp3 = true;
                if (temp2 == fund.EndDate) temp4 = true;
                break;
                //if (fund.PriceInfo.IndexOf("Money Mkt") != -1) fundsToRemove.Add(fund);
            }

            foreach (Fund fund in fundsToRemove)
            {
                _selectedFunds.Remove(fund);
            }
        }

		public void CalculateCorrelations()
		{
			foreach(Fund firstfund in _selectedFunds)
			{
                firstfund.CorrelationCoefficients.Clear();
                foreach(Fund secondfund in _selectedFunds)
				{
					double correlationCoefficient=0;
					correlationCoefficient=Statistics.Correlation(firstfund.Vector,secondfund.Vector);
					firstfund.CorrelationCoefficients.Add(secondfund.FundName,correlationCoefficient);
				}
			}
		}		
		
		public void GenerateOutputFile()
		{
			// Create an instance of StreamWriter to write text to a file.
			// The using statement also closes the StreamWriter.
			using (StreamWriter sw = new StreamWriter("CorrelationCoefficients.xls")) 
			{
				//Date range
                sw.WriteLine("\t"+_startDate.ToShortDateString()+"\t"+_endDate.ToShortDateString());
				
                //column headings
				foreach(Fund fund in _selectedFunds)
				{
					sw.Write("\t"+fund.FundName);
				}
				sw.Write("\n");

                //row information
				foreach(Fund fund in _selectedFunds)
				{
					sw.Write(fund.FundName);
                    foreach (double correlationCoefficient in fund.CorrelationCoefficients.Values)
					{
                        sw.Write("\t" + correlationCoefficient.ToString("0.00"));
					}
					sw.Write("\n");
				}
			}
		}
	}
}