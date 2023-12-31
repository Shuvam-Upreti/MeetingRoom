﻿using MeetingRoom.Data;
using MeetingRoom.Models;
using MeetingRoom.Models.AuthModel;
using MeetingRoom.Repository.IRepository;
using MeetingRoom.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using System.Net.Mail;
using System.Net;
using MeetingRoom.Services.IServices;

namespace MeetingRoom.Controllers
{
    public class BookingController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationDbContext _context;
        private readonly IEmailServices _emailServices;

        public BookingController(IUnitOfWork unitOfWork, ApplicationDbContext context, IEmailServices emailServices)
        {
            _unitOfWork = unitOfWork;
            _context = context;
            _emailServices = emailServices;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Booking> rooms = _unitOfWork.Booking.GetAll();
            return View(rooms);
        }
        public async Task<IActionResult> Create()
        {
            IEnumerable<SelectListItem> roomList = _unitOfWork.Room.GetAll().Select(
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

            //var users = _unitOfWork.UserModel.ToList();

            ViewBag.userList = userList;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(BookingRequestModel bookings)
        {
            var rooms= _unitOfWork.Room.GetAll();
            if (ModelState.IsValid)
            {
                var bookingss = _unitOfWork.Booking.GetAll().Where(b => b.RoomId == bookings.RoomId).ToList(); ;
                bool hasOverlap = false;
                foreach (var existingbooking in bookingss)
                {

                    if ((bookings.StartDateTime >= existingbooking.StartDateTime && bookings.StartDateTime < existingbooking.EndDateTime) ||
                            (bookings.EndDateTime > existingbooking.StartDateTime && bookings.EndDateTime <= existingbooking.EndDateTime) ||
                            (bookings.StartDateTime <= bookings.EndDateTime))
                    {
                        // There is an overlap
                        hasOverlap = true;
                        break;
                    }
                }

                if (!hasOverlap)
                {
                    var bookingItem = new Booking()
                    {
                        RoomId = bookings.RoomId,
                        Purpose = bookings.Purpose,
                        StartDateTime = bookings.StartDateTime,
                        EndDateTime = bookings.EndDateTime,
                        UserId=bookings.UserId
                    };
                    _unitOfWork.Booking.Add(bookingItem);
                    _unitOfWork.Save();
                    TempData["success"] = "Created Sucessfully";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["error"] = "Room is already booked during this time slot.";
                    return View(bookings);
                }
            }

            return BadRequest(ModelState);

        }

        public async Task<IActionResult> Edit(int id)
        { 
            var bookingobj = _unitOfWork.Booking.GetFirstorDefault(u => u.BookingId == id);

            if (bookingobj == null)
            {
                return NotFound();
            }

            IEnumerable<SelectListItem> roomList = _unitOfWork.Room.GetAll().Select(
                u => new SelectListItem
                {
                    Text = u.RoomName,
                    Value = u.RoomId.ToString()
                }
            );
            ViewBag.roomList = roomList;


            List<Participants> participants = _unitOfWork.Participants.GetAll().ToList();
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
                BookingId = bookingobj.BookingId,
                StartDateTime = bookingobj.StartDateTime,
                EndDateTime = bookingobj.EndDateTime,
                Purpose = bookingobj.Purpose
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Booking obj)
        {

            if (obj != null)
            {
                _unitOfWork.Booking.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "Edited Sucessfully";
                return RedirectToAction("Index");
            }
            return View(obj);
        }
        public async Task<IActionResult> Delete(int id)
        {
            var obj = _unitOfWork.Booking.GetFirstorDefault(u => u.BookingId == id);
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
                _unitOfWork.Booking.Remove(obj);
                _unitOfWork.Save();
                TempData["success"] = "Deleted Sucessfully";
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        public async Task<IActionResult> AddParticipants(int id)
        {
            IEnumerable<SelectListItem> userList = _context.Users.Select(
                u => new SelectListItem
                {
                    Text = u.UserName,
                    Value = u.Id.ToString()
                }
            );

            var users = _context.UserModel.ToList();

            ViewBag.userList = userList;

            IEnumerable<SelectListItem> bookingList = _unitOfWork.Booking.GetAll().Select(
                u => new SelectListItem
                {
                    Text = u.Purpose,
                    Value = u.BookingId.ToString()
                }
            );
            ViewBag.bookingList = bookingList;
            var booking = _unitOfWork.Booking.GetFirstorDefault(u => u.BookingId == id);

            ParticipantVModel model = new ParticipantVModel
            {
                BookingId=booking.BookingId
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddParticipants(ParticipantVModel obj)
        {
            var user = await _context.Users.FindAsync(obj.UserId);
            var bookings = _unitOfWork.Booking.GetFirstorDefault(u => u.BookingId == obj.BookingId);

            var existingParticipant = _unitOfWork.Participants.GetFirstorDefault(p => p.UserId == obj.UserId && p.BookingId == obj.BookingId);

            if (existingParticipant == null)
            {
                // Participant is not added, so add them
                Participants participant = new Participants
                {
                    UserId = obj.UserId,
                    BookingId = obj.BookingId
                };

                _unitOfWork.Participants.Add(participant);
                TempData["success"] = "Participant Added Sucessfully";
                _unitOfWork.Save();

                _emailServices.SendEmail(obj);

            }
            return RedirectToAction("Index");
        }

        public IActionResult RemoveParticipant(int id)
        {
            var participant = _unitOfWork.Participants.GetFirstorDefault(p => p.Id == id);

            if (participant == null)
            {
                return NotFound();
            }

            _unitOfWork.Participants.Remove(participant);
            _unitOfWork.Save();
            TempData["success"] = "Participant Removed Sucessfully";
            return RedirectToAction("Index");
        }

    }
}
