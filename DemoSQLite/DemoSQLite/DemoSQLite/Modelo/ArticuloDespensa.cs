using SQLite;

namespace DemoSQLite.Modelo
{
    public class ArticuloDespensa
    {
        [PrimaryKey]
        public int Id { get; set; }

        public string Descripcion { get; set; }

        public int Cantidad { get; set; }

        public double PrecioUnitario { get; set; }

        public double Total { get; set; }
    }
}