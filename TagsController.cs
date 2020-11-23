using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BlogTechTallahE.Models;
using BlogDAL.Abstract;

using Microsoft.EntityFrameworkCore;
using BlogDAL.Model;
using BlogDAL.ViewModel;
using BlogBLL.Repository;

namespace BlogTechTallahE.Controllers
{
    public class TagsController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly PostRepository _context;
        private readonly CommentRepository _contextComment;
        private readonly IndexRepository _contextIndex;
   
        private readonly TagsRepository _contextTags;

        public TagsController(ILogger<HomeController> logger , PostRepository context   , IndexRepository contextIndex , CommentRepository comment, TagsRepository tags)
        {
            _logger = logger;
            _context = context;
            _contextIndex = contextIndex;
            _contextComment = comment;
            _contextTags = tags;



        }

        public IActionResult Index()
        {





        return View(_contextIndex.GetAllIndexViewModel());

           // return View(new IndexViewModel());

        }

        [Route("tags/{tag}")]
        public IActionResult Index(string tag)
        {

            var _obj = new IndexViewModel();
            //    _obj.Post = _context.Post.Where(c => c.PostTag.Where(c => c.Tag.Name == tags).ToList()) ;  // _context.PostTag.Where(c => c.Tag.Name == tags).Include(pt => pt.Post).Include(t=>t.Tag).ToList();
            _obj.Post = _context.dbcontext.Post.Where(a => a.PostTag.Any(c => c.Tag.Name == tag)).Include(z => z.PostTag).ToList();
            _obj.Tags = _context.dbcontext.Tags.ToList();
            _obj.Categories = _contextTags.TagsAndCategoies();
            _obj.RecentPost = _context.dbcontext.Post.OrderByDescending(c => c.CretedDate).Include(za => za.PostTag).ThenInclude(pt => pt.Tag).ToList();


            return View(_obj);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new BlogDAL.Model.ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        public IActionResult section()
        {
            var _obj = new IndexViewModel();
            _obj.Tags = _context.dbcontext.Tags.Include(p=>p.PostTag).ThenInclude(t=>t.Tag).ToList();
            _obj.Categories = _context.dbcontext.Categories.Include(c=>c.PostCategory).ToList();


            return View(_obj);
        }
        [HttpPost]
        public IActionResult LoadMorePost(string id)
        {
            var _obj = new IndexViewModel();
            var array = id.Split(',');
            List<int> termsList = new List<int>();
            foreach (var item in array)
            {
                if (item != "," && item != "")
                {
                    termsList.Add(Convert.ToInt32(item));
                }
             
            }


            var userProfiles = _context.dbcontext.Post.Where(x => !termsList.Contains(x.Id)).Include(z => z.PostTag).ToList();



             _obj.Post =   _context.dbcontext.Post.Where(x => !termsList.Any(y => y == x.Id)).Include(z => z.PostTag).ToList();

            if (_obj.Post.Count()== 0)
            {
                return View("NoMorePosts", _obj);
            }
            else
            {
                return View("LoadMorePost", _obj);
           
            }
        
        }
        

    }






}
