using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.User_side;

namespace Infractructure
{
    public class InMemoryUserReviewRepository : IUserReviewControllerRepository
    {
        private readonly List<Reviews> reviews;
        public InMemoryUserReviewRepository()
        {
            reviews = [];
        }
        public void Add(Reviews review)
        {
            reviews.Add(review);
        }

        public void Delete(int id)
        {
            var review = reviews.FirstOrDefault(r => r.Id == id);
            if (review != null)
            {
                reviews.Remove(review);
            }
        }

        public List<Reviews> GetAll()
        {
            return reviews;
        }

        public Reviews? GetById(int id)
        {
            return reviews.FirstOrDefault(r => r.Id == id);
        }

        public void Update(Reviews review)
        {
            var existingReview = reviews.FirstOrDefault(r => r.Id == review.Id);
            if (existingReview != null)
            {
                existingReview.Rating = review.Rating;
                existingReview.Comment = review.Comment;
            }
        }
    }
}
