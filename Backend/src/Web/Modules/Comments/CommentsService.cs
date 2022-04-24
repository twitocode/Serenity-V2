using AutoMapper;
using Serenity.Common.Interfaces;
using Serenity.Database;
using Serenity.Database.Entities;
using Serenity.Modules.Comments.Dto;

namespace Serenity.Modules.Comments;

public class CommentsService : ICommentsService
{
    private readonly DataContext context;
    private readonly IMapper mapper;

    public CommentsService(DataContext context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

    public Task<List<Comment>> GetCommentsAsync(string postId)
    {
        var post = context.Posts.Where(post => post.Id == postId).FirstOrDefault();
        return Task.FromResult(post.Comments);
    }

    public Task<bool> DeleteAsync(string postId, User user, string commentId)
    {
        var comment = context.Comments.Where(x => x.Id == postId && x.UserId == user.Id && x.PostId == postId).FirstOrDefault();
        if (comment is null) return Task.FromResult(false);

        foreach (var reply in comment.Replies)
            context.Comments.Remove(reply);

        context.Comments.Remove(comment);
        var result = context.SaveChanges();

        if (result >= 0) return Task.FromResult(true);
        return Task.FromResult(false);
    }

    public Task<CreateCommentResponse> CreateCommentAsync(string postId, User user, CreateCommentDto dto)
    {
        var post = context.Posts.Where(x => x.Id == postId).First();

        if (post is null)
        {
            return Task.FromResult(new CreateCommentResponse(false, new() { new("CommentNotFound", $"the post with the Id of {postId} does not exist") }));
        }

        var comment = new Comment
        {
            RepliesToId = dto.RepliesToId,
            Content = dto.Content,
            UserId = user.Id,
            PostId = post.Id
        };

        post.Comments.Add(comment);
        var result = context.SaveChanges();

        if (result >= 0)
        {
            return Task.FromResult(new CreateCommentResponse(true, null));
        }

        return Task.FromResult(new CreateCommentResponse(false, new() { new("CreatePostError", "Could not create the Post") }));
    }

    public Task<CreateCommentResponse> ReplyToCommentAsync(string postId, User user, CreateCommentDto dto)
    {
        var comment = context.Comments.Where(x => x.Id == dto.RepliesToId && x.PostId == postId).First();

        if (comment is null)
        {
            return Task.FromResult(new CreateCommentResponse(false, new() { new("CommentNotFound", $"the post with the Id of {postId} does not exist") }));
        }

        var reply = new Comment
        {
            RepliesToId = dto.RepliesToId,
            Content = dto.Content,
            UserId = user.Id,
            PostId = postId
        };

        comment.Replies.Add(comment);
        var result = context.SaveChanges();

        if (result >= 0)
        {
            return Task.FromResult(new CreateCommentResponse(true, null));
        }

        return Task.FromResult(new CreateCommentResponse(false, new() { new("ReplyCommentError", $"Could not reply to the comment of Id {dto.RepliesToId}") }));
    }

    public Task<EditCommentResponse> EditCommentAsync(string postId, User user, EditCommentDto dto)
    {
        var comment = context.Comments.Where(x => x.Id == dto.CommentId && x.PostId == postId).First();

        if (comment is null)
        {
            return Task.FromResult(new EditCommentResponse(false, new() { new("CommentNotFound", $"the post with the Id of {postId} does not exist") }));
        }

        var updatedComment = mapper.Map<Comment>(comment);
        updatedComment.Content = dto.Content;

        context.Entry(comment).CurrentValues.SetValues(updatedComment);
        var result = context.SaveChanges();

        if (result >= 0)
        {
            return Task.FromResult(new EditCommentResponse(true, null));
        }

        return Task.FromResult(new EditCommentResponse(false, new() { new("EditCommentError", $"Could not edit the comment of Id {dto.CommentId}") }));
    }
}