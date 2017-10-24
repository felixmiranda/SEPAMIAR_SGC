using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using System.Net;
using RestSharp.Authenticators;

namespace SEPAMIAR_SGC.Utilities
{
	public static class Mail
	{
		private static string domain = "sgc.personaltraining.com.pe";
		private static string mail = "Personal Training Perú <app@sgc.personaltraining.com.pe>";

		public static bool Send(List<string> pTo, string pSubject, string pBody)
		{
			RestClient client = new RestClient();
			client.BaseUrl = new Uri("https://api.mailgun.net/v3");
			client.Authenticator =
					new HttpBasicAuthenticator("api",
											   "key-44f7133c62e338cd2e8b2286c0285eac");
			RestRequest request = new RestRequest();
			request.AddParameter("domain",
								 domain, ParameterType.UrlSegment);
			request.Resource = "{domain}/messages";
			request.AddParameter("from", mail);
			foreach (string to in pTo)
			{
				request.AddParameter("to", to);
			}
			request.AddParameter("subject", pSubject);
			request.AddParameter("text", pBody);

			request.Method = Method.POST;
			IRestResponse response = client.Execute(request);

			if (response.StatusCode == HttpStatusCode.OK)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		public static bool SendWithAttachments(List<string> pTo, string pSubject, string pBody, List<MailFile> pAttachments)
		{
			RestClient client = new RestClient();
			client.BaseUrl = new Uri("https://api.mailgun.net/v3");
			client.Authenticator =
					new HttpBasicAuthenticator("api",
											   "key-44f7133c62e338cd2e8b2286c0285eac");
			RestRequest request = new RestRequest();
			request.AddParameter("domain",
								 domain, ParameterType.UrlSegment);
			request.Resource = "{domain}/messages";
			request.AddParameter("from", mail);
			foreach (string to in pTo)
			{
				request.AddParameter("to", to);
			}
			request.AddParameter("subject", pSubject);
			request.AddParameter("text", pBody);

			foreach (MailFile mf in pAttachments)
			{
				request.AddParameter("attachment", "@" + mf.Name);
				request.AddFile("attachment", mf.FileBytes, mf.Name, mf.FileType);
			}

			request.Method = Method.POST;
			IRestResponse response = client.Execute(request);

			if (response.StatusCode == HttpStatusCode.OK)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
	}

	public class MailFile
	{
		public MailFile(string Name, byte[] FileBytes, string FileType)
		{
			this.Name = Name;
			this.FileBytes = FileBytes;
			this.FileType = FileType;
		}

		public string Name { get; set; }
		public byte[] FileBytes { get; set; }
		public string FileType { get; set; }
	}
}
