using System;
using System.Collections.Generic;
using System.Globalization;

namespace Calculadora
{
    class Program
    {
        static bool trabajarConEnteros = true;
        static List<string> historialOperaciones = new List<string>();

        static void Main(string[] args)
        {
            do
            {
                Console.WriteLine("¿Desea trabajar con enteros (1), decimal (2), ver historial de operaciones (3), o salir (4)?");
                int tipoNumero = Convert.ToInt32(Console.ReadLine());

                if (tipoNumero == 4)
                    return;

                if (tipoNumero == 3)
                {
                    MostrarHistorial();
                    continue;
                }

                trabajarConEnteros = tipoNumero == 1;

                bool salir = false;

                do
                {
                    Console.WriteLine("---------------CALCULADORA------------");
                    Console.WriteLine("Introduce el primer número:");
                    double a = LeerNumero();

                    Console.WriteLine("Introduzca el segundo número: ");
                    double b = LeerNumero();

                    Console.WriteLine("Elija una opción:");
                    Console.WriteLine("1- Sumar\n2 - Restar \n3 - Dividir \n4 - Multiplicar\n5 - Cambiar tipo de número\n6 - Recordar Operación\n7 - Salir ");
                    int opcion = Convert.ToInt32(Console.ReadLine());

                    // Guardar la operación antes de realizarla
                    GuardarOperacion(GetOperacionSimbolo(opcion), a, b);

                    switch (opcion)
                    {
                        case 1:
                            Operacion((x, y) => x + y, a, b);
                            break;
                        case 2:
                            Operacion((x, y) => x - y, a, b);
                            break;
                        case 3:
                            double resultadoDivision = Dividir(a, b);
                            if (!double.IsNaN(resultadoDivision))
                            {
                                Console.WriteLine(resultadoDivision);
                            }
                            break;
                        case 4:
                            Operacion((x, y) => x * y, a, b);
                            break;
                        case 5:
                            trabajarConEnteros = !trabajarConEnteros;
                            break;
                        case 6:
                            Console.WriteLine("Operación no realizada.");
                            break;
                        case 7:
                            salir = true;
                            break;
                        default:
                            Console.WriteLine("Opción inválida.");
                            Console.ReadLine();
                            break;
                    }

                    if (opcion != 5 && opcion != 7)
                    {
                        Console.WriteLine("Desea continuar: 1=si/0=no");
                        int OP = Convert.ToInt32(Console.ReadLine());
                        if (OP == 0)
                            salir = true;
                    }

                } while (!salir);

            } while (true);
        }

        static void GuardarOperacion(string operacion, double a = 0, double b = 0)
        {
            string registro = $"Operación: {operacion} - Números: {a}, {b} - Tipo: {(trabajarConEnteros ? "Enteros" : "Decimales")}";
            historialOperaciones.Add(registro);
        }

        static void MostrarHistorial()
        {
            Console.WriteLine("--------- Historial de Operaciones ---------");
            foreach (var registro in historialOperaciones)
            {
                Console.WriteLine(registro);
            }
            Console.WriteLine("--------------------------------------------");
        }

        static double LeerNumero()
        {
            string input = Console.ReadLine();
            if (trabajarConEnteros)
            {
                if (!EsNumeroEntero(input))
                {
                    Console.WriteLine("Error: Debe ingresar un número entero.");
                    return LeerNumero();
                }
                return Convert.ToInt32(input);
            }
            else
            {
                if (!EsNumeroDecimal(input))
                {
                    Console.WriteLine("Error: Debe ingresar un número decimal.");
                    return LeerNumero();
                }
                return Convert.ToDouble(input, CultureInfo.InvariantCulture);
            }
        }

        private static bool EsNumeroEntero(string input)
        {
            int result;
            return int.TryParse(input, out result);
        }

        private static bool EsNumeroDecimal(string input)
        {
            double result;
            return double.TryParse(input, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out result);
        }

        private static double Dividir(double a, double b)
        {
            if (b == 0)
            {
                Console.WriteLine("Error: No se puede dividir por cero.");
                return double.NaN;
            }
            else
            {
                return a / b;
            }
        }

        private static void Operacion(Func<double, double, double> operacion, double a, double b)
        {
            double result = operacion(a, b);
            if (trabajarConEnteros)
            {
                Console.WriteLine((int)result);
            }
            else
            {
                string resultString = result.ToString(CultureInfo.InvariantCulture);
                int decimalIndex = resultString.IndexOf('.');
                if (decimalIndex != -1)
                {
                    string parteDecimal = resultString.Substring(decimalIndex);
                    if (parteDecimal.Length > 1)
                        Console.WriteLine("0" + parteDecimal);
                    else
                        Console.WriteLine("0,0");
                }
                else
                {
                    Console.WriteLine("0,0");
                }
            }
        }

        private static string GetOperacionSimbolo(int opcion)
        {
            switch (opcion)
            {
                case 1:
                    return "+";
                case 2:
                    return "-";
                case 3:
                    return "/";
                case 4:
                    return "*";
                case 5:
                    return "Cambio de tipo";
                case 6:
                    return "Recordar operación";
                case 7:
                    return "Salir";
                default:
                    return "Opción inválida";
            }
        }
    }
}
