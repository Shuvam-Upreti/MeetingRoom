using MeetingRoom.Data;
using MeetingRoom.Models;
using MeetingRoom.Models.AuthModel;
using MeetingRoom.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;

namespace MeetingRoom.Controllers
{
    public class BookingController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookingController(ApplicationDbContext context)
        {
            _context = context;

        }

        public async Task<IActionResult> Index()
        {
            IEnumerable <Booking> rooms = _context.Bookings.ToList();
            return View(rooms);
        }
        public async Task<IActionResult> Create()
        {
            IEnumerable<SelectListItem> roomList = _context.RoomModels.Select(
                u => new SelectListItem
                {
                    Text = u.RoomName,
                    Value = u.RoomId.ToString()
                }
            );
            ViewBag.roomList = roomList;

            IEnumerable<SelectListItem> userList = _context.Users.Select(
                 u => new SelectListItem
                 {
                     Text = u.UserName,
                     Value = u.Id.ToString()
                 }
             );

            var users = _context.UserModel.ToList();

            ViewBag.userList = userList;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(BookingRequestModel bookings)
        {

            if (ModelState.IsValid)
            {
                var bookingItem = new Booking()
                {
                    RoomId = bookings.RoomId,
                    Purpose = bookings.Purpose,
                    StartDateTime = bookings.StartDateTime,
                    EndDateTime = bookings.EndDateTime,
                };
                _context.Bookings.Add(bookingItem);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return BadRequest(ModelState);

        }

        public async Task<IActionResult> Edit(int id)
        {
            var bookingobj = await _context.Bookings.FindAsync(id);

            if (bookingobj == null)
            {
                return NotFound();
            }

            IEnumerable<SelectListItem> roomList = _context.RoomModels.Select(
                u => new SelectListItem
                {
                    Text = u.RoomName,
                    Value = u.RoomId.ToString()
                }
            );
            ViewBag.roomList = roomList;


            List<Participants> participants = _context.Participants.ToList();
            List<Participants> participantslist = new List<Participants>();

            foreach (var item in participants)
            {
                var user = _context.Users.Find(item.UserId);
                UserModel abc = new UserModel
                {
                    Id = user.Id,
                    UserName = user.UserName

                };

                if (item.BookingId == id)
                {
                    item.User = abc;
                    participantslist.Add(item);
                }
            }

            ParticipantVModel model = new ParticipantVModel
            {
                RoomId = bookingobj.RoomId,
                Participants = participantslist,
                BookingId = id
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Booking obj)
        {

            if (obj != null)
            {
                _context.Bookings.Update(obj);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(obj);
        }
        public async Task<IActionResult> Delete(int id)
        {
            var obj = await _context.Bookings.FindAsync(id);
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Booking obj)
        {
            if (obj != null)
            {
                _context.Bookings.Remove(obj);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        //public IActionResult SeeParticipant(BookingRequestModel model)
        //{
        //    IEnumerable<SelectListItem> participantsList = _context.Participants.Select(
        //        u => new SelectListItem
        //        {
        //            Text=u.User.UserName, 
        //            Value=u.Id.ToString()
        //        });
        //    return View(participantsList);
        //}

        public async Task<IActionResult> AddParticipants(Participants participants)
        {
            IEnumerable<SelectListItem> userList = _context.Users.Select(
                u => new SelectListItem
                {
                    Text = u.UserName,
                    Value = u.Id.ToString()
                }
            );

            //var users = _context.UserModel.ToList();

            ViewBag.userList = userList;

            IEnumerable<SelectListItem> bookingList = _context.Bookings.Select(
                u => new SelectListItem
                {
                    Text = u.Purpose,
                    Value = u.BookingId.ToString()
                }
            );

            var bookings = _context.Bookings.ToList();

            ViewBag.bookingList = bookingList;


            //Participants obj = new Participants
            //{
            //    BookingId = participants.BookingId,
            //    UserId = participants.UserId
            //};
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddParticipants(ParticipantVModel obj)
        {
            var user = await _context.Users.FindAsync(obj.UserId);
            var bookings = await _context.Bookings.FindAsync(obj.BookingId);

            Participants participant = new Participants
            {
                UserId = obj.UserId,
                BookingId = obj.BookingId
            };

            _context.Participants.Add(participant);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
