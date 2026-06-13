namespace inaApp.Common.Enums
{
    public static class Enums
    {
        public enum TipoIdentificacion : byte
        {
            CedulaFisica = 1,
            CedulaJuridica = 2,
            DIMEX = 3,
            NITE = 4,
            Pasaporte = 5
        }

        public enum TipoVenta
        {
            contado = 1,
            credito = 2
        }

        public enum TipoPago         
        {
            efectivo = 1,
            tarjetaCredito = 2,
            tarjetaDebito = 3,
            transferencia = 4
        }

    }

}
