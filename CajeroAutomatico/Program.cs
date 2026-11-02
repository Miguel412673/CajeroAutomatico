using System;

namespace CajeroAutomatico
{
    internal class Program
    {
        static void Main(string[] args)
        {
            decimal saldo = 10000m;
            int opcion;
            int cantidadRetiros = 0;

            // VALIDACION DEL PIN
            int pinCorrecto = 6699;
            int intentosMaximos = 3;
            bool acceso = false;

            for (int intento = 1; intento <= intentosMaximos; intento++)
            {
                Console.Write("Ingrese el PIN: ");
                string entrada = Console.ReadLine() ?? "";

                if (int.TryParse(entrada, out int pinIngresado))
                {
                    if (pinIngresado == pinCorrecto)
                    {
                        Console.WriteLine("PIN correcto");
                        acceso = true;
                        break;
                    }
                    else
                    {
                        Console.WriteLine("PIN incorrecto.");
                    }
                }
                else
                {
                    Console.WriteLine("Entrada inválida: el PIN debe contener solo números.");
                }

                if (intento < intentosMaximos)
                {
                    Console.WriteLine($"Te quedan {intentosMaximos - intento} intento(s).");
                }
            }

            if (!acceso)
            {
                Console.WriteLine("PIN incorrecto. Acceso bloqueado.");
            }
            else
            {
                // MENU PRINCIPAL
                do
                {
                    Console.WriteLine("\n===== CAJERO AUTOMÁTICO =====");
                    Console.WriteLine("1. Consultar saldo");
                    Console.WriteLine("2. Depositar");
                    Console.WriteLine("3. Retirar");
                    Console.WriteLine("4. Salir");
                    Console.Write("Seleccione una opción: ");

                    if (!int.TryParse(Console.ReadLine(), out opcion))
                    {
                        Console.WriteLine("Entrada inválida.");
                        opcion = 0;
                    }
                    else
                    {
                        switch (opcion)
                        {
                            case 1:
                                Console.WriteLine($"Su saldo actual es: RD$ {saldo:N2}");
                                break;

                            case 2:
                                saldo = Depositar(saldo);
                                break;

                            case 3:
                                {
                                    Console.WriteLine("Ingrese el monto que desea retirar:");
                                    decimal monto;

                                    if (decimal.TryParse(Console.ReadLine(), out monto))
                                    {
                                        if (monto <= 0)
                                        {
                                            Console.WriteLine("El monto debe ser mayor que cero.");
                                        }
                                        else if (monto > saldo)
                                        {
                                            Console.WriteLine("No tiene suficiente saldo.");
                                        }
                                        else if (monto % 100 != 0)
                                        {
                                            Console.WriteLine("El retiro debe ser múltiplo de 100.");
                                        }
                                        else
                                        {
                                            saldo = saldo - monto;
                                            cantidadRetiros = cantidadRetiros + 1;

                                            Console.WriteLine("Retiro realizado correctamente.");
                                            Console.WriteLine("Monto retirado: RD$ " + monto);
                                            Console.WriteLine("Su saldo actual es: RD$ " + saldo);
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("Debe ingresar un monto válido.");
                                    }
                                    break;
                                }

                            case 4:
                                Console.WriteLine("Gracias por utilizar nuestro cajero.");
                                break;

                            default:
                                Console.WriteLine("Opción no válida.");
                                break;
                        }
                    }

                } while (opcion != 4);
            }
        }

        // FUNCION DE DEPOSITO
        static decimal Depositar(decimal saldo)
        {
            decimal monto;

            Console.Write("Ingrese el monto a depositar: ");

            if (decimal.TryParse(Console.ReadLine(), out monto))
            {
                if (monto > 0)
                {
                    saldo = saldo + monto;

                    Console.WriteLine("Depósito exitoso.");
                    Console.WriteLine("Nuevo saldo: RD$" + saldo);
                }
                else
                {
                    Console.WriteLine("El monto debe ser mayor que cero.");
                }
            }
            else
            {
                Console.WriteLine("Debe ingresar un número válido.");
            }

            return saldo;
        }
    }
}