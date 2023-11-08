using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using CBTIS223_v2.Models;

using QuestPDF.Fluent;
using QuestPDF.Helpers;

namespace CBTIS223_v2.Controllers
{
    public class ControllerPDF
    {

        // ULTIMO COMENTARIO
        public class HomeController : Controller
        {
            private readonly IWebHostEnvironment _host;

            public HomeController(IWebHostEnvironment host)
            {
                _host = host;
            }

            public IActionResult Index()
            {
                return View();
            }

            public IActionResult Privacy()
            {
                return View();
            }

            public IActionResult DescargarLibSS()
            {

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
                        page.Content().Column(col1 =>
                        {
                            //Texto personalizado para el No. de constancia,plantel,año
                            col1.Item().AlignRight().Text(txt =>
                            {
                                txt.Span("\n CONSTANCIA NO.: ").SemiBold()
                                .FontFamily("Arial").FontSize(12);

                                txt.Span("65").SemiBold()           //No. de constancia
                                .FontFamily("Arial").FontSize(12); //Modificable

                                txt.Span(" (CBTIS No.223)").SemiBold() //Plantel
                                .FontFamily("Arial").FontSize(12); //Modificable

                                txt.Span(" 2023").SemiBold()        //Año
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

                                txt.Span("a 30 de mayo del 2023")   //Fecha
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

                                txt.Span("ALVARADO MACIAS ISAAC ALDAIR") //Nombre del alumno
                                .Bold().FontFamily("Arial").FontSize(12); //Modificable

                                txt.Span(" No. De control: ")//Texto previo al No. contol
                                .FontFamily("Arial").FontSize(12).LetterSpacing(0.05f);

                                txt.Span("20317052230270")//No.Control del alumno
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

                                txt.Span("PREPARACIÓN DE ALIMENTOS Y BEBIDAS ")      //Carrera
                                .Bold().Underline().FontFamily("Arial").FontSize(12);//Modificable
                            });

                            //Columna las fechas referentes al periodo y el servicio prestado
                            //Texto personalizado el periodo y la descripción del servicio prestado
                            col1.Item().Text(txt =>
                            {
                                txt.Span("\nDurante el periodo comprendido del ")
                                .FontFamily("Arial").FontSize(12).LetterSpacing(0.02f); ;

                                txt.Span("28 de agosto del 2022 ")      //Periodo Inicio
                                .FontFamily("Arial").FontSize(12).LetterSpacing(0.02f); ;//Modificable

                                txt.Span("al 28 de febrero del 2023")      //Periodo Fin
                                .FontFamily("Arial").FontSize(12).LetterSpacing(0.02f); ;//Modificable

                                txt.Span("Consistiendo en: ")      //Texto
                                .FontFamily("Arial").FontSize(12).LetterSpacing(0.02f); ;

                                txt.Span("Apoyo Administrativo en TALLER DE PROMOCIÓN DEPORTIVA DEL" +
                                    "CBTIS No. 223 DE GALEANA ZACATEPEC, MOR.")      //Descripción Servicio Prestado
                                .FontFamily("Arial").FontSize(12).LetterSpacing(0.02f); ;//Modificable
                            });

                            //Columna para el espacio de firma
                            col1.Item().AlignCenter().
                            Text("\n" +
                            "________________________________________________________")
                            .Underline();
                            //Columna para la firma del Director del Plantel
                            col1.Item().AlignCenter().Text("CESAR REBOLLEDO VALLE")//Editable
                            .FontFamily("Arial").FontSize(12); ;
                            col1.Item().AlignCenter().Text("DIRECTOR DEL PLANTEL")
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
                return File(stream, "application/pdf", "libSS.pdf");
            }
        }
    }
}
