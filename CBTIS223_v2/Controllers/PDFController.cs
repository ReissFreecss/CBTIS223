using CBTIS223_v2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace CBTIS223_v2.Controllers
{
    public class PDFController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IWebHostEnvironment _host;
        private cbtis223Context _context;
        public Escuela modeloE { get; set; }


        public PDFController(ILogger<HomeController> logger, IWebHostEnvironment host, cbtis223Context context)
        {
            _logger = logger;
            _host = host;
            _context = context;
        }

        private string ObtenerIniciales(string nombre)
        {
            if (string.IsNullOrEmpty(nombre))
            {
                return string.Empty;
            }

            // Dividir el nombre en palabras
            var palabras = nombre.Split(' ');

            // Tomar la primera letra de cada palabra y concatenarlas
            string iniciales = string.Join("", palabras.Select(p => p[0]));

            return iniciales;
        }


        //Metodo que genera los PDF Hoja de liberacion servicio social
        public ActionResult DescargarPDF()
        {
            //Extraemos los datos de la escuela
            int IDEscuela = 1;
            var escuela = _context.Escuelas.FirstOrDefault(x => x.ID  == IDEscuela);

            //Extraemos fechas actuales
            string añoActual = DateTime.Now.Year.ToString();
            string diaActual = DateTime.Now.Day.ToString();
            string mesActual = DateTime.Now.ToString("MMMM");

            //Extraemos numero de folio
            string NoFolio = Request.Form["noConstancia"];

            //Obtener las iniciales junto con su nombre
            string nombreDirector = escuela.NombreDirector;
            string iniciales = ObtenerIniciales(nombreDirector);

            string tipoI = Request.Form["SelectorSP"];
            if (tipoI == "S")
            {
                string tipo = Request.Form["SelectoAL"];
                if (tipo == "1")
                {
                    //Obtenemos datos del form
                    string idEstudiante = Request.Form["idEstudiante"];
                    

                    try
                    {
                        //Realizamos busqueda de acuerdo a los datos enviados por el form para la tabla estudiantes
                        var alumno = _context.EstudiantesServicios.FirstOrDefault(x => x.NumeroControl == idEstudiante);

                        //En caso de no encotrar mandamos mensaje
                        if (alumno == null)
                        {
                            throw new Exception("El estudiante no se encuentra en la base de datos.");
                        }

                        //Realizamos busqueda de acuerdo a los datos enviados por el form para la tabla Servicio social
                        var servicioSocial = _context.ServicioSocials.FirstOrDefault(x => x.EstudianteNc == idEstudiante);

                        //En caso de no encotrar mandamos mensaje
                        if (servicioSocial == null)
                        {
                            throw new Exception("El servicio social del estudiante no se encuentra en la base de datos.");
                        }

                        //Transformo los datos de fecha a string

                        DateTime FechaInicio = servicioSocial.FechaInicioServicio;
                        DateTime FechaTermino = servicioSocial.FechaTerminoServicio;


                        // Generamos PDf Hoja de liberacion
                        var data = Document.Create(formato_CP_SS =>
                        {
                            formato_CP_SS.Page(page =>
                            {
                                page.Margin(30);

                                var textoArial = TextStyle
                                .Default.FontFamily(Fonts.Arial).FontSize(11.5f);

                                // Encabezado del documento PDF
                                //Fila para el encabezado: Logos de instituciones y datos del plantel
                                page.Header().Row(row =>
                                {
                                    //Fila para el logo de la SEP y DGETi
                                    row.ConstantItem(350).MaxWidth(350).MaxHeight(40).Placeholder();


                                    //Fila con columnas interrnas para los datos del plantel CBTis No. 223
                                    row.RelativeItem().Column(col =>
                                    {
                                        col.Item().AlignRight()
                                        .Text("Subsecretaría de Educación Media Superior")
                                        .FontFamily(Fonts.Tahoma).Bold().FontSize(8);

                                        col.Item().AlignRight()
                                        .Text("Dirección General de Educación Tecnológica Industrial y Servicios")
                                        .FontFamily(Fonts.Tahoma).FontSize(6);

                                        col.Item().AlignRight()
                                        .Text("Centro de Bachillerato Tecnológico industrial y de servicios No. 223")
                                        .FontFamily(Fonts.Tahoma).FontSize(6);

                                        col.Item().AlignRight()
                                        .Text("“MIGUEL HIDALGO Y COSTILLA”")
                                        .FontFamily(Fonts.Tahoma).FontSize(6);
                                    });
                                });

                                //Contenido del documento PDF
                                page.Content().Column(col1 =>
                                {
                                    //Columna para el No. de Constancia
                                    //Texto personalizado para el No. de constancia,plantel,año
                                    col1.Item().DefaultTextStyle(textoArial).Padding(10).AlignRight().Text(txt =>
                                    {
                                        //No. de oficio //Modificable
                                        txt.Span("\nOFICIO No. ");
                                        txt.Span(NoFolio);//Modificable
                                        txt.Span("(CBTis 223) 087");
                                        txt.Span(" /2023"); //Año en curso - Modificable

                                        //Texto personalizado para el Asunto
                                        txt.Span("\nAsunto: Carta de presentación de");
                                        txt.Span("\nServicio Social.");//Servicio Social o Prácticas //Modificable


                                        //Texto personalizado para la fecha
                                        txt.Span("\nGaleana, Zacatepec, Mor; ");      //Lugar //Modificable

                                        txt.Span("a " + diaActual+ " de " + mesActual + " del " + añoActual);   //Fecha //Modificable
                                    });


                                    //Columna los datos del Director General y la dirección
                                    col1.Item().DefaultTextStyle(textoArial).Padding(10).AlignLeft().Text(txt =>
                                    {

                                        txt.Span("JUAN CARLOS CABRERA ZUÑIGA").SemiBold();//Nombre

                                        txt.Span("\nDIRECTOR GENERAL").SemiBold();

                                        txt.Span("\nCONSORCIO BARCAZÚ").SemiBold();//


                                        txt.Span("\nPRESENTE").SemiBold().LetterSpacing(0.2f);//Inicio de la carta de liberación
                                    });


                                    col1.Item().DefaultTextStyle(textoArial).AlignCenter().Padding(10)
                                    .Text("La Dirección del CENTRO DE BACHILLERATO TECNOLÓGICO " +
                                        "industrial y de servicios No. 223, se permite presentar " +
                                        "a sus finas atenciones al alumno:").LetterSpacing(0.04f);//Nombre del alumno Modificable

                                    col1.Item().DefaultTextStyle(textoArial).AlignCenter().Padding(10)
                                    .Text(alumno.Nombre + " " + alumno.Apellidos).Bold();//Nombre del alumno Modificable


                                    //Texto personalizado para el primer parrafo del contenido
                                    col1.Item().DefaultTextStyle(textoArial).Padding(10).Text(txt =>
                                    {
                                        txt.Span("De la especialidad de ")
                                        .LetterSpacing(0.04f);
                                        txt.Span(alumno.Especialidad)// Especilidad Modificable
                                       .LetterSpacing(0.05f).Bold();

                                        txt.Span(" con número de control: ")//Texto previo al No. contol
                                        .LetterSpacing(0.05f);

                                        txt.Span(alumno.NumeroControl)//No.Control del alumno
                                        .Bold().LetterSpacing(0.02f); //Modificable

                                        txt.Span(", quien desea realizar su Servicio Social " +
                                            "en la empresa que usted dignamente representa.")//Texto después del No.Control
                                        .LetterSpacing(0.05f); //Modificable
                                    });

                                    //Columna para el texto previo y periodo prestado
                                    col1.Item().DefaultTextStyle(textoArial).Padding(10).Text(txt =>
                                    {
                                        txt.Span("El (la) alumno (a) antes mencionado (a) permanecerá " +
                                            "durante el periodo comprendido del ").LetterSpacing(0.05f);

                                        txt.Span((FechaInicio.ToString("dd MMMM"+" Del "+ "yyyy")));//Día y Mes de inicio
                                                                                                    //Modificable

                                        txt.Span(" al ");

                                        txt.Span((FechaTermino.ToString("dd MMMM"+" Del "+ "yyyy")));//Día, Mes  y Año de termino
                                                                                                     //Modificable

                                        txt.Span(", " + servicioSocial.actividad_servicio).Bold().LetterSpacing(0.02f); ;
                                    });


                                    //Columna el texto posterior a las fechas y horarios
                                    col1.Item().DefaultTextStyle(textoArial).Padding(10)
                                    .Text("En espera de informes, evaluación y constancia de nuestro participante" +
                                    ", le agradezco de antemano las atenciones que sirva a brindar al portador de" +
                                    "la presente, sin otro particular quedo de usted a sus respetables órdenes.").LetterSpacing(0.04f);



                                    col1.Item().DefaultTextStyle(textoArial).Padding(10)
                                    .Text("ATENTAMENTE").SemiBold().LetterSpacing(0.04f);


                                    //Columna para el espacio de firma
                                    col1.Item().DefaultTextStyle(textoArial).Padding(10).AlignLeft().Text(txt =>
                                    {
                                        txt.Span("\n\n\n").Underline();

                                        //Columna para la firma del Director del Plantel
                                        txt.Span(escuela.NombreDirector).SemiBold();//Editable
                                        txt.Span("\nDIRECTOR").SemiBold();
                                    });

                                });//Fin de la sección del Contenido


                                //Contenido de la piecera PDF
                                page.Footer().Column(col1 =>
                                {
                                    //NUEVA COLUMNA PARA PONER DATOS DE ARCHIVO
                                    col1.Item().AlignLeft().Padding(10).Text(txt =>
                                    {
                                        txt.Span("c.c.p - Archivo").FontFamily(Fonts.Arial).FontSize(6); ;
                                        txt.Span("\n"+iniciales+"/DIVL/CCG/*asa\n").FontFamily(Fonts.Arial).FontSize(6); ;
                                    });

                                    col1.Item().AlignLeft()
                                    .Text("Calle No Reeleción S/N, Col Lázaro Cárdenas," +
                                    "Galeana, Zacatepec, Mor. C.P. 62785").FontColor("#C59452")
                                    .FontFamily(Fonts.Tahoma).FontSize(6);
                                    col1.Item().AlignLeft()
                                        .Text("Tel. 7343434538 Correo electrónico: " +
                                        "cbtis223.escolares223.escolares@dgeti.sems.gob.mx").FontColor("#C59452")
                                        .FontFamily(Fonts.Tahoma).FontSize(6);

                                    col1.Item().Row(row2 =>
                                    {
                                        //Fila para la imagen de la piecera
                                        row2.ConstantItem(530).Height(60).Placeholder();
                                    });
                                });
                            });

                        }).GeneratePdf();

                        Stream stream = new MemoryStream(data);
                        return File(stream, "application/pdf", "Hoja Servicio Presentacion " + idEstudiante +".pdf");
                    }
                    catch (Exception ex)
                    {
                        // Maneja la excepción, por ejemplo, registrando un mensaje de error
                        // o redirigiendo a una vista de error
                        _logger.LogError(ex, "No se encontro y buscas un registro inexi");
                        return RedirectToAction("Error", "Home", new { message = ex.Message });
                    }
                }
                else
                {
                    //Obtenemos datos del form
                    string idEstudiante = Request.Form["idEstudiante"];

                    try
                    {
                        //Realizamos busqueda de acuerdo a los datos enviados por el form para la tabla estudiantes
                        var alumno = _context.EstudiantesServicios.FirstOrDefault(x => x.NumeroControl == idEstudiante);

                        //Realizamos busqueda de acuerdo a los datos enviados por el form para la tabla Servicio social
                        var servicioSocial = _context.ServicioSocials.FirstOrDefault(x => x.EstudianteNc == idEstudiante);
                       

                        //En caso de no encotrar mandamos mensaje
                        if (servicioSocial == null)
                        {
                            throw new Exception("El servicio social del estudiante no se encuentra en la base de datos.");
                        }

                        //Transformo los datos de fecha a string

                        DateTime FechaInicio = servicioSocial.FechaInicioServicio;
                        DateTime FechaTermino = servicioSocial.FechaTerminoServicio;


                        // Generamos PDf Hoja de liberacion
                        var data = Document.Create(document =>
                        {
                            document.Page(page =>
                            {
                                page.Margin(30);

                                // Encabezado del documento PDF
                                //Fila para el encabezado: Logos de instituciones y datos del plantel
                                page.Header().Row(row =>
                                {
                                    //Importar y transformar en arrays de bytes las imagenes del header
                                    var rutaHEADER = Path.Combine(_host.WebRootPath, "imagenes/HEADER.jpg");
                                    byte[] imageHEADER = System.IO.File.ReadAllBytes(rutaHEADER);

                                    //Fila para el logo de la SEP y DGETi
                                    //row.ConstantItem(350).Height(40).Image(imageHEADER);
                                    row.ConstantItem(350).Height(40).Placeholder();


                                    //Fila con columnas interrnas para los datos del plantel CBTis No. 223
                                    row.RelativeItem().Column(col =>
                                    {
                                        col.Item().AlignRight()
                                        .Text("Subsecretaría de Educación Media Superior")
                                        .FontFamily("Tahoma").Bold().FontSize(8);

                                        col.Item().AlignRight()
                                        .Text("Dirección General de Educación Tecnológica Industrial y Servicios")
                                        .FontFamily("Tahoma").FontSize(6);

                                        col.Item().AlignRight()
                                        .Text("Centro de Bachillerato Tecnológico industrial y de servicios No. 223")
                                        .FontFamily("Tahoma").FontSize(6);

                                        col.Item().AlignRight()
                                        .Text("“MIGUEL HIDALGO Y COSTILLA”")
                                        .FontFamily("Tahoma").FontSize(6);
                                    });
                                });

                                //Contenido del documento PDF
                                //Columna para el No. de Constancia
                                page.Content().Column(async col1 =>
                                {
                                    //Texto personalizado para el No. de constancia,plantel,año
                                    col1.Item().AlignRight().Text(txt =>
                                    {
                                        txt.Span("\n CONSTANCIA NO.: ").SemiBold()
                                        .FontFamily("Arial").FontSize(12);

                                        txt.Span(NoFolio).SemiBold()  //No. de constancia
                                        .FontFamily("Arial").FontSize(12); //Modificable

                                        txt.Span(" (CBTIS No.223)").SemiBold() //Plantel
                                        .FontFamily("Arial").FontSize(12); //Modificable

                                        txt.Span(añoActual).SemiBold()        //Año
                                        .FontFamily("Arial").FontSize(12); //Modificable
                                    });

                                    //Texto para el asunto
                                    col1.Item().AlignRight().Text("Asunto: CONSTANCIA DE ACREDITACIÓN ")
                                    .SemiBold().FontFamily("Arial").FontSize(12);
                                    //Texto personalizado para el Asunto
                                    col1.Item().AlignRight().Text(txt =>
                                    {
                                        txt.Span("DE SERVICIO SOCIAL").SemiBold()//Servicio Social o Prácticas
                                        .FontFamily("Arial").FontSize(12);       //Modificable
                                    });


                                    //Columna para la Fecha de expedición
                                    //Texto personalizado para la fecha
                                    col1.Item().Padding(8).AlignRight().Text(txt =>
                                    {
                                        txt.Span("Fecha: ")
                                        .FontFamily("Arial").FontSize(12);

                                        txt.Span("Galeana Morelos, ")      //Lugar
                                        .FontFamily("Arial").FontSize(12); //Modificable

                                        txt.Span("a " + diaActual + " de " + mesActual + " del " + añoActual)   //Fecha
                                        .FontFamily("Arial").FontSize(12); //Modificable
                                    });

                                    //Columna los datos del Director General
                                    col1.Item().AlignLeft().Text("RUBÉN NÚÑEZ MERCADO")
                                    .SemiBold().FontFamily("Arial").FontSize(12);//Nombre

                                    col1.Item().AlignLeft().Text("C. DIRECTOR GENERAL DE PROFESIONES")
                                    .SemiBold().FontFamily("Arial").FontSize(12);//
                                                                                 //Columnas de la dirección
                                    col1.Item().AlignLeft().Text("Viaducto Rio de la Piedad No.551,")
                                    .FontFamily("Arial").FontSize(12);//

                                    col1.Item().AlignLeft().Text("Col. Magdalena Mixhuca,")
                                    .FontFamily("Arial").FontSize(12);//Colonia

                                    col1.Item().AlignLeft().Text("Alcaldía Venustiano Carranza")
                                    .FontFamily("Arial").FontSize(12);//

                                    col1.Item().AlignLeft().Text("C.P. 15860, en la Ciudad de México")
                                    .FontFamily("Arial").FontSize(12);//CódigoPostal y Ciudad

                                    col1.Item().AlignLeft().Text("PRESENTE.")
                                    .Bold().FontFamily("Arial").FontSize(12);//Inicio de la carta de liberación

                                    //Columna para la carta con partes editables para datos del alumno
                                    //Texto personalizado para la carta y datos del alumno
                                    col1.Item().Text(txt =>
                                    {
                                        txt
                                        .Span("\nEl Director del Centro de Estudios Tecnológicos Industrial " +
                                        "y de Servicios No. ")
                                        .FontFamily("Arial").FontSize(12).LetterSpacing(0.05f);

                                        txt.Span("223.")                     //No. Plantel
                                        .FontFamily("Arial").FontSize(12).LetterSpacing(0.05f); //Modificable

                                        txt.Span("      Ubicado en: ")
                                        .FontFamily("Arial").FontSize(12).LetterSpacing(0.05f);

                                        txt.Span("Calle No Reelección S/N, Col. Lázaro Cárdenas Galeana, " +
                                            "Municipio de Zacatepec, Morelos, C.P. 62785.")//Dirección del Plantel
                                        .FontFamily("Arial").FontSize(12).LetterSpacing(0.05f);                 //Modificable

                                        txt.Span("Dependiente de la Dirección General de Educación Tecnológica Industrial " +
                                            "y de Servicios, en cumplimiento de las disposiciones relativas a la Ley " +
                                            "Reglamentaria del artículo 5° Constitucional y de lo señalado en el reglamento para" +
                                            "la prestación ")
                                        .FontFamily("Arial").FontSize(12).LetterSpacing(0.05f);

                                        txt.Span("del servicio social ")//Asunto(Servicio o Prácticas)
                                        .FontFamily("Arial").FontSize(12).LetterSpacing(0.05f); //Modificable

                                        txt.Span("de la propia Unidad, hace constar que según Documentos que operan en el plantel " +
                                            "el (la) alumno (a): ")
                                        .FontFamily("Arial").FontSize(12).LetterSpacing(0.05f);

                                        txt.Span(alumno.Nombre + " " + alumno.Apellidos) //Nombre del alumno
                                        .Bold().FontFamily("Arial").FontSize(12); //Modificable

                                        txt.Span(" No. De control: ")//Texto previo al No. contol
                                        .FontFamily("Arial").FontSize(12).LetterSpacing(0.05f);

                                        txt.Span(alumno.NumeroControl)//No.Control del alumno
                                        .Bold().FontFamily("Arial").FontSize(12).LetterSpacing(0.02f); //Modificable

                                        txt.Span(", ha prestado su servicio social correspondiente a la carrera de:")//Texto después del No.Control
                                        .FontFamily("Arial").FontSize(12).LetterSpacing(0.05f); //Modificable
                                    });

                                    //Columna para la carrera
                                    //Texto personalizado para la carrera
                                    col1.Item().AlignCenter().Text(txt =>
                                    {
                                        txt.Span("\n TÉCNICO EN ")
                                        .Bold().Underline().FontFamily("Arial").FontSize(12);

                                        txt.Span(alumno.Especialidad)      //Carrera
                                        .Bold().Underline().FontFamily("Arial").FontSize(12);//Modificable
                                    });

                                    //Columna las fechas referentes al periodo y el servicio prestado
                                    //Texto personalizado el periodo y la descripción del servicio prestado
                                    col1.Item().Text(txt =>
                                    {
                                        txt.Span("\nDurante el periodo comprendido del ")
                                        .FontFamily("Arial").FontSize(12).LetterSpacing(0.02f); ;

                                        txt.Span((FechaInicio.ToString("dd MMMM"+" Del "+ "yyyy")) + " al ")      //Periodo Inicio
                                        .FontFamily("Arial").FontSize(12).LetterSpacing(0.02f); ;//Modificable

                                        txt.Span((FechaTermino.ToString("dd MMMM"+" Del "+ "yyyy")))      //Periodo Fin
                                        .FontFamily("Arial").FontSize(12).LetterSpacing(0.02f); ;//Modificable

                                        txt.Span(" Consistiendo en: ")      //Texto
                                        .FontFamily("Arial").FontSize(12).LetterSpacing(0.02f); ;

                                        txt.Span(servicioSocial.actividad_servicio)      //Descripción Servicio Prestado
                                        .FontFamily("Arial").FontSize(12).LetterSpacing(0.02f); ;//Modificable
                                    });


                                    //Columna para el espacio de firma
                                    col1.Item().AlignCenter().
                                    Text("\n" +
                                    "________________________________________________________")
                                    .Underline();
                                    //Columna para la firma del Director del Plantel
                                    col1.Item().AlignCenter().Text(escuela.NombreDirector)//Editable
                                    .FontFamily("Arial").FontSize(12); ;
                                    col1.Item().AlignCenter().Text("DIRECTOR DEL PLANTEL ")
                                    .FontFamily("Arial").FontSize(12);


                                    //Leyenda de la firma auténtica

                                    col1.Item().Padding(4).ContentFromLeftToRight().Text("El C. Asistente de la Oficina Estatal de la Dirección General de Educación Tecnológica" +
                                        " Industrial y de Servicios en Morelos certifica que la firma que antecede es auténtica.")
                                    .FontFamily("Arial").FontSize(12).LetterSpacing(0.02f);


                                    //Columna para el espacio de firma
                                    col1.Item().AlignCenter().
                                    Text("\n" +
                                    "________________________________________________________")
                                    .Underline();
                                    //Columna para la firma del Director del Plantel
                                    col1.Item().AlignCenter().Text("JOSE MANUEL CORONEL CUEVAS")//Editable
                                    .FontFamily("Arial").FontSize(12); ;
                                    col1.Item().AlignCenter().Text("ASISTENTE ACADÉMICO DE LA OFICINA ESTATAL")
                                    .FontFamily("Arial").FontSize(12);
                                    col1.Item().AlignCenter().Text("DE LA DGETI EN MORELOS")
                                    .FontFamily("Arial").FontSize(12);


                                    col1.Item().AlignLeft()
                                        .Text("\n\nCalle No Reeleción S/N, Col Lázaro Cárdenas," +
                                        "Galeana, Zacatepec, Mor. C.P. 62785").FontColor("#C59452")
                                        .FontFamily("Tahoma").FontSize(6);
                                    col1.Item().AlignLeft()
                                        .Text("Tel. 7343434538 Correo electrónico: " +
                                        "cbtis223.escolares223.escolares@dgeti.sems.gob.mx").FontColor("#C59452")
                                        .FontFamily("Tahoma").FontSize(6);
                                });//Fin de la sección del Contenido



                                //Contenido de la piecera PDF
                                page.Footer().Row(row2 =>
                                {

                                    //Importar y transformar en arrays de bytes la imagen del footer
                                    var rutafooter = Path.Combine(_host.WebRootPath, "imagenes/FOOTER.png");
                                    byte[] imageFooter = System.IO.File.ReadAllBytes(rutafooter);

                                    //Fila para la imagen de la piecera
                                    //row2.ConstantItem(530).Height(50).Image(imageFooter);
                                    row2.ConstantItem(530).Height(50).Placeholder();

                                });
                            });

                        }).GeneratePdf();

                        Stream stream = new MemoryStream(data);
                        return File(stream, "application/pdf", "Hoja Servicio Liberacion " + idEstudiante +".pdf");
                    }
                    catch (Exception ex)
                    {
                        // Maneja la excepción, por ejemplo, registrando un mensaje de error
                        // o redirigiendo a una vista de error
                        _logger.LogError(ex, "No se encontro y buscas un registro inexi");
                        return RedirectToAction("Error", "Home", new { message = ex.Message });
                    }
                }
            }
            else
            {
                string tipo = Request.Form["SelectoAL"];
                if (tipo == "1")
                {
                    //Obtenemos datos del form
                    string idEstudiante = Request.Form["idEstudiante"];

                    try
                    {
                        //Realizamos busqueda de acuerdo a los datos enviados por el form para la tabla estudiantes
                        var alumno = _context.EstudiantesServicios.FirstOrDefault(x => x.NumeroControl == idEstudiante);
                        var alumnoP = _context.EstudiantesPracticas.FirstOrDefault(x => x.EstudianteNc == idEstudiante);

                        //En caso de no encotrar mandamos mensaje
                        if (alumno == null)
                        {
                            throw new Exception("El estudiante no se encuentra en la base de datos.");
                        }

                        //Realizamos busqueda de acuerdo a los datos enviados por el form para la tabla Servicio social
                        var servicioPracticas = _context.PracticasProfesionales.FirstOrDefault(x => x.EstudianteNc == idEstudiante);

                        //En caso de no encotrar mandamos mensaje
                        if (servicioPracticas == null)
                        {
                            throw new Exception("El servicio social del estudiante no se encuentra en la base de datos.");
                        }

                        //Transformo los datos de fecha a string

                        DateTime FechaInicio = servicioPracticas.FechaInicioPracticas;
                        DateTime FechaTermino = servicioPracticas.FechaTerminoPracticas;


                        // Generamos PDf Hoja de liberacion
                        var data = Document.Create(formato_CP_SS =>
                        {
                            formato_CP_SS.Page(page =>
                            {
                                page.Margin(30);

                                var textoArial = TextStyle
                                .Default.FontFamily(Fonts.Arial).FontSize(11.5f);

                                // Encabezado del documento PDF
                                //Fila para el encabezado: Logos de instituciones y datos del plantel
                                page.Header().Row(row =>
                                {
                                    //Fila para el logo de la SEP y DGETi
                                    row.ConstantItem(350).MaxWidth(350).MaxHeight(40).Placeholder();


                                    //Fila con columnas interrnas para los datos del plantel CBTis No. 223
                                    row.RelativeItem().Column(col =>
                                    {
                                        col.Item().AlignRight()
                                        .Text("Subsecretaría de Educación Media Superior")
                                        .FontFamily(Fonts.Tahoma).Bold().FontSize(8);

                                        col.Item().AlignRight()
                                        .Text("Dirección General de Educación Tecnológica Industrial y Servicios")
                                        .FontFamily(Fonts.Tahoma).FontSize(6);

                                        col.Item().AlignRight()
                                        .Text("Centro de Bachillerato Tecnológico industrial y de servicios No. 223")
                                        .FontFamily(Fonts.Tahoma).FontSize(6);

                                        col.Item().AlignRight()
                                        .Text("“MIGUEL HIDALGO Y COSTILLA”")
                                        .FontFamily(Fonts.Tahoma).FontSize(6);
                                    });
                                });

                                //Contenido del documento PDF
                                page.Content().Column(col1 =>
                                {
                                    //Columna para el No. de Constancia
                                    //Texto personalizado para el No. de constancia,plantel,año
                                    col1.Item().DefaultTextStyle(textoArial).Padding(10).AlignRight().Text(txt =>
                                    {
                                        //No. de oficio //Modificable
                                        txt.Span("\nOFICIO No. ");
                                        txt.Span(NoFolio);//Modificable
                                        txt.Span("(CBTis 223) 087 ");
                                        txt.Span(añoActual); //Año en curso - Modificable

                                        //Texto personalizado para el Asunto
                                        txt.Span("\nAsunto: Carta de presentación de");
                                        txt.Span("\nPrácticas Profesionales.");//Servicio Social o Prácticas //Modificable


                                        //Texto personalizado para la fecha
                                        txt.Span("\nGaleana, Zacatepec, Mor; ");      //Lugar //Modificable

                                        txt.Span("a " + diaActual + " de " + mesActual + " del " + añoActual);   //Fecha //Modificable
                                    });


                                    //Columna los datos del Director General y la dirección
                                    col1.Item().DefaultTextStyle(textoArial).Padding(10).AlignLeft().Text(txt =>
                                    {

                                        txt.Span("JUAN CARLOS CABRERA ZUÑIGA").SemiBold();//Nombre

                                        txt.Span("\nDIRECTOR GENERAL").SemiBold();

                                        txt.Span("\nCONSORCIO BARCAZÚ").SemiBold();//


                                        txt.Span("\nPRESENTE").SemiBold().LetterSpacing(0.2f);//Inicio de la carta de liberación
                                    });


                                    col1.Item().DefaultTextStyle(textoArial).AlignCenter().Padding(10)
                                    .Text("La Dirección del CENTRO DE BACHILLERATO TECNOLÓGICO " +
                                        "industrial y de servicios No. 223, se permite presentar " +
                                        "a sus finas atenciones al alumno:").LetterSpacing(0.04f);//Nombre del alumno Modificable

                                    col1.Item().DefaultTextStyle(textoArial).AlignCenter().Padding(10)
                                    .Text(alumno.Nombre + " " + alumno.Apellidos).Bold();//Nombre del alumno Modificable


                                    //Texto personalizado para el primer parrafo del contenido
                                    col1.Item().DefaultTextStyle(textoArial).Padding(10).Text(txt =>
                                    {
                                        txt.Span("De la especialidad de ")
                                        .LetterSpacing(0.04f);
                                        txt.Span(alumno.Especialidad)// Especilidad Modificable
                                       .LetterSpacing(0.05f).Bold();

                                        txt.Span(" con número de control: ")//Texto previo al No. contol
                                        .LetterSpacing(0.05f);

                                        txt.Span(alumno.NumeroControl)//No.Control del alumno
                                        .Bold().LetterSpacing(0.02f); //Modificable

                                        txt.Span(", quien desea realizar sus Prácticas Profesionales " +
                                            "en la empresa que usted dignamente representa.")//Texto después del No.Control
                                        .LetterSpacing(0.05f); //Modificable
                                    });

                                    //Columna para el texto previo y periodo prestado
                                    col1.Item().DefaultTextStyle(textoArial).Padding(10).Text(txt =>
                                    {
                                        txt.Span("El (la) alumno (a) antes mencionado (a) permanecerá " +
                                            "durante el periodo comprendido del ").LetterSpacing(0.05f);

                                        txt.Span((FechaInicio.ToString("dd MMMM"+" Del "+ "yyyy")));//Día y Mes de inicio
                                                                 //Modificable

                                        txt.Span(" al ");

                                        txt.Span((FechaTermino.ToString("dd MMMM"+" Del "+ "yyyy")));//Día, Mes  y Año de termino
                                                                        //Modificable

                                        txt.Span(", " + servicioPracticas.actividad_practicas).LetterSpacing(0.05f);
                                    });

                                    


                                    //Columna el texto posterior a las fechas y horarios
                                    col1.Item().DefaultTextStyle(textoArial).Padding(10)
                                    .Text("En espera de informes, evaluación y constancia de nuestro participante" +
                                    ", le agradezco de antemano las atenciones que sirva a brindar al portador de" +
                                    "la presente, sin otro particular quedo de usted a sus respetables órdenes.").LetterSpacing(0.04f);



                                    col1.Item().DefaultTextStyle(textoArial).Padding(10)
                                    .Text("ATENTAMENTE").SemiBold().LetterSpacing(0.04f);


                                    //Columna para el espacio de firma
                                    col1.Item().DefaultTextStyle(textoArial).Padding(10).AlignLeft().Text(txt =>
                                    {
                                        txt.Span("\n\n\n").Underline();

                                        //Columna para la firma del Director del Plantel
                                        txt.Span(escuela.NombreDirector).SemiBold();//Editable
                                        txt.Span("\nDIRECTOR").SemiBold();
                                    });

                                });//Fin de la sección del Contenido


                                //Contenido de la piecera PDF
                                page.Footer().Column(col1 =>
                                {
                                    //NUEVA COLUMNA PARA PONER DATOS DE ARCHIVO
                                    col1.Item().AlignLeft().Padding(10).Text(txt =>
                                    {
                                        txt.Span("c.c.p - Archivo").FontFamily(Fonts.Arial).FontSize(6); ;
                                        txt.Span("\n" + iniciales + "/DIVL/CCG/*asa\n").FontFamily(Fonts.Arial).FontSize(6); ;
                                    });

                                    col1.Item().AlignLeft()
                                    .Text("Calle No Reeleción S/N, Col Lázaro Cárdenas," +
                                    "Galeana, Zacatepec, Mor. C.P. 62785").FontColor("#C59452")
                                    .FontFamily(Fonts.Tahoma).FontSize(6);
                                    col1.Item().AlignLeft()
                                        .Text("Tel. 7343434538 Correo electrónico: " +
                                        "cbtis223.escolares223.escolares@dgeti.sems.gob.mx").FontColor("#C59452")
                                        .FontFamily(Fonts.Tahoma).FontSize(6);

                                    col1.Item().Row(row2 =>
                                    {
                                        //Fila para la imagen de la piecera
                                        row2.ConstantItem(530).Height(60).Placeholder();
                                    });
                                });
                            });

                        }).GeneratePdf();

                        Stream stream = new MemoryStream(data);
                        return File(stream, "application/pdf", "Hoja Practica Presentacion " + idEstudiante +".pdf");
                    }
                    catch (Exception ex)
                    {
                        // Maneja la excepción, por ejemplo, registrando un mensaje de error
                        // o redirigiendo a una vista de error
                        _logger.LogError(ex, "No se encontro y buscas un registro inexi");
                        return RedirectToAction("Error", "Home", new { message = ex.Message });
                    }
                }
                else
                {
                    //Obtenemos datos del form
                    string idEstudiante = Request.Form["idEstudiante"];

                    try
                    {
                        //Realizamos busqueda de acuerdo a los datos enviados por el form para la tabla estudiantes
                        var alumno = _context.EstudiantesServicios.FirstOrDefault(x => x.NumeroControl == idEstudiante);
                        var alumnoP = _context.EstudiantesPracticas.FirstOrDefault(x => x.EstudianteNc == idEstudiante);
                        //Realizamos busqueda de acuerdo a los datos enviados por el form para la tabla Servicio social
                        var servicioPracticas = _context.PracticasProfesionales.FirstOrDefault(x => x.EstudianteNc == idEstudiante);

                        //En caso de no encotrar mandamos mensaje
                        if (servicioPracticas == null)
                        {
                            throw new Exception("El servicio social del estudiante no se encuentra en la base de datos.");
                        }

                        //Transformo los datos de fecha a string

                        DateTime FechaInicio = servicioPracticas.FechaInicioPracticas;
                        DateTime FechaTermino = servicioPracticas.FechaTerminoPracticas;


                        // Generamos PDf Hoja de liberacion
                        var data = Document.Create(formato_LIB_PP =>
                        {
                            formato_LIB_PP.Page(page =>
                            {
                                page.Margin(30);

                                var textoArial = TextStyle
                                .Default.FontFamily(Fonts.Arial).FontSize(11.5f);

                                // Encabezado del documento PDF
                                //Fila para el encabezado: Logos de instituciones y datos del plantel
                                page.Header().Row(row =>
                                {
                                    //Fila para el logo de la SEP y DGETi
                                    row.ConstantItem(350).MaxWidth(350).MaxHeight(40).Placeholder();


                                    //Fila con columnas interrnas para los datos del plantel CBTis No. 223
                                    row.RelativeItem().Column(col =>
                                    {
                                        col.Item().AlignRight()
                                        .Text("Subsecretaría de Educación Media Superior")
                                        .FontFamily(Fonts.Tahoma).Bold().FontSize(8);

                                        col.Item().AlignRight()
                                        .Text("Dirección General de Educación Tecnológica Industrial y Servicios")
                                        .FontFamily(Fonts.Tahoma).FontSize(6);

                                        col.Item().AlignRight()
                                        .Text("Centro de Bachillerato Tecnológico industrial y de servicios No. 223")
                                        .FontFamily(Fonts.Tahoma).FontSize(6);

                                        col.Item().AlignRight()
                                        .Text("“MIGUEL HIDALGO Y COSTILLA”")
                                        .FontFamily(Fonts.Tahoma).FontSize(6);
                                    });
                                });

                                //Contenido del documento PDF
                                page.Content().Column(col1 =>
                                {
                                    //Columna para el No. de Constancia
                                    //Texto personalizado para el No. de constancia,plantel,año
                                    col1.Item().DefaultTextStyle(textoArial).Padding(10).AlignRight().Text(txt =>
                                    {
                                        //No. de oficio //Modificable
                                        txt.Span("\nOFICIO No. ");
                                        txt.Span(NoFolio);//Modificable
                                        txt.Span(añoActual);

                                        //Texto personalizado para el Asunto
                                        txt.Span("\n\nAsunto: CONSTANCIA DE LIBERACIÓN").SemiBold();
                                        txt.Span("\nDE PRÁCTICAS PROFESIONALES").SemiBold();//Servicio Social o Prácticas //Modificable
                                    });

                                    //Columna para la Fecha de expedición
                                    //Texto personalizado para la fecha
                                    col1.Item().DefaultTextStyle(textoArial).Padding(10).AlignRight().Text(txt =>
                                    {
                                        // txt.Span("Fecha: ");

                                        txt.Span("Galeana, Zacatepec, Mor; ");      //Lugar //Modificable

                                        txt.Span("a " + diaActual + " de " + mesActual + " del " + añoActual);   //Fecha //Modificable
                                    });

                                    //Columna los datos del Director General y la dirección
                                    col1.Item().DefaultTextStyle(textoArial).Padding(10).AlignLeft().Text(txt =>
                                    {

                                        txt.Span("EDUARDO ANDRADE SÁNCHEZ").SemiBold();//Nombre

                                        txt.Span("\nC. DIRECTOR GENERAL DE PROFESIONES").SemiBold();

                                        txt.Span("\nViaducto Rio de la Piedad No.551,");//

                                        txt.Span("\nCol. Magdalena Mixhuca,");//Colonia

                                        txt.Span("\nAlcaldía Venustiano Carranza");//

                                        txt.Span("\nC.P. 15860, en la Ciudad de México.");//CódigoPostal y Ciudad

                                        txt.Span("\nPRESENTE").SemiBold().LetterSpacing(0.1f);//Inicio de la carta de liberación
                                    });

                                    //Columna para la carta con partes editables para datos del alumno
                                    //Texto personalizado para la carta y datos del alumno
                                    col1.Item().DefaultTextStyle(textoArial).Padding(10).Text(txt =>
                                    {
                                        txt
                                        .Span("El Director del Centro de Estudios Tecnológicos Industrial " +
                                        "y de Servicios No. 223, ubicado en: Calle No Reelección s/n, Col. Lázaro Cárdenas Galeana, " +
                                            "Zacatepec, Morelos; dependiente de la Dirección General de Educación Tecnológica Industrial " +
                                            "en cumplimiento de las disposiciones relativas a la Ley " +
                                            "Reglamentaria del artículo 5° Constitucional y de lo señalado en el reglamento para" +
                                            "la prestación de Prácticas Profesionales de la propia Dirección, hace constar que según " +
                                            "Documentos que operan en el plantel " +
                                            "el (la) alumno (a): ")
                                        .LetterSpacing(0.04f);

                                        txt.Span(alumno.Nombre + " " + alumno.Apellidos) //Nombre del alumno
                                        .Bold(); //Modificable

                                        txt.Span(" No. De control: ")//Texto previo al No. contol
                                        .LetterSpacing(0.05f);

                                        txt.Span(alumno.NumeroControl)//No.Control del alumno
                                        .Bold().LetterSpacing(0.02f); //Modificable

                                        txt.Span(", ha prestado sus Prácticas Profesionales correspondientes a la carrera de:")//Texto después del No.Control
                                        .LetterSpacing(0.05f); //Modificable
                                    });

                                    //Columna para la carrera
                                    //Texto personalizado para la carrera
                                    col1.Item().DefaultTextStyle(textoArial).AlignCenter().Text(txt =>
                                    {
                                        txt.Span("\nTÉCNICO EN ")
                                        .Bold().Underline();

                                        txt.Span(alumno.Especialidad)      //Carrera
                                        .Bold().Underline();//Modificable
                                    });

                                    //Columna las fechas referentes al periodo y el servicio prestado
                                    //Texto personalizado el periodo y la descripción del servicio prestado
                                    col1.Item().DefaultTextStyle(textoArial).Padding(10).Text(txt =>
                                    {
                                        txt.Span("\n\nDurante el periodo comprendido de ")
                                        .LetterSpacing(0.01f); ;
                                        
                                        txt.Span((FechaInicio.ToString("dd MMMM"+" Del "+ "yyyy")));//Día y Mes de inicio
                                                                                                    //Modificable

                                        txt.Span(" al ");

                                        txt.Span((FechaTermino.ToString("dd MMMM"+" Del "+ "yyyy")));//Día, Mes  y Año de termino
                                                                                                     //Modificable

                                    });

                                    //Columna para el espacio de firma
                                    col1.Item().DefaultTextStyle(textoArial).AlignCenter().Text(txt =>
                                    {
                                        txt.Span("\n\n\n\n______________________________")
                                    .Underline();

                                        //Columna para la firma del Director del Plantel
                                        txt.Span("\n" + escuela.NombreDirector).SemiBold();//Editable
                                        txt.Span("\nDIRECTOR").SemiBold();
                                    });

                                });//Fin de la sección del Contenido


                                //Contenido de la piecera PDF
                                page.Footer().Column(col1 =>
                                {
                                    col1.Item().AlignLeft()
                                        .Text("Calle No Reeleción S/N, Col Lázaro Cárdenas," +
                                        "Galeana, Zacatepec, Mor. C.P. 62785").FontColor("#C59452")
                                        .FontFamily(Fonts.Tahoma).FontSize(6);
                                    col1.Item().AlignLeft()
                                        .Text("Tel. 7343434538 Correo electrónico: " +
                                        "cbtis223.escolares223.escolares@dgeti.sems.gob.mx").FontColor("#C59452")
                                        .FontFamily(Fonts.Tahoma).FontSize(6);

                                    col1.Item().Row(row2 =>
                                    {
                                        //Fila para la imagen de la piecera
                                        row2.ConstantItem(530).Height(60).Placeholder();
                                    });
                                });
                            });

                        }).GeneratePdf();

                        Stream stream = new MemoryStream(data);
                        return File(stream, "application/pdf", "Hoja Practica Liberacion " + idEstudiante +".pdf");
                    }
                    catch (Exception ex)
                    {
                        // Maneja la excepción, por ejemplo, registrando un mensaje de error
                        // o redirigiendo a una vista de error
                        _logger.LogError(ex, "No se encontro y buscas un registro inexi");
                        return RedirectToAction("Error", "Home", new { message = ex.Message });
                    }
                }
            }
        }
    }
}
