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

        public User(string usu_cedulaP_, string usu_nombres_, string usu_apellidos_, string usu_correo_, string usu_contrasenia_, string usu_tipo_, bool usu_aplica_zona_, bool usu_estado_)
        {
            this.usu_cedulaP = usu_cedulaP_;
            this.usu_nombres = usu_nombres_;
            this.usu_apellidos = usu_apellidos_;
            this.usu_correo = usu_correo_;
            this.usu_contrasenia = usu_contrasenia_;
            this.usu_tipo = usu_tipo_;
            this.usu_aplica_zona = usu_aplica_zona_;
            this.usu_estado = usu_estado_;
        }

    }
}
