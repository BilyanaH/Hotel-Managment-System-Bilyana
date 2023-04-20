using Hotel_Bilyana.Models;
using Hotel_Bilyana.Models.Domain;
using Hotel_Bilyana.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using System.Reflection.Emit;
using System.Security.Claims;
using static System.Net.Mime.MediaTypeNames;
using System.Text;

namespace Hotel_Bilyana.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly DatabaseContext _context;

        public DashboardController(DatabaseContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Users()
        {
            if (User.IsInRole("admin"))
            {
                var users = _context.ApplicationUsers.ToList();
                var userRoles = _context.UserRoles.ToList();
                var roles = _context.Roles.ToList();

                var userRolesDict = userRoles.ToDictionary(ur => ur.UserId, ur => roles.FirstOrDefault(r => r.Id == ur.RoleId)?.Name);

                ViewBag.UserRoles = userRolesDict;


                //    var users = _context.ApplicationUsers.ToList();
                return View(users);
            }
            else
            {
                return View("NotAuthorized");
            }
        }
        public IActionResult User_Create()
        {
            // ViewBag.ReservationId = new SelectList(_context.ReservationModel, "Id", "Id");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> User_Create([Bind("FirstName,MiddleName,LastName,EGN,PhoneNumber,IsActive,UserName,Email")] ApplicationUser user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(User));
            }
            //   ViewBag.ReservationId = new SelectList(_context.ReservationModel, "Id", "Id", client.ReservationId);
            return View(user);
        }

        public IActionResult User_Edit(string id)
        {
            if (User.IsInRole("admin"))
            {
                var user = _context.ApplicationUsers.FirstOrDefault(u => u.Id == id);

                if (user == null)
                {
                    return NotFound();
                }

                var roleId = _context.UserRoles.FirstOrDefault(ur => ur.UserId == id)?.RoleId;
                var roleName = _context.Roles.FirstOrDefault(r => r.Id == roleId)?.Name;

                user.ProfilePicture = roleName;
                ViewBag.UserRole = roleName;

                return View(user);
            }
            else
            {
                return View("NotAuthorized");
            }
        }
        [HttpPost]
        public async Task<IActionResult> User_Edit([Bind("Id,SecurityStamp,ConcurrencyStamp,FirstName,MiddleName,LastName,EGN,PhoneNumber,IsActive,UserName,NormalizedUserName,Email,NormalizedEmail,ProfilePicture,EmalCobfirmed,PasswordHash,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEnd,LockoutEnabled,AcessFailedCount")] ApplicationUser user)
        {
            user.NormalizedUserName= user.UserName.Normalize(NormalizationForm.FormD).ToUpperInvariant().Replace("İ", "I");
            user.NormalizedEmail = user.Email.Normalize(NormalizationForm.FormD).ToUpperInvariant().Replace("İ", "I");

            if (ModelState.IsValid && User.IsInRole("admin"))
            {

                try
                {
                    var newRoleName = user.ProfilePicture;
                    user.ProfilePicture = null;

                    _context.Entry(user).State = EntityState.Modified;
                    await _context.SaveChangesAsync();


                    //
                    var roleId = _context.UserRoles.FirstOrDefault(ur => ur.UserId == user.Id)?.RoleId;

                    var newRoleId = _context.Roles.FirstOrDefault(r => r.Name == newRoleName)?.Id;
                    if (newRoleId != roleId && newRoleId!=null)
                    {
                     var userRole = _context.UserRoles.FirstOrDefault(ur => ur.UserId == user.Id);
                     if (userRole != null)
                     {
                         _context.UserRoles.Remove(userRole);
                        _context.SaveChanges();
                     }

                     var newUserRole = new IdentityUserRole<string>
                     {
                        UserId = user.Id,
                        RoleId = newRoleId
                     };
                     _context.UserRoles.Add(newUserRole);
                     _context.SaveChanges();
                    }


                    //  if (userRole != null)
                    // {
                    //      userRole.RoleId = newRoleId;
                    //     _context.SaveChanges();
                    //  }
                    //
                    return RedirectToAction(nameof(Users));
                }
                catch (DbUpdateConcurrencyException)
                {
                    // Log the exception
                    // Reload the entity from the database
                    var entry = _context.Entry(user);
                    entry.Reload();
                    // Update the entity with the changes
                    entry.CurrentValues.SetValues(user);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Users));
                }
            }
            return View(user);
        }
        public IActionResult User_Details(string id)
        {
            if (User.IsInRole("admin"))
            {
                if (id == null)
                {
                 return NotFound();
                }

                var user = _context.ApplicationUsers
                  .FirstOrDefault(m => m.Id == id);
                if (user == null)
                {
                 return NotFound();
                }

                var roleId = _context.UserRoles.FirstOrDefault(ur => ur.UserId == id)?.RoleId;
                var roleName = _context.Roles.FirstOrDefault(r => r.Id == roleId)?.Name;

                //user.ProfilePicture = roleName;
                ViewBag.UserRole = roleName;
                
                return View(user);
            }
            else
            {
                return View("NotAuthorized");
            }
        }
        public async Task<IActionResult> User_Delete(string? id)
        {
            if (User.IsInRole("admin"))
            {
                if (id == null)
            {
                return NotFound();
            }

            var user = await _context.ApplicationUsers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }
            else
            {
                return View("NotAuthorized");
    }
}

        // POST: User/Delete/5
        [HttpPost, ActionName("User_Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> User_DeleteConfirmed(string id)
        {
            if (User.IsInRole("admin"))
            {
                var user = await _context.ApplicationUsers
                .FirstOrDefaultAsync(m => m.Id == id);
                _context.ApplicationUsers.Remove(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Users));
            }
            else
            {
                return View("NotAuthorized");
            }
        }
        //END - User
        public IActionResult Client()
        {
            var clients = _context.ClientModel.ToList();
            return View(clients);
        }
        public IActionResult Client_create()
        {
           // ViewBag.ReservationId = new SelectList(_context.ReservationModel, "Id", "Id");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Client_Create([Bind("FirstName,Surname,PhoneNumber,Email,IsAdult")] ClientModel client)
        {
            if (ModelState.IsValid)
            {
                _context.Add(client);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Client));
            }
         //   ViewBag.ReservationId = new SelectList(_context.ReservationModel, "Id", "Id", client.ReservationId);
            return View(client);
        }
        [HttpGet]
        public IActionResult Client_Edit(int id)
        {
            var client = _context.ClientModel.Find(id);
            if (client == null)
            {
                return NotFound();
            }
            return View(client);
        }
        [HttpPost]
        public async Task<IActionResult> Client_Edit(ClientModel client)
        {
            if (ModelState.IsValid)
            {
                _context.Update(client);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Client));
            }
            return View(client);
        }
        public async Task<IActionResult> Client_Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clientModel = await _context.ClientModel
                .FirstOrDefaultAsync(m => m.ClientId == id);
            if (clientModel == null)
            {
                return NotFound();
            }

            return View(clientModel);
        }

        // POST: Client/Delete/5
        [HttpPost, ActionName("Client_Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Client_DeleteConfirmed(int id)
        {
            var clientModel = await _context.ClientModel.FindAsync(id);
            _context.ClientModel.Remove(clientModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Client));
        }

        private bool ClientModelExists(int id)
        {
            return _context.ClientModel.Any(e => e.ClientId == id);
        }
        public IActionResult Client_Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = _context.ClientModel
                .FirstOrDefault(m => m.ClientId == id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }
        public async Task<IActionResult> Room()
        {
            var rooms = await _context.RoomModel.ToListAsync();
            return View(rooms);
        }

        public IActionResult Room_Create()
        {
            if (User.IsInRole("admin"))
            {
                ViewBag.RoomTypes = Enum.GetValues(typeof(RoomModel.RoomType)).Cast<RoomModel.RoomType>();
                // ViewBag.RoomTypes = Enum.GetValues(typeof(RoomModel.RoomType));

                return View();
            }
            else
            {
                return View("NotAuthorized");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Room_Create([Bind("Number,Capacity,Price,Description,Type,IsFree")] RoomModel room)
        {
            if (User.IsInRole("admin"))
            {
                room.Type = (RoomModel.RoomType)Enum.Parse(typeof(RoomModel.RoomType), Request.Form["Type"]);

            if (ModelState.IsValid)
            {
                _context.Add(room);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Room));
            }
            return View(room);
        }
             else
            {
                return View("NotAuthorized");
            }
        }

        public async Task<IActionResult> Room_Edit(int? id)
        {
            if (User.IsInRole("admin"))
            {
                if (id == null)
                {
                    return NotFound();
                }

                var room = await _context.RoomModel.FindAsync(id);
                if (room == null)
                {
                    return NotFound();
                }
                ViewBag.RoomTypes = Enum.GetValues(typeof(RoomModel.RoomType)).Cast<RoomModel.RoomType>();

                return View(room);
            }
            else
            {
                return View("NotAuthorized");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Room_Edit(int id, [Bind("RoomId,Number,Capacity,Price,Description,Type,IsFree")] RoomModel room)
        {
            if (User.IsInRole("admin"))
            {
                if (id != room.RoomId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(room);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoomExists(room.RoomId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Room));
            }
            return View(room);
        }
             else
            {
                return View("NotAuthorized");
            }
        }

        public async Task<IActionResult> Room_Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = await _context.RoomModel.FirstOrDefaultAsync(m => m.RoomId == id);
            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }

        public async Task<IActionResult> Room_Delete(int? id)
        {
            if (User.IsInRole("admin"))
            {
                if (id == null)
                {
                    return NotFound();
                }

                var room = await _context.RoomModel.FirstOrDefaultAsync(m => m.RoomId == id);
                if (room == null)
                {
                    return NotFound();
                }

                return View(room);
            }
            else
            {
                return View("NotAuthorized");
            }
        }

        [HttpPost, ActionName("Room_Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (User.IsInRole("admin"))
            {
                var room = await _context.RoomModel.FindAsync(id);
            _context.RoomModel.Remove(room);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Room));
        }
        else
            {
                return View("NotAuthorized");
    }
}

        private bool RoomExists(int id)
        {
            return _context.RoomModel.Any(e => e.RoomId == id);
        }
        public async Task<IActionResult> Reservation()
        {
         var reservations = await _context.ReservationModel
                .Include(r => r.Client)
                .Include(r => r.Room)
                .ToListAsync();

            return View(reservations);
        }

        // GET: Reservation/Details/5
        public async Task<IActionResult> Reservation_Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.ReservationModel
                .Include(r => r.Client)
                .Include(r => r.Room)
                .FirstOrDefaultAsync(m => m.ReservationId == id);

            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // GET: Reservation/Create
        public IActionResult Reservation_Create()
        {
            ViewData["RoomId"] = new SelectList(_context.RoomModel, "RoomId", "Number");
            ViewData["ClientId"] = new SelectList(_context.ClientModel, "ClientId", "Surname");

            return View();
        }

        // POST: Reservation/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reservation_Create([Bind("ClientId","RoomId","DateOfArrival,DateOfLeaving,IsIncludedBreakfast,AllInclusive,FinalPrice")] ReservationModel reservation)
        {
            // var options = new DbContextOptionsBuilder<DatabaseContext>()
            //                 .UseSqlServer(Configuration.GetConnectionString("conn"))
            //               .Options;
            //  var dbContext = new DatabaseContext(options);           
            // var reservation = dbContext.Reservations.FirstOrDefault(r => r.ReservationId == 1);

            reservation.FinalPrice = reservation.CalculateFinalPrice(_context);
            
            //if (ModelState.IsValid)
            //{
                _context.Add(reservation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Reservation));
            //}

            ViewData["RoomId"] = new SelectList(_context.RoomModel, "RoomId", "Number", reservation.RoomId);
            ViewData["ClientId"] = new SelectList(_context.ClientModel, "ClientId", "Surname", reservation.ClientId);
            return View(reservation);
        }

        // GET: Reservation/Edit/5
        public async Task<IActionResult> Reservation_Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.ReservationModel.FindAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }
            ViewData["RoomId"] = new SelectList(_context.RoomModel, "RoomId", "Number", reservation.RoomId);
            ViewData["ClientId"] = new SelectList(_context.ClientModel, "ClientId", "Surname", reservation.ClientId);
            return View(reservation);
        }

        // POST: Reservation/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reservation_Edit(int id, [Bind("ReservationId,ClientId,RoomId,DateOfArrival,DateOfLeaving,IsIncludedBreakfast,AllInclusive,FinalPrice")] ReservationModel reservation)
        {
            if (id != reservation.ReservationId)
            {
                return NotFound();
            }
            else
            { 
            reservation.FinalPrice = reservation.CalculateFinalPrice(_context);
            //if (ModelState.IsValid)
            //{
                try
                {
                    _context.Update(reservation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservationExists(reservation.ReservationId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Reservation));
            }
            ViewData["RoomId"] = new SelectList(_context.RoomModel, "RoomId", "Number", reservation.RoomId);
            ViewData["ClientId"] = new SelectList(_context.ClientModel, "ClientId", "Surname", reservation.ClientId);

            return View(reservation);
        }
        private bool ReservationExists(int id)
        {
            return _context.ReservationModel.Any(e => e.ReservationId == id);
        }
        public async Task<IActionResult> Reservation_Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.ReservationModel
                .Include(r => r.Client)
                .Include(r => r.Room)
                .FirstOrDefaultAsync(m => m.ReservationId == id);
            if (reservation == null)
            {
                return NotFound();
            }

            //   _context.ReservationModel.Remove(reservation);
            //  await _context.SaveChangesAsync();
            //  return RedirectToAction(nameof(Reservation));
            return View(reservation);
        }

        [HttpPost, ActionName("Reservation_Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reservation_DeleteConfirmed(int id)
        {
            var reservation = await _context.ReservationModel.FindAsync(id);
            _context.ReservationModel.Remove(reservation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Reservation));
        }
    }
}
