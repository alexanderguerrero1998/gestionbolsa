using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unach.DA.Empleo.Dominio.Core;
using Unach.DA.Empleo.Persistencia.Core.Models;
using Unach.DA.Empleo.Presentacion.CentralAdmin.ViewModel;

namespace Unach.DA.Empleo.Presentacion.CentralAdmin.Mappings
{

    public class PerfilMappings : Profile
    {

        /// <summary>
        /// Define las clases que van a ser mapeadas mediante AutoMapper, además de las clases que van a integrar campos de auditoría.
        /// </summary>
        public PerfilMappings()
        {

            //Tablas que requieren auditoria
            CreateMap<AuditoriaViewModel, Modulo>();
            CreateMap<AuditoriaViewModel, Transaccion>();
            CreateMap<AuditoriaViewModel, Rol>();
            CreateMap<AuditoriaViewModel, RolUsuario>();

            CreateMap<AuditoriaViewModel, RolTransaccion>();
            CreateMap<AuditoriaViewModel, Postulacion>();
            CreateMap<AuditoriaViewModel, EstadoPostulacion>();
            CreateMap<AuditoriaViewModel, Estudiante>();
            CreateMap<AuditoriaViewModel, Empresa>();
            CreateMap<AuditoriaViewModel, ResponsableEmpresa>();
            CreateMap<AuditoriaViewModel, Vacante>();
            CreateMap<AuditoriaViewModel, Logro>();
            CreateMap<AuditoriaViewModel, FormacionAcademica>();
            CreateMap<AuditoriaViewModel, Capacitacion>();
            CreateMap<AuditoriaViewModel, Idioma>();
            CreateMap<AuditoriaViewModel, ExperienciaLaboral>();
            CreateMap<AuditoriaViewModel, EstudianteIdioma>();


            //Tablas del proyecto
            CreateMap<Postulacion, PostulacionesViewModel>();
            CreateMap<PostulacionesViewModel, Postulacion>();
            CreateMap<Idioma, EstudianteIdiomaViewModel>();
            CreateMap<EstudianteIdiomaViewModel, Idioma>();
            CreateMap<EstudianteViewModel, Estudiante>();
            CreateMap<Estudiante, EstudianteViewModel>();
            CreateMap<EmpresaViewModel, Empresa>();
            CreateMap<Empresa, EmpresaViewModel>();
            CreateMap<ResponsableEmpresa, ResponsableEmpresaViewModel>();
            CreateMap<ResponsableEmpresaViewModel, ResponsableEmpresa>();
            CreateMap<Vacante, VacanteViewModel>();
            CreateMap<VacanteViewModel,Vacante>();
            CreateMap<Capacitacion, CapacitacionViewModel>();
            CreateMap<CapacitacionViewModel, Capacitacion>();
            CreateMap<Capacitacion, CapacitacionViewModel2>();
            CreateMap<CapacitacionViewModel2, Capacitacion>();
            CreateMap<Logro, LogroViewModel>();
            CreateMap<LogroViewModel, Logro>();
            CreateMap<FormacionAcademica, FormacionAcademicaViewModel>();
            CreateMap<FormacionAcademicaViewModel, FormacionAcademica>();
            CreateMap<UsuarioAutenticadoViewModel, AspNetUsers>();
            CreateMap<AspNetUsers, UsuarioAutenticadoViewModel>();
            CreateMap<ExperienciaLaboral, ExperienciaLaboralViewModel>();
            CreateMap<ExperienciaLaboralViewModel, ExperienciaLaboral>();
            CreateMap<EstudianteIdioma, EstudianteIdiomaViewModel>();
            CreateMap<EstudianteIdiomaViewModel, EstudianteIdioma>();

            /*

            CreateMap<AccionPersonal, AccionPersonalEdicionViewModel>();
            CreateMap<AccionPersonalViewModel, AccionPersonal>()
                .ForMember(ap => ap.Elaborado, apvm => apvm.MapFrom(src => src.FechaElaboracion))
                .ForMember(ap => ap.Remuneracion, apvm => apvm.MapFrom(src => src.RemuneracionPropuesta))
                .ForMember(ap => ap.PartidaPresupuestaria, apvm => apvm.MapFrom(src => src.PartidaPresupuestariaPropuesta))
                .ForMember(ap => ap.PartidaPresupuestariaIndividual, apvm => apvm.MapFrom(src => src.PartidaPresupuestariaIndividualPropuesta))
                .ForMember(ap => ap.Grado, apvm => apvm.MapFrom(src => src.GradoPropuesto))
                .ForMember(ap => ap.Nivel, apvm => apvm.MapFrom(src => src.NivelPropuesto))
                .ForMember(ap => ap.GrupoOcupacional, apvm => apvm.MapFrom(src => src.GrupoOcupacionalPropuesto));

            CreateMap<AccionPersonalEdicionViewModel, AccionPersonal>()
                .ForMember(ap => ap.FechaCierreAsignarAccionPersonalActual, apvm => apvm.MapFrom(src => src.FechaCierreAsignarAccionPersonalActual))
                .ForMember(ap => ap.FechaCierreAsignarAccionPersonalAnterior, apvm => apvm.MapFrom(src => src.FechaCierreAsignarAccionPersonalAnterior))
                .ForMember(ap => ap.Elaborado, apvm => apvm.MapFrom(src => src.FechaElaboracion))
                .ForMember(ap => ap.Remuneracion, apvm => apvm.MapFrom(src => src.RemuneracionPropuesta))
                .ForMember(ap => ap.PartidaPresupuestaria, apvm => apvm.MapFrom(src => src.PartidaPresupuestariaPropuesta))
                .ForMember(ap => ap.PartidaPresupuestariaIndividual, apvm => apvm.MapFrom(src => src.PartidaPresupuestariaIndividualPropuesta))
                .ForMember(ap => ap.Grado, apvm => apvm.MapFrom(src => src.GradoPropuesto))
                .ForMember(ap => ap.Nivel, apvm => apvm.MapFrom(src => src.NivelPropuesto))
                .ForMember(ap => ap.GrupoOcupacional, apvm => apvm.MapFrom(src => src.GrupoOcupacionalPropuesto));

            CreateMap<AccionPersonalImprimirViewModel, AccionPersonalValidacionViewModel>();

            CreateMap<AdendaEditarViewModel, Adenda>()
                .ForMember(a => a.ObjetoModificado, avm => avm.MapFrom(src => src.ModificarObjeto))
                .ForMember(a => a.RemuneracionModificada, avm => avm.MapFrom(src => src.ModificarRemuneracion))
                .ForMember(a => a.PlazoModificado, avm => avm.MapFrom(src => src.ModificarPlazo));

            CreateMap<AdendaMigracionViewModel, Adenda>();
            CreateMap<PermisoServidorViewmodel, PermisoServidor>();
            CreateMap<AcreditarSaldoEditarViewModel, SaldoVacaciones>();

            */
        }
    }
}
