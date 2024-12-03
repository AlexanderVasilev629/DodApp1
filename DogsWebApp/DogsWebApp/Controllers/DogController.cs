using DogsApp.Infrastructure.Data;
using DogsApp.Infrastructure.Data.Entities;
using DogsWebApp.Models.Dog;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DogsWebApp.Controllers
{
    public class DogController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DogController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: DogController
        public ActionResult Index(string searchStringBreed, string searchStringName)
        {
            List<DogAllViewModel> dogs = _context.Dogs.Select(dogFromDb => new DogAllViewModel
            {

                Id = dogFromDb.Id,
                Name = dogFromDb.Name,
                Age = dogFromDb.Age,
                Breed = dogFromDb.Breed,
                Picture = dogFromDb.Picture,

            }).ToList();

            if (!String.IsNullOrEmpty(searchStringBreed) && !String.IsNullOrEmpty(searchStringName))
            {
                dogs = dogs.Where(x => x.Breed.ToLower().Contains(searchStringBreed.ToLower()) && x.Name.ToLower().Contains(searchStringName.ToLower())).ToList();
            }
            else if (!String.IsNullOrEmpty(searchStringBreed))
            {
                dogs = dogs.Where(x => x.Breed.ToLower().Contains(searchStringBreed.ToLower())).ToList();
            }
            else if (!String.IsNullOrEmpty(searchStringName))
            {
                dogs = dogs.Where(x => x.Name.ToLower().Contains(searchStringName.ToLower())).ToList();
            }

            return View(dogs);
        }

        // GET: DogController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DogController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DogCreateViewModel blindingModel)
        {
            if (ModelState.IsValid)
            {
                Dog dogFromDb = new Dog
                {
                    Name = blindingModel.Name,
                    Age = blindingModel.Age,
                    Breed = blindingModel.Breed,
                    Picture = blindingModel.Picture,
                };

                _context.Dogs.Add(dogFromDb);
                _context.SaveChanges();

                return this.RedirectToAction("Success");
            }
            return this.View();
        }

        public IActionResult Success()
        {
            return this.View();
        }

        // GET: DogController/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Dog? item = _context.Dogs.Find(id);

            if (item == null)
            {
                return NotFound();
            }

            DogEditViewModel dog = new DogEditViewModel()
            {
                Id = item.Id,
                Name = item.Name,
                Age = item.Age,
                Breed = item.Breed,
                Picture = item.Picture
            };

            return View(dog);
        }

        // POST: DogController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, DogEditViewModel blindingModel)
        {
            if (ModelState.IsValid)
            {
                Dog dog = new Dog
                {
                    Id = id,
                    Name = blindingModel.Name,
                    Age = blindingModel.Age,
                    Breed= blindingModel.Breed,
                    Picture = blindingModel.Picture
                };
                _context.Dogs.Update(dog);
                _context.SaveChanges();
                return this.RedirectToAction("Index");
            }
            return View(blindingModel);
        }

        // GET: DogController/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Dog? item = _context.Dogs.Find(id);
            if (item == null)
            {
                return NotFound();
            }

            DogDetailsViewModel dog = new DogDetailsViewModel()
            {
                Id = item.Id,
                Name = item.Name,
                Age = item.Age,
                Breed = item.Breed,
                Picture = item.Picture
            };
            return View(dog);
        }

        // GET: DogController/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Dog? item = _context.Dogs.Find(id);
            if (item == null)
            {
                return NotFound();
            }

            DogDeleteViewModel dog = new DogDeleteViewModel()
            {
                Id = item.Id,
                Name = item.Name,
                Age = item.Age,
                Breed = item.Breed,
                Picture = item.Picture
            };
            return View(dog);
        }

        // POST: DogController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            Dog? item = _context.Dogs.Find(id);

            if (item == null)
            {
                return NotFound();
            }

            _context.Dogs.Remove(item);
            _context.SaveChanges();
            return this.RedirectToAction("Index", "Dog");
        }
    }
}