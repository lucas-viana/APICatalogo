using System.Runtime.Serialization;

namespace APICatalogo.Pagination
{
    public enum Criterio
    {
        [EnumMember(Value = "maior")]
        maior,
        [EnumMember(Value = "menor")]
        menor,
        [EnumMember(Value = "igual")]
        igual
    }
}
