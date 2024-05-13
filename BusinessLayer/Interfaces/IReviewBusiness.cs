using ModelLayer.Models;
using RepositoryLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Interfaces
{
    public interface IReviewBusiness
    {
        ReviewEntity AddReview(ReviewModel reviewModel);
        List<ReviewEntity> GetAllReviews();
        ReviewEntity GetReviewByReviewId(int reviewId);
        ReviewEntity GetReviewByUserName(string userName);
        List<ReviewEntity> GetReviewsByRating(double rating);
        bool DeleteReviewById(int reviewId);

    }
}
