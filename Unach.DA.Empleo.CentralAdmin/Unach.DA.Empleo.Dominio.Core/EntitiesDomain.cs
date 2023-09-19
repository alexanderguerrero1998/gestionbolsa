using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unach.DA.Empleo.Persistencia.Core.Models;

namespace Unach.DA.Empleo.Dominio.Core
{
    public class EntitiesDomain : IDisposable
    {
        SicoaContext contexto;
        

        public EntitiesDomain(DbContextOptions<SicoaContext> options) 
        {
            contexto = new SicoaContext(options);
        }

        public void GuardarTransacciones()
        {
            contexto.SaveChanges();
        }

        #region Dispose
        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    //context.Dispose();
                }
            }
            this.disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
        public IList<T> ExecuteStoredProcedure<T>(string name, params (string, object)[] nameValueParams) where T : class
        {
            DbCommand command = contexto.LoadStoredProcedure(name).WithSqlParams(nameValueParams);

            if (command.Connection.State == System.Data.ConnectionState.Closed)
                command.Connection.Open();
            try
            {
                using (var reader = command.ExecuteReader())
                {
                    return reader.MapToList<T>();
                }
            }
            finally
            {
                command.Connection.Close();
            }
        }
        public List<T> ExecuteStoredProcedureSingleFieldResult<T>(string name, params (string, object)[] nameValueParams)// where T : class
        {
            List<T> result = new List<T>();
            DbCommand command = contexto.LoadStoredProcedure(name).WithSqlParams(nameValueParams);

            if (command.Connection.State == System.Data.ConnectionState.Closed)
                command.Connection.Open();
            try
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(reader.GetFieldValue<T>(0));
                    }
                    //return reader.MapToList<T>();
                }
            }
            finally
            {
                command.Connection.Close();
            }
            return result;
        }
        private Repositorio<Idioma> idiomaRepositorio;
        public Repositorio<Idioma> IdiomaRepositorio
        {
            get
            {
                if (idiomaRepositorio == null)
                {
                    idiomaRepositorio = new Repositorio<Idioma>(contexto);
                }
                return idiomaRepositorio;
            }
        }
        private Repositorio<EstudianteIdioma> estudianteIdiomaRepositorio;
        public Repositorio<EstudianteIdioma> EstudianteIdiomaRepositorio
        {
            get
            {
                if (estudianteIdiomaRepositorio == null)
                {
                    estudianteIdiomaRepositorio = new Repositorio<EstudianteIdioma>(contexto);
                }
                return estudianteIdiomaRepositorio;
            }
        }
        private Repositorio<ResponsableEmpresa> responsableEmpresaRepositorio;
        public Repositorio<ResponsableEmpresa> ResponsableEmpresaRepositorio
        {
            get
            {
                if (responsableEmpresaRepositorio == null)
                {
                    responsableEmpresaRepositorio = new Repositorio<ResponsableEmpresa>(contexto);
                }
                return responsableEmpresaRepositorio;
            }
        }


        private Repositorio<Postulacion> postulacionRepositorio;
        public Repositorio<Postulacion> PostulacionRepositorio
        {
            get
            {
                if (postulacionRepositorio == null)
                {
                    postulacionRepositorio = new Repositorio<Postulacion>(contexto);
                }
                return postulacionRepositorio;
            }
        }


        private Repositorio<EstadoPostulacion> estadoPostulacionRepositorio;
        public Repositorio<EstadoPostulacion> EstadoPostulacionRepositorio
        {
            get
            {
                if (estadoPostulacionRepositorio == null)
                {
                    estadoPostulacionRepositorio = new Repositorio<EstadoPostulacion>(contexto);
                }
                return estadoPostulacionRepositorio;
            }
        }





        private Repositorio<Estudiante> estudianteRepositorio;
        public Repositorio<Estudiante> EstudianteRepositorio
        {
            get
            {
                if (postulacionRepositorio == null)
                {
                    estudianteRepositorio = new Repositorio<Estudiante>(contexto);
                }
                return estudianteRepositorio;
            }
        }

        private Repositorio<Empresa> empresaRepositorio;
        public Repositorio<Empresa> EmpresaRepositorio
        {
            get
            {
                if (empresaRepositorio == null)
                {
                    empresaRepositorio = new Repositorio<Empresa>(contexto);
                }
                return empresaRepositorio;
            }
        }

        private Repositorio<Vacante> vacanteRepositorio;
        public Repositorio<Vacante> VacanteRepositorio
        {
            get
            {
                if (vacanteRepositorio == null)
                {
                    vacanteRepositorio = new Repositorio<Vacante>(contexto);
                }
                return vacanteRepositorio;
            }
        }

        private Repositorio<TipoVacante> tipoVacanteRepositorio;
        public Repositorio<TipoVacante> TipoVacanteRepositorio
        {
            get
            {
                if (tipoVacanteRepositorio == null)
                {
                    tipoVacanteRepositorio = new Repositorio<TipoVacante>(contexto);
                }
                return tipoVacanteRepositorio;
            }
        }


        private Repositorio<Capacitacion> capacitacionRepositorio;
        public Repositorio<Capacitacion> CapacitacionRepositorio
        {
            get
            {
                if (capacitacionRepositorio == null)
                {
                    capacitacionRepositorio = new Repositorio<Capacitacion>(contexto);
                }
                return capacitacionRepositorio;
            }
        }

        private Repositorio<TipoCapacitacion> tipoCapacitacionRepositorio;
        public Repositorio<TipoCapacitacion> TipoCapacitacionRepositorio
        {
            get
            {
                if (tipoCapacitacionRepositorio == null)
                {
                    tipoCapacitacionRepositorio = new Repositorio<TipoCapacitacion>(contexto);
                }
                return tipoCapacitacionRepositorio;
            }
        }


        private Repositorio<TipoLogro> tipoLogroRepositorio;
        public Repositorio<TipoLogro> TipoLogroRepositorio
        {
            get
            {
                if (tipoLogroRepositorio == null)
                {
                    tipoLogroRepositorio = new Repositorio<TipoLogro>(contexto);
                }
                return tipoLogroRepositorio;
            }
        }


        private Repositorio<Logro> logroRepositorio;
        public Repositorio<Logro> LogroRepositorio
        {
            get
            {
                if (tipoLogroRepositorio == null)
                {
                    logroRepositorio = new Repositorio<Logro>(contexto);
                }
                return logroRepositorio;
            }
        }



        private Repositorio<FormacionAcademica> formacionAcademicaRepositorio;
        public Repositorio<FormacionAcademica> FormacionAcademicaRepositorio
        {
            get
            {
                if (formacionAcademicaRepositorio == null)
                {
                    formacionAcademicaRepositorio = new Repositorio<FormacionAcademica>(contexto);
                }
                return formacionAcademicaRepositorio;
            }
        }


        private Repositorio<TipoFormacionAcademica> tipoFormacionAcademicaRepositorio;
        public Repositorio<TipoFormacionAcademica> TipoFormacionAcademicaRepositorio
        {
            get
            {
                if (tipoFormacionAcademicaRepositorio == null)
                {
                    tipoFormacionAcademicaRepositorio = new Repositorio<TipoFormacionAcademica>(contexto);
                }
                return tipoFormacionAcademicaRepositorio;
            }
        }

        private Repositorio<RolUsuario> rolUsuarioRepositorio;

        public Repositorio<RolUsuario> RolUsuarioRepositorio
        {
            get
            {
                if (rolUsuarioRepositorio == null)
                {
                    rolUsuarioRepositorio = new Repositorio<RolUsuario>(contexto);
                }
                return rolUsuarioRepositorio;
            }
        }

        private Repositorio<Rol> rolRepositorio;

        public Repositorio<Rol> RolRepositorio
        {
            get
            {
                if (rolRepositorio == null)
                {
                    rolRepositorio = new Repositorio<Rol>(contexto);
                }
                return rolRepositorio;
            }
        }

        private Repositorio<Transaccion> transaccionRepositorio;

        public Repositorio<Transaccion> TransaccionRepositorio
        {
            get
            {
                if (transaccionRepositorio == null)
                {
                    transaccionRepositorio = new Repositorio<Transaccion>(contexto);
                }
                return transaccionRepositorio;
            }
        }

        private Repositorio<RolTransaccion> rolTransaccionRepositorio;
        public Repositorio<RolTransaccion> RolTransaccionRepositorio
        {
            get
            {
                if (rolTransaccionRepositorio == null)
                {
                    rolTransaccionRepositorio = new Repositorio<RolTransaccion>(contexto);
                }
                return rolTransaccionRepositorio;
            }
        }

        private Repositorio<Modulo> moduloRepositorio;
        public Repositorio<Modulo> ModuloRepositorio
        {
            get
            {
                if (moduloRepositorio == null)
                {
                    moduloRepositorio = new Repositorio<Modulo>(contexto);
                }
                return moduloRepositorio;
            }
        }

        private Repositorio<Sistema> sistemaRepositorio;
        public Repositorio<Sistema> SistemaRepositorio
        {
            get
            {
                if (sistemaRepositorio == null)
                {
                    sistemaRepositorio = new Repositorio<Sistema>(contexto);
                }
                return sistemaRepositorio;
            }
        }

        private Repositorio<AspNetUsers> aspNetUsersRepositorio;
        public Repositorio<AspNetUsers> AspNetUsersRepositorio
        {
            get
            {
                if (sistemaRepositorio == null)
                {
                    aspNetUsersRepositorio = new Repositorio<AspNetUsers>(contexto);
                }
                return aspNetUsersRepositorio;
            }
        }

        private Repositorio<ExperienciaLaboral> experienciaLaboralRepositorio;
        public Repositorio<ExperienciaLaboral> ExperienciaLaboralRepositorio
        {
            get
            {
                if (sistemaRepositorio == null)
                {
                    experienciaLaboralRepositorio = new Repositorio<ExperienciaLaboral>(contexto);
                }
                return experienciaLaboralRepositorio;
            }
        }


        private Repositorio<TipoExperiencialaboral> tipoExperienciaLaboralRepositorio;
        public Repositorio<TipoExperiencialaboral> TipoExperienciaLaboralRepositorio
        {
            get
            {
                if (sistemaRepositorio == null)
                {
                    tipoExperienciaLaboralRepositorio = new Repositorio<TipoExperiencialaboral>(contexto);
                }
                return tipoExperienciaLaboralRepositorio;
            }
        }








    }
}
