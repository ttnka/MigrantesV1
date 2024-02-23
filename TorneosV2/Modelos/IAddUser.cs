using System;
namespace TorneosV2.Modelos
{
    public interface IAddUser
    {
        Task<ApiRespuesta<AddUser>> FirmaIn(AddUser addUsuario);
        Task<ApiRespuesta<AddUser>> CrearNewAcceso(AddUser NewUser);
    }
}

