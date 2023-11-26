using CBTIS223_v2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using System.ComponentModel;
using System.Drawing.Imaging;
using LicenseContext = OfficeOpenXml.LicenseContext;

namespace CBTIS223_v2.Controllers
{
    public class EstadisticasPraController : Controller
    {
        public Escuela? escuela { get; set; }
        private readonly ILogger<HomeController> _logger;
        private IWebHostEnvironment _host;
        private cbtis223Context _context;
        public Escuela modeloE { get; set; }
        public Institucione modeloI { get; set; }


        public EstadisticasPraController(ILogger<HomeController> logger, IWebHostEnvironment host, cbtis223Context context)
        {
            _logger = logger;
            _host = host;
            _context = context;
        }
        public IActionResult ExportToExcelEspe(string CicloForm, string Espe)
        {
            //Obtener fecha del dia
            string añoActual = DateTime.Now.Year.ToString();
            string diaActual = DateTime.Now.Day.ToString();
            string mesActual = DateTime.Now.ToString("MMMM");

            //Obtener el logo del plantel
            var rutaHeader = Path.Combine(_host.WebRootPath, "imagenes", "EncabezadoExcel.png");
            // Obtiene los datos que deseas exportar (usando la consulta SQL modificada)
            var data = GetDataForExportEspe(CicloForm, Espe);
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            // Crea un nuevo paquete de Excel
            using (var package = new ExcelPackage())
            {
                // Agrega una hoja de trabajo al paquete
                var worksheet = package.Workbook.Worksheets.Add("Estadisticas");
                //Encabezado del libro principal 
                // Insertar la imagen en la hoja de Excel
                var picture = worksheet.Drawings.AddPicture("MiImagen", new FileInfo(rutaHeader));
                picture.SetPosition(0, 0, 0, 0);
                // Ajustar el tamaño de la imagen (ancho x alto)
                picture.SetSize(180, 100);
                worksheet.Cells["A6:D6"].Merge = true;
                worksheet.Cells["A6"].Style.Font.Bold = true;
                worksheet.Cells["A6"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                worksheet.Cells["A6"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White);
                worksheet.Cells["A6"].Value = "Centro de Bachillerato Tecnológico Industrial y de Servicios No. 223";
                // worksheet.Cells["E6:F6"].Merge = true;
                worksheet.Cells["E6"].Style.Font.Bold = true;
                worksheet.Cells["E6"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                worksheet.Cells["E6"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White);
                worksheet.Cells["E6"].Value = "Expedido el día: " + diaActual + " de " + mesActual + " del " + añoActual;
                // Agrega los encabezados de las columnas
                worksheet.Cells["A8"].Style.Font.Bold = true;
                worksheet.Cells["A8"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                worksheet.Cells["A8"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.PapayaWhip);
                worksheet.Cells["A8"].Value = "Ciclo Escolar";
                worksheet.Cells["B8"].Style.Font.Bold = true;
                worksheet.Cells["B8"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                worksheet.Cells["B8"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.PapayaWhip);
                worksheet.Cells["B8"].Value = "Especialidad";
                worksheet.Cells["C8"].Style.Font.Bold = true;
                worksheet.Cells["C8"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                worksheet.Cells["C8"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.PapayaWhip);
                worksheet.Cells["C8"].Value = "Institución";
                worksheet.Cells["D8"].Style.Font.Bold = true;
                worksheet.Cells["D8"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                worksheet.Cells["D8"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.PapayaWhip);
                worksheet.Cells["D8"].Value = "No. Alumnos";

                // Aplica un formato de negrita y un fondo de color gris al título

                // titleCell.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                //titleCell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Gray);

                // Llena los datos en el archivo Excel
                var row = 9;
                foreach (var item in data)
                {
                    worksheet.Cells[$"A{row}"].Value = item.Ciclo;
                    worksheet.Cells[$"B{row}"].Value = item.Especialidad;
                    worksheet.Cells[$"C{row}"].Value = item.InstitucionServicio;
                    worksheet.Cells[$"D{row}"].Value = item.CantidadAlumnos;

                    row++;
                }

                // Guarda el paquete en un MemoryStream
                using (var stream = new MemoryStream())
                {
                    package.SaveAs(stream);
                    stream.Position = 0;

                    // Devuelve el archivo Excel como una descarga
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "PracticasProfesionalesData.xlsx");
                }
            }
        }


        public List<ExportData> GetDataForExportEspe(string CicloForm, string Espe)
        {
            using (var dbContext = new cbtis223Context())
            {
                var data = dbContext.PracticasProfesionales
                    .Where(ss => ss.EstudianteNcNavigation.EstudianteNcNavigation.Ciclo == CicloForm && ss.EstudianteNcNavigation.EstudianteNcNavigation.Especialidad == Espe)
                    .GroupBy(ss => new {
                        ss.IdInstiPracticasNavigation.Institucion,
                        ss.EstudianteNcNavigation.EstudianteNcNavigation.Ciclo,
                        ss.EstudianteNcNavigation.EstudianteNcNavigation.Especialidad
                    })
                    .Select(group => new ExportData
                    {
                        InstitucionServicio = group.Key.Institucion,
                        Ciclo = group.Key.Ciclo,
                        Especialidad = group.Key.Especialidad,
                        CantidadAlumnos = group.Count()
                    })
                    .ToList();

                return data;
            }
        }


        public IActionResult ExportToExcelInsti(string CicloForm, int Insti)
        {

            //Obtener fecha del dia
            string añoActual = DateTime.Now.Year.ToString();
            string diaActual = DateTime.Now.Day.ToString();
            string mesActual = DateTime.Now.ToString("MMMM");

            //Obtener el logo del plantel
            var rutaHeader = Path.Combine(_host.WebRootPath, "imagenes", "EncabezadoExcel.png");
            // Obtiene los datos que deseas exportar (usando la consulta SQL modificada)
            var data = GetDataForExportInsti(CicloForm, Insti);
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            // Crea un nuevo paquete de Excel
            using (var package = new ExcelPackage())
            {
                // Agrega una hoja de trabajo al paquete
                var worksheet = package.Workbook.Worksheets.Add("Estadisticas");
                //Encabezado del libro principal 
                // Insertar la imagen en la hoja de Excel
                var picture = worksheet.Drawings.AddPicture("MiImagen", new FileInfo(rutaHeader));
                picture.SetPosition(0, 0, 0, 0);
                // Ajustar el tamaño de la imagen (ancho x alto)
                picture.SetSize(180, 100);
                worksheet.Cells["A6:D6"].Merge = true;
                worksheet.Cells["A6"].Style.Font.Bold = true;
                worksheet.Cells["A6"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                worksheet.Cells["A6"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White);
                worksheet.Cells["A6"].Value = "Centro de Bachillerato Tecnológico Industrial y de Servicios No. 223";
                // worksheet.Cells["E6:F6"].Merge = true;
                worksheet.Cells["E6"].Style.Font.Bold = true;
                worksheet.Cells["E6"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                worksheet.Cells["E6"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White);
                worksheet.Cells["E6"].Value = "Expedido el día: " + diaActual + " de " + mesActual + " del " + añoActual;
                // Agrega los encabezados de las columnas
                worksheet.Cells["A8"].Style.Font.Bold = true;
                worksheet.Cells["A8"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                worksheet.Cells["A8"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.PapayaWhip);
                worksheet.Cells["A8"].Value = "Ciclo Escolar";
                worksheet.Cells["B8"].Style.Font.Bold = true;
                worksheet.Cells["B8"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                worksheet.Cells["B8"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.PapayaWhip);
                worksheet.Cells["B8"].Value = "Especialidad";
                worksheet.Cells["C8"].Style.Font.Bold = true;
                worksheet.Cells["C8"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                worksheet.Cells["C8"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.PapayaWhip);
                worksheet.Cells["C8"].Value = "Institución";
                worksheet.Cells["D8"].Style.Font.Bold = true;
                worksheet.Cells["D8"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                worksheet.Cells["D8"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.PapayaWhip);
                worksheet.Cells["D8"].Value = "No. Alumnos";

                // Llena los datos en el archivo Excel
                var row = 9;
                foreach (var item in data)
                {
                    var A1 = worksheet.Cells[$"A{row}"].Value = item.Ciclo;
                    worksheet.Cells[$"B{row}"].Value = item.InstitucionServicio;
                    worksheet.Cells[$"C{row}"].Value = item.CantidadAlumnos;
                    row++;
                }


                // Guarda el paquete en un MemoryStream
                using (var stream = new MemoryStream())
                {
                    package.SaveAs(stream);
                    stream.Position = 0;

                    // Devuelve el archivo Excel como una descarga
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "PracticasProfesionalesData.xlsx");
                }
            }
        }


        public List<ExportData> GetDataForExportInsti(string CicloForm, int Insti)
        {
            using (var dbContext = new cbtis223Context())
            {
                var data = dbContext.PracticasProfesionales
                    .Where(ss => ss.EstudianteNcNavigation.EstudianteNcNavigation.Ciclo == CicloForm && ss.IdInstiPracticas == Insti)
                    .GroupBy(ss => new {
                        ss.IdInstiPracticasNavigation.Institucion,
                        ss.EstudianteNcNavigation.EstudianteNcNavigation.Ciclo,
                    })
                    .Select(group => new ExportData
                    {
                        InstitucionServicio = group.Key.Institucion,
                        Ciclo = group.Key.Ciclo,
                        CantidadAlumnos = group.Count()
                    })
                    .ToList();

                return data;
            }
        }

    }
}
