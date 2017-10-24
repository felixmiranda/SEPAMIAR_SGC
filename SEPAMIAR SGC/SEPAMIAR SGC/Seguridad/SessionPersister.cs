using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SEPAMIAR_SGC.Seguridad
{
	public static class SessionPersister
	{
		static string UsuarioSessionVar = "EmailUsuario";

		public static string EmailUsuario
		{
			get
			{
				if (HttpContext.Current == null)
				{
					return string.Empty;
				}
				var sessionVar = HttpContext.Current.Session[UsuarioSessionVar];
				if (sessionVar != null)
				{
					return sessionVar as string;
				}
				return null;
			}
			set
			{
				HttpContext.Current.Session[UsuarioSessionVar] = value;
			}
		}

		static string ClientNameVar = "NombreCliente";

		public static string NombreCliente
		{
			get
			{
				if (HttpContext.Current == null)
				{
					return string.Empty;
				}
				var sessionVar = HttpContext.Current.Session[ClientNameVar];
				if (sessionVar != null)
				{
					return sessionVar as string;
				}
				return null;
			}
			set
			{
				HttpContext.Current.Session[ClientNameVar] = value;
			}
		}

		static string ClientAreaVar = "AreaCliente";

		public static Models.UserProfiles AreaCliente
		{
			get
			{
				if (HttpContext.Current == null)
				{
					return Models.UserProfiles.Default;
				}
				var sessionVar = HttpContext.Current.Session[ClientAreaVar];
				if (sessionVar != null)
				{
					return (Models.UserProfiles)sessionVar;
				}
				return Models.UserProfiles.Default;
			}
			set
			{
				HttpContext.Current.Session[ClientAreaVar] = value;
			}
		}

		static string ImgBase64Var = "ImgBase64";

		public static string ImgBase64
		{
			get
			{
				if (HttpContext.Current == null)
				{
					return string.Empty;
				}
				var sessionVar = HttpContext.Current.Session[ImgBase64Var];
				if (sessionVar != null)
				{
					return sessionVar as string;
				}
				return null;
			}
			set
			{
				HttpContext.Current.Session[ImgBase64Var] = value;
			}
		}

		static string UserIdVar = "UserId";

		public static int UserId
		{
			get
			{
				if (HttpContext.Current == null)
				{
					return 0;
				}
				var sessionVar = HttpContext.Current.Session[UserIdVar];
				if (sessionVar != null)
				{
					return (int)sessionVar;
				}
				return 0;
			}
			set
			{
				HttpContext.Current.Session[UserIdVar] = value;
			}
		}
	}
}