using DemoSQLite.Modelo;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DemoSQLite.Administrador
{
    public class AdminBaseDatos
    {
        #region ctor

        public AdminBaseDatos()
        {
            InicializarAsync().SafeFireAndForget(false);
        }

        #endregion


        #region Propiedades

        public const SQLiteOpenFlags Flags =
            // open the database in read/write mode
            SQLiteOpenFlags.ReadWrite |
            // create the database if it doesn't exist
            SQLiteOpenFlags.Create |
            // enable multi-threaded database access
            SQLiteOpenFlags.SharedCache;

        private static string RutaBaseDatos
        {
            get
            {
                var basePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                return Path.Combine(basePath, "DemoSQLite.db3");
            }
        }

        #endregion

        #region Lazy

        private static readonly Lazy<SQLiteAsyncConnection> inicializador = new Lazy<SQLiteAsyncConnection>(() =>
        {
            return new SQLiteAsyncConnection(RutaBaseDatos, Flags);
        });

        private static SQLiteAsyncConnection Contexto => inicializador.Value;

        private static bool inicializado = false;

        private async Task InicializarAsync()
        {
            if (!inicializado)
            {
                // la base de datos esta vacia, no tiene tablas
                if (!Contexto.TableMappings.Any(m => m.MappedType.Name == typeof(ArticuloDespensa).Name))
                {
                    await Contexto.CreateTablesAsync(CreateFlags.None,
                        // tablas
                        typeof(ArticuloDespensa)).ConfigureAwait(false);
                    inicializado = true;
                }
            }
        }

        #endregion

        #region Insertar

        // insertar elemento de la despensa
        public async Task InsertarElemento(ArticuloDespensa nElemento)
        {
            await Contexto.InsertAsync(nElemento);
        }

        #endregion

        #region Actualizar

        // actualizar el articulo
        public async Task ActualizarArticulo(ArticuloDespensa articulo)
        {
            var queryArticulo = from art in Contexto.Table<ArticuloDespensa>()
                                where art.Id == articulo.Id
                                select art;

            List<ArticuloDespensa> despensa = await queryArticulo.ToListAsync();

            if (despensa.Count > 0)
            {
                ArticuloDespensa arti = despensa.First();

                arti.Cantidad = articulo.Cantidad;
                arti.Descripcion = articulo.Descripcion;
                arti.PrecioUnitario = articulo.PrecioUnitario;
                arti.Total = articulo.Total;

                await Contexto.UpdateAsync(arti);
            }
        }

        #endregion

        #region Seleccionar

        // seleccionar todos los elementos
        public async Task<List<ArticuloDespensa>> ObtenerListaDespensa()
        {
            return await Contexto.Table<ArticuloDespensa>().ToListAsync();
        }

        public async Task<ArticuloDespensa> ObtenerArticuloDespensa(int IdArticulo)
        {
            var queryArticulo = from art in Contexto.Table<ArticuloDespensa>()
                                where art.Id == IdArticulo
                                select art;

            List<ArticuloDespensa> despensa = await queryArticulo.ToListAsync();

            return despensa.FirstOrDefault();
        }

        public async Task<int> ObtenerUltimoId()
        {
            var queryDespensa = await Contexto.Table<ArticuloDespensa>().ToListAsync();

            if (queryDespensa.Count == 0)
            {
                return 0;
            }

            return queryDespensa.Max(m => m.Id);
        }

        #endregion

        #region Eliminar

        public async Task EliminarArticulo(int IdArticulo)
        {
            ArticuloDespensa arti = await ObtenerArticuloDespensa(IdArticulo);

            if (arti != null)
            {
                await Contexto.DeleteAsync(arti);
            }
        }

        #endregion
    }

    public static class TaskExtensions
    {
        public static async void SafeFireAndForget(this Task task,
            bool returnToCallingContext, Action<Exception> onExeption = null)
        {
            try
            {
                await task.ConfigureAwait(returnToCallingContext);
            }
            catch (Exception ex) when (onExeption != null)
            {
                onExeption(ex);
            }
        }
    }
}
