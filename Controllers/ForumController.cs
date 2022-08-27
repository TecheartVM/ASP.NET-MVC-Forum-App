using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC_test.Data;
using MVC_test.Models;

namespace MVC_test.Controllers
{
    public class ForumController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ForumDbContext _db;

        public ForumController(ILogger<HomeController> logger, ForumDbContext dbContext)
        {
            _logger = logger;
            _db = dbContext;
        }

        public IActionResult Index()
        {
            return View(_db.Topics);
        }

        [HttpGet]
        public async Task<IActionResult> Topic(Topic topic)
        {
            User? user = await _db.Users.FirstOrDefaultAsync(u => u.Id == topic.AuthorId);
            if (user != null)
                topic.Author = user;

            List<Comment> comments = _db.Comments
                .Where(c => c.TopicId == topic.Id)
                .OrderBy(c => c.CreationTime)
                .ToList();

            List<int> userIds = new List<int>();
            comments.ForEach(c =>
            {
                if (c.AuthorId != null)
                    userIds.Add((int)c.AuthorId);
            });

            List<User> users = _db.Users.Where(u => userIds.Contains(u.Id)).ToList();
            comments.ForEach(c =>
            {
                foreach (User u in users)
                    if (u.Id == c.AuthorId)
                        c.Author = u;
            });

            topic.Comments = comments;

            return View(topic);
        }

        [Authorize]
        [HttpGet]
        public IActionResult CreateTopic()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateTopic(Topic topic)
        {
            string? username = User.Identity?.Name;
            if (username != null)
            {
                User? user = await _db.Users.FirstOrDefaultAsync(u => u.Name == username);
                topic.AuthorId = user?.Id;
            }

            _db.Topics.Add(topic);
            await _db.SaveChangesAsync();
            return RedirectToAction("Topic", topic);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Comment(Comment comment)
        {
            string? username = User.Identity?.Name;
            if(username != null)
            {
                User? user = await _db.Users.FirstOrDefaultAsync(u => u.Name == username);
                comment.AuthorId = user?.Id;
            }

            _db.Comments.Add(comment);
            await _db.SaveChangesAsync();

            Topic? t = await _db.Topics.FindAsync(comment.TopicId);
            if (t != null)
                return RedirectToAction("Topic", t);
            return NotFound();
        }
    }
}
