namespace Geocontrol_PPI_NET_9.Models.Auth
{
    public class User
    {
        public string usu_cedulaP { get; set; } = "";
        public string usu_nombres { get; set; } = "";
        public string usu_apellidos { get; set; } = "";
        public string usu_correo { get; set; } = "";
        public string usu_contrasenia { get; set; } = "";
        public string usu_tipo { get; set; } = "";
        public bool usu_aplica_zona { get; set; } = false;
        public bool usu_estado { get; set; } = false;

        public bool? Creation { get; set; }

        /// <summary>
        /// Constructor created for the inicialization on dapper
        /// </summary>
        public User()
        {
            this.usu_cedulaP = "";
            this.usu_nombres = "";
            this.usu_apellidos = "";
            this.usu_correo = "";
            this.usu_contrasenia = "";
            this.usu_tipo = "";
            this.usu_aplica_zona = false;
            this.usu_estado = false;
        }

    }
}
