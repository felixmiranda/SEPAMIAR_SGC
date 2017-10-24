using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SEPAMIAR_SGC.Models;

namespace SEPAMIAR_SGC.ViewModel
{
	public class ClientesGeneralViewModel
	{
		public string codigo { get; set; }
		public string nombres { get; set; }
		public string apellidos { get; set; }
		public genero genero { get; set; }
		public string email { get; set; }
		public string email_empresa { get; set; }
		public fuente como_se_entero { get; set; }
		public bool activo { get; set; }
		public DateTime created_at { get; set; }
	}
}