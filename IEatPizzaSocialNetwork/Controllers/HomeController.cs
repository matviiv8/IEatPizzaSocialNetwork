using IEatPizzaSocialNetwork.Domain.Core.Entities;
using IEatPizzaSocialNetwork.Models;
using IEatPizzaSocialNetwork.WebUI.Models;
using IEatPizzaSocialNetwork.WebUI.Paging;
using IEatPizzaSocialNetwork.WebUI.Paging.Model;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net.Http.Formatting;

namespace IEatPizzaSocialNetwork.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public IActionResult CreateUser(User user)
        {
            var userId = NotUniqueUserId(user);

            if (userId == 0)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://localhost:7127/api/User");

                    var postTask = client.PostAsJsonAsync<User>("User", user);
                    postTask.Wait();

                    var result = postTask.Result;

                    if (result.IsSuccessStatusCode)
                    {
                        return CreateForm();
                    }
                }
            }
            else
            {
                return UpdateForm(userId);
            }

            return this.RedirectToAction("Error", "Home");
        }

        public IActionResult Feed(string search, int page = 1)
        {
            var forms = GetForms();
            var formsViewModel = new List<FeedViewModel>();

            foreach(var form in forms)
            {
                formsViewModel.Add(new FeedViewModel
                {
                    LastDateAndTimeSentForm = form.LastDateAndTimeSentForm,
                    CountSentForm = form.CountSentForm,
                    Name = GetUsers().Where(user => user.Id == form.UserId).First().Name,
                });
            }

            formsViewModel = formsViewModel.OrderByDescending(form => form.LastDateAndTimeSentForm).ToList();

            //search
            ViewData["Search"] = search;

            if (!string.IsNullOrEmpty(search))
            {
                formsViewModel = formsViewModel.Where(form => form.Name.ToLower().Contains(search.ToLower())).ToList();
            }

            //paging
            int pageSize = 7;
            var count = formsViewModel.Count();
            var items = formsViewModel.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            PageViewModel pagingInfo = new PageViewModel(count, page, pageSize);

            ShowAllFeedsViewModel allFeeds = new ShowAllFeedsViewModel
            {
                AllFeeds = items,
                PagingInfo = pagingInfo,
            };

            return View(allFeeds);
        }

        private int NotUniqueUserId(User currentUser)
        {
            using (var client = new HttpClient())
            {
                HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7127/api/User");
                var response = client.SendAsync(requestMessage).Result;
                var data = response.Content.ReadAsAsync<List<User>>().Result;

                foreach (var user in data)
                {
                    if (user.Email.Equals(currentUser.Email) && user.Name.Equals(currentUser.Name))
                    {
                        return user.Id;
                    }
                }
            }

            return 0;
        }

        private int CountSentForm(User user)
        {
            var form = new Form();

            using (var client = new HttpClient())
            {
                HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7127/api/Form/" + user.Id);
                var response = client.SendAsync(requestMessage).Result;
                form = response.Content.ReadAsAsync<Form>().Result;
            }

            return form.CountSentForm;
        }

        private IEnumerable<User> GetUsers()
        {
            var users = new List<User>();
            using (var client = new HttpClient())
            {
                HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7127/api/User");
                var response = client.SendAsync(requestMessage).Result;
                users = response.Content.ReadAsAsync<List<User>>().Result;
            }

            return users;
        }

        private IEnumerable<Form> GetForms()
        {
            var forms = new List<Form>();

            using (var client = new HttpClient())
            {
                HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7127/api/Form");
                var response = client.SendAsync(requestMessage).Result;
                forms = response.Content.ReadAsAsync<List<Form>>().Result;
            }

            return forms;
        }

        [HttpPost]
        public IActionResult CreateForm()
        {
            var user = GetUsers().LastOrDefault();
            var form = new Form
            {
                UserId = user.Id,
                LastDateAndTimeSentForm = DateTime.Now,
                CountSentForm = 1,
            };

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7127/api/Form");

                var postTask = client.PostAsJsonAsync<Form>("Form", form);
                postTask.Wait();

                var result = postTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    return this.RedirectToAction("Feed", "Home");
                }
            }

            return this.RedirectToAction("Error", "Home");
        }

        [HttpPost]
        public IActionResult UpdateForm(int userId)
        {
            var user = GetUsers().Where(user => user.Id == userId).FirstOrDefault();
            var form = GetForms().Where(form => form.UserId == userId).FirstOrDefault();

            var newForm = new Form
            {
                Id = form.Id,
                LastDateAndTimeSentForm = DateTime.Now,
                CountSentForm = CountSentForm(user) + 1,
                UserId = form.UserId
            };

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7127/api/Form");

                var postTask = client.PutAsJsonAsync<Form>("Form", newForm);
                postTask.Wait();

                var result = postTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    return this.RedirectToAction("Feed", "Home");
                }
            }

            return this.RedirectToAction("Error", "Home");
        }
    }
}