using System;
using System.Collections;
using System.Net;
using System.IO;
using System.Security.Authentication;

namespace PriceHistory
{
	/// <summary>
	/// Summary description for PriceServer.
	/// </summary>
	public class PriceServer
	{
		private WebProxy _proxy;
		private IDictionary _fundTable=new SortedList();
		private IDictionary _priceInfo=new SortedList();

		public PriceServer()
		{
            _proxy = new WebProxy();
            
            //Needed for NSN WebProxy
            //_proxy=new WebProxy("172.16.42.40:8080");
		}
		
		public IDictionary FundTable
		{
			get{return _fundTable;}
		}
		
		public IDictionary PriceInfo
		{
			get{return _priceInfo;}
		}

		public void GetFundTable()
		{
			string requestUri;
			string responseFromServer;
			requestUri=BuildQuery();
			responseFromServer=ReadFromWeb(requestUri);
			ParseFundTable(responseFromServer);
		}
		
		public string BuildQuery()
		{
			string requestUri;
			string absolutePath="https://personal.vanguard.com/us/funds/tools/pricehistorysearch";
            string Sc = "?Sc=1";
			requestUri =absolutePath+Sc;
			return requestUri;
		}
		
		public string BuildQuery(string fundId,DateTime DTstartDate,DateTime DTendDate)
		{
			string requestUri;
			string absolutePath="https://personal.vanguard.com/us/funds/tools/pricehistorysearch";
			string radio="?radio=1";
			string results="&results=get";
			string FundType="&FundType=VanguardFunds";
			string FundIntExt="&FundIntExt=INT";
			string FundId="&FundId="+fundId;
			string Sc="&Sc=1";
			string fundName="&fundName="+fundId;
			string fundValue="&fundValue="+fundId;
			string radiobutton2="&radiobutton2=1";
			string beginDate="&beginDate="+DTstartDate.Month+"%2F"+DTstartDate.Day+"%2F"+DTstartDate.Year;
			string endDate="&endDate="+DTendDate.Month+"%2F"+DTendDate.Day+"%2F"+DTendDate.Year;
			requestUri=absolutePath+radio+results+FundType+FundIntExt+FundId+Sc+fundName+fundValue+radiobutton2+beginDate+endDate;
			return requestUri;
		}
		
		public string ReadFromWeb(string requestUri)
		{
            const SslProtocols _Tls12 = (SslProtocols)0x00000C00;
            const SecurityProtocolType Tls12 = (SecurityProtocolType)_Tls12;
            ServicePointManager.SecurityProtocol = Tls12;

			// Create a request for the URL.         
			WebRequest request = WebRequest.Create (requestUri);
			
			// If required by the server, set the proxy.
			request.Proxy=_proxy;
			
			// If required by the server, set the credentials.
			request.Credentials = CredentialCache.DefaultCredentials;

			// Get the response.
            request.Timeout = 10000;
			HttpWebResponse response = (HttpWebResponse)request.GetResponse ();
			
			// Display the status if needed.
			//MessageBox.Show (response.StatusDescription);
			
			// Get the stream containing content returned by the server.
			Stream dataStream = response.GetResponseStream ();
			
			// Open the stream using a StreamReader for easy access.
			StreamReader reader = new StreamReader (dataStream);
			
			// Read the content.
			string responseFromServer = reader.ReadToEnd ();
			
			//for debug: check for consistent response length
			//int length=responseFromServer.Length;

			// Cleanup the streams and the response.
			reader.Close ();
			dataStream.Close ();
			response.Close ();

			return responseFromServer;
		}

		public void ParseFundTable(string responseFromServer)
		{
			string[] parsedStrings;
			string[] chunks;
			IList fundtablelines=new ArrayList();

			//split responseFromServer: newline as separator
			parsedStrings=responseFromServer.Split('\n');

			//populate fundtablelines with parsedStrings having "</option>"
			fundtablelines.Clear();
			foreach(string line in parsedStrings)
			{
				if(line.IndexOf("</option>")!=-1)
				{
					fundtablelines.Add(line);
				}
			}
			
			//remove first 5 lines (not fund table info)
			for(int count=0;count<5;count++)
			{
				if(fundtablelines.Count>0)fundtablelines.RemoveAt(0);
			}
			
			_fundTable.Clear();
			foreach(string line in fundtablelines)
			{
				chunks=line.Split('\"','>','<');
				_fundTable.Add(chunks[4],chunks[2]);
			}
		}

		public void RetrievePriceInfo(string fundNumber,DateTime startDate,DateTime endDate)
		{
			string requestUri;
			string responseFromServer;
			requestUri=BuildQuery(fundNumber,startDate,endDate);
			responseFromServer=ReadFromWeb(requestUri);
			ParsePriceInfo(responseFromServer);
		}
		
		public void ParsePriceInfo(string responseFromServer)
		{
			string[] parsedStrings;
			string[] chunks;
			IList pricelines=new ArrayList();

			//split responseFromServer: newline as separator
			parsedStrings=responseFromServer.Split('\n');
			
			//populate pricelines with parsedStrings having "$" and "%"
			pricelines.Clear();
			foreach(string line in parsedStrings)
			{
				if(line.IndexOf("$")!=-1&&(line.IndexOf("%")!=-1||line.IndexOf("&#8212;")!=-1))
				{
					pricelines.Add(line);
				}
			}

			_priceInfo.Clear();
			foreach(string line in pricelines)
			{
				chunks=line.Split('$','>','<');
				_priceInfo.Add(chunks[4],chunks[9]);
			}
		}
	}
}
