using ModelLayer.Models;
using RepositoryLayer.Context;
using RepositoryLayer.Entities;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepositoryLayer.Services
{
    public class ReviewRepo : IReviewRepo
    {

        private readonly FunDooDBContext funDooDBContext;

        public ReviewRepo(FunDooDBContext funDooDBContext)
        {
            this.funDooDBContext = funDooDBContext;
        }

        public ReviewEntity AddReview(ReviewModel reviewModel)
        {
            ReviewEntity reviewEntity = new ReviewEntity();
            reviewEntity.UserName = reviewModel.UserName;
            reviewEntity.Feedback = reviewModel.Feedback;
            reviewEntity.Rating = reviewModel.Rating;
            reviewEntity.CreatedAt = DateTime.Now;
            reviewEntity.UpdatedAt = DateTime.Now;

            funDooDBContext.Reviews.Add(reviewEntity);
            funDooDBContext.SaveChanges();

            return reviewEntity;
        }

        public List<ReviewEntity> GetAllReviews()
        {
            var reviews = funDooDBContext.Reviews.ToList();
            return reviews;
        }

        public ReviewEntity GetReviewByReviewId(int reviewId)
        {
            var review = funDooDBContext.Reviews.FirstOrDefault(r => r.ReviewId == reviewId);
            if (review != null)
                return review;
            else
                throw new Exception("Review not found for requested reviewId: " + reviewId);
        }

        public ReviewEntity GetReviewByUserName(string userName)
        {
            var review = funDooDBContext.Reviews.FirstOrDefault(r => r.UserName == userName);
            if (review != null)
                return review;
            else
                throw new Exception("Review not found for requested userName: " + userName);
        }

        public List<ReviewEntity> GetReviewsByRating(double rating)
        {
            //List<ReviewEntity> reviews = funDooDBContext.Reviews.ToList().FindAll(r => r.Rating == rating);

            List<ReviewEntity> reviews = (from r in funDooDBContext.Reviews where r.Rating == rating select r).ToList();
            if (reviews != null)
                return reviews;
            else
                throw new Exception("Reviews not found for requested rating: " + rating);
        }

        public bool DeleteReviewById(int reviewId)
        {
            var review = funDooDBContext.Reviews.FirstOrDefault(r => r.ReviewId == reviewId);
            Console.WriteLine(review);
            if (review != null)
            {
                funDooDBContext.Remove(review);
                funDooDBContext.SaveChanges();
                return true;
            }
            else
                throw new Exception("Review not found for requested id: " + reviewId);
            
        }


    }
}
