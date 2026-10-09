using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unosoft.EFactoringManager.Identity    
{
    [Table("#tmp_ResumenVigentesClientes", Schema = "dbo")]
    public class ConsolidadoOperaciones : Entity<int>
    {
        #region Overrides of Entity<long>
        [NotMapped]
        public override int Id { get; set; }
        #endregion
        public string oficina { get; set; }
        [Key]
        public string no_id_emisor { get; set; }
        public string razon_social_emisor { get; set; }
        
        public string no_id_pagador { get; set; }
        public string razon_social_pagador { get; set; }
        public string comercial { get; set; }

        //Operaciones
        public int no_op { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime fecha_op { get; set; }
        [Column(TypeName = "real")]
        public double factor_cliente { get; set; }
        public string tipo_factoring { get; set; }

        //DetalleOperaciones
        [Column(TypeName = "money")]
        public decimal valor_liquido { get; set; }
        [Column(TypeName = "money")]
        public decimal valor_descuento { get; set; }
        [Column(TypeName = "money")]
        public decimal rcf_giro { get; set; }
        [Column(TypeName = "money")]
        public decimal gmf { get; set; }
        [Column(TypeName = "money")]
        public decimal valor_desembolso { get; set; }
        [Column(TypeName = "money")]
        public decimal ingresos { get; set; }

        //Titulos
        [Key]
        public string no_titulo { get; set; }
        public int plazo_negocio { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime fecha_v_to_real {  get; set; }
        [Column(TypeName = "money")]
        public decimal valor_neto { get; set; }
        [Column(TypeName = "real")]
        public Single desembolso {  get; set; }
        [Column(TypeName = "money")]
        public decimal valor_remanente { get; set; }

        //DetOpeInversionistas
        public string no_id_fondeador { get; set; }
        public string razon_social_fondeador {  get; set; }
        [Column(TypeName = "real")]
        public double factor_fondeo { get; set; }
        [Column(TypeName = "money")]
        public decimal costo_de_fondeo { get; set; }
        [Column(TypeName = "numeric(20,8)")]
        public decimal trm { get; set; }
    }
}
