using Abp.Application.Services;
using Abp.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unosoft.EFactoringManager.Ciudades.Dto;

namespace Unosoft.EFactoringManager.Ciudades
{

    public class CiudadesServices : ApplicationService, ICiudadesServices
    {
        #region Atributos
        CiudadesManager _ciudadesManager;
        #endregion

        public CiudadesServices(CiudadesManager ciudadesManager)
        {
            _ciudadesManager = ciudadesManager;
        }
        [HttpGet]
        public async Task<List<CiudadesDto>> ObtenerCiudades(string nomCiudad)
        {
            try
            {
                List<CiudadesDto> ciudadesDto = new List<CiudadesDto>();

                var ciudades = await _ciudadesManager.ObtenerCiudades(nomCiudad);
                foreach (var ciudad in ciudades)
                {
                    CiudadesDto ciudadDto = new CiudadesDto();
                    ciudadDto.IdCiudad = ciudad.IdCiudad;
                    ciudadDto.IdDepartamento = ciudad.IdDepartamento;
                    ciudadDto.IdPais = ciudad.IdPais;
                    ciudadDto.NomCiudad = ciudad.NomCiudad +"/"+ ciudad.Departamento.NomDepartamento + "/" + ciudad.Pais.NomPais;
                    ciudadesDto.Add(ciudadDto);
                }
                return ciudadesDto;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
