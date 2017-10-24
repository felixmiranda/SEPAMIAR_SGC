using SEPAMIAR_SGC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace SEPAMIAR_SGC.Seguridad
{
	public class CustomPrincipal : IPrincipal
	{
		private int usuarioId;

		public CustomPrincipal(int us, string usMail)
		{
			this.usuarioId = us;
			this.Identity = new GenericIdentity(usMail);
		}

		public IIdentity Identity
		{
			get;
			set;
		}

		public bool IsInRole(string role)
		{
			_SGCModel db = new _SGCModel();
			/*var roles = role.Split(new char[] { ',' });//db.Formularios.ToList<Formularios>();
			return roles.Any(r => this.usuario.usuario_permisos.Contains(r));*/

			int AccessId = db.permisos.Where(m => m.codigo_interno == role).Select(m => m.id).FirstOrDefault();
			usuario_permisos userAccess =  db.usuario_permisos.Where(m => m.usuario_id == usuarioId && m.permiso_id == AccessId && m.deleted_at == null).FirstOrDefault();

			//usuario_permisos usuario_permiso = db.usuario_permisos.Where(up => up.usuarios.email == this.usuario.email && up.permiso_id == permiso_id).FirstOrDefault();

			return (userAccess != null);
		}
	}
}