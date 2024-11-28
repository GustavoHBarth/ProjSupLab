using Microsoft.AspNetCore.Mvc;
using System.IO.Compression;

namespace ProjSupLab.Controllers
{
    public class PastasController : Controller
    {
        public IActionResult JuntarArquivos()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> JuntarArquivos(List<IFormFile> files)
        {
            if (files == null || files.Count == 0)
            {
                TempData["Error"] = "Por favor, selecione ao menos um arquivo.";
                return RedirectToAction("JuntarArquivos");
            }

            try
            {
                // Cria o stream de memória para o ZIP
                using (var memoryStream = new MemoryStream())
                {
                    using (var zipArchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                    {
                        foreach (var file in files)
                        {
                            // Lê o conteúdo do arquivo enviado em um buffer
                            using (var fileStream = file.OpenReadStream())
                            {
                                var fileBytes = new byte[file.Length];
                                await fileStream.ReadAsync(fileBytes, 0, (int)file.Length);

                                // Cria uma entrada no ZIP
                                var zipEntry = zipArchive.CreateEntry(file.FileName, CompressionLevel.Optimal);

                                using (var entryStream = zipEntry.Open())
                                {
                                    // Escreve o conteúdo do buffer na entrada do ZIP
                                    await entryStream.WriteAsync(fileBytes, 0, fileBytes.Length);
                                }
                            }
                        }
                    }

                    // Volta ao início do stream do ZIP antes de retornar
                    memoryStream.Seek(0, SeekOrigin.Begin);

                    // Retorna o ZIP como arquivo para download
                    return File(memoryStream, "application/zip", "ArquivosJuntos.zip");
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Erro ao processar arquivos: " + ex.Message;
                return RedirectToAction("JuntarArquivos");
            }
        }

    }

}
