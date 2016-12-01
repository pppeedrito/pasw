using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Practica_pasw
{
    class Program
    {
        const string ArchivoPassw = "ArchivoPass.txt";
        static void Main(string[] args)
        {
            //creamos la carpeta donde se guardara el paasww
            if (!(File.Exists(ArchivoPassw)))
            {
                using (File.Create(ArchivoPassw))
                {

                }
            }
            int Resposta;
            string[] GuardarUsuario = new string[10];
            do
            {

                //creamos un menu chulo y controlamos los posibles errores. 
                Console.WriteLine("***********************************************************");
                Console.WriteLine("              Bienvenido al menu de autenticación          ");
                Console.WriteLine("***********************************************************");
                Console.WriteLine("     Introduce unas de las siguientes acciones: ");
                Console.WriteLine("                      1->Registrarse");
                Console.WriteLine("                      2->Validarse");
                Console.WriteLine("                      3->SALIR  ! ");
                Console.WriteLine("***********************************************************");
                try
                {
                    Resposta = Convert.ToInt32(Console.ReadLine());

                }
                catch (Exception)
                {
                    Console.WriteLine("Valor Erroneo.Vuelve a probar:");
                    Resposta = Convert.ToInt32(Console.ReadLine());
                }


                switch (Resposta)
                {
                    case 1: registrarse(); break;
                    case 2: validar(); break;

                }
                if (Resposta > 3 || Resposta < 1)
                {
                    Console.WriteLine("    Valor introducido erroneo, vuelve a probar : ");

                    if (Resposta == 3)
                    {
                        Console.WriteLine("Has sortit."); break;
                    }
                }
            } while (Resposta != 3);
        }
        


                
            
            

        private static void validar()
        {
            //Inicializamos las variable
            string password;
            string nombre;
            byte[] saltLeido;

            string[] fichero;

            
            //Le preguntamos su nombre y lo guardamos en el archivo.text (con una , al final)
            Console.WriteLine("Escribe tu nombre. \n");
            nombre = Console.ReadLine();

            //Le preguntamos su password,
            Console.WriteLine("Escribe tu contraseña. \n");
            password = EntraPassword();

            /////Cojo el salt del archivo y calculo el hash con la contraseña que introduzca y la comparo con el hash del archivo
            string apartado = LlegiUsuari(nombre);


            fichero = apartado.Split(',');
            saltLeido = Convert.FromBase64String(fichero[1]);


            //Llamamos a la funcion calcular hash, con el password y el salt concatenado
            string hashString = CalculaHash(password, saltLeido);



            if (hashString == fichero[2])
            {
                Console.WriteLine("\nUsuario autentificado.\n");
            }
            else
            {
                Console.WriteLine("\nUsuario o la contraseña no son validos.\n");
            }
        }
    

        private static void registrarse()
        {//Inicializamos las variable
            string Nombre;
            string Contraseña;
            String NombreExistente;
            string[] Almacen;

            //Le preguntamos su nombre y lo guardamos en el ArchivoPass.text y por el nombre -> (nombre,)
            Console.WriteLine("Escribe tu nombre de usuario: \n");
            Nombre = Console.ReadLine();

            string ARCHIVO = LlegiUsuari(Nombre);

            Almacen = ARCHIVO.Split(',');
            NombreExistente = (Almacen[0]);

            //controlamos que el nombre no este vacio o  no sea el correcto
            if (NombreExistente == "404" || NombreExistente == null)
            {
                //Creamos el salt
                byte[] salt;
                new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);

                //Le preguntamos su password y se calcula el hash y lo guardamos en el ArchivoPass.text  y por el pass (passw,)
                Console.WriteLine("Escribe tu contraseña: \n");
                Contraseña  = EntraPassword();

                //Llamamos a la funcion calcular hash, con el password y el salt concatenado
                string HashString = CalculaHash(Contraseña, salt);


                //Aqui usamos la constante (Inicializada arriba con el nombre del archivo.) para escribir 
                //en el archivo.txt el nombre, el salt(convertida a String) y el hash del password
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(ArchivoPassw, true))
                {
                    file.WriteLine(Nombre + ',' + Convert.ToBase64String(salt) + ',' + HashString);
                }
                Console.WriteLine("\nUsuario registrado correctamente.\n");
            }
            else
            {
                Console.WriteLine("\nEl nombre de este usuario ya existe.\n");
            }
        }

        private static string EntraPassword()
        {
            //cambia el pasww por * para no mostrar la constraseña
            string password = null;
            ConsoleKeyInfo key;
            do
            {
                key = Console.ReadKey(true);

                if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                {
                    password += key.KeyChar;
                    Console.Write("*");
                }
                else if (key.Key == ConsoleKey.Backspace)
                {
                    Console.Write("\b \b");

                }
            }
            while (key.Key != ConsoleKey.Enter);
            return password;
        }


        private static string LlegiUsuari(string nombre)
        {
            {
                try
                {
                    using (StreamReader lector = new StreamReader(ArchivoPassw))
                    {
                        //Este while se usa para que vaya leiendo las lineas del archivo
                        while (lector.Peek() > -1)
                        {
                            string linia = lector.ReadLine();
                            string[] user;
                            if (!String.IsNullOrEmpty(linia))
                            {
                                //Separamos nombre y  hash
                                user = linia.Split(',');
                                //Comprueba que el nombre del usuario exista
                                if (user[0].Equals(nombre))
                                {
                                    return user[0] + ',' + user[1] + ',' + user[2];
                                }
                            }
                        }

                    }
                    // devuelve el usuario.
                    return "404";

                }
                catch
                {
                    // devolvemos nulo para indicar que no se ha podido lear el fichaero
                    return null;
                }

            }



            
    }
        /////retorna un string amb el HASH resultat o null si hi ha error
        static string CalculaHash(string pass, byte[] salt)
        {

            try
            {
                //calculamos el hash con el password y el sal juntos
                var calcular = new Rfc2898DeriveBytes(pass, salt, 10000);
                byte[] Hash = calcular.GetBytes(32);
                return Convert.ToBase64String(Hash);
            }
            catch (Exception)
            {
                Console.WriteLine("Error calculant el hash");
                Console.ReadKey(true);
                return null;
            }
        }

    }
}

    


    

