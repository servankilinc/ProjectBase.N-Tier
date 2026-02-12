using AutoMapper;
using Model.Auth.SignUp;
using Model.Dtos.Blog.Commands;
using Model.Dtos.Blog.Queries;
using Model.Dtos.BlogComment.Commands;
using Model.Dtos.BlogComment.Queries;
using Model.Dtos.BlogLike.Commands;
using Model.Dtos.BlogLike.Queries;
using Model.Dtos.Category.Commands;
using Model.Dtos.Category.Queries;
using Model.Dtos.User.Commands;
using Model.Dtos.User.Queries;
using Model.Entities;

namespace Business.Mappings;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        // CreateMap<source, dest>

        #region Blog
        CreateMap<Blog, Blog>().ForAllMembers(opt => opt.Condition((src, dest, srcMember, destMember) => !Equals(srcMember, destMember)));

        CreateMap<Blog, BlogReportDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.AuthorId, opt => opt.MapFrom(src => src.AuthorId))
            .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.Author != default ? src.Author.Name : default))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
            .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != default ? src.Category.Name : default))
            .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date))
            .ForMember(dest => dest.BannerImage, opt => opt.MapFrom(src => src.BannerImage))
            .ForMember(dest => dest.LikeCount, opt => opt.MapFrom(src => src.LikeCount))
            .ForMember(dest => dest.CommentCount, opt => opt.MapFrom(src => src.CommentCount))
            .ForMember(dest => dest.CreateDateUtc, opt => opt.MapFrom(src => src.CreateDateUtc))
            .ForMember(dest => dest.UpdateDateUtc, opt => opt.MapFrom(src => src.UpdateDateUtc))
            .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted))
            .ForMember(dest => dest.DeletedDateUtc, opt => opt.MapFrom(src => src.DeletedDateUtc))
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember, destMember) => !Equals(srcMember, destMember)));

        CreateMap<Blog, BlogBasicResponseDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.AuthorId, opt => opt.MapFrom(src => src.AuthorId))
            .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.Author != default ? $"{src.Author.Name} {src.Author.LastName}".Trim() : default))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date))
            .ForMember(dest => dest.LikeCount, opt => opt.MapFrom(src => src.LikeCount))
            .ForMember(dest => dest.CommentCount, opt => opt.MapFrom(src => src.CommentCount))
            .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != default ? src.Category.Name : default))
            .ForMember(dest => dest.BannerImage, opt => opt.MapFrom(src => src.BannerImage))
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember, destMember) => !Equals(srcMember, destMember)));

        CreateMap<Blog, BlogDetailResponseDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.AuthorId, opt => opt.MapFrom(src => src.AuthorId))
            .ForMember(dest => dest.AuthorFirstName, opt => opt.MapFrom(src => src.Author != default ? src.Author.Name : default))
            .ForMember(dest => dest.AuthorLastName, opt => opt.MapFrom(src => src.Author != default ? src.Author.LastName : default))
            .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != default ? src.Category.Name : default))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
            .ForMember(dest => dest.BannerImage, opt => opt.MapFrom(src => src.BannerImage))
            .ForMember(dest => dest.LikeCount, opt => opt.MapFrom(src => src.LikeCount))
            .ForMember(dest => dest.CommentCount, opt => opt.MapFrom(src => src.CommentCount))
            .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date))
            .ForMember(dest => dest.CommentList, opt => opt.MapFrom(src => src.BlogComments))
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember, destMember) => !Equals(srcMember, destMember)));

        CreateMap<Blog, BlogLikeListResponseDto>()
            .ForMember(dest => dest.BlogId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.LikeCount, opt => opt.MapFrom(src => src.LikeCount))
            .ForMember(dest => dest.UserList, opt => opt.MapFrom(src => src.BlogLikes != default ? src.BlogLikes.Select(x => x.User) : default))
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember, destMember) => !Equals(srcMember, destMember)));

        CreateMap<BlogCreateDto, Blog>()
            .ForMember(dest => dest.AuthorId, opt => opt.MapFrom(src => src.AuthorId))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
            .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
            .ForMember(dest => dest.BannerImage, opt => opt.MapFrom(src => src.BannerImage))
            .ReverseMap();

        CreateMap<BlogUpdateDto, Blog>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
            .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
            .ForMember(dest => dest.BannerImage, opt => opt.MapFrom(src => src.BannerImage))
            .ReverseMap();
        #endregion

        #region BlogComment
        CreateMap<BlogComment, BlogComment>().ForAllMembers(opt => opt.Condition((src, dest, srcMember, destMember) => !Equals(srcMember, destMember)));

        CreateMap<BlogComment, BlogCommentReportDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.BlogId, opt => opt.MapFrom(src => src.BlogId))
            .ForMember(dest => dest.BlogTitle, opt => opt.MapFrom(src => src.Blog != default ? src.Blog.Title : default))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User != default ? src.User.Name : default))
            .ForMember(dest => dest.Comment, opt => opt.MapFrom(src => src.Comment))
            .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date))
            .ForMember(dest => dest.CreateDateUtc, opt => opt.MapFrom(src => src.CreateDateUtc))
            .ForMember(dest => dest.UpdateDateUtc, opt => opt.MapFrom(src => src.UpdateDateUtc))
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember, destMember) => !Equals(srcMember, destMember)));

        CreateMap<BlogComment, BlogCommentBasicResponseDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.BlogId, opt => opt.MapFrom(src => src.BlogId))
            .ForMember(dest => dest.Comment, opt => opt.MapFrom(src => src.Comment))
            .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date))
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember, destMember) => !Equals(srcMember, destMember)));

        CreateMap<BlogComment, BlogCommentDetailResponseDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.BlogId, opt => opt.MapFrom(src => src.BlogId))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.UserFirstName, opt => opt.MapFrom(src => src.User != default ? src.User.Name : default))
            .ForMember(dest => dest.UserLastName, opt => opt.MapFrom(src => src.User != default ? src.User.LastName : default))
            .ForMember(dest => dest.Comment, opt => opt.MapFrom(src => src.Comment))
            .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date))
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember, destMember) => !Equals(srcMember, destMember)));

        CreateMap<BlogCommentCreateDto, BlogComment>()
            .ForMember(dest => dest.BlogId, opt => opt.MapFrom(src => src.BlogId))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.Comment, opt => opt.MapFrom(src => src.Comment))
            .ReverseMap();

        CreateMap<BlogCommentUpdateDto, BlogComment>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Comment, opt => opt.MapFrom(src => src.Comment))
            .ReverseMap();
        #endregion

        #region BlogLike
        CreateMap<BlogLike, BlogLike>().ForAllMembers(opt => opt.Condition((src, dest, srcMember, destMember) => !Equals(srcMember, destMember)));

        CreateMap<BlogLike, BlogLikeResponseDto>()
            .ForMember(dest => dest.BlogId, opt => opt.MapFrom(src => src.BlogId))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.UserFirstName, opt => opt.MapFrom(src => src.User != default ? src.User.Name : default))
            .ForMember(dest => dest.UserLastName, opt => opt.MapFrom(src => src.User != default ? src.User.LastName : default))
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember, destMember) => !Equals(srcMember, destMember)));

        CreateMap<BlogLikeCreateDto, BlogLike>()
            .ForMember(dest => dest.BlogId, opt => opt.MapFrom(src => src.BlogId))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ReverseMap();

        CreateMap<BlogLikeDeleteDto, BlogLike>()
            .ForMember(dest => dest.BlogId, opt => opt.MapFrom(src => src.BlogId))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ReverseMap();
        #endregion

        #region Category
        CreateMap<Category, Category>().ForAllMembers(opt => opt.Condition((src, dest, srcMember, destMember) => !Equals(srcMember, destMember)));

        CreateMap<Category, CategoryReportDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.CreateDateUtc, opt => opt.MapFrom(src => src.CreateDateUtc))
            .ForMember(dest => dest.UpdateDateUtc, opt => opt.MapFrom(src => src.UpdateDateUtc))
            .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted))
            .ForMember(dest => dest.DeletedDateUtc, opt => opt.MapFrom(src => src.DeletedDateUtc))
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember, destMember) => !Equals(srcMember, destMember)));

        CreateMap<Category, CategoryResponseDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember, destMember) => !Equals(srcMember, destMember)));

        CreateMap<Category, CategoryBlogsResponseDto>()
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src))
            .ForMember(dest => dest.BlogList, opt => opt.MapFrom(src => src.Blogs))
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember, destMember) => !Equals(srcMember, destMember)));

        CreateMap<CategoryCreateDto, Category>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ReverseMap();

        CreateMap<CategoryUpdateDto, Category>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ReverseMap();
        #endregion

        #region User
        CreateMap<User, User>().ForAllMembers(opt => opt.Condition((src, dest, srcMember, destMember) => !Equals(srcMember, destMember)));

        CreateMap<SignUpRequest, User>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FirstName))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember, destMember) => !Equals(srcMember, destMember)));

        CreateMap<User, UserReportDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Addres, opt => opt.MapFrom(src => src.Addres))
            .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.BirthDate))
            .ForMember(dest => dest.CreateDateUtc, opt => opt.MapFrom(src => src.CreateDateUtc))
            .ForMember(dest => dest.UpdateDateUtc, opt => opt.MapFrom(src => src.UpdateDateUtc))
            .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted))
            .ForMember(dest => dest.DeletedDateUtc, opt => opt.MapFrom(src => src.DeletedDateUtc))
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember, destMember) => !Equals(srcMember, destMember)));

        CreateMap<User, UserBasicResponseDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember, destMember) => !Equals(srcMember, destMember)));

        CreateMap<User, UserDetailResponseDto>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Addres, opt => opt.MapFrom(src => src.Addres))
            .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.BirthDate))
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember, destMember) => !Equals(srcMember, destMember)));

        CreateMap<User, UserBlogsResponseDto>()
            .ForMember(dest => dest.User, opt => opt.MapFrom(src => src))
            .ForMember(dest => dest.BlogList, opt => opt.MapFrom(src => src.Blogs))
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember, destMember) => !Equals(srcMember, destMember)));

        CreateMap<UserCreateDto, User>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Addres, opt => opt.MapFrom(src => src.Addres))
            .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.BirthDate))
            .ReverseMap();

        CreateMap<UserUpdateDto, User>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
            .ForMember(dest => dest.Addres, opt => opt.MapFrom(src => src.Addres))
            .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.BirthDate))
            .ReverseMap();
        #endregion
    }
}
