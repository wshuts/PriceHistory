using System;
using System.Collections;

namespace PriceHistory
{
	/// <summary>
	/// Summary description for Statistics.
	/// </summary>
	public class Statistics
	{
		static Statistics()
		{
		}

		public static double Mean(IList vector)
		{
			double sum=0;
			double mean=0;
			
			foreach(double element in vector)
			{
				sum=sum+element;
			}
			
			mean=sum/vector.Count;
			return mean;
		}
		
		public static double StandardDeviation(IList vector)
		{
			double mean=0;
			double difference=0;
			double square=0;
			double sum=0;
			double meanSquare=0;
			double standardDeviation=0;

			mean=Mean(vector);
			
			foreach(double element in vector)
			{
				difference=element-mean;
				square=Math.Pow(difference,2.0);
				sum=sum+square;
			}
			
			meanSquare=sum/vector.Count;
			standardDeviation=Math.Sqrt(meanSquare);
			return standardDeviation;

		}

		public static double Correlation(IList vector1,IList vector2)
		{
			//TODO: Error trap if lengths of vectors are not equal

			double mean1=0;
			double mean2=0;
			double standardDeviation1=0;
			double standardDeviation2=0;
			IList differences1=new ArrayList();
			IList differences2=new ArrayList();
			int index=0;
			IList differenceProducts=new ArrayList();
			double correlation=0;

			mean1=Mean(vector1);
			mean2=Mean(vector2);
			standardDeviation1=StandardDeviation(vector1);
			standardDeviation2=StandardDeviation(vector2);

			differences1.Clear();
			foreach(double element1 in vector1)
			{
				differences1.Add(element1-mean1);
			}
			
			differences2.Clear();
			foreach(double element2 in vector2)
			{
				differences2.Add(element2-mean2);
			}
			
			differenceProducts.Clear();
			for(index=0;index<differences1.Count;index++)
			{
				differenceProducts.Add((double)differences1[index]*(double)differences2[index]);
			}
			
			correlation=Mean(differenceProducts)/(standardDeviation1*standardDeviation2);
			return correlation;
		}
	}
}
