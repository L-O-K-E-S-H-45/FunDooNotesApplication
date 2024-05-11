using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Models;
using RepositoryLayer.Entities;
using System;
using System.Collections.Generic;

namespace FunDooNotesApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewBusiness reviewBusiness;

        public ReviewController(IReviewBusiness reviewBusiness)
        {
            this.reviewBusiness = reviewBusiness;
        }


        [HttpPost("review")]
        public ActionResult AddReview(ReviewModel reviewModel)
        {
            var response = reviewBusiness.AddReview(reviewModel);
            if (response != null)
                return Ok(new ResponseModel<ReviewEntity> {IsSuccess = true, Message = "Review added successfully", Data = response });
            else
                return BadRequest(new ResponseModel<ReviewEntity> { IsSuccess = false, Message = "Review add failed", Data = response });
            
        }

        [HttpGet("reviews")]
        public IActionResult GetAllReviews()
        {
            var response = reviewBusiness.GetAllReviews();
            if (response != null)
                return Ok(new ResponseModel<List<ReviewEntity>> { IsSuccess = true, Message = "Reviews found successfully", Data = response });
            else
                return BadRequest(new ResponseModel<List<ReviewEntity>> { IsSuccess = false, Message = "Review not found", Data = response });
        }

        [HttpGet("review")]
        public IActionResult GetReviewByReviewId(int reviewId)
        {
            try
            {
                    var response = reviewBusiness.GetReviewByReviewId(reviewId);
                    return Ok(new ResponseModel<ReviewEntity> { IsSuccess = true, Message = "Review found successfully", Data = response });
             }
            catch (Exception ex)
            {
                    return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Review not found", Data = ex.Message });
            }

        }
        [HttpGet("reviewByUsername")]
        public IActionResult GetReviewByUserName(string userName)
        {
            try
            {
                var response = reviewBusiness.GetReviewByUserName(userName);
                return Ok(new ResponseModel<ReviewEntity> { IsSuccess = true, Message = "Review found successfully", Data = response });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Review not found", Data = ex.Message });
            }
        }

        [HttpGet("ReviewsByRating")]
        public IActionResult GetReviewsByRating(double rating)
        {
            try
            {
                var response = reviewBusiness.GetReviewsByRating(rating);
                return Ok(new ResponseModel<List<ReviewEntity>> { IsSuccess = true, Message = "Review found successfully", Data = response });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Review not found", Data = ex.Message });
            }
        }

        [HttpDelete("delete")]
        public IActionResult DeleteByReviewId(int reviewId)
        {
            try
            {
                var response = reviewBusiness.DeleteReviewById(reviewId);
                return Ok(new ResponseModel<string> { IsSuccess = true, Message = "Review deleted successfully", Data = "Review deleted" });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<string> { IsSuccess = false, Message = "Review not deleted", Data = ex.Message });
            }
        }

    }
}
