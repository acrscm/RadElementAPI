using System.Text.Json.Serialization;

namespace RadElement.Core.DTO
{
    /// <summary>
    /// Gets or sets the modality type
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ModalityType
    {
        /// <summary>
        /// Modaility Type is CR
        /// </summary>
        CR,

        /// <summary>
        /// Modaility Type is CT
        /// </summary>
        CT,

        /// <summary>
        /// Modaility Type is DX
        /// </summary>
        DX,

        /// <summary>
        /// Modaility Type is IVUS
        /// </summary>
        IVUS,

        /// <summary>
        /// Modaility Type is MG
        /// </summary>
        MG,

        /// <summary>
        /// Modaility Type is MR
        /// </summary>
        MR,

        /// <summary>
        /// Modaility Type is NM
        /// </summary>
        NM,

        /// <summary>
        /// Modaility Type is PT
        /// </summary>
        PT,

        /// <summary>
        /// Modaility Type is RF
        /// </summary>
        RF,

        /// <summary>
        /// Modaility Type is RG
        /// </summary>
        RG,

        /// <summary>
        /// Modaility Type is US
        /// </summary>
        US,

        /// <summary>
        /// Modaility Type is XA
        /// </summary>
        XA,
    }
}
