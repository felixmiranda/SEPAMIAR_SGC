using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SEPAMIAR_SGC.Models;
using Microsoft.Azure.WebJobs;

namespace ActiveClientsValidationJob
{
	public class Functions
	{
		// This function will get triggered/executed when a new message is written 
		// on an Azure Queue called queue.
		public static void ProcessQueueMessage([QueueTrigger("activeclientsvalidation")] string message, TextWriter logger)
		{
			using (_SGCModel db = new _SGCModel())
			{
				foreach (clientes c in db.clientes.AsEnumerable())
				{
					DateTime d = db.programa_clientes.Where(m => m.cliente_id == c.id_alt).OrderByDescending(m => m.fecha_inicio).Select(m => m.fecha_fin).FirstOrDefault();

					if (d != null && d < DateTimeOffset.Now.Date)
					{
						c.activo = false;
						logger.WriteLine("Cliente código: " + c.codigo + ", ha cambiado su estado a Inactivo.");
					}
				}
			}			
		}
	}
}



