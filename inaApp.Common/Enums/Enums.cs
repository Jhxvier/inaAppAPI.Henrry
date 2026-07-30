using System.ComponentModel.DataAnnotations;

namespace inaApp.Common.Enums
{
    public static class Enums
    {
        public enum TipoImpuesto : byte
        {
            [Display(Name = "Impuesto al Valor Agregado")]
            IVA = 1,
            [Display(Name = "Impuesto Selectivo de Consumo")]
            ImpuestoSelectivoConsumo = 2,
            [Display(Name = "Impuesto único a los combustibles")]
            ImpuestoUnicoCombustibles = 3,
            [Display(Name = "Impuesto específico de bebidas alcohólicas")]
            ImpuestoEspecificoBebidasAlcoholicas = 4,
            [Display(Name = "Impuesto sobre bebidas envasadas y jabones")]
            BebidasEnvasadasSinAlcoholYJabones = 5,
            [Display(Name = "Impuesto a los productos de tabaco")]
            ImpuestoProductosTabaco = 6,
            [Display(Name = "IVA con cálculo especial")]
            IVACalculoEspecial = 7,
            [Display(Name = "IVA Régimen de Bienes Usados")]
            IVARegimenBienesUsados = 8,
            [Display(Name = "Impuesto específico al cemento")]
            ImpuestoEspecificoCemento = 12,
            [Display(Name = "Otros impuestos")]
            OtrosImpuestos = 99
        }

        public enum TipoDocumento : byte
        {
            [Display(Name = "Factura Electrónica")]
            FacturaElectronica = 1,
            [Display(Name = "Nota de Crédito Electrónica")]
            NotaCreditoElectronica = 2
        }
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
