using BusinessLayer.Interfaces;
using ModelLayer.Models;
using RepositoryLayer.Entities;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Services
{
    public class ReviewsBusiness : IReviewBusiness
    {
        private readonly IReviewRepo reviewRepo;

        public ReviewsBusiness(IReviewRepo reviewRepo)
        {
            this.reviewRepo = reviewRepo;
        }

        public ReviewEntity AddReview(ReviewModel reviewModel)
        {
            return reviewRepo.AddReview(reviewModel);
        }

        public List<ReviewEntity> GetAllReviews()
        {
            return reviewRepo.GetAllReviews();
        }

        public ReviewEntity GetReviewByReviewId(int reviewId)
        {
            return reviewRepo.GetReviewByReviewId((int)reviewId);
        }

        public ReviewEntity GetReviewByUserName(string userName)
        {
            return reviewRepo.GetReviewByUserName(userName);
        }

        public List<ReviewEntity> GetReviewsByRating(double rating)
        {
            return reviewRepo.GetReviewsByRating(rating);
        }

        public bool DeleteReviewById(int reviewId)
        {
            return reviewRepo.DeleteReviewById(reviewId);
        }
    }
}
