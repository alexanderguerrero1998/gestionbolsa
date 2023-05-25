namespace Unach.DA.Empleo.Presentacion.CentralAdmin.Models
{
    public class Api
    {
        public int Total { get; set; }
        public int Start { get; set; }
        public int Limit { get; set; }
        public List<ClientGrant> ClientGrants { get; set; }
    }
    public class ClientGrant
    {
        // Aquí puedes agregar propiedades adicionales según la estructura del objeto "client_grants" en el JSON
    }


}
