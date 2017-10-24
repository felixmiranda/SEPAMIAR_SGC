using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEPAMIAR_SGC.Utilities
{
	public class Mobile
	{
		public Dictionary<string, object> GetDictForJSON(string message, Dictionary<string, object> data, MobileResponse code)
		{
			Dictionary<string, object> oDict = new Dictionary<string, object>();

			oDict.Add("message", message);
			oDict.Add("content", data);
			oDict.Add("status", code);

			return oDict;
		}
	}

	public enum MobileResponse
	{
		Success,
		Error
	}
}
