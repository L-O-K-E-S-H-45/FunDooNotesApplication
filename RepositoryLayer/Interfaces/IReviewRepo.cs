using ModelLayer.Models;
using RepositoryLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Interfaces
{
    public interface IReviewRepo 
    {

        ReviewEntity AddReview(ReviewModel reviewModel);
        List<ReviewEntity> GetAllReviews();
        ReviewEntity GetReviewByReviewId(int reviewId);
        ReviewEntity GetReviewByUserName(string userName);
        List<ReviewEntity> GetReviewsByRating(double rating);

        bool DeleteReviewById(int reviewId);
    }
}
