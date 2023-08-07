using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RandevuSistemi.Models;
using RandevuSistemi.Models.Dto;
using RandevuSistemi.Models.Entities;

namespace HayvanBarinagi.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        public bool AdminControl()
        {
            if (User.Identity.IsAuthenticated) 
            {
                
                string userName = User.Identity.Name;

                if (userName == "admin") // Kullanıcı adı "admin" mi?
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        Context context = new Context();

        public IActionResult Index()
        {
            if (AdminControl())
            {
                var anaBilimDallari = context.AnaBilimDallari.ToList();
                return View(anaBilimDallari);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        public IActionResult AnaBilimDaliEkle()
        {
            if (AdminControl())
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public IActionResult AnaBilimDaliEkle(AnaBilimDali anaBilimDali)
        {
            var newAnaBilimDali = new AnaBilimDali
            {
                Name = anaBilimDali.Name
            };

            context.AnaBilimDallari.Add(newAnaBilimDali);
            context.SaveChanges();

            return RedirectToAction("Index", "Admin");
        }

        [HttpGet]
        public IActionResult AnaBilimDaliDuzenle(int Id)
        {
            if (AdminControl())
            {
                var anaBilimDali = context.AnaBilimDallari.Find(Id);
                return View(anaBilimDali);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public IActionResult AnaBilimDaliDuzenle(AnaBilimDali anaBilimDali)
        {
            var selectedAnimal = context.AnaBilimDallari.Find(anaBilimDali.Id);

            selectedAnimal.Name = anaBilimDali.Name;
            context.SaveChanges();

            return RedirectToAction("Index", "Admin");
        }

        public IActionResult AnaBilimDaliSil(int Id)
        {
            var anaBilimDali = context.AnaBilimDallari.Find(Id);
            context.AnaBilimDallari.Remove(anaBilimDali);
            context.SaveChanges();

            return RedirectToAction("Index", "Admin");
        }

        public IActionResult Poliklinik()
        {
            if (AdminControl())
            {
                var poliklinikler = context.Poliklinikler.ToList();
                var tumAnabilimDallari = context.AnaBilimDallari.ToList();

                var poliklinikModelList = new List<PoliklinikveAnabilimDaliAdi>();

                foreach (var poliklinik in poliklinikler)
                {
                    var anaBilimDali = tumAnabilimDallari.FirstOrDefault(abd => abd.Id == poliklinik.AnaBilimDaliId);

                    var poliklinikModel = new PoliklinikveAnabilimDaliAdi
                    {
                        poliklinik = poliklinik,
                        AnaBilimDaliAdi = anaBilimDali?.Name
                    };

                    poliklinikModelList.Add(poliklinikModel);
                }

                return View(poliklinikModelList);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        public IActionResult PoliklinikEkle()
        {
            if (AdminControl())
            {
                var anaBilimDaliListesi = context.AnaBilimDallari.Select(h => new SelectListItem
                {
                    Value = h.Id.ToString(),
                    Text = h.Name
                }).ToList();

                return View(anaBilimDaliListesi);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public IActionResult PoliklinikEkle(IFormCollection form)
        {
            // Form verilerini almak için FormCollection nesnesini kullanımı

            string name = form["Name"];
            int selectedAnaBilimDaliId = Convert.ToInt32(form["id"]);

            using (var dbContext = new Context())
            {
                Poliklinik yeniPoliklinik = new Poliklinik
                {
                    Name = name,
                    AnaBilimDaliId = selectedAnaBilimDaliId
                };

                dbContext.Poliklinikler.Add(yeniPoliklinik);
                dbContext.SaveChanges();
            }
            return RedirectToAction("Poliklinik", "Admin");
        }

        public IActionResult PoliklinikSil(int Id)
        {
            var poliklinik = context.Poliklinikler.Find(Id);
            context.Poliklinikler.Remove(poliklinik);
            context.SaveChanges();

            return RedirectToAction("Poliklinik", "Admin");
        }

        public IActionResult Doktor()
        {
            if (AdminControl())
            {
                var tumDoktorlar = context.Doktorlar.ToList();
                var tumPoliklinikler = context.Poliklinikler.ToList();

                var poliklinikModelList = new List<DoktorVePoliklinikAdi>();

                foreach (var doktor in tumDoktorlar)
                {
                    var poliklinikler = tumPoliklinikler.FirstOrDefault(abd => abd.Id == doktor.PoliklinikId);

                    var doktorVePoliklinik = new DoktorVePoliklinikAdi
                    {
                        doctor = doktor,
                        PoliklinikAdi = poliklinikler?.Name
                    };

                    poliklinikModelList.Add(doktorVePoliklinik);
                }

                return View(poliklinikModelList);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        public IActionResult DoktorEkle()
        {
            if (AdminControl())
            {
                var poliklinikListesi = context.Poliklinikler.Select(h => new SelectListItem
                {
                    Value = h.Id.ToString(),
                    Text = h.Name
                }).ToList();

                return View(poliklinikListesi);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public IActionResult DoktorEkle(IFormCollection form)
        {
            // Form verilerini almak için FormCollection nesnesini kullanımı

            string name = form["Name"];
            int selectedpoliklinikId = Convert.ToInt32(form["id"]);

            using (var dbContext = new Context())
            {
                Doktor yeniDoktor = new Doktor
                {
                    AdSoyad = name,
                    PoliklinikId = selectedpoliklinikId
                };

                dbContext.Doktorlar.Add(yeniDoktor);
                dbContext.SaveChanges();
            }
            return RedirectToAction("Doktor", "Admin");
        }

        public IActionResult DoktorSil(int Id)
        {
            var doktor = context.Doktorlar.Find(Id);
            context.Doktorlar.Remove(doktor);
            context.SaveChanges();

            return RedirectToAction("Doktor", "Admin");
        }

        public IActionResult Randevu()
        {
            if (AdminControl())
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

    }
}
