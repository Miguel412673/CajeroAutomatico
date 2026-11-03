using System;

class Program
{
    static void Main()
    {
        string respuesta;

        do
        {
            decimal monto;
            int plazo;
            decimal tasa;
            decimal cuota;
            decimal totalIntereses;
            decimal totalPagado;

            Console.WriteLine("\n=== SIMULADOR DE PRESTAMOS ===");

            // PARTE 1: Solicitar prestamo
            SolicitarPrestamo(out monto, out plazo);

            // PARTE 2: Categoria y tasa de interes
            tasa = CalcularTasa();

            // PARTE 3: Calcular cuota mensual
            cuota = CalcularCuota(monto, tasa, plazo);

            // PARTE 4: Mostrar tabla de amortizacion
            MostrarAmortizacion(
                monto,
                tasa,
                plazo,
                cuota,
                out totalIntereses,
                out totalPagado);

            // PARTE 5: Resumen final
            ResumenFinal(
                monto,
                cuota,
                totalIntereses,
                totalPagado);

            Console.Write("\nDesea simular otro prestamo? (S/N): ");
            respuesta = (Console.ReadLine() ?? "").Trim().ToUpper();

        } while (respuesta == "S");

        Console.WriteLine("\nGracias por utilizar el simulador.");
    }

    // PARTE 1: SOLICITAR Y VALIDAR PRESTAMO
    static void SolicitarPrestamo(out decimal monto, out int plazo)
    {
        decimal montoMinimo = 5000;
        decimal montoMaximo = 500000;
        int plazoMinimo = 6;
        int plazoMaximo = 60;

        monto = 0;
        plazo = 0;

        do
        {
            Console.WriteLine("Introduzca el monto que desea solicitar:");

            if (decimal.TryParse(Console.ReadLine(), out monto) &&
                monto >= montoMinimo && monto <= montoMaximo)
            {
                Console.WriteLine($"Monto valido: RD$ {monto:N2}");
                break;
            }

            Console.WriteLine("Monto invalido, debe ser entre RD$ 5,000 y RD$ 500,000");

        } while (true);

        do
        {
            Console.WriteLine("Introduzca el plazo a pagar (6 a 60 meses):");

            if (int.TryParse(Console.ReadLine(), out plazo) &&
                plazo >= plazoMinimo && plazo <= plazoMaximo)
            {
                Console.WriteLine($"El plazo es valido: {plazo} meses");
                break;
            }

            Console.WriteLine("Plazo invalido, debe ser entre 6 y 60 meses");

        } while (true);
    }

    // PARTE 2: CATEGORIA DEL SOCIO Y TASA DE INTERES
    static decimal CalcularTasa()
    {
        string categoria;
        decimal tasaAnual = 0;

        do
        {
            Console.Write("\nIngrese la categoria del socio (A, B o C): ");
            categoria = (Console.ReadLine() ?? "").Trim().ToUpper();

            switch (categoria)
            {
                case "A":
                    tasaAnual = 0.12m;
                    break;

                case "B":
                    tasaAnual = 0.15m;
                    break;

                case "C":
                    tasaAnual = 0.18m;
                    break;

                default:
                    Console.WriteLine("Categoria invalida. Intente nuevamente.");
                    break;
            }

        } while (categoria != "A" && categoria != "B" && categoria != "C");

        decimal tasaMensual = tasaAnual / 12;

        Console.WriteLine($"Categoria seleccionada: {categoria}");
        Console.WriteLine($"Tasa anual: {(tasaAnual * 100):N2}%");
        Console.WriteLine($"Tasa mensual: {(tasaMensual * 100):N2}%");

        return tasaMensual;
    }

    // PARTE 3: CALCULAR CUOTA MENSUAL
    static decimal CalcularCuota(decimal monto, decimal tasaMensual, int plazo)
    {
        Console.WriteLine("\n===== CALCULO DE CUOTA MENSUAL =====");

        decimal cuotaMensual;

        if (tasaMensual == 0)
        {
            cuotaMensual = monto / plazo;
        }
        else
        {
            double tasa = (double)tasaMensual;
            double cantidad = Math.Pow(1 + tasa, -plazo);

            cuotaMensual = monto * tasaMensual / (1 - (decimal)cantidad);
        }

        cuotaMensual = Math.Round(cuotaMensual, 2);

        Console.WriteLine("Monto del prestamo: RD$ " + monto.ToString("N2"));
        Console.WriteLine("Plazo de pago: " + plazo + " meses");
        Console.WriteLine("Tasa de interes mensual: " + (tasaMensual * 100).ToString("N2") + "%");
        Console.WriteLine("La cuota mensual es: RD$ " + cuotaMensual.ToString("N2"));

        return cuotaMensual;
    }

    // PARTE 4: TABLA DE AMORTIZACION
    static void MostrarAmortizacion(
        decimal monto,
        decimal tasaMensual,
        int plazo,
        decimal cuotaMensual,
        out decimal totalIntereses,
        out decimal totalPagado)
    {
        decimal saldo = monto;
        totalIntereses = 0;
        decimal totalCapital = 0;
        totalPagado = 0;

        Console.WriteLine("\n========== TABLA DE AMORTIZACION ==========");

        Console.WriteLine("{0,-10}{1,-16}{2,-18}{3,-16}",
            "Cuota", "Interes", "Abono capital", "Saldo");

        for (int mes = 1; mes <= plazo; mes++)
        {
            decimal interes = Math.Round(saldo * tasaMensual, 2);

            decimal abonoCapital;
            decimal pago;

            if (mes == plazo)
            {
                abonoCapital = saldo;
                pago = abonoCapital + interes;
            }
            else
            {
                abonoCapital = Math.Round(cuotaMensual - interes, 2);

                if (abonoCapital > saldo)
                {
                    abonoCapital = saldo;
                }

                pago = interes + abonoCapital;
            }

            saldo = Math.Round(saldo - abonoCapital, 2);

            totalIntereses += interes;
            totalCapital += abonoCapital;
            totalPagado += pago;

            Console.WriteLine("{0,-10}{1,-16:N2}{2,-18:N2}{3,-16:N2}",
                mes, interes, abonoCapital, saldo);
        }

        Console.WriteLine("\n========== TOTALES ==========");
        Console.WriteLine($"Total de intereses: RD$ {totalIntereses:N2}");
        Console.WriteLine($"Total de capital: RD$ {totalCapital:N2}");
        Console.WriteLine($"Total pagado: RD$ {totalPagado:N2}");
        Console.WriteLine($"Saldo pendiente: RD$ {saldo:N2}");
    }

    // PARTE 5: RESUMEN FINAL
    static void ResumenFinal(
        decimal monto,
        decimal cuota,
        decimal totalIntereses,
        decimal totalPagado)
    {
        Console.WriteLine("\n========== RESUMEN FINAL ==========");

        Console.WriteLine($"Monto solicitado: RD$ {monto:N2}");
        Console.WriteLine($"Cuota mensual: RD$ {cuota:N2}");
        Console.WriteLine($"Total de intereses: RD$ {totalIntereses:N2}");
        Console.WriteLine($"Total a pagar: RD$ {totalPagado:N2}");

        Console.WriteLine("===================================");
    }
}


