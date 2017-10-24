using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SEPAMIAR_SGC.Models;
using System.Data.Entity;

namespace SEPAMIAR_SGC.Utilities
{
	public class BackgroundJobs
	{
		_SGCModel db = new _SGCModel();

		public async Task AskForPermission(solicitud_permisos sp)
		{
			db.solicitud_permisos.Add(sp);
			await db.SaveChangesAsync();
		}

		public async Task<List<solicitud_permisos>> CheckForPendingPermissions(int userId)
		{
			var lstSP = db.solicitud_permisos.Where(m => m.usuario_jefe == userId);

			return await lstSP.ToListAsync();
		}
	}
}
