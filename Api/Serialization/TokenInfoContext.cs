namespace Minimal.Api.Serialization
{
    using Minimal.Api.Authentication;
    using System.Text.Json.Serialization;

    //requisitos para criação da classe
    //Adicionar a propripedade
    //definir como partial
    //extender de JsonSerializerContext
    //recompilar o projeto
    [JsonSerializable(typeof(TokenInfo))]
    internal partial class TokenInfoContext : JsonSerializerContext
    {       
    }
}
