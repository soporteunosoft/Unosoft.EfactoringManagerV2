using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.AutoMapper;
using System.Text.Json.Serialization;

namespace Unosoft.EFactoringManager.Identity
{
    [Table("EstadosSolicitudes", Schema = "dbo")]
    public class EstadosSolicitudes : Entity<int>
    {
        //#region Overrides of Entity<long>
        ///// <summary>
        ///// Unique identifier for this entity.
        ///// </summary>
        ///

        public string TipoDocumento { get; set; }

        [Column("EstadoSolicitud")]
        //[DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public override int Id { get; set; }
        public string NomEstadoSol { get; set; }
        public string DesEstadoSol { get; set; }

        [StringLength(20)]
        public string IdUsrMod { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? FecModifica { get; set; }
        [JsonIgnore]
        public List<SolicitudesOperaciones> SolicitudeOperaciones { get; set; }

    }
}
