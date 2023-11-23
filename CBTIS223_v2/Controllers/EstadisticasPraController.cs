using CBTIS223_v2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using System.ComponentModel;
using LicenseContext = OfficeOpenXml.LicenseContext;

namespace CBTIS223_v2.Controllers
{
    public class EstadisticasPraController : Controller
    {
        public IActionResult ExportToExcelEspe(string CicloForm, string Espe)
        {
            // Obtiene los datos que deseas exportar (usando la consulta SQL modificada)
            var data = GetDataForExportEspe(CicloForm, Espe);
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            // Crea un nuevo paquete de Excel
            using (var package = new ExcelPackage())
            {
                // Agrega una hoja de trabajo al paquete
                var worksheet = package.Workbook.Worksheets.Add("Estadisticas");


                // Agrega los encabezados de las columnas
                worksheet.Cells["A1"].Style.Font.Bold = true;
                worksheet.Cells["A1"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                worksheet.Cells["A1"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Cyan);
                worksheet.Cells["A1"].Value = "Ciclo Escolar";
                worksheet.Cells["B1"].Style.Font.Bold = true;
                worksheet.Cells["B1"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                worksheet.Cells["B1"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Cyan);
                worksheet.Cells["B1"].Value = "Especialidad";
                worksheet.Cells["C1"].Style.Font.Bold = true;
                worksheet.Cells["C1"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                worksheet.Cells["C1"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Cyan);
                worksheet.Cells["C1"].Value = "Institución";
                worksheet.Cells["D1"].Style.Font.Bold = true;
                worksheet.Cells["D1"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                worksheet.Cells["D1"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Cyan);
                worksheet.Cells["D1"].Value = "CantidadAlumnos";

                // Aplica un formato de negrita y un fondo de color gris al título

                // titleCell.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                //titleCell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Gray);

                // Llena los datos en el archivo Excel
                var row = 2;
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
            // Obtiene los datos que deseas exportar (usando la consulta SQL modificada)
            var data = GetDataForExportInsti(CicloForm, Insti);
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            // Crea un nuevo paquete de Excel
            using (var package = new ExcelPackage())
            {
                // Agrega una hoja de trabajo al paquete
                var worksheet = package.Workbook.Worksheets.Add("Estadisticas");


                // Agrega los encabezados de las columnas
                worksheet.Cells["A1"].Style.Font.Bold = true;
                worksheet.Cells["A1"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                worksheet.Cells["A1"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Cyan);
                worksheet.Cells["A1"].Value = "Ciclo Escolar";
                worksheet.Cells["B1"].Style.Font.Bold = true;
                worksheet.Cells["B1"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                worksheet.Cells["B1"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Cyan);
                worksheet.Cells["B1"].Value = "Institución";
                worksheet.Cells["C1"].Style.Font.Bold = true;
                worksheet.Cells["C1"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                worksheet.Cells["C1"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Cyan);
                worksheet.Cells["C1"].Value = "CantidadAlumnos";

                // Llena los datos en el archivo Excel
                var row = 2;
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
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ServicioSocialData.xlsx");
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
