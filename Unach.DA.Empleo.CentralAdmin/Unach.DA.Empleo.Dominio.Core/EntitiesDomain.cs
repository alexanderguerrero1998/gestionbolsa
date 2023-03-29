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





    }
}
