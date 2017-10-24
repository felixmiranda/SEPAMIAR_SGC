using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEPAMIAR_SGC.Utilities
{
	public static class CurrentDate
	{
		public static DateTime getNow()
		{
			var myTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SA Pacific Standard Time");
			var currentDateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, myTimeZone);
			return currentDateTime;
		}

		public static DateTime getToday()
		{
			var myTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SA Pacific Standard Time");
			var currentDateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, myTimeZone);
			return currentDateTime.Date;
		}
	}
}
