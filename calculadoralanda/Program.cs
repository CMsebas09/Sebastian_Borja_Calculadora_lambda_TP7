using System;
using System.Collections.Generic;
using System.Globalization;

namespace Calculadora
{
    class Program
    {
        // Declaración de variables estáticas para controlar el tipo de números y el historial de operaciones
        static bool trabajarConEnteros = true;
        static List<string> historialOperaciones = new List<string>();

        // Método principal que ejecuta la calculadora
        static void Main(string[] args)
        {
            do
            {
                // Menú principal para elegir entre trabajar con enteros, decimales, ver historial o salir
                Console.WriteLine("¿Desea trabajar con enteros (1), decimal (2), ver historial de operaciones (3), o salir (4)?");
                int tipoNumero = Convert.ToInt32(Console.ReadLine());

                if (tipoNumero == 4)
                    return; // Salir del programa si se elige la opción 4

                if (tipoNumero == 3)
                {
                    MostrarHistorial(); // Mostrar historial de operaciones si se elige la opción 3
                    continue;
                }

                trabajarConEnteros = tipoNumero == 1; // Establecer el tipo de números según la elección del usuario

                bool salir = false;

                do
                {
                    Console.WriteLine("---------------CALCULADORA------------");
                    Console.WriteLine("Introduce el primer número:");
                    double a = LeerNumero(); // Obtener el primer número

                    Console.WriteLine("Introduzca el segundo número: ");
                    double b = LeerNumero(); // Obtener el segundo número

                    Console.WriteLine("Elija una opción:");
                    Console.WriteLine("1- Sumar\n2 - Restar \n3 - Dividir \n4 - Multiplicar\n5 - Cambiar tipo de número\n6 - Recordar Operación\n7 - Salir ");
                    int opcion = Convert.ToInt32(Console.ReadLine()); // Obtener la opción de operación

                    // Guardar la operación antes de realizarla
                    GuardarOperacion(GetOperacionSimbolo(opcion), a, b);

                    // Realizar la operación seleccionada
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
                            trabajarConEnteros = !trabajarConEnteros; // Cambiar entre enteros y decimales
                            break;
                        case 6:
                            Console.WriteLine("Operación no realizada."); // Opción no implementada
                            break;
                        case 7:
                            salir = true; // Salir del bucle interno si se elige la opción 7
                            break;
                        default:
                            Console.WriteLine("Opción inválida.");
                            Console.ReadLine();
                            break;
                    }

                    // Preguntar al usuario si desea continuar con más operaciones
                    if (opcion != 5 && opcion != 7)
                    {
                        Console.WriteLine("Desea continuar: 1=si/0=no");
                        int OP = Convert.ToInt32(Console.ReadLine());
                        if (OP == 0)
                            salir = true; // Salir del bucle interno si se elige la opción 0
                    }

                } while (!salir); // Repetir hasta que se elija la opción de salir

            } while (true); // Repetir el bucle principal infinitamente
        }

        // Método para guardar la operación en el historial
        static void GuardarOperacion(string operacion, double a = 0, double b = 0)
        {
            string registro = $"Operación: {operacion} - Números: {a}, {b} - Tipo: {(trabajarConEnteros ? "Enteros" : "Decimales")}";
            historialOperaciones.Add(registro);
        }

        // Método para mostrar el historial de operaciones
        static void MostrarHistorial()
        {
            Console.WriteLine("--------- Historial de Operaciones ---------");
            foreach (var registro in historialOperaciones)
            {
                Console.WriteLine(registro);
            }
            Console.WriteLine("--------------------------------------------");
        }

        // Método para leer un número desde la entrada estándar
        static double LeerNumero()
        {
            string input = Console.ReadLine();
            if (trabajarConEnteros)
            {
                if (!EsNumeroEntero(input))
                {
                    Console.WriteLine("Error: Debe ingresar un número entero.");
                    return LeerNumero(); // Volver a pedir un número si no es un entero válido
                }
                return Convert.ToInt32(input);
            }
            else
            {
                if (!EsNumeroDecimal(input))
                {
                    Console.WriteLine("Error: Debe ingresar un número decimal.");
                    return LeerNumero(); // Volver a pedir un número si no es un decimal válido
                }
                return Convert.ToDouble(input, CultureInfo.InvariantCulture);
            }
        }

        // Método para verificar si una cadena representa un número entero
        private static bool EsNumeroEntero(string input)
        {
            int result;
            return int.TryParse(input, out result);
        }

        // Método para verificar si una cadena representa un número decimal
        private static bool EsNumeroDecimal(string input)
        {
            double result;
            return double.TryParse(input, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out result);
        }

        // Método para realizar la operación matemática seleccionada
        private static void Operacion(Func<double, double, double> operacion, double a, double b)
        {
            double result = operacion(a, b);
            if (trabajarConEnteros)
            {
                Console.WriteLine((int)result); // Mostrar el resultado como entero si se está trabajando con enteros
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

        // Método para obtener el símbolo de la operación según la opción seleccionada
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
