import env from "@beam-australia/react-env";
import { createApi, fetchBaseQuery } from "@reduxjs/toolkit/query/react";
import { CreatePostDto, EditPostDto, Response } from "../../types/api";

export const postsApi = createApi({
	reducerPath: "postsApi",
	tagTypes: ["Posts"],
	baseQuery: fetchBaseQuery({
		baseUrl: env("NEXT_PUBLIC_SERVER_URL") ?? "http://localhost:8000/posts",
		prepareHeaders: (headers) => {
			const token = localStorage.getItem("JWT_ACCESS_TOKEN");

			if (token) {
				headers.set("Authorization", `Bearer ${token}`);
			}

			return headers;
		},
	}),
	endpoints: (builder) => ({
		editPost: builder.mutation<Response, EditPostDto>({
			query: (body) => ({
				url: "/",
				body,
				method: "POST",
			}),
			invalidatesTags: ["Posts"],
		}),
		createPost: builder.mutation<Response, CreatePostDto>({
			query: (body) => ({
				url: "/",
				body,
				method: "PUT",
			}),
			invalidatesTags: ["Posts"],
		}),
		deletePost: builder.mutation<Response, string>({
			query: (id) => ({
				url: `/${id}`,
				method: "PUT",
			}),
			invalidatesTags: ["Posts"],
		}),
		getMyPosts: builder.query<Response, number>({
			query: (page) => `my?page=${page}`,
			providesTags: ["Posts"],
		}),
		getRecentPosts: builder.query<Response, number>({
			query: (page) => `?page=${page}`,
		}),
		getPostById: builder.query<Response, string>({
			query: (id) => `/${id}`,
		}),
		getUserPostsById: builder.query<Response, string>({
			query: (id) => `/${id}`,
		}),
	}),
});

export const {
	useCreatePostMutation,
	useDeletePostMutation,
	useEditPostMutation,
	useGetMyPostsQuery,
	useGetRecentPostsQuery,
	useGetUserPostsByIdQuery,
	useGetPostByIdQuery,
} = postsApi;
