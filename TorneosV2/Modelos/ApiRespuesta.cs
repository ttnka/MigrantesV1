using System;
namespace TorneosV2.Modelos
{
    public class ApiRespuesta<TEntity> where TEntity : class
    {
        public bool Exito { get; set; }
        public List<string> MsnError { get; set; } = new List<string>();
        public TEntity Data { get; set; }
        public string Texto { get; set; }

    }
}

